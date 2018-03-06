using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartGameUI : MonoBehaviour {

    int nowUI = 1;                  //标识当前按钮
    public Button startBtn;         //游戏中心按钮
    public Text btnTxt;             //当前按钮文字
    public Button leftBtn;          //左切换按钮
    public Button rightBtn;         //右切换按钮
    public GameObject rankUI;       //排行榜UI
    GameObject audioPrefeb;         //播放器预制体
    AudioSource tishiAudio;         //播放器
    AudioClip tiShi;                //音效
	void Start ()
    {
        startBtn = transform.Find("Btn_Start").GetComponent<Button>();
        btnTxt = transform.Find("Btn_Start/Text").GetComponent<Text>();
        leftBtn = transform.Find("Btn_Left").GetComponent<Button>();
        rightBtn = transform.Find("Btn_Right").GetComponent<Button>();
        startBtn.onClick.AddListener(StartGameOnClick);
        leftBtn.onClick.AddListener(LeftBtnOnClick);
        rightBtn.onClick.AddListener(RightOnClick);
        audioPrefeb = Resources.Load("AudioPrefeb") as GameObject;
        tiShi = Resources.Load("按钮类7", typeof(AudioClip)) as AudioClip;
    }
	
	
	void Update () {
		
	}

    public void LeftBtnOnClick()
    {
        GameObject tmpAuido = Instantiate(audioPrefeb, transform);
        tishiAudio = tmpAuido.GetComponent<AudioSource>();
        tishiAudio.clip = tiShi;
        tishiAudio.Play();
        Destroy(tmpAuido, 1f);
        if (rankUI!=null)
        {
            rankUI.transform.DOScale(Vector3.zero, 1);
        }
        switch (nowUI)
        {
            case 1:
                btnTxt.text = "退出游戏";
                startBtn.onClick.RemoveAllListeners();
                startBtn.onClick.AddListener(EndGameOnClick);
                nowUI = 3;
                break;
            case 2:
                btnTxt.text = "开始游戏";
                startBtn.onClick.RemoveAllListeners();
                startBtn.onClick.AddListener(StartGameOnClick);
                nowUI = 1;
                break;
            case 3:
                btnTxt.text = "最高分";
                startBtn.onClick.RemoveAllListeners();
                startBtn.onClick.AddListener(GetVauleOnClick);
                nowUI = 2;
                break;
        }
    }

    public void RightOnClick()
    {
        GameObject tmpAuido = Instantiate(audioPrefeb,transform);
        tishiAudio = tmpAuido.GetComponent<AudioSource>();
        tishiAudio.clip = tiShi;
        tishiAudio.Play();
        Destroy(tmpAuido,1f);

        if (rankUI != null)
        {
            rankUI.transform.DOScale(Vector3.zero, 1);
        }
        switch (nowUI)
        {
            case 1:
                btnTxt.text = "最高分";
                startBtn.onClick.RemoveAllListeners();
                startBtn.onClick.AddListener(GetVauleOnClick);
                nowUI = 2;
                break;
            case 2:
                btnTxt.text = "退出游戏";
                startBtn.onClick.RemoveAllListeners();
                startBtn.onClick.AddListener(EndGameOnClick);
                nowUI = 3;
                break;
            case 3:
                btnTxt.text = "开始游戏";
                startBtn.onClick.RemoveAllListeners();
                startBtn.onClick.AddListener(StartGameOnClick);
                nowUI = 1;
                break;

        }
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGameOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RunStart");
    }
    /// <summary>
    /// 查看最高分
    /// </summary>
    public void GetVauleOnClick()
    {
        GameObject tmpAuido = Instantiate(audioPrefeb, transform);
        tishiAudio = tmpAuido.GetComponent<AudioSource>();
        tishiAudio.clip = tiShi;
        tishiAudio.Play();
        Destroy(tmpAuido, 1f);
        if (rankUI==null)
        {
            rankUI = Instantiate(Resources.Load("UiPreFeb/Img_Rank") as GameObject, transform);
        }
        rankUI.transform.localScale = Vector3.zero;
        rankUI.transform.DOScale(new Vector3(1,1,1),1);
    }

    public void EndGameOnClick()
    {
        Application.Quit();
    }
}
