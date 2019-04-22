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
        yield return new WaitForSeconds(0.25f);
        cam.stereoTargetEye = StereoTargetEyeMask.Left;
        yield return new WaitForSeconds(0.25f);
        cam.stereoTargetEye = StereoTargetEyeMask.None;
        cam.fieldOfView = FOV;

    }
}
