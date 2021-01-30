using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputJoyStick : UI_Base
{
    RectTransform m_rectBack;
    RectTransform m_rectJoystick;

   
    float m_fRadius;
    float m_fSpeed = 2.0f;
    float m_fSqr = 0f;

    Vector3 m_vecMove;

    Vector2 m_vecNormal;

    bool m_bTouch = false;

    Transform m_CharacterTr;


    void Awake()
    {
        m_rectBack = GetComponent<RectTransform>();
        m_rectJoystick = transform.Find("Joystick").GetComponent<RectTransform>();

        // JoystickBackground의 반지름입니다.
        m_fRadius = m_rectBack.rect.width * 0.5f;

        AddUIEvent(gameObject, OnDrag, E_UIEVENT.DRAG);
  //      AddUIEvent(gameObject, OnPointerDown, E_UIEVENT.BEGIN_DRAG);
        AddUIEvent(gameObject, OnPointerUp, E_UIEVENT.EXIT_DRAG);
    }

    CharacterBase m_character;

    public void Init(Transform tr)
    {
        m_CharacterTr = tr;
        m_character = m_CharacterTr.GetComponent<CharacterBase>();
    }

    //void Update()
    //{
    //    if (m_bTouch && m_CharacterTr)
    //    {
    //        m_CharacterTr.position += m_vecMove;
    //    }

    //}

    void OnTouch(Vector2 vecTouch)
    {
        Vector2 dir = new Vector2(vecTouch.x - m_rectBack.localPosition.x , vecTouch.y - m_rectBack.localPosition.y );

        //// vec값을 m_fRadius 이상이 되지 않도록 합니다.
        dir = Vector2.ClampMagnitude(dir, m_fRadius);
        m_rectJoystick.localPosition = dir;

        if (dir.sqrMagnitude < 0.15f) return;
        if (m_character && m_character.CheckCanControll() == false ) return;
        
        // 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
        float fSqr = (m_rectBack.localPosition - m_rectJoystick.localPosition).sqrMagnitude / (m_fRadius * m_fRadius);

        // 터치위치 정규화
        Vector2 vecNormal = dir.normalized;

        m_vecMove = new Vector3(vecNormal.x * fSqr, 0f, vecNormal.y * fSqr);
       // m_CharacterTr.forward = m_vecMove;
        m_CharacterTr.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
        m_character.ChangeState(E_CHARACTER_STATE.RUN);
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.CurrentUICanvas.GetComponent<RectTransform>(), eventData.position, UIManager.Instance.UICamara, out pos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

        OnTouch(pos);
        m_bTouch = true;
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (m_character && m_character.CheckCanControll())
    //    {
    //        m_character.ChangeState(E_CHARACTER_STATE.RUN);
    //    }
    //}

    public void OnPointerUp(PointerEventData eventData)
    {
        // 원래 위치로 되돌립니다.
        m_rectJoystick.localPosition = Vector2.zero;
        m_bTouch = false;

        if (m_character && m_character.CheckCanControll())
        {
            m_character.ChangeState(E_CHARACTER_STATE.IDLE);
        }

    }
}