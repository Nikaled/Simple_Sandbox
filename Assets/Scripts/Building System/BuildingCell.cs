using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCell : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;
    public string ObjectName = "------";
    public TextMeshProUGUI ObjectNameText; 
    public RawImage ObjectScreen;
    public int CoinPrice;
    public TextMeshProUGUI PriceText;
    private void Start()
    {
        InitializeButtons();
        if(CoinPrice > 0)
        {
        PriceText.text = CoinPrice.ToString();
        }
        else
        {
            PriceText.gameObject.SetActive(false);
        }
        
    }
    private void InitializeButtons()
    {
        if(gameObject.GetComponent<Button>() != null)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(delegate { SendPrefabToManager(); });
        }
    }
    public void SendPrefabToManager()
    {
        if(Geekplay.Instance.PlayerData.Coins >= CoinPrice)
        {
            BuildingManager.instance.ActivateBuildingButton(false);
            CanvasManager.instance.ShowBuildingMenu(false);
            Geekplay.Instance.PlayerData.Coins -= CoinPrice;
            BuildingManager.instance.SetBuildingObject(objectPrefab);
        }
        else
        {
            CanvasManager.instance.ShowInAppShop(true);
        }
    }
}
