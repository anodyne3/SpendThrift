using UnityEngine;
using UnityEngine.UI;

public abstract class View : MonoBehaviour
{
    [SerializeField] public Button CloseButton;
    private int contextId;

    public ViewType viewType => GetViewType();

    private void Awake()
    {
        CloseButton.onClick.AddListener(Hide);
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public abstract ViewType GetViewType();
}
