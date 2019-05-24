using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private ManagerVars vars;
    private void Awake()
    {
        vars = ManagerVars.GetManagerVars();
        EventCenter.AddListener<string>(EventDefine.PlayAudio, PlayAudio);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.PlayAudio, PlayAudio);
    }

    /// <summary>
    ///  播放声音
    /// </summary>
    /// <param name="pu"></param>
    private void PlayAudio(string pu) {
        if (!GameManager.Instance.GetIsMusicOn()) return;
        switch (pu) {
            case "button":
                AudioSource.PlayClipAtPoint(vars.buttonClip,transform.position);
                break;
            case "hit":
                AudioSource.PlayClipAtPoint(vars.hitClip, transform.position);
                break;
            case "diamond":
                AudioSource.PlayClipAtPoint(vars.diamondClip, transform.position);
                break;
            case "fall":
                AudioSource.PlayClipAtPoint(vars.fallClip, transform.position);
                break;
            case "jump":
                AudioSource.PlayClipAtPoint(vars.jumpClip, transform.position);
                break;
            default:
                break;
        }
    }
}
