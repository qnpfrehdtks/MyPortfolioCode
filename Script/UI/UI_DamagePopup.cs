using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DamagePopup : MonoBehaviour, IPoolingObject
{
    Animator m_Animator;
    TMPro.TextMeshProUGUI m_Text;

    public bool IsInPool { get; set; } = false;
    public int StartCount { get; set; } = 3;

    public void Initialize(GameObject factory)
    {
        m_Text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        m_Animator = GetComponent<Animator>();
    }
    public void OnPushToQueue()
    {

    }

    public void OnPopFromQueue()
    {
        m_Text.text = "";
    }

    public void SetColor(Color color)
    {
        m_Text.color = color;
    }

    public void PushToPool()
    {
        PoolingManager.Instance.PushToPool(gameObject);
    }

    Transform m_TargetTr;
    Vector3 m_Offset;


    void LateUpdate()
    {
        if (m_TargetTr == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(m_TargetTr.position + m_Offset);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        Vector2 localPos = Vector3.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIManager.Instance.CanvsRectTransform, screenPos, 
            UIManager.Instance.UICamara, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

        transform.localPosition = localPos;
    }

    public void SetTarget(Transform tr, Vector3 offset)
    {
        m_TargetTr = tr;
        m_Offset = offset;
    }

    public void SetText(string cnt, float speed = 1.0f, float textSize = 24)
    {
        m_Text.text = cnt;
        m_Text.fontSize = textSize;
        m_Animator.SetTrigger("Up");
        m_Animator.speed = speed;
    }

}
