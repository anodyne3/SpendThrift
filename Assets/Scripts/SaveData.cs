using System;
using System.Xml.Linq;
using System.Xml.Serialization;


public interface ISaveData
{
    int id { get; }
}

public interface ISaveName : ISaveData
{
    string name { get; }
}

public class SaveData : ISaveData
{
    [XmlElement] public int id { get; set; }

    public SaveData()
    {
        id = -1;
    }

    protected SaveData(int newId)
    {
        id = newId;
    }

    protected static void Save() { }

    public void Load() { }

    public void Delete() { }

    public virtual bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        newSaveData = null;
        return false;
    }
}

public class UserData : SaveData, ISaveName
{
    public UserData() : base(-1) { }

    public UserData(int newId, string newName) : base(newId)
    {
        name = newName;
    }

    [XmlElement] public string name { get; set; }

    public override bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        if (int.TryParse(xElement.Element("id")?.Value, out var id))
        {
            newSaveData = new UserData(id, xElement.Element("name")?.Value);
            return true;
        }

        return base.TryParse(xElement, out newSaveData);
    }

    public void UpdateName(string newName)
    {
        name = newName;
        SaveSystem.SaveData<UserData>();
    }
}

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

    public override bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        if (int.TryParse(xElement.Element("id")?.Value, out var loadedId) &&
            int.TryParse(xElement.Element("parentCategoryId")?.Value, out var parentId))
        {
            newSaveData = new CategoryData(loadedId, xElement.Element("name")?.Value, parentId);
            return true;
        }

        return base.TryParse(xElement, out newSaveData);
    }

    public void Update(string newName, int newParentCategoryId)
    {
        name = newName;
        parentCategoryId = newParentCategoryId;
        SaveSystem.SaveData<CategoryData>();
    }
}

public class SpendData : SaveData
{
    public SpendData() { }

    public SpendData(int newId, DateTime newDate, int newCategory, float newAmount, string newDescription) : base(newId)
    {
        date = newDate;
        category = newCategory;
        amount = newAmount;
        description = newDescription;
    }

    public DateTime date { get; set; }
    public int category { get; set; }
    public float amount { get; set; }
    public string description { get; set; }

    public override bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        if (int.TryParse(xElement.Element("id")?.Value, out var id) &&
            DateTime.TryParse(xElement.Element("date")?.Value, out var date) &&
            int.TryParse(xElement.Element("category")?.Value, out var category) &&
            float.TryParse(xElement.Element("amount")?.Value, out var amount))
        {
            newSaveData = new SpendData(id, date, category, amount, xElement.Element("description")?.Value);
            return true;
        }

        return base.TryParse(xElement, out newSaveData);
    }
}
