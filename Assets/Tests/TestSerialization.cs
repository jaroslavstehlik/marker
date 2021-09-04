using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Signals;

[JsonObject(MemberSerialization.OptIn)]
public class SerializedClass
{
    [JsonProperty("myList")]
    public SignalList<float> myList { get; } = new SignalList<float>();

    [JsonProperty("myValue")]
    public Signal<float> myValue { get; } = new Signal<float>();
}

public class TestSerialization : MonoBehaviour
{
    private SerializedClass _serializedClass = new SerializedClass();

    private void OnEnable()
    {
        _serializedClass.myList.changed += OnMyListChanged;
        _serializedClass.myValue.changed += OnMyValueChanged;
        Test();
    }

    private void OnDisable()
    {
        _serializedClass.myList.changed -= OnMyListChanged;
        _serializedClass.myValue.changed -= OnMyValueChanged;
    }

    private void OnMyListChanged()
    {
        LogArray(_serializedClass.myList, "OnMyListChanged: ");
    }

    private void OnMyValueChanged(float value)
    {
        Debug.Log($"MyValueChanged: {value}");
    }

    void LogArray<T>(IEnumerable<T> array, string message = "") 
    {
        string output = message;
        foreach (T item in array)
        {
            output += $"{item},";
        }
        Debug.Log(output);
    }

    void Test()
    {
        _serializedClass.myList.AddRange(new []{1f, 5f, 10f, 120f});
        _serializedClass.myValue.value = 5f;
        string serializedString = JsonConvert.SerializeObject(_serializedClass);
        Debug.Log($"Serialized: \n{serializedString}");
        _serializedClass.myList.Clear();
        _serializedClass.myValue.value = 0;
        JsonConvert.PopulateObject(serializedString, _serializedClass);
        Debug.Log("Deserialized");
        LogArray(_serializedClass.myList);
        Debug.Log(_serializedClass.myValue.value);
    }
}
