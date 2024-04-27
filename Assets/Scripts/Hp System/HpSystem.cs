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
        if(RootObject != null)
        {
            if (!RootObject.CompareTag("Player"))
            {
               var fx =  Instantiate(DestroyAnimation, RootObject.transform.position, Quaternion.identity);
                fx.transform.parent = null;
                fx.transform.DOScale(RootObject.transform.localScale, 0);
                Debug.Log(RootObject.GetComponent<MeshRenderer>().bounds.size);
                fx.transform.localScale *= 3;
            }
        Destroy(RootObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}