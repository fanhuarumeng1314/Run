using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayController : MonoBehaviour {

    public PlayCharacter character;
    public Vector2 ScreenAixY = new Vector2(0,1);             //屏幕坐标系Y轴
    public Vector2 ScreenAixX = new Vector2(1,0);             //屏幕坐标系X轴
    Vector2 startPos;
    Vector2 EndPos;
    bool isInput = false;
    GameObject exitUI;
    public float angle = 0;
    void Start ()
    {
        character = GetComponent<PlayCharacter>();
    }


    void Update()
    {
        character.Move();
#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //character.Jump();
            if (!(transform.position.y > 0.25f))
            {
                character.isJump = true;
            }

        }
        float moveDir = Input.GetAxis("Horizontal");
        character.ManualMove(moveDir);

        if (Input.GetKeyDown(KeyCode.J))
        {
            character.RotateLeft();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            character.RotateRight();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            character.Slide();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exitUI != null)
            {
                exitUI.SetActive(true);
            }
            else
            {
                exitUI = Instantiate(Resources.Load("UiPreFeb/Img_Exit"), GameObject.Find("Canvas").transform) as GameObject;
            }
            GameMode.Instance.gameState = false;
        }
        #region //PC测试
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    startPos = Camera.main.ViewportToScreenPoint(Input.mousePosition);
        //}

        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{
        //    EndPos = Camera.main.ViewportToScreenPoint(Input.mousePosition);
        //    isInput = true;
        //}
        #endregion
#endif
        #region //手机代码
#if UNITY_ANDROID
        character.ManualMove(Input.acceleration.x);
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                startPos = Input.touches[0].position;
            }

            if (Input.touches[0].phase == TouchPhase.Ended && Input.touches[0].phase != TouchPhase.Canceled)
            {
                EndPos = Input.touches[0].position;
                isInput = true;
            }
        }
        if (Application.platform == RuntimePlatform.Android && (Input.GetKeyDown(KeyCode.Escape)))
        {
            if (exitUI != null)
            {
                exitUI.SetActive(true);
            }
            else
            {
                exitUI = Instantiate(Resources.Load("UiPreFeb/Img_Exit"), GameObject.Find("Canvas").transform) as GameObject;
            }
            GameMode.Instance.gameState = false;
        }

        if (isInput && GameMode.Instance.gameState)
        {
            Vector2 nowDir = EndPos - startPos;                   //计算手指滑动之间的向量
            float cosValueX = Vector3.Dot(nowDir, ScreenAixX) / nowDir.magnitude * ScreenAixX.magnitude;    //与屏幕坐标的X轴的余弦值
            float cosValueY = Vector3.Dot(nowDir, ScreenAixY) / nowDir.magnitude * ScreenAixY.magnitude; //与屏幕坐标的Y轴余弦值
            angle = Mathf.Acos(cosValueY) * Mathf.Rad2Deg;

            Debug.Log(angle + " === " + cosValueX + " === " + cosValueY + " === ");
            if (cosValueX < 0)
            {
                if (angle > 45 && angle < 135)
                {
                    character.RotateLeft();
                    Debug.Log("左转" + "    " + angle);
                }
                else if (angle > 0 && angle < 45)
                {
                    character.isJump = true;
                    Debug.Log("跳跃" + "    " + angle);
                }
                else if (angle > 135 && angle < 180)
                {
                    character.Slide();
                    Debug.Log("下蹲" + "    " + angle);
                }
            }
            else if (cosValueX == 0)
            {
                if (angle > 0)
                {
                    character.isJump = true;
                    Debug.Log("跳跃" + "    " + angle);
                }
                else
                {
                    character.Slide();
                    Debug.Log("下蹲" + "    " + angle);
                }
            }
            else
            {
                if (angle > 45 && angle < 135)
                {
                    character.RotateRight();
                    Debug.Log("右转" + "    " + angle);
                }
                else if (angle > 0 && angle < 45)
                {
                    character.isJump = true;
                    Debug.Log("跳跃" + "    " + angle);
                }
                else if (angle > 135 && angle < 180)
                {
                    character.Slide();
                    Debug.Log("下蹲" + "    " + angle);
                }
            }

            isInput = false;
        }

#endif
    }
    #endregion
}
