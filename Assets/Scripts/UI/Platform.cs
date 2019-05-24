using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    public SpriteRenderer[] spriteRenderers; // 平台的主题渲染器
    public GameObject obstacle; // 平台子物体障碍物，用于随机左右方向
    private Rigidbody2D m_rigidbody; // 平台刚体组件
    private float fallTime_;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    public void Init(Sprite sprite, float fallTime, int obstacleDir) {
        for (int i = 0; i < spriteRenderers.Length; i ++) {
            spriteRenderers[i].sprite = sprite;
        }

        // 障碍物方向
        if (obstacleDir == 0)
        {
            if (obstacle != null)
            {
                obstacle.transform.localPosition = new Vector3(-obstacle.transform.localPosition.x, obstacle.transform.localPosition.y, obstacle.transform.localPosition.z);
            }
        }
        fallTime_ = fallTime;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGameStart || !GameManager.Instance.IsPlayerMove) return;
        fallTime_ -= Time.deltaTime;
        // 平台掉落
        if (fallTime_ < 0)
        {
            if (m_rigidbody.bodyType != RigidbodyType2D.Dynamic)
            {
                m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
                StartCoroutine("Fall");
            }

        }

        // 偏离摄像机，游戏结束
        if (transform.position.y - Camera.main.transform.position.y < -6) {
            StartCoroutine("Fall");
        }
    }

     private IEnumerable Fall() {        
        yield return new WaitForSeconds(1.2f);
        gameObject.SetActive(false);
    }
}
