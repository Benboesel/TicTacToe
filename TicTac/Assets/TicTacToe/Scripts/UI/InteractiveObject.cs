using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    protected virtual void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("InteractiveObject");
    }
}
