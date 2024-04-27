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
    private GameObject pendingObj;
    public float rotateAmount = 45;
    bool IsBuildingOpened;
    [SerializeField] GameObject BuildingMenu;
    [SerializeField] Material OnBuildingMaterial;

    [SerializeField] Player player;
    public static BuildingManager instance;

    private bool IsDeletingBuilding;
    private void Awake()
    {
        instance = this;
    }
    void Update()
    {

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
            Vector3 MaxDistancePos = ray.GetPoint(50);
            Physics.Raycast(MaxDistancePos, Vector3.down, out DownHit, 500, layerMask);
            pos = DownHit.point;
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
        if (Input.GetMouseButtonDown(1))
        {
            ActivateDeletingState(false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            DeleteObject();
        }
    }
    public void ActivateDeletingState(bool Is)
    {
        IsDeletingBuilding = Is;
        if (Is)
        {
            player.SwitchPlayerState(Player.PlayerState.DeletingBuilding);
        }
        else
        {
            player.SwitchPlayerState(Player.PlayerState.Idle);
        }
        BuildingMenu.SetActive(IsDeletingBuilding);
        player.examplePlayer.LockCursor(IsDeletingBuilding);
    }
    private void DeleteObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Cross.transform.position);
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            GameObject deletingObject = hit.collider.gameObject;
            if (IsItDestructable(deletingObject))
            {
                Destroy(deletingObject);
            }
            else
            {
                Debug.Log("Этот объект нельзя удалить");
            }
        }
        bool IsItDestructable(GameObject deletingObject)
        {
            Debug.Log(deletingObject.layer);
            if(((NotDeletingLayerMask & (1 << deletingObject.layer)) != 0))
            {
                return false;
            }
            //return (!deletingObject.CompareTag("Ground"));
            return true;
        }
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
            //renderers[i].materials = null;
            renderers[i].material = OnBuildingMaterial;

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
