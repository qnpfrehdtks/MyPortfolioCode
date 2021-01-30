using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    bool m_isPauseObserve;

    Dictionary<TouchPhase, List<IControllerObserver>> m_DicObservers =
    new Dictionary<TouchPhase, List<IControllerObserver>>();

    public override void InitializeManager()
    {
    }

    public bool UpdateInput()
    {
        if (m_DicObservers.Count <= 0)
            return false;

        if (Input.touchCount == 0) return false;

        for(int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            TouchEvent(touch);
        }

        return false;
    }

    public void AllRemoveObserver()
    {
        m_DicObservers.Clear();
    }

    public void RemoveObserver(TouchPhase touchPhase, IControllerObserver observer)
    {
        List<IControllerObserver> list;

        if (m_DicObservers.TryGetValue(touchPhase, out list))
        {
            list.Remove(observer);
        }
    }

    public void AddObserver(TouchPhase touchPhase, IControllerObserver observer)
    {
        List<IControllerObserver> list;

        if (m_DicObservers.TryGetValue(touchPhase, out list))
        {
            list.Add(observer);
        }
        else
        {
            list = new List<IControllerObserver>();
            list.Add(observer);
            m_DicObservers.Add(touchPhase, list);
        }
    }

    private void TouchEvent(Touch touch)
    {
        List<IControllerObserver> list;
        
        if (m_DicObservers.TryGetValue(touch.phase, out list))
        {
            foreach (var obs in list)
            {
                if (obs.fingerID != touch.fingerId) continue;

                switch(touch.phase)
                {
                    case TouchPhase.Moved:
                        obs.OnMoved(touch);
                        break;
                    case TouchPhase.Began:
                        obs.OnTouchBegan(touch);
                        break;
                    case TouchPhase.Stationary:
                        obs.OnTouchStationary(touch);
                        break;
                    case TouchPhase.Ended:
                        obs.OnTouchEnded(touch);
                        break;
                }
            }
        }
    }


}
