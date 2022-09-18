using UnityEngine;
using UnityEngine.UI;

public class SpendView : SubView<SpendData>
{
    [SerializeField] private Button previousButton, nextButton, periodButton;

    protected override void OnAwake()
    {
        base.OnAwake();

        previousButton.onClick.AddListener(PreviousPeriod);
        periodButton.onClick.AddListener(SelectPeriod);
        nextButton.onClick.AddListener(NextPeriod);
    }

    private void PreviousPeriod() { }
    private void SelectPeriod() { }
    private void NextPeriod() { }

    public override ViewType GetViewType() => ViewType.Spend;
}
