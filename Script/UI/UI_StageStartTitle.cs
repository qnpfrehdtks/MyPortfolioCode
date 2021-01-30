using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageStartTitle : MonoBehaviour
{
    TMPro.TextMeshProUGUI m_Text;
    Animator m_Animator;

    private void Awake()
    {
        m_Text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        m_Animator = GetComponent<Animator>();
    }

    public void StartStage(string stageInfo, float time)
    {
        gameObject.SetActive(true);
        m_Text.text = stageInfo;
        m_Animator.SetTrigger("Open");

        StartCoroutine(EndStartStage_C(time));
    }

    IEnumerator EndStartStage_C(float time)
    {
        yield return new WaitForSeconds(time);

        m_Animator.SetTrigger("Close");
        
    }



}
