using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptOut)]
public class CategoryData : SaveData, ISaveName
{
    public CategoryData() : base(0) { }

    public CategoryData(int id, string name, int parentCategoryId) : base(id)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
    }

    public string Name { get; set; }
    public int ParentCategoryId { get; set; } = -1;

    public void SetAsDefault()
    {
        Database.SettingsData.DefaultCategoryId = ID;
        PlayerPrefs.SetInt(SaveSystem.DefaultCategoryKey, ID);
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<CategoryData>();
    }
}
