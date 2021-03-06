using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private RectTransform _scrollRectTransform;
    [SerializeField] private RectTransform _scrollRectContentTransform = default;
    [SerializeField] private RectTransform _miniMapRectTransform;
    [SerializeField] private RectTransform _backgroundRectTransform;
    [SerializeField] private RectTransform _foregroundRectTransform;
    [SerializeField] private Slider _slider = default;
    [SerializeField] private Text _magnificationText = default;

    private void OnEnable()
    {
        LabelSerializer.instance.labels.imagePreviewMagnification.changed += OnImagePreviewMagnificationChanged;
        OnImagePreviewMagnificationChanged(LabelSerializer.instance.labels.imagePreviewMagnification.value);
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
        UpdateMagnificationText();
    }

    private void Update()
    {
        Rect miniMapRect = _miniMapRectTransform.rect;
        Rect scrollRectRect = _scrollRectTransform.rect;
        Rect scrollRectContentRect = _scrollRectContentTransform.rect;

        Vector2 contentPosition = (Vector2)_scrollRectContentTransform.localPosition + new Vector2(-scrollRectRect.size.x * 0.5f, scrollRectRect.size.y * 0.5f);
        Vector2 contentPositionNormalized = contentPosition / (scrollRectRect.size);
        
        Vector2 contentSizeDelta = scrollRectContentRect.size / scrollRectRect.size;
        float scrollRectAspect = scrollRectRect.size.x / scrollRectRect.size.y;

        Vector2 backgroundPosition = new Vector2((miniMapRect.size.x * scrollRectAspect - miniMapRect.size.x) * 0.5f, 0f);
        Vector2 backgroundSizeDelta = new Vector2(miniMapRect.size.x * scrollRectAspect, miniMapRect.size.y);
        _backgroundRectTransform.anchoredPosition = backgroundPosition;
        _backgroundRectTransform.sizeDelta = backgroundSizeDelta;
        
        Vector2 foregroundPosition = backgroundSizeDelta * contentPositionNormalized;
        Vector2 foregroundSizeDelta = new Vector2(backgroundSizeDelta.x * contentSizeDelta.x, backgroundSizeDelta.y * contentSizeDelta.y);
        _foregroundRectTransform.anchoredPosition = foregroundPosition;
        _foregroundRectTransform.sizeDelta = foregroundSizeDelta;
    }

    private void OnDisable()
    {
        LabelSerializer.instance.labels.imagePreviewMagnification.changed -= OnImagePreviewMagnificationChanged;
        _slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnImagePreviewMagnificationChanged(float value)
    {
        _slider.value = value;
        UpdateMagnificationText();
    }

    void OnSliderValueChanged(float value)
    {
        value = SliderSnap(value);
        LabelSerializer.instance.labels.imagePreviewMagnification.value = value;
    }

    float SliderSnap(float value)
    {
        if (Mathf.Abs(1f - value) < 0.1f) return 1f;
        return value;
    }
    
    void UpdateMagnificationText()
    {
        float magnification = LabelSerializer.instance.labels.imagePreviewMagnification.value;
        _magnificationText.text = magnification.ToString("P", CultureInfo.InvariantCulture);
    }
}
