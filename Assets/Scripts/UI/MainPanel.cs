using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour {

    private Button startBtn; // 开始按钮
    private Button shopBtn; // 商店按钮
    private Button scoreBtn; // 分数排名
    private Button videoBtn; // 声音设置按钮
    private Button resetBtn; // 充值游戏数据按钮
    private Image m_music; // 声音开启关闭 标识
    private Image m_image;
    private ManagerVars vars;

    private void Awake () {
        vars = ManagerVars.GetManagerVars();
        startBtn = transform.Find("Btn_Start").GetComponent<Button>();
        startBtn.onClick.AddListener(OnStartClick);

        shopBtn = transform.Find("Btns/Btn_Shop").GetComponent<Button>();
        shopBtn.onClick.AddListener(OnShopButtonClick);

        resetBtn = transform.Find("Btns/Btn_Reset").GetComponent<Button>();
        resetBtn.onClick.AddListener(OnResetButtonClick);

        scoreBtn = transform.Find("Btns/Btn_Score").GetComponent<Button>();
        videoBtn = transform.Find("Btns/Btn_Video").GetComponent<Button>();
        videoBtn.onClick.AddListener(OnVideoButtonClick);

        m_image = transform.Find("Btns/Btn_Shop/Image").GetComponent<Image>();
        m_music = transform.Find("Btns/Btn_Video/Image").GetComponent<Image>();
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, UpdateSprite);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, UpdateSprite);
    }

    /// <summary>
    /// 重置数据按钮
    /// </summary>
    private void OnResetButtonClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        EventCenter.Broadcast(EventDefine.ResetPanel);
    }

    /// <summary>
    /// 更新角色皮肤
    /// </summary>
    /// <param name="index"></param>
    private void UpdateSprite(int index) {
        m_image.sprite = vars.scrollSpriteList[index];
    }

    /// <summary>
    /// 点击声音设置按钮
    /// </summary>
    private void OnVideoButtonClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        bool isMusicOn = GameManager.Instance.GetIsMusicOn();
        if (isMusicOn)
        {
            m_music.sprite = vars.musicOff;
        }
        else {
            m_music.sprite = vars.musicOn;
        }
        GameManager.Instance.SetIsMusicOn(!isMusicOn);
    }


    /// <summary>
    /// 打开商店
    /// </summary>
    private void OnShopButtonClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        EventCenter.Broadcast(EventDefine.ShowShopPanel);
    }

    /// <summary>
    /// 点击再来一次
    /// </summary>
    private void Start()
    {
        if (GameData.IsAgainGame) { OnStartClick(); }
        UpdateSprite(GameManager.Instance.GetSelectSkin());
    }

    /// <summary>
    /// 开始游戏 按钮点击
    /// </summary>
    private void OnStartClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        GameManager.Instance.IsGameStart = true;
        EventCenter.Broadcast(EventDefine.ShowGamePanel);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
