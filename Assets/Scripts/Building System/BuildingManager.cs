using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildingManager : MonoBehaviour
{
    private GameObject CurrentPrefab;
    private Vector3 pos;
    private RaycastHit hit;
    [SerializeField] Image Cross;
    [SerializeField] LayerMask layerMask;
    [SerializeField] LayerMask NotDeletingLayerMask;
    [SerializeField] LayerMask TurnRedIgnoreMask;
    private GameObject pendingObj;
    public float rotateAmount = 45;
    bool IsBuildingOpened;
    [SerializeField] GameObject BuildingMenu;
    [SerializeField] Material OnBuildingMaterial;
    [SerializeField] Material RedMaterial;

    [SerializeField] Player player;
    public static BuildingManager instance;

    private bool IsDeletingBuilding;
    List<Material[]> CashedMaterialsOnDeleting;
    private GameObject deletingObject;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(deletingObject != null)
        {
            TurnDeletingObjectNormalAndClearFields();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            ActivateBuildingButton(!IsBuildingOpened);
        }
        if (player.currentState != Player.PlayerState.Building && player.currentState != Player.PlayerState.DeletingBuilding)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Cross.transform.position);
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
        }
        else
        {
            RaycastHit DownHit = new();
            Vector3 MaxDistancePos = ray.GetPoint(20);
            pos = MaxDistancePos;
            // ------- Привязка объектов к полу
            //Physics.Raycast(MaxDistancePos, Vector3.down, out DownHit, 500, layerMask);
            //pos = DownHit.point;
        }

        if (player.currentState == Player.PlayerState.DeletingBuilding)
        {
            DeletingBuildingInput();
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
    public void SetBuildingObject(GameObject ObjectPrefab)
    {
        CurrentPrefab = ObjectPrefab;
        SelectObject();
    }
    private void DeletingBuildingInput()
    {
        Debug.Log("Deleting Input");
        if (Input.GetMouseButtonDown(1))
        {
            player.SwitchPlayerState(Player.PlayerState.Idle);
        }
        Ray ray = Camera.main.ScreenPointToRay(Cross.transform.position);
        if (Physics.Raycast(ray, out hit, 1000, TurnRedIgnoreMask))
        {
            TurnDeletingObjectNormalAndClearFields();
            deletingObject = hit.collider.gameObject;
            Debug.Log("deleting object:" + deletingObject);
            if (IsItDestructable(deletingObject))
            {
                TurnRedDeletingObject(deletingObject.GetComponentsInChildren<MeshRenderer>());
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
    private void TurnDeletingObjectNormalAndClearFields()
    {
        if (deletingObject != null && CashedMaterialsOnDeleting != null)
        {
            TurnNormalDeletingObject(deletingObject.GetComponentsInChildren<MeshRenderer>());
            Debug.Log("Turn normal");
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
        Debug.Log(deletingObject.layer);
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
        Destroy(pendingObj);
        if (Is)
        {
            player.currentState = Player.PlayerState.Building;
        }
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
    private void TurnRedDeletingObject(MeshRenderer[] renderers)
    {
        List<Material[]> materials = new();
        for (int i = 0; i < renderers.Length; i++)
        {
            materials.Add(renderers[i].materials);
            Material[] RedMats = new Material[renderers[i].materials.Length];
            for (int j = 0; j < renderers[i].materials.Length; j++)
            {
                RedMats[j] = RedMaterial;
            }
            renderers[i].materials = RedMats;
        }
        CashedMaterialsOnDeleting = materials;
    }
    private void TurnNormalDeletingObject(MeshRenderer[] renderers)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] CashedMat = CashedMaterialsOnDeleting[i];
            renderers[i].materials = CashedMat;
        }
    }
    private void PlaceObject()
    {
        Instantiate(CurrentPrefab, pendingObj.transform.position, pendingObj.transform.rotation);
        Destroy(pendingObj);
        Debug.Log("Object placed");
        player.SwitchPlayerState(Player.PlayerState.Idle);

    }
}
