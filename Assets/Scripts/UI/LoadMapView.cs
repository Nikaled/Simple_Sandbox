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
                        MapsEmptySlotText[i].text = "���� 1";
                    }
                    else
                    {
                        //MapsEmptySlotImage[i].SetActive(true);
                        MapsEmptySlotText[i].text = "������ ����";
                    }

                    break;
                case 1:
                    if (Geekplay.Instance.PlayerData.NameMap2 != null && Geekplay.Instance.PlayerData.NameMap2 != string.Empty)
                    {
                        //MapsEmptySlotImage[i].SetActive(false);
                        MapsEmptySlotText[i].text = "���� 2";
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
                        MapsEmptySlotText[i].text = "���� 3";

                    }
                    else
                    {
                        //MapsEmptySlotImage[i].SetActive(true);
                        MapsEmptySlotText[i].text = "������ ����";
                    }

                    break;
                case 3:
                    if (Geekplay.Instance.PlayerData.NameMap4 != null && Geekplay.Instance.PlayerData.NameMap4 != string.Empty)
                    {
                        //MapsEmptySlotImage[i].SetActive(false);
                        MapsEmptySlotText[i].text = "���� 4";
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
