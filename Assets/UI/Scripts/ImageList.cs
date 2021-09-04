using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Signals;

public enum SelectionOperation
{
    ReplaceAll,
    Add,
    Subtract
}

public class ImageList : MonoBehaviour
{
    [SerializeField] ImageListButton _imageListButton = default;
    [SerializeField] ScrollRect _scrollRect = default;

    private SignalList<string> imagePaths => LabelSerializer.instance.labels.imagePaths;
    private SignalList<string> imagePathSelection => LabelSerializer.instance.labels.imagePathSelection;
    private Signal<string> workingImagePath => LabelSerializer.instance.labels.workingImagePath;
    
    private List<ImageListButton> _buttons = new List<ImageListButton>();
    
    void OnEnable()
    {
        imagePaths.changed += OnImagePathsChanged;
        imagePathSelection.changed += OnImagePathSelectionChanged;
        workingImagePath.changed += OnWorkingImagePathChanged;
        Rebuild();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousImage();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextImage();
        }
    }

    void OnDisable()
    {
        imagePaths.changed -= OnImagePathsChanged;
        imagePathSelection.changed -= OnImagePathSelectionChanged;
        workingImagePath.changed -= OnWorkingImagePathChanged;
    }

    private void OnWorkingImagePathChanged(string obj)
    {
        int imageIndex = FindImageIndex(workingImagePath.value);
        SelectImage(imageIndex);
        Rebuild();
    }

    private void OnImagePathSelectionChanged()
    {
        Rebuild();
    }

    private void OnImagePathsChanged()
    {
        Rebuild();
    }

    void OnImagesChanged(List<string> images)
    {
        Rebuild();
    }

    void Rebuild()
    {
        UpdateList();
        UpdateLabels();
    }

    void UpdateList()
    {
        if (imagePaths == null)
        {
            ResizeList(0);
        }
        else
        {
            ResizeList(imagePaths.Count);
        }
    }

    void ResizeList(int newSize)
    {
        int buttonCount = _buttons.Count;
        int removeObjects = buttonCount - newSize;
        int addObjects = newSize - buttonCount;

        for (int i = 0; i < removeObjects; i++)
        {
            GameObject.Destroy(_buttons[0].gameObject);
            _buttons.RemoveAt(0);
        }

        for (int i = 0; i < addObjects; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(_imageListButton.gameObject, _scrollRect.content, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            ImageListButton imageListButton = go.GetComponent<ImageListButton>();
            imageListButton.onClick += OnImageListButtonClick;
            _buttons.Add(imageListButton);
        }
    }

    void UpdateLabels()
    {
        if (imagePaths == null) return;
        for(int i = 0; i < imagePaths.Count; i++)
        {
            ImageListButton imageListButton = _buttons[i];
            imageListButton.text = imagePaths[i];
            imageListButton.selected = imagePathSelection.Contains(imagePaths[i]);
            imageListButton.indication = imagePaths[i] == workingImagePath.value;
        }
    }

    void OnImageListButtonClick(int index)
    {        
        SelectImage(index, GetSelectionOperation());
    }

    public int FindImageIndex(string path)
    {
        if (imagePaths == null || imagePaths.Count < 1) return -1;
        for(int i = 0; i < imagePaths.Count; i++)
        {
            if (imagePaths[i] == path) return i;
        }
        return -1;
    }

    public void PreviousImage()
    {
        if (imagePaths.Count == 0) return;
        int index = FindImageIndex(workingImagePath.value);
        if (index < 0) index = 0;
        int newIndex = Modulo(index - 1, imagePaths.Count);
        SelectImage(newIndex, GetSelectionOperation());
    }

    public void NextImage()
    {
        if (imagePaths.Count == 0) return;
        if (imagePaths.Count == 0) return;
        int index = FindImageIndex(workingImagePath.value);
        if (index < 0) index = 0;
        int newIndex = Modulo(index + 1, imagePaths.Count);
        SelectImage(newIndex, GetSelectionOperation());
    }

    public void SelectImage(int index, SelectionOperation selectionOperation = SelectionOperation.ReplaceAll)
    {
        switch(selectionOperation)
        {
            case SelectionOperation.ReplaceAll:
                imagePathSelection.Clear();
                if (imagePaths != null && index > -1 && index < imagePaths.Count)
                {
                    imagePathSelection.Add(imagePaths[index]);
                    workingImagePath.value = imagePaths[index];
                }
                break;
            case SelectionOperation.Add:                
                if (imagePaths != null && index > -1 && index < imagePaths.Count)
                {
                    if (!imagePathSelection.Contains(imagePaths[index]))
                    {
                        imagePathSelection.Add(imagePaths[index]);
                    }
                    workingImagePath.value = imagePaths[index];
                }
                break;
            case SelectionOperation.Subtract:
                if (imagePaths != null && index > -1 && index < imagePaths.Count)
                {
                    if (imagePathSelection.Contains(imagePaths[index]))
                    {
                        imagePathSelection.Remove(imagePaths[index]);
                    }
                    workingImagePath.value = imagePaths[index];
                }
                break;
        }
        
        Rebuild();
    }

    static int Modulo(int x, int m)
    {
        return (x % m + m) % m;
    }

    static SelectionOperation GetSelectionOperation()
    {
        bool add = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool subbtract = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        SelectionOperation selectionOperation = SelectionOperation.ReplaceAll;
        if (add) selectionOperation = SelectionOperation.Add;
        if (subbtract) selectionOperation = SelectionOperation.Subtract;
        return selectionOperation;
    }

}
