using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerToolButton : MonoBehaviour
{
    public MarkerTool markerTool { get; set; }

    [SerializeField] private Button _button;
    [SerializeField] private Text _text;

    public Action<MarkerTool> onClick;
    
    public string text
    {
        get => _text.text;
        set => _text.text = value;
    }

    private bool _selected;
    public bool selected
    {
        get => _selected;
        set => SetSelected(value);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClickHandler);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClickHandler);
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
    
    void OnClickHandler()
    {
        onClick?.Invoke(markerTool);
    }
}
