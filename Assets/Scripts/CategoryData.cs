using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

[JsonObject(MemberSerialization.OptOut)]
public class CategoryData : SaveData, ISaveName
{
    public CategoryData() : base(0) { }

    public CategoryData(int newId, string newName, int newParentCategoryId) : base(newId)
    {
        name = newName;
        parentCategoryId = newParentCategoryId;
    }

    public string name { get; set; }
    public int parentCategoryId { get; set; }

    public void SetAsDefault()
    {
        Database.settingsData.defaultCategoryId = id;
        PlayerPrefs.SetInt(SaveSystem.DefaultCategoryKey, id);
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<CategoryData>();
    }

    //todo - only initialize on change to categoryData - abstraction?
    public static Dictionary<int, int> InitializeCategoryDropdown(TMP_Dropdown categoryDropdown, int categoryId)
    {
        categoryDropdown.interactable = true;
        var categoriesToDropdownIndex = new Dictionary<int, int> {{0, -1}};
        categoryDropdown.options = new List<TMP_Dropdown.OptionData> {new() {text = "None"}};

        foreach (var category in Database.categoryData)
        {
            categoriesToDropdownIndex.Add(categoryDropdown.options.Count, category.id);
            categoryDropdown.options.Add(new TMP_Dropdown.OptionData {text = category.name});
        }

        categoryDropdown.value = 0;
        foreach (var kvp in categoriesToDropdownIndex)
            if (kvp.Value == categoryId)
                categoryDropdown.value = kvp.Key;

        categoryDropdown.RefreshShownValue();

        return categoriesToDropdownIndex;
    }
}
