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

    public bool IsFirstPlay;




    public List<bool> CarsOpened;   
    public List<bool> AirTransportOpened;   
    public List<bool> AirportOpened;   
    public List<bool> FarmOpened;
    public List<bool> MilitaryOpened;   
    public List<bool> CityOpened;   
    public List<bool> CitizensOpened;   
    public List<bool> AnimalsOpened;   
    /////InApps//////
    public string lastBuy;
}