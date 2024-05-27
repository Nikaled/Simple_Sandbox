using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppShopCell : MonoBehaviour
{
    public string PurName;
    public int PurGold;
    public Button BuyGoldButton;
    public void SubscribeOnPurchase()
    {
        Geekplay.Instance.SubscribeOnPurchase(PurName, GetGold);
        BuyGoldButton.onClick.AddListener(delegate { InAppOperation(); });
    }
    private void InAppOperation()
    {
        Geekplay.Instance.RealBuyItem(PurName);
    }
    private void GetGold()
    {
        Geekplay.Instance.PlayerData.Coins += PurGold;
    }
}
