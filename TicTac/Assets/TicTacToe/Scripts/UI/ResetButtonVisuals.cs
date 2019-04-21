using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonVisuals : MonoBehaviour
{
    [SerializeField] private TouchableButton button;

    public void Awake()
    {
        button.OnButtonHeld += ButtonHeld;
        button.OnButtonDown += ButtonDown;
        button.OnButtonUp += ButtonUp;
    }

    public void ButtonHeld(float amount)
    {
        Vector3 scale = transform.localScale;
        scale.z = Mathf.Clamp((1 - amount), .005f, 1);
        transform.localScale = scale;
    }

    public void ButtonUp()
    {
        Vector3 scale = transform.localScale;
        scale.z = 1;
        transform.localScale = scale;

    }

    public void ButtonDown()
    {

    }

}
