using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
   public KeyCode screenShotButton;
void Update()
{
    if (Input.GetKeyDown(screenShotButton))
    {
        StartCoroutine(TakeScreenShot());
    }
}
IEnumerator TakeScreenShot()
{
    yield return new WaitForEndOfFrame();
    ScreenCapture.CaptureScreenshot("screenshot.png", 6);
}
}
