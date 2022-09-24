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
        date = newDate;
        categoryId = newCategory;
        amount = newAmount;
        description = newDescription;
        splitShares = newSplitShares;
    }

    public DateTime date { get; set; }
    public int categoryId { get; set; }
    public float amount { get; set; }
    public string description { get; set; }
    public List<SplitShare> splitShares { get; set; } = new();
    public bool isRecurring { get; set; }

    public override void Save()
    {
        base.Save();

        SaveSystem.SaveData<SpendData>();
    }

    public bool CanAddUser(out UserData userData)
    {
        userData = null;
        
        foreach (var user in Database.userData)
            if (splitShares.Find(x => x.userId == user.id) is null)
            {
                userData = user;
                return true;
            }

        return false;
    }

    public void AddSplitShare(int addedUserId)
    {
        splitShares.Add(new SplitShare(addedUserId) {LiabilitySplit = UpdateLiabilitySplits(), PaymentSplit = 0f});
        Save();
        ViewManager.RefreshView(ViewType.EditSplitShares);

        float UpdateLiabilitySplits()
        {
            var newLiability = 0f;

            foreach (var splitShare in splitShares)
            {
                splitShare.LiabilitySplit -= newLiability += splitShare.LiabilitySplit / (splitShares.Count + 1);
            }

            return newLiability;
        }
    }


    [Serializable]
    public class SplitShare
    {
        public SplitShare(int newUserId)
        {
            userId = newUserId;
        }

        public int userId { get; set; }

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
    }

    public static SplitShare DefaultSplitShare()
    {
        return new SplitShare(Database.settingsData.defaultUserId);
    }
}
