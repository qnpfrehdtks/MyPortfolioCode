using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class DesturctionTile : MonoBehaviour, IPoolingObject
{

    E_TILE_TYPE m_TileType;
    E_TILEDIR m_TileDir;

    public void Init(E_TILE_TYPE type, E_TILEDIR tileDir)
    {
        m_TileDir = tileDir;
        m_TileType = type;
    }
    public bool IsInPool { get; set; } = false;
    public int StartCount { get; set; } = 400;

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
