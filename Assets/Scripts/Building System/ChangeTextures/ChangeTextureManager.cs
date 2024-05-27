using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTextureManager : MonoBehaviour
{
    [SerializeField] Button PreviousButton;
    [SerializeField] Button NextButton;
    private ObjectVariants objectVariants;
    public static ChangeTextureManager instance;
    private string[] VariantNames;
    [SerializeField] private TextMeshProUGUI VariantText;
    [SerializeField] private GameObject VariantUI;
    private void Awake()
    {
        instance = this;
        VariantNames = new string[] {"ЦВЕТ 1", "ЦВЕТ 2", "ЦВЕТ 3", "ЦВЕТ 4", "ЦВЕТ 5"};
    }
    public void ChangeVariantText(int index)
    {
        VariantText.text = VariantNames[index];
    }
    public void ButtonsInitialize(GameObject RootObject)
    {
        VariantUI.gameObject.SetActive(true);
        Debug.Log(RootObject.name);
        objectVariants = RootObject.GetComponent<ObjectVariants>();
        if (objectVariants == null)
        {
            objectVariants = RootObject.GetComponentInParent<ObjectVariants>();
        }
        if (objectVariants == null)
        {
        objectVariants =  RootObject.GetComponentInChildren<ObjectVariants>();
        }
        if(objectVariants != null)
        {
            PreviousButton.onClick.AddListener(delegate { objectVariants.ChangeTextures(-1); });
            NextButton.onClick.AddListener(delegate { objectVariants.ChangeTextures(1); });
            ChangeVariantText(objectVariants.FindTextureIndex());
        }
        else
        {
            VariantUI.gameObject.SetActive(false);
        }
    }
    public void ClearButtonListeners()
    {
        PreviousButton.onClick.RemoveAllListeners();
        NextButton.onClick.RemoveAllListeners();
    }
}
