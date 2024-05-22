using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] public TextMeshProUGUI CoinsText;
    [SerializeField] public Image Crosshair;
    [SerializeField] private GameObject BuildingMenu;
    [SerializeField] private GameObject InAppShop;
    [SerializeField] public GameObject SaveMapUI;
    [Header("PC Interfaces")]
    [SerializeField] GameObject _helicopterInstruction;
    [SerializeField] GameObject _planeInstruction;
    [SerializeField] GameObject _objectInteractionInstruction;
    [SerializeField] GameObject _ControlCarInstruction;

    [SerializeField] GameObject _EnterTransportInstruction;
    [SerializeField] GameObject _EnterCitizenInstruction;

    [SerializeField] GameObject _rotatingModeInstruction;
    [SerializeField] GameObject _idleInstruction;
    [SerializeField] GameObject _buildingModeInstruction;
    [SerializeField] GameObject _deletingModeInstruction;
    [SerializeField] GameObject CanvasPCInterface;
    [Header("Mobile Interfaces")]
    [SerializeField] GameObject CanvasMobileInterface;
    [SerializeField] GameObject LeftButtonsZone;
    [SerializeField] GameObject RightButtonsZone;
    [SerializeField] GameObject HelicopterMobileInstruction;
    [SerializeField] GameObject PlaneMobileInstruction;
    [SerializeField] public Button DoButton;
    [SerializeField] public Button InteracteButton;
    [Header("Rotating Mode")]
    [SerializeField] GameObject _rotatingChosenObjectModeInstruction;
    [SerializeField] Slider[] RotatingModeSlidersScale;
    [SerializeField] Slider[] RotatingModeSlidersRotation;

    private bool InAppShopActive;
    private bool SaveMapUIActive;

    private void Awake()
    {
        InteracteButton.gameObject.SetActive(false);
        instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInAppShop(!InAppShopActive);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowSaveMapUI(!SaveMapUIActive);
        }
    }
    private void Start()
    {
        Geekplay.Instance.GameReady();
        ChangeCoinsText(Geekplay.Instance.PlayerData.Coins);
        if (Geekplay.Instance.mobile)
        {
            CanvasMobileInterface.SetActive(true);
            CanvasPCInterface.SetActive(false);
        }
        else
        {
            CanvasMobileInterface.SetActive(false);
            CanvasPCInterface.SetActive(true);
        }
    }
    #region Mobile
    private void ShowMobileIdleButtons(bool Is)
    {
        LeftButtonsZone.SetActive(Is);
        RightButtonsZone.SetActive(Is);
    }
    public void ShowSaveMapUI(bool Is)
    {
        SaveMapUIActive = Is;
        SaveMapUI.SetActive(Is);
        Player.instance.examplePlayer.LockCursor(!Is);
    }
    public void ShowHelicopterMobileInstruction(bool Is)
    {
        HelicopterMobileInstruction.SetActive(Is);
        ShowMobileIdleButtons(!Is);
    }
    public void ShowPlaneMobileInstruction(bool Is)
    {
        PlaneMobileInstruction.SetActive(Is);
        ShowMobileIdleButtons(!Is);
    }
    #endregion
    private void OnEnable()
    {
        Geekplay.Instance.PlayerData.CoinsChanged += ChangeCoinsText;
    }
    private void OnDisable()
    {
        Geekplay.Instance.PlayerData.CoinsChanged -= ChangeCoinsText;
    }
    private void ChangeCoinsText(int NewValue)
    {
        CoinsText.text = NewValue.ToString();
    }
    public void ShowHelicopterInstruction(bool Is)
    {
        _helicopterInstruction.SetActive(Is);
    }
    public void ShowTransportEnterInstruction(bool Is)
    {
        _EnterTransportInstruction.SetActive(Is);
        ShowObjectInteructInstruction(false);
    }
    public void ShowControlCarInstruction(bool Is)
    {
        _ControlCarInstruction.SetActive(Is);
    }

    public void ShowInAppShop(bool Is)
    {
        InAppShopActive = Is;
        InAppShop.SetActive(Is);
        if (Geekplay.Instance.mobile == false)
        {
            Player.instance.examplePlayer.LockCursor(!Is);
        }
    }
    public void ShowCitizenEnterInstruction(bool Is)
    {
        _EnterCitizenInstruction.SetActive(Is);
        ShowObjectInteructInstruction(false);
        ShowTransportEnterInstruction(false);
    }
    public void ShowPlaneInstruction(bool Is)
    {
        _planeInstruction.SetActive(Is);
    }
    public void ShowBuildingMenu(bool Is)
    {
        BuildingMenu.SetActive(Is);
    }
    public void ShowObjectInteructInstruction(bool Is)
    {
        _objectInteractionInstruction.SetActive(Is);
    }
    public void ShowRotatingModeInstruction(bool Is)
    {
        _rotatingModeInstruction.SetActive(Is);
    }
    public void ShowDeletingModeInstruction(bool Is)
    {
        _deletingModeInstruction.SetActive(Is);
    }
    public void ShowBuildingModeInstruction(bool Is)
    {
        _buildingModeInstruction.SetActive(Is);
    }
    public void ShowIdleInstruction(bool Is)
    {
        _idleInstruction.SetActive(Is);
    }
    public void ShowChosenObjectRotatingModeInstruction(bool Is, Vector3 Scale, Vector3 Rotation)
    {
        _rotatingChosenObjectModeInstruction.SetActive(Is);
        if (Is)
        {
            float[] ScaleParametres = new float[] { Scale.x, Scale.y, Scale.z };
            float[] RotationParametres = new float[] { Rotation.x, Rotation.y, Rotation.z };
            for (int i = 0; i < ScaleParametres.Length; i++)
            {
                if (ScaleParametres[i] > RotatingModeSlidersScale[i].maxValue)
                {
                    ScaleParametres[i] = RotatingModeSlidersScale[i].maxValue;
                }
            }
            for (int i = 0; i < RotationParametres.Length; i++)
            {
                if (RotationParametres[i] < RotatingModeSlidersRotation[i].minValue)
                {
                    RotationParametres[i] += 360f;
                }
                if (RotationParametres[i] > RotatingModeSlidersRotation[i].maxValue)
                {
                    RotationParametres[i] -= 360f;
                }
            }
            for (int i = 0; i < RotatingModeSlidersScale.Length; i++)
            {
                RotatingModeSlidersScale[i].value = ScaleParametres[i];
            }
            for (int i = 0; i < RotatingModeSlidersRotation.Length; i++)
            {
                RotatingModeSlidersRotation[i].value = RotationParametres[i];
            }
        }

    }
}
