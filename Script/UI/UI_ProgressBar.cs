using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TextBarStyle
{
    NONE,
    PERCENT,
    DATA
}

public class UI_ProgressBar : MonoBehaviour
{
    [SerializeField]
    TextBarStyle m_TextStyle;

    Image m_ProgressImage;
    Image m_ProgressBackImage;
    Coroutine m_Corutine;
    TMPro.TextMeshProUGUI m_Text;

    private void Awake()
    {
        m_ProgressBackImage = Common.FindChild<Image>(gameObject, "ProgressBarBack");
        m_ProgressImage = Common.FindChild<Image>(gameObject, "ProgressBar");
        m_Text = Common.FindChild<TMPro.TextMeshProUGUI>(gameObject, "TextBar");
    }

    public void Init(float value, float maxValue, TextBarStyle textStyle)
    {
        m_ProgressImage.fillAmount = value / maxValue;
        this.m_TextStyle = textStyle;
        if (m_ProgressBackImage == null) return;
        m_ProgressBackImage.fillAmount = m_ProgressImage.fillAmount;
    }



    public void Init(int value, int maxValue, TextBarStyle textStyle)
    {


        m_ProgressImage.fillAmount = value / maxValue;

        this.m_TextStyle = textStyle;

        SetText(value, maxValue);
        if (m_ProgressBackImage == null) return;
        m_ProgressBackImage.fillAmount = m_ProgressImage.fillAmount;
    }

    void SetText(int value , int maxValue)
    {
        if (m_Text == null) return;

        switch(m_TextStyle)
        {
            case TextBarStyle.NONE:
                m_Text.text = "";
                break;
            case TextBarStyle.DATA:
                m_Text.text = $"{value} / {maxValue}";
                break;
            case TextBarStyle.PERCENT:
                m_Text.text = $"{(value / (float)maxValue)}";
                break;
        }
    }

    public void SetProgressBar(float currentValue, float maxValue)
    {
        m_ProgressImage.fillAmount = currentValue / (float)maxValue;
        SetText((int)currentValue, (int)maxValue);

        if (m_ProgressBackImage == null) return;
        if (m_Corutine != null)
        {
            StopCoroutine(m_Corutine);
        }

        m_Corutine = StartCoroutine(ProgressBar_C(0.4f));
    }

    public void SetProgressBar(int currentValue, int maxValue)
    {
        m_ProgressImage.fillAmount = currentValue / (float)maxValue;
        SetText(currentValue, maxValue);

        if (m_ProgressBackImage == null) return;
        if (m_Corutine != null)
        {
            StopCoroutine(m_Corutine);
        }

        m_Corutine = StartCoroutine(ProgressBar_C(0.4f));
    }

    IEnumerator ProgressBar_C(float time)
    { 
        yield return new WaitForSeconds(time);

        while(m_ProgressImage.fillAmount < m_ProgressBackImage.fillAmount)
        {
            m_ProgressBackImage.fillAmount -= 0.1f * Time.deltaTime;
            yield return null;
        }

        m_ProgressBackImage.fillAmount = m_ProgressImage.fillAmount;

    }

    public void SetSprite(Sprite sprite)
    {
        if(sprite == null)
        {
            m_ProgressImage.gameObject.SetActive(false);
            return;
        }
        m_ProgressImage.gameObject.SetActive(true);
        m_ProgressImage.sprite = sprite;
    }





}
