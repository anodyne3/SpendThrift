using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
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

    //todo - only initialize on change to userData - abstraction? 
    public static Dictionary<int, int> InitializeUserDropdown(TMP_Dropdown userDropdown, int userId)
    {
        userDropdown.interactable = true;
        var usersToDropdownIndex = new Dictionary<int, int>();
        userDropdown.options.Clear();

        foreach (var user in Database.userData)
        {
            usersToDropdownIndex.Add(userDropdown.options.Count, user.id);
            userDropdown.options.Add(new TMP_Dropdown.OptionData {text = user.name});
        }

        userDropdown.value = 0;
        foreach (var kvp in usersToDropdownIndex)
            if (kvp.Value == userId)
                userDropdown.value = kvp.Key;

        userDropdown.RefreshShownValue();

        return usersToDropdownIndex;
    }
}
