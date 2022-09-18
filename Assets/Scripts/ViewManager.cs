using UnityEngine;

public class ViewManager : MonoBehaviour
{
    private static View[] AllViews;
    private static View ActiveView;
    private static SubView<SaveData> ActiveSubView;

    private void Awake()
    {
        AllViews = GetComponentsInChildren<View>(true);
        Database.LoadData();
    }

    public static void ShowView(ViewType viewType, int[] contextId = null)
    {
        View requiredView = null;

        foreach (var view in AllViews)
            if (view.viewType == viewType)
            {
                requiredView = view;
                break;
            }

        if (requiredView == ActiveView || requiredView == null)
            return;

        switch (requiredView)
        {
            case SubView<SaveData> subView when ActiveSubView == subView:
                return;
            case SubView<SaveData> subView:
                SetActiveView(subView, contextId);
                break;
            case IEditView editView:
                SetActiveView(editView, contextId);
                break;
            default:
                SetActiveView(requiredView, contextId);
                break;
        }
    }

    public static void RefreshView(ViewType viewType)
    {
        if (GetView(viewType) is IRefreshControls view)
            view.RefreshControls();
    }

    private static View GetView(ViewType viewType)
    {
        View requiredView = null;

        foreach (var view in AllViews)
            if (view.viewType == viewType)
                requiredView = view;

        return requiredView;
    }

    private static void SetActiveView(View nextView, int[] contextId)
    {
        if (ActiveView)
            ActiveView.Hide();

        ActiveView = nextView;
        ActiveView.Show(contextId);
    }

    private static void SetActiveView(SubView<SaveData> nextView, int[] contextId)
    {
        if (ActiveSubView)
            ActiveSubView.Hide();

        ActiveSubView = nextView;
        ActiveSubView.Show(contextId);
    }

    private static void SetActiveView(IEditView nextView, int[] contextId)
    {
        nextView.Show(contextId);
    }
}