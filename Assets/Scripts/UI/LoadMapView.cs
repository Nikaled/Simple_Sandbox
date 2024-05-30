using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadMapView : MonoBehaviour
{
    [SerializeField] GameObject[] MapsEmptySlotImage;
    [SerializeField] TextMeshProUGUI[] MapsEmptySlotText;
    private void Start()
    {
        MapSlotsViewUpdate();
        //Geekplay.Instance.PlayerData.MapDate1 = null;
        //    Geekplay.Instance.PlayerData.MapDate2 = null;
        //Geekplay.Instance.PlayerData.MapDate3 = null;
        //Geekplay.Instance.PlayerData.MapDate4 = null;
    }
    public void MapSlotsViewUpdate()
    {
        for (int i = 0; i < MapsEmptySlotText.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if (Geekplay.Instance.PlayerData.NameMap1 != null && Geekplay.Instance.PlayerData.NameMap1 != string.Empty)
                    {
                        //MapsEmptySlotImage[i].SetActive(false);
                        MapsEmptySlotText[i].text = Geekplay.Instance.PlayerData.MapDate1;
                    }
                    else
                    {
                        //MapsEmptySlotImage[i].SetActive(true);
                        MapsEmptySlotText[i].text = "Пустой слот";
                    }

                    break;
                case 1:
                    if (Geekplay.Instance.PlayerData.NameMap2 != null && Geekplay.Instance.PlayerData.NameMap2 != string.Empty)
                    {
                        //MapsEmptySlotImage[i].SetActive(false);
                        MapsEmptySlotText[i].text = Geekplay.Instance.PlayerData.MapDate2;
                    }
                    else
                    {
                        //MapsEmptySlotImage[i].SetActive(true);
                    }

                    break;
                case 2:
                    if (Geekplay.Instance.PlayerData.NameMap3 != null && Geekplay.Instance.PlayerData.NameMap3 != string.Empty)
                    {
                        //MapsEmptySlotImage[i].SetActive(false);
                        MapsEmptySlotText[i].text = Geekplay.Instance.PlayerData.MapDate3;

                    }
                    else
                    {
                        //MapsEmptySlotImage[i].SetActive(true);
                        MapsEmptySlotText[i].text = "Пустой слот";
                    }

                    break;
                case 3:
                    if (Geekplay.Instance.PlayerData.NameMap4 != null && Geekplay.Instance.PlayerData.NameMap4 != string.Empty)
                    {
                        //MapsEmptySlotImage[i].SetActive(false);
                        MapsEmptySlotText[i].text = Geekplay.Instance.PlayerData.MapDate4;
                    }
                    else
                    {
                        //MapsEmptySlotImage[i].SetActive(true);
                    }
                    break;
            }
        }
    }
}
