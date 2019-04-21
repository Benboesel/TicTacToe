using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButtonVisuals : MonoBehaviour
{
    [SerializeField] private TouchableButton button;
    [SerializeField] private Color PressedColor;
    private Color initialColor;
    [SerializeField] private MeshRenderer meshRenderer;

    public void Awake()
    {
        initialColor = meshRenderer.material.color;
        button.OnButtonHeld += ButtonHeld;
        button.OnButtonDown += ButtonDown;
        button.OnButtonUp += ButtonUp;
    }

    public void ButtonHeld(float amount)
    {
        SetDepth(amount);
    }

    public void ButtonUp()
    {
        SetDepth(0);
    }

    private void SetDepth(float depth)
    {
        meshRenderer.material.color = Color.Lerp(initialColor, PressedColor, depth);
        depth = Mathf.Clamp((1 - depth), .005f, 1);
        Vector3 scale = transform.localScale;
        scale.z = depth;
        transform.localScale = scale;
    }

    public void ButtonDown()
    {

    }

}
