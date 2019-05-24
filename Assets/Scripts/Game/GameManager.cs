using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class GameManager : MonoBehaviour {

    // 游戏是否开始
    public bool IsGameStart { set; get; }

    // 游戏是否结束
    public bool IsGameOver { set; get; }

    // 游戏是否结束
    public bool IsPause { set; get; }

    // 玩家是否开始移动
    public bool IsPlayerMove { set; get; }

    private ManagerVars vars;
    public static GameManager Instance;
    public GameData data;
    private int gameScore = 0; // 玩家得分

    private bool isFirstGame; // 是否第一次游戏
    private bool isMusicOn; // 是否开启游戏声音
    private int[] bestScoreArr; // 游戏分数排名
    private int selectSkin; // 选择的皮肤
    private bool[] skinUnlockArr; // 皮肤是否解锁
    private int diamondCount; // 钻石数量
    private bool canRead_; // 是否读取的到游戏存档数据

    private void Awake()
    {
        Instance = this;
        EventCenter.AddListener(EventDefine.AddScore, SetScore);
        EventCenter.AddListener<int>(EventDefine.AddDiamondCount, SetDiamondCount);
        vars = ManagerVars.GetManagerVars();

        InitGameData();

        // 测试 排序
        /*int[] aa = new int[] { 8, 4, 90, 8, 34, 67, 1, 26, 17, 18, 5, 7, 9, 6, 32, 56, 41, 58, 54, 86, 52, 524 };
        aa = InsertRank(aa);
        for (int a = 0; a < aa.Length; a++) {
            Debug.Log(aa[a]);
        }*/
    }

    /// <summary>
    /// 初始化游戏数据
    /// </summary>
    private void InitGameData() {
        Read();

        if (canRead_)
        {
            isMusicOn = data.GetIsMusicOn();
            bestScoreArr = data.GetBestScoreArr();
            selectSkin = data.GetSelectSkin();
            skinUnlockArr = data.GetSkinUnlockArr();
            diamondCount = data.GetDiamondCount();
        }
        else // 第一次游戏
        {
            isFirstGame = false;
            isMusicOn = true;
            bestScoreArr = new int[4];
            selectSkin = 0;
            skinUnlockArr = new bool[vars.scrollSpriteList.Count];
            skinUnlockArr[0] = true;
            diamondCount = 10;

            data = new GameData();
            Save();
        }

    }

    /// <summary>
    /// 验证皮肤是否解锁
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetSkinUnlock(int index)
    {
        return skinUnlockArr[index];
    }

    /// <summary>
    /// 解锁新皮肤
    /// </summary>
    /// <param name="index"></param>
    public void SetSkinUnlock(int index)
    {
        skinUnlockArr[index] = true;
        Save();
    }

    /// <summary>
    /// 设置声音是否开启
    /// </summary>
    /// <param name="value"></param>
    public void SetIsMusicOn(bool value) {
        isMusicOn = value;
        Save();
    }

    /// <summary>
    /// 获取声音是否开启
    /// </summary>
    /// <returns></returns>
    public bool GetIsMusicOn() {
        return isMusicOn;
    }

    /// <summary>
    /// 获取角色拥有钻石 总数
    /// </summary>
    /// <returns></returns>
    public int GetDiamondCount() {
        return diamondCount;
    }

    /// <summary>
    /// 更新钻石总数
    /// </summary>
    /// <param name="dc"></param>
    public void SetDiamondCount(int dc) {
        this.diamondCount += dc;
        Save();
    }

    /// <summary>
    /// 存储当前选择皮肤
    /// </summary>
    /// <param name="index"></param>
    public void SetSelectSkin(int index) {
        this.selectSkin = index;
        Save();
    }

    /// <summary>
    /// 获取当前所选皮肤
    /// </summary>
    /// <returns></returns>
    public int GetSelectSkin() {
        return selectSkin;
    }

    /// <summary>
    /// 充值游戏数据
    /// </summary>
    public void ResetData() {
        isFirstGame = false;
        isMusicOn = true;
        bestScoreArr = new int[3];
        selectSkin = 0;
        skinUnlockArr = new bool[vars.scrollSpriteList.Count];
        skinUnlockArr[0] = true;
        diamondCount = 10;

        data = new GameData();
        Save();
    }

    // 更新玩家得分
    private void SetScore()
    {
        gameScore++;
    }

    /// <summary>
    /// 获取玩家分数
    /// </summary>
    /// <returns></returns>
    public int GetGameScore() {
        return gameScore;
    }

    /// <summary>
    /// 设置游戏分数排名
    /// </summary>
    public void SetScoreRank() {
        if (gameScore > bestScoreArr[2]) {
            bestScoreArr[2] = gameScore;
            bestScoreArr = InsertRank(bestScoreArr);
            Save();
        }
    }

    /// <summary>
    /// 获取最高分数
    /// </summary>
    /// <returns></returns>
    public int GetBestScore() {
        return bestScoreArr[0];
    }

    /// <summary>
    /// 选择排序,从大到小
    /// </summary>
    private int[] SelectRank(int[] arr) {
        int len = arr.Length;
        for (int i = 0; i < len; i++) {
            int m = i;
            for (int j = i + 1; j < len; j++) {
                if (arr[m] < arr[j]) {
                    m = j;
                }
            }
            if (m != i) Swap(arr, m, i);
        }
        return arr;
        // 去掉数组最后一个元素
        /*List<int> arr_ = arr.ToList();
        arr_.RemoveAt(len - 1);
        return arr_.ToArray();*/
    }

    /// <summary>
    /// 冒泡排序，从大到小
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    private int[] BubblingRank(int[] arr) {

        int len = arr.Length;
        for (int i = 0; i < len-1; i++) {
            for (int j = 0; j < len-1 - i; j++) {
                if (arr[j] < arr[j+1]) {
                    Swap(arr, j, j+1);
                }
            }
        }

        return arr;
    }

    /// <summary>
    /// 插入排序，从大到小
    /// </summary>
    /// <param name="arr"></param>
    /// <returns></returns>
    private int[] InsertRank(int[] arr) {
        int len = arr.Length;
        for (int i = 1; i < len; i++) {
            int j = i;
            while (j > 0)
            {
                if (arr[j] > arr[j - 1]) Swap(arr, j, j - 1);
                j--;
            }
        }

        return arr;
    }

    /// <summary>
    /// 交换数组元素
    /// </summary>
    private void Swap(int[] arr, int a, int b) {
        if (a == b) return;

        int t_ = arr[a];
        arr[a] = arr[b];
        arr[b] = t_;
    }

    /// <summary>
    /// 写入数据
    /// </summary>
    private void Save() {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Create(Application.persistentDataPath + "/GameData.data"))
            {
                data.SetBestScoreArr(bestScoreArr);
                data.SetDiamondCount(diamondCount);
                data.SetIsFirstGame(isFirstGame);
                data.SetIsMusicOn(isMusicOn);
                data.SetSelectSkin(selectSkin);
                data.SetSkinUnlockArr(skinUnlockArr);
                bf.Serialize(fs, data);
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    private void Read() {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = File.Open(Application.persistentDataPath + "/GameData.data", FileMode.Open))
            {
                data = (GameData)bf.Deserialize(fs);
                canRead_ = true;
            }
        }
        catch (System.Exception e)
        {

            Debug.Log(e.Message);
        }
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.AddScore,SetScore);
    }
}
