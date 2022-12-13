using UnityEngine;
using UnityEngine.UI;

public abstract class ControlItem<T> : MonoBehaviour
{
    protected internal T Data { get; private set; }

    public void SetData(T data)
    {
        Data = data;

        Refresh();
    }

    protected abstract void Refresh();
}

public abstract class ToolsControlItem<T> : ControlItem<T> where T : SaveData, new()
{
    private ItemToolDropdown itemToolDropdown;

    //merge with initialization
    private void Init()
    {
        if (GetComponentInChildren(typeof(ItemToolDropdown)) is not ItemToolDropdown anItemToolDropdown)
            return;

        itemToolDropdown = anItemToolDropdown;
        itemToolDropdown.optionSelected.AddListener(ShowView);
    }

    private void OnDestroy()
    {
        if (itemToolDropdown)
            itemToolDropdown.optionSelected.RemoveListener(ShowView);
    }

    private void ShowView(ItemToolOptions itemToolOptions)
    {
        var context = new[] { Data?.ID ?? Data.GetFreeId(), (int)itemToolOptions };

        switch (Data)
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

    public void Initialize(ItemToolOptions itemToolOptions)
    {
        if (!itemToolDropdown)
            Init();

        itemToolDropdown.interactable = Data.ID != Database.UnassignedCategoryId || Data is not CategoryData;

        itemToolDropdown.InitializeDropdown(itemToolOptions);
    }

    protected override void Refresh() { }
}

public abstract class NewControl<T> : ControlItem<T>
{
    [SerializeField] private Button addItemButton;

    private void Awake() => addItemButton.onClick.AddListener(ShowNewItemPanel);
    private void OnDestroy() => addItemButton.onClick.RemoveListener(ShowNewItemPanel);
    protected abstract void ShowNewItemPanel();
    protected override void Refresh() { }
}