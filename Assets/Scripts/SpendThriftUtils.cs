using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class SpendThriftUtils
{
    public static void SetConsistentFontSize(IEnumerable<Component> list, int lineCount = 1)
    {
        var allTextItems = new List<TextMeshProUGUI>();
        foreach (var item in list)
        {
            if (item.GetComponentInChildren(typeof(TextMeshProUGUI)) is not TextMeshProUGUI textMeshProUGUI)
                continue;

            textMeshProUGUI.enableAutoSizing = true;
            allTextItems.Add(textMeshProUGUI);
        }

        Canvas.ForceUpdateCanvases();

        var smallestFontSize = GetSmallestFontSize(allTextItems);

        foreach (var text in allTextItems)
        {
            text.enableAutoSizing = false;
            text.fontSize = smallestFontSize * lineCount;
        }
    }

    public static void SetConsistentFontSize(Transform groupParent, int lineCount = 1)
    {
        var list = new List<Transform>(groupParent.GetComponentsInChildren<Transform>());
        list.Remove(groupParent);

        var allTextItems = new List<TextMeshProUGUI>();
        foreach (var item in list)
        {
            if (item.GetComponent(typeof(TextMeshProUGUI)) is not TextMeshProUGUI textMeshProUGui)
                continue;

            allTextItems.Add(textMeshProUGui);
        }

        foreach (var text in allTextItems)
            text.enableAutoSizing = true;

        Canvas.ForceUpdateCanvases();

        var smallestFontSize = GetSmallestFontSize(allTextItems);

        foreach (var text in allTextItems)
        {
            text.enableAutoSizing = false;
            text.fontSize = smallestFontSize * lineCount;
        }
    }

    private static float GetSmallestFontSize(List<TextMeshProUGUI> allTextItems)
    {
        var smallestFontSize = float.MaxValue;

        foreach (var text in allTextItems)
        {
            if (text.fontSize < smallestFontSize)
            {
                smallestFontSize = text.fontSize;
            }
        }

        return smallestFontSize;
    }
}
