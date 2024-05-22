using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCenter : MonoBehaviour
{
    Vector3 CashedPositionInObject;
    [SerializeField] MeshRenderer RootObjectMesh;
    [Header("Transport only")]
    [SerializeField] GameObject RootObjectEmpty;
    public bool IsManualSet;
    private void OnEnable()
    {

        if (IsManualSet == false)
        {
            gameObject.transform.position = RootObjectMesh.bounds.center;
        }
        if (RootObjectMesh != null)
        {
            gameObject.transform.SetParent(RootObjectMesh.gameObject.transform);

        }
        else if (RootObjectEmpty != null)
        {
            gameObject.transform.parent = RootObjectEmpty.transform.parent;
            //CashedPositionInObject = transform.localPosition;
            //gameObject.transform.parent = null;
        }
    }
    public void SetRotatingCenter()
    {
        if (IsManualSet == false)
        {
            gameObject.transform.position = RootObjectMesh.bounds.center;
        }
        if (RootObjectMesh != null)
        {
            gameObject.transform.parent = RootObjectMesh.transform;
            gameObject.transform.parent = RootObjectMesh.transform.parent;
            RootObjectMesh.transform.parent = gameObject.transform;
        }
        else if (RootObjectEmpty != null)
        {
            RootObjectEmpty.transform.parent = gameObject.transform;
        }
    }
    public void UnbindRotatingCenter()
    {
        RootObjectMesh.transform.parent = gameObject.transform.parent;  
        gameObject.transform.parent = RootObjectMesh.transform.parent;
    }
}
