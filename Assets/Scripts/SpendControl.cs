using System.Globalization;
using TMPro;
using UnityEngine;

public class SpendControl : ToolsControlItem<SpendData>
{
    [SerializeField] private TextMeshProUGUI dateText, amountText, categoryText, descriptionText;

    protected override void Refresh()
    {
        base.Refresh();

        dateText.text = data.date.ToString("ddd dd MMM yy");
        amountText.text = data.amount.ToString("C", CultureInfo.CurrentCulture);
        categoryText.text = Database.GetCategoryData(data.category).name;
        descriptionText.text = data.description;
    }
}
