using System.Globalization;
using TMPro;
using UnityEngine;

public class SpendControl : ToolsControlItem<SpendData>
{
    [SerializeField] private TextMeshProUGUI dateText, amountText, categoryText, descriptionText;

    protected override void Refresh()
    {
        base.Refresh();

        dateText.text = Data.Date.ToString("ddd dd MMM yy");
        amountText.text = Data.Amount.ToString("C", CultureInfo.CurrentCulture);
        categoryText.text = Database.GetSaveData<CategoryData>(Data.CategoryId).Name;
        descriptionText.text = Data.Description;
    }
}
