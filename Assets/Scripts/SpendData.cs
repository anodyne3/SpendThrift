using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptOut)]
public class SpendData : SaveData
{
    public SpendData() { }

    public SpendData(int newId, DateTime newDate, int newCategory, float newAmount, string newDescription,
        List<SplitShare> newSplitShares) : base(newId)
    {
        Date = newDate;
        CategoryId = newCategory;
        Amount = newAmount;
        Description = newDescription;
        SplitShares = newSplitShares;
    }

    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int CategoryId { get; set; } = Database.SettingsData.DefaultCategoryId;

    public float Amount
    {
        get => amount;
        set => amount = value;
    }

    private float amount;

    public string Description { get; set; }
    public List<SplitShare> SplitShares { get; set; } = new();
    public bool IsRecurring { get; set; }

    public bool CanAddUser(out UserData userData)
    {
        userData = null;

        foreach (var user in Database.UserData)
            if (SplitShares.Find(x => x.UserId == user.ID) is null)
            {
                userData = user;
                return true;
            }

        return false;
    }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<SpendData>();
    }
}

[Serializable]
public class SplitShare
{
    public SplitShare(int newUserId)
    {
        UserId = newUserId;
    }

    public int UserId { get; set; }

    public float PaymentSplit
    {
        get => paymentSplit;
        set => paymentSplit = Mathf.Clamp(value, 0.0f, 1.0f);
    }

    public float LiabilitySplit
    {
        get => liabilitySplit;
        set => liabilitySplit = Mathf.Clamp(value, 0.0f, 1.0f);
    }

    private float liabilitySplit = 1.0f, paymentSplit = 1.0f;

    public static SplitShare DefaultSplitShare()
    {
        return new SplitShare(Database.SettingsData.DefaultUserId);
    }
}
