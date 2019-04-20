using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHeightAdjuster : OVRGrabbable
{
    [SerializeField] private Transform table;
    private Vector3 initialPosition;
    public Animator handleAnimator;

    protected override void Start()
    {
        base.Start();
        initialPosition = transform.localPosition;
    }

    public override void OnHoverEnter(OVRGrabber hand)
    {
        base.OnHoverEnter(hand);
        HoverHandle();
    }

    public override void OnHoverExit(OVRGrabber hand)
    {
        base.OnHoverExit(hand);
        if(!isGrabbed)
        {
            UnhoverHandle();
        }
    }

    private void HoverHandle()
    {
        handleAnimator.SetBool("Hovered", true);
    }

    private void UnhoverHandle()
    {
        handleAnimator.SetBool("Hovered", false);
    }
    
    public void Update()
    {
        if(isGrabbed)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.x = table.position.x;
            targetPosition.z = table.position.z;
            table.position = targetPosition;
        }
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        transform.localPosition = initialPosition;
        transform.localRotation = Quaternion.identity;
        if(!IsHovered)
        {
            UnhoverHandle();
        }
    }
}
