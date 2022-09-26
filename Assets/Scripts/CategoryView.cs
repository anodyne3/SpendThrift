public class CategoryView : SubView<CategoryData>
{
    public override void RefreshControls()
    {
        Database.TryClearUnassignedCategory();

        base.RefreshControls();
    }

    protected override ViewType GetViewType() => ViewType.Category;
}
