using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedBuilding : MonoBehaviour
{
    public string ObjectName;
    [HideInInspector] public int BuildingIndex;
    [HideInInspector] public int CurrentTextureIndex;
    [HideInInspector] public int CurrentHp;

    [HideInInspector] public Vector3 CurrentRotationOfPoint;
    [HideInInspector] public Vector3 CurrentRotationOfObject;
    [HideInInspector] public Vector3 CurrentRotationOfTransport;

    [HideInInspector] public Vector3 CurrentScale;
    [HideInInspector] public Vector3 CurrentPosition;

    public void SaveHp()
    {
        HpSystem hpSystem = GetComponentInChildren<HpSystem>();
        if (hpSystem != null)
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
        if (SerializeBuildingManager.instance != null)
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
        SerializedBuildingData BuildingData = new(BuildingIndex, CurrentTextureIndex, CurrentHp, CurrentRotationOfPoint, CurrentRotationOfObject, CurrentRotationOfTransport, CurrentScale, CurrentPosition);
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
        CurrentRotationOfTransport = BuildingData.CurrentRotationOfTransport;

        LoadPositionAndScale();
        LoadRotation();
        LoadCurrentTextureIndex();
        LoadHp();
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
            if (center.RootObjectEmpty != null)
            {
                center.RootObjectEmpty.transform.eulerAngles = CurrentRotationOfTransport;
            }
            center.UnbindRotatingCenter();

        }
        else
        {
            gameObject.transform.eulerAngles = CurrentRotationOfPoint;
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
            if (center.RootObjectEmpty != null)
            {
                CurrentRotationOfTransport = center.RootObjectEmpty.transform.eulerAngles;
            }
            center.UnbindRotatingCenter();

        }
        else
        {
            CurrentRotationOfPoint = gameObject.transform.eulerAngles;
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

        CurrentPosition = transform.position;
        //CurrentPosition = transform.localPosition;
        RotatingCenter center = GetComponentInChildren<RotatingCenter>();
        if (center != null)
        {
            center.SetRotatingCenter();
            if (center.RootObjectMesh != null)
            {
                CurrentScale = center.RootObjectMesh.gameObject.transform.localScale;
            }
            if (center.RootObjectEmpty != null)
            {
                CurrentScale = center.RootObjectEmpty.gameObject.transform.localScale;
                CurrentPosition = center.RootObjectEmpty.gameObject.transform.position;
            }
            center.UnbindRotatingCenter();
        }
        else
        {
            ScalingParent ScaleParent = GetComponentInChildren<ScalingParent>();
            if (ScaleParent != null)
            {
                ScaleParent.SetThisAsParent();
                CurrentScale = ScaleParent.transform.localScale;
                ScaleParent.SetThisAsChild();

            }
            else
            {
                CurrentScale = transform.localScale;

            }
        }
        Debug.Log(gameObject.name + " Позиция сохранена:" + CurrentPosition);
    }
    public void LoadPositionAndScale()
    {

        transform.position = CurrentPosition;
        //transform.localPosition = CurrentPosition;
        RotatingCenter center = GetComponentInChildren<RotatingCenter>();
        if (center != null)
        {

            center.SetRotatingCenter();
            if (center.RootObjectMesh != null)
            {
                center.RootObjectMesh.gameObject.transform.localScale = CurrentScale;
            }
            if (center.RootObjectEmpty != null)
            {
                center.RootObjectEmpty.gameObject.transform.localScale = CurrentScale;
                gameObject.transform.position = CurrentPosition;
                center.RootObjectEmpty.gameObject.transform.position = CurrentPosition;

            }

            center.UnbindRotatingCenter();
        }
        else
        {
            ScalingParent ScaleParent = GetComponentInChildren<ScalingParent>();
            if (ScaleParent != null)
            {
                ScaleParent.SetThisAsParent();
                ScaleParent.transform.localScale = CurrentScale;
                ScaleParent.SetThisAsChild();

            }
            else
            {
                transform.localScale = CurrentScale;
            }

            Debug.Log("Current position of " + gameObject.name + ":" + CurrentPosition);
            Debug.Log("Current position of " + gameObject.name + " по факту:" + transform.position);
        }
    }
}
