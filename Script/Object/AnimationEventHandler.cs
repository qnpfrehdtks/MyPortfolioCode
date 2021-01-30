using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    System.Action AnimationEvent;

    public void Init()
    {
        AnimationEvent = null;
    }

    public void SetAnimationEvent(System.Action e)
    {
        AnimationEvent = e;
    }

    public void AddAnimationEvent(System.Action e)
    {
        AnimationEvent += e;
    }

   public void CastSkill()
   {
        AnimationEvent?.Invoke();
   }
}
