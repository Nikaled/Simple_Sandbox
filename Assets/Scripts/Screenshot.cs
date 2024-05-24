using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Screenshot : MonoBehaviour
{
    [DllImport("__Internal")]
	private static extern void DownloadFile(byte[] array, int byteLength, string fileName);

    public void DownloadScreenshot ()
    {
        StartCoroutine(ScreenDo());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DownloadScreenshot();
        }
    }
    IEnumerator ScreenDo()
    {
        //Выключаем весь UI

        CanvasManager.instance.ShowAllUI(false);
        yield return new WaitForEndOfFrame();

        //ДЕЛАЕМ СКРИН НА ВЕБЕ
        var texture = ScreenCapture.CaptureScreenshotAsTexture();
		byte[] textureBytes = texture.EncodeToPNG();
        DownloadFile(textureBytes, textureBytes.Length, $"screenshot_{Geekplay.Instance.PlayerData.downloadsCount2}.png");
        Destroy(texture);

        //ВКЛЮЧАЕМ UI
        CanvasManager.instance.ShowAllUI(true);

        Geekplay.Instance.PlayerData.downloadsCount2++;
        Geekplay.Instance.Save();
    }
}
