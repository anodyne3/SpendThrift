using System.Globalization;
using TMPro;
using UnityEngine;

public class SpendControl : ToolsControlItem<SpendData>
{
    [SerializeField] private TextMeshProUGUI dateText, amountText, categoryText, descriptionText;

    protected override void Refresh()
    {
        base.Refresh();

        dateText.text = data.date.ToString("dd MMM yy");
        amountText.text = data.amount.ToString("C", CultureInfo.CurrentCulture);
        categoryText.text = Database.GetSaveData<CategoryData>(data.categoryId).name;
        descriptionText.text = data.description;
    }
}
