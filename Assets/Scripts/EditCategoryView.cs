using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditCategoryView : EditView<CategoryData>
{
    [SerializeField] private TMP_InputField categoryName;
    [SerializeField] private TMP_Dropdown categoryDropdown;

    private int categoryId => categoryDropdown.value - 1;

    private void InitializeDropdown()
    {
        categoryDropdown.interactable = true;
        categoryDropdown.options = new List<TMP_Dropdown.OptionData> {new TMP_Dropdown.OptionData {text = "None"}};

        foreach (var category in Database.categoryData)
        {
            categoryDropdown.options.Add(new TMP_Dropdown.OptionData {text = category.name});
        }

        categoryDropdown.value = saveData?.parentCategoryId + 1 ?? 0;
        categoryDropdown.RefreshShownValue();
    }

    private void TestNameChange(string newName)
    {
        var isEmpty = string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName);
        var isValid = itemToolOptions == ItemToolOptions.Edit && newName == saveData?.name ||
                      Database.IsUniqueName<CategoryData>(newName);

        confirmChangesButton.interactable = !isEmpty && isValid;
        alertText.enabled = !isValid;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        categoryName.contentType = TMP_InputField.ContentType.Name;
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (itemToolOptions)
        {
            case ItemToolOptions.Edit:
                saveData.Update(categoryName.text, categoryId);
                break;
            case ItemToolOptions.Delete:
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

    protected override void RefreshView()
    {
        categoryName.text = saveData?.name;

        switch (itemToolOptions)
        {
            case ItemToolOptions.Delete:
                alertText.text = "Are you sure you wish to permanently remove this Category?";
                confirmChangesButton.interactable = true;
                categoryDropdown.interactable = false;
                break;
            default:
                alertText.text = "Please select a unique name for the new Category.";
                categoryName.onValueChanged.AddListener(TestNameChange);
                confirmChangesButton.interactable = itemToolOptions == ItemToolOptions.Edit;
                InitializeDropdown();
                break;
        }
    }

    public override ViewType GetViewType() => ViewType.EditCategory;
}
