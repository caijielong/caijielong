using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PlayerManager : MonoBehaviour {

    private ManagerVars vars; // 资源管理器 
    private bool isMoveLeft; // 是否点击屏幕左侧 
    private Vector3 nextPlatformLeft; // 下一个平台左边的位置 
    private Vector3 nextPlatformRight; // 下一个平台右边的位置 
    private bool isJumping; // 人物是否正在跳跃中

    private Transform rayDown,rayLeft,rayRight; // 射线检测起点
    public LayerMask platformLayer, obstacleLayer; // 射线检测指定 Layer层
    private Rigidbody2D myBody; // 人物的刚体组件
    private SpriteRenderer spriteRenderer; // 人物的图片渲染组件

    // Use this for initialization
    private void Awake () {
        vars = ManagerVars.GetManagerVars();
        rayDown = transform.Find("RayDown").transform;
        rayLeft = transform.Find("LeftDown").transform;
        rayRight = transform.Find("RightDown").transform;
        myBody = transform.GetComponent<Rigidbody2D>();
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
        EventCenter.AddListener<int>(EventDefine.ChangeSkin, UpdateSprite);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<int>(EventDefine.ChangeSkin, UpdateSprite);
    }

    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线，检测是否点击的UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    // Update is called once per frame
    void Update ()
    {
        Debug.DrawRay(rayDown.position, Vector2.down*1, Color.red);
        Debug.DrawRay(rayLeft.position, Vector2.left * 0.2f, Color.blue);
        Debug.DrawRay(rayRight.position, Vector2.right * 0.2f, Color.red);

        //if (Application.platform == RuntimePlatform.Android ||
        //    Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    int fingerId = Input.GetTouch(0).fingerId;
        //    if (EventSystem.current.IsPointerOverGameObject(fingerId)) return;
        //}
        //else
        //{
        //    if (EventSystem.current.IsPointerOverGameObject()) return;
        //}

        if (IsPointerOverGameObject(Input.mousePosition)) return;
        if (GameManager.Instance.IsGameStart == false || GameManager.Instance.IsGameOver == true || GameManager.Instance.IsPause) return;

        // 按键  按下判断是在屏幕的左侧还是右侧
        if (Input.GetMouseButtonDown(0) && isJumping == false &&nextPlatformLeft != Vector3.zero) {
            isJumping = true;
            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x <= Screen.width/2) {
                isMoveLeft = true;
            }
            else if(mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;
            }
            Jump();
            GameManager.Instance.IsPlayerMove = true;
            EventCenter.Broadcast(EventDefine.SpawnPlatform);
        }

        // 往下跳检测
        if (myBody.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            EventCenter.Broadcast(EventDefine.PlayAudio, "fall");
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            Debug.Log("game is over ~~");

            //TODO 调用结束面板
            StartCoroutine(DelayGameOver());
        }

        // 左右碰到障碍物检测
        if (IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
            EventCenter.Broadcast(EventDefine.PlayAudio, "hit");
            GameManager.Instance.IsGameOver = true;           
            GameObject go = ObjectPoolManager.Instance.GetDeathEffect();
            go.SetActive(true);
            go.transform.position = transform.position;
            Debug.Log("game is over  ~~ left-right");
            StartCoroutine(DelayGameOver());
        }

        // 偏离摄像机，游戏结束
        if (transform.position.y - Camera.main.transform.position.y < -6 && GameManager.Instance.IsGameOver == false)
        {
            EventCenter.Broadcast(EventDefine.PlayAudio, "fall");
            GameManager.Instance.IsGameOver = true;           
            Debug.Log("game is over  ~~ move camera");
            StartCoroutine(DelayGameOver());           
        }
    }

    IEnumerator DelayGameOver() {       
        GameManager.Instance.SetScoreRank();
        yield return new WaitForSeconds(1f);
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
        Destroy(gameObject);
    }

    /// <summary>
    /// 更新皮肤
    /// </summary>
    /// <param name="index"></param>
    private void UpdateSprite(int index) {
        spriteRenderer.sprite = vars.characterSpriteList[index];
    }

    private void Start()
    {
        UpdateSprite(GameManager.Instance.GetSelectSkin());
    }

    /// <summary>
    /// 左右检测障碍物
    /// </summary>
    /// <returns></returns>
    private bool IsRayObstacle() {
        RaycastHit2D hitLeft = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.2f, obstacleLayer);
        if (hitLeft.collider != null && hitLeft.collider.tag == "Obstacle") return true;

        RaycastHit2D hitRight = Physics2D.Raycast(rayRight.position, Vector2.right, 0.2f, obstacleLayer);
        if (hitRight.collider != null && hitRight.collider.tag == "Obstacle") return true;
        return false;
    }

    private GameObject lastHitGo = null;
    /// <summary>
    /// 向下射线检测
    /// </summary>
    private bool IsRayPlatform() {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null && hit.collider.tag == "Platform") {
            // 记录玩家分数
            if (lastHitGo == null)
            {
                lastHitGo = hit.collider.gameObject;
                return true;
            }
            if (hit.collider.gameObject != lastHitGo)
            {
                lastHitGo = hit.collider.gameObject;
                EventCenter.Broadcast(EventDefine.AddScore);
            }
            return true;
        }
        return false;              
    }

    private void Jump() {

        if (isMoveLeft)
        {
            transform.DOMoveX(nextPlatformLeft.x, 0.2f);
            transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else {
            transform.DOMoveX(nextPlatformRight.x, 0.2f);
            transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
            transform.localScale = Vector3.one;
        }
        EventCenter.Broadcast(EventDefine.PlayAudio, "jump");
    }

    // 碰撞器
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pickup")
        {
            collision.gameObject.SetActive(false);
            EventCenter.Broadcast<int>(EventDefine.AddDiamondCount, 1);
            EventCenter.Broadcast(EventDefine.PlayAudio, "diamond");
        }
    }

    // 触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatformPos = collision.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x - vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x + vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
        }       
    }
}
