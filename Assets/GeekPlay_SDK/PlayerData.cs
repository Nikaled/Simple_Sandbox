using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Coins { 
        get
        {
            return _coins;
        }
        set
        {
            _coins = value;
            CoinsChanged?.Invoke(_coins);
        }
    }
    public event Action<int> CoinsChanged;
    private int _coins;
    /////InApps//////
    public string lastBuy;
}