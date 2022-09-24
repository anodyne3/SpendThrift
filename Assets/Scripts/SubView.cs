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

    protected override void OnAwake()
    {
        base.OnAwake();

        Instantiate(newControlPrefab, controlList);
        GenerateControls();
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

    private void GenerateControls()
    {
        var saveData = Database.GetDataList<T>();

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
