using UnityEngine;

public abstract class View : MonoBehaviour
{
    [SerializeField] protected bool isFixed;

    protected int[] context;

    public ViewType ViewType => GetViewType();

    public virtual void Show(int[] newContext)
    {
        context = newContext;

        gameObject.SetActive(true);

        OnShow();
    }

    protected virtual void OnShow()
    {
    }

    public void Hide()
    {
        gameObject.SetActive(isFixed);

        if (this is not FloatingView)
            return;

        if (ViewManager.FloatingViewStack.TryPop(out var superView))
            ViewManager.RefreshView(superView.ViewType);
    }

    protected abstract ViewType GetViewType();
}