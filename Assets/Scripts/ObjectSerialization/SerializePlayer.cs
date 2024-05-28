using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializePlayer : MonoBehaviour
{
    public static SerializePlayer instance;
    private void Awake()
    {
        instance = this;
    }
    public void LoadSkinByIndex(int index)
    {
        Debug.Log("Load player skin");
        Player player = Player.instance;
        for (int i = 0; i < player.PlayerMeshes.Length; i++)
        {
            player.PlayerMeshes[i].gameObject.SetActive(false);
            Debug.Log(player.PlayerMeshes[i].name);
        }
        player.CurrentCitizenMesh = player.PlayerMeshes[index];
        player.PlayerMeshes[index].gameObject.SetActive(true);
    }
    public int SaveSkinIndex()
    {
        Player player = Player.instance;
        for (int i = 0; i < player.PlayerMeshes.Length; i++)
        {
            if (player.PlayerMeshes[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }
        Debug.Log("Ошибка: Сейв скина не произошел");
        return -1;
    }
    public int SaveTextureIndex()
    {
        Player player = Player.instance;
        var CitizenVariants = player.CurrentCitizenMesh.GetComponent<CitizenObjectVariants>();
        return CitizenVariants.FindTextureIndex();
    }
    public void LoadTextureByIndex(int Index)
    {
        Player player = Player.instance;
        var CitizenVariants = player.CurrentCitizenMesh.GetComponent<CitizenObjectVariants>();
        CitizenVariants.ChangeTexturesOnLoad(Index);
    }
}
