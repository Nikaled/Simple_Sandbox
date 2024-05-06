using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HpSystem : MonoBehaviour
{
    [SerializeField] GameObject RootObject;
    [SerializeField] GameObject MeshObject;
    [SerializeField] Image HpBar;
    [SerializeField] private int MaxHp;
    [SerializeField] TextMeshProUGUI HpText;
    [SerializeField] GameObject DestroyAnimation;
    [SerializeField]  public int CurrentHP
    {
        get { return _currentHP; }
        set
        {
            _currentHP = value;
            HpBar.fillAmount = (float)_currentHP / MaxHp;
            HpText.text = $"{_currentHP} / {MaxHp}";
        }
    }
    private int _currentHP;
    public event Action OnDied;
    private void Start()
    {
        CurrentHP = MaxHp;
        HpBar.gameObject.SetActive(false);
    }

    public void TakeDamage(int DamageCount)
    {
        if (DamageCount < 0)
        {
            Debug.Log("Ќанесено отрицательное количество урона");
            return;
        }
        CurrentHP -= DamageCount;
        HpBar.gameObject.SetActive(true);
        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }
        if(CurrentHP == 0)
        {
            ObjectDies();
        }

    }
    private void ObjectDies()
    {
        OnDied?.Invoke();
        if(MeshObject != null)
        {
            if (!MeshObject.CompareTag("Player"))
            {
                Explosion(MeshObject);
            }
        Destroy(RootObject);
        }
        else
        {
            MeshObject = RootObject;
            Explosion(RootObject);
            if(RootObject !=null)
            Destroy(RootObject);
        }
    }
    private void Explosion(GameObject rootObject)
    {
        if (RootObject == null)
        {
            GameObject parent = this.transform.parent.gameObject;
            if (parent.GetComponent<HpSystemCollision>() != null)
            {
                RootObject = parent;
                rootObject = parent;
            }
        }
            if (rootObject.GetComponent<MeshRenderer>() == null)
            {
                return;
            }
        Vector3 bounds = rootObject.GetComponent<MeshRenderer>().bounds.size;
        float SumOfSides = FindSumOfSides(bounds) / 9 ;
        Vector3 ExplosionPos = new Vector3(HpBar.transform.position.x, HpBar.transform.position.y-bounds.y, HpBar.transform.position.z);
        var fx = Instantiate(DestroyAnimation, ExplosionPos, Quaternion.identity);
        fx.transform.parent = null;
        fx.transform.DOScale(DestroyAnimation.transform.localScale *SumOfSides, 0);
    }
    private float FindSumOfSides(Vector3 bounds)
    {
        float[] Vectors = new float[] { bounds.x, bounds.y, bounds.z };
        float largerSide = 0;
        for (int i = 0; i < Vectors.Length; i++)
        {
            largerSide+= Vectors[i];
        }
        return largerSide;
    }
}