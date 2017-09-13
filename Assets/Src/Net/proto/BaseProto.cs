using System;
using UnityEngine;
using System.Collections;
using ProtoBuf;

// 玩家信息

public struct PlayerInfo
{
    /// <summary>
    /// 玩家唯一id
    /// </summary>
    public UInt32 m_id;
    /// <summary>
    /// channel_user_id
    /// </summary>             
    public string m_account;
    /// <summary>
    /// // 玩家当前在哪里（比如玩家已经在玩麻将，此时强制杀掉进程，重新登录后，会再次进入上次的游戏内）
    /// </summary>
    public GameType m_inGame;
    /// <summary>
    /// 玩家所创建的房间号，如果没有则为0
    /// </summary>        
    public UInt32 m_cdRoomId;
    /// <summary>
    /// 玩家当前所在的房间号，如果没有则为0
    /// </summary> 
    public UInt32 m_atRoomId;
    /// <summary>
    ///玩家等级，暂时用不到，留着扩展
    /// </summary>
    public UInt32 m_level;
    /// <summary>
    ///玩家房卡数
    /// </summary>          
    public UInt32 m_roomCard;
    /// <summary>
    ///玩家钻石数
    /// </summary>
    public UInt32 m_diamond;
    /// <summary>
    ///玩家金币数
    /// </summary>
    public int m_gold;

    public string m_ip;

    public int sex;//（1MAN 2Woman）
}

public class BaseProto : ISingleton<BaseProto>
{
    /// <summary>
    /// 查询 战绩和回访数据
    /// </summary>
    /// <param name="charId"></param>
    /// <returns></returns>
    //public bool QueryInfoRequest(UInt32 charId)
    public bool QueryInfoRequest(QueryInfoReq pack)
    {
        UInt16 command = (UInt16)CLIToLGIProtocol.CLI_TO_LGI_QUERY_INFO;
        Debug.Log("申请了" + pack.queryType);
        return GameNetWork.Inst().SendDataToLoginServer(command, pack);
    }
    /// <summary>
    /// 接收查询到的战绩信息和回访数据
    /// </summary>
    /// <param name="rsp"></param>
    /// <returns></returns>
    public bool QueryInfoResponse(QueryInfoRsp rsp)
    {
        Debug.Log("收到了" + rsp.queryType);

        switch (rsp.queryType)
        {
            case QueryInfoRsp.QueryType.ZhanJi:
                PublicEvent.GetINS.ReciveZhanJiHuiFang(rsp);
                break;
            //case QueryInfoRsp.QueryType.HuiFang:
            //    break;
            case QueryInfoRsp.QueryType.FriendList:
                break;
            case QueryInfoRsp.QueryType.CharInfo:
                PublicEvent.GetINS.ReciveDiamond(rsp);
                break;
            default:
                break;
        }

        return true;
    }


    /// <summary>
    ///在进入游戏的时候已经录入了相关的信息。 m_id ，m_account， m_inGame，m_cdRoomId，m_atRoomId，m_level，m_roomCard，m_diamond，m_gold，m_ip
    /// </summary>
    static public PlayerInfo playerInfo;
    static public uint SeverPlayerNum = 0;


    // 选择某个游戏，比如选择麻将
    public bool EnterGameRequest(GameType gameType)
    {
        EnterGameReq pack = new EnterGameReq();
        UInt16 command = (UInt16)CLIToLGIProtocol.CLI_TO_LGI_ENTER_GAME;
        pack.charId = playerInfo.m_id;
        pack.enterGame = gameType;
        Debug.LogWarning(string.Format("Send CLI_TO_LGI_ENTER_GAME {0}", gameType));
        return GameNetWork.Inst().SendDataToLoginServer(command, pack);
    }

    // 选择某个游戏，比如选择麻将 --> 服务器返回包
    public bool EnterGameResponse(EnterGameRsp rsp)
    {
        if (rsp.result == EnterGameRsp.Result.SUCC)
        {
            playerInfo.m_id = rsp.charId;
            playerInfo.m_inGame = rsp.enterGame;
            playerInfo.m_account = rsp.channel_user_id;
            Debug.LogWarning("Recv LGI_TO_CLI_ENTER_GAME Succ");
            PublicEvent.GetINS.Fun_SuccessIntoHall();
        }
        else
        {
            PublicEvent.GetINS.Fun_FaillIntoHall();
            Debug.LogWarning("Recv LGI_TO_CLI_ENTER_GAME Fail");
        }
        return true;
    }

    // 退出某个游戏，比如选择麻将
    public bool ExitGameRequest()
    {
        ExitGameReq pack = new ExitGameReq();
        UInt16 command = (UInt16)CLIToLGIProtocol.CLI_TO_LGI_EXIT_GAME;
        pack.charId = playerInfo.m_id;
        pack.exitGame = playerInfo.m_inGame;
        Debug.LogWarning("Send CLI_TO_LGI_EXIT_GAME");
        return GameNetWork.Inst().SendDataToLoginServer(command, pack);
    }

    // 退出某个游戏，比如退出麻将 --> 服务器返回包
    public bool ExitGameResponse(ExitGameRsp rsp)
    {
        if (rsp.result == ExitGameRsp.Result.SUCC)
        {
            playerInfo.m_inGame = rsp.exitGame;
            Debug.LogWarning("Recv LGI_TO_CLI_EXIT_GAME Succ");
        }
        else
        {
            Debug.LogWarning("Recv LGI_TO_CLI_EXIT_GAME Fail");
        }
        return true;
    }

    // 创建某个游戏的房间
    public bool CreateRoomRequest(CreateRoomReq reqPack)
    {
        UInt16 command = (UInt16)CLIToLGIProtocol.CLI_TO_LGI_CREATE_ROOM;
        reqPack.charId = BaseProto.playerInfo.m_id;
        reqPack.account = "";
        Debug.LogWarning("Send CLI_TO_LGI_CREATE_ROOM");
        return GameNetWork.Inst().SendDataToLoginServer(command, reqPack);
    }



    // 创建某个游戏的房间 --> 服务器返回包
    public bool CreateRoomResponse(CreateRoomRsp rsp)
    {
        if (rsp.result == CreateRoomRsp.Result.SUCC)
        {
            playerInfo.m_id = rsp.charId;
            playerInfo.m_cdRoomId = rsp.roomId;
            playerInfo.m_inGame = rsp.gameType;
            Debug.LogWarning("Recv LGI_TO_CLI_CREATE_ROOM Succ");
            PublicEvent.GetINS.Fun_reciveIsCreatRoom(rsp);
            EnterRoomRequest();
        }
        else
        {
            Debug.LogWarning("Recv LGI_TO_CLI_CREATE_ROOM Fail");
            PublicEvent.GetINS.Fun_reciveIsCreatRoom(rsp);
        }
        return true;
    }

    // 进入某个游戏的房间
    public bool EnterRoomRequest()
    {
        EnterRoomReq pack = new EnterRoomReq();
        UInt16 command = (UInt16)CLIToLGIProtocol.CLI_TO_LGI_ENTER_ROOM;
        pack.charId = playerInfo.m_id;
        pack.gameType = playerInfo.m_inGame;
        pack.roomId = playerInfo.m_cdRoomId;
        pack.account = "";
        Debug.LogWarning("Send CLI_TO_LGI_ENTER_ROOM");
        Debug.Log("请求进入房间 " + pack.gameType);
        return GameNetWork.Inst().SendDataToLoginServer(command, pack);
    }

    // 进入某个游戏的房间 --> 服务器返回包
    public bool EnterRoomResponse(EnterRoomRsp rsp)
    {

        if (rsp.result == EnterRoomRsp.Result.SUCC)
        {
            SeverPlayerNum = (uint)rsp.mjRoom.charIds.Count;
            //rsp.mjRoom.roomRuleInfo.xlchRule.xlch
            Debug.Log("服务器返回包：进入房间成功！EnterRoomResponse");
            playerInfo.m_inGame = rsp.gameType;
            PublicEvent.GetINS.Fun_JoinResult(rsp);
            //进入房间之后就把当前的房间号码记录下来 
            playerInfo.m_atRoomId = rsp.roomId;

            Debug.LogWarning("Recv LGI_TO_CLI_ENTER_ROOM Succ");
        }
        else
        {

            PublicEvent.GetINS.Fun_JoinResult(rsp);
            //    Debug.Log("进入服务器失败");
        }
        
        return true;
    }

    // 退出某个游戏的房间
    public bool ExitRoomRequest()
    {
        ExitRoomReq pack = new ExitRoomReq();
        UInt16 command = (UInt16)CLIToLGIProtocol.CLI_TO_LGI_EXIT_ROOM;
        pack.charId = playerInfo.m_id;
        pack.roomId = playerInfo.m_atRoomId;
        Debug.LogWarning("Send CLI_TO_LGI_EXIT_ROOM");
        return GameNetWork.Inst().SendDataToLoginServer(command, pack);
    }

    // 退出某个游戏的房间 --> 服务器返回包
    public bool ExitRoomResponse(ExitRoomRsp rsp)
    {
        if (rsp.result == ExitRoomRsp.Result.SUCC)
        {
            playerInfo.m_atRoomId = 0;
            playerInfo.m_cdRoomId = 0;
            PublicEvent.GetINS.Fun_ExitRoomSucc();
            Debug.Log("Recv LGI_TO_CLI_EXIT_ROOM Succ");
        }
        else
        {
            Debug.Log("Recv LGI_TO_CLI_EXIT_ROOM Fail");
        }
        return true;
    }

    // 发送聊天消息
    public bool SendChatMsgRequest(ChatMessageReq pack)
    {
        UInt16 command = (UInt16)CLIToLGIProtocol.CLI_TO_LGI_CHAT_MESAGE;
        //  Debug.Log("发送消息"+pack.senderId+pack.msgType);
        return GameNetWork.Inst().SendDataToLoginServer(command, pack);
    }

    // 收到别人发来的聊天消息
    public bool RecvChatMsgResponse(ChatMessageRsp rsp)
    {
        Debug.Log("收到别人发的消息");

        PublicEvent.GetINS.Fun_reciveOtherMessage(rsp);
        return true;
    }

    // 服务器广播消息，比如走马灯之类的 LGI_TO_CLI_NOTIFY_MESSAGE
    public bool NotifyMessage(NotifyServerMessage rsp)
    {
        switch (rsp.msgType)
        {
            case NotifyServerMessage.MsgType.ROLL:
                PublicEvent t = new PublicEvent();
                t.Fun_recivePublicText(rsp.roll);
                break;
            case NotifyServerMessage.MsgType.LETTER:

                break;
            default:
                break;
        }

        return true;
    }

    // LGI_TO_CLI_ROOM_INFO
    /// <summary>
    /// 有玩家进入或者退出房间，刷新房间里面的消息
    /// </summary>
    /// <param name="roomInfo"></param>
    /// <returns></returns>
    public bool OnSyncRoomInfo(SyncRoomInfo roomInfo)
    {
        ProtoBuf.CharacterInfo temp = null;
        for (int i = 0; i < roomInfo.charIds.Count; i++)
        {
            if (roomInfo.charIds[i] == roomInfo.triggerCharId)
            {
                temp = roomInfo.charInfos[i];
                break;
            }
        }
        if (temp == null)
        {
            int seat = GameManager.GM.GetPlayerNum(roomInfo.triggerCharId);
            GameManager.GM.DeletePlayerData(roomInfo.triggerCharId);
        }
        else
        {
            SeverPlayerNum = (uint)roomInfo.charIds.Count;
            //PublicEvent.GetINS.Fun_SameIpTip((int)SeverPlayerNum);
            GameManager.GM.JoinPlayerData(temp);
        }
        return true;
    }
}
