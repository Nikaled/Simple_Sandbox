using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBuildingCell : BuildingCell
{
    [SerializeField] Material SkyMaterial;
    protected override IEnumerator SendPrefabToManagerCor()
    {
        yield return new WaitForSeconds(0.1f);
        if (IsOpened)
        {
            BuildingManager.instance.ActivateBuildingButton(false);
            CanvasManager.instance.ShowBuildingMenu(false);
            BuildingManager.instance.SwitchPlayerStateToBuilding();
            ChangeSky();
        }
        else
        {
            if (Geekplay.Instance.PlayerData.Coins >= CoinPrice)
            {
                IsOpened = true;
                BuildingManager.instance.ActivateBuildingButton(false);
                CanvasManager.instance.ShowBuildingMenu(false);
                BuildingManager.instance.SwitchPlayerStateToBuilding();
                Geekplay.Instance.PlayerData.Coins -= CoinPrice;
                ChangeSky();
                OpenedInvoker();
                priceObj.gameObject.SetActive(false);
            }
            else
            {
                CanvasManager.instance.ShowInAppShop(true);
            }
        }
    }
    private void ChangeSky()
    {
        RenderSettings.skybox = SkyMaterial;
    }
    protected override void OpenedInvoker()
    {
        //base.OpenedInvoker();
        ItemOpened?.Invoke();
    }
}
