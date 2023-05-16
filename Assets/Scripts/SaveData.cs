public interface ISaveData
{
    int ID { get; set; }
    void Save();
}

public interface ISaveName : ISaveData
{
    string Name { get; }
}

public class SaveData : ISaveData
{
    public int ID { get; set; }

    public SaveData()
    {
        ID = -1;
    }

    protected SaveData(int newId)
    {
        ID = newId;
    }

    public virtual void Save() { } //virtual??
}

public class SettingsData : SaveData
{
    public int DefaultUserId { get; set; }
    public int DefaultCategoryId { get; set; }
    public SpendDateRangeData DateRangeData { get; set; }
}

public class SpendDateRangeData : SaveData
{
    public DateRangeType DateRangeType { get; set; } = DateRangeType.Month;
    public DateRange DateRange { get; set; }
}