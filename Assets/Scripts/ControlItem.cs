using UnityEngine;
using UnityEngine.UI;

public abstract class ControlItem<T> : MonoBehaviour
{
    protected T data { get; private set; }

    public void SetData(T newData)
    {
        data = newData;

        Refresh();
    }

    protected abstract void Refresh();
}

public abstract class ToolsControlItem<T> : ControlItem<T> where T : SaveData, new()
{
    private ItemToolDropdown itemToolDropdown;

    public void Initialize(ItemToolOptions itemToolOptions)
    {
        if (GetComponentInChildren(typeof(ItemToolDropdown)) is not ItemToolDropdown anItemToolDropdown)
            return;

        itemToolDropdown = anItemToolDropdown;
        itemToolDropdown.InitializeDropdown(itemToolOptions);
        itemToolDropdown.optionSelected.AddListener(ShowView);
    }

    private void ShowView(ItemToolOptions itemToolOptions)
    {
        var context = new[] {data?.id ?? Database.GetFreeId<T>(), (int) itemToolOptions};

        switch (data)
        {
            case SpendData _:
                ViewManager.ShowView(ViewType.EditSpend, context);
                break;
            case UserData userData when itemToolOptions == ItemToolOptions.Default:
                userData.SetAsDefault();
                break;
            case UserData _:
                ViewManager.ShowView(ViewType.EditUser, context);
                break;
            case CategoryData categoryData when itemToolOptions == ItemToolOptions.Default:
                categoryData.SetAsDefault();
                break;
            case CategoryData _:
                ViewManager.ShowView(ViewType.EditCategory, context);
                break;
        }
    }

    protected override void Refresh() { }

    private void OnDestroy() => itemToolDropdown.optionSelected.RemoveListener(ShowView);
}

public abstract class NewControl<T> : ControlItem<T>
{
    [SerializeField] private Button addItemButton;

    private void Awake() => addItemButton.onClick.AddListener(ShowNewItemPanel);

    protected abstract void ShowNewItemPanel();

    protected override void Refresh() { }

    private void OnDestroy() => addItemButton.onClick.RemoveListener(ShowNewItemPanel);
}
