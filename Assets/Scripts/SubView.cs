using System.Collections.Generic;
using UnityEngine;

public interface IRefreshControls
{
    public abstract void RefreshControls();
}

public abstract class SubView<T> : View, IRefreshControls where T : SaveData, new()
{
    [SerializeField] private RectTransform controlList;
    [SerializeField] private ToolsControlItem<T> controlPrefab;
    [SerializeField] private NewControl<T> newControlPrefab;
    [SerializeField] private ItemToolOptions dropdownOptions;

    private readonly List<ToolsControlItem<T>> controls = new List<ToolsControlItem<T>>();

    protected override void OnAwake()
    {
        base.OnAwake();

        Initialize(Database.GetData<T>());
    }

    public void Initialize(List<T> saveData)
    {
        base.OnAwake();

        Instantiate(newControlPrefab, controlList);
        GenerateControls(saveData);
    }

    public virtual void RefreshControls()
    {
        foreach (var control in controls)
        {
            Destroy(control.gameObject);
        }

        controls.Clear();
        GenerateControls(Database.GetData<T>());
    }

    protected virtual void GenerateControls(List<T> saveData)
    {
        if (saveData == null)
            return;

        foreach (var item in saveData)
        {
            var control = Instantiate(controlPrefab, controlList);
            control.Initialize(dropdownOptions);
            control.SetData(item);
            controls.Add(control);
        }
    }
}
