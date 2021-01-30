using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionComponent : MonoBehaviour, ICameraObstacle, IInteractableObject
{
    CharacterBase m_myCharacter;

    private void Awake()
    {
        m_myCharacter = GetComponent<CharacterBase>();
    }

    public bool IsFade { get; set; }

    public void EnterCollisionToCameraRay()
    {
        if (m_myCharacter == null) return;
        if (m_myCharacter.m_isMyCharacter) return;
        if (IsFade) return;

       // m_myCharacter.OnFade();

        IsFade = true;
    }

    public void ExitCollisionToCameraRay()
    {
        if (m_myCharacter == null) return;
        if (m_myCharacter.m_isMyCharacter) return;
        if (IsFade) return;

        IsFade = false;
       // m_myCharacter.OffFade();
    }

    /// <summary>
    /// 상대방과 충돌시 상대방에게 일으킬 함수,
    /// </summary>
    /// <param name="otherCharacter"></param>
    /// <param name="hitPos"></param>
    public void OnEnterCollision(ICombatEntity otherCharacter, Vector3 hitPos)
    {
        
    }

    public void OnExitCollision(ICombatEntity character)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_myCharacter.gameObject) return;
        if (other.gameObject.layer == (int)E_LAYER.Bullet || other.gameObject.layer == (int)E_LAYER.Character)
        {
            IInteractableObject interactObject = other.GetComponent<IInteractableObject>();
            if (interactObject == null)
            {
                return;
            }

            interactObject.OnEnterCollision(m_myCharacter.m_StatController, m_myCharacter.transform.position);
        }
    }



}
