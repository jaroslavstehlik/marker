using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageListButton : MonoBehaviour
{
    const int MAX_PATH_LENGTH = 30;
    [SerializeField] Text _label;
    [SerializeField] Button _button;
    [SerializeField] Image _indicator;

    public Action<int> onClick;

    public string text {
        get => _label.text;
        set => SetText(value);
    }

    private bool _selected;
    public bool selected {
        get => _selected;
        set => SetSelected(value);
    }

    private bool _indication;
    public bool indication {
        get => _indication;
        set => SetIndication(value);
    }

    void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        SetSelected(_selected);
        SetIndication(_indication);
    }

    void OnDisable()
    {
        _button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        onClick?.Invoke(transform.GetSiblingIndex());
    }

    void SetSelected(bool value)
    {
        _selected = value;
        ColorBlock colorBlock = _button.colors;
        Color color = value ? Color.cyan : Color.white;
        colorBlock.normalColor = Color.Lerp(Color.white, color, 0.5f);
        colorBlock.selectedColor = Color.Lerp(Color.white, color, 0.75f);
        colorBlock.highlightedColor = Color.Lerp(Color.white, color, 1f);
        _button.colors = colorBlock;
    }

    void SetIndication(bool value)
    {
        _indication = value;
        _indicator.enabled = value;
    }

    void SetText(string value)
    {
        if (value.Length > MAX_PATH_LENGTH)
        {
            value = $"..{value.Substring(value.Length - MAX_PATH_LENGTH, MAX_PATH_LENGTH)}";
        }
        _label.text = value;
    }
}
