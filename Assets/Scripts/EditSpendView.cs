using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IRefreshView
{
    public void RefreshView();
}

public class EditSpendView : EditView<SpendData>, IRefreshView
{
    [SerializeField] private TMP_InputField day, month, year, amount, description;
    [SerializeField] private DictionaryDropdown categoryDropdown;
    [SerializeField] private DictionaryDropdown userDropdown;
    [SerializeField] private Button userSplitButton;
    [SerializeField] private Toggle isRecurringToggle;

    private DateTime DateFromInput => new(int.Parse(year.text), int.Parse(month.text), int.Parse(day.text));

    private float SpendAmount =>
        float.TryParse(amount.text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out var spendAmount)
            ? spendAmount
            : 0.00f;

    protected override void Awake()
    {
        base.Awake();

        amount.contentType = TMP_InputField.ContentType.DecimalNumber;

        day.contentType =
            month.contentType =
                year.contentType = TMP_InputField.ContentType.IntegerNumber;

        userSplitButton.onClick.AddListener(ShowSplits);
        amount.onEndEdit.AddListener(ValidateSpend);
        userDropdown.onValueChanged.AddListener(UpdateUser);

        dataTypeName = "Transaction";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        userSplitButton.onClick.RemoveListener(ShowSplits);
        amount.onEndEdit.RemoveListener(ValidateSpend);
        userDropdown.onValueChanged.RemoveListener(UpdateUser);
    }

    private void ShowSplits()
    {
        ViewManager.ShowView(ViewType.EditSplitShares, context);
    }

    private void ValidateSpend(string amountText)
    {
        confirmChangesButton.interactable = userSplitButton.interactable =
            float.TryParse(amountText, NumberStyles.Currency, CultureInfo.CurrentCulture, out var newAmount) &&
            newAmount > 0;

        amount.SetTextWithoutNotify(newAmount.ToString("C", CultureInfo.CurrentCulture));
        SaveData.Amount = newAmount;
    }

    private void UpdateUser(int index)
    {
        if (SaveData.SplitShares == null)
            return;

        SaveData.SplitShares[0].UserId = userDropdown.OptionId;
    }

    void IRefreshView.RefreshView() => InitializeDropdowns();

    protected override void RefreshView()
    {
        if (SaveData.SplitShares.Count == 0)
            SaveData.SplitShares.Add(SplitShare.DefaultSplitShare());

        InitializeDropdowns();

        day.text = SaveData.Date.Day.ToString(CultureInfo.InvariantCulture);
        month.text = SaveData.Date.Month.ToString(CultureInfo.InvariantCulture);
        year.text = SaveData.Date.Year.ToString(CultureInfo.InvariantCulture);
        amount.text = SaveData.Amount.ToString("C", CultureInfo.CurrentCulture);
        description.text = SaveData.Description;
        isRecurringToggle.isOn = SaveData.IsRecurring;

        confirmChangesButton.interactable = itemToolOptions > 0;
        userSplitButton.interactable = SaveData.Amount > 0;

        RefreshAlertMessage(itemToolOptions == ItemToolOptions.Delete);
        description.textComponent.enabled = itemToolOptions != ItemToolOptions.Delete;
    }

    private void InitializeDropdowns()
    {
        categoryDropdown.InitializeDropdown(Database.CategoryData, new List<int> {Database.UnassignedCategoryId});
        categoryDropdown.ShowOptionById(SaveData.CategoryId);

        userDropdown.InitializeDropdown(Database.UserData);

        var isSplit = SaveData.SplitShares.Count > 1;
        if (isSplit)
            userDropdown.InsertOption(0, "Split...", -1);

        userDropdown.ShowOptionById(isSplit ? -1 : SaveData.SplitShares[0].UserId);

        userDropdown.interactable = !isSplit;
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (itemToolOptions)
        {
            case ItemToolOptions.Default:
                return;
            case ItemToolOptions.Delete:
                DeleteItem();
                break;
            case ItemToolOptions.Duplicate:
                DuplicateItem(new SpendData(SaveData.GetFreeId(),
                    DateFromInput,
                    categoryDropdown.OptionId,
                    SpendAmount,
                    description.text,
                    SaveData.SplitShares));

                break;
            default:
            case ItemToolOptions.Edit:
                SaveData.Date = DateFromInput;
                SaveData.CategoryId = categoryDropdown.OptionId;
                SaveData.Amount = SpendAmount;
                SaveData.Description = description.text;
                SaveData.IsRecurring = isRecurringToggle.isOn;
                SaveData.Save();
                break;
        }

        ViewManager.RefreshView(ViewType.Spend);
    }

    protected override ViewType GetViewType() => ViewType.EditSpend;
}
