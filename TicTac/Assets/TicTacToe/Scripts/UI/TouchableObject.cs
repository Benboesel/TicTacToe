using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TouchableObject : InteractiveObject
{
    public Action<Finger> OnTouching;
    public Action<Finger> OnTouchUp;
    public Action<Finger> OnTouchDown;
    public bool IsTouching { get; private set; }
    public List<Finger> activeHands = new List<Finger>();

    public virtual void TouchDown(Finger hand)
    {
        if(!activeHands.Contains(hand))
        {
            if(activeHands.Count == 0)
            {
                IsTouching = true;
                if(OnTouchDown != null)
                {
                    OnTouchDown(hand);
                }
            }
            activeHands.Add(hand);
        }
    }

    public virtual void TouchUp(Finger hand)
    {
        if(activeHands.Contains(hand))
        {
            activeHands.Remove(hand);
            if(activeHands.Count == 0)
            {
                IsTouching = false;
                if(OnTouchUp != null)
                {
                    OnTouchUp(hand);
                }
            }
        }
    }

    public virtual void Touching(Finger hand)
    {
        Finger primaryHand = activeHands[activeHands.Count - 1];
        if(hand == primaryHand && OnTouching != null)
        {
            OnTouching(hand);
        }
    }

}
