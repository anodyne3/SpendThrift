using Newtonsoft.Json;
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
}
