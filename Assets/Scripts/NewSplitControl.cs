using UnityEngine.Events;

public class NewSplitControl : NewControl<SpendData>
{
    public UnityAction AddSplitShare { get; set; }

    protected override void ShowNewItemPanel() => AddSplitShare.Invoke();
}
