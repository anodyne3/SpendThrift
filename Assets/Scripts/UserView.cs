using UnityEngine;
using UnityEngine.UI;

public class UserView : View
{
    [SerializeField] private Button addUserButton, editUserButton;

    protected override void OnAwake()
    {
        base.OnAwake();

        addUserButton.onClick.AddListener(AddUser);
        editUserButton.onClick.AddListener(EditUser);
    }

    public int GetFreeId()
    {
        return 1;
    }

    public int SelectUser()
    {
        return 0;
    }

    private void AddUser() { }

    private void EditUser() { }

    public override ViewType GetViewType()
    {
        return ViewType.User;
    }
}
