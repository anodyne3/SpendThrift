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

    protected T saveData { get; private set; }

    protected ItemToolOptions itemToolOptions;

    protected override void OnAwake()
    {
        base.OnAwake();

        cancelChangesButton.onClick.AddListener(CancelChanges);
        confirmChangesButton.onClick.AddListener(ConfirmChanges);
    }

    protected abstract void RefreshView();

    protected virtual void ConfirmChanges() // todo - save here
    {
        context ??= new[] {Database.GetFreeId<T>(), 0};

        Hide();
    }

    protected virtual void CancelChanges()
    {
        Hide();
    }

    protected virtual void DeleteItem()
    {
        Database.DeleteSaveData<T>(context[0]);
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
            itemToolOptions = 0;
        }
        else
        {
            saveData = (T) Database.GetSaveData<T>(context[0]);
            itemToolOptions = (ItemToolOptions) context[1];
        }

        RefreshView();
    }
}
