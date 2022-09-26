using TMPro;
using UnityEngine;

public class CategoryControl : ToolsControlItem<CategoryData>
{
    [SerializeField] private TextMeshProUGUI nameText;

    protected override void Refresh()
    {
        base.Refresh();

        nameText.text = Data.Name;
    }
}
