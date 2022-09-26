using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IEditView
{
    void Show(int[] contextId);
}

public abstract class EditView<T> : View, IEditView where T : ISaveData, new()
{
    protected T SaveData { get; private set; }

    [SerializeField] private Button cancelChangesButton;
    [SerializeField] protected Button confirmChangesButton;
    [SerializeField] protected TextMeshProUGUI alertText;
    [SerializeField] protected string dataTypeName;

    private string DeleteMessage => $"Are you sure you wish to permanently remove this {dataTypeName}?";
    private string UniqueNameMessage => $"Please select a unique name for the new {dataTypeName}.";

    protected ItemToolOptions itemToolOptions;

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
        if (itemToolOptions == 0)
            DeleteItem();

        Hide();
    }

    protected virtual void ConfirmChanges()
    {
        Hide();
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
            SaveData = new T();
            SaveData.AddNewDataWithFreeID();
            context ??= new[] {SaveData.ID, 0};
            itemToolOptions = 0;
        }
        else
        {
            SaveData = (T) Database.GetISaveData<T>(context[0]);
            itemToolOptions = (ItemToolOptions) context[1];
        }

        RefreshView();
    }

    protected abstract void RefreshView();

    protected void RefreshName<TU>(TU saveData, TMP_InputField nameText) where TU : ISaveName
    {
        nameText.text = saveData?.Name;
        nameText.interactable = itemToolOptions != ItemToolOptions.Delete;
    }

    protected void RefreshAlertMessage(bool showAlert, string alertMessage = default)
    {
        alertText.enabled = showAlert;
        alertText.text = !string.IsNullOrEmpty(alertMessage)
            ? alertMessage
            : itemToolOptions == ItemToolOptions.Delete
                ? DeleteMessage
                : UniqueNameMessage;
    }

    protected void TestNameChange(string newName)
    {
        if (SaveData is not ISaveName saveName)
            return;

        var isEmpty = string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName);
        var isValid = itemToolOptions == ItemToolOptions.Edit && newName == saveName.Name ||
                      SaveData.IsUniqueName(newName);

        confirmChangesButton.interactable = !isEmpty && isValid;
        alertText.enabled = !isValid;
    }

    protected void DeleteItem()
    {
        SaveData.DeleteISaveData(context[0]);
        SaveData.Save();
    }

    protected void DuplicateItem(T newSaveData)
    {
        newSaveData.SetNewData();
    }
}
