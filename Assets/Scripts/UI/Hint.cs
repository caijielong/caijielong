using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hint : MonoBehaviour {

    private Image m_img_hint;
    private Text m_text_hint;

    private void Awake()
    {
        m_img_hint = GetComponent<Image>();
        m_text_hint = GetComponentInChildren<Text>();
        m_img_hint.color = new Color(m_img_hint.color.r, m_img_hint.color.g, m_img_hint.color.b, 0);
        m_text_hint.color = new Color(m_text_hint.color.r, m_text_hint.color.g, m_text_hint.color.b, 0);
        EventCenter.AddListener<string>(EventDefine.ShowHint, Show);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.ShowHint, Show);
    }

    private void Show(string str) {
        StopCoroutine("Delay");
        transform.localPosition = new Vector3(0, -70, 0);
        transform.DOLocalMoveY(0, 0.3f).OnComplete(()=> {
            StartCoroutine("Delay");
        });

        m_img_hint.DOColor(new Color(m_img_hint.color.r, m_img_hint.color.g, m_img_hint.color.b, 0.4f), 0.15f);
        m_text_hint.DOColor(new Color(m_text_hint.color.r, m_text_hint.color.g, m_text_hint.color.b, 1), 0.15f);
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(0.9f);
        transform.DOLocalMoveY(70, 0.2f);
        m_img_hint.DOColor(new Color(m_img_hint.color.r, m_img_hint.color.g, m_img_hint.color.b, 0), 0.15f);
        m_text_hint.DOColor(new Color(m_text_hint.color.r, m_text_hint.color.g, m_text_hint.color.b, 0), 0.15f);
    }
}
