using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloorPosChange : MonoBehaviour {

    public Vector3 nowLocalPos;             //现在的相对位置
    public Quaternion nowLocalRotate;       //现在的相对旋转
    public GameObject parentObj;            //现在的父物体
    public GameObject nowChilderProp;       //自己生成的道具物体
    public GameObject player;               //玩家物体
    PropTag nowProp;                        //当前生成物体的标识
    public bool isRotate = false;           //是否能够饶轴旋转
    private void Awake()
    {
        nowLocalPos = transform.localPosition;
        nowLocalRotate = transform.localRotation;
        parentObj = transform.parent.gameObject;
        player = GameObject.FindObjectOfType<PlayCharacter>().gameObject;
    }

    void Start()
    {
        //player = GameObject.FindObjectOfType<PlayCharacter>().gameObject;
    }

    void Update()
    {
        if (isRotate)
        {
            transform.RotateAround(parentObj.transform.position, parentObj.transform.forward,30*Time.deltaTime);
        }
    }
    /// <summary>
    /// 初始化当前物体的相对坐标以及旋转
    /// </summary>
    public void Init()
    {
        transform.DOLocalMove(nowLocalPos, 0.4f);
        transform.DOLocalRotateQuaternion(nowLocalRotate, 0.4f);
        if (nowChilderProp!=null)
        {
            if (nowProp == PropTag.Gold)
            {
                var tmpGold = nowChilderProp.GetComponent<GoldCollider>();
                tmpGold.InitGold();
            }
            else
            {
                var tmpMagent = nowChilderProp.GetComponent<MagnetCollider>();
                tmpMagent.InitMagnet();
            }
        }
    }
    /// <summary>
    /// 改变这个物体的相对位置
    /// </summary>
    public void ChangePos()
    {
        float randomX = Random.Range(-10,10);
        float randomY = Random.Range(-10,10);
        while (Mathf.Abs(randomX)<=6.0f || Mathf.Abs(randomY) <= 6.0f)//改变的坐标必须离初始位置大于6个单位的距离
        {
            randomX = Random.Range(-10, 10);
            randomY = Random.Range(-10, 10);
        }
        transform.localPosition += transform.right * randomX;
        transform.localPosition += transform.up * randomY;
    }
    /// <summary>
    /// 改变这个物体的相对旋转
    /// </summary>
    public void ChangeRotate()
    {
        transform.Rotate(transform.right, Random.Range(0, 180));
        transform.Rotate(transform.forward, Random.Range(0, 180));
        transform.Rotate(transform.up, Random.Range(0, 180));
    }
    /// <summary>
    /// 生成金币
    /// </summary>
    public void GreatGold()
    {
        if (GameMode.Instance.Golds.Count > 0)
        {
            nowChilderProp = GameMode.Instance.Golds[0];
            nowChilderProp.SetActive(true);
            nowChilderProp.transform.position = transform.position + transform.up * 1.5f;
            GameMode.Instance.Golds.RemoveAt(0);
        }
        else
        {
            nowChilderProp = Instantiate(GameMode.Instance.Gold, transform.position + transform.up * 1.5f, transform.rotation);

        }
        GoldCollider tmpGoldCollider = nowChilderProp.GetComponent<GoldCollider>();
        tmpGoldCollider.isGet = true;
        tmpGoldCollider.nowPlayer = player;
        tmpGoldCollider.ChangeGoldPos();
        tmpGoldCollider.nowPos = transform.position + transform.up * 1.5f;
        tmpGoldCollider.nowParent = gameObject;
        nowProp = PropTag.Gold;                             //金币标识
    }
    /// <summary>
    /// 生成磁铁
    /// </summary>
    public void GreatMagnet()
    {
        if (GameMode.Instance.Magnets.Count > 0)
        {
            nowChilderProp = GameMode.Instance.Magnets[0];
            nowChilderProp.SetActive(true);
            nowChilderProp.transform.position = transform.position + transform.up * 1.5f;
            GameMode.Instance.Magnets.RemoveAt(0);
        }
        else
        {
            nowChilderProp = Instantiate(GameMode.Instance.Magnet, transform.position + transform.up * 1.5f, transform.rotation);
        }
        var magentCollider = nowChilderProp.GetComponent<MagnetCollider>();
        magentCollider.nowParent = gameObject;
        magentCollider.nowPos = transform.position + transform.up * 1.5f;
        magentCollider.ChangeMagnetPos();
        nowProp = PropTag.Magnet;                         //磁铁标识
    }
    /// <summary>
    /// 设置道具物体的坐标
    /// </summary>
    public void SetPropPos()
    {
        if (nowChilderProp != null)
        {
            nowChilderProp.transform.position = new Vector3(0,200,1);
            if (nowProp == PropTag.Gold)
            {
                GameMode.Instance.Golds.Add(nowChilderProp);
            }
            else
            {
                GameMode.Instance.Magnets.Add(nowChilderProp);
            }
            nowChilderProp = null;
        }
    }
}
