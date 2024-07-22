using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameStart : MonoSingleton<GameStart>
{
    private GameObject m_obj;
    protected override void Awake()
    {
        base.Awake();
        GameObject.DontDestroyOnLoad(gameObject);
        
        ResourceManager.Instance.Init(this);
        ObjectManager.Instance.Init(transform.Find("RecyclePoolTrs"), transform.Find("SceneTrs"));

        //热更新初始化，使HotPatchManager可使用协程进行服务器数据的获取
        HotPatchManager.Instance.Init(this);

        UIManager.Instance.Init(transform.Find("UIRoot") as RectTransform, transform.Find("UIRoot/WndRoot") as RectTransform, transform.Find("UIRoot/UICamera").GetComponent<Camera>(), transform.Find("UIRoot/EventSystem").GetComponent<EventSystem>());
    }
    // Use this for initialization
    void Start ()
    {
        UIManager.Instance.PopUpWnd(ConStr.HOTFIX,resource:true);
    }

    /// <summary>
    /// 进入游戏
    /// </summary>
    /// <param name="progress">进度条显示</param>
    /// <param name="text">下载、更新等速度文本内容</param>
    /// <returns></returns>
    public IEnumerator StartGame(Image progress, Text text)
    {
        progress.fillAmount = 0;
        yield return null;
        text.text = "加载本地数据... ...";

        AssetBundleManager.Instance.LoadAssetBundleConfig();
        progress.fillAmount = 0.2f;
        yield return null;
        text.text = "加载数据表... ...";

        LoadConfiger();
        progress.fillAmount = 0.7f;
        yield return null;
        text.text = "加载配置... ...";

        RegisterUI();
        progress.fillAmount = 0.9f;
        yield return null;
        text.text = "初始化地图... ...";//text的内容根据需求自定义设定填写即可

        GameMapManager.Instance.Init(this);//初始化完成
        progress.fillAmount = 1f;
        yield return null;
    }

    //注册UI窗口
    void RegisterUI()
    {
        UIManager.Instance.Register<MenuUi>(ConStr.MENUPANEL);
        UIManager.Instance.Register<LoadingUi>(ConStr.LOADINGPANEL);
        UIManager.Instance.Register<HotFixUi>(ConStr.HOTFIX);
    }

    //加载配置表
    void LoadConfiger()
    {
        //ConfigerManager.Instance.LoadData<MonsterData>(CFG.TABLE_MONSTER);
        //ConfigerManager.Instance.LoadData<BuffData>(CFG.TABLE_BUFF);
    }
	
	// Update is called once per frame
	void Update ()
    {
        UIManager.Instance.OnUpdate();
	}

    /// <summary>
    /// 打开热更确认界面
    /// </summary>
    /// <param name="title"></param>
    /// <param name="des"></param>
    /// <param name="confirmAction"></param>
    /// <param name="cancleAction"></param>
    public static void OpenCommonConfirm(string title, string des, UnityEngine.Events.UnityAction confirmAction, UnityEngine.Events.UnityAction cancleAction)
    {
        GameObject commonObj = GameObject.Instantiate(Resources.Load<GameObject>("CommonConfirm")) as GameObject;
        commonObj.transform.SetParent(UIManager.Instance.m_WndRoot, false);
        CommonConfirm commonItem = commonObj.GetComponent<CommonConfirm>();
        commonItem.Show(title, des, confirmAction,cancleAction);
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        ResourceManager.Instance.ClearCache();
        Resources.UnloadUnusedAssets();
        Debug.Log("清空编辑器缓存");
#endif
    }
}
