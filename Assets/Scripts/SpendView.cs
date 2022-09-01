using UnityEngine;
using UnityEngine.UI;

public class SpendView : View
{
    [SerializeField] private Button userButton, categoryButton;

    protected override void OnAwake()
    {
        base.OnAwake();

        userButton.onClick.AddListener(EditUsers);
        categoryButton.onClick.AddListener(EditCategories);
    }

    private void EditUsers()
    {
        ViewManager.ShowView(ViewType.User);
    }

    private void EditCategories()
    {
        ViewManager.ShowView(ViewType.Category);
    }

    public override ViewType GetViewType()
    {
        return ViewType.Spend;
    }
}
