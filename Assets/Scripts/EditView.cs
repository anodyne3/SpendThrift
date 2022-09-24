using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IEditView
{
    void Show(int[] contextId);
}

public abstract class EditView<T> : View, IEditView where T : ISaveData, new()
{
    [SerializeField] private Button cancelChangesButton;
    [SerializeField] protected Button confirmChangesButton;
    [SerializeField] protected TextMeshProUGUI alertText;

    protected T saveData { get; set; }

    protected ItemToolOptions ItemToolOptions;

    protected override void OnAwake()
    {
        base.OnAwake();

        cancelChangesButton.onClick.AddListener(CancelChanges);
        confirmChangesButton.onClick.AddListener(ConfirmChanges);
    }

    protected abstract void RefreshView();

    protected virtual void ConfirmChanges()
    {
        Hide();
    }

    protected virtual void CancelChanges()
    {
        Hide();
    }

    protected virtual void DeleteItem()
    {
        Database.DeleteSaveData<T>(context[0]);
        saveData.Save();
    }

    protected virtual void DuplicateItem(SaveData newSaveData)
    {
        Database.SetNewData(newSaveData);
    }

    protected override void OnShow()
    {
        base.OnShow();

        ProcessContext();
    }

    private void ProcessContext()
    {
        if (context == null)
        {
            saveData = default;
            context ??= new[] {Database.GetFreeId<T>(), 0};
            ItemToolOptions = 0;
        }
        else
        {
            saveData = (T) Database.GetISaveData<T>(context[0]);
            ItemToolOptions = (ItemToolOptions) context[1];
        }

        RefreshView();
    }
}
