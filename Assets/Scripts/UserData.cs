using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptOut)]
public class UserData : SaveData, ISaveName
{
    public UserData() : base(-1) { }

    public UserData(int newId, string newName) : base(newId)
    {
        Name = newName;
    }

    public string Name { get; set; }

    public void SetAsDefault()
    {
        Database.SettingsData.DefaultUserId = ID;
        PlayerPrefs.SetInt(SaveSystem.DefaultUserKey, ID);
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<UserData>();
    }
}
