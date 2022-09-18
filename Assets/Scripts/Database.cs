﻿using System.Collections.Generic;

public static class Database
{
    public static List<CategoryData> categoryData { get; private set; } = new();
    public static List<SpendData> spendData { get; private set; } = new();
    public static List<UserData> userData { get; private set; } = new();
    public static SettingsData settingsData { get; private set; } = new();

    public static UserData DefaultUser => GetISaveData<UserData>(settingsData.defaultUserId);
    public static CategoryData DefaultCategory => GetISaveData<CategoryData>(settingsData.defaultCategoryId);

    public static void LoadData()
    {
        spendData = SaveSystem.LoadData<SpendData>();
        userData = SaveSystem.LoadData<UserData>();
        categoryData = SaveSystem.LoadData<CategoryData>();
        settingsData = SaveSystem.LoadSettings();
    }

    public static T GetISaveData<T>(int id) where T : SaveData, new()
    {
        var t = new T();

        switch (t)
        {
            case SpendData _:
                return GetSpendData(id) as T;
            case UserData _:
                return GetUserData(id) as T;
            case CategoryData _:
                return GetCategoryData(id) as T;
        }

        return t;
    }

    public static ISaveData GetSaveData<T>(int id) where T : ISaveData, new()
    {
        var t = new T();

        switch (t)
        {
            case CategoryData _:
                return GetCategoryData(id);
            case SpendData _:
                return GetSpendData(id);
            case UserData _:
                return GetUserData(id);
        }

        return t as SaveData;
    }

    public static List<T> GetData<T>() where T : SaveData, new()
    {
        var t = new T();

        switch (t)
        {
            case CategoryData _:
                return categoryData as List<T>;
            case SpendData _:
                return spendData as List<T>;
            case UserData _:
                return userData as List<T>;
        }

        return null;
    }

    public static void DeleteSaveData<T>(int id) where T : ISaveData, new()
    {
        var t = new T();

        switch (t)
        {
            case CategoryData _:
                categoryData.Remove(GetCategoryData(id));
                SaveSystem.SaveData<CategoryData>();
                break;
            case SpendData _:
                spendData.Remove(GetSpendData(id));
                SaveSystem.SaveData<SpendData>();
                break;
            case UserData _:
                userData.Remove(GetUserData(id));
                SaveSystem.SaveData<UserData>();
                break;
        }
    }

    public static CategoryData GetCategoryData(int id)
    {
        return categoryData.Find(x => x.id == id);
    }

    private static SpendData GetSpendData(int id)
    {
        return spendData.Find(x => x.id == id);
    }

    private static UserData GetUserData(int id)
    {
        return userData.Find(x => x.id == id);
    }

    public static void SetNewData<T>(int id) where T : new()
    {
        var newSaveData = new T();
        ISaveData oldData;

        switch (newSaveData)
        {
            case CategoryData newData:
                oldData = categoryData.Find(x => x.id == newData.id);
                categoryData.Remove((CategoryData) oldData);
                categoryData.Add(newData);
                break;
            case SpendData newData:
                oldData = spendData.Find(x => x.id == newData.id);
                spendData.Remove((SpendData) oldData);
                spendData.Add(newData);
                break;
            case UserData newData:
                oldData = userData.Find(x => x.id == newData.id);
                userData.Remove((UserData) oldData);
                userData.Add(newData);
                break;
        }
    }

    public static void SetNewData(SaveData saveData)
    {
        ISaveData oldData;

        switch (saveData)
        {
            case CategoryData newData:
                oldData = categoryData.Find(x => x.id == newData.id);
                categoryData.Remove((CategoryData) oldData);
                categoryData.Add(newData);
                SaveSystem.SaveData<CategoryData>();
                break;
            case SpendData newData:
                oldData = spendData.Find(x => x.id == newData.id);
                spendData.Remove((SpendData) oldData);
                spendData.Add(newData);
                SaveSystem.SaveData<SpendData>();
                break;
            case UserData newData:
                oldData = userData.Find(x => x.id == newData.id);
                userData.Remove((UserData) oldData);
                userData.Add(newData);
                SaveSystem.SaveData<UserData>();
                break;
        }
    }

    public static int GetFreeId<T>() where T : ISaveData, new()
    {
        var freeId = 0;
        List<ISaveData> selectedDataSet;

        var t = new T();

        switch (t)
        {
            case CategoryData _:
                selectedDataSet = new List<ISaveData>(categoryData);
                break;
            case SpendData _:
                selectedDataSet = new List<ISaveData>(spendData);
                break;
            case UserData _:
                selectedDataSet = new List<ISaveData>(userData);
                break;
            default:
                return freeId;
        }

        selectedDataSet.Sort((x, y) => x.id.CompareTo(y.id));

        foreach (var data in selectedDataSet)
        {
            if (data.id != freeId)
                return freeId;

            freeId++;
        }

        return freeId;
    }

    public static bool IsUniqueName<T>(string newName) where T : SaveData, new()
    {
        var saveData = GetData<T>();
        if (saveData == null)
            return true;

        foreach (var category in saveData)
        {
            if (((ISaveName) category).name == newName)
                return false;
        }

        return true;
    }
}
