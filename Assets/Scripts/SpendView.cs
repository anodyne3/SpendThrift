using System;
using UnityEngine;
using UnityEngine.UI;

public class SpendView : SubView<SpendData>
{
    [SerializeField] private Button previousButton, nextButton, dateRangeButton;

    private DateRange dateRange;
    private DateRangeType currentDateRangeType;

    protected override void Awake()
    {
        SetDateRange(DateTime.UtcNow, DateRangeType.Month);

        base.Awake();

        previousButton.onClick.AddListener(PreviousPeriod);
        nextButton.onClick.AddListener(NextPeriod);
        dateRangeButton.onClick.AddListener(ShowDateRange);
    }

    private void OnDestroy()
    {
        previousButton.onClick.RemoveListener(PreviousPeriod);
        nextButton.onClick.RemoveListener(NextPeriod);
    }

    private void PreviousPeriod()
    {
        dateRange.SubtractTime(DateRangeType.Month);

        base.RefreshControls();

        GenerateControls();
    }

    private void NextPeriod()
    {
        dateRange.AddTime(DateRangeType.Month);

        base.RefreshControls();

        GenerateControls();
    }

    private void ShowDateRange()
    {
        ViewManager.ShowView(ViewType.DateRange);
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

    public void SetDateRange(DateTime startDate, DateRangeType dateRangeType, bool normalPeriod = true)
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
}