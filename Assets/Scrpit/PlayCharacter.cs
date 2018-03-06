using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayCharacter : MonoBehaviour {

    CharacterController playCharact;
    Vector3 tmpPos;
    public bool isJump = false;
    public Animator nowAnmimator;
    public float powerSpeed;
    public float moveSpeed = 5.0f;
    public float timeJumpSpeed;             //跳跃间隔计时器
    public int Hp = 100;                    //血条暂未使用
    bool isRotaEnd = true;                  //转向动画是否结束
    void Start ()
    {
        playCharact = GetComponent<CharacterController>();
        nowAnmimator = GetComponent<Animator>();
    }
	
	
	void Update ()
    {
        if (!GameMode.Instance.gameState)
        {
            return;
        }
        timeJumpSpeed -= Time.deltaTime;
        moveSpeed += 0.1f * Time.deltaTime;
        if (transform.position.y<-6f && GameMode.Instance.gameState)
        {
            GameMode.Instance.gameState = false;
            GameMode.Instance.SortRank();
            GameMode.Instance.UpdataRankFile();
            GameMode.Instance.DieFloorInit();
            Instantiate(Resources.Load("UiPreFeb/Img_Die"), GameObject.Find("Canvas").transform);
            Debug.Log("玩家死亡");
        }
        //Move();
        if (isJump && transform.position.y>0.05f)
        {
            if (timeJumpSpeed<0)
            {

                Jump();
            }
           
        }
        else
        {
            tmpPos.y += playCharact.isGrounded ? 0f : Physics.gravity.y * Time.deltaTime * 1f;
        }
        playCharact.Move(tmpPos);
        nowAnmimator.SetFloat("RunSpeed",playCharact.velocity.magnitude);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var tmp_FloorCollider = hit.gameObject.GetComponent<FloorCollider>();
        if (tmp_FloorCollider)
        {
            if (!tmp_FloorCollider.initState)
            {
                return;
            }
            if (tmp_FloorCollider.isInit)
            {
                GameMode.Instance.nowSettled = hit.gameObject;
                tmp_FloorCollider.isInit = false;
                GameMode.Instance.TmpInit(tmp_FloorCollider);
            }
        }
    }
    /// <summary>
    /// 移动
    /// </summary>
    public void Move()
    {
        tmpPos = transform.forward * moveSpeed * Time.deltaTime;
    }
    /// <summary>
    /// 跳跃
    /// </summary>
    public void Jump()
    {
        tmpPos.y += 15f * Time.deltaTime;     //跳跃参数暂定
        nowAnmimator.SetBool("IsJump", true);
        StartCoroutine(InitJump());
        if (transform.position.y>=3.5f)
        {
            timeJumpSpeed = 0.9f;
            isJump = false;
        }
    }
    IEnumerator InitJump()
    {
        yield return new WaitForSeconds(0.6f);
        nowAnmimator.SetBool("IsJump", false);
        yield return null;
    }
    /// <summary>
    /// 左转
    /// </summary>
    public void RotateLeft()
    {
        if (isRotaEnd)
        {
            isRotaEnd = false;
            transform.Rotate(Vector3.up, -90);
            Quaternion tmpLeft = transform.rotation;
            transform.Rotate(Vector3.up, 90);
            Tween t =  transform.DORotateQuaternion(tmpLeft, 0.4f);
            t.OnComplete(()=> isRotaEnd = true);
        }

    }
    /// <summary>
    /// 右转
    /// </summary>
    public void RotateRight()
    {
        if (isRotaEnd)
        {
            isRotaEnd = false;
            transform.Rotate(Vector3.up, 90);
            Quaternion tmpRight = transform.rotation;
            transform.Rotate(Vector3.up, -90);
            Tween t = transform.DORotateQuaternion(tmpRight, 0.4f);
            t.OnComplete(()=> isRotaEnd = true);
        }


    }

    public void ManualMove(float movedir)
    {
        tmpPos += transform.right * movedir * 8 * Time.deltaTime;
        if (movedir > 0.2f)
        {
            nowAnmimator.SetBool("IsLeftMove", false);
            nowAnmimator.SetBool("IsRotaMove", true);
            nowAnmimator.SetBool("IsRightMove", true);

        }
        else if (movedir < -0.2f)
        {
            nowAnmimator.SetBool("IsRightMove", false);
            nowAnmimator.SetBool("IsRotaMove", true);
            nowAnmimator.SetBool("IsLeftMove", true);
        }
        else
        {
            nowAnmimator.SetBool("IsLeftMove", false);
            nowAnmimator.SetBool("IsRightMove", false);
            nowAnmimator.SetBool("IsRotaMove",false);
        }
    }
    /// <summary>
    /// 下滑
    /// </summary>
    public void Slide()
    {
        nowAnmimator.SetBool("IsSilde",true);
        playCharact.center = new Vector3(0,0.5f,0);
        playCharact.height = 1f;
        StartCoroutine(InitSlide());
    }

    IEnumerator InitSlide()
    {
        yield return new WaitForSeconds(1f);
        nowAnmimator.SetBool("IsSilde", false);
        playCharact.center = new Vector3(0, 0.89f, 0);
        playCharact.height = 2f;
        yield return null;
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public void Die()
    {

    }
}
