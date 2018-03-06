using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieScrpit : MonoBehaviour {

    public Button coutiunBtn;
    public Button exitBtm;
	void Start ()
    {
        coutiunBtn.onClick.AddListener(CoutiunOnClick);
        exitBtm.onClick.AddListener(ExitButtonOnClick);
    }
	
	
	void Update () {
		
	}

    public void CoutiunOnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("RunStart");
    }

    public void ExitButtonOnClick()
    {
        Application.Quit();
    }
}
