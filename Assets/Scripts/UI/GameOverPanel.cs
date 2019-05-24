using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour {

    private Text maxScore;
    private Text score;
    private Text diamondNum;
    private Button again;
    private Button home;
    private Button rank;

    private Transform news;

    private void Awake()
    {
        news = transform.Find("Img_New");
        news.gameObject.SetActive(false);
        score = transform.Find("Text_Score").GetComponent<Text>();
        maxScore = transform.Find("Obj_MaxScore/Text_MaxScore").GetComponent<Text>();
        diamondNum = transform.Find("Obj_DiamondNum/Text_DiamondNum").GetComponent<Text>();

        again = transform.Find("Text_Btn_Again").GetComponent<Button>();
        again.onClick.AddListener(AgainClick);
        home = transform.Find("Img_Btn_Home").GetComponent<Button>();
        home.onClick.AddListener(HomeClick);
        rank = transform.Find("Img_Btn_Rank").GetComponent<Button>();
        rank.onClick.AddListener(RankClick);

        EventCenter.AddListener(EventDefine.ShowGameOverPanel, ShowSelf);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 游戏结束 展示
    /// </summary>
    private void ShowSelf()
    {
        if (GameManager.Instance.GetGameScore() >= GameManager.Instance.GetBestScore()) {
            news.gameObject.SetActive(true);
        }
        maxScore.text = GameManager.Instance.GetBestScore().ToString();
        score.text = GameManager.Instance.GetGameScore().ToString();
        diamondNum.text = GameManager.Instance.GetDiamondCount().ToString();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 返回首页
    /// </summary>
    private void HomeClick()
    {
        // 再次加载当前的场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = false;
    }

    /// <summary>
    /// 排名按钮点击
    /// </summary>
    private void RankClick()
    {

    }

    /// <summary>
    /// 点击再来一次 事件
    /// </summary>
    private void AgainClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameData.IsAgainGame = true;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowGameOverPanel, ShowSelf);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
