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

    public void SendPrefabToManager()
    {
        BuildingManager.instance.SetBuildingObject(objectPrefab);
    }
}
