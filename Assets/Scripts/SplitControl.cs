using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SplitControl : ControlItem<SpendData.SplitShare>
{
    public UnityAction<SpendData.SplitShare> removeSplit;
    
    [SerializeField] private DictionaryDropdown userDropdown;
    [SerializeField] private TMP_InputField liability, payment;
    [SerializeField] private Button removeUser;

    private float spendAmount;

    private void Awake()
    {
        liability.contentType = payment.contentType = TMP_InputField.ContentType.DecimalNumber;
    }

    public void RefreshSliders(float newSpendAmount)
    {
        spendAmount = newSpendAmount;
        
        liability.text = ClampedSpend(data.LiabilitySplit * spendAmount).ToString("C");
        payment.text = ClampedSpend(data.PaymentSplit * spendAmount).ToString("C");
    }

    private float ClampedSpend(float newAmount)
    {
        return Mathf.Clamp(newAmount, 0.0f, spendAmount);
    }
    
    protected override void Refresh()
    {
        var isSplit = removeSplit != null;
        
        userDropdown.InitializeDropdown(Database.userData/*, data.userId*/); // should hide the ones already in the list of splitShares
        userDropdown.ShowOptionById(data.userId);
        userDropdown.onValueChanged.AddListener(AssignUser);

        removeUser.onClick.AddListener(RemoveUser);
        removeUser.interactable = isSplit;
        
        liability.onEndEdit.AddListener(UpdateLiability);
        payment.onEndEdit.AddListener(UpdatePayment);
    }

    private void AssignUser(int id)
    {
        data.userId = userDropdown.optionId;
    }

    private void UpdateLiability(string liabilityText)
    {
        data.LiabilitySplit = ParseAndClamp(liabilityText);
    }

    private void UpdatePayment(string paymentText)
    {
        data.PaymentSplit = ParseAndClamp(paymentText);
    }

    private float ParseAndClamp(string newAmountText)
    {
        if (float.TryParse(newAmountText, NumberStyles.Currency, CultureInfo.CurrentCulture, out var newAmount))
            return ClampedSpend(newAmount) / spendAmount;

        return 0;
    }

    private void RemoveUser()
    {
        removeSplit?.Invoke(data);
    }

    private void OnDestroy()
    {
        userDropdown.onValueChanged.RemoveListener(_ => data.userId = userDropdown.optionId);
        removeUser.onClick.RemoveListener(RemoveUser);
    }
}
