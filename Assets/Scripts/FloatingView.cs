using UnityEngine;
using UnityEngine.UI;

public abstract class FloatingView : View
{
    [SerializeField] private Button cancelChangesButton;
    [SerializeField] protected Button confirmChangesButton;

    protected virtual void Awake()
    {
        cancelChangesButton.onClick.AddListener(CancelChanges);
        confirmChangesButton.onClick.AddListener(ConfirmChanges);
    }

    protected virtual void OnDestroy()
    {
        cancelChangesButton.onClick.RemoveListener(CancelChanges);
        confirmChangesButton.onClick.RemoveListener(ConfirmChanges);
        CancelChanges();
    }

    protected virtual void CancelChanges()
    {
        Hide();
    }

    protected virtual void ConfirmChanges()
    {
        Hide();
    }

    protected override void OnShow()
    {
        RefreshView();
    }

    protected abstract void RefreshView();
}