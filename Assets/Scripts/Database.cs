using System.Collections.Generic;

public static class Database
{
    public static List<CategoryData> CategoryData { get; private set; } = new();
    public static List<SpendData> SpendData { get; private set; } = new();
    public static List<UserData> UserData { get; private set; } = new();
    public static SettingsData SettingsData { get; private set; } = new();

    public static int UnassignedCategoryId => -2;

    public static void LoadData()
    {
        SpendData = SaveSystem.LoadData<SpendData>();
        UserData = SaveSystem.LoadData<UserData>();
        CategoryData = SaveSystem.LoadData<CategoryData>();
        SettingsData = SaveSystem.LoadSettings();

        TryClearUnassignedCategory();
    }

    public static void TryClearUnassignedCategory()
    {
        if (CategoryData.Find(x => x.ID == UnassignedCategoryId) is not { } unassignedCategory ||
            CheckForStrays(unassignedCategory) != 0)
            return;

        CategoryData.Remove(unassignedCategory);
        SaveSystem.SaveData<CategoryData>();
    }

    public static int CheckForStrays<T>(T saveData) where T : SaveData
    {
        var strays = new List<SaveData>();

        foreach (var category in CategoryData)
            if (category.ParentCategoryId == saveData.ID)
                strays.Add(category);

        foreach (var spend in SpendData)
            if (spend.CategoryId == saveData.ID)
                strays.Add(spend);

        if (strays.Count > 0 && !CategoryData.Exists(x => x.ID == UnassignedCategoryId))
            SetNewData(new CategoryData(UnassignedCategoryId, "Unassigned", -1));

        if (saveData.ID == UnassignedCategoryId)
            return strays.Count;

        foreach (var stray in strays)
        {
            switch (stray)
            {
                case CategoryData strayCategory:
                    strayCategory.ParentCategoryId = UnassignedCategoryId;

                    break;
                case SpendData straySpend:
                    straySpend.CategoryId = UnassignedCategoryId;
                    break;
            }

            stray.Save();
        }

        return strays.Count;
    }

    public static void AddNewDataWithFreeID<T>(this T saveData) where T : ISaveData
    {
        saveData.ID = saveData.GetFreeId();
        saveData.GetIDataList()?.Add(saveData);
        saveData.Save();
    }

    public static void SetNewData<T>(this T saveData) where T : ISaveData, new()
    {
        saveData.DeleteISaveData(saveData.ID);
        saveData.GetIDataList()?.Add(saveData);
        saveData.Save();
    }

    public static void DeleteISaveData<T>(this T saveData, int id) where T : ISaveData, new()
    {
        saveData.GetIDataList().Remove((T)GetISaveData<T>(id));
    }

    private static List<T> GetIDataList<T>(this T t) where T : ISaveData
    {
        switch (t)
        {
            case CategoryData _:
                return CategoryData as List<T>;
            case SpendData _:
                return SpendData as List<T>;
            case UserData _:
                return UserData as List<T>;
        }

        return null;
    }

    public static ISaveData GetISaveData<T>(int id) where T : ISaveData, new()
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
            case SpendDateRangeData _:
                return SettingsData.DateRangeData;
        }

        return t as SaveData;
    }

    public static T GetSaveData<T>(int id) where T : SaveData, new()
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

    private static CategoryData GetCategoryData(int id)
    {
        return CategoryData.Find(x => x.ID == id);
    }

    private static SpendData GetSpendData(int id)
    {
        return SpendData.Find(x => x.ID == id);
    }

    private static UserData GetUserData(int id)
    {
        return UserData.Find(x => x.ID == id);
    }

    public static int GetFreeId<T>(this T saveData) where T : ISaveData
    {
        var freeId = 0;
        var selectedDataSet = new List<T>(saveData.GetIDataList());

        selectedDataSet.Sort((x, y) => x.ID.CompareTo(y.ID));

        foreach (var data in selectedDataSet)
        {
            if (data.ID != freeId)
                return freeId;

            freeId++;
        }

        return freeId;
    }

    public static bool IsUniqueName<T>(this T saveData, string newName) where T : ISaveData
    {
        var saveDataList = saveData.GetIDataList();
        if (saveDataList == null)
            return true;

        foreach (var saveName in saveDataList)
            if (((ISaveName)saveName).Name == newName)
                return false;

        return true;
    }

    public static List<T> GetDataList<T>() where T : SaveData, new()
    {
        var t = new T();

        switch (t)
        {
            case CategoryData _:
                return CategoryData as List<T>;
            case SpendData _:
                return SpendData as List<T>;
            case UserData _:
                return UserData as List<T>;
        }

        return null;
    }
}