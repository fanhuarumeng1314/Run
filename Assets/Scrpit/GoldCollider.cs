using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldCollider : MonoBehaviour {

    [HideInInspector]
    public GameObject nowParent;                //现在的父物体
    public GameObject nowPlayer;                //玩家物体
    public float distance = 0f;                 //与玩家的距离

    public Vector3 nowPos;                      //现在的位置
    public bool isRotaAround = false;           //是否饶轴旋转
    public bool isRota = false;                 //是否绕自己旋转
    public bool isGet = true;                   //是否能够被获取
    private void Update()
    {
        if (isRotaAround)
        {
            transform.RotateAround(nowParent.transform.position, nowParent.transform.forward, 30 * Time.deltaTime);
        }

        if (isRota)
        {
            transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        }

        distance = (transform.position - nowPlayer.transform.position).magnitude;
        if (distance<=10 && GameMode.Instance.isMagnet == true && isGet)
        {
            isGet = false;
            var tmpFloor = nowParent.GetComponent<FloorPosChange>();
            tmpFloor.nowChilderProp = null;
            transform.parent = nowPlayer.transform;
            Tween t =  transform.DOLocalMove(Vector3.zero, 1.0f);
            t.OnComplete(()=> GoldSave());

        }
    }
    /// <summary>
    /// 改变金币位置
    /// </summary>
    public void ChangeGoldPos()
    {
        int yDistance = Random.Range(3, 10);
        int xDistance = Random.Range(3, 6);
        transform.position += transform.up * yDistance + transform.right * xDistance;
        isRotaAround = true;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitGold()
    {
        isRotaAround = false;
        transform.rotation = Quaternion.identity;
        transform.DOMove(nowPos,0.4f);
        isRota = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayController>();
        if (player)
        {
            GameMode.Instance.GreatColdAudio();
            GameMode.Instance.goldNumber++;
            GameMode.Instance.SetGoldTxt();
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 动画结束后保存这个金币的引用
    /// </summary>
    public void GoldSave()
    {
        transform.parent = null;
        GameMode.Instance.Golds.Add(gameObject);
    }
}
