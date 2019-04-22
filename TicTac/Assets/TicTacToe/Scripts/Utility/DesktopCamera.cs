using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DesktopCamera : MonoBehaviour
{
    [SerializeField] private float FOV;
    private IEnumerator Start()
    {
        Camera cam = GetComponent<Camera>();
        yield return new WaitForEndOfFrame();
        UnityEngine.XR.XRSettings.showDeviceView = false;

    }
}
