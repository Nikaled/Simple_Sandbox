using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Coins { 
        get
        {
            return _coinsDontUse;
        }
        set
        {
            _coinsDontUse = value;
            CoinsChanged?.Invoke(_coinsDontUse);
        }
    }
    public event Action<int> CoinsChanged;
    public int _coinsDontUse;

    public bool IsFirstPlay;

    public int BuildCount;
    public int DestroyCount;



    public List<bool> CarsOpened;   
    public List<bool> AirTransportOpened;   
    public List<bool> AirportOpened;   
    public List<bool> FarmOpened;
    public List<bool> MilitaryOpened;   
    public List<bool> CityOpened;   
    public List<bool> CitizensOpened;   
    public List<bool> AnimalsOpened;
    public List<SerializedBuildingData> BuildingData;
    public List<SerializedBuildingData> BuildingDataMap1;
    public List<SerializedBuildingData> BuildingDataMap2;
    public List<SerializedBuildingData> BuildingDataMap3;
    public List<SerializedBuildingData> BuildingDataMap4;
    public string NameMap1;
    public string NameMap2;
    public string NameMap3;
    public string NameMap4;

    public int CurrentSaveSlotLoading;
    public bool IsPlayerMapLoad;
    /////InApps//////
    public string lastBuy;

    public int downloadsCount2;
}