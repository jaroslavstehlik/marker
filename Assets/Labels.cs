using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptIn)]
public class Labels
{
    [JsonProperty("rectangleLabels")]
    Dictionary<string, RectangleLabel> _rectangleLabels;
    [JsonProperty("directoryPath")]
    string _directoryPath;
    [JsonProperty("labelNames")]
    List<string> _labelNames;
    [JsonProperty("imagePaths")]
    List<string> _imagePaths;
    [JsonProperty("imagePathSelection")]
    List<string> _imagePathSelection;
    [JsonProperty("workingImagePath")]
    string _workingImagePath;    

    public Action<string> workingImagePathChanged;
    public Action<List<string>> onImagesChanged;
    public float imagePreviewMagnification = 1f;

    public Dictionary<string, RectangleLabel> rectangleLabels { get => _rectangleLabels; }
    public string directoryPath { get => _directoryPath; set => _directoryPath = value; }
    public List<string> labelNames { get => _labelNames; }
    public List<string> imagePaths { get => _imagePaths; }
    public List<string> imagePathSelection { get => _imagePathSelection; }

    public string workingImagePath {
        get {
            return _workingImagePath;
        }
        set {
            if (_workingImagePath != value)
            {
                _workingImagePath = value;
                workingImagePathChanged?.Invoke(_workingImagePath);
            }
        }
    }

    public Labels() {
        _rectangleLabels = new Dictionary<string, RectangleLabel>();
        _labelNames = new List<string>();
        _imagePaths = new List<string>();
        _imagePathSelection = new List<string>();
    }

    public void AddLabel(string filename, RectangleLabel mark)
    {
        if(_rectangleLabels.ContainsKey(filename))
        {
            _rectangleLabels[filename] = mark;
        } else
        {
            _rectangleLabels.Add(filename, mark);
        }
    }

    public void RemoveLabel(string filename)
    {
        _rectangleLabels.Remove(filename);
    }

    public bool ContainsLabel(string filename)
    {
        return _rectangleLabels.ContainsKey(filename);
    }

    public bool GetLabel(string filename, out RectangleLabel label)
    {
        if (!_rectangleLabels.ContainsKey(filename))
        {
            label = new RectangleLabel();
            return false;
        }

        label = _rectangleLabels[filename];
        return true;
    }

    public void AddImage(string filename)
    {
        if (!_imagePaths.Contains(filename))
        {
            _imagePaths.Add(filename);
            _imagePaths.Sort();

            onImagesChanged?.Invoke(_imagePaths);
        }
    }

    public void AddImages(string[] filenames)
    {
        for (int i = 0; i < filenames.Length; i++)
        {
            if (!_imagePaths.Contains(filenames[i]))
            {
                _imagePaths.Add(filenames[i]);
            }
        }

        _imagePaths.Sort();
        onImagesChanged?.Invoke(_imagePaths);
    }

    public void RemoveImage(string filename)
    {
        _imagePaths.Remove(filename);        
        _imagePathSelection.Remove(filename);
        _imagePaths.Sort();

        onImagesChanged?.Invoke(_imagePaths);
        if (!_imagePaths.Contains(workingImagePath))
        {
            workingImagePath = null;
        }
    }

    public void RemoveImages(string[] filenames)
    {
        for (int i = 0; i < filenames.Length; i++)
        {
            _imagePaths.Remove(filenames[i]);
            _imagePathSelection.Remove(filenames[i]);
        }
        _imagePaths.Sort();

        onImagesChanged?.Invoke(_imagePaths);
        if(!_imagePaths.Contains(workingImagePath))
        {
            workingImagePath = null;
        }
    }

    public void RemoveSelectedImages()
    {
        RemoveImages(imagePathSelection.ToArray());
        imagePathSelection.Clear();
    }

    public bool ContainsImage(string filename)
    {
        return _imagePaths.Contains(filename);
    }

    public List<string> GetImages()
    {
        return _imagePaths;
    }

    public void OnLoaded()
    {
        workingImagePathChanged?.Invoke(_workingImagePath);
    }

    public void Clear()
    {
        _directoryPath = "";
        _labelNames.Clear();
        _imagePaths.Clear();
        _imagePathSelection.Clear();
        _workingImagePath = "";

        workingImagePathChanged?.Invoke(_workingImagePath);
        onImagesChanged?.Invoke(_imagePaths);
    }
}
