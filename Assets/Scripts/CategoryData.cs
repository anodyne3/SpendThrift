using System.Xml.Linq;
using UnityEngine;

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
        PlayerPrefs.SetInt(SaveSystem.DefaultCategoryKey, id);
    }

    public override bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        if (!int.TryParse(xElement.Element("id")?.Value, out var saveId) ||
            !int.TryParse(xElement.Element("parentCategoryId")?.Value, out var saveParentCategoryId))
            return base.TryParse(xElement, out newSaveData);

        newSaveData = new CategoryData(saveId, xElement.Element("name")?.Value, saveParentCategoryId);
        return true;
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<CategoryData>();
    }
}
