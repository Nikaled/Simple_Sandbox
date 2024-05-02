using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawGroupManager : MonoBehaviour
{
    [SerializeField] GameObject[] SourceObjects;
    [SerializeField] Camera RawCamera;
    public Camera[] RawCameras;
    void Start()
    {
        for (int i = 0; i < SourceObjects.Length; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
