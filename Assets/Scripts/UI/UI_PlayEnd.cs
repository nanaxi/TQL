using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using ProtoBuf;

public struct ThePlayerInfo
{
    string Name;
    string Gold;
    Sprite Head;
    string ThisGold;
    int[] Tag;

    public string Name1
    {
        get
        {
            return Name;
        }

        set
        {
            Name = value;
        }
    }

    public string Gold1
    {
        get
        {
            return Gold;
        }

        set
        {
            Gold = value;
        }
    }

    public Sprite Head1
    {
        get
        {
            return Head;
        }

        set
        {
            Head = value;
        }
    }

    public string ThisGold1
    {
        get
        {
            return ThisGold;
        }

        set
        {
            ThisGold = value;
        }
    }
    /// <summary>
    /// 0胡  1自摸   2放炮   3没有胡
    /// </summary>
    public int[] Tag1
    {
        get
        {
            return Tag;
        }

        set
        {
            Tag = value;
        }
    }

    public ThePlayerInfo(string name = "缺省", string gold = "缺省", Sprite head = null, string thisgold = "缺省", int[] tag = null)
    {
        Name = name;
        Gold = gold;
        Head = head;
        ThisGold = thisgold;
        Tag = tag;
    }

}

public class UI_PlayEnd : MonoBehaviour
{
    Transform ThisTrans = null;
    Button Share = null;
    Button Ready = null;
    GameObject Hu, ZiMo, DianPao;
    public GameObject Player0Cards;
    public GameObject Player1Cards;
    public GameObject Player2Cards;
    public GameObject Player3Cards;
    /// <summary>
    /// 手牌
    /// </summary>
    public GameObject card;
    public GameObject backCard;
    public GameObject gangcard;
    public GameObject nullCard;
    //List<PlayerInfo> Players = new List<PlayerInfo>();
    /// <summary>
    /// 放杠和放炮的次数
    /// </summary>
    public Text[] fgang=new Text[4];
    void Awake()
    {
        Init();
    }

    // Use this for initialization
    void Start()
    {
        GameManager.GM.GameEnd = true;
        PublicEvent.GetINS.Event_ExitRoomSucc += Rest;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        Invoke("show", 4.0f);
    }
    void show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        ShowFace.Ins.DistoryDelayAnim();
    }
    void Init()
    {
        ThisTrans = this.gameObject.transform;
        Share = ThisTrans.Find("BG/Share").GetComponent<Button>();
        Ready = ThisTrans.Find("BG/Ready").GetComponent<Button>();
    }
    public void Default(ThePlayerInfo[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            ThisTrans.Find("BG/Player" + (1 + i) + "/Tag0").gameObject.SetActive(false);
            ThisTrans.Find("BG/Player" + (1 + i) + "/Tag1").gameObject.SetActive(false);
            ThisTrans.Find("BG/Player" + (1 + i) + "/Tag2").gameObject.SetActive(false);
        }

        for (int i = 0; i < players.Length; i++)
        {
            ThePlayerInfo Temp = new ThePlayerInfo(players[i].Name1, players[i].Gold1, players[i].Head1, players[i].ThisGold1, players[i].Tag1);
            ThisTrans.Find("BG/Player" + (1 + i) + "/Head/Mask/HeadSprite").GetComponent<Image>().sprite = Temp.Head1;
            ThisTrans.Find("BG/Player" + (1 + i) + "/NickName").GetComponent<Text>().text = "昵称：" + Temp.Name1;
            ThisTrans.Find("BG/Player" + (1 + i) + "/AllGold").GetComponent<Text>().text = "积分：" + Temp.Gold1;
            ThisTrans.Find("BG/Player" + (1 + i) + "/ThisGold").GetComponent<Text>().text = Temp.ThisGold1;
            for (int p = 0; p < players[i].Tag1.Length; p++)
            {
                if (players[i].Tag1[p] == 1)
                    ThisTrans.Find("BG/Player" + (1 + i) + "/Tag" + p).gameObject.SetActive(true);
            }
        }
        Share.onClick.AddListener(ShareToWX);
        Ready.onClick.AddListener(ReadyToPlay);
    }
    void ShareToWX()
    {
        Debug.Log("Share");
        GameManager.GM.Share(0);
    }
    void ReadyToPlay()
    {
        if (GameManager.GM.DS.PlayRoom.GetComponent<UI_PlayRoom>().junum > 20 || GameManager.GM.DS.PlayRoom.GetComponent<UI_PlayRoom>().junum < 0)
        {
            Debug.Log("GameManager.GM.DS.PlayRoom.GetComponent<UI_PlayRoom>().junum：" + GameManager.GM.DS.PlayRoom.GetComponent<UI_PlayRoom>().junum);
            GameManager.GM.DS.GameOver = GameManager.GM.PopUI(ResPath.GameOver);
            if (GameManager.GM.DS.GameOver != null)
            {
                Debug.Log("GameOver的UI不为空");
                GameManager.GM.DS.GameOver.GetComponent<UI_GameOver>().Default(PublicEvent.GetINS.GameOverRsp);
            }
            Rest();
        }
        else
        {
            PublicEvent.GetINS.Fun_ReadyToPlayNew();
            Debug.Log("Ready");
            Rest();
        }

    }
    void Rest()
    {
        GameManager.GM.GameEnd = false;
        PublicEvent.GetINS.Event_ExitRoomSucc -= Rest;
        Debug.Log("发生销毁！");
        GameManager.GM.DS.PlayEnd = null;
        Destroy(this.gameObject);
        Destroy(this);
    }
    public void SetCard(ProtoBuf.MJGameOver rsp)
    {

        for (int i = 0; i < rsp.players.Count; i++)
        {
            if (i == 0)
            {
                SetPlayerCard(Player0Cards, rsp, i);
            }
            if (i == 1)
            {
                SetPlayerCard(Player1Cards, rsp, i);
            }
            if (i == 2)
            {
                SetPlayerCard(Player2Cards, rsp, i);
            }
            if (i == 3)
            {
                SetPlayerCard(Player3Cards, rsp, i);
            }
        }
    }
    #region 牌面处理
    void SetPlayerCard(GameObject Player, ProtoBuf.MJGameOver rsp, int num)
    {
        for (int k = 0; k < rsp.players[num].pengCards.Count; k++)//加入碰的牌
        {
            SetPengCard(Player.transform, rsp.players[num].pengCards[k].ToCard().ToName());
        }

        for (int l = 0; l < rsp.players[num].gangInfos.Count; l++)//加入杆的牌
        {
            if (rsp.players[num].gangInfos[l].catag == 2)//说明当前的这个牌是暗牌
            {
                SetAnGangCard(Player.transform, rsp.players[num].gangInfos[l].card.ToCard().ToName());
            }
            else//说明这个是明牌
            {
                SetGangCard(Player.transform, rsp.players[num].gangInfos[l].card.ToCard().ToName());
            }
        }
        int cards = rsp.players[num].restCards.Count;//手牌数量
        rsp.players[num].restCards.Sort();//排序
        for (int p = 0; p < cards; p++)
        {
            SetHandCard(Player.transform, rsp.players[num].restCards[p].ToCard().ToName());

        }
        for (int z = 0; z < rsp.players[num].huInfos.Count; z++)//胡牌
        {
            if (rsp.players[num].huInfos[0].card != 216)
            {
                SetNullCard(Player.transform);
                SetHandCard(Player.transform, rsp.players[num].huInfos[0].card.ToCard().ToName());
            }
        }



        //放杠次数rsp.players[0].fanggangInfos[0].ganger  "放炮次数:" + rsp.players[0].fpInfos.Count;
        List<string> Temp = new List<string>();
        //Temp.Clear();
        //if (rsp.players[0].fanggangInfos.Count > 0)
        //{
        //    for (int i = 0; i < rsp.players[0].fanggangInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[0].fanggangInfos[i].ganger) + "放杠 ");
        //        //Debug.Log("players[0]给" + GameManager.GM.GetPlayerName(rsp.players[0].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    for (int i = 0; i < rsp.players[0].fpInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[0].fpInfos[i].huer) + "放炮 ");
        //        //Debug.Log("players[0]给" + GameManager.GM.GetPlayerName(rsp.players[0].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    if (Temp.Count > 1)
        //        for (int i = 1; i < Temp.Count; i++)
        //        {
        //            Temp[0].Insert(Temp[0].Length, Temp[i]);
        //        }
        //    fgang[0].text = Temp[0];
        //}
        //Temp.Clear();
        //if (rsp.players[1].fanggangInfos.Count > 0)
        //{
        //    for (int i = 0; i < rsp.players[1].fanggangInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[1].fanggangInfos[i].ganger) + "放杠 ");
        //        //Debug.Log("players[1]给" + GameManager.GM.GetPlayerName(rsp.players[1].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    for (int i = 0; i < rsp.players[1].fpInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[1].fpInfos[i].huer) + "放炮 ");
        //        //Debug.Log("players[1]给" + GameManager.GM.GetPlayerName(rsp.players[1].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    if (Temp.Count > 1)
        //        for (int i = 1; i < Temp.Count; i++)
        //        {
        //            Temp[0].Insert(Temp[0].Length, Temp[i]);
        //        }
        //    fgang[1].text = Temp[0];
        //}
        //Temp.Clear();
        //if (rsp.players[2].fanggangInfos.Count > 0)
        //{
        //    for (int i = 0; i < rsp.players[2].fanggangInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[2].fanggangInfos[i].ganger) + "放杠 ");
        //        //Debug.Log("players[2]给" + GameManager.GM.GetPlayerName(rsp.players[2].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    for (int i = 0; i < rsp.players[2].fpInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[2].fpInfos[i].huer) + "放炮 ");
        //        //Debug.Log("players[2]给" + GameManager.GM.GetPlayerName(rsp.players[2].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    if (Temp.Count > 1)
        //        for (int i = 1; i < Temp.Count; i++)
        //        {
        //            Temp[0].Insert(Temp[0].Length, Temp[i]);
        //        }
        //    fgang[2].text = Temp[0];
        //}
        //Temp.Clear();
        //if (rsp.players[3].fanggangInfos.Count > 0)
        //{
        //    for (int i = 0; i < rsp.players[3].fanggangInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[3].fanggangInfos[i].ganger) + "放杠 ");
        //        //Debug.Log("players[3]给" + GameManager.GM.GetPlayerName(rsp.players[3].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    for (int i = 0; i < rsp.players[3].fpInfos.Count; i++)
        //    {
        //        Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[3].fpInfos[i].huer) + "放炮 ");
        //        //Debug.Log("players[3]给" + GameManager.GM.GetPlayerName(rsp.players[3].fanggangInfos[i].ganger) + "放杠 ");
        //    }
        //    if (Temp.Count > 1)
        //        for (int i = 10; i < Temp.Count; i++)
        //        {
        //            Debug.Log("" + Temp[i]);
        //            Temp[0].Insert(Temp[0].Length, Temp[i]);
        //        }
        //    fgang[3].text = Temp[0];
        //}
        //Temp.Clear();
        for (int P = 0; P < 4; P++)
        {
            //for (int i = 0; i < rsp.players[P].fanggangInfos.Count; i++)
            //{
            //    Debug.Log("当前玩家"+GameManager.GM.GetPlayerName(rsp.players[P].charId) +"的ganger:" + GameManager.GM.GetPlayerName(rsp.players[P].fanggangInfos[i].ganger));
            //}
            if(rsp.players[P].fanggangInfos.Count>0)
            Temp.Add("放杠次数："+ rsp.players[P].fanggangInfos.Count+"    ");
            //if (rsp.players[P].fanggangInfos.Count > 1)
            //{
            //    for (int i = 0; i < 4; i++)//玩家总数
            //    {
            //        int Gangnum = 0;
            //        for (int Z = 0; Z < rsp.players[P].fanggangInfos.Count; Z++)
            //        {
            //            if (GameManager.GM._AllPlayerData[i].ID == rsp.players[P].fanggangInfos[Z].ganger)
            //            {
            //                Gangnum++;
            //            }
            //        }
            //        //Debug.Log(GameManager.GM._AllPlayerData[i].Name + "杠的次数：" + Gangnum);
            //        if(Gangnum!=0)
            //        Temp.Add("给" + GameManager.GM.GetPlayerName(GameManager.GM._AllPlayerData[i].ID) + "放杠" + "X" + Gangnum + " ");
            //    }
            //}
            //for (int i = 0; i < 4; i++)
            //{
            //    int Gangnum = 0;
            //    for (int Z = 0; Z < rsp.players[P].fanggangInfos.Count; Z++)
            //    {
            //        if (GameManager.GM._AllPlayerData[i].ID == rsp.players[P].fanggangInfos[Z].ganger)
            //        {
            //            Gangnum++;
            //        }
            //    }
            //}
            if (rsp.players[P].fpInfos.Count > 0)
            {
                for (int i = 0; i < rsp.players[P].fpInfos.Count; i++)
                {
                    Temp.Add("给" + GameManager.GM.GetPlayerName(rsp.players[P].fpInfos[i].huer) + "放炮 ");
                }
            }
            if (Temp.Count > 1)
                for (int i = 1; i < Temp.Count; i++)
                {
                    Temp[0] += Temp[i];
                }
            if(Temp.Count>0)
                fgang[P].text = Temp[0];
            Temp.Clear();
        }
    }
    /// <summary>
    /// 设定当前的空牌给指定的位置
    /// </summary>
    /// <param name="SetPostion"></param>
    void SetNullCard(Transform SetPostion)
    {
        var temp = Instantiate(nullCard);
        temp.SetActive(true);
        temp.transform.SetParent(SetPostion, false);
    }
    /// <summary>
    /// 放置当前暗刚的位置
    /// </summary>
    /// <param name="Setposetiom"></param>
    /// <param name="card"></param>
    void SetAnGangCard(Transform Setpostion, string thecard)
    {
        var t = Instantiate(backCard);
        t.SetActive(true);
        t.transform.SetParent(Setpostion, false);
        t.transform.Find("card/Image").GetComponent<Image>().sprite = GetPrefabSprite(thecard);
        SetNullCard(Setpostion);
        SetNullCard(Setpostion);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Setpostion"></param>
    /// <param name="thecard"></param>
    void SetGangCard(Transform Setpostion, string thecard)
    {
        var t = Instantiate(gangcard);
        t.SetActive(true);
        t.transform.SetParent(Setpostion, false);
        t.transform.Find("Image").GetComponent<Image>().sprite = GetPrefabSprite(thecard);
        t.transform.Find("Right/Image").GetComponent<Image>().sprite = GetPrefabSprite(thecard);
        t.transform.Find("card/Image").GetComponent<Image>().sprite = GetPrefabSprite(thecard);
        SetNullCard(Setpostion);
        SetNullCard(Setpostion);
        SetNullCard(Setpostion);
    }
    /// <summary>
    /// 设定当前的碰牌
    /// </summary>
    /// <param name="Setpostion"></param>
    /// <param name="thecard"></param>
    void SetPengCard(Transform Setpostion, string thecard)
    {
        for (int i = 0; i < 3; i++)
        {
            var t = Instantiate(card);
            t.SetActive(true);
            t.transform.SetParent(Setpostion, false);
            t.transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(thecard);
        }
        SetNullCard(Setpostion);
    }
    /// <summary>
    /// 设定当前的牌
    /// </summary>
    /// <param name="Setpostion"></param>
    /// <param name="theCard"></param>
    void SetHandCard(Transform Setpostion, string theCard)
    {
        var t = Instantiate(card);
        t.SetActive(true);
        t.transform.SetParent(Setpostion, false);
        t.transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(theCard);
    }
    Sprite tempSprite = null;
    Sprite GetPrefabSprite(string name)
    {
        tempSprite = Resources.Load<GameObject>("Prefabs/Cards/" + name).GetComponent<Image>().sprite;
        if (tempSprite != null)
        {
            return tempSprite;
        }
        else
        {
            Debug.Log("老哥，翻车啦！");
            tempSprite = Resources.Load<Image>("Prefabs/Cards/5h").sprite;
        }
        return tempSprite;

    }
    #endregion
}
