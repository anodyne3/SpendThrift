using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button summaryViewButton, forecastViewButton, addSpendButton;

    private void Awake()
    {
        summaryViewButton.onClick.AddListener(ShowSummary);
        forecastViewButton.onClick.AddListener(ShowForecast);
        addSpendButton.onClick.AddListener(AddSpend);
    }

    private void ShowSummary()
    {
        ViewManager.ShowView(ViewType.Summary);
    }

    private void ShowForecast()
    {
        ViewManager.ShowView(ViewType.Forecast);
    }

    private void AddSpend()
    {
        ViewManager.ShowView(ViewType.Spend);
    }
}

public enum ViewType { Summary, User, Spend, Forecast, Category }

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
    private string categoryName;
    private int id;
    private int parentCategory;
}
