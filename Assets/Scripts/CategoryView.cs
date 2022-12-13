public class CategoryView : SubView<CategoryData>
{
    public override void RefreshControls()
    {
        Database.TryClearUnassignedCategory();

        base.RefreshControls();
    }

    protected override void SortControls()
    {
        base.SortControls();

        foreach (var control in controls)
        {
            if (control.Data.ParentCategoryId == Database.UnassignedCategoryId)
                continue;

            var parentControl = controls.Find(x => x.Data.ParentCategoryId == control.Data.ParentCategoryId);
            ((CategoryControl)parentControl).childCategories.Add((CategoryControl)control);
        }
    }

    protected override ViewType GetViewType() => ViewType.Category;
}