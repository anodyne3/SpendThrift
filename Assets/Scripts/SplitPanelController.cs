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
    private SpendData tempData;

    protected override void Awake()
    {
        base.Awake();

        totalAmount.contentType = TMP_InputField.ContentType.DecimalNumber;

        totalAmount.onEndEdit.AddListener(UpdateTotalAmount);
        blockerButton.onClick.AddListener(CancelChanges);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        totalAmount.onEndEdit.RemoveListener(UpdateTotalAmount);
        blockerButton.onClick.RemoveListener(CancelChanges);
    }

    private void UpdateTotalAmount(string totalAmountText)
    {
        if (!float.TryParse(totalAmountText, NumberStyles.Currency, CultureInfo.CurrentCulture, out var newTotalAmount))
            return;

        tempData.Amount = newTotalAmount;
        totalAmount.SetTextWithoutNotify(newTotalAmount.ToString("C", CultureInfo.CurrentCulture));

        foreach (var control in controls)
        {
            control.RefreshSplits(tempData.Amount);
        }
    }

    protected override void OnShow()
    {
        base.OnShow();

        tempData = SaveData;

        RefreshControls();
    }

    public void RefreshControls()
    {
        if (tempData == null)
            return;

        if (tempData.CanAddUser(out _))
        {
            if (!addControl)
            {
                addControl = Instantiate(addControlPrefab, controlList);
                addControl.AddSplitShare = AddSplit;
                addControl.SetData(tempData);
            }
        }
        else if (addControl)
        {
            Destroy(addControl.gameObject);
        }

        foreach (var control in controls)
            Destroy(control.gameObject);

        controls.Clear();
        GenerateControls();
        ViewManager.RefreshView(ViewType.EditSpend);
    }

    private void GenerateControls()
    {
        foreach (var item in tempData.SplitShares)
        {
            var control = Instantiate(controlPrefab, controlList);
            control.removeSplit = tempData.SplitShares.Count > 1 ? RemoveSplit : null;
            control.SetData(item);
            control.RefreshSplits(tempData.Amount);
            controls.Add(control);
        }
    }

    private void AddSplit()
    {
        if (SaveData.CanAddUser(out var addedUser))
            tempData.SplitShares.Add(new SplitShare(addedUser.ID)
                {LiabilitySplit = UpdateLiabilitySplits(), PaymentSplit = 0f});

        ViewManager.RefreshView(ViewType.EditSplitShares);

        float UpdateLiabilitySplits()
        {
            var newLiability = 0f;

            foreach (var splitShare in tempData.SplitShares)
            {
                var change = splitShare.LiabilitySplit / (tempData.SplitShares.Count + 1);
                newLiability += change;
                splitShare.LiabilitySplit -= change;
            }

            return newLiability;
        }
    }

    private void RemoveSplit(SplitShare splitShare)
    {
        if (tempData.SplitShares.Contains(splitShare))
        {
            tempData.SplitShares.Remove(splitShare);
            AdjustRemainingSplits();
            tempData.Save();
            RefreshControls();
        }

        void AdjustRemainingSplits()
        {
            var remainingSplitCount = tempData.SplitShares.Count;
            var liability = splitShare.LiabilitySplit / tempData.SplitShares.Count;
            var payment = splitShare.PaymentSplit / remainingSplitCount;

            foreach (var saveDataSplitShare in tempData.SplitShares)
            {
                saveDataSplitShare.LiabilitySplit += liability;
                saveDataSplitShare.PaymentSplit += payment;
            }
        }
    }

    //todo theres a bug in here where savedata becomes null
    protected override void RefreshView()
    {
        totalAmount.SetTextWithoutNotify(SaveData.Amount.ToString("C", CultureInfo.CurrentCulture));
    }

    protected override void ConfirmChanges()
    {
        tempData.SetNewData();

        base.ConfirmChanges();

        ViewManager.RefreshView(ViewType.EditSpend);
    }

    protected override ViewType GetViewType() => ViewType.EditSplitShares;
}
