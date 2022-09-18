using System.Xml.Linq;
using System.Xml.Serialization;


public interface ISaveData
{
    int id { get; }
}

public interface ISaveName : ISaveData
{
    string name { get; }
    void SetAsDefault();
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

    public virtual void Save() { }

    public virtual bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        newSaveData = null;
        return false;
    }
}

public class SettingsData : SaveData
{
    public int defaultUserId { get; set; }
    public int defaultCategoryId { get; set; }
}
