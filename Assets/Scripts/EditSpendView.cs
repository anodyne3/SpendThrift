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

    private DateTime dateFromInput => new(int.Parse(year.text), int.Parse(month.text), int.Parse(day.text));

    private float SpendAmount()
    {
        return float.TryParse(amount.text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out var spendAmount)
            ? spendAmount
            : 0.00f;
    }

    private void ShowSplits()
    {
        ViewManager.ShowView(ViewType.EditSplitShares, context);
    }

    private void InitializeDropdowns()
    {
        categoryDropdown.InitializeDropdown(Database.categoryData);
        categoryDropdown.ShowOptionById(saveData?.categoryId ?? Database.settingsData.defaultCategoryId);

        userDropdown.InitializeDropdown(Database.userData);

        var isSplit = saveData?.splitShares?.Count > 1;
        if (isSplit)
        {
            userDropdown.InsertOption(0, "Split...", -1);
        }

        userDropdown.ShowOptionById(isSplit
            ? -1
            : saveData?.splitShares?[0]?.userId ?? Database.settingsData.defaultUserId);

        userDropdown.interactable = !isSplit;
    }

    private void ValidateSpend(string newAmountText)
    {
        confirmChangesButton.interactable = userSplitButton.interactable =
            float.TryParse(newAmountText, NumberStyles.Currency, CultureInfo.CurrentCulture, out var newAmount) &&
            newAmount > 0;

        if (saveData == null)
            return;

        saveData.amount = newAmount;
    }

    private void UpdateUser(int index)
    {
        saveData.splitShares[0].userId = userDropdown.optionId;
    }

    private SpendData SetNewDataInDatabase()
    {
        var newSaveData = new SpendData(context[0],
            dateFromInput,
            categoryDropdown.optionId,
            SpendAmount(),
            description.text,
            new List<SpendData.SplitShare> {new(userDropdown.optionId)});

        Database.SetNewData(newSaveData);

        return newSaveData;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        amount.contentType = TMP_InputField.ContentType.DecimalNumber;

        day.contentType =
            month.contentType =
                year.contentType = TMP_InputField.ContentType.IntegerNumber;

        userSplitButton.onClick.AddListener(ShowSplits);
        amount.onValueChanged.AddListener(ValidateSpend);
        userDropdown.onValueChanged.AddListener(UpdateUser);
    }

    protected override void RefreshView()
    {
        InitializeDropdowns();

        day.text = saveData?.date.Day.ToString(CultureInfo.InvariantCulture) ?? DateTime.Today.Day.ToString();
        month.text = saveData?.date.Month.ToString(CultureInfo.InvariantCulture) ?? DateTime.Today.Month.ToString();
        year.text = saveData?.date.Year.ToString(CultureInfo.InvariantCulture) ?? DateTime.Today.Year.ToString();
        amount.text = saveData?.amount.ToString(CultureInfo.InvariantCulture);
        description.text = saveData?.description;
        isRecurringToggle.isOn = saveData?.isRecurring ?? false;
        
        confirmChangesButton.interactable = ItemToolOptions > 0;
        userSplitButton.interactable = saveData?.amount > 0;

        alertText.enabled = ItemToolOptions == ItemToolOptions.Delete;
        description.textComponent.enabled = ItemToolOptions != ItemToolOptions.Delete;

        if (ItemToolOptions == ItemToolOptions.Delete)
            alertText.text = "Are you sure you wish to permanently delete this Transaction?";
        
        saveData ??= SetNewDataInDatabase();
    }

    protected override void ConfirmChanges()
    {
        Hide();

        switch (ItemToolOptions)
        {
            case ItemToolOptions.Edit:
                saveData.date = dateFromInput;
                saveData.categoryId = categoryDropdown.optionId;
                saveData.amount = SpendAmount();
                saveData.description = description.text;
                saveData.isRecurring = isRecurringToggle.isOn;
                saveData.Save();
                break;
            case ItemToolOptions.Delete:
                DeleteItem();
                break;
            case ItemToolOptions.Duplicate:
                DuplicateItem(new SpendData(Database.GetFreeId<SpendData>(),
                    dateFromInput,
                    categoryDropdown.optionId,
                    SpendAmount(),
                    description.text,
                    saveData.splitShares));

                break;
            default:
                saveData.Save();

                break;
        }

        ViewManager.RefreshView(ViewType.Spend);
    }

    private void OnDestroy()
    {
        userSplitButton.onClick.RemoveListener(ShowSplits);
        amount.onValueChanged.RemoveListener(ValidateSpend);
        userDropdown.onValueChanged.RemoveListener(UpdateUser);
    }

    public override ViewType GetViewType() => ViewType.EditSpend;

    void IRefreshView.RefreshView()
    {
        RefreshView();
    }
}
