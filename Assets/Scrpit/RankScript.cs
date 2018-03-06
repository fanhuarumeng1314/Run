using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class RankScript : MonoBehaviour {

    public Text rankOne;
    public Text rankTwo;
    public Text rankThree;
    Text[] rank = new Text[3];
    string[] vauleRank = new string[3];
    public string fuckNumber;
    void Start ()
    {
        rank[0] = rankOne;
        rank[1] = rankTwo;
        rank[2] = rankThree;
        var csList = LoadFile(Application.persistentDataPath, "Run.txt");
        if (csList == null)
        {
            csList = LoadFile(Application.persistentDataPath, "Run.txt");
        }
        Debug.Log(Application.persistentDataPath + "sasss");
        int m = 0;
        foreach (string str in csList)
        {
            vauleRank[m] = str;
            m++;
        }

        for (int i=0;i<3;i++)
        {
            rank[i].text = string.Format("第{0}名分数:{1}", (i + 1), vauleRank[i].ToString());
        }
    }
	
	// Update is called once per frame
	void Update () {
		
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
                sw.WriteLine("0");
            }
            //关闭流
            sw.Close();
            //销毁流
            sw.Dispose();
            return null;
        }

        try
        {
            sr = File.OpenText(path + "//" + name);
        }
        catch (Exception e)
        {
            //路径与名称未找到文件则直接返回空
            return null;
        }

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
}
