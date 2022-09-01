using System;
using UnityEngine;
using UnityEngine.UI;

public class SpendView : View
{
    [SerializeField] private Button userButton, categoryButton;

    protected override void OnAwake()
    {
        base.OnAwake();

        userButton.onClick.AddListener(EditUsers);
        categoryButton.onClick.AddListener(EditCategories);
    }

    private void EditUsers()
    {
        ViewManager.ShowView(ViewType.User);
    }

    private void EditCategories()
    {
        ViewManager.ShowView(ViewType.Category);
    }

    public override ViewType GetViewType()
    {
        return ViewType.Spend;
    }
}

public class SpendControl : ControlItem<SpendData>
{
    public override void Refresh(){}


}

public abstract class ControlItem<T> : MonoBehaviour where T : SaveData
{
    protected T data;
    
    void SetData(T newData)
    {
        data = newData;
        
        Refresh();
    }
    
    public abstract void Refresh();
}

public class SpendData : SaveData
{
    public SpendData(int newId, SpendCategory newCategory, float newAmount, string newDescription) : base (newId)
    {
        category = newCategory;
        description = newDescription;
        amount = newAmount;
    }

    public DateTime date {get; private set;}
    public SpendCategory category {get; private set;}
    public float amount {get; private set;}
    public string description {get; private set;}
    
    public void SetAmount(float newAmount)
    {
        amount = newAmount;
        Save();
    }
    
    public void SetCategory(SpendCategory newCategory)
    {
        category = newCategory;
        Save();
    }
    
    public void SetDescription(string newDescription)
    {
        description = newDescription;
        Save();
    }
}

public class SaveData
{
    public int id {get; private set;}
    
    public SaveData(int newId)
    {
        id = newId;
    }

    public void Save()
    {
    
    }
    
    public void Load()
    {
    
    }
    
    public void Delete()
    {
    
    }
}