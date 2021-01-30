using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour, IPoolingObject
{
    public bool IsInPool { get; set; } = false;

    [SerializeField]
    public int Gold;

    public int StartCount { get; set; } = 30;
    public void Initialize(GameObject factory)
    {

    }
    public void OnPushToQueue()
    {

    }
    public void OnPopFromQueue()
    { 
    }

}
