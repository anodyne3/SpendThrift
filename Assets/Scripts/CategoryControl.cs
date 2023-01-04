using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryControl : ToolsControlItem<CategoryData>
{
    public List<CategoryControl> childCategories;
    public float rectWidth;

    [SerializeField] private TextMeshProUGUI nameText;

    private bool hasChildren => childCategories.Count > 0;

    private Button toggleChildVisibility;
    private bool showChildren;

    private readonly Quaternion shownAngle = new(0, 0, 0, 1), hiddenAngle = new(0, 0, 0.707106769f, 0.707106769f);

    private void Awake()
    {
        if (TryGetComponent(out toggleChildVisibility))
            toggleChildVisibility.onClick.AddListener(ToggleChildVisibility);
    }

    public void SetInitialWidth(float scrollViewWidth)
    {
        rectWidth = scrollViewWidth;

        if (TryGetComponent(typeof(RectTransform), out var component) && component is RectTransform rectTransform)
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, scrollViewWidth);
        }
    }

    public void OnSorted()
    {
        Refresh();
        RefreshChildVisibility();
    }

    private void ToggleChildVisibility()
    {
        if (!hasChildren)
            return;

        showChildren = !showChildren;

        RefreshChildVisibility();
    }

    private void RefreshChildVisibility()
    {
        toggleChildVisibility.image.transform.localRotation = showChildren ? shownAngle : hiddenAngle;

        foreach (var childCategory in childCategories)
        {
            if (childCategory.hasChildren && childCategory.showChildren)
                childCategory.ToggleChildVisibility();

            childCategory.gameObject.SetActive(showChildren);
        }
    }

    protected override void Refresh()
    {
        base.Refresh();

        nameText.text = Data.Name;
        toggleChildVisibility.image.enabled = hasChildren;
    }
}