using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClassRect : MonoBehaviour
{
    [SerializeField] private ClassRectangle _classRectangle;
    [SerializeField] private Color _color;
    [SerializeField] private string _label;
    [SerializeField] private Vector2 _center;
    [SerializeField] private Vector2 _size = Vector2.one;

    private void Update()
    {
        _classRectangle.center = _center;
        _classRectangle.size = _size;
        _classRectangle.label = _label;
        _classRectangle.color = _color;
    }
}
