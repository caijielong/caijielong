using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgTheme : MonoBehaviour {

    private SpriteRenderer sr_;
    private ManagerVars vars;

	// Use this for initialization
	private void Awake () {
        sr_ = GetComponent<SpriteRenderer>();
        
        // 生成随机的背景图
        vars = ManagerVars.GetManagerVars(); 
        int i = Random.Range(0, vars.bgThemeSpriteList.Count);
        sr_.sprite = vars.bgThemeSpriteList[i];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
