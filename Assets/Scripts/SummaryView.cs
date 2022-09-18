using UnityEngine;
using UnityEngine.UI;

public class SummaryView : View
{
    [SerializeField] private Button addSpendButton, editSpendButton;

    protected override void OnAwake()
    {
        base.OnAwake();

        addSpendButton.onClick.AddListener(AddSpend);
        editSpendButton.onClick.AddListener(EditSpend);
    }

    private void AddSpend()
    {
        ViewManager.ShowView(ViewType.Spend);
    }
    
    private void EditSpend()
    {
        ViewManager.ShowView(ViewType.Spend, new []{0});
    }
    
    public override ViewType GetViewType() => ViewType.Summary;
}