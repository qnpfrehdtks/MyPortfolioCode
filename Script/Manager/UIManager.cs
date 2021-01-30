using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    enum Text
    {
        GOLD_TEXT
    }

    enum GoldImage
    {
        GOLD_IMAGE
    }

    public Canvas CurrentUICanvas
    {
        get
        {
            if (m_CurrentSceneUI == null)
                return null;

            if(m_CurrentSceneUICanvas == null)
            {
                m_CurrentSceneUICanvas = m_CurrentSceneUI.GetComponent<Canvas>();
                return m_CurrentSceneUICanvas;
            }
            else
            {
                return m_CurrentSceneUICanvas;
            }
        }
    }

    public RectTransform CanvsRectTransform
    {
        get
        {
            if (CurrentUICanvas == null)
                return null;

            if (m_CurrentRectTr == null)
            {
                m_CurrentRectTr = CurrentUICanvas.GetComponent<RectTransform>();
                return m_CurrentRectTr;
            }
            else
            {
                return m_CurrentRectTr;
            }
        }
    }


    public int m_currentLayerOrder { private set; get; } = 0;

    Stack<UI_Popup> m_StackUI = new Stack<UI_Popup>();

    public UI_Base m_CurrentSceneUI;
     Canvas m_CurrentSceneUICanvas;
    RectTransform m_CurrentRectTr;
    
    E_SCENE_UI_TYPE m_sceneType = E_SCENE_UI_TYPE.NONE;
    GameObject m_HUDTextPrefab;
    GameObject m_TooltipBoxPrefab;
    GameObject m_HealthPrefab;
    GameObject m_BaseSlotPrefab;
    UI_DamagePopup m_InfoText;

    public Camera UICamara { get; private set; }
    public override void InitializeManager()
    {
        GameObject uiCamaraPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/HUDCamera");
        GameObject uiCamera = ResourceManager.Instance.instantiate(uiCamaraPrefab);
        UICamara = uiCamera.GetComponent<Camera>();
        UICamara.transform.SetParent(transform);

        m_HealthPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/HPBar");
        PoolingManager.Instance.CreatePool(m_HealthPrefab.gameObject);

        m_HUDTextPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/HUD");
        PoolingManager.Instance.CreatePool(m_HUDTextPrefab.gameObject);

        m_TooltipBoxPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/ToolTipBox");
        PoolingManager.Instance.CreatePool(m_TooltipBoxPrefab.gameObject);

        m_BaseSlotPrefab = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/BaseSlot");
        PoolingManager.Instance.CreatePool(m_BaseSlotPrefab.gameObject);

        m_InfoText = PoolingManager.Instance.PopFromPool(m_HUDTextPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<UI_DamagePopup>();
        m_InfoText.GetComponent<Animator>().speed = 0.0f;
        m_InfoText.gameObject.SetActive(false);
    }

    public UI_HealthBar AttachHealthBarToObject(GameObject target, Vector3 offset, TextBarStyle style)
    {
        if (CurrentUICanvas == null) return null;

        GameObject go = PoolingManager.Instance.PopFromPool(m_HealthPrefab,Vector3.zero, Quaternion.identity);
        UI_HealthBar healthBar = Common.GetOrAddComponent<UI_HealthBar>(go);

        if (healthBar == null) return null;
        
        healthBar.transform.SetParent(CurrentUICanvas.transform);
        healthBar.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
        healthBar.Init(target, offset, style, CurrentUICanvas);


        return healthBar;
    }

    UI_Tooltipbox currentTooltipBox;

    /// <summary>
    /// Damage
    /// </summary>
    /// <param name="str"></param>
    public void ShowToolTipBox( Vector3 pos, string str, string titleStr)
    {
        Vector2 screenPos;

        if (currentTooltipBox == null)
        {
            currentTooltipBox = PoolingManager.Instance.PopFromPool(m_TooltipBoxPrefab.gameObject, pos, Quaternion.identity).GetComponent<UI_Tooltipbox>();
            currentTooltipBox.transform.SetParent(CurrentUICanvas.transform);
            currentTooltipBox.transform.localRotation = Quaternion.identity;
            currentTooltipBox.transform.localScale = Vector3.one * 0.75f;
            //pos -= new Vector3( currentTooltipBox.GetComponent<RectTransform>().rect.width * 0.5f, currentTooltipBox.GetComponent<RectTransform>().rect.height * 0.5f, 0);
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvsRectTransform, pos, UICamara, out screenPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환
        currentTooltipBox.transform.localPosition = screenPos - new Vector2(currentTooltipBox.GetComponent<RectTransform>().rect.width, -currentTooltipBox.GetComponent<RectTransform>().rect.height) * currentTooltipBox.transform.localScale.x * 0.3f;
        currentTooltipBox.ShowTooptipBox(titleStr, str);
    }

    public UI_ItemSlot CreateSlot<T>() 
    {
        UI_ItemSlot slot = PoolingManager.Instance.PopFromPool(m_BaseSlotPrefab.gameObject, Vector3.zero, Quaternion.identity).GetComponent<UI_ItemSlot>();
        return slot;
    }

    public void OffToolTipBox()
    {
        if (currentTooltipBox != null)
            PoolingManager.Instance.PushToPool(currentTooltipBox.gameObject);

        currentTooltipBox = null;
    }

    /// <summary>
    /// Damage
    /// </summary>
    /// <param name="str"></param>
    public void ShowDamagePopUp( Transform target, Vector3 offset, string str, Color color, float scale ,int textSize = 12, float radius = 50.0f)
    {
        var localPos = Vector2.zero;

        Vector3 RandomRadius = (Random.insideUnitSphere) * radius;
        RandomRadius.z = 0.0f;
        RandomRadius.y = 0.0f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset + RandomRadius);

        if (screenPos.z < 0.0f)
        {
            screenPos *= -1.0f;
        }

        UI_DamagePopup dpu = PoolingManager.Instance.PopFromPool(m_HUDTextPrefab.gameObject, screenPos, Quaternion.identity).GetComponent<UI_DamagePopup>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvsRectTransform, screenPos, UICamara, out localPos); // 스크린 좌표를 다시 체력바 UI 캔버스 좌표로 변환

        dpu.transform.SetParent(CurrentUICanvas.transform);
        dpu.transform.localRotation = Quaternion.identity;
        dpu.transform.localPosition = localPos;
        dpu.transform.localScale = Vector3.one * scale;

        dpu.SetTarget(target, offset);
        dpu.SetColor(color);
        dpu.SetText(str ,1 , textSize);
    }

    public void ShowGameInfoText(Vector3 pos, string str, Color color)
    {
        Vector3 newPos = Camera.main.WorldToScreenPoint(pos);
        m_InfoText.transform.SetParent(m_CurrentSceneUI.transform);

        m_InfoText.transform.position = newPos;
        m_InfoText.transform.rotation = Quaternion.identity;

        m_InfoText.gameObject.SetActive(true);
        m_InfoText.SetText(str, 0.4f, 16);
        m_InfoText.SetColor(color);
    }

    public void HideHUDText()
    {
        if(m_InfoText)
        {
            m_InfoText.transform.SetParent(null);
            m_InfoText.SetText("");
            m_InfoText.gameObject.SetActive(false);
        }
    }

    public void OnChangeGoldUI(int earnedGold)
    {
        if (m_CurrentSceneUI == null)
            return;

        var GoldText = m_CurrentSceneUI.GetText((int)Text.GOLD_TEXT);

        if (GoldText != null)
        {
            int gold = int.Parse(GoldText.text);
            gold += earnedGold;
            GoldText.text = gold.ToString();
        }
    }

    public void CloseSceneUI() 
    {
       if(m_CurrentSceneUI != null)
       {
           ResourceManager.Instance.DestroyObject(m_CurrentSceneUI.gameObject);
       }
        m_CurrentSceneUI = null;
    }

    public void ShowUIScene(E_SCENE_UI_TYPE type)
    {
        if(m_CurrentSceneUI != null && m_sceneType == type)
        {
            return;
        }

        m_sceneType = type;

        CloseSceneUI();
        HideHUDText();

        GameObject go = ResourceManager.Instance.instantiate<GameObject>("Prefabs/UI/Scene/" + type.ToString());
        m_CurrentSceneUI = go.GetComponentInChildren<UI_Base>();

        if (m_CurrentSceneUI != null)
        {
            Canvas canvas = m_CurrentSceneUI.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = UICamara;
            canvas.planeDistance = 1.0f;

            m_CurrentSceneUI.OnShowUp();
        }

    }

    public void ShowPopUp(string popName, string noticeStr, string str ,System.Action<PointerEventData> okAction, System.Action<PointerEventData> cancleAction)
    {
        GameObject popUpgo = ResourceManager.Instance.instantiate<GameObject>($"Prefabs/UI/Popup/{popName}");

        if(popUpgo == null)
        {
            return;
        }

        UI_Popup popup = Common.GetOrAddComponent<UI_Popup>(popUpgo);

        Canvas canvas = Common.GetOrAddComponent<Canvas>(popUpgo);
        canvas.sortingOrder = ++m_currentLayerOrder;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = UICamara;
        canvas.overrideSorting = true;
        canvas.planeDistance = 1.0f;

        popup.InitPopUp(noticeStr, str, okAction, cancleAction);

        m_StackUI.Push(popup);
        return;
    }

    public void ClosePopUp()
    {
        if (m_StackUI.Count == 0)
            return;

        UI_Popup popUp = m_StackUI.Pop();
        ResourceManager.Instance.DestroyObject(popUp.gameObject);

        if (m_StackUI.Count == 0)
        {
            m_currentLayerOrder = 0;
        }
        else
        {
            UI_Popup peekUI = m_StackUI.Peek();
            Canvas canvas = Common.GetOrAddComponent<Canvas>(peekUI.gameObject);

            m_currentLayerOrder = canvas.sortingOrder;
        }
    }
}
