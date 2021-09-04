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
    public SignalDictionary<string, List<RectangleLabel>> rectangleLabels { get; } =
        new SignalDictionary<string, List<RectangleLabel>>();

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

    public Action onLoaded = default;
    
    public void AddLabel(string filename, RectangleLabel mark)
    {
        if(rectangleLabels.ContainsKey(filename))
        {
            rectangleLabels[filename].Add(mark);
        } else
        {
            rectangleLabels.Add(filename, new List<RectangleLabel>(){mark});
        }
    }

    public void RemoveLabel(string filename)
    {
        rectangleLabels.Remove(filename);
    }

    public bool ContainsLabel(string filename)
    {
        return rectangleLabels.ContainsKey(filename);
    }

    public bool GetLabels(string filename, out List<RectangleLabel> labels)
    {
        if (!rectangleLabels.ContainsKey(filename))
        {
            labels = null;
            return false;
        }

        labels = rectangleLabels[filename];
        return true;
    }

    public void AddImage(string filename)
    {
        if (!imagePaths.Contains(filename))
        {
            imagePaths.value.Add(filename);
            imagePaths.value.Sort();
        }
        imagePaths.ForceChange();
    }

    public void AddImages(string[] filenames)
    {
        for (int i = 0; i < filenames.Length; i++)
        {
            if (!imagePaths.Contains(filenames[i]))
            {
                imagePaths.value.Add(filenames[i]);
            }
        }

        imagePaths.value.Sort();
        imagePaths.ForceChange();
    }

    public void RemoveImage(string filename)
    {
        imagePaths.value.Remove(filename);        
        imagePathSelection.value.Remove(filename);
        imagePaths.value.Sort();
        imagePaths.ForceChange();
        imagePathSelection.ForceChange();
        
        if (!imagePaths.Contains(workingImagePath.value))
        {
            workingImagePath.value = null;
        }
    }

    public void RemoveImages(string[] filenames)
    {
        for (int i = 0; i < filenames.Length; i++)
        {
            imagePaths.value.Remove(filenames[i]);
            imagePathSelection.value.Remove(filenames[i]);
        }
        imagePaths.value.Sort();
        imagePaths.ForceChange();
        imagePathSelection.ForceChange();
        
        if(!imagePaths.Contains(workingImagePath.value))
        {
            workingImagePath.value = null;
        }
    }

    public void RemoveSelectedImages()
    {
        RemoveImages(imagePathSelection.ToArray());
        imagePathSelection.Clear();
    }

    public bool ContainsImage(string filename)
    {
        return imagePaths.Contains(filename);
    }

    public void Clear()
    {
        directoryPath.value = "";
        workingImagePath.value = "";
        labelNames.Clear();
        imagePaths.Clear();
        imagePathSelection.Clear();
    }
}
