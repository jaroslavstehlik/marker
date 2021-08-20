using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    [SerializeField] private RectTransform _content = default;
    [SerializeField] private ClassRectangle _classRectanglePrefab = default;

    private List<ClassRectangle> _rectangles;

    private void Awake()
    {
        _rectangles = new List<ClassRectangle>();
    }

    private void OnEnable()
    {
        RebuildLabels();
        LabelSerializer.instance.labels.workingImagePathChanged += WorkingImagePathChanged;
    }

    private void OnDisable()
    {
        LabelSerializer.instance.labels.workingImagePathChanged -= WorkingImagePathChanged;
    }

    void WorkingImagePathChanged(string workingImagePath)
    {
        StartCoroutine(LateRebuildLabels());
    }

    IEnumerator LateRebuildLabels()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        RebuildLabels();
    }

    public void RebuildLabels()
    {
        LabelSerializer.instance.labels.rectangleLabels.TryGetValue(LabelSerializer.instance.labels.workingImagePath, 
            out List<RectangleLabel> rectangleLabels);
        
        int newSize = rectangleLabels != null ? rectangleLabels.Count : 0;
        int labelCount = _rectangles.Count;
        int removeObjects = labelCount - newSize;
        int addObjects = newSize - labelCount;

        for (int i = 0; i < removeObjects; i++)
        {
            GameObject.Destroy(_rectangles[0].gameObject);
            _rectangles.RemoveAt(0);
        }

        for (int i = 0; i < addObjects; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(_classRectanglePrefab.gameObject, _content, false);
            ClassRectangle classRectangle = go.GetComponent<ClassRectangle>();
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            _rectangles.Add(classRectangle);
        }

        if (rectangleLabels != null)
        {
            for (int i = 0; i < rectangleLabels.Count; i++)
            {
                RectangleLabel rectangleLabel = rectangleLabels[i];
                _rectangles[i].label = rectangleLabel.name;
                _rectangles[i].center = rectangleLabel.center;
                _rectangles[i].size = rectangleLabel.size;
            }
        }
    }
}
