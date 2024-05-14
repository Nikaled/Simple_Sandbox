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
        VariantNames = new string[] {"Вариант 1", "Вариант 2", "Вариант 3", "Вариант 4", "Вариант 5"};
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
            objectVariants.FindTextureIndex();
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
