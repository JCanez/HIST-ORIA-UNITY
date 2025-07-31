using System;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public string filename = "Screenshots/123.png";
    public int superSize = 1;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            TakeScreenshot();
    }

    private void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot(filename, superSize);
        Debug.Log("Capturado");
    }
}
