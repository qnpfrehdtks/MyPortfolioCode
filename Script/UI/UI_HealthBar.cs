using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealthBar : MonoBehaviour , IPoolingObject
{
    public UI_ProgressBar m_UIProgressBar { get; private set;  }
    public bool IsInPool { get; set; } = false;
    public int StartCount { get; set; } = 20;

    CharacterBase m_Target;
    RectTransform m_RectTr;
    Vector3 m_Offset;
    RectTransform m_parentCanvasTr;

    void Awake()
    {
        m_UIProgressBar = Common.GetOrAddComponent<UI_ProgressBar>(gameObject);
        m_RectTr = GetComponent<RectTransform>();
    }

    public void Init(GameObject target, Vector3 offset, TextBarStyle style, Canvas parentCanvas)
    {
        m_Offset = offset;
        m_Target = target.GetComponent<CharacterBase>();
        m_UIProgressBar.Init(m_Target.Health, m_Target.Stat.MaxHP, style);

        if(parentCanvas)
            m_parentCanvasTr = parentCanvas.GetComponent<RectTransform>();

    }

    private void LateUpdate()
    {
        if (m_Target == null || m_parentCanvasTr == null) return;

        var screenPos = Camera.main.WorldToScreenPoint(m_Target.transform.position + m_Offset); // 몬스터의 월드 3d좌표를 스크린좌표로 변환

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(m_parentCanvasTr, screenPos, UIManager.Instance.UICamara, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

        m_RectTr.localPosition = localPos; // 체력바 위치조정
    }

    public void Initialize(GameObject factory) { }
    public void OnPushToQueue() { }
    public void OnPopFromQueue() { }


}
