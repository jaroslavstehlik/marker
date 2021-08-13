using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageMarker : MonoBehaviour
{
    [SerializeField] private RectTransform _rawImageRectTransform = default;
    [SerializeField] private RawImage _rawImage = default;
    [SerializeField] private Image _scrollViewImage = default;
    
    private Texture2D _texture2D = default;
    byte[] _whiteTexture;

    void Awake()
    {
        _whiteTexture = Texture2D.whiteTexture.EncodeToPNG();
        _texture2D = new Texture2D(2, 2);
        _rawImage.texture = _texture2D;
    }

    void OnEnable()
    {
        LabelSerializer.instance.labels.workingImagePathChanged += WorkingImagePathChanged;
        WorkingImagePathChanged(LabelSerializer.instance.labels.workingImagePath);
        UpdateScrollView();
    }

    private void Update()
    {
        UpdateSize();
        UpdateScrollView();
    }

    private void OnDisable()
    {
        LabelSerializer.instance.labels.workingImagePathChanged -= WorkingImagePathChanged;
    }

    void WorkingImagePathChanged(string imagePath)
    {
        if (!File.Exists(imagePath))
        {
            _texture2D.LoadImage(_whiteTexture);
            return;
        }

        byte[] bytes = File.ReadAllBytes(imagePath);
        _texture2D.LoadImage(bytes);
    }

    void UpdateSize()
    {
        float aspectRatio = 1f;
        if (_texture2D.height > 0 && _texture2D.width > 0)
        {
            aspectRatio = (float) _texture2D.width / (float) _texture2D.height;
        }

        float height = _texture2D.height * LabelSerializer.instance.labels.imagePreviewMagnification;
        _rawImageRectTransform.sizeDelta = new Vector2(height * aspectRatio, height);
    }

    void UpdateScrollView()
    {
        _scrollViewImage.raycastTarget = LabelSerializer.instance.labels.activeMarkerTool == MarkerTool.MOVE_TOOL;
    }
}
