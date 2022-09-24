using System.Collections.Generic;
using TMPro;

public class DictionaryDropdown : TMP_Dropdown
{
    public int optionId => dataToDropdownIndex[value];

    private Dictionary<int, int> dataToDropdownIndex = new();

    public void InitializeDropdown(IEnumerable<ISaveName> saveData, int hideSelfId = -99)
    {
        interactable = true;
        ClearOptions();

        foreach (var data in saveData)
        {
            if (data.id == hideSelfId)
                continue;

            dataToDropdownIndex.Add(options.Count, data.id);
            options.Add(new OptionData {text = data.name});
        }
    }

    public void InsertOption(int index, string text, int id)
    {
        index = index > options.Count ? options.Count : index;
        
        options.Insert(index, new OptionData(text));
        var tempDataToDropdownIndex = new Dictionary<int, int>();

        for (var i = 0; i < index; i++)
        {
            tempDataToDropdownIndex.Add(i, dataToDropdownIndex[i]);
        }

        tempDataToDropdownIndex.Add(index, id);

        for (var i = tempDataToDropdownIndex.Count; i < options.Count; i++)
        {
            tempDataToDropdownIndex.Add(i, dataToDropdownIndex[i - 1]);
        }

        dataToDropdownIndex = tempDataToDropdownIndex;
    }

    public void ShowOptionById(int shownId)
    {
        foreach (var kvp in dataToDropdownIndex)
            if (kvp.Value == shownId)
            {
                value = kvp.Key;
                RefreshShownValue();
                break;
            }
    }

    private new void ClearOptions()
    {
        dataToDropdownIndex.Clear();
        base.ClearOptions();
    }
}
