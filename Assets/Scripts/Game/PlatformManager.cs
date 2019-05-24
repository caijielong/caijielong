using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformGroupType
{
    Winter,Grass
}
public class PlatformManager : MonoBehaviour {

    private ManagerVars vars; // 资源管理器
    private Vector3 platformSpawnPosition; // 下一个平台生成位置
    private bool isLeftSpawn; // 平台往左往右生成定义
    private int spawnPlatformCount; // 生成平台数量
    public Vector3 startSpawn = new Vector3(0,-2.4f,0); // 第一个平台生成位置    

    private Sprite selectPlatformSprite; // 随机生成一个皮肤
    private PlatformGroupType groupType; // 存储随机主题 
    public int milestoneCount = 20; // 里程碑数，（玩家到达该分数进行掉落计算）
    public float fallTime = 2f; // 一开始掉落的时间
    public float minFallTime = 0.5f; // 最小的掉落时间
    public float coefficient = 0.9f; // 掉落时间系数

    private void Awake()
    {
        // 监听生成一个平台
        EventCenter.AddListener(EventDefine.SpawnPlatform, DecidePath);
        vars = ManagerVars.GetManagerVars();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.SpawnPlatform, DecidePath);
    }

    // Use this for initialization
    void Start () {
        RandomPlatformTheme();
        platformSpawnPosition = startSpawn;       
        for (int i = -4; i < 4; i++)
        {
            spawnPlatformCount = i;
            DecidePath();
        }
        //生成人物
        GameObject player = Instantiate(vars.characterPre);
        player.transform.position = new Vector3(0, -1.8f, 0);
    }      

    /// <summary>
    /// 随机生成一个平台
    /// </summary>
    private void RandomPlatformTheme() {
        int ran = Random.Range(0, vars.platformThemeSpriteList.Count);
        selectPlatformSprite = vars.platformThemeSpriteList[ran];

        if (ran == 2)
        {
            groupType = PlatformGroupType.Winter;
        }
        else {
            groupType = PlatformGroupType.Grass;
        }
    }
    

    /// <summary>
    /// 确定路径
    /// </summary>
    private void DecidePath() {
        if (spawnPlatformCount > 0) {
            spawnPlatformCount--;
            SpawnPlatform();
        }
        else
        {
            isLeftSpawn = !isLeftSpawn;
            spawnPlatformCount = Random.Range(1, 4);
            SpawnPlatform();
        }
    }

    /// <summary>
    /// 生成平台
    /// </summary>
    private void SpawnPlatform() {
        int obstacle_n = Random.Range(0, 2);

        // 生成单个平台
        if (spawnPlatformCount >= 1)
        {
            SpawnOnePlatform(obstacle_n);
            SpawnDiamond();
        }
        // 生成组合平台
        else if (spawnPlatformCount == 0) {
            int ran = Random.Range(0, 3);
            // 生成组合通用平台
            if (ran == 0)
            {
                SpawnCommonPlatformGroup(obstacle_n);
            }
            // 生成草地，雪地 组合平台
            else if (ran == 1)
            {
                switch (groupType)
                {
                    case PlatformGroupType.Grass:
                        SpawnGrassPlatformGroup(obstacle_n);
                        break;
                    case PlatformGroupType.Winter:
                        SpawnWinterPlatformGroup(obstacle_n);
                        break;
                    default:
                        break;
                }
            }
            // 生成钉子平台
            else {
                SpawnSpikePlatformGroup(obstacle_n);
            }
        }     

        if (isLeftSpawn) { // 向左生成
            platformSpawnPosition = new Vector3(platformSpawnPosition.x - vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
        }
        else // 向右生成
        {
            platformSpawnPosition = new Vector3(platformSpawnPosition.x + vars.nextXPos, platformSpawnPosition.y + vars.nextYPos, 0);
        }
    }

    private void SpawnDiamond() {

        if (!GameManager.Instance.IsPlayerMove) return;
        // 随机生成钻石
        int diamond_num = Random.Range(0, 15);
        if (diamond_num == 9)
        {
            GameObject diamondObj = ObjectPoolManager.Instance.GetDiamond();
            diamondObj.SetActive(true);
            diamondObj.transform.position = new Vector3(platformSpawnPosition.x, platformSpawnPosition.y + 0.5f, 0);
        }
    }

    /// <summary>
    /// 生成单个平台
    /// </summary>
    private void SpawnOnePlatform(int obstacle_n)
    {
        GameObject go = ObjectPoolManager.Instance.GetNormalPlatform();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, fallTime, obstacle_n);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成通用组合平台
    /// </summary>
    private void SpawnCommonPlatformGroup(int obstacle_n)
    {
        GameObject go = ObjectPoolManager.Instance.GetCommonPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, fallTime, obstacle_n);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成草地组合平台
    /// </summary>
    private void SpawnGrassPlatformGroup(int obstacle_n)
    {
        GameObject go = ObjectPoolManager.Instance.GetGrassPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, fallTime, obstacle_n);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成冬季组合平台
    /// </summary>
    private void SpawnWinterPlatformGroup(int obstacle_n)
    {
        GameObject go = ObjectPoolManager.Instance.GetWinterPlatformGroup();
        go.transform.position = platformSpawnPosition;
        go.GetComponent<Platform>().Init(selectPlatformSprite, fallTime, obstacle_n);
        go.SetActive(true);
    }

    /// <summary>
    /// 生成钉子组合平台
    /// </summary>
    private void SpawnSpikePlatformGroup(int obstacle_n)
    {
        
        GameObject temp;
        Vector3 v3_;
        int isLeft_ = 1;
        if (isLeftSpawn) // 左边钉子
        {
            temp = ObjectPoolManager.Instance.GetLeftSpikePlatform();
            v3_ = new Vector3(-vars.spikeXPos, vars.nextXPos, 0);
        }
        // 右边钉子
        else
        { 
            isLeft_ = 2;
            temp = ObjectPoolManager.Instance.GetRightSpikePlatform();
            v3_ = new Vector3(vars.spikeXPos, vars.nextXPos, 0);
        }
        temp.SetActive(true);

        // 钉子生成随机正常平台数量
        int spikeToNormalNum = Random.Range(3, 6);
        
        for (int i = 0; i < spikeToNormalNum; i++) {
            GameObject temp_ = ObjectPoolManager.Instance.GetNormalPlatform();
            temp_.transform.SetParent(temp.transform);
            temp_.transform.localPosition = v3_;
            temp_.GetComponent<Platform>().Init(selectPlatformSprite, fallTime, obstacle_n);
            temp_.SetActive(true);
            if (isLeft_ == 1)
            {
                v3_ = new Vector3(v3_.x - vars.nextXPos,v3_.y + vars.nextYPos, v3_.z);
            }
            else
            {
                v3_ = new Vector3(v3_.x + vars.nextXPos, v3_.y + vars.nextYPos, v3_.z);
            }           
        }

        // 钉子平台生成随机子平台
        
        temp.transform.position = platformSpawnPosition;
        temp.GetComponent<Platform>().Init(selectPlatformSprite, fallTime, obstacle_n);

    }

    // Update is called once per frame
    void Update () {

        if (GameManager.Instance.IsGameStart || GameManager.Instance.IsPlayerMove) {
            UpdateFallTime();
        }
	}


    /// <summary>
    /// 更新平台掉落时间
    /// </summary>
    private void UpdateFallTime() {
        if (GameManager.Instance.GetGameScore() > milestoneCount) {
            milestoneCount *= 2;
            fallTime *= coefficient;
            if (fallTime < minFallTime)
            {
                fallTime = minFallTime;
            }
        }
    }
}
