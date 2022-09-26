using System.Collections.Generic;
using UnityEngine;

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

    private readonly List<ToolsControlItem<T>> controls = new();

    protected List<T> controlData;

    protected virtual void Awake()
    {
        controlData = Database.GetDataList<T>();
        
        Instantiate(newControlPrefab, controlList);
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
