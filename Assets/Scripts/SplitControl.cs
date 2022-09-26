using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SplitControl : ControlItem<SplitShare>
{
    public UnityAction<SplitShare> removeSplit;

    [SerializeField] private DictionaryDropdown userDropdown;
    [SerializeField] private TMP_InputField liability, payment;
    [SerializeField] private Button removeUser;

    private float spendAmount;

    private void Awake()
    {
        liability.contentType = payment.contentType = TMP_InputField.ContentType.DecimalNumber;

        userDropdown.onValueChanged.AddListener(AssignUser);
        liability.onEndEdit.AddListener(UpdateLiability);
        payment.onEndEdit.AddListener(UpdatePayment);
        removeUser.onClick.AddListener(RemoveUser);
    }

    private void OnDestroy()
    {
        userDropdown.onValueChanged.RemoveListener(AssignUser);
        liability.onEndEdit.RemoveListener(UpdateLiability);
        payment.onEndEdit.RemoveListener(UpdatePayment);
        removeUser.onClick.RemoveListener(RemoveUser);
    }

    private void AssignUser(int id)
    {
        // if (Data.UserId exists in the splitShares) swap them

        Data.UserId = userDropdown.OptionId;
    }

    private void UpdateLiability(string liabilityText)
    {
        Data.LiabilitySplit = ParseAndClamp(liabilityText);
        liability.SetTextWithoutNotify((Data.LiabilitySplit * spendAmount).ToString("C", CultureInfo.CurrentCulture));
    }

    private void UpdatePayment(string paymentText)
    {
        Data.PaymentSplit = ParseAndClamp(paymentText);
        payment.SetTextWithoutNotify((Data.PaymentSplit * spendAmount).ToString("C", CultureInfo.CurrentCulture));
    }

    private void RemoveUser()
    {
        removeSplit?.Invoke(Data);
    }

    private float ParseAndClamp(string newAmountText)
    {
        if (float.TryParse(newAmountText, NumberStyles.Currency, CultureInfo.CurrentCulture, out var newAmount))
            return ClampedSpend(newAmount) / spendAmount;

        return 0;
    }

    private float ClampedSpend(float newAmount)
    {
        return Mathf.Clamp(newAmount, 0.0f, spendAmount);
    }

    public void RefreshSplits(float newSpendAmount)
    {
        spendAmount = newSpendAmount;

        liability.SetTextWithoutNotify( /*ClampedSpend*/
            (Data.LiabilitySplit * spendAmount).ToString("C", CultureInfo.CurrentCulture));

        payment.SetTextWithoutNotify( /*ClampedSpend*/
            (Data.PaymentSplit * spendAmount).ToString("C", CultureInfo.CurrentCulture));
    }

    protected override void Refresh()
    {
        var isSplit = removeSplit != null;

        userDropdown.InitializeDropdown(Database.UserData);
        userDropdown.ShowOptionById(Data.UserId);

        removeUser.interactable = isSplit;
    }
}
