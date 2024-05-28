using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SerializeBuildingManager : MonoBehaviour
{
    [SerializeField] GameObject[] AllPrefabsInGame;
    public static SerializeBuildingManager instance;
    [HideInInspector] public List<SerializedBuilding> BuildingsOnScene = new();
   [HideInInspector] public List<SerializedBuildingData> BuildingData = new();

    private void Awake()
    {
        instance = this;
    }
    public void ToMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void ReloadSceneButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void TryLoadPlayerSkin(int SkinIndex, int TextureIndex)
    {
        SerializePlayer.instance.LoadSkinByIndex(SkinIndex);
        SerializePlayer.instance.LoadTextureByIndex(TextureIndex);
    }
    private void SavePlayerSkin(ref int SkinSlot, ref int TextureSlot)
    {
        SkinSlot = SerializePlayer.instance.SaveSkinIndex();
        TextureSlot = SerializePlayer.instance.SaveTextureIndex();
    }
    public void SaveInMapSlot(int number)
    {
        for (int i = 0; i < BuildingsOnScene.Count; i++)
        {
            BuildingData.Add(BuildingsOnScene[i].SaveBuilding());
        }
        Geekplay.Instance.PlayerData.BuildingData = new();
        Geekplay.Instance.PlayerData.BuildingData.AddRange(BuildingData);
        Debug.Log("Сохранено объектов: " + Geekplay.Instance.PlayerData.BuildingData.Count);
        switch (number)
        {
            case 1:
                Geekplay.Instance.PlayerData.BuildingDataMap1 = Geekplay.Instance.PlayerData.BuildingData;
                Geekplay.Instance.PlayerData.NameMap1 = SceneManager.GetActiveScene().name;
                SavePlayerSkin(ref Geekplay.Instance.PlayerData.PlayerSkin1, ref Geekplay.Instance.PlayerData.PlayerTexture1);
                break;
            case 2:
                Geekplay.Instance.PlayerData.BuildingDataMap2 = Geekplay.Instance.PlayerData.BuildingData;
                Geekplay.Instance.PlayerData.NameMap2 = SceneManager.GetActiveScene().name;
                SavePlayerSkin(ref Geekplay.Instance.PlayerData.PlayerSkin2, ref Geekplay.Instance.PlayerData.PlayerTexture2);
                break;
            case 3:
                Geekplay.Instance.PlayerData.BuildingDataMap3 = Geekplay.Instance.PlayerData.BuildingData;
                Geekplay.Instance.PlayerData.NameMap3 = SceneManager.GetActiveScene().name;
                SavePlayerSkin(ref Geekplay.Instance.PlayerData.PlayerSkin3, ref Geekplay.Instance.PlayerData.PlayerTexture3);
                break;
            case 4:
                Geekplay.Instance.PlayerData.BuildingDataMap4 = Geekplay.Instance.PlayerData.BuildingData;
                Geekplay.Instance.PlayerData.NameMap4 = SceneManager.GetActiveScene().name;
                SavePlayerSkin(ref Geekplay.Instance.PlayerData.PlayerSkin4, ref Geekplay.Instance.PlayerData.PlayerTexture4);
                break;
        }
        BuildingData.Clear();
        Geekplay.Instance.Save();
    }
    public int FindObjectPrefabIndex(string buildingName)
    {
        for (int i = 0; i < AllPrefabsInGame.Length; i++)
        {
            if(buildingName == AllPrefabsInGame[i]?.GetComponent<SerializedBuilding>()?.ObjectName)
            {
                return i;
            }
        }
        Debug.Log("Не найден префаб в списке:"+buildingName);
        return -1;
    }
    private void Start()
    {
        if (Geekplay.Instance.PlayerData.IsPlayerMapLoad)
        {
            LoadBuildingsFromSlot();
            Geekplay.Instance.PlayerData.IsPlayerMapLoad = false;
            Geekplay.Instance.Save();
        }
    }
    private void LoadBuildingsFromSlot()
    {
        switch (Geekplay.Instance.PlayerData.CurrentSaveSlotLoading)
        {
            case 1:
                LoadBuildings(Geekplay.Instance.PlayerData.BuildingDataMap1);
                TryLoadPlayerSkin(Geekplay.Instance.PlayerData.PlayerSkin1, Geekplay.Instance.PlayerData.PlayerTexture1);
                break;
            case 2:
                LoadBuildings(Geekplay.Instance.PlayerData.BuildingDataMap2);
                TryLoadPlayerSkin(Geekplay.Instance.PlayerData.PlayerSkin2, Geekplay.Instance.PlayerData.PlayerTexture2);
                break;
            case 3:
                LoadBuildings(Geekplay.Instance.PlayerData.BuildingDataMap3);
                TryLoadPlayerSkin(Geekplay.Instance.PlayerData.PlayerSkin3, Geekplay.Instance.PlayerData.PlayerTexture3);
                break;
            case 4:
                LoadBuildings(Geekplay.Instance.PlayerData.BuildingDataMap4);
                TryLoadPlayerSkin(Geekplay.Instance.PlayerData.PlayerSkin4, Geekplay.Instance.PlayerData.PlayerTexture4);
                break;
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Geekplay.Instance?.PlayerData?.BuildingData?.Count > 0)
            {
                LoadBuildings(Geekplay.Instance.PlayerData.BuildingDataMap1);
                TryLoadPlayerSkin(Geekplay.Instance.PlayerData.PlayerSkin1, Geekplay.Instance.PlayerData.PlayerTexture1);
            }
        }
    }
    public void SaveBuildings()
    {
        for (int i = 0; i < BuildingsOnScene.Count; i++)
        {
            BuildingData.Add(BuildingsOnScene[i].SaveBuilding());
        }
        Geekplay.Instance.PlayerData.BuildingData = new();
        Geekplay.Instance.PlayerData.BuildingData.AddRange(BuildingData);
        Debug.Log("Сохранено объектов: " + Geekplay.Instance.PlayerData.BuildingData.Count);
        Geekplay.Instance.Save();
    }
    private void LoadBuildings(List<SerializedBuildingData> BuildingData)
    {
        StartCoroutine(L(BuildingData));
        Debug.Log("Загружено объектов:" + BuildingData.Count);
    }

    IEnumerator L(List<SerializedBuildingData> BuildingData)
    {
        for (int i = 0; i < BuildingData.Count; i++)
        {

            GameObject LoadedObj = Instantiate(AllPrefabsInGame[BuildingData[i].BuildingIndex]);
            SerializedBuilding SerObj = LoadedObj.GetComponent<SerializedBuilding>();
            if (SerObj != null)
            {
                SerObj.LoadBuilding(BuildingData[i]);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
