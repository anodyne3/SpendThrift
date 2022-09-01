using UnityEngine;
using UnityEngine.UI;

public class CategoryView : View
{
    [SerializeField] private Button addCategoryButton, editCategoryButton;

    protected override void OnAwake()
    {
        base.OnAwake();

        addCategoryButton.onClick.AddListener(AddCategory);
        editCategoryButton.onClick.AddListener(EditCategory);
    }

    public int GetFreeId()
    {
        return 1;
    }

    public int SelectUser()
    {
        return 0;
    }

    private void AddCategory() { }

    private void EditCategory() { }

    public override ViewType GetViewType()
    {
        return ViewType.Category;
    }
}
