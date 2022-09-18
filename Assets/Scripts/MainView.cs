using System.Collections.Generic;
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

    private void Start()
    {
        SpendThriftUtils.SetConsistentFontSize(new List<Component> {categoryButton, spendButton, usersButton});
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
