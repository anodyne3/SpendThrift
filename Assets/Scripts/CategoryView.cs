public class CategoryView : SubView<CategoryData>
{
    public override void RefreshControls()
    {
        Database.TryClearUnassignedCategory();
        
        base.RefreshControls();
    }

    public override ViewType GetViewType() => ViewType.Category;
}
