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
            ((CategoryControl)control).childCategories.Clear(); //is this necessary?
            
            if (control.Data.ParentCategoryId < 0)
                continue;

            if (controls.Find(x => x.Data.ID == control.Data.ParentCategoryId) is not CategoryControl parentControl)
                continue;

            if (control is CategoryControl childControl)
            {
                childControl.SetInitialWidth(parentControl.rectWidth - SubcategoryMargin);
                parentControl.childCategories.Add(childControl);
                SetSiblingIndex(parentControl, childControl);
            }

            parentControl.OnSorted();
        }
    }
    
    private void SetSiblingIndex(CategoryControl parentControlItem, CategoryControl childControlItem)
    {
        var parentIndex = controls.Find(x => x.Data.ID == parentControlItem.Data.ID);
        
        childControlItem.transform.SetSiblingIndex(parentIndex.transform.GetSiblingIndex() + parentControlItem.childCategories.Count);
    }

    protected override ViewType GetViewType() => ViewType.Category;
}