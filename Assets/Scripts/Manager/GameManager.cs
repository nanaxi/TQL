using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using ProtoBuf;
using System.IO;
using System;
public delegate void SetSprite(Sprite sprite, int num = 0);

public struct _PlayerInfo
{
    /// <summary>
    /// 玩家名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 玩家唯一ID
    /// </summary>
    public uint ID;
    /// <summary>
    /// 玩家IP
    /// </summary>
    public string IP;
    /// <summary>
    /// 玩家头像
    /// </summary>
    public string Head;
    /// <summary>
    /// 玩家钻石
    /// </summary>
    public uint Diamond;
    /// <summary>
    /// 钱
    /// </summary>
    public int Money;

    public int sex;


    public _PlayerInfo(string Name, uint ID, string IP, string Head, uint Diamond, int Money, int sex)
    {
        this.Name = Name;
        this.ID = ID;
        this.IP = IP;
        this.Head = Head;
        this.Diamond = Diamond;
        this.Money = Money;
        this.sex = sex;
    }
}


public class GameManager : MonoBehaviour
{
    public RectTransform Canvas;

    public static GameManager GM;
    public DataStorage DS;
    public bool ingame = false;
    public bool GameEnd = false;
    public QueryInfoRsp combatGainRsp;
    Dictionary<string, Sprite> HeadCaChe = new Dictionary<string, Sprite>();
    /// <summary>
    /// 屏幕高的一半
    /// </summary>
    public float SHeight;
    public float SWide;
    public string JsonPath
    {
        get
        {
            string path = null;
            if (Application.platform == RuntimePlatform.IPhonePlayer)//判断平台
            {
                //path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);//ios 平台 就会获取documents路径
                //path = path.Substring(0, path.LastIndexOf('/')) + "/Documents/";
                path = Application.persistentDataPath;//安卓平台
            }
            else
            {
                path = Application.persistentDataPath;//安卓平台
            }
            return path;
        }
    }
    public string ToName(string base64Str)
    {
        return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64Str));
    }

    /// <summary>
    /// 当前是几人麻将模式就把这个数字改为几人
    /// </summary>
    public static uint PlayerNum = 4;
    public int LastGain0 = 0, LastGain1 = 0, LastGain2 = 0;
    public int ReplayRecordsNum = 0;
    public int ReplayRoundsNum = 0;
    public string MyHead;




    public void GetHead(string url, SetSprite Action, int num = 0)
    {
        if (HeadCaChe.ContainsKey(url))
        {
            Action(HeadCaChe[url], num);
        }
        else
        {
            if (PlayerPrefs.HasKey(url))
            {
                var file = PlayerPrefs.GetString(url);
                StartCoroutine(LocalLoadSprite(file, url, Action, num));
            }
            else
            {
                StartCoroutine(LoadSprite(url, Action, num));
            }
        }
    }
    IEnumerator LocalLoadSprite(string file, string url, SetSprite Action, int num = 0)
    {
        WWW www = new WWW("file://" + file);
        yield return www;
        if (www.texture == null)
        {
            PlayerPrefs.DeleteKey(file);
            yield return LoadSprite(url, Action, num);
        }
        else
        {
            var te = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
            Action(te, num);
            HeadCaChe.Add(url, te);
        }
    }
    IEnumerator LoadSprite(string url, SetSprite Action, int num = 0)
    {
        WWW www = new WWW(url);
        yield return www;
        if (www != null)
            if (www.texture != null)
            {
                var te = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));

                string str = Application.persistentDataPath + GetTimeStr();

                PlayerPrefs.SetString(url, str);
                //File.WriteAllBytes(str, www.texture.EncodeToPNG());
                PlayerPrefs.Save();
                Action(te, num);
                HeadCaChe.Add(url, te);
            }
    }

    void SetHead(Sprite sprite)
    {
        // string url = "";
        //   GameManager.GM.GetHead(url, SetHead);
        Image head = null;

        head.sprite = sprite;
    }

    /// <summary>
    /// 当前游戏所有玩家的数据
    /// </summary>
    public _PlayerInfo[] _AllPlayerData = new _PlayerInfo[4];
    public QueryInfoRsp rsp_Save = null;
    public int GetPlayerNum(uint palyerID)
    {
        for (int i = 0; i < _AllPlayerData.Length; i++)
        {
            if (_AllPlayerData[i].ID == palyerID)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetPlayerSex(uint playerID)
    {
        for (int i = 0; i < _AllPlayerData.Length; i++)
        {
            if (_AllPlayerData[i].ID == playerID)
            {
                return _AllPlayerData[i].sex;
            }
        }
        return 1;
    }
    public string GetPlayerName(uint playerID)
    {
        for (int i = 0; i < _AllPlayerData.Length; i++)
        {
            if (_AllPlayerData[i].ID == playerID)
            {
                return _AllPlayerData[i].Name;
            }
        }
        return "缺省";
    }
    public string GetPlayerIp(uint playerID)
    {
        for (int i = 0; i < _AllPlayerData.Length; i++)
        {
            if (_AllPlayerData[i].ID == playerID)
            {
                return _AllPlayerData[i].IP;
            }
        }
        return "192";
    }
    public void MyPlayerData(AccountLoginRsp Rsp)
    {
        rsp_Save = null;

        _AllPlayerData[0] = new _PlayerInfo(ToName(Rsp.userName), Rsp.charId, Rsp.ip, MyHead, Rsp.diamond, (int)Rsp.gold, (int)GlobalSettings.sex);
        Debug.Log("名字" + _AllPlayerData[0].Name);
    }


    /// <summary>
    /// 客户端进入之后，有玩家进入
    /// </summary>
    /// <param name="info"></param>
    public void JoinPlayerData(ProtoBuf.CharacterInfo info)
    {
        for (int i = GameManager.GM._AllPlayerData.Length - 1; i > 0; i--)
        {
            if (_AllPlayerData[i].ID == 0)//如果没有人
            {
                _AllPlayerData[i] = new _PlayerInfo(ToName(info.userName), info.charId, info.ip, info.portrait, info.diamond, (int)info.gold, (int)info.sex);
                break;
            }
        }
        PublicEvent.GetINS.Fun_JoinRoomPlayerUpdata();
        Debug.Log("客户端进入之后，有玩家进入PlayerNum:" + BaseProto.SeverPlayerNum);
    }
    /// <summary>
    ///已经指定了位置的
    /// </summary>
    /// <param name="info"></param>
    /// <param name="num"></param>
    public void ReSetAllPlayerData(ProtoBuf.CharacterInfo info, int num)
    {
        Debug.Log("重新进入房间之后，之前玩家的状态" + "Player人数：" + BaseProto.SeverPlayerNum);
        _AllPlayerData[num] = new _PlayerInfo(ToName(info.userName), info.charId, info.ip, info.portrait, info.diamond, (int)info.gold, (int)info.sex);
    }
    public void DeletePlayerData(uint PlayerID)
    {
        for (int i = 0; i < _AllPlayerData.Length; i++)
        {
            if (_AllPlayerData[i].ID == PlayerID)
            {
                _AllPlayerData[i].ID = 0;
                _AllPlayerData[i].IP = i.ToString();
                break;
            }
        }
    }


    // Use this for initialization
    void Awake()
    {
        GM = this;
    }
    private void Start()
    {
        Canvas = this.GetComponent<RectTransform>();
        Resources.Load<GameObject>(ResPath.PlayerInfo);
        GameNetWork.Inst().Init();
        LoginProcessor.Inst().Init();
        SHeight = GetComponent<RectTransform>().sizeDelta.y;
        SWide = GetComponent<RectTransform>().sizeDelta.x;
    }
    /// <summary>
    /// 输入地址然后返回预制件，记得注册这个，并且记得回收
    /// </summary>
    /// <param name="adress">ResPath</param>
    /// <returns></returns>
    public GameObject PopUI(string adress)
    {
        GameObject Temp = null;
        var t = Resources.Load<GameObject>(adress);
        Temp = Instantiate(t);
        Temp.SetActive(true);
        Temp.transform.SetParent(this.transform, false);
        Temp.transform.SetAsLastSibling();
        return Temp;
    }
    string GetTimeStr()
    {
        return System.DateTime.Now.Year + "" + System.DateTime.Now.Month + "" + System.DateTime.Now.Day + "" + System.DateTime.Now.Hour + "" + System.DateTime.Now.Minute + "" + System.DateTime.Now.Second + ".png";
    }
    IEnumerator open()
    {
        //GameObject Temp = null;
        yield return new WaitForSeconds(0.2f);

        //Temp = Instantiate(Resources.Load<GameObject>(ResPath.Main));
        DataStorage.GetIns.Main.SetActive(true);
        //Temp.transform.SetParent(this.transform, false);
        DataStorage.GetIns.Main.transform.SetAsLastSibling();

    }
    float i = 0;
    public string GameType;

    void Update()
    {
        i += Time.deltaTime;
        if (i > 0.1)
        {
            i = 0;
            LoginProcessor.Inst().Update();
            GameNetWork.Inst().Update();
        }
    }
    //void FixedUpdate()
    //{

    //}

    void OnDestroy()
    {
        LoginProcessor.Inst().UnInit();
        GameNetWork.Inst().UnInit();
    }


    public void Share(int num)
    {
            StartCoroutine(WorkToShare(num));
    }

    IEnumerator WorkToShare(int nums)
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        info["mediaType"] = "1"; //分享类型： 0-文字 1-图片 2-网址  
        info["shareTo"] = nums.ToString(); //分享到：0-聊天 1-朋友圈 2-收藏  

        //_LogLevel.Log("查询分享分类"+nums,_LogLevel.WorkingLog);
        if (Application.platform == RuntimePlatform.IPhonePlayer)//判断平台
        {
            info["thumbImage"] = Application.persistentDataPath + "/icon.png";

            //Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            //screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            //screenShot.Apply();
            //string screenShotFile = Application.persistentDataPath + "/Screenshot.png";
            //System.IO.File.WriteAllBytes(screenShotFile, screenShot.EncodeToJPG());
            //info["imagePath"] = Application.persistentDataPath + "/Screenshot.png";
        }
        else
        {
            yield return StartCoroutine(CaptureScreenshot2());
            info["imagePath"] = Application.persistentDataPath + "/Screenshot.png";
        }
        AnySDKManager.SendShare(info);
    }

    IEnumerator CaptureScreenshot2()
    {

        //_LogLevel.Log("安卓机 新建纹理", _LogLevel.WorkingLog);
        var rect = new Rect(Screen.width * 0, Screen.height * 0, Screen.width * 1, Screen.height * 1);
        // 先创建一个的空纹理，大小可根据实现需要来设置  
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        // 读取屏幕像素信息并存储为纹理数据，  
        yield return new WaitForEndOfFrame();
        //_LogLevel.Log("安卓机 等待一帧", _LogLevel.WorkingLog);
        screenShot.ReadPixels(rect, 0, 0);
        //_LogLevel.Log("安卓机 读取屏幕", _LogLevel.WorkingLog);
        screenShot.Apply();
        screenShot.Compress(false);
        //_LogLevel.Log("安卓机 保存图片", _LogLevel.WorkingLog);
        //TextureScale.Bilinear(screenShot, 860, 484);     // /2.1 75 30.1kb  2.7 85 31.49kb
        //_LogLevel.Log("安卓机 压缩图片`1", _LogLevel.WorkingLog);
        //JPGEncoder encoder = new JPGEncoder(screenShot, 50);//质量1~100 //90
        //_LogLevel.Log("安卓机 压缩图片`2", _LogLevel.WorkingLog);
        //encoder.doEncoding();
        //_LogLevel.Log("安卓机 等待写入", _LogLevel.WorkingLog);
        //while (!encoder.isDone)
        //{
        //    yield return null;
        //}
        //_LogLevel.Log("安卓机 准备写入路径", _LogLevel.WorkingLog);
        Byte[] encoder = screenShot.EncodeToJPG(50);
        string screenShotFile = Application.persistentDataPath + "/Screenshot.png";
        //_LogLevel.Log("安卓机 准备写入存储卡", _LogLevel.WorkingLog);
        //System.IO.File.WriteAllBytes(screenShotFile, encoder);
        //_LogLevel.Log("安卓机 写入存储卡", _LogLevel.WorkingLog);
        yield return null;
    }

   /// <summary>
   /// 邀请
   /// </summary>
   /// <param name="GameType">游戏类型</param>
   /// <param name="roundNum">回合</param>
   /// <param name="gameRule">游戏规则</param>
    public void ShareLink(string GameType, string roundNum, string gameRule)
    {
        Onyaoqing(GameType, roundNum, gameRule);
    }

    void Onyaoqing(string GameType, string roundNum, string gamerule)
    {
        Dictionary<string, string> info = new Dictionary<string, string>();
        info["mediaType"] = "2"; //分享类型： 0-文字 1-图片 2-网址  
        info["shareTo"] = "0"; //分享到：0-聊天 1-朋友圈 2-收藏  

        info["title"] = "铜雀楼-" + GameType;
        info["imagePath"] = Application.persistentDataPath + "/icon.png";
        info["url"] = "http://www.baidu.com/";
        info["text"] = "快加入房间号:" + BaseProto.playerInfo.m_atRoomId + "," +roundNum + "局." + GameType + "[" + gamerule + "]";

        if (Application.platform == RuntimePlatform.IPhonePlayer)//判断平台
        {
            info["thumbImage"] = Application.persistentDataPath + "/icon.png";
        }
        else
        {
            info["thumbSize"] = "64";
        }
        AnySDKManager.SendShare(info);
    }
}
