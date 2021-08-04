using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    Labels labels = default;
    List<string> imagePaths => labels.imagePaths;
    List<string> imagePathSelection => labels.imagePathSelection;
    
    List<ImageListButton> buttons = new List<ImageListButton>();
    
    void OnEnable()
    {
        LabelSerializer.instance.onDataLoaded += OnDataLoaded;
        OnDataLoaded(LabelSerializer.instance.labels);
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
        LabelSerializer.instance.onDataLoaded -= OnDataLoaded;
    }

    void OnDataLoaded(Labels labels)
    {
        if (this.labels != labels)
        {
            this.labels = labels;
            this.labels.onImagesChanged += OnImagesChanged;
        }

        int imageIndex = FindImageIndex(this.labels.workingImagePath);        
        SelectImage(imageIndex);
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
        int buttonCount = buttons.Count;
        int removeObjects = buttonCount - newSize;
        int addObjects = newSize - buttonCount;

        for (int i = 0; i < removeObjects; i++)
        {
            GameObject.Destroy(buttons[0].gameObject);
            buttons.RemoveAt(0);
        }

        for (int i = 0; i < addObjects; i++)
        {
            GameObject go = GameObject.Instantiate<GameObject>(_imageListButton.gameObject, _scrollRect.content, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            ImageListButton imageListButton = go.GetComponent<ImageListButton>();
            imageListButton.onClick += OnImageListButtonClick;
            buttons.Add(imageListButton);
        }
    }

    void UpdateLabels()
    {
        if (imagePaths == null) return;
        for(int i = 0; i < imagePaths.Count; i++)
        {
            ImageListButton imageListButton = buttons[i];
            imageListButton.text = imagePaths[i];
            imageListButton.selected = imagePathSelection.Contains(imagePaths[i]);
            imageListButton.indication = imagePaths[i] == labels.workingImagePath;
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
        int index = FindImageIndex(labels.workingImagePath);
        if (index < 0) index = 0;
        int newIndex = Modulo(index - 1, imagePaths.Count);
        SelectImage(newIndex, GetSelectionOperation());
    }

    public void NextImage()
    {
        if (imagePaths.Count == 0) return;
        if (imagePaths.Count == 0) return;
        int index = FindImageIndex(labels.workingImagePath);
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
                    labels.workingImagePath = imagePaths[index];
                }
                break;
            case SelectionOperation.Add:                
                if (imagePaths != null && index > -1 && index < imagePaths.Count)
                {
                    if (!imagePathSelection.Contains(imagePaths[index]))
                    {
                        imagePathSelection.Add(imagePaths[index]);
                    }
                    labels.workingImagePath = imagePaths[index];
                }
                break;
            case SelectionOperation.Subtract:
                if (imagePaths != null && index > -1 && index < imagePaths.Count)
                {
                    if (imagePathSelection.Contains(imagePaths[index]))
                    {
                        imagePathSelection.Remove(imagePaths[index]);
                    }
                    labels.workingImagePath = imagePaths[index];
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
