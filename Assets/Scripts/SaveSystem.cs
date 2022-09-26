using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string DataPath = Application.persistentDataPath;

    private static readonly string SpendPath = $"{DataPath}/Spend.json";
    private static readonly string UserPath = $"{DataPath}/User.json";
    private static readonly string CategoryPath = $"{DataPath}/Category.json";

    public const string DefaultCategoryKey = "defaultCategoryKey";
    public const string DefaultUserKey = "defaultUserKey";

    public static SettingsData LoadSettings()
    {
        return new SettingsData
        {
            DefaultUserId = PlayerPrefs.GetInt(DefaultUserKey, 0),
            DefaultCategoryId = PlayerPrefs.GetInt(DefaultCategoryKey, 0)
        };
    }

    public static List<T> LoadData<T>() where T : SaveData, new()
    {
        var path = GetPath<T>();

        var saveData = new List<T>();

        if (!File.Exists(path))
            return saveData;

        try
        {
            var json = File.ReadAllText(path);
            saveData = JsonConvert.DeserializeObject<List<T>>(json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return saveData ?? new List<T>();
    }

    public static void SaveData<T>() where T : SaveData, new()
    {
        var data = Database.GetDataList<T>();
        var json = JsonConvert.SerializeObject(data);
        File.WriteAllText(GetPath<T>(), json);
    }

    private static string GetPath<T>() where T : SaveData, new()
    {
        var path = "";

        var t = new T();

        switch (t)
        {
            case CategoryData _:
                path = CategoryPath;
                break;
            case SpendData _:
                path = SpendPath;
                break;
            case UserData _:
                path = UserPath;
                break;
        }

        return path;
    }
}
