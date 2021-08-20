using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClassRectangle : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Text _classText;
    [SerializeField] private Image _classTextBackgroundImage;
    [SerializeField] private Image _rectImage;

    private RectTransform _parent;
    private Color _color = Color.cyan;
    private Vector2 _center;
    private Vector2 _size;
    public Vector2 center
    {
        get => _center;
        set => SetCenter(value);
    }
    
    public Vector2 size
    {
        get => _size;
        set => SetSize(value);
    }

    public string label
    {
        get => _classText.text;
        set => SetLabel(value);
    }
    
    public Color color
    {
        get => _color;
        set => SetColor(value);
    }

    private void Awake()
    {
        _parent = transform.parent as RectTransform;
    }

    private void OnEnable()
    {
        SetCenter(_center);
        SetSize(_size);
        SetColor(_color);
    }

    void SetCenter(Vector2 value)
    {
        Rect parentRect = _parent.rect;
        _rectTransform.anchoredPosition = new Vector2(parentRect.width * (value.x - 0.5f), parentRect.height * (value.y - 0.5f));
    }
    
    void SetSize(Vector2 value)
    {
        Rect parentRect = _parent.rect;
        _rectTransform.sizeDelta = new Vector2(parentRect.width * value.x, parentRect.height * value.y);
    }

    void SetColor(Color value)
    {
        _color = value;
        _rectImage.color = value;
        _classTextBackgroundImage.color = value;
        
        float intensity = 0.299f * value.r + 0.587f * value.g + 0.114f * value.b;
        float newIntensity = 1f - Mathf.Round(intensity);
        _classText.color = new Color(newIntensity, newIntensity, newIntensity, 1f);
    }

    float Resolve(float value)
    {
        return 1f-Mathf.Round(value);
    }
    
    void SetLabel(string value)
    {
        _classText.text = value;
    }
}
