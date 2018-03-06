using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour {

    FloorPosChange[] childrenFloors;    //子物体上的脚本集合
    public bool isInit = true;          //是否能够进行初始化标识
    public bool initState = true;       //物体的初始化状态
    public int number;                  //这个地板的编号
    public NameTag nowTag;              //这个物体的标签
    FloorPosChange[] nowObsFloors;      //现在物体上陷阱的脚本集合
    public GameObject nowObs;           //现在的陷阱
    public ObsTag nowObsTag;            //现在的障碍标识
    public float time = 1.2f;
    void Awake ()
    {
        childrenFloors = GetComponentsInChildren<FloorPosChange>();
    }
	

	void Update () {
		
	}
    /// <summary>
    /// 协程进行初始化
    /// </summary>
    /// <returns></returns>
    public IEnumerator Init()
    {
        yield return new WaitForSeconds(time);
        gameObject.transform.position = new Vector3(0, 200, 1);
        for (int i=0;i< childrenFloors.Length;i++)
        {
            childrenFloors[i].SetPropPos();
        }
        if (nowObs!=null)
        {
            if (nowObsTag == ObsTag.Jump)
            {
                GameMode.Instance.obsJumps.Add(nowObs);
            }
            else
            {
                GameMode.Instance.obsDowns.Add(nowObs);
            }
            nowObs.transform.position = new Vector3(0, 200, 1);
            nowObsFloors = null;
            nowObs = null;
        }
        yield return null;
    }
    /// <summary>
    /// 改变子物体的坐标以及旋转同时打开绕物体旋转的开关
    /// </summary>
    public void ChangeChildrensPos()
    {
        RandomGreatProp();
        if (childrenFloors!=null)
        {
            for (int i=0;i<childrenFloors.Length;i++)
            {
                childrenFloors[i].ChangePos();
                childrenFloors[i].ChangeRotate();
                childrenFloors[i].isRotate = true;
            }
        }
        initState = false;
    }
    /// <summary>
    /// 初始化子物体的坐标，并关闭旋转开关
    /// </summary>
    public void InitChildrenPos()
    {
        if (childrenFloors != null)
        {
            for (int i = 0; i < childrenFloors.Length; i++)
            {
                childrenFloors[i].Init();
                childrenFloors[i].isRotate = false;
            }
        }
        initState = true;

        if (nowObsFloors!=null)
        {
            for (int i = 0; i < nowObsFloors.Length; i++)
            {
                nowObsFloors[i].Init();
                nowObsFloors[i].isRotate = false;
            }
        }
    }
    /// <summary>
    /// 随机生成物体障碍，金币，磁铁
    /// </summary>
    public void RandomGreatProp()
    {
        int greatType = Random.Range(1,11);
        if (greatType == 1)            //障碍OBS
        {
            if (GameMode.Instance.roundObs<0)
            {
                GreatObs();
                GameMode.Instance.roundObs = 5;
            }
        }
        else if(greatType > 3 && greatType < 7)
        {
            int propType = Random.Range(1,51);
            if (propType> 31&& propType<51)          //磁铁
            {
                int pos = Random.Range(0, 3);
                childrenFloors[pos].GreatMagnet();
            }
            else
            {
                int propNumber = Random.Range(1,4);
                for (int i=0;i<propNumber;i++)
                {
                    childrenFloors[i].GreatGold();
                }
            }
        }
    }
    /// <summary>
    /// 生成新的障碍
    /// </summary>
    public void GreatObs()
    {
        if (nowTag != NameTag.FloorTag)
        {
            return;
        }

        int obsType = Random.Range(1,3);
        if (obsType == 1)
        {
            if (GameMode.Instance.obsJumps.Count > 0)
            {
                nowObs = GameMode.Instance.obsJumps[0];
                GameMode.Instance.obsJumps.RemoveAt(0);
            }
            else
            {
                nowObs = Instantiate(GameMode.Instance.obsJump, transform.position, transform.rotation);
            }
            nowObs.transform.position = transform.position + transform.up;
            nowObs.transform.rotation = transform.rotation;
            nowObsFloors = nowObs.GetComponentsInChildren<FloorPosChange>();
            nowObsTag = ObsTag.Jump;
        }
        else
        {
            if (GameMode.Instance.obsDowns.Count > 0)
            {
                nowObs = GameMode.Instance.obsDowns[0];
                GameMode.Instance.obsDowns.RemoveAt(0);
            }
            else
            {
                nowObs = Instantiate(GameMode.Instance.obsDown, transform.position, transform.rotation);
            }
            nowObs.transform.position = transform.position + transform.up*3;
            nowObs.transform.rotation = transform.rotation;
            nowObsFloors = nowObs.GetComponentsInChildren<FloorPosChange>();
            nowObsTag = ObsTag.Down;
        }

        if (nowObsFloors!=null)
        {
            for (int i =0;i< nowObsFloors.Length;i++)
            {
                nowObsFloors[i].parentObj = gameObject;
                nowObsFloors[i].ChangePos();
                nowObsFloors[i].ChangeRotate();
                nowObsFloors[i].isRotate = true;
            }
        }
    }
}
