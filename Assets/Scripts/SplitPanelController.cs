using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SplitPanelController : EditView<SpendData>, IRefreshControls
{
    [SerializeField] private Button blockerButton;
    [SerializeField] private TMP_InputField totalAmount;
    [SerializeField] private RectTransform controlList;
    [SerializeField] private SplitControl controlPrefab;
    [SerializeField] private NewSplitControl addControlPrefab;

    private NewSplitControl addControl;
    private readonly List<SplitControl> controls = new();

    protected override void OnAwake()
    {
        base.OnAwake();

        totalAmount.contentType = TMP_InputField.ContentType.DecimalNumber;
        totalAmount.onValueChanged.AddListener(UpdateTotalAmount);
        blockerButton.onClick.AddListener(CancelChanges);
    }

    protected override void OnShow()
    {
        base.OnShow();

        RefreshControls();
    }

    protected override void RefreshView()
    {
        totalAmount.SetTextWithoutNotify(saveData.amount.ToString(CultureInfo.CurrentCulture));
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();
        
        ViewManager.RefreshView(ViewType.EditSpend);
    }

    private void UpdateTotalAmount(string totalAmountText)
    {
        if (!float.TryParse(totalAmountText, NumberStyles.Currency, CultureInfo.CurrentCulture, out var newTotalAmount))
            return;

        var oldTotalAmount = saveData.amount;
        var amountChange = oldTotalAmount - newTotalAmount / oldTotalAmount;
        foreach (var split in saveData.splitShares)
        {
            split.LiabilitySplit *= amountChange;
            split.PaymentSplit *= amountChange;
        }

        saveData.amount = newTotalAmount;

        foreach (var control in controls)
        {
            control.RefreshSliders(saveData.amount);
        }
    }

    public void RefreshControls()
    {
        if (saveData == null)
            return;

        if (saveData.CanAddUser(out _))
        {
            if (!addControl)
            {
                addControl = Instantiate(addControlPrefab, controlList);
                addControl.SetData(saveData);
            }
        }
        else if (addControl)
        {
            Destroy(addControl.gameObject);
        }

        foreach (var control in controls)
        {
            Destroy(control.gameObject);
        }

        controls.Clear();
        GenerateControls();
        ViewManager.RefreshView(ViewType.EditSpend);
    }

    private void GenerateControls()
    {
        foreach (var item in saveData.splitShares)
        {
            var control = Instantiate(controlPrefab, controlList);
            control.removeSplit = saveData.splitShares.Count > 1 ? RemoveSplit : null;
            control.SetData(item);
            control.RefreshSliders(saveData.amount);
            controls.Add(control);
        }
    }

    private void RemoveSplit(SpendData.SplitShare splitShare)
    {
        if (saveData.splitShares.Contains(splitShare))
        {
            saveData.splitShares.Remove(splitShare);
            AdjustRemainingSplits(splitShare);
            saveData.Save();
            RefreshControls();
        }
    }

    private void AdjustRemainingSplits(SpendData.SplitShare splitShare)
    {
        var remainingSplitCount = saveData.splitShares.Count;
        var liability = splitShare.LiabilitySplit / saveData.splitShares.Count;
        var payment = splitShare.PaymentSplit / remainingSplitCount;

        foreach (var saveDataSplitShare in saveData.splitShares)
        {
            saveDataSplitShare.LiabilitySplit += liability;
            saveDataSplitShare.PaymentSplit += payment;
        }
    }

    private void OnDestroy()
    {
        totalAmount.onValueChanged.RemoveListener(UpdateTotalAmount);
        blockerButton.onClick.RemoveListener(CancelChanges);
    }

    public override ViewType GetViewType() => ViewType.EditSplitShares;
}
