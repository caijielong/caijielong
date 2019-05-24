using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    public int initSpawnCount = 5;
    public List<GameObject> normalPlatformList = new List<GameObject>();
    public List<GameObject> grassPlatformList = new List<GameObject>();
    public List<GameObject> winterPlatformList = new List<GameObject>();
    public List<GameObject> commonPlatformList = new List<GameObject>();
    public List<GameObject> spikePlatformListLeft = new List<GameObject>();
    public List<GameObject> spikePlatformListRight = new List<GameObject>();
    public List<GameObject> deathEffectList = new List<GameObject>();
    public List<GameObject> diamondList = new List<GameObject>();

    private ManagerVars vars;
    public int initCount = 5;

    private void Awake()
    {
        Instance = this;
        vars = ManagerVars.GetManagerVars();
        Init();
    }

    /// <summary>
    /// 生成对象池，防止平台数量太多，降低内存
    /// </summary>
    private void Init() {

        for (int i = 0; i < initCount; i++)
        {
            InstantiateObject(vars.normalPre, ref normalPlatformList);

            for (int j = 0; j < vars.commonPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.commonPlatformGroup[j],ref commonPlatformList);
            }

            for (int j = 0; j < vars.grassPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.grassPlatformGroup[j], ref grassPlatformList);
            }

            for (int j = 0; j < vars.winterPlatformGroup.Count; j++)
            {
                InstantiateObject(vars.winterPlatformGroup[j], ref winterPlatformList);
            }

            InstantiateObject(vars.deathEffect, ref deathEffectList);
            InstantiateObject(vars.spikePlatformGroup[0], ref spikePlatformListLeft);
            InstantiateObject(vars.spikePlatformGroup[1], ref spikePlatformListRight);
            InstantiateObject(vars.diamond, ref diamondList);
        }
    }

    private GameObject InstantiateObject(GameObject pre, ref List<GameObject> addList) {
        GameObject go = Instantiate(pre, transform);
        go.SetActive(false);
        addList.Add(go);
        return go;
    }

    /// <summary>
    /// 调用普通单个平台
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetNormalPlatform() {
        for (int i = 0; i < normalPlatformList.Count; i++) {
            if (normalPlatformList[i].activeInHierarchy == false) {
                return normalPlatformList[i];
            }
        }

        return InstantiateObject(vars.normalPre, ref normalPlatformList);
    }

    /// <summary>
    /// 获取通用组合平台
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetCommonPlatformGroup()
    {
        for (int i = 0; i < commonPlatformList.Count; i++)
        {
            if (commonPlatformList[i].activeInHierarchy == false)
            {
                return commonPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.commonPlatformGroup.Count);
        return InstantiateObject(vars.commonPlatformGroup[ran], ref commonPlatformList);
    }

    /// <summary>
    /// 获取草地组合平台
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetGrassPlatformGroup()
    {
        for (int i = 0; i < grassPlatformList.Count; i++)
        {
            if (grassPlatformList[i].activeInHierarchy == false)
            {
                return grassPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.grassPlatformGroup.Count);
        return InstantiateObject(vars.grassPlatformGroup[ran], ref grassPlatformList);
    }

    /// <summary>
    /// 获取冬季组合平台
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetWinterPlatformGroup()
    {
        for (int i = 0; i < winterPlatformList.Count; i++)
        {
            if (winterPlatformList[i].activeInHierarchy == false)
            {
                return winterPlatformList[i];
            }
        }
        int ran = Random.Range(0, vars.winterPlatformGroup.Count);
        return InstantiateObject(vars.winterPlatformGroup[ran], ref winterPlatformList);
    }

    /// <summary>
    /// 获取左边钉子组合平台
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetLeftSpikePlatform()
    {
        for (int i = 0; i < spikePlatformListLeft.Count; i++)
        {
            if (spikePlatformListLeft[i].activeInHierarchy == false)
            {
                return spikePlatformListLeft[i];
            }
        }

        return InstantiateObject(vars.spikePlatformGroup[0], ref spikePlatformListLeft);
    }

    /// <summary>
    /// 获取右边钉子组合平台
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetRightSpikePlatform()
    {
        for (int i = 0; i < spikePlatformListRight.Count; i++)
        {
            if (spikePlatformListRight[i].activeInHierarchy == false)
            {
                return spikePlatformListRight[i];
            }
        }

        return InstantiateObject(vars.spikePlatformGroup[1], ref spikePlatformListRight);
    }

    /// <summary>
    /// 获取死亡特效
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetDeathEffect()
    {
        for (int i = 0; i < deathEffectList.Count; i++)
        {
            if (deathEffectList[i].activeInHierarchy == false)
            {
                return deathEffectList[i];
            }
        }

        return InstantiateObject(vars.deathEffect, ref deathEffectList);
    }

    /// <summary>
    /// 获取钻石
    /// </summary>
    /// <returns>GameObject</returns>
    public GameObject GetDiamond()
    {
        for (int i = 0; i < diamondList.Count; i++)
        {
            if (diamondList[i].activeInHierarchy == false)
            {
                return diamondList[i];
            }
        }

        return InstantiateObject(vars.diamond, ref diamondList);
    }
}
