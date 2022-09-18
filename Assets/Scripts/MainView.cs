using System;
using UnityEngine;
using UnityEngine.UI;

public class MainView : View
{
    [SerializeField] private Button modeButton, spendButton, usersButton, categoryButton;

    private bool forecastMode;

    private void Awake()
    {
        modeButton.onClick.AddListener(SwitchMode);
        usersButton.onClick.AddListener(() => ViewManager.ShowView(ViewType.User));
        spendButton.onClick.AddListener(() => ViewManager.ShowView(ViewType.Spend));
        categoryButton.onClick.AddListener(() => ViewManager.ShowView(ViewType.Category));
        ViewManager.ShowView(ViewType.Main);
    }

    private void SwitchMode()
    {
        ViewManager.ShowView(forecastMode ? ViewType.Summary : ViewType.Forecast);

        forecastMode = !forecastMode;
    }

    private void ShowForecast()
    {
        ViewManager.ShowView(ViewType.Forecast);
    }

    public override ViewType GetViewType() => ViewType.Main;
}

[Serializable]
public class Spend
{
    private int id;
    private DateTime date;
    private float amount;
    private int spendCategoryId;
    private int[] splitIds;
    private string description;
    private bool isRecurring;
}

[Serializable]
public class SplitShare
{
    private int id;
    private int userId;
    private float split;
}

[Serializable]
public class User
{
    private int id;
    private string name;
}

[Serializable]
public class SpendCategory
{
    private int id;
    private string categoryName;
    private int parentCategory;
}
