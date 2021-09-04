using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Newtonsoft.Json;
using Signals;

[JsonObject(MemberSerialization.OptIn)]
public class Labels
{
    [JsonProperty("rectangleLabels")]
    Dictionary<string, List<RectangleLabel>> _rectangleLabels;
    
    [JsonProperty("directoryPath")]
    public Signal<string> directoryPath { get; } = new Signal<string>();

    [JsonProperty("labelNames")] 
    public SignalList<string> labelNames { get; } = new SignalList<string>();
    
    [JsonProperty("imagePaths")]
    public SignalList<string> imagePaths { get; } = new SignalList<string>();
    
    [JsonProperty("imagePathSelection")]
    public SignalList<string> imagePathSelection { get; } = new SignalList<string>();

    [JsonProperty("workingImagePath")] 
    public Signal<string> workingImagePath { get; } = new Signal<string>();
    
    [JsonProperty("imagePreviewMagnification")] 
    public Signal<float> imagePreviewMagnification { get; } = new Signal<float>(1f);
    
    [JsonProperty("activeMarkerTool")]
    public Signal<MarkerTool> activeMarkerTool { get; } = new Signal<MarkerTool>(MarkerTool.MOVE_TOOL);

    public Dictionary<string, List<RectangleLabel>> rectangleLabels { get => _rectangleLabels; }

    public Action onLoaded = default;
    
    public Labels() {
        _rectangleLabels = new Dictionary<string, List<RectangleLabel>>();
    }

    public void AddLabel(string filename, RectangleLabel mark)
    {
        if(_rectangleLabels.ContainsKey(filename))
        {
            _rectangleLabels[filename].Add(mark);
        } else
        {
            _rectangleLabels.Add(filename, new List<RectangleLabel>(){mark});
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

    public bool GetLabels(string filename, out List<RectangleLabel> labels)
    {
        if (!_rectangleLabels.ContainsKey(filename))
        {
            labels = null;
            return false;
        }

        labels = _rectangleLabels[filename];
        return true;
    }

    public void AddImage(string filename)
    {
        Debug.Log(("Labels.AddImage"));
        if (!imagePaths.Contains(filename))
        {
            imagePaths.Add(filename);
            imagePaths.Sort();
        }
    }

    public void AddImages(string[] filenames)
    {
        Debug.Log(("Labels.AddImages"));
        for (int i = 0; i < filenames.Length; i++)
        {
            if (!imagePaths.Contains(filenames[i]))
            {
                imagePaths.Add(filenames[i]);
            }
        }

        imagePaths.Sort();
    }

    public void RemoveImage(string filename)
    {
        Debug.Log(("Labels.RemoveImage"));
        imagePaths.Remove(filename);        
        imagePathSelection.Remove(filename);
        imagePaths.Sort();
        
        if (!imagePaths.Contains(workingImagePath.value))
        {
            workingImagePath.value = null;
        }
    }

    public void RemoveImages(string[] filenames)
    {
        Debug.Log(("Labels.RemoveImages"));
        for (int i = 0; i < filenames.Length; i++)
        {
            imagePaths.Remove(filenames[i]);
            imagePathSelection.Remove(filenames[i]);
        }
        imagePaths.Sort();
        
        if(!imagePaths.Contains(workingImagePath.value))
        {
            workingImagePath.value = null;
        }
    }

    public void RemoveSelectedImages()
    {
        Debug.Log(("Labels.RemoveSelectedImages"));
        RemoveImages(imagePathSelection.ToArray());
        imagePathSelection.Clear();
    }

    public bool ContainsImage(string filename)
    {
        return imagePaths.Contains(filename);
    }

    public void Clear()
    {
        Debug.Log(("Labels.Clear"));
        directoryPath.value = "";
        workingImagePath.value = "";
        labelNames.Clear();
        imagePaths.Clear();
        imagePathSelection.Clear();
    }
}
