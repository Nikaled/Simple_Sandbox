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
    [SerializeField] public GameObject CoinsUI;
    [SerializeField] public GameObject MultiplatformUI;
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
    [SerializeField] GameObject CarMobileInstruction;
    [SerializeField] GameObject AppShopButton;
    [SerializeField] public Button DoButton;
    [SerializeField] public Button InteracteButton;
    [Header("Rotating Mode")]
    [SerializeField] GameObject _rotatingChosenObjectModeInstruction;
    [SerializeField] Slider[] RotatingModeSlidersScale;
    [SerializeField] Slider[] RotatingModeSlidersRotation;

    private bool InAppShopActive;
    private bool SaveMapUIActive;
    [Header("Unlock cursor Windows")]
    [SerializeField] private List<GameObject> UnlockCursorWindows;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if(Player.instance.currentState == Player.PlayerState.Idle)
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
    }
    private void Start()
    {
        Geekplay.Instance.GameReady();
        ChangeCoinsText(Geekplay.Instance.PlayerData.Coins);
        if (Geekplay.Instance.mobile)
        {
            CanvasMobileInterface.SetActive(true);
            CanvasPCInterface.SetActive(false);
            InteracteButton.gameObject.SetActive(false);
            ShowMobileIdleButtons(true);
        }
        else
        {
            CanvasMobileInterface.SetActive(false);
            CanvasPCInterface.SetActive(true);
            AppShopButton.SetActive(false);
        }
    }
    #region Mobile
    private void ShowMobileIdleButtons(bool Is)
    {
        LeftButtonsZone.SetActive(Is);
        RightButtonsZone.SetActive(Is);
    }

    public void ShowAllUI(bool Is)
    {
        if (Geekplay.Instance.mobile)
        {
        CanvasMobileInterface.SetActive(Is);
        }
        else
        {
        CanvasPCInterface.SetActive(Is);
        }
        MultiplatformUI.SetActive(Is);
    }
    public void ShowSaveMapUI(bool Is)
    {
        SaveMapUIActive = Is;
        SaveMapUI.SetActive(Is);
        if (Is)
        {
        Player.instance.examplePlayer.LockCursor(false);
        }
        else
        {
            CheckActiveUnlockCursorWindows();
        }
    }
    public void ShowHelicopterMobileInstruction(bool Is)
    {
        HelicopterMobileInstruction.SetActive(Is);
        ShowMobileIdleButtons(!Is);
    }
    public void ShowCarMobileInstruction(bool Is)
    {
        CarMobileInstruction.SetActive(Is);
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
        Geekplay.Instance.LockCursorAfterAd += CheckActiveUnlockCursorWindows;
    }
    private void OnDisable()
    {
        Geekplay.Instance.PlayerData.CoinsChanged -= ChangeCoinsText;
        Geekplay.Instance.LockCursorAfterAd -= CheckActiveUnlockCursorWindows;
    }
    public void CheckActiveUnlockCursorWindows()
    {
        Cursor.lockState = CursorLockMode.Locked;

        for (int i = 0; i < UnlockCursorWindows.Count; i++)
        {
            if(UnlockCursorWindows[i].activeInHierarchy == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
#if UNITY_EDITOR
        if(Geekplay.Instance.mobile == true)
        {
            Cursor.lockState = CursorLockMode.None;
        }
#endif
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
        if (Is)
        {
        ShowObjectInteructInstruction(false);
            ShowCitizenEnterInstruction(false);
        }
    }
    public void ShowControlCarInstruction(bool Is)
    {
        _ControlCarInstruction.SetActive(Is);
    }

    public void ShowInAppShop(bool Is)
    {
        InAppShopActive = Is;
        InAppShop.SetActive(Is);
        if (Is)
        {
       Player.instance.examplePlayer.LockCursor(false);
        }
        else
        {
            CheckActiveUnlockCursorWindows();
        }
    }
    public void ShowCitizenEnterInstruction(bool Is)
    {
        _EnterCitizenInstruction.SetActive(Is);
        if (Is)
        {
            ShowObjectInteructInstruction(false);
            ShowTransportEnterInstruction(false);
        }
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
        if (Is)
        {
            ShowTransportEnterInstruction(false);
            ShowCitizenEnterInstruction(false);
        }

    }
    public void ShowRotatingModeInstruction(bool Is)
    {
        _rotatingModeInstruction.SetActive(Is);
    }
    public void ShowDeletingModeInstruction(bool Is)
    {
        _deletingModeInstruction.SetActive(Is);
        ShowCitizenEnterInstruction(false);
        ShowTransportEnterInstruction(false);
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
            ShowTransportEnterInstruction(false);
            ShowCitizenEnterInstruction(false);
            ShowObjectInteructInstruction(false);
        }
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
