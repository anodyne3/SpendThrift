using TMPro;
using UnityEngine;

public class DynamicButton : ControlItem<int>, IToggleActive
{
    [SerializeField] private TextMeshProUGUI labelText;

    protected override void Refresh()
    {
        labelText.text = Data.ToString();
    }

    public void ToggleActive(bool value)
    {
    }
}