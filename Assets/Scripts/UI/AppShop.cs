using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AppShop : MonoBehaviour
{
    public AppShopCell[] ShopCells;

    private void Start()
    {
        for (int i = 0; i < ShopCells.Length; i++)
        {
            ShopCells[i].SubscribeOnPurchase();
        }
    }
}
