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
        usersButton.onClick.AddListener(() => ShowView(ViewType.User));
        spendButton.onClick.AddListener(ShowSpend);
        usersButton.onClick.AddListener(ShowUsers);
        categoryButton.onClick.AddListener(ShowCategory);
    }

    private void SwitchMode()
    {
        if (forecastMode)
            ViewManager.ShowView(ViewType.Summary);
        else
            ViewManager.ShowView(ViewType.Forecast);
            
        forecastMode = !forecastMode;
    }
    
    void ShowView(ViewType viewType)
    {
        switch(viewType)
        {
            case ViewType.User:
                ViewManager.ShowView(ViewType.User);
                break;
        }
    }

    private void ShowForecast()
    {
        ViewManager.ShowView(ViewType.Forecast);
    }

    private void ShowSpend()
    {
        ViewManager.ShowView(ViewType.Spend);
    }
    
    private void ShowUsers()
    {
        ViewManager.ShowView(ViewType.User);
    }
        
    private void ShowCategory()
    {
        ViewManager.ShowView(ViewType.Category);
    }
    
    public override ViewType GetViewType() => ViewType.Main;
}

public enum ViewType { Summary, User, Spend, Forecast, Category, Main }

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
