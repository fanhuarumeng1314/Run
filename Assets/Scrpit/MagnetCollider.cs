using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagnetCollider : MonoBehaviour {

    [HideInInspector]
    public GameObject nowParent;                //现在的父物体

    public Vector3 nowPos;                      //现在的位置
    public bool isRotaAround = false;           //是否饶轴旋转
    public bool isRota = false;                 //是否绕自己旋转
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
    }
    /// <summary>
    /// 改变磁铁位置
    /// </summary>
    public void ChangeMagnetPos()
    {
        int yDistance = Random.Range(3, 10);
        int xDistance = Random.Range(3, 10);
        transform.position += transform.up * yDistance + transform.right * xDistance;
        isRotaAround = true;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitMagnet()
    {
        isRotaAround = false;
        transform.rotation = Quaternion.identity;
        transform.DOMove(nowPos, 0.4f);
        isRota = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayController>();
        if (player)
        {
            var tmpFloor = nowParent.GetComponent<FloorPosChange>();
            tmpFloor.nowChilderProp = null;
            GameMode.Instance.magnetTime = 10.0f;
            gameObject.SetActive(false);
            GameMode.Instance.Magnets.Add(gameObject);
        }
    }
}
