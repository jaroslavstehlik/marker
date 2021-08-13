using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerToolbar : MonoBehaviour
{
    [SerializeField] private MarkerToolButton _markerToolButton;
    [SerializeField] private RectTransform _container;
    private MarkerToolButton[] _markerToolButtons;
    
    private void Awake()
    {
        string[] markerToolNames = Enum.GetNames(typeof(MarkerTool));
        MarkerTool[] markerToolValues = (MarkerTool[])Enum.GetValues(typeof(MarkerTool));
        _markerToolButtons = new MarkerToolButton[markerToolNames.Length];
        
        for (int i = 0; i < markerToolNames.Length; i++)
        {
            GameObject markerToolGo = Instantiate(_markerToolButton.gameObject, _container);
            markerToolGo.transform.localRotation = Quaternion.identity;
            markerToolGo.transform.localScale = Vector3.one;
            MarkerToolButton markerToolButton = markerToolGo.GetComponent<MarkerToolButton>();
            markerToolButton.markerTool = markerToolValues[i];
            markerToolButton.text = markerToolNames[i];
            markerToolButton.onClick += OnMarkerToolButtonClick;
            _markerToolButtons[i] = markerToolButton;
        }
    }

    private void OnEnable()
    {
        UpdateMarkerToolButtons();
    }

    private void OnDisable()
    {
        
    }

    void OnMarkerToolButtonClick(MarkerTool markerTool)
    {
        LabelSerializer.instance.labels.activeMarkerTool = markerTool;
        UpdateMarkerToolButtons();
    }

    void UpdateMarkerToolButtons()
    {
        MarkerTool markerTool = LabelSerializer.instance.labels.activeMarkerTool;
        for (int i = 0; i < _markerToolButtons.Length; i++)
        {
            _markerToolButtons[i].selected = _markerToolButtons[i].markerTool == markerTool;
        }
    }
}
