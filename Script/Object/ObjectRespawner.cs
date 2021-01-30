using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_PILE_TYPE
{
    ONE,
    THREE,
    SIX,
    TEN,
    TEN2
}

public class ObjectRespawner : MonoBehaviour
{
    [SerializeField]
    E_PILE_TYPE m_Type = E_PILE_TYPE.ONE;

    List<IRespawnObject> m_ListInteractObject = new List<IRespawnObject>();
    List<Vector3> m_ListOffset = new List<Vector3>();

  //  float RegenTime = 5.0f;

    GameObject m_Wood;

    public void Init()
    {
        m_ListOffset.Clear();
        m_ListInteractObject.Clear();
        regenList.Clear();

        switch (m_Type)
        {
            case E_PILE_TYPE.ONE:
                m_ListOffset.Add(Vector3.zero);
                break;
            case E_PILE_TYPE.THREE:
                m_ListOffset.Add(Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * 0.3f);
                m_ListOffset.Add(Vector3.forward * -0.3f);
                break;
            case E_PILE_TYPE.SIX:
                m_ListOffset.Add(Vector3.zero);
                m_ListOffset.Add(Vector3.forward * 0.6f);
                m_ListOffset.Add(Vector3.forward * -0.6f);
                m_ListOffset.Add(Vector3.forward * 0.3f + Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * -0.3f + Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.up * 0.4f);
                break;
            case E_PILE_TYPE.TEN:
                m_ListOffset.Add(Vector3.zero);

                m_ListOffset.Add(Vector3.forward * 0.6f);
                m_ListOffset.Add(Vector3.forward * -0.6f);

                m_ListOffset.Add(Vector3.forward * 1.2f);
                m_ListOffset.Add(Vector3.forward * -1.2f);

                m_ListOffset.Add(Vector3.forward * 1.8f);
                m_ListOffset.Add(Vector3.forward * -1.8f);

                m_ListOffset.Add(Vector3.forward * 0.3f + Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * -0.3f + Vector3.up * 0.2f);

                m_ListOffset.Add(Vector3.forward * 0.9f + Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * -0.9f + Vector3.up * 0.2f);

                m_ListOffset.Add(Vector3.forward * 1.5f + Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * -1.5f + Vector3.up * 0.2f);

                m_ListOffset.Add( Vector3.up * 0.4f);

                m_ListOffset.Add(Vector3.forward * -0.6f + Vector3.up * 0.4f);
                m_ListOffset.Add(Vector3.forward * 0.6f + Vector3.up * 0.4f);

                m_ListOffset.Add(Vector3.up * 0.4f);
                break;
            case E_PILE_TYPE.TEN2:
                m_ListOffset.Add(Vector3.zero );
                m_ListOffset.Add(Vector3.forward * 0.6f );
                m_ListOffset.Add(Vector3.forward * -0.6f);

                m_ListOffset.Add(Vector3.forward * 0.3f + Vector3.up * 0.2f);
                m_ListOffset.Add(Vector3.forward * -0.3f + Vector3.up * 0.2f);

                m_ListOffset.Add(Vector3.zero + Vector3.up * 0.4f);
                m_ListOffset.Add(Vector3.forward * 0.6f + Vector3.up * 0.4f);
                m_ListOffset.Add(Vector3.forward * -0.6f + Vector3.up * 0.4f);

                m_ListOffset.Add(Vector3.forward * 0.3f + Vector3.up * 0.6f);
                m_ListOffset.Add(Vector3.forward * -0.3f + Vector3.up * 0.6f);


                break;
        }

        m_Wood = ResourceManager.Instance.Load<GameObject>("Prefabs/Wood");
        RegenWood();
    }

    public void AllClearObject()
    {
        m_ListOffset.Clear();
        m_ListInteractObject.Clear();
        regenList.Clear();
    }

    public void RegenWood()
    {
        if (m_ListInteractObject.Count == m_ListOffset.Count)
            return;

        for (int i = 0; i < m_ListOffset.Count; i++)
        {   
            GameObject respawnItem = PoolingManager.Instance.PopFromPool(m_Wood, transform.position + m_ListOffset[i], transform.rotation);
            IRespawnObject respawn = respawnItem.GetComponent<IRespawnObject>();

            respawnItem.transform.SetParent(transform);
            respawnItem.transform.localPosition = m_ListOffset[i];

            if (respawn != null)
            {
                respawn.RespawnNumber = m_ListInteractObject.Count;
                respawn.InitializeRespawn(this);

                m_ListInteractObject.Add(respawn);
            }
            else
            {
                Common.LogError("Not have a respawn Item " + respawnItem.name);
            }
        }
    }

    IEnumerator ReGenWood_C(float time)
    {
        yield return new WaitForSeconds(time);

        if (m_ListInteractObject.Count == m_ListOffset.Count)
        {
            regenList.Clear();
            yield break;
        }

        for (int i = 0; i < regenList.Count; i++)
        {
            int idx = regenList[i];

            GameObject respawnItem = PoolingManager.Instance.PopFromPool(m_Wood, transform.position + m_ListOffset[idx], transform.rotation);
            IRespawnObject respawn = respawnItem.GetComponent<IRespawnObject>();

            respawnItem.transform.SetParent(transform);
            respawnItem.transform.localPosition = m_ListOffset[idx];

            if (respawn != null)
            {
                respawn.RespawnNumber = idx;
                respawn.InitializeRespawn(this);

                m_ListInteractObject.Add(respawn);
            }
            else
            {
                Common.LogError("Not have a respawn Item " + respawnItem.name);
            }
        }

        regenList.Clear();

    }

    List<int> regenList = new List<int>();
    Coroutine m_RegenCoroutine;

    public void RemoveWood(IRespawnObject obj)
    {
        if(m_ListInteractObject.Remove(obj))
        {
            regenList.Add(obj.RespawnNumber);

            if (m_ListInteractObject.Count == m_ListOffset.Count)
                return;

            if (m_RegenCoroutine != null)
            {
                StopCoroutine(m_RegenCoroutine);
                m_RegenCoroutine = null;
            }

            StartCoroutine(ReGenWood_C(3.0f));
        }
    }


}
