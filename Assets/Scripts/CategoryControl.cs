using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoryControl : ToolsControlItem<CategoryData>
{
    public List<CategoryControl> childCategories;

    [SerializeField] private TextMeshProUGUI nameText;

    private Button toggleChildVisibility;
    private bool showChildren;

    private void Awake()
    {
        if (TryGetComponent(out toggleChildVisibility))
            toggleChildVisibility.onClick.AddListener(ToggleChildVisibility);
    }

    private void ToggleChildVisibility()
    {
        showChildren = !showChildren;

        foreach (var childCategory in childCategories)
            childCategory.gameObject.SetActive(showChildren);
    }

    protected override void Refresh()
    {
        base.Refresh();

        nameText.text = Data.Name;
    }
}