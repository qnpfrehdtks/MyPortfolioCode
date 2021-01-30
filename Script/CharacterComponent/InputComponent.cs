using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    public int fingerID { get; set; } = 0;
    public TouchPhase keyCode { get; set; }
    private CharacterBase m_chracterBase;

    float m_rotateRate;

    Vector3 prePos;

    public void Init(CharacterBase character)
    {
        m_chracterBase = character;
        m_rotateRate = 2.0f;
    }

    void Update()
    {
#if UNITY_EDITOR_WIN // 디버그용ㅋ
        if (m_chracterBase == null) return;
        if (!m_chracterBase.m_isMyCharacter) return;

        Vector2 touchPos;

        if (Input.GetMouseButton(0))
        {
            touchPos = Input.mousePosition - prePos;
            m_chracterBase.transform.Rotate(new Vector3(0, touchPos.x, 0) * m_rotateRate * 5 * Time.deltaTime);

            prePos = Input.mousePosition;
            
        }

#elif !UNITY_EDITOR    
        InputManager.Instance.UpdateInput();
#endif
    }

    Vector2 preTouchPosition;
    public void OnMoved(Touch touch)
    {
        if (m_chracterBase == null) return;

        Vector2 dir = touch.deltaPosition;

        if (dir.sqrMagnitude > 0.0f)
        {
            m_chracterBase.transform.rotation = Quaternion.LookRotation(new Vector3(dir.y, 0.0f, -dir.x));
        }
    }

    public void OnTouchStationary(Touch touch)
    {

    }


    public void OnTouchBegan(Touch touch)
    {
        preTouchPosition = touch.position;
        m_chracterBase.ChangeState(E_CHARACTER_STATE.RUN);
    }

    public void OnTouchEnded(Touch touch)
    {
        m_chracterBase.ChangeState(E_CHARACTER_STATE.IDLE);
    }


}
