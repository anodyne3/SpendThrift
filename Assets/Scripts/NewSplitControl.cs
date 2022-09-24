public class NewSplitControl : NewControl<SpendData>
{
    protected override void ShowNewItemPanel()
    {
        if (data.CanAddUser(out var addedUser))
        {
            data.AddSplitShare(addedUser.id);
        }

        ViewManager.RefreshView(ViewType.EditSplitShares);
    }
}
