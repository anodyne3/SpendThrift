using UnityEngine;

public class CategoryView : SubView<CategoryData>
{
    private const float SubcategoryMargin = 50.0f;

    protected override void Awake()
    {
        base.Awake();

        if (newControlItem.TryGetComponent(typeof(NewControl<CategoryData>), out var component) &&
            component is NewCategoryControl newCategoryControl &&
            newCategoryControl.TryGetComponent(typeof(RectTransform), out component) &&
            component is RectTransform rectTransform)
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollViewWidth);
    }

    protected override void GenerateControls()
    {
        base.GenerateControls();

        foreach (var item in controls)
            if (item is CategoryControl categoryControl)
                categoryControl.SetInitialWidth(scrollViewWidth);

        SortControls();
    }

    public override void RefreshControls()
    {
        Database.TryClearUnassignedCategory();

        base.RefreshControls();

        SortControls();
    }

    protected override void SortControls()
    {
        base.SortControls();

        foreach (var control in controls)
        {
            if (control is CategoryControl categoryControl)
                categoryControl.childCategories.Clear();
        }

        foreach (var control in controls)
        {
            if (control.Data.ID == Database.UnassignedCategoryId)
            {
                control.transform.SetSiblingIndex(1);
                continue;
            }
            
            if (control.Data.ParentCategoryId < 0)
                continue;

            if (controls.Find(x => x.Data.ID == control.Data.ParentCategoryId) is not CategoryControl parentControl)
                continue;

            if (control is CategoryControl childControl)
            {
                childControl.SetInitialWidth(parentControl.rectWidth);
                parentControl.childCategories.Add(childControl);
                SetSiblingIndex(parentControl, childControl);
            }

            parentControl.OnSorted();
        }

        foreach (var control in controls)
        {
            if (control is not CategoryControl categoryControl || categoryControl.childCategories.Count < 1)
                continue;

            foreach (var childCategory in categoryControl.childCategories)
                childCategory.SetInitialWidth(categoryControl.rectWidth - SubcategoryMargin);
        }
    }

    private void SetSiblingIndex(CategoryControl parentControlItem, Component childControlItem)
    {
        var parentIndex = controls.Find(x => x.Data.ID == parentControlItem.Data.ID);

        childControlItem.transform.SetSiblingIndex(parentIndex.transform.GetSiblingIndex() +
                                                   parentControlItem.childCategories.Count);
    }

    protected override ViewType GetViewType() => ViewType.Category;
}