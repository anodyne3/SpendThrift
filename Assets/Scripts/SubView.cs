using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IRefreshControls
{
    public void RefreshControls();
}

public abstract class SubView<T> : View, IRefreshControls where T : SaveData, new()
{
    [SerializeField] private RectTransform controlList;
    [SerializeField] private ToolsControlItem<T> controlPrefab;
    [SerializeField] private NewControl<T> newControlPrefab;
    [SerializeField] private ItemToolOptions dropdownOptions;

    protected readonly List<ToolsControlItem<T>> controls = new();
    protected NewControl<T> newControlItem;

    protected List<T> controlData;
    protected float scrollViewWidth;

    protected virtual void Awake()
    {
        controlData = Database.GetDataList<T>();

        if (GetComponentInChildren(typeof(ScrollRect), true) is ScrollRect scrollView)
        {
            if (scrollView.TryGetComponent(typeof(RectTransform), out var component) &&
                component is RectTransform rectTransform)
                scrollViewWidth = rectTransform.rect.width;
        }

        newControlItem = Instantiate(newControlPrefab, controlList);

        GenerateControls();
    }

    protected virtual void GenerateControls()
    {
        if (controlData == null)
            return;

        foreach (var item in controlData)
        {
            var control = Instantiate(controlPrefab, controlList);
            control.SetData(item);
            control.Initialize(dropdownOptions);
            controls.Add(control);
        }
    }

    protected virtual void SortControls()
    {
    }

    public virtual void RefreshControls()
    {
        foreach (var control in controls)
        {
            Destroy(control.gameObject);
        }

        controls.Clear();
        GenerateControls();
    }
}