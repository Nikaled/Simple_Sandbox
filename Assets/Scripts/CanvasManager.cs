using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] GameObject _helicopterInstruction;
    [SerializeField] GameObject _planeInstruction;
    [SerializeField] GameObject _EnterTransportInstruction;
    [SerializeField] public Image Crosshair;
    Vector2 ThirdViewCrossPosition = new Vector2(150, 150);
    Vector2 FirstViewCrossPosition = new Vector2(0, 0);
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
    public void IsCrossForThirdView(bool Is)
    {
        if (Is)
        {
            Crosshair.gameObject.transform.localPosition = ThirdViewCrossPosition;
        }
        else
        {
            Crosshair.gameObject.transform.localPosition = FirstViewCrossPosition;
        }
    }
}
