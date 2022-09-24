using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemToolDropdown : TMP_Dropdown
{
    public UnityEvent<ItemToolOptions> optionSelected;

    private readonly Dictionary<int, ItemToolOptions> itemOptionsToDropdownIndex = new();

    public void InitializeDropdown(ItemToolOptions dropdownOptions)
    {
        options = new List<OptionData>();
        foreach (Enum itemTool in Enum.GetValues(typeof(ItemToolOptions)))
        {
            if (!dropdownOptions.HasFlag(itemTool))
                continue;

            itemOptionsToDropdownIndex.Add(itemOptionsToDropdownIndex.Count, (ItemToolOptions) itemTool);
            var optionData = new OptionData {text = itemTool.ToString()};
            options.Add(optionData);
        }

        SetValueWithoutNotify(-1);
        onValueChanged.AddListener(OptionSelected);
    }

    private void OptionSelected(int optionIndex)
    {
        optionSelected.Invoke(itemOptionsToDropdownIndex[optionIndex]);
    }

    #region Overrides

    protected override void DestroyDropdownList(GameObject dropdownList)
    {
        base.DestroyDropdownList(dropdownList);
        SetValueWithoutNotify(-1);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        onValueChanged.RemoveListener(OptionSelected);
    }

    #endregion
}
