using TMPro;
using UnityEngine;

public class EditUserView : EditView<UserData>
{
    [SerializeField] private TMP_InputField userName;

    protected override void Awake()
    {
        base.Awake();

        userName.contentType = TMP_InputField.ContentType.Name;
        userName.onValueChanged.AddListener(TestNameChange);

        dataTypeName = "Spender";
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        userName.onValueChanged.RemoveListener(TestNameChange);
    }

    protected override void RefreshView()
    {
        RefreshName(SaveData, userName);
        RefreshAlertMessage(itemToolOptions > 0 && itemToolOptions != ItemToolOptions.Edit);

        confirmChangesButton.interactable = itemToolOptions is ItemToolOptions.Delete or ItemToolOptions.Edit;
    }

    protected override void ConfirmChanges()
    {
        base.ConfirmChanges();

        switch (itemToolOptions)
        {
            case ItemToolOptions.Default:
                return;
            case ItemToolOptions.Delete:
                if (Database.SettingsData.DefaultUserId == SaveData?.ID)
                    Database.SettingsData.DefaultUserId = 0;

                DeleteItem();
                break;
            case ItemToolOptions.Duplicate:
                DuplicateItem(new UserData(SaveData.GetFreeId(), userName.text));
                break;
            default:
            case ItemToolOptions.Edit:
                SaveData.Name = userName.text;
                SaveData.Save();
                break;
        }

        ViewManager.RefreshView(ViewType.User);
    }


    protected override ViewType GetViewType() => ViewType.EditUser;
}
