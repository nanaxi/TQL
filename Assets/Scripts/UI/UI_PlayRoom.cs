using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UI_PlayRoom : MonoBehaviour
{
    public Text CenterTime = null;
    Transform ThisTrans = null;
    /// <summary>
    /// 标识器的上下左右
    /// </summary>
    public GameObject up, left, down, right;
    /// <summary>
    /// 碰杠胡
    /// </summary>
    public Button Peng, Gang, Hu, Guo;
    public Button Quit_BTN, Setting_BTN, FaceAndChat_BTN, Voice_BTN;
    /// <summary>
    /// 本地准备按钮
    /// </summary>
    public Button Ready;
    public Button Share;
    /// <summary>
    /// 选缺用的万筒条
    /// </summary>
    public Button Wan, Tong, Tiao;
    /// <summary>
    /// 本地选三张
    /// </summary>
    public Button XSZ;

    public GameObject XZDD, GAMJ;
    public GameObject Ready0, Ready1, Ready2, Ready3;
    public GameObject Que0, Que1, Que2, Que3;
    public GameObject hsz0, hsz1, hsz2, hsz3;
    public GameObject noque0, noque1, noque2, noque3;
    public GameObject nohsz0, nohsz1, nohsz2, nohsz3;
    public Text ju, cardsNum;

    public Text Pop0, Pop1, Pop2, Pop3;
    public Text RoomNum;
    public Text RealtyTime;
    public Text rule;
    void Awake()
    {
        Init();
        Reg();
    }
    // Use this for initialization
    void Start()
    {
        GameManager.GM.ingame = true;
        Default();

    }
    void Init()
    {
        ThisTrans = this.gameObject.transform;
    }
    void Default()
    {
        Peng.onClick.AddListener(PressedPeng);
        Gang.onClick.AddListener(PressedGang);
        Hu.onClick.AddListener(PressedHu);
        Guo.onClick.AddListener(PressedGuo);
        Quit_BTN.onClick.AddListener(PressedQuit);
        Setting_BTN.onClick.AddListener(PressedSetting);
        FaceAndChat_BTN.onClick.AddListener(PressedFaceAndChat);
        Voice_BTN.onClick.AddListener(PressedVoice);

        Ready.onClick.AddListener(PressedReady);
        Share.onClick.AddListener(ShareToFriend);
        XSZ.onClick.AddListener(PressedXSZ);

        Tiao.onClick.AddListener(delegate { PressedQue(2); });
        Tong.onClick.AddListener(delegate { PressedQue(1); });
        Wan.onClick.AddListener(delegate { PressedQue(0); });

        DefaultPlayerInformation();

        SetNavigation(0);
    }
    /// <summary>
    /// 初始化玩家信息
    /// </summary>
    void DefaultPlayerInformation()
    {
        XSZ.gameObject.SetActive(false);

        Peng.gameObject.SetActive(false);
        Gang.gameObject.SetActive(false);
        Hu.gameObject.SetActive(false);
        Guo.gameObject.SetActive(false);

        Wan.gameObject.SetActive(false);

        Tong.gameObject.SetActive(false);

        Tiao.gameObject.SetActive(false);


        Que0.SetActive(false);
        Que1.SetActive(false);
        Que2.SetActive(false);
        Que3.SetActive(false);

        hsz0.SetActive(false);
        hsz1.SetActive(false);
        hsz2.SetActive(false);
        hsz3.SetActive(false);


        noque0.SetActive(false);
        noque1.SetActive(false);
        noque2.SetActive(false);
        noque3.SetActive(false);

        nohsz0.SetActive(false);
        nohsz1.SetActive(false);
        nohsz2.SetActive(false);
        nohsz3.SetActive(false);


        for (int i = 0; i < BaseProto.SeverPlayerNum; i++)
        {
            ThisTrans.Find("Players/Head" + i + "/Zhuang").gameObject.SetActive(false);
        }
    }

    Image HeadSprite = null;
    void SetPlyerInformation(string name, string gold, string head, int Pos = 0)
    {
        GameManager.GM.GetHead(head, SetHead, Pos);
        //ThisTrans.Find("Players/Head" + Pos + "/Mask/HeadSprite").GetComponent<Image>().name= head;//.sprite = HeadSprite.sprite
        ThisTrans.Find("Players/Head" + Pos + "/Name").GetComponent<Text>().text = name;
        ThisTrans.Find("Players/Head" + Pos + "/Gold/Text").GetComponent<Text>().text = gold;
        ThisTrans.Find("Players/Head" + Pos + "/Zhuang").gameObject.SetActive(false);
    }

    void PlayerComing(string name, string gold, string head, int pos)
    {
        SetPlyerInformation(name, gold, head, pos);
    }
    void PlayerReadyed(uint PlayerID)
    {
        switch (GameManager.GM.GetPlayerNum(PlayerID))
        {
            case 0:
                Ready0.SetActive(true);
                break;
            case 1:
                Ready1.SetActive(true);
                break;
            case 2:
                Ready2.SetActive(true);
                break;
            case 3:
                Ready3.SetActive(true);
                break;
            default:
                break;
        }
    }
    List<uint> SamePlayers = new List<uint>();
    void ShowZhuang(uint PlayerID)
    {
        int i = GameManager.GM.GetPlayerNum(PlayerID);
        Debug.Log("第" + i + "个玩家");
        ThisTrans.Find("Players/Head" + i + "/Zhuang").gameObject.SetActive(true);
        Ready0.SetActive(false);
        Ready1.SetActive(false);
        Ready2.SetActive(false);
        Ready3.SetActive(false);

        for (int P = 0; P < 4; P++)
        {
            for (int z = 3; z >= 0; z--)
            {
                if (GameManager.GM._AllPlayerData[z].ID != 0 && GameManager.GM._AllPlayerData[P].IP == GameManager.GM._AllPlayerData[z].IP)
                {
                    if (!SamePlayers.Contains(GameManager.GM._AllPlayerData[z].ID))//不存在则添加进去
                    {
                        Debug.Log("添加！！！！！");
                        SamePlayers.Add(GameManager.GM._AllPlayerData[z].ID);
                    }
                }
            }
        }
        if (SamePlayers.Count > 0)
            if (GameManager.GM.DS.Notic == null)
            {
                GameManager.GM.DS.Notic = GameManager.GM.PopUI(ResPath.Notic);
                if (SamePlayers.Count == 2)
                {
                    Debug.Log("2个人");
                    GameManager.GM.DS.Notic.GetComponent<UI_Notic>().SetMessage("\n温馨提示：有玩家在同一网络下游戏！" + "\n" + GameManager.GM.GetPlayerName(SamePlayers[0]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[0]) + "\n" + GameManager.GM.GetPlayerName(SamePlayers[1]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[1]));
                }
                if (SamePlayers.Count == 3)
                {
                    Debug.Log("3个人");
                    GameManager.GM.DS.Notic.GetComponent<UI_Notic>().SetMessage("\n温馨提示：有玩家在同一网络下游戏！" + "\n" + GameManager.GM.GetPlayerName(SamePlayers[0]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[0]) + "\n" + GameManager.GM.GetPlayerName(SamePlayers[1]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[1]) + "\n" + GameManager.GM.GetPlayerName(SamePlayers[2]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[2]));
                }
                if (SamePlayers.Count == 4)
                {
                    Debug.Log("4个人");
                    GameManager.GM.DS.Notic.GetComponent<UI_Notic>().SetMessage("\n温馨提示：有玩家在同一网络下游戏！" + "\n" + GameManager.GM.GetPlayerName(SamePlayers[0]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[0]) + "\n" + GameManager.GM.GetPlayerName(SamePlayers[1]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[1]) + "\n" + GameManager.GM.GetPlayerName(SamePlayers[2]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[2]) + "\n" + GameManager.GM.GetPlayerName(SamePlayers[3]) + "  " + GameManager.GM.GetPlayerIp(SamePlayers[3]));
                }
            }
        Share.gameObject.SetActive(false);
    }
    void SetHead(Sprite sprite, int num = 0)
    {
        ThisTrans.Find("Players/Head" + num + "/Mask/HeadSprite").GetComponent<Image>().sprite = sprite;
        //HeadSprite.sprite = sprite;
        //HeadSprite.color = new Color(255, 255, 255, 255);
    }

    #region 指针方向和倒计时
    /// <summary>
    /// 设定指针方向
    /// </summary>
    /// <param name="num">0：自己 1：左边 2：上边 3：右边</param>
    void SetNavigation(int num)
    {
        ReFreshRealtyTime();
        CancelInvoke();
        switch (num)
        {
            case 0:
                up.SetActive(false);
                left.SetActive(false);
                down.SetActive(true);
                right.SetActive(false);
                break;
            case 1:
                up.SetActive(false);
                left.SetActive(true);
                down.SetActive(false);
                right.SetActive(false);
                break;
            case 2:
                up.SetActive(true);
                left.SetActive(false);
                down.SetActive(false);
                right.SetActive(false);
                break;
            case 3:
                up.SetActive(false);
                left.SetActive(false);
                down.SetActive(false);
                right.SetActive(true);
                break;
            default:
                break;
        }
        ReFreshTime();
    }
    uint TheTime = 0;
    void ReFreshTime()
    {
        TheTime = 10;
        InvokeRepeating("ReduceTime", 0, 1.0f);
    }
    void ReduceTime()
    {
        if (TheTime > 0)
            CenterTime.text = (--TheTime).ToString();

    }
    #endregion
    DateTime _time;
    void ReFreshRealtyTime()
    {
        _time = DateTime.Now;
        RealtyTime.text = _time.Hour.ToString() + ":" + _time.Minute.ToString();
    }
    /// <summary>
    /// 注册事件
    /// </summary>
    void Reg()
    {
        //SetNavigation
        PublicEvent.GetINS.Event_ExitRoomSucc += Quit;
        PublicEvent.GetINS.Event_JoinRoomPlayerUpdata += PlayerUpdata;
        PublicEvent.GetINS.Event_KeyiXQ += CanXQ;
        PublicEvent.GetINS.Event_reciveSelectQue += ReciveXQ;
        //PublicEvent.GetINS.Event_KeyiH3z += CanXSZ;
        PublicEvent.GetINS.Event_ReciveChange3ZhangResult += ReciveHSZ;
        PublicEvent.GetINS.EnableH3zButton += ShowHSZ_BTN;
        PublicEvent.GetINS.DisableH3zButton += DisHSZ_BTN;
        PublicEvent.GetINS.Event_recivePlayerReady += PlayerReadyed;
        PublicEvent.GetINS.Event_reciveZhuang += ShowZhuang;
        PublicEvent.GetINS.Fun_UpdatePaishu += SetCardNum;

        PublicEvent.GetINS.Event_KeYiPeng += recivePeng;
        PublicEvent.GetINS.Event_KeYiGang += reciveGang;
        PublicEvent.GetINS.Event_KeYiHu += reciveHu;
        PublicEvent.GetINS.Event_reciveGuo += reciveGuo;
        PublicEvent.GetINS.Event_DirLight += SetNavigation;
        PublicEvent.GetINS.Event_ReadyToPlayNew += SetPlayerDefault;
        PublicEvent.GetINS.Event_reciveGetFirstCards += ReciveCard;

        PublicEvent.GetINS.Fun_reciveMessagePreDefine += ShowAnim;
        PublicEvent.GetINS.Event_reciveMessageText += ShowPop;
        PublicEvent.GetINS.PlaySound += ShowVoice;
        PublicEvent.GetINS.Evnet_ReciveChange3ZhangOthercharid += ShowHsz;
        PublicEvent.GetINS.Event_DisHead += Showhead;
    }

    public void NextJu()
    {
        --junum;
        ju.text = "剩余" + junum.ToString() + "局";
    }
    public void SetCardNum(int value)
    {
        cardsNum.text = "剩余" + value.ToString() + "张";
    }

    #region 游戏内按钮点击事件
    /// <summary>
    /// 按下碰
    /// </summary>
    void PressedPeng()
    {
        Peng.gameObject.SetActive(false);
        Guo.gameObject.SetActive(false);
        Gang.gameObject.SetActive(false);
        Hu.gameObject.SetActive(false);
        PublicEvent.GetINS.Fun_SentPeng(peng);
        Debug.Log("碰");
    }
    /// <summary>
    /// 按下杠
    /// </summary>
    void PressedGang()
    {
        Peng.gameObject.SetActive(false);
        Guo.gameObject.SetActive(false);
        Gang.gameObject.SetActive(false);
        Hu.gameObject.SetActive(false);
        PublicEvent.GetINS.Fun_SentGang(gang, GangOricharid);
        Debug.Log("杠");
    }
    /// <summary>
    /// 按下胡
    /// </summary>
    void PressedHu()
    {
        Peng.gameObject.SetActive(false);
        Guo.gameObject.SetActive(false);
        Gang.gameObject.SetActive(false);
        Hu.gameObject.SetActive(false);
        PublicEvent.GetINS.Fun_SentHu(hu, oricharid);
        Debug.Log("胡");
    }
    /// <summary>
    /// 按下过
    /// </summary>
    void PressedGuo()
    {
        Peng.gameObject.SetActive(false);
        Guo.gameObject.SetActive(false);
        Gang.gameObject.SetActive(false);
        Hu.gameObject.SetActive(false);
        PublicEvent.GetINS.Fun_SentGuo(55);
        Debug.Log("过");
    }
    void PressedReady()
    {
        Debug.Log("本地准备");
        PublicEvent.GetINS.Fun_SentClientPre();
        Ready.gameObject.SetActive(false);
        //ShowFace.Ins.PlayAnim(Face.guafeng, 1, 5);
        //ShowFace.Ins.PlayAnim(Face.xiayu, 3, 8);
    }
    string gr="";
    void ShareToFriend()
    {       
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            Debug.Log("分享成功！");
        }
        else
        {
            if (GameManager.GM.GameType == "ga")
                GameManager.GM.ShareLink("广安麻将", RoomNum.text, gr);
            else
                GameManager.GM.ShareLink("血战到底", RoomNum.text, gr);
        }
    }
    void CanXSZ()
    {
        Debug.Log("收到可以换三张的消息");
    }
    void PressedXSZ()
    {
        XSZ.gameObject.SetActive(false);
        Debug.Log("本地换三张发送");
        PublicEvent.GetINS.OnClickH3z();
    }
    void CanXQ()
    {
        Debug.Log("收到可以选缺的消息");
        Wan.gameObject.SetActive(true);
        Tong.gameObject.SetActive(true);
        Tiao.gameObject.SetActive(true);
    }
    void PressedQue(int WTT)
    {
        switch (WTT)
        {
            case 0:
                {
                    Debug.Log("万");
                    PublicEvent.GetINS.Fun_SentSelectQue(1);
                }
                break;
            case 1:
                {
                    Debug.Log("筒");
                    PublicEvent.GetINS.Fun_SentSelectQue(2);
                }
                break;
            case 2:
                {
                    Debug.Log("条");
                    PublicEvent.GetINS.Fun_SentSelectQue(3);
                }
                break;
            default:
                break;
        }
    }
    #endregion
    #region 服务器接收到的碰杠胡果的消息
    uint peng, gang, hu;
    uint oricharid, GangOricharid;
    void recivePeng(uint value, uint Oricharid)
    {
        reciveGuo();
        peng = value;
        oricharid = Oricharid;
        Peng.gameObject.SetActive(true);
    }
    void reciveGang(List<uint> value, uint Oricharid)
    {
        reciveGuo();
        gang = value[0];
        GangOricharid = Oricharid;
        Gang.gameObject.SetActive(true);
    }
    void reciveHu(uint value, uint Oricharid)
    {
        reciveGuo();
        hu = value;
        oricharid = Oricharid;
        Hu.gameObject.SetActive(true);
    }
    void reciveGuo()
    {
        Guo.gameObject.SetActive(true);
    }
    #endregion
    #region 游戏外按钮点击事件
    void PressedQuit()
    {
        Debug.Log("退出到上一级");
        GameManager.GM.DS.IsVote = GameManager.GM.PopUI(ResPath.IsQuit);
    }
    void Quit()
    {
        ShowFace.Ins.DisAllAnim();
        BaseProto.playerInfo.m_atRoomId = 0;
        BaseProto.playerInfo.m_cdRoomId = 0;
        ParticleManager.GetIns.SwitchSence(1);
        Invoke("Rest", 0.2f);
    }
    void PressedSetting()
    {
        var temp = Resources.Load("PopUI/Setting") as GameObject;
        temp = Instantiate(temp);
        temp.transform.SetParent(transform.parent, false);

        temp.transform.SetAsLastSibling();
    }
    void PressedFaceAndChat()
    {
        var temp = Resources.Load("PopUI/UsedChat") as GameObject;
        temp = Instantiate(temp);
        temp.transform.SetParent(transform.parent, false);
        temp.transform.SetAsLastSibling();
    }
    void PressedVoice()
    {
        //Debug.Log("pressed");
    }
    #endregion
    /// <summary>
    /// 接受服务器发给我的信息,不包含游戏内玩家的顺序之类的信息，那些数据是通过PlayerUpdata来进行的
    /// </summary>
    /// <param name="rsp"></param>
    ProtoBuf.EnterRoomRsp RoomRsp = null;
    public uint junum = 0;
    public bool DingQue = false, HSZ = false;
    public List<string> GameRule = new List<string>();
    public void ReciveRoomData(ProtoBuf.EnterRoomRsp rsp)
    {
        RoomRsp = rsp;
        RoomNum.text = "房间号:" + rsp.mjRoom.roomId.ToString();
        if (rsp.mjRoom.roomRuleInfo.gaRule != null)
        {
            GameManager.GM.GameType = "ga";
            junum = rsp.mjRoom.roomRuleInfo.gaRule.roundNum - 1;
            Debug.Log("广安规则" + rsp.mjRoom.roomRuleInfo.gaRule.roundNum + "局");
            ju.text = "剩余:" + junum.ToString() + "局";
            GAMJ.SetActive(true);
            XZDD.SetActive(false);
            Debug.Log("RoomRsp.mjRoom.roomRuleInfo.gaRule.ga.Count" + RoomRsp.mjRoom.roomRuleInfo.gaRule.ga.Count);

            for (int i = 0; i < RoomRsp.mjRoom.roomRuleInfo.gaRule.ga.Count; i++)
            {
                switch (RoomRsp.mjRoom.roomRuleInfo.gaRule.ga[i])
                {
                    case ProtoBuf.GARule.GA.OGM:
                        //GameRule.Add("");
                        break;
                    case ProtoBuf.GARule.GA.NGM:
                        //GameRule.Add("");
                        break;
                    case ProtoBuf.GARule.GA.DSY:
                        GameRule.Add("大三元 ");
                        break;
                    case ProtoBuf.GARule.GA.XSY:
                        GameRule.Add("小三元 ");
                        break;
                    case ProtoBuf.GARule.GA.BZ:
                        GameRule.Add("板子 ");
                        break;
                    case ProtoBuf.GARule.GA.FJ:
                        GameRule.Add("飞机 ");
                        break;
                    case ProtoBuf.GARule.GA.ZP:
                        GameRule.Add("字牌算飞机 ");
                        break;
                    case ProtoBuf.GARule.GA.DQ:
                        GameRule.Add("定缺 ");
                        DingQue = true;
                        break;
                    case ProtoBuf.GARule.GA.BF:
                        GameRule.Add("比番 ");
                        break;
                    case ProtoBuf.GARule.GA.H3Z:
                        GameRule.Add("换三张 ");
                        HSZ = true;
                        break;
                    case ProtoBuf.GARule.GA.ZMJF:
                        GameRule.Add("自摸加番 ");
                        break;
                    case ProtoBuf.GARule.GA.KXW:
                        GameRule.Add("卡心五 ");
                        break;
                    case ProtoBuf.GARule.GA.PH:
                        GameRule.Add("屁胡不能胡 ");
                        break;
                    case ProtoBuf.GARule.GA.SBLH:
                        GameRule.Add("十八罗汉自选 ");
                        break;
                    case ProtoBuf.GARule.GA.DMQ:
                        GameRule.Add("大门清 ");
                        break;
                    case ProtoBuf.GARule.GA.YTL:
                        GameRule.Add("一条龙 ");
                        break;
                    default:
                        break;
                }
            }
        }
        if (rsp.mjRoom.roomRuleInfo.xzddRule != null)
        {
            GameManager.GM.GameType = "xz";
            Debug.Log("血战到底规则:" + rsp.mjRoom.roomRuleInfo.xzddRule.roundNum + "局");
            junum = rsp.mjRoom.roomRuleInfo.xzddRule.roundNum - 1;
            ju.text = "剩余:" + junum.ToString() + "局";
            GAMJ.SetActive(false);
            XZDD.SetActive(true);
            Debug.Log("RoomRsp.mjRoom.roomRuleInfo.xzddRule.flags.Count" + RoomRsp.mjRoom.roomRuleInfo.xzddRule.flags.Count);
            for (int i = 0; i < RoomRsp.mjRoom.roomRuleInfo.xzddRule.flags.Count; i++)
            {
                switch (RoomRsp.mjRoom.roomRuleInfo.xzddRule.flags[i])
                {
                    case ProtoBuf.XZDDRule.XZDD.ZMJD:
                        GameRule.Add("自摸加底 ");
                        break;
                    case ProtoBuf.XZDDRule.XZDD.ZMJF:
                        GameRule.Add("自摸加番 ");
                        break;
                    case ProtoBuf.XZDDRule.XZDD.DYJJD:
                        GameRule.Add("幺九将对 ");
                        break;
                    case ProtoBuf.XZDDRule.XZDD.H3Z:
                        GameRule.Add("换三张 ");
                        HSZ = true;
                        break;
                    case ProtoBuf.XZDDRule.XZDD.DGH_ZM:
                        GameRule.Add("点杠花 ");
                        break;
                    case ProtoBuf.XZDDRule.XZDD.DGH_FP:
                        GameRule.Add("点杠炮 ");
                        break;
                    case ProtoBuf.XZDDRule.XZDD.TDH:
                        GameRule.Add("天地胡 ");
                        break;
                    case ProtoBuf.XZDDRule.XZDD.MQZZ:
                        GameRule.Add("门清中张 ");
                        break;
                    case ProtoBuf.XZDDRule.XZDD.XQUE:
                        GameRule.Add("定缺 ");
                        DingQue = true;
                        break;
                    default:
                        break;
                }
            }
        }
        if (GameRule.Count > 1)
            for (int i = 1; i < GameRule.Count; i++)
            {
                //Debug.Log("GameRule[i]:"+ GameRule[i]);
                GameRule[0] += GameRule[i];
                //GameRule[0].Insert(GameRule[0].Length, "00");
                //Debug.Log(GameRule[0]);
            }
        if (GameRule.Count > 0)
            gr = GameRule[0];

        Rule.onClick.AddListener(delegate { GameManager.GM.PopUI(ResPath.Notic).GetComponent<UI_Notic>().SetMessage(gr); });
        //rule.text = gr;
    }
    public Button Rule=null;
    /// <summary>
    /// 排序之后刷新首次进入当前房间内的玩家数据
    /// </summary>
    void PlayerUpdata(ProtoBuf.MJRoomInfo value)
    {
        Debug.Log("玩家数据更新！");
        for (int i = 0; i < 4; i++)
        {
            var t = ThisTrans.Find("Players/Head" + i).gameObject;
            if (GameManager.GM._AllPlayerData[i].ID != 0)
            {
                int pos = i;
                t.GetComponent<Button>().onClick.AddListener(delegate
                {
                    //Debug.Log("name:" + GameManager.GM._AllPlayerData[pos].Name + "          id:" + GameManager.GM._AllPlayerData[pos].ID);
                    //pos = i;//如果出现点开玩家头像结果信息出现问题可能就是这里出现了问题
                    if (GameManager.GM.DS.PlayerInfo != null)
                    {
                        Destroy(GameManager.GM.DS.PlayerInfo);
                        GameManager.GM.DS.PlayerInfo = null;
                    }
                    if (GameManager.GM.DS.PlayerInfo == null)
                    {
                        var temp = GameManager.GM.PopUI(ResPath.PlayerInfo);
                        GameManager.GM.DS.PlayerInfo = temp;
                        UI_PlayerInfo player = temp.GetComponent<UI_PlayerInfo>();

                        player.SetInfo(GameManager.GM._AllPlayerData[pos].Name, GameManager.GM._AllPlayerData[pos].ID.ToString(), GameManager.GM._AllPlayerData[pos].IP.ToString(), GameManager.GM._AllPlayerData[pos].Diamond.ToString(), GameManager.GM._AllPlayerData[pos].Head);
                    }
                });
                //Debug.Log("pos" + pos);
                t.SetActive(true);
            }
            else
            {
                t.SetActive(false);
            }

        }
        if (value != null)
        {
            Debug.Log("玩家数据不为空！");
            for (int p = 0; p < value.charIds.Count; p++)
            {
                if (value.charStates[p].isZB > 0)
                {
                    Debug.Log("玩家" + p + "准备了！");
                    PlayerReadyed(value.charIds[p]);
                }
                ///这部分功能服务器没有提供给我
                //if (value.charStates[p].isQZ > 0)
                //{
                //    Debug.Log("玩家" + p + "选缺了！");
                //    PlayerReadyed(value.charIds[p]);
                //}
                //if (value.charStates[p] > 0)
                //{
                //    Debug.Log("玩家" + p + "选三张了！");
                //    PlayerReadyed(value.charIds[p]);
                //}
            }
        }
        StartCoroutine("loadPlayer");
    }
    ProtoBuf.MJGameOP[] xxx = (ProtoBuf.MJGameOP[])Enum.GetValues(typeof(ProtoBuf.MJGameOP));
    IEnumerator loadPlayer()
    {
        for (int i = 0; i < 4; i++)
        {
            if (GameManager.GM._AllPlayerData[i].ID != 0)
            {
                yield return null;
                PlayerComing(GameManager.GM._AllPlayerData[i].Name, GameManager.GM._AllPlayerData[i].Money.ToString(), GameManager.GM._AllPlayerData[i].Head, i);
            }
        }
        if (RoomRsp.mjRoom.zjId != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerReadyed(GameManager.GM._AllPlayerData[i].ID);
            }

            ShowZhuang(RoomRsp.mjRoom.zjId);
            Ready.gameObject.SetActive(false);
            Share.gameObject.SetActive(false);

            if (HSZ)
                for (int z = 0; z < 4; z++)
                {
                    switch (RoomRsp.mjRoom.cardsInfos[z].xQue)
                    {
                        case ProtoBuf.MJCardType.MJ_CARD_TYPE_WAN:
                            ReciveXQ(RoomRsp.mjRoom.charStates[z].charId, 1);
                            break;
                        case ProtoBuf.MJCardType.MJ_CARD_TYPE_TONG:
                            ReciveXQ(RoomRsp.mjRoom.charStates[z].charId, 2);
                            break;
                        case ProtoBuf.MJCardType.MJ_CARD_TYPE_TIAO:
                            ReciveXQ(RoomRsp.mjRoom.charStates[z].charId, 3);
                            break;
                        default:
                            break;
                    }
                    if (RoomRsp.mjRoom.cardsInfos[z].x3zIn.Count > 0)
                    {//已经有人选择了
                        ShowHsz(RoomRsp.mjRoom.charIds[z]);
                    }
                    else
                    {
                        switch (GameManager.GM.GetPlayerNum(RoomRsp.mjRoom.charIds[z]))
                        {
                            case 0:
                                nohsz0.SetActive(true);
                                break;
                            case 1:
                                nohsz1.SetActive(true);
                                break;
                            case 2:
                                nohsz2.SetActive(true);
                                break;
                            case 3:
                                nohsz3.SetActive(true);
                                break;
                            default:
                                break;
                        }
                    }
                    if (RoomRsp.mjRoom.cardsInfos[z].x3zIn.Count == 0)
                    {//选三张牌阶段
                        //Debug.Log("现在是选三张阶段");
                        switch (GameManager.GM.GetPlayerNum(RoomRsp.mjRoom.charIds[z]))
                        {
                            case 0:
                                nohsz0.SetActive(true);
                                break;
                            case 1:
                                nohsz1.SetActive(true);
                                break;
                            case 2:
                                nohsz2.SetActive(true);
                                break;
                            case 3:
                                nohsz3.SetActive(true);
                                break;
                            default:
                                break;
                        }
                        //PublicEvent.GetINS.Fun_KeyiH3z();
                    }
                    if (RoomRsp.mjRoom.cardsInfos[z].x3zIn.Count == 0 && RoomRsp.mjRoom.charInfos[z].charId == BaseProto.playerInfo.m_id)
                    {//选三张牌阶段
                        //Debug.Log("现在是选三张阶段");
                        PublicEvent.GetINS.Fun_KeyiH3z();
                    }
                    else if (RoomRsp.mjRoom.cardsInfos[z].x3zOut.Count > 0 && RoomRsp.mjRoom.charInfos[z].charId == BaseProto.playerInfo.m_id)
                    {
                        //现在是已经换完三张的阶段
                        if (DingQue && RoomRsp.mjRoom.cardsInfos[z].xQue == ProtoBuf.MJCardType.MJ_CARD_TYPE_HUA)
                        {
                            PublicEvent.GetINS.Fun_KeyiXQ();
                            noque0.SetActive(true);
                            noque1.SetActive(true);
                            noque2.SetActive(true);
                            noque3.SetActive(true);
                        }
                    }
                }
            else if (DingQue)
                for (int p = 0; p < 4; p++)
                {
                    ///选缺状态
                    if (RoomRsp.mjRoom.cardsInfos[p].xQue > 0)
                    {
                        Debug.Log("玩家" + p + "选缺了！");
                        switch (RoomRsp.mjRoom.cardsInfos[p].xQue)
                        {
                            case ProtoBuf.MJCardType.MJ_CARD_TYPE_WAN:
                                ReciveXQ(RoomRsp.mjRoom.charStates[p].charId, 1);
                                break;
                            case ProtoBuf.MJCardType.MJ_CARD_TYPE_TONG:
                                ReciveXQ(RoomRsp.mjRoom.charStates[p].charId, 2);
                                break;
                            case ProtoBuf.MJCardType.MJ_CARD_TYPE_TIAO:
                                ReciveXQ(RoomRsp.mjRoom.charStates[p].charId, 3);
                                break;
                            case ProtoBuf.MJCardType.MJ_CARD_TYPE_HUA:
                                if (RoomRsp.mjRoom.charInfos[p].charId == BaseProto.playerInfo.m_id)
                                {
                                    PublicEvent.GetINS.Fun_KeyiXQ();
                                    noque0.SetActive(true);
                                    noque1.SetActive(true);
                                    noque2.SetActive(true);
                                    noque3.SetActive(true);
                                }
                                break;
                            default:
                                break;
                        }

                    }

                }


            PublicEvent.GetINS.Fun_ReUpdateMj(RoomRsp.mjRoom);
            for (int x = 0; x < RoomRsp.mjRoom.roomCache.charList.Count; x++)
            {

                if (GameManager.GM.GetPlayerNum(RoomRsp.mjRoom.roomCache.charList[x].charId) == 0)
                {
                    for (int i = 0; i < RoomRsp.mjRoom.roomCache.charList[x].opList.Count; i++)
                    {
                        switch (xxx[(int)RoomRsp.mjRoom.roomCache.charList[x].opList[i] - 1])
                        {
                            case ProtoBuf.MJGameOP.MJ_OP_MOPAI:
                                //mo
                                break;
                            case ProtoBuf.MJGameOP.MJ_OP_CHUPAI:
                                //chu
                                PublicEvent.GetINS.Fun_DirLight(GameManager.GM.GetPlayerNum(RoomRsp.mjRoom.roomCache.charList[x].charId));
                                SetCardNum((int)RoomRsp.mjRoom.cardsInfos[x].roundRestCardNum);
                                break;
                            case ProtoBuf.MJGameOP.MJ_OP_GUO:
                                //guo
                                break;
                            case ProtoBuf.MJGameOP.MJ_OP_PENG:
                                Debug.Log("断线重连可以碰");
                                recivePeng(RoomRsp.mjRoom.roomCache.charList[x].cardList[0], RoomRsp.mjRoom.roomCache.charList[x].oriCharId);
                                PublicEvent.GetINS.Fun_DirLight(GameManager.GM.GetPlayerNum(RoomRsp.mjRoom.roomCache.charList[x].oriCharId));
                                //碰
                                break;
                            case ProtoBuf.MJGameOP.MJ_OP_GANG:
                                //gang
                                reciveGang(RoomRsp.mjRoom.roomCache.charList[x].cardList, RoomRsp.mjRoom.roomCache.charList[x].oriCharId);
                                PublicEvent.GetINS.Fun_DirLight(GameManager.GM.GetPlayerNum(RoomRsp.mjRoom.roomCache.charList[x].oriCharId));
                                Debug.Log("断线重连可以杠");

                                break;
                            case ProtoBuf.MJGameOP.MJ_OP_HU:
                                Debug.Log("断线重连可以hu");
                                reciveHu(RoomRsp.mjRoom.roomCache.charList[x].cardList[0], RoomRsp.mjRoom.roomCache.charList[x].oriCharId);
                                PublicEvent.GetINS.Fun_DirLight(GameManager.GM.GetPlayerNum(RoomRsp.mjRoom.roomCache.charList[x].oriCharId));
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }

    void SetPlayerDefault()
    {
        Ready.gameObject.SetActive(true);
        DefaultPlayerInformation();
        for (int i = 0; i < 4; i++)
        {
            transform.Find("Players/Head" + i + "/WTT").gameObject.SetActive(false);
        }

    }
    public void UpdateGold()
    {
        for (int i = 0; i < 4; i++)
        {
            int Pos = GameManager.GM.GetPlayerNum(GameManager.GM._AllPlayerData[i].ID);
            ThisTrans.Find("Players/Head" + Pos + "/Gold/Text").GetComponent<Text>().text = GameManager.GM._AllPlayerData[i].Money.ToString();
        }
    }
    void ReciveCard(List<uint> Cards, int seat)
    {
        if (HSZ)
        {
            Debug.Log("开始换三张");
            nohsz0.SetActive(true);
            nohsz1.SetActive(true);
            nohsz2.SetActive(true);
            nohsz3.SetActive(true);
            PublicEvent.GetINS.Fun_KeyiH3z();
        }
        else
        {
            if (DingQue)
            {
                Debug.Log("开始定缺");
                noque0.SetActive(true);
                noque1.SetActive(true);
                noque2.SetActive(true);
                noque3.SetActive(true);
                PublicEvent.GetINS.Fun_KeyiXQ();
            }
        }
    }
    void ShowHSZ_BTN()
    {
        XSZ.gameObject.SetActive(true);
    }
    void DisHSZ_BTN()
    {
        XSZ.gameObject.SetActive(false);
    }
    void ReciveHSZ(List<uint> Change3ZhangCard)
    {
        if (Change3ZhangCard.Count > 0)
        {
            PublicEvent.GetINS.Fun_KeyiXQ();
            noque0.SetActive(true);
            noque1.SetActive(true);
            noque2.SetActive(true);
            noque3.SetActive(true);
        }

    }
    void ReciveXQ(uint PlayerID, uint CardType)
    {

        // 注: 1是万，3是条，2是筒
        //for (int i = 0; i < 4; i++)
        //{
        int Pos = 0;
        Pos = GameManager.GM.GetPlayerNum(PlayerID);
        Debug.Log("选缺的位置：" + Pos);
        var t = transform.Find("Players/Head" + Pos + "/WTT").gameObject;
        t.SetActive(true);
        t.GetComponent<Image>().sprite = GetWTT(CardType);

        if (PlayerID == BaseProto.playerInfo.m_id)
        {
            Wan.gameObject.SetActive(false);
            Tong.gameObject.SetActive(false);
            Tiao.gameObject.SetActive(false);
        }


        switch (Pos)
        {
            case 0:
                Que0.SetActive(true);
                noque0.SetActive(false);
                break;
            case 1:
                Que1.SetActive(true);
                noque1.SetActive(false);
                break;
            case 2:
                Que2.SetActive(true);
                noque2.SetActive(false);
                break;
            case 3:
                Que3.SetActive(true);
                noque3.SetActive(false);
                break;
            default:
                break;
        }
        if (Que0.activeSelf)
            if (Que1.activeSelf)
                if (Que2.activeSelf)
                    if (Que3.activeSelf)
                    {
                        Que0.SetActive(false);
                        Que1.SetActive(false);
                        Que2.SetActive(false);
                        Que3.SetActive(false);
                    }
        //}
    }
    Sprite GetWTT(uint CardType)
    {
        if (CardType == 1)
            return Resources.Load<GameObject>("Prefabs/ChooseQue/Q_WAN").GetComponent<Image>().sprite;
        if (CardType == 3)
            return Resources.Load<GameObject>("Prefabs/ChooseQue/Q_TIAO").GetComponent<Image>().sprite;
        if (CardType == 2)
            return Resources.Load<GameObject>("Prefabs/ChooseQue/Q_TONG").GetComponent<Image>().sprite;

        return null;
    }

    void Rest()
    {
        GameManager.GM.ingame = false;
        PublicEvent.GetINS.Event_ExitRoomSucc -= Quit;
        PublicEvent.GetINS.Event_JoinRoomPlayerUpdata -= PlayerUpdata;
        PublicEvent.GetINS.Event_KeyiXQ -= CanXQ;
        PublicEvent.GetINS.Event_reciveSelectQue -= ReciveXQ;
        //PublicEvent.GetINS.Event_KeyiH3z -= CanXSZ;
        PublicEvent.GetINS.Event_ReciveChange3ZhangResult -= ReciveHSZ;
        PublicEvent.GetINS.EnableH3zButton -= ShowHSZ_BTN;
        PublicEvent.GetINS.DisableH3zButton -= DisHSZ_BTN;
        PublicEvent.GetINS.Event_recivePlayerReady -= PlayerReadyed;
        PublicEvent.GetINS.Event_reciveZhuang -= ShowZhuang;
        PublicEvent.GetINS.Fun_UpdatePaishu -= SetCardNum;
        PublicEvent.GetINS.Event_KeYiPeng -= recivePeng;
        PublicEvent.GetINS.Event_KeYiGang -= reciveGang;
        PublicEvent.GetINS.Event_KeYiHu -= reciveHu;
        PublicEvent.GetINS.Event_reciveGuo -= reciveGuo;
        PublicEvent.GetINS.Event_DirLight -= SetNavigation;
        PublicEvent.GetINS.Event_ReadyToPlayNew -= SetPlayerDefault;
        PublicEvent.GetINS.Event_reciveGetFirstCards -= ReciveCard;

        PublicEvent.GetINS.Fun_reciveMessagePreDefine -= ShowAnim;
        PublicEvent.GetINS.Event_reciveMessageText -= ShowPop;
        PublicEvent.GetINS.PlaySound -= ShowVoice;
        PublicEvent.GetINS.Evnet_ReciveChange3ZhangOthercharid -= ShowHsz;
        PublicEvent.GetINS.Event_DisHead -= Showhead;

        GameManager.GM.DS.PlayRoom = null;
        for (int i = 1; i < 4; i++)
        {
            GameManager.GM.DeletePlayerData(GameManager.GM._AllPlayerData[i].ID);
            Debug.Log("删除玩家：" + GameManager.GM._AllPlayerData[i].Name);
        }
        Destroy(this.gameObject);

    }
    void ShowHsz(uint charid)
    {
        ThisTrans.Find("Players/Head" + GameManager.GM.GetPlayerNum(charid) + "/IsChoose3card").gameObject.SetActive(true);

        switch (GameManager.GM.GetPlayerNum(charid))
        {
            case 0:
                nohsz0.SetActive(false);
                break;
            case 1:
                nohsz1.SetActive(false);
                break;
            case 2:
                nohsz2.SetActive(false);
                Debug.Log("ASRDFTGDHJKLKLJHHGFDSFGHJ");
                break;
            case 3:
                nohsz3.SetActive(false);
                break;
            default:
                break;
        }


        Debug.Log("出现了！");
        if (hsz0.activeSelf)
        {
            if (hsz1.activeSelf)
            {
                if (hsz2.activeSelf)
                {
                    if (hsz3.activeSelf)
                    {
                        hsz0.SetActive(false);
                        hsz1.SetActive(false);
                        hsz2.SetActive(false);
                        hsz3.SetActive(false);
                    }
                }
            }
        }
    }
    void ShowVoice(uint SendId, string url)
    {
        string t = GameManager.GM.GetPlayerNum(SendId).ToString();
        //GameManager.GM.PopUI(ResPath.Notic).GetComponent<UI_Notic>().SetMessage(url);
        ThisTrans.Find("Players/Head" + t + "/guangquan").gameObject.SetActive(true);
        StartCoroutine(DisShow(t));
    }
    IEnumerator DisShow(string SendId)
    {
        yield return new WaitForSeconds(3.0f);
        ThisTrans.Find("Players/Head" + SendId + "/guangquan").gameObject.SetActive(false);
    }
    void Showhead(uint player, bool dispear)
    {
        if (dispear)
        {
            GameManager.GM.PopUI(ResPath.Notic).GetComponent<UI_Notic>().SetMessage(GameManager.GM.GetPlayerName(player) + "玩家掉线了！");
            ThisTrans.Find("Players/Head" + GameManager.GM.GetPlayerNum(player) + "/Mask/HeadSprite").GetComponent<Image>().color = Color.grey;
        }
        else
        {
            GameManager.GM.PopUI(ResPath.Notic).GetComponent<UI_Notic>().SetMessage(GameManager.GM.GetPlayerName(player) + "玩家回到了游戏！");
            ThisTrans.Find("Players/Head" + GameManager.GM.GetPlayerNum(player) + "/Mask/HeadSprite").GetComponent<Image>().color = Color.white;
        }
    }
    void ShowAnim(uint Sender, string Value)
    {
        switch (Value)
        {
            case "x0xxd0":
                ShowFace.Ins.PlayAnim(Face.cry, GameManager.GM.GetPlayerNum(Sender));
                break;
            case "x0xxd1":
                ShowFace.Ins.PlayAnim(Face.dance, GameManager.GM.GetPlayerNum(Sender), 2);
                break;
            case "x0xxd2":
                ShowFace.Ins.PlayAnim(Face.jing, GameManager.GM.GetPlayerNum(Sender));
                break;
            case "x0xxd3":
                ShowFace.Ins.PlayAnim(Face.tanqi, GameManager.GM.GetPlayerNum(Sender));
                break;
            case "x0xxd4":
                ShowFace.Ins.PlayAnim(Face.thanks, GameManager.GM.GetPlayerNum(Sender));
                break;
            case "x0xxd5":
                ShowFace.Ins.PlayAnim(Face.why, GameManager.GM.GetPlayerNum(Sender));
                break;
            case "x0xxd6":
                ShowFace.Ins.PlayAnim(Face.yeah, GameManager.GM.GetPlayerNum(Sender), 2);
                break;
            case "x0xxd7":
                ShowFace.Ins.PlayAnim(Face.hate, GameManager.GM.GetPlayerNum(Sender), 3);
                break;
            default:
                break;
        }

    }
    int showpopTime = 3;
    bool Pop0_bool = false, Pop1_bool = false, Pop2_bool = false, Pop3_bool = false;
    void ShowPop(uint Sender, string Value)
    {

        int i = GameManager.GM.GetPlayerNum(Sender);
        SoundMag.GetINS.ChatSound(Value, GameManager.GM.GetPlayerSex(Sender), i);
        switch (i)
        {
            case 0:
                Pop0.text = Value;
                Pop0_bool = true;
                Pop0.transform.parent.parent.gameObject.SetActive(true);
                break;
            case 1:
                Pop1.text = Value;
                Pop1_bool = true;
                Pop1.transform.parent.parent.gameObject.SetActive(true);
                break;
            case 2:
                Pop2.text = Value;
                Pop2_bool = true;
                Pop2.transform.parent.parent.gameObject.SetActive(true);
                break;
            case 3:
                Pop3.text = Value;
                Pop3_bool = true;
                Pop3.transform.parent.parent.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    float t1 = 0, t2 = 0, t3 = 0, t0 = 0;
    void Update()
    {
        if (Pop0_bool)
        {
            if (t0 > showpopTime)
            {
                Pop0_bool = false;
                t0 = 0;
                Pop0.transform.parent.parent.gameObject.SetActive(false);
            }
            else
            {
                t0 += Time.deltaTime;
            }
        }
        if (Pop1_bool)
        {
            if (t1 > showpopTime)
            {
                Pop1_bool = false;
                t1 = 0;
                Pop1.transform.parent.parent.gameObject.SetActive(false);
            }
            else
            {
                t1 += Time.deltaTime;
            }
        }
        if (Pop2_bool)
        {
            if (t2 > showpopTime)
            {
                Pop2_bool = false;
                t2 = 0;
                Pop2.transform.parent.parent.gameObject.SetActive(false);
            }
            else
            {
                t2 += Time.deltaTime;
            }
        }
        if (Pop3_bool)
        {
            if (t3 > showpopTime)
            {
                Pop3_bool = false;
                t3 = 0;
                Pop3.transform.parent.parent.gameObject.SetActive(false);
            }
            else
            {
                t3 += Time.deltaTime;
            }
        }
    }
}
