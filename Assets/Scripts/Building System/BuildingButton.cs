using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] GameObject objectPrefab;

    public void SendPrefabToManager()
    {
        BuildingManager.instance.SetBuildingObject(objectPrefab);
    }
}
