using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class RectangleLabelEditor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private MarkerManager _markManager = default;
    [SerializeField] private DraggableScrollRect _draggableScrollRect = default;
    [SerializeField] private RectTransform _container = default;
    [SerializeField] private RectTransform _image = default;

    private RectDrawState _rectDrawState = RectDrawState.None;
    private Vector2 _pointerDownPosition;
    private Vector2 _pointerUpPosition;
    
    enum RectDrawState
    {
        None,
        DrawFirstCorner,
        DrawSecondCorner,
        Finished
    }

    void SetState(RectDrawState state)
    {
        if (LabelSerializer.instance.labels.activeMarkerTool.value != MarkerTool.RECTANGLE_TOOL)
        {
            _rectDrawState = RectDrawState.None;
            return;
        }
        
        Labels labels = LabelSerializer.instance.labels;
        
        _rectDrawState = state;
        switch (state)
        {
            case RectDrawState.None:
                break;
            case RectDrawState.DrawFirstCorner:
                break;
            case RectDrawState.DrawSecondCorner:
                break;
            case RectDrawState.Finished:
                RectangleLabel rectangleLabel = new RectangleLabel();
                rectangleLabel.minX = Mathf.Min(_pointerDownPosition.x, _pointerUpPosition.x);
                rectangleLabel.minY = Mathf.Min(_pointerDownPosition.y, _pointerUpPosition.y);
                rectangleLabel.maxX = Mathf.Max(_pointerDownPosition.x, _pointerUpPosition.x);
                rectangleLabel.maxY = Mathf.Max(_pointerDownPosition.y, _pointerUpPosition.y);
                rectangleLabel.name = "Test";
                labels.AddLabel(LabelSerializer.instance.labels.workingImagePath.value, rectangleLabel);
                _markManager.RebuildLabels();
                SetState(RectDrawState.None);
                break;
        }
    }
    
    private void OnEnable()
    {
        _draggableScrollRect.onBeginDrag += OnBeginDrag;
        _draggableScrollRect.onDrag += OnDrag;
        _draggableScrollRect.onEndDrag += OnEndDrag;
        _rectDrawState = RectDrawState.None;
    }

    private void OnDisable()
    {
        _draggableScrollRect.onBeginDrag -= OnBeginDrag;
        _draggableScrollRect.onDrag -= OnDrag;
        _draggableScrollRect.onEndDrag -= OnEndDrag;
        _rectDrawState = RectDrawState.None;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_rectDrawState != RectDrawState.None) return;
        Vector2 localPosition = GetNormalizedPosition(eventData.position);
        _pointerDownPosition = localPosition;
        SetState(RectDrawState.DrawFirstCorner);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetState(RectDrawState.DrawSecondCorner);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_rectDrawState == RectDrawState.DrawFirstCorner)
        {
            SetState(RectDrawState.DrawSecondCorner);   
        } else if (_rectDrawState == RectDrawState.DrawSecondCorner)
        {
            Vector2 localPosition = GetNormalizedPosition(eventData.position);
            _pointerUpPosition = localPosition;
            SetState(RectDrawState.Finished);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    Vector2 GetNormalizedPosition(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_container, position, null,
            out Vector2 localPosition);

        Rect imageRect = _image.rect;

        Vector2 normalizedPosition;
        normalizedPosition.x = localPosition.x / imageRect.width + 0.5f;
        normalizedPosition.y = 1f - ((localPosition.y / imageRect.height - 0.5f) * -1f);
        
        return normalizedPosition;
    }
}
