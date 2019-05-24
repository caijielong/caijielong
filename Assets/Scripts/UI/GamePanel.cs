using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour {

    private Button pauseBtn; // 暂停按钮
    private Button playBtn; // 继续按钮
    private Text scoreText; // 游戏得分
    private Text diamondText; // 钻石数量

	// Use this for initialization
	private void Awake () {
        pauseBtn = transform.Find("Btns/Img_Pause").GetComponent<Button>();
        pauseBtn.onClick.AddListener(PauseOnClick);

        playBtn = transform.Find("Btns/Img_Play").GetComponent<Button>();
        playBtn.onClick.AddListener(PlayOnClick);
        playBtn.gameObject.SetActive(false);

        scoreText = transform.Find("Text_Score").GetComponent<Text>();
        diamondText = transform.Find("Diamond/Text_DiamondNum").GetComponent<Text>();
        EventCenter.AddListener(EventDefine.ShowGamePanel, ShowGamePanel); // 监听游戏开始事件
        EventCenter.AddListener(EventDefine.AddScore, ShowScore); // 监听游戏得分事件
        EventCenter.AddListener(EventDefine.UpdateDiamondCount, ShowDiamond);
        gameObject.SetActive(false);
    }

    // 更新游戏得分
    private void ShowScore() {
        scoreText.text = GameManager.Instance.GetGameScore().ToString();
    }

    // 更新钻石分数
    private void ShowDiamond()
    {
        diamondText.text = GameManager.Instance.GetDiamondCount().ToString();
    }

    /// <summary>
    /// 点击游戏暂停事件
    /// </summary>
    private void PauseOnClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        pauseBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
        GameManager.Instance.IsPause = true;
        Time.timeScale = 0;        
    }

    /// <summary>
    /// 点击游戏继续事件
    /// </summary>
    private void PlayOnClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        pauseBtn.gameObject.SetActive(true);
        playBtn.gameObject.SetActive(false);
        GameManager.Instance.IsPause = false;
        Time.timeScale = 1;
    }

    private void ShowGamePanel() {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGamePanel, ShowGamePanel);
        EventCenter.RemoveListener(EventDefine.AddScore, ShowScore);
        EventCenter.RemoveListener(EventDefine.UpdateDiamondCount, ShowDiamond);
    }

    // Update is called once per frame
    void Update () {
		      
	}
}
