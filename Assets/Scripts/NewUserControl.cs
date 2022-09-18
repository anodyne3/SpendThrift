public class NewUserControl : NewControl<UserData>
{
    protected override void ShowNewItemPanel()
    {
        ViewManager.ShowView(ViewType.EditUser);
    }
}
