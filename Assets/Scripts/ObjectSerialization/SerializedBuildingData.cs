using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedBuildingData
{
    public int BuildingIndex;
    public int CurrentTextureIndex;
    public int CurrentHp;
    public Vector3 CurrentRotation;
    public Vector3 CurrentRotationOfObject;
    public Vector3 CurrentScale;
    public Vector3 CurrentPosition;
    public Vector3 CurrentRotationOfTransport;
    public SerializedBuildingData(int BuildingIndex, int CurrentTextureIndex, int CurrentHp, Vector3 CurrentRotation, Vector3 CurrentRotationOfObject, Vector3 CurrentRotationOfTransport, Vector3 CurrentScale, Vector3 CurrentPosition)
    {
        this.BuildingIndex = BuildingIndex;
        this.CurrentTextureIndex = CurrentTextureIndex;
        this.CurrentHp = CurrentHp;
        this.CurrentRotation = CurrentRotation;
        this.CurrentRotationOfObject = CurrentRotationOfObject;
        this.CurrentScale = CurrentScale;
        this.CurrentPosition = CurrentPosition;
        this.CurrentRotationOfTransport = CurrentRotationOfTransport;
    }

}
