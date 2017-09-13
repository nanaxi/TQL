using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using ProtoBuf;

public class UI_GameOver : MonoBehaviour
{
    Transform ThisTrans = null;
    Button Share = null;
    Button EndToHall = null;
    void Awake()
    {
        Init();
    }
    // Use this for initialization
    void Start()
    {
        //Init();
        Default();
    }
    /// <summary>
    /// 初始化并且获取组件
    /// </summary>
    void Init()
    {
        ThisTrans = transform;
        Share = ThisTrans.Find("BG/Share").GetComponent<Button>();
        EndToHall = ThisTrans.Find("BG/EndToHall").GetComponent<Button>();
        Share.onClick.AddListener(ShareToWX);
        EndToHall.onClick.AddListener(End);
        //Default();
    }
    /// <summary>
    /// 初始化数据,绑定函数
    /// </summary>
    public void Default(MJGameOver Thisrsp = null)
    {
       
        List<int> yinjia = new List<int>();
        if (Thisrsp != null)
            for (int i = 0; i < Thisrsp.players.Count; i++)
            {
                int[] tag = new int[] { 0, 0, 0, 0, 0, 0 };
                Debug.Log("当前人员的id：" + Thisrsp.players[i].charId);
                Debug.Log("当前人员的AllGold：" + Thisrsp.players[i].restCards);
                Debug.Log("当前人员的gold：" + Thisrsp.players[i].charId);
                Debug.Log("当前人员的hu：" + Thisrsp.players[i].huInfos.Count);
                //龙七对：X1
                //小七对：X1
                //大对子：X1
                //平胡清一色：X1
                //平胡混一色：X1
                //平胡：X1

                for (int p = 0; p < Thisrsp.players[i].huInfos.Count; p++)
                {
                    if (Thisrsp.players[i].huInfos[p].catag > 1 && Thisrsp.players[i].huInfos[p].catag < 5)
                    {//龙七对
                        tag[0]++;
                    }
                    if (Thisrsp.players[i].huInfos[p].catag > 4 && Thisrsp.players[i].huInfos[p].catag < 8)
                    {//小七对
                        tag[1]++;
                    }
                    if (Thisrsp.players[i].huInfos[p].catag > 7 && Thisrsp.players[i].huInfos[p].catag < 11)
                    {//大对子
                        tag[2]++;
                    }
                    if (Thisrsp.players[i].huInfos[p].catag == 11)
                    {//平胡清一色  
                        tag[3]++;
                    }
                    if (Thisrsp.players[i].huInfos[p].catag == 12)
                    {//平胡混一色
                        tag[4]++;
                    }
                    if (Thisrsp.players[i].huInfos[p].catag == 13)
                    {//平胡
                        tag[5]++;
                    }
                }
                int pos = GameManager.GM.GetPlayerNum(Thisrsp.players[i].charId);
                GameManager.GM.GetHead(GameManager.GM._AllPlayerData[pos].Head, SetHead, i);

                string temp = "BG/Player" + i.ToString() + "/NickName";
                //Debug.Log(temp);
                ThisTrans.FindChild(temp).GetComponent<Text>().text = "昵称：" + GameManager.GM.GetPlayerName(Thisrsp.players[i].charId);

                temp = "BG/Player" + i.ToString() + "/AllGold";
                //Debug.Log(temp);
                ThisTrans.FindChild(temp).GetComponent<Text>().text = "积分：" + Thisrsp.players[i].restGold.ToString();

                temp = "BG/Player" + i.ToString() + "/FinalGold";
                //Debug.Log(temp);
                ThisTrans.FindChild(temp).GetComponent<Text>().text = Thisrsp.players[i].restGold.ToString();

                
                //Debug.Log(temp);
                yinjia.Add(Thisrsp.players[i].restGold);
                //if (Thisrsp.players[i].restGold > 0)
                //{
                //    ThisTrans.FindChild(temp).gameObject.SetActive(true);
                //}
                //else
                //{
                //    ThisTrans.FindChild(temp).gameObject.SetActive(false);
                //}

                temp = "BG/Player" + i.ToString() + "/Content";
                ThisTrans.FindChild(temp).GetComponent<Text>().text = "龙七对：X" + tag[0] + "\n" + "小七对：X" + tag[1] + "\n" + "大对子：X" + tag[2] + "\n" + "平胡清一色：X" + tag[3] + "\n" + "平胡混一色：X" + tag[4] + "\n" + "平胡：X" + tag[5];

            }
        for (int i = 0; i < yinjia.Count; i++)
        {
            for (int p = 0; p < yinjia.Count; p++)
            {
                if (yinjia[i] < yinjia[p])
                {
                    yinjia[i] = 0;
                }
            }
        }
        for (int i = 0; i < yinjia.Count; i++)
        {
            string temp = "BG/Player" + i.ToString() + "/Tag3";
            if (yinjia[i] > 0)
            {
                ThisTrans.FindChild(temp).gameObject.SetActive(true);
            }
            else
            {
                ThisTrans.FindChild(temp).gameObject.SetActive(false);
            }
        }
    }
    void SetHead(Sprite sprite, int num = 0)
    {
        ThisTrans.FindChild("BG/Player" + num + "/Head/Mask/HeadSprite").GetComponent<Image>().sprite = sprite;
    }
    void ShareToWX()
    {
        Debug.Log("分享");
        GameManager.GM.Share(0);
    }
    void End()
    {
        PublicEvent.GetINS.Fun_ExitRoomSucc();
        Debug.Log("返回大厅");

        Invoke("Rest", 0.2f);
    }
    void Rest()
    {
        GameManager.GM.DS.GameOver = null;
        Destroy(this.gameObject);
        Destroy(this);
    }
}
