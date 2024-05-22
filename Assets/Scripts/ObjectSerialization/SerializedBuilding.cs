using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedBuilding : MonoBehaviour
{
    public string ObjectName;
    [HideInInspector] public int BuildingIndex;
    [HideInInspector] public int CurrentTextureIndex;
    [HideInInspector] public int CurrentHp;

    [HideInInspector] public Vector3 CurrentRotationOfPoint;// реализовать позже
    [HideInInspector] public Vector3 CurrentRotationOfObject;// реализовать позже

    [HideInInspector] public Vector3 CurrentScale;
    [HideInInspector] public Vector3 CurrentPosition;

    public void SaveHp()
    {
        HpSystem hpSystem = GetComponentInChildren<HpSystem>();
        if(hpSystem != null)
        {
            CurrentHp = hpSystem.CurrentHP;
        }
    }
    private void Start()
    {
        SerializeBuildingManager.instance.BuildingsOnScene.Add(this);
    }
    private void OnDestroy()
    {
        if(SerializeBuildingManager.instance != null)
        {
            SerializeBuildingManager.instance.BuildingsOnScene.Remove(this);
        }
    }
    public SerializedBuildingData SaveBuilding()
    {
        BuildingIndex = SerializeBuildingManager.instance.FindObjectPrefabIndex(ObjectName);
        SaveCurrentTextureIndex();
        SaveHp();
        SavePositionAndScale();
        SaveRotation();
        Debug.Log("Current rotation of Root object:" + CurrentRotationOfObject);
      SerializedBuildingData BuildingData = new(BuildingIndex, CurrentTextureIndex, CurrentHp, CurrentRotationOfPoint, CurrentRotationOfObject, CurrentScale, CurrentPosition);
        return BuildingData;
    }
    public void LoadBuilding(SerializedBuildingData BuildingData)
    {
        BuildingIndex = BuildingData.BuildingIndex;
        CurrentTextureIndex = BuildingData.CurrentTextureIndex;
        CurrentHp = BuildingData.CurrentHp;
        CurrentRotationOfPoint = BuildingData.CurrentRotation;
        CurrentRotationOfObject = BuildingData.CurrentRotationOfObject;
        CurrentScale = BuildingData.CurrentScale;
        CurrentPosition = BuildingData.CurrentPosition;

        LoadRotation();
        LoadCurrentTextureIndex();
        LoadHp();
        LoadPositionAndScale();
        Debug.Log("Current rotation of Root object On Load:" + CurrentRotationOfObject);
    }
    public void LoadRotation()
    {
            transform.Rotate(CurrentRotationOfObject);
        RotatingCenter center = GetComponentInChildren<RotatingCenter>();
        if (center != null)
        {
            center.SetRotatingCenter();
            center.gameObject.transform.eulerAngles = CurrentRotationOfPoint;
            center.UnbindRotatingCenter();

        }
        else
        {
            //gameObject.transform.eulerAngles = CurrentRotationOfPoint;
        }
    }
    public void SaveRotation()
    {
        CurrentRotationOfObject = transform.rotation.eulerAngles;


        RotatingCenter center = GetComponentInChildren<RotatingCenter>();
        if (center != null)
        {
            center.SetRotatingCenter();
            CurrentRotationOfPoint = center.gameObject.transform.eulerAngles;
            center.UnbindRotatingCenter();
        }
        else
        {
            //CurrentRotationOfPoint = gameObject.transform.eulerAngles;
        }
    }
    public void LoadHp()
    {
        HpSystem hpSystem = GetComponentInChildren<HpSystem>();
        if (hpSystem != null)
        {
            hpSystem.CurrentHP = CurrentHp;
        }
    }
    public void SaveCurrentTextureIndex()
    {
        ObjectVariants objectVars = GetComponentInChildren<ObjectVariants>();
        if (objectVars != null)
        {
            CurrentTextureIndex = objectVars.FindTextureIndex();
        }
    }
    public void LoadCurrentTextureIndex()
    {
        ObjectVariants objectVars = GetComponentInChildren<ObjectVariants>();
        if (objectVars != null)
        {
            objectVars.ChangeTexturesOnLoad(CurrentTextureIndex);
        }
    }
    public void SavePositionAndScale()
    {

        CurrentScale = transform.localScale;
        CurrentPosition = transform.position;
    }
    public void LoadPositionAndScale()
    {
        transform.position = CurrentPosition;
        transform.localScale = CurrentScale;
    }
}
