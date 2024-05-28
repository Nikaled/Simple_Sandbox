using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdWarning : MonoBehaviour
{
    public int TimeToShowWarning;
    public GameObject WarningPanel;
    public TextMeshProUGUI WarningText;
    private void Start()
    {
        StartCoroutine(AwaitAndShowWarningPanel());
        LocalizateText(5);
        Debug.Log("Geekplay.Instance.cashedCursorMode" + Geekplay.Instance.cashedCursorModeSilence);
    }
    private void OnEnable()
    {
        Geekplay.Instance.ShowedAdInEditor += ResumeTime;
    }
    private void OnDisable()
    {
        Geekplay.Instance.ShowedAdInEditor -= ResumeTime;
    }
    private IEnumerator AwaitAndShowWarningPanel()
    {
        yield return new WaitForSeconds(TimeToShowWarning);
        WarningPanel.SetActive(true);
        StartCoroutine(StartTimer());
    }
    private void ResumeTime()
    {
        Time.timeScale = 1;
    }
    private IEnumerator StartTimer()
    {
        Cursor.lockState = CursorLockMode.None;
        Geekplay.Instance.IsAdWarningShowing = true;
        Time.timeScale = 0f;
        int Timer = 5;
        while (Timer != 0)
        {
            LocalizateText(Timer);
            Timer--;
            yield return new WaitForSecondsRealtime(1f);
        }
        Geekplay.Instance.ShowInterstitialAd();
        Geekplay.Instance.IsAdWarningShowing = false;
        Geekplay.Instance.PlayerData.Coins++;
        Geekplay.Instance.Save();
        WarningPanel.SetActive(false);
        StartCoroutine(AwaitAndShowWarningPanel());
#if UNITY_EDITOR
        CanvasManager.instance.CheckActiveUnlockCursorWindows();
#endif
    }

    private void LocalizateText(int Timer)
    {
        if (Geekplay.Instance.language == "ru")
            WarningText.text = $"реклама через: {Timer}";
        if (Geekplay.Instance.language == "en")
            WarningText.text = $"Advertisement in: {Timer}";
        if (Geekplay.Instance.language == "tr")
            WarningText.text = $"Saniye sonra reklam verin: {Timer}";

    }
}