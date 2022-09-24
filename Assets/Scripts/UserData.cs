using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptOut)]
public class UserData : SaveData, ISaveName
{
    public UserData() : base(-1) { }

    public UserData(int newId, string newName) : base(newId)
    {
        name = newName;
    }

    public string name { get; set; }

    public void SetAsDefault()
    {
        Database.settingsData.defaultUserId = id;
        PlayerPrefs.SetInt(SaveSystem.DefaultUserKey, id);
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<UserData>();
    }
}
