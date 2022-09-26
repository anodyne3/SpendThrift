using System.Collections.Generic;
using TMPro;

public class DictionaryDropdown : TMP_Dropdown
{
    public int OptionId => dataToDropdownIndex[value];

    private Dictionary<int, int> dataToDropdownIndex = new();

    public void InitializeDropdown(IEnumerable<ISaveName> saveData, List<int> hideIds = null)
    {
        ClearOptions();

        foreach (var data in saveData)
        {
            if (hideIds != null && hideIds.Contains(data.ID))
                continue;

            dataToDropdownIndex.Add(options.Count, data.ID);
            options.Add(new OptionData {text = data.Name});
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
