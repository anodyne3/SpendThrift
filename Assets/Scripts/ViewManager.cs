using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static View[] allViews;

    private void Awake()
    {
        allViews = GetComponentsInChildren<View>(true);
    }

    public static void ShowView(ViewType viewType, int? contextId = null)
    {
        foreach (var view in allViews)
        {
            if (view.viewType == viewType)
                view.Show();
            else
                view.Hide();
        }
    }
}
