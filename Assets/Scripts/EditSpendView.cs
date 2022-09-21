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
    [SerializeField] private RectTransform splitPanel;

    private List<SpendData.SplitShare> splitShares = new();
    private int categoryId => categoriesToDropdownIndex[categoryDropdown.value];
    private Dictionary<int, int> categoriesToDropdownIndex = new();
    private Dictionary<int, int> usersToDropdownIndex = new();

    private DateTime dateFromInput => new(int.Parse(year.text), int.Parse(month.text), int.Parse(day.text));

    private float SpendAmount()
    {
        return float.TryParse(amount.text, NumberStyles.Currency, NumberFormatInfo.CurrentInfo, out var spendAmount)
            ? spendAmount
            : 0.00f;
    }

    private void ShowSplits()
    {
        splitPanel.gameObject.SetActive(true);
    }

    private void InitializeDropdowns()
    {
        categoriesToDropdownIndex =
            CategoryData.InitializeCategoryDropdown(categoryDropdown,
                saveData?.categoryId ?? Database.settingsData.defaultCategoryId);

        usersToDropdownIndex = UserData.InitializeUserDropdown(userDropdown,
            saveData?.splitShares?[0]?.userId ?? Database.settingsData.defaultUserId);
    }

    private void ValidateSpend(string newName)
    {
        var zeroSpend = float.Parse(amount.text) <= 0;

        confirmChangesButton.interactable = !zeroSpend;
    }

    private void UpdateUser(int index)
    {
        splitShares[0].userId = usersToDropdownIndex[index];
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        amount.characterValidation = TMP_InputField.CharacterValidation.Decimal;

        day.contentType =
            month.contentType =
                year.contentType = TMP_InputField.ContentType.IntegerNumber;

        userSplitButton.onClick.AddListener(ShowSplits);
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

        switch (ItemToolOptions)
        {
            case ItemToolOptions.Delete:
                alertText.text = "Are you sure you wish to permanently delete this Transaction?";
                confirmChangesButton.interactable = true;
                break;
            default:
                confirmChangesButton.interactable = ItemToolOptions == ItemToolOptions.Edit;
                splitShares =
                    new List<SpendData.SplitShare>(saveData?.splitShares ?? new[] {SpendData.DefaultSplitShare()});

                amount.onValueChanged.AddListener(ValidateSpend);
                userDropdown.onValueChanged.AddListener(UpdateUser);
                break;
        }
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (ItemToolOptions)
        {
            case ItemToolOptions.Edit:
                saveData.date = dateFromInput;
                saveData.categoryId = categoryId;
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
                    categoryId,
                    SpendAmount(),
                    description.text));

                break;
            default:
                Database.SetNewData(new SpendData(context[0],
                    dateFromInput,
                    categoryId,
                    SpendAmount(),
                    description.text));

                break;
        }

        base.ConfirmChanges();

        ViewManager.RefreshView(ViewType.Spend);
    }

    protected override void OnHide()
    {
        base.OnHide();

        amount.onValueChanged.RemoveListener(ValidateSpend);
    }

    public override ViewType GetViewType() => ViewType.EditSpend;
}
