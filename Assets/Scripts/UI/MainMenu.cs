using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Geekplay.Instance.GameReady();
        Geekplay.Instance.ShowInterstitialAd();
    }
    public void LoadStandartMap()
    {
        Geekplay.Instance.PlayerData.IsPlayerMapLoad = false;
        Geekplay.Instance.Save();
    }
    public void ShowInterstialOnMapChosen()
    {
        Geekplay.Instance.ShowInterstitialAd();
    }
    public void LoadChosenMap(int index)
    {
       
        switch (index)
        {
            case 1:
                if(Geekplay.Instance.PlayerData.NameMap1 !=string.Empty && Geekplay.Instance.PlayerData.NameMap1 != null)
                {
                    Geekplay.Instance.PlayerData.IsPlayerMapLoad = true;
                    Geekplay.Instance.PlayerData.CurrentSaveSlotLoading = index;
                    Geekplay.Instance.Save();
                SceneManager.LoadScene(Geekplay.Instance.PlayerData.NameMap1);
                }
                break;
            case 2:
                if (Geekplay.Instance.PlayerData.NameMap2 != string.Empty && Geekplay.Instance.PlayerData.NameMap2 != null)
                {
                    Geekplay.Instance.PlayerData.IsPlayerMapLoad = true;
                    Geekplay.Instance.PlayerData.CurrentSaveSlotLoading = index;
                    Geekplay.Instance.Save();
                    SceneManager.LoadScene(Geekplay.Instance.PlayerData.NameMap2);
                }
                break;
            case 3:
                if (Geekplay.Instance.PlayerData.NameMap3 != string.Empty && Geekplay.Instance.PlayerData.NameMap3 != null)
                {
                    Geekplay.Instance.PlayerData.IsPlayerMapLoad = true;
                    Geekplay.Instance.PlayerData.CurrentSaveSlotLoading = index;
                    Geekplay.Instance.Save();
                    SceneManager.LoadScene(Geekplay.Instance.PlayerData.NameMap3);
                }
                break;
            case 4:
                if (Geekplay.Instance.PlayerData.NameMap4 != string.Empty && Geekplay.Instance.PlayerData.NameMap4 != null)
                {
                    Geekplay.Instance.PlayerData.IsPlayerMapLoad = true;
                    Geekplay.Instance.PlayerData.CurrentSaveSlotLoading = index;
                    Geekplay.Instance.Save();
                    SceneManager.LoadScene(Geekplay.Instance.PlayerData.NameMap4);
                }
                break;
        }
    }
}
