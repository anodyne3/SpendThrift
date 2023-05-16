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

    private const string DateRangeTypeKey = "dateRangeTypeKey";
    private const string DateRangeStartKey = "dateRangeStartKey";
    private const string DateRangeEndKey = "dateRangeEndKey";

    public static SettingsData LoadSettings()
    {
        var dateRangeType = (DateRangeType)PlayerPrefs.GetInt(DateRangeTypeKey, 2);
        var newStartDate = DateTime.TryParse(PlayerPrefs.GetString(DateRangeStartKey), out var startDate)
            ? startDate
            : DateTime.UtcNow;

        var newDateRange = new SpendDateRangeData
        {
            DateRangeType = dateRangeType,
            DateRange = new DateRange(newStartDate,
                DateTime.TryParse(PlayerPrefs.GetString(DateRangeEndKey), out var endDate)
                    ? endDate
                    : GetEndTime())
        };

        return new SettingsData
        {
            DefaultUserId = PlayerPrefs.GetInt(DefaultUserKey, 0),
            DefaultCategoryId = PlayerPrefs.GetInt(DefaultCategoryKey, 0),
            DateRangeData = newDateRange
        };

        DateTime GetEndTime()
        {
            switch (dateRangeType)
            {
                case DateRangeType.Day:
                    return newStartDate.AddDays(1);
                case DateRangeType.Week:
                    return newStartDate.AddDays(7);
                case DateRangeType.Month:
                    return newStartDate.AddMonths(1);
                case DateRangeType.Quarter:
                    return newStartDate.AddMonths(3);
                case DateRangeType.Year:
                    return newStartDate.AddYears(1);
                default:
                    return DateTime.MaxValue;
            }
        }
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