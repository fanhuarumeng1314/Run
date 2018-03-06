using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SetingUI : MonoBehaviour {

    public Button gameContinue;             //继续游戏按钮
    public Button allVolume;                //音量按钮
    public Button returnMenu;               //返回菜单
    public Button exitSet;                  //退出设置界面的按钮
    public Slider volumeValue;              //音量的数值
    public bool changeVolume = false;       //是否改变音量
    void Start ()
    {
        gameContinue.onClick.AddListener(ContinueOnclick);
        allVolume.onClick.AddListener(VolumeOnclick);
        returnMenu.onClick.AddListener(ReturnMenu);
        exitSet.onClick.AddListener(ExitSetOnclick);
    }
	
	void Update ()
    {
        if (changeVolume)
        {
            GameMode.Instance.audioBGPlay.volume = volumeValue.value;
        }	
	}
    /// <summary>
    /// 继续游戏的响应事件
    /// </summary>
    public void ContinueOnclick()
    {
        AudioPlay();
        GameMode.Instance.gameState = true;
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 音量的响应事件
    /// </summary>
    public void VolumeOnclick()
    {
        AudioPlay();
        volumeValue.gameObject.SetActive(true);
        volumeValue.gameObject.transform.localScale = Vector3.zero;
        gameContinue.gameObject.SetActive(false);
        allVolume.gameObject.SetActive(false);
        returnMenu.gameObject.SetActive(false);
        volumeValue.gameObject.transform.DOScale(1,1);
        changeVolume = true;
    }
    /// <summary>
    /// 返回菜单的响应事件
    /// </summary>
    public void ReturnMenu()
    {
        AudioPlay();
        GameMode.Instance.SortRank();
        GameMode.Instance.UpdataRankFile();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameStartUI");
    }
    /// <summary>
    /// 退出设置菜单的响应事件
    /// </summary>
    public void ExitSetOnclick()
    {
        AudioPlay();
        GameMode.Instance.gameState = true;
        gameContinue.gameObject.SetActive(true);
        allVolume.gameObject.SetActive(true);
        returnMenu.gameObject.SetActive(true);
        volumeValue.gameObject.SetActive(false);
        gameObject.SetActive(false);
        changeVolume = false;
    }

    void AudioPlay()
    {
        GameObject tmpAudioPlay = Instantiate(GameMode.Instance.audioPrefeb);
        AudioSource audioPlay = tmpAudioPlay.GetComponent<AudioSource>();
        audioPlay.clip = GameMode.Instance.tishiClip;
        audioPlay.Play();
        Destroy(tmpAudioPlay, 1f);
    }
}
