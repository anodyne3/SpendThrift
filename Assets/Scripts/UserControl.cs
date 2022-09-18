using TMPro;
using UnityEngine;

public class UserControl : ToolsControlItem<UserData>
{
    [SerializeField] private TextMeshProUGUI nameText;

    protected override void Refresh()
    {
        base.Refresh();
        
        nameText.text = data.name;
    }
}
