using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CalendarPanel : View
{
    private enum DynamicButtonsType { Days, Months, Years }

    [SerializeField] private Button monthButton, yearButton, earlierYearsButton, laterYearsButton;
    [SerializeField] private TextMeshProUGUI monthButtonLabel, yearButtonLabel;
    [SerializeField] private GridLayoutGroup dynamicButtonGroup;
    [SerializeField] private DynamicButton dynamicButtonPrefab;

    private DateTime startDate;
    private GenericPool<DynamicButton> buttonPool;
    private DynamicButtonsSetting currentButtonSettings;

    private DynamicButtonsSetting daysButtonsSettings, monthsButtonsSettings, yearsButtonsSettings;

    private void Awake()
    {
        daysButtonsSettings = ScriptableObject.CreateInstance<DynamicButtonsSetting>();
        daysButtonsSettings.SetNew(DynamicButtonsType.Days, 72, 72, 42, 7);
        monthsButtonsSettings.SetNew(DynamicButtonsType.Months, 167, 100, 12, 3);
        yearsButtonsSettings.SetNew(DynamicButtonsType.Years, 167, 100, 4, 20);

        buttonPool =
            new GenericPool<DynamicButton>(() => Instantiate(dynamicButtonPrefab, dynamicButtonGroup.transform));
    }

    private void RefreshDynamicButtons(DynamicButtonsSetting newButtonType)
    {
        if (currentButtonSettings == newButtonType)
            return;

        currentButtonSettings = newButtonType;

        dynamicButtonGroup.cellSize = currentButtonSettings.buttonSize;
        dynamicButtonGroup.constraintCount = currentButtonSettings.columnCount;

        foreach (var dynamicButton in buttonPool.ActiveItems)
            buttonPool.Release(dynamicButton);

        //todo need to set starting day, like monday
        //todo need names of the months
        //todo need to show current year in the middle of the range offered
        for (var i = 0; i < currentButtonSettings.buttonCount; i++)
        {
            var newButton = buttonPool.Get();

            switch (currentButtonSettings.buttonsType)
            {
                case DynamicButtonsType.Days:
                    newButton.SetData(startDate.Day + i);
                    break;
                case DynamicButtonsType.Months:
                    newButton.SetData(startDate.Month + i);
                    break;
                case DynamicButtonsType.Years:
                    newButton.SetData(startDate.Year + i);
                    break;
            }
        }
    }

    protected override ViewType GetViewType() => ViewType.Calendar;

    private class DynamicButtonsSetting : ScriptableObject
    {
        public void SetNew(DynamicButtonsType newButtonsType, int buttonSizeX, int buttonSizeY,
            int newButtonCount, int newColumnCount)
        {
            buttonsType = newButtonsType;
            buttonSize = new Vector2(buttonSizeX, buttonSizeY);
            buttonCount = newButtonCount;
            columnCount = newColumnCount;
        }

        public DynamicButtonsType buttonsType;
        public Vector2 buttonSize;
        public int buttonCount, columnCount;
    }
}