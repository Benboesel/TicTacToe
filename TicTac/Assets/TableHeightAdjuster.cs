using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableHeightAdjuster : OVRGrabbable
{
    [SerializeField] private Transform table;
    private Vector3 initialPosition;
    public Vector3 grabOffset;

    protected override void Start()
    {
        base.Start();
        initialPosition = transform.localPosition;
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

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        grabOffset = transform.position - hand.transform.position;

        base.GrabBegin(hand, grabPoint);
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(linearVelocity, angularVelocity);
        transform.localPosition = initialPosition;
        transform.localRotation = Quaternion.identity;
    }
}
