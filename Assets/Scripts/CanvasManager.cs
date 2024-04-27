using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] GameObject _helicopterInstruction;
    [SerializeField] GameObject _planeInstruction;
    [SerializeField] GameObject _EnterTransportInstruction;
    private void Awake()
    {
        instance = this;
    }
    public void ShowHelicopterInstruction(bool Is)
    {
        _helicopterInstruction.SetActive(Is);
    }
    public void ShowTransportEnterInstruction(bool Is)
    {
        _EnterTransportInstruction.SetActive(Is);
    }
    public void ShowPlaneInstruction(bool Is)
    {
        _planeInstruction.SetActive(Is);
    }
}
