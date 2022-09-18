public class NewCategoryControl : NewControl<CategoryData>
{
    protected override void ShowNewItemPanel()
    {
        ViewManager.ShowView(ViewType.EditCategory);
    }
}
