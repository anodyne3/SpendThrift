using UnityEngine;

public abstract class View : MonoBehaviour
{
    [SerializeField] protected bool isFixed;

    protected int[] context;
    public ViewType viewType => GetViewType();

    private void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public virtual void Show(int[] newContext)
    {
        context = newContext;

        gameObject.SetActive(true);

        OnShow();
    }

    protected virtual void OnShow() { }

    public virtual void Hide()
    {
        gameObject.SetActive(isFixed);

        OnHide();
    }

    protected virtual void OnHide() { }

    public abstract ViewType GetViewType();
}
