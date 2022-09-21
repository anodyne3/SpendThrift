using System;
using Newtonsoft.Json;

[JsonObject(MemberSerialization.OptOut)]
public class SpendData : SaveData
{
    public SpendData() { }

    public SpendData(int newId, DateTime newDate, int newCategory, float newAmount, string newDescription) : base(newId)
    {
        date = newDate;
        categoryId = newCategory;
        amount = newAmount;
        description = newDescription;
    }

    public DateTime date { get; set; }
    public int categoryId { get; set; }
    public float amount { get; set; }
    public string description { get; set; }
    public SplitShare[] splitShares { get; set; }
    public bool isRecurring { get; set; }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<SpendData>();
    }

    [Serializable]
    public class SplitShare
    {
        // public int spendId { get; set; }
        public int userId { get; set; }
        public float paymentSplit { get; set; } = 1.0f;
        public float liabilitySplit { get; set; } = 1.0f;
    }
    
    public static SplitShare DefaultSplitShare(/*int id*/)
    {
        return new SplitShare {/*spendId = id, */userId = Database.settingsData.defaultUserId};
    }
}
