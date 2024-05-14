using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class BuildingManager : MonoBehaviour
{
    private GameObject CurrentPrefab;
    private Vector3 pos;
    private RaycastHit hit;
    [SerializeField] Image Cross;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask NotDeletingLayerMask;
    [SerializeField] LayerMask TurnRedIncludeMask;
    private GameObject pendingObj;
    public float rotateAmount = 45;
    bool IsBuildingOpened;
    [SerializeField] GameObject BuildingMenu;
    [SerializeField] Material OnBuildingMaterial;
    [SerializeField] Material RedMaterial;
    [SerializeField] Material YellowMaterial;

    [SerializeField] Player player;
    public static BuildingManager instance;

    private bool IsDeletingBuilding;
    List<Material[]> CashedMaterialsOnDeleting;
    List<Material[]> CashedMaterialsOnRotating;
    private GameObject deletingObject;
    private GameObject rotatingObject;

    Vector3 RotatingCashedRotating;
    Vector3 RotatingCashedScale;
    private float CashedRotatingX;
    private float CashedRotatingY;
    private float CashedRotatingZ;
    public bool RotateChosenObjectMode { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    public void SetBuildingObject(GameObject ObjectPrefab)
    {
        CurrentPrefab = ObjectPrefab;
        SelectObject();
    }
    public void BuildingInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Cross.transform.position);
        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            pos = hit.point;
        }
        else
        {
            RaycastHit DownHit = new();
            Vector3 MaxDistancePos = ray.GetPoint(20);
            pos = MaxDistancePos;
        }
        if (pendingObj != null)
        {
            pendingObj.transform.position = pos;

            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotateObject();
            }
        }
    }
    public void DeletingBuildingInput()
    {

        Debug.Log("Deleting Input");
        Ray ray = Camera.main.ScreenPointToRay(Cross.transform.position);
        if (Physics.Raycast(ray, out hit, 1000, TurnRedIncludeMask))
        {
            if(deletingObject != hit.collider.gameObject)
            {
                TurnDeletingObjectNormalAndClearFields();
                deletingObject = hit.collider.gameObject;
                Debug.Log("deleting object:" + deletingObject);
                if (IsItDestructable(deletingObject))
                {
                    TurnIntoColorChosenObject(deletingObject.GetComponentsInChildren<MeshRenderer>(), RedMaterial, ref CashedMaterialsOnDeleting);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                DeleteObject(deletingObject);
            }
        }
        else
        {
            TurnDeletingObjectNormalAndClearFields();
        }
    }
    public void TurnDeletingObjectNormalAndClearFields()
    {
        if (deletingObject != null && CashedMaterialsOnDeleting != null)
        {
            TurnNormalChosenObject(deletingObject.GetComponentsInChildren<MeshRenderer>(), CashedMaterialsOnDeleting);
        }
        CashedMaterialsOnDeleting = null;
        deletingObject = null;
    }

    public void DeletingButton(bool Is)
    {
        ActivateDeletingMode(Is);
        SwitchPlayerState();
    }

    public void ActivateDeletingMode(bool Is)
    {
        IsDeletingBuilding = Is;
        BuildingMenu.SetActive(IsDeletingBuilding);
        player.examplePlayer.LockCursor(IsDeletingBuilding);
        TurnDeletingObjectNormalAndClearFields();
    }
    private void SwitchPlayerState()
    {
        if (IsDeletingBuilding)
        {
            player.SwitchPlayerState(Player.PlayerState.DeletingBuilding);
        }
        else
        {
            player.SwitchPlayerState(Player.PlayerState.Idle);
        }
    }
    private void DeleteObject(GameObject deletingObject)
    {

        if (IsItDestructable(deletingObject))
        {
            Destroy(deletingObject);
        }
        else
        {
            Debug.Log("Этот объект нельзя удалить");
        }
    }
    private bool IsItDestructable(GameObject deletingObject)
    {
        if (((NotDeletingLayerMask & (1 << deletingObject.layer)) != 0))
        {
            return false;
        }
        return true;
    }
    public void ActivateBuildingButton(bool Is)
    {
        IsBuildingOpened = Is;
        BuildingMenu.SetActive(IsBuildingOpened);
        player.examplePlayer.LockCursor(!IsBuildingOpened);

        if (pendingObj != null) { Destroy(pendingObj); }
    }
    private void RotateObject()
    {
        pendingObj.transform.Rotate(Vector3.up, rotateAmount);
    }
    public void SelectObject()
    {
        pendingObj = Instantiate(CurrentPrefab, pos, transform.rotation);
        DeactivateColliders(pendingObj.GetComponentsInChildren<Collider>());
        SetOnBuildMaterial(pendingObj.GetComponentsInChildren<MeshRenderer>());
        if (pendingObj.GetComponentInChildren<Rigidbody>() != null) pendingObj.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        Debug.Log("Object Selected");
        player.SwitchPlayerState(Player.PlayerState.Building);
        player.examplePlayer.LockCursor(true);
        pendingObj.transform.LookAt(player.transform);
        pendingObj.transform.DORotate(new Vector3(0, pendingObj.transform.position.y, 0), 0);

    }
    private void PlaceObject()
    {
        var newObj = Instantiate(CurrentPrefab, pendingObj.transform.position, pendingObj.transform.rotation);
        if (CurrentPrefab.CompareTag("Road"))
        {
            newObj.transform.position += new Vector3(0, 0.2f, 0);
        }
        Destroy(pendingObj);
        Debug.Log("Object placed");
        player.SwitchPlayerState(Player.PlayerState.Idle);

    }
    private void DeactivateColliders(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }
    private void SetOnBuildMaterial(MeshRenderer[] renderers)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                Destroy(renderers[i].materials[j]);
            }
            renderers[i].material = OnBuildingMaterial;

        }
    }
    private void TurnIntoColorChosenObject(MeshRenderer[] renderers, Material colorMaterial, ref List<Material[]> CashedMaterials)
    {
        List<Material[]> materials = new();
        for (int i = 0; i < renderers.Length; i++)
        {
            materials.Add(renderers[i].materials);
            Material[] RedMats = new Material[renderers[i].materials.Length];
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                RedMats[j] = colorMaterial;
            }
            renderers[i].materials = RedMats;
        }
        CashedMaterials = materials;
    }
    private void TurnNormalChosenObject(MeshRenderer[] renderers, List<Material[]> CashedMaterials)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] CashedMat = CashedMaterials[i];
            renderers[i].materials = CashedMat;
        }
    }

    #region RotatingBuildings

    public void ActivateRotatingMode(bool IsActive)
    {
        CanvasManager.instance.ShowRotatingModeInstruction(IsActive);
        TurnRotatingObjectNormalAndClearFields();
    }
    public void RotatingInput()
    {
        if (!RotateChosenObjectMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Cross.transform.position);
            if (Physics.Raycast(ray, out hit, 1000, TurnRedIncludeMask))
            {
                if(rotatingObject != hit.collider.gameObject)
                {
                    TurnRotatingObjectNormalAndClearFields();


                    rotatingObject = hit.collider.gameObject;
                    Debug.Log("rotatingObject object:" + rotatingObject);
                    if (IsItDestructable(rotatingObject))
                    {
                        TurnIntoColorChosenObject(rotatingObject.GetComponentsInChildren<MeshRenderer>(), YellowMaterial, ref CashedMaterialsOnRotating);
                    }
                }
               
            }
            else
            {
                TurnRotatingObjectNormalAndClearFields();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (rotatingObject != null)
                {
                    ActivateRotateChosenObjectMode(true);
                }
            }
        }
        else
        {
            RotateChosenObjectInput();
        }
    }
    public void TurnRotatingObjectNormalAndClearFields()
    {
        if (rotatingObject != null && CashedMaterialsOnRotating != null)
        {
            Debug.Log("Turn object from yellow to normal" + rotatingObject.name);
            TurnNormalChosenObject(rotatingObject.GetComponentsInChildren<MeshRenderer>(), CashedMaterialsOnRotating);
        }
        CashedMaterialsOnRotating = null;
        rotatingObject = null;
    }
    private void ActivateRotateChosenObjectMode(bool Is)
    {
        RotateChosenObjectMode = Is;
        RotatingCashedRotating = rotatingObject.transform.eulerAngles;
        RotatingCashedScale = rotatingObject.transform.localScale;
        CanvasManager.instance.ShowRotatingModeInstruction(false);
        CanvasManager.instance.ShowChosenObjectRotatingModeInstruction(Is, Scale:RotatingCashedScale, Rotation: RotatingCashedRotating);
        if (Is)
        {
            ChangeTextureManager.instance.ButtonsInitialize(rotatingObject);
            TurnNormalChosenObject(rotatingObject.GetComponentsInChildren<MeshRenderer>(), CashedMaterialsOnRotating);
        }
        if (!Is)
        {
            ChangeTextureManager.instance.ClearButtonListeners();
        }
        player.examplePlayer.LockCursor(!Is);
    }
    private void RotateChosenObjectInput()
    {
        if (rotatingObject == null)
        {
            Debug.Log("Объект кручения null");
        }
    }
    public void ApplyRotatingChanges()
    {
        ActivateRotateChosenObjectMode(false);
        player.SwitchPlayerState(Player.PlayerState.Idle);
    }
    public void CancelRotatingChanges()
    {
        rotatingObject.transform.localScale = RotatingCashedScale;
        rotatingObject.transform.eulerAngles = RotatingCashedRotating;
        CanvasManager.instance.ShowChosenObjectRotatingModeInstruction(true, Scale: RotatingCashedScale, Rotation: RotatingCashedRotating);
    }
    #region RotatingSliders
    public void RotatingSliderScaleX(float IncreaseNumber)
    {
        rotatingObject.transform.DOScaleX( IncreaseNumber, 0);
    }
    public void RotatingSliderScaleY(float IncreaseNumber)
    {
        rotatingObject.transform.DOScaleY(IncreaseNumber, 0);
    }
    public void RotatingSliderScaleZ(float IncreaseNumber)
    {
        rotatingObject.transform.DOScaleZ( IncreaseNumber,0);
    }
    public void RotatingSliderRotateX(float IncreaseNumber)
    {
        rotatingObject.transform.rotation = Quaternion.Euler(IncreaseNumber, rotatingObject.transform.eulerAngles.y, rotatingObject.transform.eulerAngles.z);
    }
    public void RotatingSliderRotateY(float IncreaseNumber)
    {
        rotatingObject.transform.rotation = Quaternion.Euler(rotatingObject.transform.eulerAngles.x, IncreaseNumber, rotatingObject.transform.eulerAngles.z);
    }
    public void RotatingSliderRotateZ(float IncreaseNumber)
    {
        rotatingObject.transform.rotation = Quaternion.Euler(rotatingObject.transform.eulerAngles.x, rotatingObject.transform.eulerAngles.y, IncreaseNumber);
    }

    #endregion
    #endregion

}
