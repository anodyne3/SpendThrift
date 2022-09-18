using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class EditSpendView : EditView<SpendData>
{
    [SerializeField] private TMP_InputField day, month, year, amount, description;

    private int categoryId;

    protected override void OnAwake()
    {
        base.OnAwake();

        amount.characterValidation = TMP_InputField.CharacterValidation.Decimal;

        day.contentType =
        month.contentType = 
        year.contentType = TMP_InputField.ContentType.IntegerNumber;
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        var newDate = new DateTime(int.Parse(year.text), int.Parse(month.text), int.Parse(day.text));
        var newAmount = float.Parse(amount.text);

        Database.SetNewData(new SpendData(context[0], newDate, categoryId, newAmount, description.text));
    }

    protected override void RefreshView()
    {
        day.text = saveData?.date.Day.ToString(CultureInfo.InvariantCulture);
        month.text = saveData?.date.Month.ToString(CultureInfo.InvariantCulture);
        year.text = saveData?.date.Year.ToString(CultureInfo.InvariantCulture);
        amount.text = saveData?.amount.ToString(CultureInfo.InvariantCulture);
        description.text = saveData?.description;
    }

    public override ViewType GetViewType() => ViewType.EditSpend;
}
