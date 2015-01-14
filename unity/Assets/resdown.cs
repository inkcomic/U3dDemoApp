using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class resdown : MonoBehaviour
{
    private static resdown _inst = null;
    public static resdown inst
    {
        get
        {
            return _inst;
        }
    }

    void Awake()
    {
        _inst = this;
    }
    // Use this for initialization
    void Start()
    {
        List<string> wantdownGroup = new List<string>();
        GetCheckGroups(wantdownGroup);
        ResmgrNative.Instance.BeginInit("http://192.168.1.200:8080/publish/"/*"http://lightszero.github.io/publish/"*/, OnInitFinish, wantdownGroup);
        strState = "检查资源";

    }
    void GetCheckGroups(List<string> oGroups)
    {
        oGroups.Add("demo");
        oGroups.Add("test1");
        oGroups.Add("test1_ios");
    }
    bool indown = false;
    void OnInitFinish(System.Exception err)
    {
        //string sss=Application.persistentDataPath;
        if (err == null)
        {
            ResmgrNative.Instance.taskState.Clear();
            strState = "检查资源完成";
            List<string> wantdownGroup = new List<string>();
            GetCheckGroups(wantdownGroup);
            var downlist = ResmgrNative.Instance.GetNeedDownloadRes(wantdownGroup);
            foreach (var d in downlist)
            {
                d.Download(null);
            }
            ResmgrNative.Instance.WaitForTaskFinish(DownLoadFinish);
            indown = true;
        }
        else
            strState = null;
    }
    void DownLoadFinish()
    {
        indown = false;
        strState = "更新完成";


        GameState.inst.ResourceUpdateDone();

        foreach (var file in ResmgrNative.Instance.verLocal.groups["test1_ios"].listfiles.Values)
        {
            if(file.FileName.Contains(".jpg"))
            {
                file.BeginLoadTexture2D((tex, tag) =>
                    {
                        loadedTexs.Add(tex);
                    });
            }
        }
       
        //加载打散场景例子
        /*Engine001.Instance.LoadLayout("test1/prefabs", "scene", (obj) =>
            {
                Debug.Log("we load the scene:" + obj.name);
            }
        );*/
        //engine.LoadLayout()

    }
    //Engine001 engine;
    // Update is called once per frame
    void Update()
    {
        ResmgrNative.Instance.Update();
        
        if (indown)
        {
            strState = "have downloaded:" + ResmgrNative.Instance.taskState.downloadcount + " /Total " + ResmgrNative.Instance.taskState.taskcount;
        }
        
        GameState.inst.Update();  
    }
    string strState = "";
    List<Texture2D> loadedTexs = new List<Texture2D>();
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 100), strState);
        for(int i=0;i<loadedTexs.Count;i++)
        {
            GUI.DrawTexture(new Rect(0, 50 + i * 50, 50, 50), loadedTexs[i]);
        }

        GameState.inst.OnGUI();
    }

    public void StartChildCoroutine(IEnumerator coroutineMethod)
    {
        StartCoroutine(coroutineMethod);
    }
}
