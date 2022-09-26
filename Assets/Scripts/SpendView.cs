using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpendView : SubView<SpendData>
{
    private enum DateRangeType { Day, Week, Month, Quarter, Year, All }

    [SerializeField] private Button previousButton, nextButton;
    [SerializeField] private TMP_Dropdown periodDropdown;

    private DateRange dateRange;
    private DateRangeType currentDateRangeType;

    protected override void Awake()
    {
        SetDateRange(DateTime.UtcNow, DateRangeType.Month);
        periodDropdown.AddOptions(new List<string>(Enum.GetNames(typeof(DateRangeType))));

        base.Awake();

        previousButton.onClick.AddListener(PreviousPeriod);
        nextButton.onClick.AddListener(NextPeriod);
        periodDropdown.onValueChanged.AddListener(SelectPeriod);
    }

    private void OnDestroy()
    {
        previousButton.onClick.RemoveListener(PreviousPeriod);
        nextButton.onClick.RemoveListener(NextPeriod);
        periodDropdown.onValueChanged.RemoveListener(SelectPeriod);
    }

    private void PreviousPeriod()
    {
        dateRange.SubtractTime(DateRangeType.Month);
        GenerateControls();
    }

    private void SelectPeriod(int periodChange)
    {
        currentDateRangeType = (DateRangeType) periodChange;
        SetDateRange(dateRange.StartDate, currentDateRangeType);
    }

    private void NextPeriod()
    {
        dateRange.AddTime(DateRangeType.Month);
        GenerateControls();
    }

    protected override void GenerateControls()
    {
        controlData = Database.SpendData.FindAll(x => IsWithinDateRange(x.Date));
        controlData.Sort((x, y) => x.Date.CompareTo(y.Date));

        base.GenerateControls();
    }

    private bool IsWithinDateRange(DateTime testDate)
    {
        return dateRange.StartDate <= testDate && testDate <= dateRange.EndDate;
    }

    private void SetDateRange(DateTime startDate, DateRangeType dateRangeType, bool normalPeriod = true)
    {
        currentDateRangeType = dateRangeType;
        DateTime startDateTime = DateTime.MinValue, endDateTime = DateTime.MaxValue;

        switch (dateRangeType)
        {
            case DateRangeType.Day:
                startDateTime = endDateTime = startDate;
                break;
            case DateRangeType.Week:
                startDateTime = normalPeriod ? GetDayOfTheWeek(startDate, DayOfWeek.Monday) : startDate;
                endDateTime = startDateTime.AddDays(7);
                break;
            case DateRangeType.Month:
                startDateTime = normalPeriod ? new DateTime(startDate.Year, startDate.Month, 1) : startDate;
                endDateTime = startDateTime.AddMonths(1);
                break;
            case DateRangeType.Quarter:
                startDateTime = normalPeriod ? new DateTime(startDate.Year, startDate.Month, 1) : startDate;
                endDateTime = startDateTime.AddMonths(3);
                break;
            case DateRangeType.Year:
                startDateTime = normalPeriod ? new DateTime(startDate.Year, 1, 1) : startDate;
                endDateTime = startDateTime.AddYears(1);
                break;
            default:
            case DateRangeType.All:
                break;
        }

        dateRange = new DateRange(startDateTime, endDateTime);

        DateTime GetDayOfTheWeek(DateTime dateTime, DayOfWeek dayOfWeek)
        {
            while (dateTime.DayOfWeek != dayOfWeek)
            {
                dateTime = dateTime.AddDays(-1);
            }

            return dateTime;
        }
    }

    protected override ViewType GetViewType() => ViewType.Spend;

    private class DateRange
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
}
