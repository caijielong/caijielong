using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShopPanel : MonoBehaviour {

    private Transform parent; // 滚动条子物体目录
    private ManagerVars vars;
    private float x_; // 滚动条子物体的长
    private Text name_; // 人物名称
    private Text m_diamond; // 钻石拥有数量
    private Button m_back; // 返回按钮
    private Text m_itemDiamond; // 当前角色所需钻石数
    private int currentIndex = 0; // 当前所选的第几个角色
    private Button btnBuy; // 购买按钮
    private Button btnUse; // 使用按钮

    private Transform m_Btn_Buy,m_Btn_Use;

    private void Awake()
    {
        m_Btn_Buy = transform.Find("Btn_Buy");
        m_Btn_Use = transform.Find("Btn_Use");

        vars = ManagerVars.GetManagerVars();
        parent = transform.Find("ScrollRect/Parent");
        name_ = transform.Find("Text_Name").GetComponent<Text>();
        m_diamond = transform.Find("Obj_Diamond/Text_Diamond").GetComponent<Text>();
        m_back = transform.Find("Btn_Back").GetComponent<Button>();
        m_back.onClick.AddListener(BackButtonClick);
        m_itemDiamond = transform.Find("Btn_Buy/Text").GetComponent<Text>();
        btnBuy = m_Btn_Buy.GetComponent<Button>();
        btnUse = m_Btn_Use.GetComponent<Button>();
        btnBuy.onClick.AddListener(OnBuyButtonClick);
        btnUse.onClick.AddListener(OnUseButtonClick);
        x_ = vars.scrollItem.transform.GetComponent<RectTransform>().sizeDelta.x;      

        EventCenter.AddListener(EventDefine.ShowShopPanel, SetSelf);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        InitScroll();
    }

    /// <summary>
    /// 初始化滚动条
    /// </summary>
    private void InitScroll()
    {
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2((vars.scrollSpriteList.Count + 2) * x_, 0);

        for (int i = 0; i < vars.scrollSpriteList.Count; i++)
        {
            GameObject go = Instantiate(vars.scrollItem, parent);
            go.GetComponentInChildren<Image>().sprite = vars.scrollSpriteList[i];
            go.transform.localPosition = new Vector3((i + 1) * x_, 0, 0);

            // 根据角色是否已经购买 显示颜色
            if (GameManager.Instance.GetSkinUnlock(i))
            {
                go.GetComponentInChildren<Image>().color = Color.white;
            }
            else
            {
                go.GetComponentInChildren<Image>().color = Color.gray;
            }
        }
        parent.transform.localPosition = new Vector3(GameManager.Instance.GetSelectSkin() * -160, 0);
    }

    private void Update()
    {
        currentIndex = (int)Mathf.Round(parent.localPosition.x / -160);
        SetItem(currentIndex);
        if (Input.GetMouseButtonUp(0))
        {
            parent.DOLocalMoveX(currentIndex * -x_, 0.2f);
            //parent.localPosition = new Vector3(currentIndex * -x_,0,0);           
        }
    }

    private void ActiveBuyUse() {
        if (GameManager.Instance.GetSkinUnlock(currentIndex))
        {
            m_Btn_Buy.gameObject.SetActive(false);
            m_Btn_Use.gameObject.SetActive(true);
        }
        else {
            m_Btn_Buy.gameObject.SetActive(true);
            m_Btn_Use.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 购买角色按钮
    /// </summary>
    private void OnBuyButtonClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        int nowDiamondCount = GameManager.Instance.GetDiamondCount();
        if (GameManager.Instance.GetDiamondCount() < vars.itemDiamond[currentIndex] || GameManager.Instance.GetSkinUnlock(currentIndex)) {
            //TODO 钻石数不足提示或者已经购买
            EventCenter.Broadcast<string>(EventDefine.ShowHint, "钻石不足");
            return;
        }
            
        GameManager.Instance.SetDiamondCount(-vars.itemDiamond[currentIndex]);
        GameManager.Instance.SetSkinUnlock(currentIndex);
        parent.GetChild(currentIndex).GetComponentInChildren<Image>().color = Color.white;
        m_Btn_Buy.gameObject.SetActive(false);
        m_Btn_Use.gameObject.SetActive(true);
    }

    /// <summary>
    /// 使用角色按钮
    /// </summary>
    private void OnUseButtonClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        GameManager.Instance.SetSelectSkin(currentIndex);
        BackButtonClick();
        EventCenter.Broadcast<int>(EventDefine.ChangeSkin, currentIndex);
    }

    /// <summary>
    /// 返回按钮点击
    /// </summary>
    private void BackButtonClick() {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示 商品页
    /// </summary>
    private void SetSelf() {
        gameObject.SetActive(true);
        m_diamond.text = GameManager.Instance.GetDiamondCount().ToString();

    }

    /// <summary>
    /// 人物列表
    /// </summary>
    /// <param name="index"></param>
    private void SetItem(int index) {
        for (int i = 0; i < parent.childCount; i++) {
            if(i == index) {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().DOSizeDelta(new Vector2(x_, x_), 0.15f);
            } else
            {
                parent.GetChild(i).GetChild(0).GetComponent<RectTransform>().DOSizeDelta(new Vector2(x_/2, x_/2), 0.15f);
            }           
        }

        name_.text = vars.itemName[index];
        m_itemDiamond.text = vars.itemDiamond[index].ToString();
        ActiveBuyUse();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowShopPanel, SetSelf);
    }
}
