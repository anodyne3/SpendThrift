using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum DateRangeType { Day, Week, Month, Quarter, Year, All, Custom }

public class DateRangePanel : FloatingView
{
    [SerializeField] private Button startDateButton, endDateButton;
    [SerializeField] private TMP_InputField startDateInput, endDateInput;
    [SerializeField] private TMP_Dropdown periodDropdown;

    protected override void Awake()
    {
        base.Awake();
        
        startDateButton.onClick.AddListener(ShowStartDateCalendar);
        endDateButton.onClick.AddListener(ShowEndDateCalendar);

        startDateInput.contentType = TMP_InputField.ContentType.DecimalNumber;
        endDateInput.contentType = TMP_InputField.ContentType.DecimalNumber;

        periodDropdown.AddOptions(new List<string>(Enum.GetNames(typeof(DateRangeType))));
        periodDropdown.onValueChanged.AddListener(SelectPeriod);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        startDateButton.onClick.RemoveListener(ShowStartDateCalendar);
        endDateButton.onClick.RemoveListener(ShowEndDateCalendar);
        startDateInput.onEndEdit.RemoveListener(ValidateStartDate);
        endDateInput.onEndEdit.RemoveListener(ValidateEndDate);
        periodDropdown.onValueChanged.RemoveListener(SelectPeriod);
    }

    public void RefreshControls()
    {
        periodDropdown.value = (int)Database.SettingsData.DateRangeData.DateRangeType;
        startDateInput.text = Database.SettingsData.DateRangeData.DateRange.StartDate.ToShortDateString();
        endDateInput.text = Database.SettingsData.DateRangeData.DateRange.EndDate.ToShortDateString();
    }

    private void ShowStartDateCalendar()
    {
    }

    private void ShowEndDateCalendar()
    {
    }

    private void ValidateStartDate(string dateText)
    {
    }

    private void ValidateEndDate(string dateText)
    {
    }

    private void SelectPeriod(int periodChange)
    {
        // SaveData.DateRangeType = (DateRangeType)periodChange;
    }

    protected override void RefreshView()
    {
        RefreshControls();
    }

    protected override ViewType GetViewType() => ViewType.DateRange;
}

public struct DateRange
{
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public DateRange(DateTime start, DateTime end)
    {
        StartDate = start;
        EndDate = end;
    }

    public void AddTime(DateRangeType dateRangeType)
    {
        ChangeTime(dateRangeType);
    }

    public void SubtractTime(DateRangeType dateRangeType)
    {
        ChangeTime(dateRangeType, false);
    }

    private void ChangeTime(DateRangeType dateRangeType, bool add = true)
    {
        switch (dateRangeType)
        {
            case DateRangeType.Day:
                StartDate = StartDate.AddDays(add ? 1 : -1);
                EndDate = EndDate.AddDays(add ? 1 : -1);
                break;
            case DateRangeType.Week:
                StartDate = StartDate.AddDays(add ? 1 : -7);
                EndDate = EndDate.AddDays(add ? 1 : -7);
                break;
            case DateRangeType.Month:
                StartDate = StartDate.AddMonths(add ? 1 : -1);
                EndDate = EndDate.AddMonths(add ? 1 : -1);
                break;
            case DateRangeType.Quarter:
                StartDate = StartDate.AddMonths(add ? 1 : -3);
                EndDate = EndDate.AddMonths(add ? 1 : -3);
                break;
            case DateRangeType.Year:
                StartDate = StartDate.AddYears(add ? 1 : -1);
                EndDate = EndDate.AddYears(add ? 1 : -1);
                break;
            default:
            case DateRangeType.All:
                StartDate = DateTime.MinValue;
                EndDate = DateTime.MaxValue;
                break;
        }
    }
}