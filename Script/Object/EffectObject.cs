using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour, IPoolingObject
{
    public bool IsInPool { get; set; } = false;
    public int StartCount { get; set; } = 20;

    ParticleSystem m_ps;
    Coroutine m_currentCoroutine;

    private void Awake()
    {
        m_ps = GetComponent<ParticleSystem>();
    }

    public void Initialize(GameObject factory)
    {
        transform.SetParent(factory.transform);
    }
    public void OnPushToQueue()
    {
        m_ps.Stop();

        if (m_currentCoroutine != null)
        {
            StopCoroutine(m_currentCoroutine);
            m_currentCoroutine = null;
        }

        gameObject.SetActive(false);
    }

    public void OnPopFromQueue()
    {
    }

    public void DestroyEffect(float time = 0.0f)
    {
        if (m_currentCoroutine != null)
        {
            StopCoroutine(m_currentCoroutine);
            m_currentCoroutine = null;
        }

        if (time <= 0.0001f)
        {
            PoolingManager.Instance.PushToPool(gameObject);
            return;
        }

        m_currentCoroutine = StartCoroutine(StartDestroy_C(time));
    }

    IEnumerator StartDestroy_C(float time)
    {
        yield return new WaitForSeconds(time);

        PoolingManager.Instance.PushToPool(gameObject);
    }


}
