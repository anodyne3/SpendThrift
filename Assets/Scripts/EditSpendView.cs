using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditSpendView : EditView<SpendData>
{
    [SerializeField] private TMP_InputField day, month, year, amount, description;
    [SerializeField] private TMP_Dropdown categoryDropdown;
    [SerializeField] private TMP_Dropdown userDropdown;
    [SerializeField] private Button userSplitButton;
    [SerializeField] private Toggle isRecurringToggle;

    private List<SpendData.SplitShare> splitShares = new();

    private DateTime dateFromInput => new(int.Parse(year.text), int.Parse(month.text), int.Parse(day.text));

    private float SpendAmount()
    {
        return float.TryParse(amount.text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out var spendAmount)
            ? spendAmount
            : 0.00f;
    }

    private void InitializeDropdowns()
    {
        categoryDropdown.interactable = true;
        categoryDropdown.options.Clear();

        foreach (var category in Database.categoryData)
        {
            categoryDropdown.options.Add(new TMP_Dropdown.OptionData {text = category.name});
        }

        categoryDropdown.value = saveData?.category ?? Database.DefaultCategory.id;
        categoryDropdown.RefreshShownValue();

        userDropdown.interactable = true;
        userDropdown.options.Clear();

        foreach (var user in Database.userData)
        {
            userDropdown.options.Add(new TMP_Dropdown.OptionData {text = user.name});
        }

        userDropdown.value = splitShares[0].userId;
        userDropdown.RefreshShownValue();
    }

    private void ValidateSpend(string newName)
    {
        var zeroSpend = float.Parse(amount.text) <= 0;

        confirmChangesButton.interactable = !zeroSpend;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        amount.characterValidation = TMP_InputField.CharacterValidation.Decimal;

        day.contentType =
            month.contentType =
                year.contentType = TMP_InputField.ContentType.IntegerNumber;
    }

    protected override void OnShow()
    {
        base.OnShow();

        InitializeDropdowns();
    }

    protected override void RefreshView()
    {
        day.text = saveData?.date.Day.ToString(CultureInfo.InvariantCulture) ?? DateTime.Today.Day.ToString();
        month.text = saveData?.date.Month.ToString(CultureInfo.InvariantCulture) ?? DateTime.Today.Month.ToString();
        year.text = saveData?.date.Year.ToString(CultureInfo.InvariantCulture) ?? DateTime.Today.Year.ToString();
        amount.text = saveData?.amount.ToString(CultureInfo.InvariantCulture);
        description.text = saveData?.description;

        switch (itemToolOptions)
        {
            case ItemToolOptions.Delete:
                alertText.text = "Are you sure you wish to permanently delete this Transaction?";
                confirmChangesButton.interactable = true;
                break;
            default:
                confirmChangesButton.interactable = itemToolOptions == ItemToolOptions.Edit;
                splitShares = new List<SpendData.SplitShare>(saveData?.splitShares ??
                                                             new[] {SpendData.DefaultSplitShare(saveData?.id ?? 0)});

                amount.onValueChanged.AddListener(ValidateSpend);
                userDropdown.onValueChanged.AddListener((x) => splitShares[0].userId = x);
                break;
        }
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (itemToolOptions)
        {
            case ItemToolOptions.Edit:
                saveData.date = dateFromInput;
                saveData.category = categoryDropdown.value;
                saveData.amount = SpendAmount();
                saveData.description = description.text;
                saveData.isRecurring = isRecurringToggle.isOn;
                saveData.splitShares = splitShares.ToArray();
                saveData.Save();
                break;
            case ItemToolOptions.Delete:
                DeleteItem();
                break;
            case ItemToolOptions.Duplicate:
                DuplicateItem(new SpendData(Database.GetFreeId<SpendData>(),
                    dateFromInput,
                    categoryDropdown.value,
                    SpendAmount(),
                    description.text));

                break;
            default:
                Database.SetNewData(new SpendData(context[0],
                    dateFromInput,
                    categoryDropdown.value,
                    SpendAmount(),
                    description.text));

                break;
        }

        ViewManager.RefreshView(ViewType.Spend);
    }

    protected override void OnHide()
    {
        base.OnHide();

        amount.onValueChanged.RemoveListener(ValidateSpend);
    }

    public override ViewType GetViewType() => ViewType.EditSpend;
}
