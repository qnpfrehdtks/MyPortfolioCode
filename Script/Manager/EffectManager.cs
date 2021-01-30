using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    List<EffectObject> m_listEffectOriginal = new List<EffectObject>();
    public override void InitializeManager()
    {
        string[] enumName = System.Enum.GetNames(typeof(E_EFFECT));

        for (int i = 0; i < enumName.Length; i++)
        {
            string path = "Prefabs/Effects/" + enumName[i];
            if (enumName[i] == "NONE" || enumName[i] == "END") continue;

            GameObject effectOriginal = ResourceManager.Instance.Load<GameObject>(path);

            GameObject go = ResourceManager.Instance.instantiate(effectOriginal, transform);
            EffectObject effect = Common.GetOrAddComponent<EffectObject>(go);
            PoolingManager.Instance.PushToPool(go);
            m_listEffectOriginal.Add(effect);
        }
    }

    public GameObject PlayEffect_AttachToObject(E_EFFECT effect, Vector3 pos, Quaternion quat, Vector3 offset, Transform tr, bool isInstance = true, float DestroyTime = 2.0f)
    {
        GameObject effectObject = PoolingManager.Instance.PopFromPool(effect.ToString(), pos, quat);

        if (effectObject == null)
        {
            return null;
        }

        EffectObject effectObj = Common.GetOrAddComponent<EffectObject>(effectObject);

        if (effectObj != null)
        {
            if (isInstance)
                effectObj.DestroyEffect(DestroyTime);

            if (tr != null)
            {
                effectObj.transform.SetParent(tr);
                effectObj.transform.localPosition = offset;
            }
        }

        return effectObject;
    }

    public GameObject PlayEffect(E_EFFECT effect, Vector3 pos, Quaternion quat, bool isInstance = true, float DestroyTime = 2.0f, Transform tr = null)
    {
        return PlayEffect_AttachToObject(effect, pos, quat, Vector3.zero, tr, isInstance, DestroyTime);
    }

    public void AllStopEffect()
    {
        for (int i = 0; i < m_listEffectOriginal.Count; i++)
        {
            StopEffect(m_listEffectOriginal[i]);
        }
    }

    public void StopEffect(GameObject effect)
    {
        if (effect == null)
        {
            return;
        }

        PoolingManager.Instance.PushToPool(effect);
    }

    public void StopEffect(EffectObject effect)
    {
        if(effect == null)
        {
            return;
        }

        PoolingManager.Instance.PushToPool(effect.gameObject);
    }
}
