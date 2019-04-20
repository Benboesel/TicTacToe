using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger : MonoBehaviour
{
    public enum HandSide
    {
        Left,
        Right
    }

    public HandSide handSide;
    private GameObject fingerTipParent;
    private Rigidbody rigidBody;

    public void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        transform.parent = null;
        StartCoroutine(StickToFingerTip());
    }

    protected IEnumerator StickToFingerTip()
    {
        OvrAvatar avatar = FindObjectOfType<OvrAvatar>();
        while (fingerTipParent == null)
        {
            if (avatar != null)
            {
                if (handSide == HandSide.Left && avatar.GetHandTransform(OvrAvatar.HandType.Left, OvrAvatar.HandJoint.IndexTip) != null)
                {
                    fingerTipParent = avatar.GetHandTransform(OvrAvatar.HandType.Left, OvrAvatar.HandJoint.IndexTip).gameObject;
                }
                if (handSide == HandSide.Right && avatar.GetHandTransform(OvrAvatar.HandType.Right, OvrAvatar.HandJoint.IndexTip) != null)
                {
                    fingerTipParent = avatar.GetHandTransform(OvrAvatar.HandType.Right, OvrAvatar.HandJoint.IndexTip).gameObject;
                }
                yield return null;
            }
        }
    }

    private void FixedUpdate()
    {
        if(fingerTipParent == null)
        {
            return;
        }
        rigidBody.velocity = (fingerTipParent.transform.position - rigidBody.position) / Time.deltaTime;
        rigidBody.MoveRotation(fingerTipParent.transform.rotation);
    }

    private void OnTriggerStay(Collider other)
    {
        if(IsTouchable(other.transform))
        {
            other.GetComponentInParent<TouchableObject>().Touching(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsTouchable(other.transform))
        {
            other.GetComponentInParent<TouchableObject>().TouchDown(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(IsTouchable(other.transform))
        {
            other.GetComponentInParent<TouchableObject>().TouchUp(this);
        }
    }

    private bool IsTouchable(Transform transform)
    {
        return transform.GetComponentInParent<TouchableObject>() != null;
    }

}
