using UnityEngine;
using UnityEditor;

[System.Serializable]
public class GameData
{
    public static bool IsAgainGame;

    private bool isFirstGame; // 是否第一次游戏
    private bool isMusicOn; // 是否开启游戏声音
    private int[] bestScoreArr; // 游戏分数排名
    private int selectSkin; // 选择的皮肤
    private bool[] skinUnlockArr; // 皮肤是否解锁
    private int diamondCount; // 钻石数量

    public void SetIsFirstGame(bool isFirstGame)
    {
        this.isFirstGame = isFirstGame;
    }

    public void SetIsMusicOn(bool isMusicOn)
    {
        this.isMusicOn = isMusicOn;
    }

    public void SetBestScoreArr(int[] bestScore) {
        this.bestScoreArr = bestScore;
    }

    public void SetSelectSkin(int selectSkin) {
        this.selectSkin = selectSkin;
    }

    public void SetSkinUnlockArr(bool[] skinUnlocked) {
        this.skinUnlockArr = skinUnlocked;
    } 

    public void SetDiamondCount(int diamondCount) {
        this.diamondCount = diamondCount;
    }

    public bool GetIsFirstGame() {
        return this.isFirstGame;
    }

    public bool GetIsMusicOn()
    {
        return this.isMusicOn;
    }

    public int[] GetBestScoreArr() {
        return this.bestScoreArr;
    }

    public int GetSelectSkin()
    {
        return this.selectSkin;
    }

    public bool[] GetSkinUnlockArr()
    {
        return this.skinUnlockArr;
    }

    public int GetDiamondCount()
    {
        return this.diamondCount;
    }
}
