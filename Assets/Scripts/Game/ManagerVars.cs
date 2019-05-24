using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 创建资源管理器
// [CreateAssetMenu(menuName = "CreateManagerVarsContainer")]
public class ManagerVars : ScriptableObject {

    public List<Sprite> bgThemeSpriteList = new List<Sprite>(); // 游戏背景随机图片
    public List<Sprite> platformThemeSpriteList = new List<Sprite>(); // 平台背景图
    public GameObject normalPre; // 平台预制体    
    public GameObject characterPre; // 人物预制体
    public GameObject scrollItem; // 皮肤预制体

    public List<Sprite> scrollSpriteList = new List<Sprite>(); // 滚动条皮肤组合列表
    public List<Sprite> characterSpriteList = new List<Sprite>(); // 角色皮肤组合列表
    public List<GameObject> commonPlatformGroup = new List<GameObject>(); // 通用组合平台
    public List<GameObject> grassPlatformGroup = new List<GameObject>(); // 草地组合平台 
    public List<GameObject> winterPlatformGroup = new List<GameObject>(); // 冬季组合平台
    public List<GameObject> spikePlatformGroup = new List<GameObject>(); // 钉子组合平台
    public GameObject deathEffect; // 玩家销毁特效
    public GameObject diamond; // 钻石

    public float nextXPos = 0.554f;
    public float nextYPos = 0.645f;
    public float spikeXPos = 1.65f;
    public Sprite musicOn, musicOff;// 声音设置 开启关闭 图标
    public List<string> itemName = new List<string>(); // 角色名称列表
    public List<int> itemDiamond = new List<int>(); // 角色需要钻石数
    public AudioClip buttonClip; // 按钮点击声音
    public AudioClip diamondClip; // 迟到钻石声音
    public AudioClip fallClip; // 失败声音
    public AudioClip hitClip; // 碰到障碍物声音
    public AudioClip jumpClip; // 跳声音

    public static ManagerVars GetManagerVars() {
        return Resources.Load<ManagerVars>("ManagerVarsContainer");
    }
}
