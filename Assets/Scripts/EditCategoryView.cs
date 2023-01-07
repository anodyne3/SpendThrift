using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditCategoryView : EditView<CategoryData>
{
    [SerializeField] private TMP_InputField categoryName;
    [SerializeField] private DictionaryDropdown categoryDropdown;

    protected override void Awake()
    {
        base.Awake();

        categoryName.onValueChanged.AddListener(TestNameChange);

        dataTypeName = "Category";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        categoryName.onValueChanged.RemoveListener(TestNameChange);
    }

    protected override void RefreshView()
    {
        RefreshName(SaveData, categoryName);
        RefreshAlertMessage(itemToolOptions > 0 && itemToolOptions != ItemToolOptions.Edit);

        categoryDropdown.InitializeDropdown(Database.CategoryData,
            new List<int> { SaveData.ID, Database.UnassignedCategoryId });
        categoryDropdown.InsertOption(0, "None", -1);
        categoryDropdown.ShowOptionById(SaveData?.ParentCategoryId ?? -1);

        confirmChangesButton.interactable = itemToolOptions is ItemToolOptions.Delete or ItemToolOptions.Edit;
        categoryDropdown.interactable = itemToolOptions != ItemToolOptions.Delete;
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (itemToolOptions)
        {
            case ItemToolOptions.Default:
                return;
            default:
            case ItemToolOptions.Edit:
                SaveData.Name = categoryName.text;
                SaveData.ParentCategoryId = categoryDropdown.OptionId;
                SaveData.Save();
                break;
            case ItemToolOptions.Delete:
                if (Database.SettingsData.DefaultCategoryId == SaveData.ID)
                    Database.SettingsData.DefaultCategoryId = 0;

                Database.CheckForStrays(SaveData);
                DeleteItem();
                break;
            case ItemToolOptions.Duplicate:
                DuplicateItem(new CategoryData(SaveData.GetFreeId(),
                    categoryName.text,
                    categoryDropdown.OptionId));

                break;
        }

        ViewManager.RefreshView(ViewType.Category);
    }

    protected override ViewType GetViewType() => ViewType.EditCategory;
}