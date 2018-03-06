using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOut : MonoBehaviour {

    public Button nowBtn;               //现在的按钮
    public GameObject setUI;            //UI设置界面
    public GameObject nowSetUI;         //现在实例化的设置界面
	void Start ()
    {
        nowBtn = GetComponent<Button>();
        setUI = Resources.Load("UiPreFeb/Img_Set") as GameObject;
        nowBtn.onClick.AddListener(OnClick);
    }
	
	
	void Update () {
		
	}
    public void OnClick()
    {
        GameMode.Instance.gameState = false;
        GameMode.Instance.GreatSetlled();
        GameObject tmpAudioPlay = Instantiate(GameMode.Instance.audioPrefeb,transform);
        AudioSource audioPlay = tmpAudioPlay.GetComponent<AudioSource>();
        audioPlay.clip = GameMode.Instance.tishiClip;
        audioPlay.Play();
        Destroy(tmpAudioPlay,1f);
        if (nowSetUI == null && setUI !=null)
        {
            nowSetUI = Instantiate(setUI,transform.parent);
        }
        else if(nowSetUI != null)
        {
            nowSetUI.SetActive(true);
        }
        //nowBtn.onClick.RemoveAllListeners();
        //nowBtn.onClick.AddListener(OnClick2);
    }
    public void OnClick2()
    {
        GameMode.Instance.gameState = true;
        nowBtn.onClick.RemoveAllListeners();
        nowBtn.onClick.AddListener(OnClick);
    }
}
