public class NewSpendControl : NewControl<SpendData>
{
    protected override void ShowNewItemPanel()
    {
        ViewManager.ShowView(ViewType.EditSpend);
    }
}