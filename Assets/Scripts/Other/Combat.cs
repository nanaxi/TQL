using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/// <summary>
/// 单个小的
/// </summary>
public class Combat : MonoBehaviour
{
    public Text Time;
    public Text RoomNumber;
    public Text Player0, Player1, Player2, Player3;
    public Button Open;
    // Use this for initialization
    void Start()
    {
        Open.onClick.AddListener(delegate { OpenRound(); OpenLocalRound(); });
    }
    ProtoBuf.MJRoomRecord RecordRsp;


    /// <summary>
    /// 总结算
    /// </summary>
    /// <param name="Record"></param>
    public void SetCombatInformation(ProtoBuf.MJRoomRecord Record)
    {
        RecordRsp = Record;
        RoomNumber.text = "房间号:" + Record.roomId.ToString();


        string time = Record.rounds[0].roundOverTime.ToString();
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(time + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        var TimeOut = dtStart.Add(toNow);

        Time.text = "时间：" + TimeOut.Month + "月" + TimeOut.Day + "日";
        int[] p = new int[4];
        //for (int i = 0; i < Record.rounds.Count; i++)
        //{
        //    for (int z = 0; z < Record.rounds[i].players.Count; z++)
        //    {
        //        p[z] += Record.rounds[i].players[z].;
        //    }
        //}
        Player0.text = GameManager.GM.ToName(Record.rounds[0].players[0].name) + "得分:" + Record.rounds[0].players[0].restGold;
        Player1.text = GameManager.GM.ToName(Record.rounds[0].players[1].name) + "得分:" + Record.rounds[0].players[1].restGold;
        Player2.text = GameManager.GM.ToName(Record.rounds[0].players[2].name) + "得分:" + Record.rounds[0].players[2].restGold;
        Player3.text = GameManager.GM.ToName(Record.rounds[0].players[3].name) + "得分:" + Record.rounds[0].players[3].restGold;
    }
    ProtoBuf.MJRoundRecord MjRoundRecordRsp;
    /// <summary>
    /// 设定当前的信息
    /// </summary>
    /// <param name="Record"></param>
    public void SetLocalCombatInformation(ProtoBuf.MJRoundRecord Record)
    {
        MjRoundRecordRsp = Record;
        RoomNumber.text = "房间号:" + Record.playBack.roomId.ToString();


        string time = Record.roundOverTime.ToString();
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(time + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        var TimeOut = dtStart.Add(toNow);

        Time.text = "时间：" + TimeOut.Month + "月" + TimeOut.Day + "日"+ TimeOut.Hour+"时"+ TimeOut.Minute+"分";

        Player0.text = GameManager.GM.ToName(Record.players[0].name) + "得分:" + Record.players[0].gold;
        Player1.text = GameManager.GM.ToName(Record.players[1].name) + "得分:" + Record.players[1].gold;
        Player2.text = GameManager.GM.ToName(Record.players[2].name) + "得分:" + Record.players[2].gold;
        Player3.text = GameManager.GM.ToName(Record.players[3].name) + "得分:" + Record.players[3].gold;
    }

    void OpenRound()
    {
        if (RecordRsp != null)
        {
            GameObject item = null;
            Debug.Log("参加局数：" + RecordRsp.rounds.Count);
            item = GameManager.GM.PopUI("Prefabs/CombatItems");
            GameManager.GM.DS.CombatItems = item;
            item.GetComponent<CombatItems>().SetComBat(RecordRsp);
        }
    }
    void OpenLocalRound()
    {
        if (MjRoundRecordRsp != null)
        {
            Debug.Log("进入游戏！");
            if (GameManager.GM.DS.PlayBack == null)
            {
                ParticleManager.GetIns.SwitchSence(2);
                var temp = GameManager.GM.PopUI(ResPath.PlayBack);
                temp.GetComponent<UI_PlayBack>().ReciveRoomData(MjRoundRecordRsp);
                GameManager.GM.DS.PlayBack = temp;
                
            }     
        }
    }
}
