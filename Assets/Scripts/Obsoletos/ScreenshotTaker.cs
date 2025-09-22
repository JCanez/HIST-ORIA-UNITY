using System;
using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    string filename;
    public int superSize = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            TakeScreenshot();
    }

    private void TakeScreenshot()
    {
        filename = "Screenshots/" + Time.deltaTime + ".png";
        ScreenCapture.CaptureScreenshot(filename, superSize);
        Debug.Log("Capturado");
    }
}
