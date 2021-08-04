using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SFB;

public class Manager : MonoBehaviour
{    
    ExtensionFilter projectExtensionFilter = new ExtensionFilter( "Project files", "json");
    ExtensionFilter imageExtensionFilter = new ExtensionFilter("Image files", "jpg", "jpeg", "png");

    private void Awake()
    {
        LoadLastProject();
    }

    public void LoadLastProject()
    {
        bool loaded = LabelSerializer.instance.Load(LabelSerializer.projectPath);
        if (!loaded)
        {
            LabelSerializer.instance.LoadDefaults();
        }
    }

    public void CreateProject()
    {
        LabelSerializer.projectPath = LabelSerializer.DEFAULT_PROJECT_PATH;
        LabelSerializer.instance.LoadDefaults();
    }

    public void OpenProject()
    {
        string projectPath = LabelSerializer.projectPath;
        string directoryPath = Path.GetDirectoryName(projectPath);
        if(!Directory.Exists(directoryPath))
        {
            directoryPath = LabelSerializer.DEFAULT_PROJECT_PATH;
        }
        string[] path = StandaloneFileBrowser.OpenFilePanel("Open project", directoryPath, new ExtensionFilter[] { projectExtensionFilter }, false);
        if (path.Length < 1)
        {
            Debug.Log("No path selected!");
            return;
        }        
        LabelSerializer.projectPath = path[0];
        ReloadProject();
    }

    public void ReloadProject()
    {
        LabelSerializer.instance.Load(LabelSerializer.projectPath);
    }

    public void SaveProject()
    {
        LabelSerializer.instance.Save(LabelSerializer.projectPath);
    }

    public void SaveProjectAs()
    {
        string projectPath = LabelSerializer.projectPath;
        string directoryPath = Path.GetDirectoryName(projectPath);
        if (!Directory.Exists(directoryPath))
        {
            directoryPath = LabelSerializer.DEFAULT_PROJECT_PATH;
        }
        string path = StandaloneFileBrowser.SaveFilePanel("Save project", directoryPath, LabelSerializer.DEFAULT_PROJECT_NAME, new ExtensionFilter[] { projectExtensionFilter });
        if(string.IsNullOrEmpty(path))
        {
            Debug.Log("No file saved!");
            return;
        }
        
        LabelSerializer.projectPath = path;
        SaveProject();
    }

    public void AddImages()
    {
        string[] imageFiles = StandaloneFileBrowser.OpenFilePanel("Import images", LabelSerializer.projectPath, new ExtensionFilter[] { imageExtensionFilter }, true);
        LabelSerializer.instance.labels.AddImages(imageFiles);
    }

    public void RemoveImages()
    {
        LabelSerializer.instance.labels.RemoveSelectedImages();
    }
}
