using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Globalization;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class LabelSerializer
{
    static LabelSerializer _instance;
    public static LabelSerializer instance => _instance;

    public const string LABEL_SERIALIZER_PROJECT_PATH = nameof(LABEL_SERIALIZER_PROJECT_PATH);
    public const string DEFAULT_PROJECT_NAME = "newLabelProject.json";
    public static string DEFAULT_PROJECT_PATH => Path.Combine(Application.dataPath, DEFAULT_PROJECT_NAME);

    public Action<Labels> onDataLoaded;
    private Labels _labels = null;
    public Labels labels => _labels;

    public static readonly CultureInfo culture = CultureInfo.InvariantCulture;

    /// <summary>
    /// Default json serialization settings
    /// </summary>
    public static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings
    {
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        StringEscapeHandling = StringEscapeHandling.Default,
        Formatting = Formatting.None,
        NullValueHandling = NullValueHandling.Include,
        MissingMemberHandling = MissingMemberHandling.Ignore,
        ContractResolver = UnitySerializeContractResolver.Instance,
        Culture = CultureInfo.InvariantCulture
    };

    /// <summary>
    /// Pretty json format for better human readability
    /// </summary>
    public static JsonSerializerSettings jsonSettingsPretty {
        get {
            JsonSerializerSettings output = jsonSettings;
            output.Formatting = Formatting.Indented;
            return output;
        }
    }

    public LabelSerializer()
    {
        _instance = this;
        LoadDefaults();
    }

    public static string projectPath {
        get {
            if (!PlayerPrefs.HasKey(LABEL_SERIALIZER_PROJECT_PATH)) return null;
            return PlayerPrefs.GetString(LABEL_SERIALIZER_PROJECT_PATH);
        }
        set {
            PlayerPrefs.SetString(LABEL_SERIALIZER_PROJECT_PATH, value);
        }
    }

    public void LoadDefaults()
    {
        if (_labels == null)
        {
            _labels = new Labels();
        }
        _labels.Clear();
        _labels.directoryPath = DEFAULT_PROJECT_PATH;
        onDataLoaded?.Invoke(_labels);
    }

    public bool Load(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"LabelSerializer load failed, path is null or empty!");
            return false;
        }
        string directoryPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"LabelSerializer load failed, Directory does not exist! {directoryPath}");
            return false;
        }
        if (!File.Exists(path))
        {
            Debug.LogError($"LabelSerializer load failed, File does not exist! {path}");
            return false;
        }
        string serializedString = File.ReadAllText(path);
        if (string.IsNullOrEmpty(serializedString))
        {
            Debug.LogError($"LabelSerializer load failed, File is empty! {path}");
            return false;
        }
        JsonConvert.PopulateObject(serializedString, _labels, jsonSettings);
        labels.OnLoaded();
        onDataLoaded?.Invoke(_labels);        
        return true;
    }

    public bool Save(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError($"LabelSerializer save failed, path is null or empty!");
            return false;
        }
        string directoryPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(directoryPath))
        {
            Debug.LogError($"LabelSerializer save failed, Directory does not exist! {directoryPath}");
            return false;
        }
        string serializedString = JsonConvert.SerializeObject(_labels);
        if(string.IsNullOrEmpty(serializedString))
        {
            Debug.LogError($"LabelSerializer save failed, string is empty! {path}");
            return false;
        }
        File.WriteAllText(path, serializedString);
        return true;
    }
}
