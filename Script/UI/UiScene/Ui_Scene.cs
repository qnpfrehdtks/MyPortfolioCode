using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Scene : UI_Base
{
    protected UnityEngine.EventSystems.EventSystem m_myEventSystem;

    public override void Initialize(GameObject factory)
    {
        base.Initialize(factory);

        m_myEventSystem = UnityEngine.EventSystems.EventSystem.current;
    }

    public override void OnPopFromQueue()
    {
        base.OnPopFromQueue();
       
    }
}
