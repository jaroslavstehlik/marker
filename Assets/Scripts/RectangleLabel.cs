using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RectangleLabel
{
    public string name;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    public Vector2 center => new Vector2(Mathf.Lerp(minX, maxX, 0.5f), Mathf.Lerp(minY, maxY, 0.5f));
    public Vector2 size => new Vector2(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY));
}
