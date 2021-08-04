using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageMarker : MonoBehaviour
{
    [SerializeField] RawImage _rawImage = default;
    [SerializeField] AspectRatioFitter _aspectRatioFitter = default;
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
    }

    void OnDisable()
    {
        LabelSerializer.instance.labels.workingImagePathChanged -= WorkingImagePathChanged;
    }

    void WorkingImagePathChanged(string imagePath)
    {
        if(!File.Exists(imagePath))
        {
            _texture2D.LoadImage(_whiteTexture);
            return;
        }

        byte[] bytes = File.ReadAllBytes(imagePath);
        _texture2D.LoadImage(bytes);
        if (_texture2D.height == 0)
        {
            _aspectRatioFitter.aspectRatio = 1f;
        }
        else
        {
            _aspectRatioFitter.aspectRatio = (float)_texture2D.width / (float)_texture2D.height;
        }
    }
}
