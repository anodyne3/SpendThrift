using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

public static class SaveSystem
{
    private static readonly string DataPath = Application.persistentDataPath;
    private static readonly string SpendPath = $"{DataPath}/Spend.xml";
    private static readonly string UserPath = $"{DataPath}/User.xml";
    private static readonly string CategoryPath = $"{DataPath}/Category.xml";
    
    public const string DefaultCategoryKey = "defaultCategoryKey";
    public const string DefaultUserKey = "defaultUserKey";

    public static List<T> LoadData<T>() where T : SaveData, new()
    {
        var path = GetPath<T>();
        var saveFile = new XDocument();

        if (File.Exists(path))
        {
            saveFile = XDocument.Load(path);
        }

        var saveData = new List<T>();
        var root = saveFile.Root;

        if (root == null)
            return saveData;

        var t = new T();
        foreach (var element in root.Elements())
        {
            if (t.TryParse(element, out var data))
                saveData.Add((T) data);
        }

        return saveData;
    }

    public static SettingsData LoadSettings()
    {
        return new SettingsData
        {
            defaultUserId = PlayerPrefs.GetInt(DefaultUserKey, 0),
            defaultCategoryId = PlayerPrefs.GetInt(DefaultCategoryKey, 0)
        };
    }

    public static void SaveData<T>() where T : SaveData, new()
    {
        var saveFile = new XDocument();
        var data = Database.GetData<T>();

        using (var stream = saveFile.CreateWriter())
        {
            var xmlSerializer = new XmlSerializer(data.GetType(), new XmlRootAttribute("Root"));
            xmlSerializer.Serialize(stream, data);
        }

        saveFile.Save(GetPath<T>());
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
