using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCenter : MonoBehaviour
{
    [SerializeField] MeshRenderer RootObjectMesh;
    [Header("Transport only")]
    [SerializeField] GameObject RootObjectEmpty;
    public bool IsManualSet;
    private void OnEnable()
    {
        if(IsManualSet == false)
        {
        gameObject.transform.position = RootObjectMesh.bounds.center;
        }
        if(RootObjectMesh != null)
        {
        RootObjectMesh.transform.parent = gameObject.transform;
        }
         else if(RootObjectEmpty != null)
        {
            RootObjectEmpty.transform.parent = gameObject.transform;
        }
    }
}
