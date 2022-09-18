using System;
using System.Xml.Linq;

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
    public SplitShare[] splitShares { get; set; }
    public bool isRecurring { get; set; }

    public override bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        if (!int.TryParse(xElement.Element("id")?.Value, out var saveId) ||
            !DateTime.TryParse(xElement.Element("date")?.Value, out var saveDate) ||
            !int.TryParse(xElement.Element("category")?.Value, out var saveCategory) ||
            !float.TryParse(xElement.Element("amount")?.Value, out var saveAmount))
            return base.TryParse(xElement, out newSaveData);

        newSaveData = new SpendData(saveId, saveDate, saveCategory, saveAmount, xElement.Element("description")?.Value);
        return true;
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<SpendData>();
    }

    [Serializable]
    public class SplitShare
    {
        public int spendId { get; set; }
        public int userId { get; set; }
        public float split { get; set; } = 1.0f;
    }
    
    public static SplitShare DefaultSplitShare(int id)
    {
        return new SplitShare {spendId = id, userId = Database.DefaultUser.id};
    }
}
