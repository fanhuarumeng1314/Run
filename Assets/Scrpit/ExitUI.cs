using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitUI : MonoBehaviour {

    public Button exitBtn;          //退出按钮
    public Button returnBtn;        //返回按钮
	void Start ()
    {
        exitBtn = transform.Find("Btn_Exit").GetComponent<Button>();
        returnBtn = transform.Find("Btn_Return").GetComponent<Button>();
        exitBtn.onClick.AddListener(()=>Application.Quit());
        returnBtn.onClick.AddListener(ReturnOnclick);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReturnOnclick()
    {
        GameObject tmpAudioPlay = Instantiate(GameMode.Instance.audioPrefeb, transform);
        AudioSource audioPlay = tmpAudioPlay.GetComponent<AudioSource>();
        audioPlay.clip = GameMode.Instance.tishiClip;
        audioPlay.Play();
        Destroy(tmpAudioPlay, 1f);
        GameMode.Instance.gameState = true;
        gameObject.SetActive(false);
    }
}
