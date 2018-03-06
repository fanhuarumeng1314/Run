using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum NameTag         //地板标识
{
    FloorTag,
    RotaTag,
    TrapTag,
}
public enum PropTag         //道具标识
{
    None,
    Gold,
    Magnet,
}
public enum ObsTag          //障碍标识
{
    None,
    Jump,
    Down,
}
public class GameMode : MonoBehaviour {

    public static GameMode Instance;
    public bool gameState = true;           //游戏状态 true，游戏进行；false，暂停。
    public bool isGreatSettled;             //是否生成落脚点
    public GameObject nowSettled;           //现在玩家脚下的物体，用于游戏暂停
    public GameObject tmpSettled;           //临时生成的落脚点物体
    public int goldNumber;                  //金币数量
    public int[] rankArray = new int[3];    //排名信息
    public float magnetTime = 0f;           //磁铁状态持续时间
    public bool isMagnet;                   //是否有磁铁状态
    public Text goldTxt;                    //金币数量显示的文本UI
#region//音效相关
    public GameObject audioPrefeb;          //音量物体
    public AudioSource audioBGPlay;         //音效播放组件
    public AudioClip tishiClip;             //提示音效
    public AudioClip bgClip;                //背景音效
    public AudioClip goldClip;              //金币碰撞音效
#endregion
#region//生成道路流程
    public int floorNumber;
    public GameObject guideObj;             //引导物体
    public GameObject mapFloor;             //地板
    public GameObject rotaFloor;            //转向地板
    public GameObject trapFloor;            //陷阱
    public GameObject obsJump;              //需要跳跃的障碍
    public GameObject obsDown;              //需要下蹲的障碍
    [HideInInspector]
    public List<FloorCollider> tmpFloors;   //封装现在游戏中所有的地板
    public List<GameObject> floors;         //封装所有备用地板物体
    public List<GameObject> nowFloors;      //现在能够进行动画效果的地板物体
    public List<GameObject> rotaFloors;     //封装所有的备用转向物体
    public List<GameObject> trapFloors;     //封装所有的备用陷阱物体
    public List<GameObject> obsJumps;       //封装所有的备用的需要跳跃的障碍
    public List<GameObject> obsDowns;       //封装所有的备用的需要下蹲的障碍
    int round;                              //上一个道路的回合数
    [HideInInspector]
    public int roundObs;                    //上一次障碍生成的回合数
#endregion
    public GameObject Gold;                 //金币
    public GameObject Magnet;               //磁铁

    public List<GameObject> Golds;          //封装备用的金币物体
    public List<GameObject> Magnets;        //封装备用的磁铁物体
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        int m = 0;
        var csList = LoadFile(Application.persistentDataPath, "Run.txt");
        if (csList == null)
        {
            csList = LoadFile(Application.persistentDataPath, "Run.txt");
        }
        foreach (string str in csList)
        {
            rankArray[m] = int.Parse(str);
            m++;
        }
        #region//读取物体相关
        guideObj = Resources.Load("GuideObj") as GameObject;
        mapFloor = Resources.Load("Map_Floor") as GameObject;
        rotaFloor = Resources.Load("RotaFloor") as GameObject;
        trapFloor = Resources.Load("TrapFloor") as GameObject;
        Gold = Resources.Load("Gold") as GameObject;
        Magnet = Resources.Load("MagnetObj") as GameObject;
        obsJump = Resources.Load("ObsJump") as GameObject;
        obsDown = Resources.Load("ObsDown") as GameObject;
        guideObj = Instantiate(guideObj, new Vector3(0, -0.5f, 0), Quaternion.identity) as GameObject;
        audioPrefeb = Resources.Load("AudioPrefeb") as GameObject;
        tishiClip = Resources.Load("按钮类7", typeof(AudioClip)) as AudioClip;
        bgClip = Resources.Load("快乐嬉戏", typeof(AudioClip)) as AudioClip;
        goldClip = Resources.Load("叮－清脆", typeof(AudioClip)) as AudioClip;
        #endregion
        GreatBGAudioPlay();
        for (int i = 0; i < 25; i++)
        {
            var tmpMapFloor = Instantiate(mapFloor, guideObj.transform.position, guideObj.transform.rotation) as GameObject;
            var tmp_FloorColider = tmpMapFloor.GetComponent<FloorCollider>();
            tmp_FloorColider.nowTag = NameTag.FloorTag;                     //标记自己的标签
            tmp_FloorColider.number = floorNumber;                          //初始编号赋值
            floorNumber++;
            tmpMapFloor.name += i.ToString();
            guideObj.transform.position += guideObj.transform.forward;      //生成一个道路移动一下位置
            tmpFloors.Add(tmp_FloorColider);
        }
#region//先生成一部分物体进行备用
        for (int i = 0; i < 20; i++)
        {
            var tmpMapFloor = Instantiate(mapFloor, new Vector3(0, 200, 1), Quaternion.identity) as GameObject;
            var tmp_FloorColider = tmpMapFloor.GetComponent<FloorCollider>();
            tmp_FloorColider.nowTag = NameTag.FloorTag;                     //标记自己的标签
            floors.Add(tmpMapFloor);
        }

        for (int i = 0;i<5;i++)
        {
            GameObject rotaObj = Instantiate(rotaFloor, new Vector3(0, 200, 1), Quaternion.identity);
            var tmp_FloorColider = rotaObj.GetComponent<FloorCollider>();
            tmp_FloorColider.nowTag = NameTag.RotaTag;          //转向标识
            rotaFloors.Add(rotaObj);
        }

        for (int i=0;i<20;i++)
        {
            GameObject trapObj = Instantiate(trapFloor, new Vector3(0, 200, 1), Quaternion.identity);
            trapObj.name += i.ToString();
            var tmp_FloorColider = trapObj.GetComponent<FloorCollider>();
            tmp_FloorColider.nowTag = NameTag.TrapTag;          //陷阱标识
            trapFloors.Add(trapObj);
        }
#endregion
        for (int i=0;i<30;i++)
        {
            var tmpMapFloor = Instantiate(mapFloor, guideObj.transform.position, guideObj.transform.rotation) as GameObject;
            var tmp_FloorColider = tmpMapFloor.GetComponent<FloorCollider>();
            tmp_FloorColider.nowTag = NameTag.FloorTag;                     //标记自己的标签
            tmp_FloorColider.number = floorNumber;                          //初始编号赋值
            floorNumber++;
            tmp_FloorColider.ChangeChildrensPos();
            nowFloors.Add(tmpMapFloor);
            guideObj.transform.position += guideObj.transform.forward;      //生成一个道路移动一下位置
            tmpFloors.Add(tmp_FloorColider);
        }
    }
	
	void Update ()
    {
        //if (!gameState && tmpSettled==null)
        //{
        //    tmpSettled = Instantiate(nowSettled.gameObject,nowSettled.transform.position,nowSettled.transform.rotation);
        //    Destroy(tmpSettled.GetComponent<FloorCollider>());
        //    isGreatSettled = true;
        //}
        if (!gameState)
        {
            return;
        }
        if (tmpSettled!=null && isGreatSettled)
        {
            Invoke("DeletaTmpSettled",1f);
            isGreatSettled = false;
        }
        if (magnetTime > 0)
        {
            magnetTime -= Time.deltaTime;
            isMagnet = true;
        }
        else
        {
            isMagnet = false;
        }
		
	}
    /// <summary>
    /// 删除临时落脚物体
    /// </summary>
    public void DeletaTmpSettled()
    {
        Destroy(tmpSettled);
        tmpSettled = null;
    }
    /// <summary>
    /// 设置下一个生成物体的类型
    /// </summary>
    public void SetGreatMap()
    {
        int randomSeed = Random.Range(1,26);
        //randomSeed = 1;
        if (randomSeed <= 20)                                   //道路
        {
            GreatMapFloor();
        }
        else if (randomSeed > 20 && randomSeed <= 23)           //转向
        {
            if (round <= 0)
            {
                round = 10;
                GreatRotaFloor();
            }
            else
            {
                GreatMapFloor();
            }
            
        }
        else                                                    //陷阱
        {
            GreatTrapFloor();
        }
        round--;
        roundObs--;
    }
    /// <summary>
    /// 生成新的地板
    /// </summary>
    public void GreatMapFloor()
    {
        GameObject tmp_Floor = null;
        if (floors.Count > 0)
        {
            tmp_Floor = floors[0];
            //tmp_Floor.SetActive(true);
            tmp_Floor.transform.position = guideObj.transform.position;
            tmp_Floor.transform.rotation = guideObj.transform.rotation;
            floors.RemoveAt(0);

        }
        else
        {
            tmp_Floor = Instantiate(mapFloor, guideObj.transform.position, guideObj.transform.rotation) as GameObject;
        }
        nowFloors.Add(tmp_Floor);
        FloorCollider tmp_FloorCollider = tmp_Floor.GetComponent<FloorCollider>();
        tmp_FloorCollider.ChangeChildrensPos();
        tmp_FloorCollider.number = floorNumber;             //编号赋值
        floorNumber++;
        guideObj.transform.position += guideObj.transform.forward;
        tmpFloors.Add(tmp_FloorCollider);

    }
    /// <summary>
    /// 生成转向地板
    /// </summary>
    public void GreatRotaFloor()
    {
        GameObject rotaObj;
        FloorCollider tmp_FloorColider = null;
        guideObj.transform.position += guideObj.transform.forward;
        if (rotaFloors.Count > 0)           //保存的列表当中有进行取出操作
        {
            rotaObj = rotaFloors[0];
            //rotaObj.SetActive(true);
            tmp_FloorColider = rotaObj.GetComponent<FloorCollider>();
            tmp_FloorColider.nowTag = NameTag.RotaTag;          //转向标识
            rotaObj.transform.position = guideObj.transform.position;
            rotaObj.transform.rotation = guideObj.transform.rotation;
            tmp_FloorColider.isInit = true;
            tmp_FloorColider.ChangeChildrensPos();
            rotaFloors.RemoveAt(0);
        }
        else
        {
            rotaObj = Instantiate(rotaFloor,guideObj.transform.position,guideObj.transform.rotation);
            tmp_FloorColider = rotaObj.GetComponent<FloorCollider>();
            tmp_FloorColider.nowTag = NameTag.RotaTag;          //转向标识
            tmp_FloorColider.ChangeChildrensPos();
        }

        int rotaModel = Random.Range(1, 3);
        if (rotaModel == 1)             //左转向
        {
            guideObj.transform.Rotate(Vector3.up, -90f);
            guideObj.transform.position += guideObj.transform.forward*2;
        }
        else                            //右转向
        {
            guideObj.transform.Rotate(Vector3.up, 90f);
            guideObj.transform.position += guideObj.transform.forward * 2;
        }
        nowFloors.Add(rotaObj);     //将这个物体添加进待处理列表
        tmp_FloorColider.number = floorNumber;              //编号赋值
        floorNumber++;
        tmpFloors.Add(tmp_FloorColider);                
    }
    /// <summary>
    /// 生成陷阱
    /// </summary>
    public void GreatTrapFloor()
    {
        GameObject trapObj;
        FloorCollider tmp_FloorCollider;
        if (trapFloors.Count > 0)
        {
            trapObj = trapFloors[0];
            //trapObj.SetActive(true);
            trapObj.transform.position = guideObj.transform.position;
            trapObj.transform.rotation = guideObj.transform.rotation;
            tmp_FloorCollider = trapObj.GetComponent<FloorCollider>();
            tmp_FloorCollider.nowTag = NameTag.TrapTag;         //陷阱标识
            tmp_FloorCollider.isInit = true;
            trapFloors.RemoveAt(0);
        }
        else
        {
            trapObj = Instantiate(trapFloor, guideObj.transform.position, guideObj.transform.rotation) as GameObject;
            tmp_FloorCollider = trapObj.GetComponent<FloorCollider>();
            tmp_FloorCollider.nowTag = NameTag.TrapTag;         //陷阱标识
        }

        int trapType = Random.Range(1, 4);

        if (trapType == 1)
        {
            trapObj.transform.position += trapObj.transform.right;
        }
        else if (trapType == 2)
        {
            trapObj.transform.position -= trapObj.transform.right;
        }
        guideObj.transform.position += guideObj.transform.forward * 3;
        tmp_FloorCollider.ChangeChildrensPos();
        nowFloors.Add(trapObj);
        tmp_FloorCollider.number = floorNumber;                 //编号赋值
        floorNumber++;
        tmpFloors.Add(tmp_FloorCollider);
    }
    /// <summary>
    /// 从动画列表取出一个物体进行动画还原
    /// </summary>
    public void InitFloor()
    {
        if (nowFloors.Count > 0)
        {
            FloorCollider tmp_FloorCollider = nowFloors[0].GetComponent<FloorCollider>();
            tmp_FloorCollider.isInit = true;
            tmp_FloorCollider.InitChildrenPos();
            nowFloors.RemoveAt(0);
        }
    }

    public void TmpInit(FloorCollider tmpFloor)
    {
        int nowIndex = 0;                   //当前物体列表中的索引
        foreach (var floor in tmpFloors)
        {
            if (floor.number==tmpFloor.number)
            {
                nowIndex = tmpFloors.IndexOf(floor);
            }
        }

        for (int i = nowIndex; i >= 0; i--)
        {
            SetGreatMap();
            InitFloor();
            FloorCollider tmp_FloorCollider = tmpFloors[0];
            StartCoroutine(tmp_FloorCollider.Init());
            if (tmp_FloorCollider.nowTag == NameTag.FloorTag)
            {
                floors.Add(tmp_FloorCollider.gameObject);
            }
            else if (tmp_FloorCollider.nowTag == NameTag.RotaTag)
            {
                rotaFloors.Add(tmp_FloorCollider.gameObject);
            }
            else
            {
                trapFloors.Add(tmp_FloorCollider.gameObject);
            }
            tmpFloors.RemoveAt(0);
        }

    }

    public void SetGoldTxt()
    {
        goldTxt.text = goldNumber.ToString();
    }

    public ArrayList LoadFile(string path, string name)
    {
        //使用流的形式读取
        StreamReader sr = null;
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();

            for (int i = 0; i < 3; i++)
            {
                //以行的形式写入信息
                sw.WriteLine("2金币" + i.ToString());
            }
            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
            return null;
        }
        sr = File.OpenText(path + "//" + name);
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            //一行一行的读取
            //将每一行的内容存入数组链表容器中
            arrlist.Add(line);
        }
        //关闭流
        sr.Close();
        //销毁流
        sr.Dispose();
        //将数组链表容器返回
        return arrlist;
    }

    public void DeleteFile(string path, string name)
    {
        File.Delete(path + "//" + name);

    }

    public void GreatFile(string path, string name, string data)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(data);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }
    /// <summary>
    /// 排行榜文件排序
    /// </summary>
    public void SortRank()
    {
        for (int i=0;i<3;i++)
        {
            if (rankArray[i]<goldNumber)
            {
                if (i!=2)
                {
                    for (int j = 1; j >= i; j--)
                    {
                        rankArray[j + 1] = rankArray[j];
                    }
                }
                rankArray[i] = goldNumber;                
                break;
            }
        }
    }
    /// <summary>
    /// 刷新排行榜文件
    /// </summary>
    public void UpdataRankFile()
    {
        DeleteFile(Application.persistentDataPath, "Run.txt");
        for (int i = 0;i<3;i++)
        {
            GreatFile(Application.persistentDataPath, "Run.txt",rankArray[i].ToString());
        }
    }
    /// <summary>
    /// 死亡地板初始化
    /// </summary>
    public void DieFloorInit()
    {
        if (tmpFloors.Count>0)
        {
            for (int i=0;i<tmpFloors.Count;i++)
            {
                tmpFloors[i].gameObject.SetActive(false);
            }
        }
    }

    public void GreatSetlled()
    {
        tmpSettled = Instantiate(nowSettled.gameObject, nowSettled.transform.position, nowSettled.transform.rotation);
        Destroy(tmpSettled.GetComponent<FloorCollider>());
        isGreatSettled = true;
    }
    /// <summary>
    /// 创建背景音乐播放器
    /// </summary>
    public void GreatBGAudioPlay()
    {
        GameObject tmpAudioPlay = Instantiate(GameMode.Instance.audioPrefeb,transform);
        audioBGPlay = tmpAudioPlay.GetComponent<AudioSource>();
        audioBGPlay.volume = 1;
        audioBGPlay.loop = true;
        audioBGPlay.clip = GameMode.Instance.bgClip;
        audioBGPlay.Play();
    }
    /// <summary>
    /// 创建新的金币音效
    /// </summary>
    public void GreatColdAudio()
    {
        GameObject tmpAudioPlay = Instantiate(GameMode.Instance.audioPrefeb, transform);
        audioBGPlay = tmpAudioPlay.GetComponent<AudioSource>();
        audioBGPlay.volume = 1;
        audioBGPlay.loop = false;
        audioBGPlay.clip = GameMode.Instance.goldClip;
        audioBGPlay.Play();
        Destroy(tmpAudioPlay,2.0f);
    }
}
