using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResrtPanel : MonoBehaviour {

    private Image m_bg;
    private Transform m_bg_;
    private Button m_btn_yes;
    private Button m_btn_no;

    private void Awake()
    {
        m_bg = transform.Find("Img_Bg").GetComponent<Image>();
        m_bg_ = transform.Find("Img_");

        m_btn_yes = transform.Find("Img_/Btn_Yes").GetComponent<Button>();
        m_btn_yes.onClick.AddListener(OnYesButtonClick);
        m_btn_no = transform.Find("Img_/Btn_No").GetComponent<Button>();
        m_btn_no.onClick.AddListener(OnNoButtonClick);
        
        EventCenter.AddListener(EventDefine.ResetPanel, Show);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ResetPanel, Show);
    }

    private void Show() {
        gameObject.SetActive(true);
        m_bg_.localScale = Vector3.zero;
        m_bg.color = new Color(m_bg.color.r, m_bg.color.g, m_bg.color.b, 0);

        m_bg_.DOScale(Vector3.one, 0.3f);
        m_bg.DOColor(new Color(m_bg.color.r, m_bg.color.g, m_bg.color.b, 0.4f), 0.2f);
    }

    private void OnNoButtonClick() {
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        gameObject.SetActive(false);
    }

    private void OnYesButtonClick() {
        GameManager.Instance.ResetData();
        EventCenter.Broadcast(EventDefine.PlayAudio, "button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
