using TMPro;
using UnityEngine;

public class EditUserView : EditView<UserData>
{
    [SerializeField] private TMP_InputField userName;

    private void TestNameChange(string newName)
    {
        var isEmpty = string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName);
        var isValid = ItemToolOptions == ItemToolOptions.Edit && newName == saveData?.name ||
                      Database.IsUniqueName<UserData>(newName);

        confirmChangesButton.interactable = !isEmpty && isValid;
        alertText.enabled = !isValid;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        userName.contentType = TMP_InputField.ContentType.Name;
    }

    protected override void RefreshView()
    {
        userName.text = saveData?.name; //todo - DRY - EditCatView 

        userName.interactable = ItemToolOptions != ItemToolOptions.Delete;
        alertText.enabled = ItemToolOptions > 0 && ItemToolOptions != ItemToolOptions.Edit;

        switch (ItemToolOptions)
        {
            case ItemToolOptions.Delete:
                alertText.text = "Are you sure you wish to permanently remove this User?";
                confirmChangesButton.interactable = true;
                break;
            default:
                alertText.text = "Please select a unique name for the new User.";
                userName.onValueChanged.AddListener(TestNameChange);
                confirmChangesButton.interactable = ItemToolOptions == ItemToolOptions.Edit;
                break;
        }
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (ItemToolOptions)
        {
            case ItemToolOptions.Edit:
                saveData.name = userName.text;
                saveData.Save();
                break;
            case ItemToolOptions.Delete:
                DeleteItem();
                break;
            case ItemToolOptions.Duplicate:
                DuplicateItem(new UserData(Database.GetFreeId<UserData>(), userName.text));
                break;
            default:
                Database.SetNewData(new UserData(context[0], userName.text));
                break;
        }
        
        ViewManager.RefreshView(ViewType.User);
    }

    protected override void OnHide()
    {
        base.OnHide();

        userName.onValueChanged.RemoveListener(TestNameChange);
    }

    public override ViewType GetViewType() => ViewType.EditUser;
}
