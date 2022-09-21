public interface ISaveData
{
    int id { get; }
    void Save();
}

public interface ISaveName : ISaveData
{
    string name { get; }
    void SetAsDefault();
}

public class SaveData : ISaveData
{
    public int id { get; set; }

    public SaveData()
    {
        id = -1;
    }

    protected SaveData(int newId)
    {
        id = newId;
    }

    public virtual void Save() { }
}

public class SettingsData : SaveData
{
    public int defaultUserId { get; set; }
    public int defaultCategoryId { get; set; }
}
