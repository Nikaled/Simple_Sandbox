using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizationTutorial : MonoBehaviour
{
    [Header("World Canvas")]
    [SerializeField] TextMeshProUGUI Phrase1;
    [SerializeField] TextMeshProUGUI Phrase2;
    [SerializeField] TextMeshProUGUI Phrase3;
    [SerializeField] TextMeshProUGUI Phrase4;
    [SerializeField] TextMeshProUGUI Phrase5;
    [SerializeField] TextMeshProUGUI Phrase6;
    [SerializeField] TextMeshProUGUI PhraseRotateDeleting;
    [Header("EndTutorialUI")]
    [SerializeField] TextMeshProUGUI EndAsk;
    [SerializeField] TextMeshProUGUI Decline;
    [SerializeField] TextMeshProUGUI Confirm;
    private void Start()
    {
        if (Geekplay.Instance.mobile)
        {
            if (Geekplay.Instance.language == "ru")
            {
                RuMobileLocalization();
            }
            if (Geekplay.Instance.language == "en")
            {
                EnMobileLocalization();
            }
            if (Geekplay.Instance.language == "tr")
            {
                TrMobileLocalization();
            }
        }
        else
        {
            if (Geekplay.Instance.language == "ru")
            {
                RuPCLocalization();
            }
            if (Geekplay.Instance.language == "en")
            {
                EnPCLocalization();
            }
            if (Geekplay.Instance.language == "tr")
            {
                TrPCLocalization();
            }
        }
    }
    private void RuPCLocalization()
    {
        Phrase1.text = "��� ��������� �������� ��������� ������� <color=orange>[B]</color>";
        Phrase2.text = "������� ����� <color=orange> 1-5 </color> ����� ������� ������ � <color=orange> [���] </color> ����� ������������!";
        Phrase3.text = "�������� �������� � ������� <color=orange> [B] </color> � ������ ��!";
        Phrase4.text = "������ � ������ � �������� ���!";
        Phrase5.text = "� �������� ���� ���� ���� �����, �������� � ������ ������! ������!";
        Phrase6.text = "����� ����, ����� ��������� ��������!";
        PhraseRotateDeleting.text = "�� ������ �������� ����������� ������, ���� ������� <color=orange>[M]</color> � �������� �� ����!";
        EndAsk.text = "������ ��������� ��������?";
        Decline.text = "���";
        Confirm.text = "��";
    }
    private void RuMobileLocalization()
    {
        Phrase1.text = "��� ��������� �������� ��������� ������� <color=orange>[B]</color>";
        Phrase2.text = "������� ����� <color=orange> 1-5 </color> ����� ������� ������ � <color=orange> [���] </color> ����� ������������!";
        Phrase3.text = "�������� �������� � ������� <color=orange> [B] </color> � ������ ��!";
        Phrase4.text = "������ � ������ � �������� ���!";
        Phrase5.text = "� �������� ���� ���� ���� �����, �������� � ������ ������! ������!";
        Phrase6.text = "����� ����, ����� ��������� ��������!";
        PhraseRotateDeleting.text = "�� ������ �������� ����������� ������, ���� ������� <color=orange>[M]</color> � �������� �� ����!";
        EndAsk.text = "������ ��������� ��������?";
        Decline.text = "���";
        Confirm.text = "��";
    }
    private void EnPCLocalization() { }
    private void EnMobileLocalization() { }
    private void TrPCLocalization() { }
    private void TrMobileLocalization() { }
}