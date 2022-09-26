using UnityEngine;
using UnityEngine.UI;

public class SummaryView : View
{
    [SerializeField] private Button addSpendButton, editSpendButton;

    protected void Awake()
    {
        addSpendButton.onClick.AddListener(AddSpend);
        editSpendButton.onClick.AddListener(EditSpend);
    }

    private void OnDestroy()
    {
        addSpendButton.onClick.RemoveListener(AddSpend);
        editSpendButton.onClick.RemoveListener(EditSpend);
    }

    private static void AddSpend()
    {
        ViewManager.ShowView(ViewType.Spend);
    }

    private static void EditSpend()
    {
        ViewManager.ShowView(ViewType.Spend, new[] {0});
    }

    protected override ViewType GetViewType() => ViewType.Summary;
}
