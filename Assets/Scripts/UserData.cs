using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class UserData : SaveData, ISaveName
{
    public UserData() : base(-1) { }

    public UserData(int newId, string newName) : base(newId)
    {
        name = newName;
    }

    [XmlElement] public string name { get; set; }

    public void SetAsDefault()
    {
        PlayerPrefs.SetInt(SaveSystem.DefaultUserKey, id);
    }
    
    public override bool TryParse(XElement xElement, out SaveData newSaveData)
    {
        if (!int.TryParse(xElement.Element("id")?.Value, out var saveId))
            return base.TryParse(xElement, out newSaveData);

        newSaveData = new UserData(saveId, xElement.Element("name")?.Value);
        return true;
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<UserData>();
    }
}
