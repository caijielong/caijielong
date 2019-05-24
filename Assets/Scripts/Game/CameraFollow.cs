using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 摄像机跟随
/// </summary>
public class CameraFollow : MonoBehaviour {

    private Transform target;
    private Vector2 vel;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {

        // 只会执行一次
        if (target == null && GameObject.FindGameObjectWithTag("Player") != null) {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            offset = target.position - transform.position;
        }
	}

    private void FixedUpdate()
    {
        if (target != null) {
            float posX = Mathf.SmoothDamp(transform.position.x, target.position.x - offset.x, ref vel.x, 0.05f);
            float posY = Mathf.SmoothDamp(transform.position.y, target.position.y - offset.y, ref vel.y, 0.05f);

            if (posY > transform.position.y) {
                transform.position = new Vector3(posX, posY, transform.position.z);
            }
        }
    }
}
