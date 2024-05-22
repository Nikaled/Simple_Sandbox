using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaverBuildingMenu : MonoBehaviour
{
    [SerializeField] BuildingContentManager[] ContentManagers;
    List<List<bool>> BuildingMenuDataList = new();
    public static SaverBuildingMenu instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Geekplay.Instance.PlayerData.IsFirstPlay = false;
        //Geekplay.Instance.PlayerData.IsFirstPlay = true;


        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.CarsOpened);
        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.AirTransportOpened);
        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.AirportOpened);
        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.FarmOpened);
        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.MilitaryOpened);
        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.CityOpened);
        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.CitizensOpened);
        BuildingMenuDataList.Add(Geekplay.Instance.PlayerData.AnimalsOpened);

        //Geekplay.Instance.Save();
        if (Geekplay.Instance.PlayerData.IsFirstPlay == false)
        {
            LoadItemsState();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Geekplay.Instance.PlayerData.Coins += 15;
            Geekplay.Instance.Save();
        }
    }
    public void ClearSave()
    {
        for (int i = 0; i < BuildingMenuDataList.Count; i++)
        {
            for (int j = 0; j < BuildingMenuDataList[i].Count; j++)
            {
                BuildingMenuDataList[i][j] = false;
            }
        }
        Geekplay.Instance.PlayerData.IsFirstPlay = true;
        Geekplay.Instance.Save();
    }
    public void SaveItemsState(List<bool> contentState, BuildingContentManager ContentManager)
    {
        Geekplay.Instance.PlayerData.IsFirstPlay = false;
       int ContentManagerIndex = -1;
        for (int i = 0; i < ContentManagers.Length; i++)
        {
            if(ContentManager == ContentManagers[i])
            {
                ContentManagerIndex = i;
            }
        }

        BuildingMenuDataList[ContentManagerIndex].Clear();
        BuildingMenuDataList[ContentManagerIndex].AddRange(contentState);
        Geekplay.Instance.Save();
        Debug.Log("Geekplay.Instance.PlayerData.CarsOpened.Length:" + Geekplay.Instance.PlayerData.CarsOpened.Count);
    }
    public void LoadItemsState()
    {
        for (int i = 0; i < BuildingMenuDataList.Count; i++)
        {
            Debug.Log("����� ������:" + i + "/ ���������� ��������: " + BuildingMenuDataList[i].Count);

            List<bool> States = BuildingMenuDataList[i];
            for (int j = 0; j < ContentManagers[i].CellsInGrid.Length; j++)
            {
                if(j > States.Count-1)
                {
                    Debug.Log("����� ������:" + i + "/ ���������� ��������: " + BuildingMenuDataList[i].Count + "/// ���������� ���������");
                    continue;
                }
                ContentManagers[i].CellsInGrid[j].IsOpened = States[j];
                ContentManagers[i].CellsInGrid[j].CheckPriceVisible();
            }
        }
    }
}
