using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchableButton : TouchableObject
{
    protected void OnEnable()
    {
        OnTouchDown += TouchDownEvent;
        OnTouching += TouchingEvent;
    }

    protected void OnDisable()
    {
        OnTouchDown -= TouchDownEvent;
        OnTouching -= TouchingEvent;
    }

    private Vector3 touchOffset;
    public float Value;
    public float MaxButtonDepth = .1f;
    public float LerpBackSpeed = 20f;
    private float ButtonUpThreshold = 0.01f;
    private float ButtonDownThreshold = 0.9f;

    public Action OnButtonUp;
    public Action OnButtonDown;
    public Action<float> OnButtonHeld;
    public bool IsButtonDown = false;

    protected void Update()
    {
        if(!IsTouching)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * LerpBackSpeed);
        }
        SetButtonValue();
    }

    protected virtual void SetButtonValue()
    {
        float currentButtonDepth = transform.localPosition.z;
        float previousValue = Value;
        Value = currentButtonDepth / MaxButtonDepth;
        float valueDelta = Mathf.Abs(Value - previousValue);
        if(valueDelta > 0.001f)
        {
            ButtonHeld();
        }
        if(Value >= ButtonDownThreshold && !IsButtonDown)
        {
            ButtonDown();
        }
        if(Value <= ButtonUpThreshold && IsButtonDown)
        {
            ButtonUp();
        }
    }

    protected virtual void ButtonDown()
    {
        IsButtonDown = true;
        if(OnButtonDown != null)
        {
            OnButtonDown.Invoke();
        }
    }

    protected virtual void ButtonUp()
    {
        IsButtonDown = false;
        if(OnButtonUp != null)
        {
            OnButtonUp.Invoke();
        }
    }

    protected virtual void ButtonHeld()
    {
        if(OnButtonHeld != null)
        {
            OnButtonHeld.Invoke(Value);
        }
    }

    protected virtual void TouchDownEvent(Finger hand)
    {
        touchOffset = hand.transform.position - transform.position;
    }

    protected virtual void TouchingEvent(Finger hand)
    {
        Vector3 targetPosition = transform.parent.InverseTransformPoint(hand.transform.position - touchOffset);
        transform.localPosition = GetClampedPosition(targetPosition);
    }
    
    protected Vector3 GetClampedPosition(Vector3 position)
    {
        position.x = 0;
        position.y = 0;
        position.z = Mathf.Clamp(position.z, 0, MaxButtonDepth);
        return position;
    }
}
