using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditCategoryView : EditView<CategoryData>
{
    [SerializeField] private TMP_InputField categoryName;
    [SerializeField] private TMP_Dropdown categoryDropdown;

    private int categoryId => categoriesToDropdownIndex[categoryDropdown.value];
    private Dictionary<int, int> categoriesToDropdownIndex = new();

    private void TestNameChange(string newName)
    {
        var isEmpty = string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName);
        var isValid = ItemToolOptions == ItemToolOptions.Edit && newName == saveData?.name ||
                      Database.IsUniqueName<CategoryData>(newName);

        confirmChangesButton.interactable = !isEmpty && isValid;
        alertText.enabled = !isValid;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        categoryName.contentType = TMP_InputField.ContentType.Name;
    }

    protected override void RefreshView()
    {
        categoryName.text = saveData?.name; //todo - DRY - EditUserView 

        categoryName.interactable = ItemToolOptions != ItemToolOptions.Delete;
        alertText.enabled = ItemToolOptions > 0 && ItemToolOptions != ItemToolOptions.Edit;
        categoriesToDropdownIndex =
            CategoryData.InitializeCategoryDropdown(categoryDropdown, saveData?.parentCategoryId ?? 0);

        switch (ItemToolOptions)
        {
            case ItemToolOptions.Delete:
                alertText.text = "Are you sure you wish to permanently remove this Category?";
                confirmChangesButton.interactable = true;
                categoryDropdown.interactable = false;
                break;
            default:
                alertText.text = "Please select a unique name for the new Category.";
                categoryName.onValueChanged.AddListener(TestNameChange);
                confirmChangesButton.interactable = ItemToolOptions == ItemToolOptions.Edit;
                break;
        }
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (ItemToolOptions)
        {
            case ItemToolOptions.Edit:
                saveData.name = categoryName.text;
                saveData.parentCategoryId = categoryId;
                saveData.Save();
                break;
            case ItemToolOptions.Delete:
                Database.CheckForStrays(saveData);
                DeleteItem();
                break;
            case ItemToolOptions.Duplicate:
                DuplicateItem(new CategoryData(Database.GetFreeId<CategoryData>(), categoryName.text, categoryId));
                break;
            default:
                Database.SetNewData(new CategoryData(context[0], categoryName.text, categoryId));
                break;
        }

        ViewManager.RefreshView(ViewType.Category);
    }

    protected override void OnHide()
    {
        base.OnHide();

        categoryName.onValueChanged.RemoveListener(TestNameChange);
    }

    public override ViewType GetViewType() => ViewType.EditCategory;
}
