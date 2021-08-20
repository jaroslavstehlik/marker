using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableScrollRect : ScrollRect
{
    private bool _draggable = true;

    public Action<PointerEventData> onBeginDrag = default;
    public Action<PointerEventData> onDrag = default;
    public Action<PointerEventData> onEndDrag = default;

    public bool draggable
    {
        get => _draggable;
        set => SetDraggable(value);
    }

    void SetDraggable(bool value)
    {
        _draggable = value;
        if(!draggable) StopMovement();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(_draggable) base.OnBeginDrag(eventData);
        onBeginDrag?.Invoke(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(_draggable) base.OnDrag(eventData);
        onDrag?.Invoke(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(_draggable) base.OnEndDrag(eventData);
        onEndDrag?.Invoke(eventData);
    }
}
