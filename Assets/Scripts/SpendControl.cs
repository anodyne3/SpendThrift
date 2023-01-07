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
        categoryText.text = LongName();
        descriptionText.text = Data.Description;
    }

    private string LongName()
    {
        var catData = Database.GetSaveData<CategoryData>(Data.CategoryId);
        var longName = catData.Name;

        while (catData.ParentCategoryId > -1)
        {
            catData = Database.GetSaveData<CategoryData>(catData.ParentCategoryId);
            longName = catData.Name + " - " + longName;
        }

        return longName;
    }
}
