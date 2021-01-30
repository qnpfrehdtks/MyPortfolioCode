﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour, IInteractableObject, IPoolingObject, IRespawnObject
{
    public bool IsInPool { get; set; } = false;
    public int RespawnNumber { get; set; }
    public int StartCount { get; set; } = 30;

    BoxCollider m_BoxCollider;
    MeshRenderer m_MeshRenderer;
    CharacterBase m_ownedCharacter;

    Rigidbody m_RB;

    ObjectRespawner m_MyRespawner;

    Shader m_TrShader;
    Shader m_OriginShader;

    Animator m_Animator;

    void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_BoxCollider = GetComponentInChildren<BoxCollider>();
        m_RB = GetComponentInChildren<Rigidbody>();
        m_TrShader = Shader.Find("Custom/ToonShader_Tr");
        m_OriginShader = Shader.Find("Custom/ToonShader");
    }

    public void InitializeRespawn(ObjectRespawner respawner)
    {
        m_MyRespawner = respawner;
    }

    public void OnRespawn(int idx)
    {
        RespawnNumber = idx;
    }

    public void OnEnterCollision(ICombatEntity character, Vector3 hitPos)
    {
       
    }

    public void OnExitCollision(ICombatEntity character)
    {

    }

    public void Initialize(GameObject factory)
    {
        transform.SetParent(factory.transform);
    }

    public void OnPushToQueue()
    {
        gameObject.SetActive(false);

        m_RB.isKinematic = true;
        m_RB.useGravity = false;

        m_BoxCollider.enabled = false;
    }

    public void OnPopFromQueue()
    {
        gameObject.SetActive(true);

        m_RB.isKinematic = true;
        m_RB.useGravity = false;

        m_BoxCollider.enabled = true;
        SetColor(new Color(0.6f,0.4f,0));

        
        gameObject.layer = LayerMask.NameToLayer("Interactive");
    }


    public void SetColor(Color color)
    {
        m_MeshRenderer.material.color = color;
    }

    public void OnPopFromStack()
    {
        transform.position = m_ownedCharacter.transform.position + m_ownedCharacter.transform.forward * 0.1f - transform.up * 0.25f;
        transform.SetParent(null);

        m_BoxCollider.enabled = true;
        ExitCollisionToCameraRay();
        m_Animator.SetTrigger("idle");

   //     EffectManager.Instance.PlayEffect(E_EFFECT.GetItem, Vector3.zero, Quaternion.Euler(-90, 0, 0), true, 2.0f, transform);

        gameObject.layer = LayerMask.NameToLayer("GroundItem");
        
    }

    public void FlyAway()
    {
        if (m_RB == null)
            return;

        if (gameObject.activeSelf == false)
            return;

        transform.SetParent(null);
        m_RB.isKinematic = false;
        m_RB.useGravity = true;

        m_BoxCollider.enabled = true;
        m_RB.velocity += Vector3.up * 10.0f;
    }

    public bool IsFade { get; set; }

    public void EnterCollisionToCameraRay()
    {
        if (m_ownedCharacter && m_ownedCharacter.m_isMyCharacter)
            return;

        if (IsFade)
            return;

        MeshRenderer[] skinRender = GetComponentsInChildren<MeshRenderer>();

        if (skinRender != null)
        {
            foreach (var skin in skinRender)
            {
                skin.material.shader = m_TrShader;

                if (skin.material.HasProperty("_Color"))
                {
                    Color prevColor = skin.material.GetColor("_Color");
                    skin.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 0.4f));
                    skin.material.renderQueue += 1;
                }
            }
        }

        IsFade = true;
    }

    public void ExitCollisionToCameraRay()
    {
        if (m_ownedCharacter && m_ownedCharacter.m_isMyCharacter)
            return;

        if (!IsFade)
            return;

        MeshRenderer[] skinRender = GetComponentsInChildren<MeshRenderer>();

        if (skinRender != null)
        {
            foreach (var skin in skinRender)
            {
                skin.material.shader = m_OriginShader;

                if (skin.material.HasProperty("_Color"))
                {
                    Color prevColor = skin.material.GetColor("_Color");
                    skin.material.SetColor("_Color", new Color(prevColor.r, prevColor.g, prevColor.b, 1.0f));
                    skin.material.renderQueue -= 1;
                }
            }
        }

        IsFade = false;
    }


}
