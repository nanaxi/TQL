using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardManager : MonoBehaviour
{
    public GameObject 我牌的位置;
    public Vector2 Vect_我牌的位置;
    public GameObject 左牌的位置;
    public Vector2 Vect_左牌的位置;
    public GameObject 上牌的位置;
    public Vector2 Vect_上牌的位置;
    public GameObject 右牌的位置;
    public Vector2 Vect_右牌的位置;

    public GameObject 回放左边位置;
    public Vector2 Vect_回放左边位置;
    public GameObject 回放上边位置;
    public Vector2 Vect_回放上边位置;
    public GameObject 回放右边位置;
    public Vector2 Vect_回放右边位置;

    public GameObject All;



    //本地牌lIST
    public List<GameObject> 我的牌列表 = new List<GameObject>();
    private List<GameObject> 我现有牌的列表 = new List<GameObject>();
    public List<GameObject> 我的碰杠 = new List<GameObject>();
    public List<GameObject> 我的出牌 = new List<GameObject>();

    public GameObject 我的胡牌;
    public GameObject 我的出牌动画;



    //左本地牌List
    public List<GameObject> 左边游戏牌列表 = new List<GameObject>();
    public List<GameObject> 左边回放牌列表 = new List<GameObject>();


    private List<GameObject> 左边使用的牌列表 = new List<GameObject>();
    private List<GameObject> 左现有牌的列表 = new List<GameObject>();

    public List<GameObject> 左边的碰杠 = new List<GameObject>();
    public List<GameObject> 左边的出牌 = new List<GameObject>();

    public GameObject 左边的胡牌;
    public GameObject 左边的出牌动画;

    //上本地牌List
    public List<GameObject> 上边游戏牌列表 = new List<GameObject>();
    public List<GameObject> 上边回放牌列表 = new List<GameObject>();

    private List<GameObject> 上边使用的牌列表 = new List<GameObject>();
    private List<GameObject> 上现有牌的列表 = new List<GameObject>();

    public List<GameObject> 上边的碰杠 = new List<GameObject>();
    public List<GameObject> 上边的出牌 = new List<GameObject>();

    public GameObject 上边的胡牌;
    public GameObject 上边的出牌动画;

    //右本地牌List
    public List<GameObject> 右边游戏牌列表 = new List<GameObject>();
    public List<GameObject> 右边回放牌列表 = new List<GameObject>();

    private List<GameObject> 右边使用的牌列表 = new List<GameObject>();
    private List<GameObject> 右现有牌的列表 = new List<GameObject>();

    public List<GameObject> 右边的碰杠 = new List<GameObject>();
    public List<GameObject> 右边的出牌 = new List<GameObject>();


    public GameObject 右边的胡牌;
    public GameObject 右边的出牌动画;


    //出牌小箭头
    public GameObject 绿箭头;

    /// <summary>
    /// 本地手牌数据列表
    /// </summary>
    public List<uint> CardInMyHand = new List<uint>();

    //回放时使用 左上右 手牌
    public List<uint> CardInLeft = new List<uint>();
    public List<uint> CardInUp = new List<uint>();
    public List<uint> CardInRight = new List<uint>();

    /// <summary>
    /// 碰的牌要放在这个列表
    /// </summary>
    public List<uint> MyPeng = new List<uint>();
    public List<uint> LeftPeng = new List<uint>();
    public List<uint> TopPeng = new List<uint>();
    public List<uint> RightPeng = new List<uint>();
    /// <summary>
    /// 杠的牌要放在这个列表
    /// </summary>
    public List<uint> MyGang = new List<uint>();


    //碰杠都放里头 主要是用它的顺序
    public List<uint> MyPengGang = new List<uint>();
    public List<uint> LeftPengGang = new List<uint>();
    public List<uint> TopPengGang = new List<uint>();
    public List<uint> RightPengGang = new List<uint>();
    //换三张牌

    List<uint> H3zCards = new List<uint>();
    List<GameObject> H3zObj = new List<GameObject>();
    List<GameObject> H3zMask = new List<GameObject>();
    List<GameObject> H3zHandCard = new List<GameObject>();
    int H3zType = -1; //-1 null 0w 1t 2b
    int w;
    int t;
    int b;


    uint[][] Hulist = new uint[4][]; // charid * 胡顺序   0-2
    uint[][] ZHlist = new uint[4][];//  charid * 胡的类型 0点炮 1自摸 2没胡

    uint WaitGangCard = 999;  //等待杠的牌
    uint WaitGangCharid = 999;
    uint WaitGangOricharid = 999;
    GameObject WaitPopCard;   //等待出的牌



    int Que = -1; //-1 null 0w 1t 2b
    int ZhuangJiaid = -1;          //庄家 0是自己

    public bool KeyiChupai = false;     //是否可出牌
    int ShengqiPai = -1;         //本地升起的牌是哪张
    bool PengHouChuPai = false;  //true为碰后打牌状态
    int Click = -1; //0 不能点击
    bool inH3z = false;
    bool inXQ = false;

    //BaseUI_C MJUI;

    bool IsInRepick = false;     //重连 
    public bool IsInReview = false;     //回放


    void Ins()
    {
        Vect_我牌的位置 = 我牌的位置.GetComponent<RectTransform>().anchoredPosition;
        Vect_左牌的位置 = 左牌的位置.GetComponent<RectTransform>().anchoredPosition;
        Vect_上牌的位置 = 上牌的位置.GetComponent<RectTransform>().anchoredPosition;
        Vect_右牌的位置 = 右牌的位置.GetComponent<RectTransform>().anchoredPosition;

        Vect_回放左边位置 = 回放左边位置.GetComponent<RectTransform>().anchoredPosition;
        Vect_回放上边位置 = 回放上边位置.GetComponent<RectTransform>().anchoredPosition;
        Vect_回放右边位置 = 回放右边位置.GetComponent<RectTransform>().anchoredPosition;
        //MJUI = UIManager.Instance.FindBaseUI("PublicMJSpritsUI");
        MjAddListion();
        RestMyHandCardList(false);
    }
    //重置我现有牌的列表
    public void RestMyHandCardList(bool isinreview)
    {
        IsInReview = isinreview;
        #region 麻将对象初始化
        if (IsInReview)
        {
            Image[] MJimages = 我牌的位置.GetComponentsInChildren<Image>();
            foreach (var item in MJimages)
            {
                item.raycastTarget = false;
            }
            //左边的碰杠[0].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(45, 24);
            回放左边位置.SetActive(true);
            回放上边位置.SetActive(true);
            回放右边位置.SetActive(true);
            左牌的位置.SetActive(false);
            上牌的位置.SetActive(false);
            右牌的位置.SetActive(false);
        }
        else
        {
            Image[] MJimages = 我牌的位置.GetComponentsInChildren<Image>();
            foreach (var item in MJimages)
            {
                item.raycastTarget = true;
            }
            //左边的碰杠[0].transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(45, 0);

            左牌的位置.SetActive(true);
            上牌的位置.SetActive(true);
            右牌的位置.SetActive(true);
            回放左边位置.SetActive(false);
            回放上边位置.SetActive(false);
            回放右边位置.SetActive(false);
        }
        #endregion
        #region 麻将对象引用数据初始化
        我现有牌的列表.Clear();
        左现有牌的列表.Clear();
        上现有牌的列表.Clear();
        右现有牌的列表.Clear();

        左边使用的牌列表.Clear();
        右边使用的牌列表.Clear();
        上边使用的牌列表.Clear();

        for (int i = 0; i < 我的牌列表.Count; i++)
        {
            if (IsInReview)
            {
                //回放使用回放的牌
                左边使用的牌列表.Add(左边回放牌列表[i]);
                左现有牌的列表.Add(左边回放牌列表[i]);

                上边使用的牌列表.Add(上边回放牌列表[i]);
                上现有牌的列表.Add(上边回放牌列表[i]);

                右边使用的牌列表.Add(右边回放牌列表[i]);
                右现有牌的列表.Add(右边回放牌列表[i]);
            }
            else
            {
                左边使用的牌列表.Add(左边游戏牌列表[i]);
                左现有牌的列表.Add(左边游戏牌列表[i]);

                上边使用的牌列表.Add(上边游戏牌列表[i]);
                上现有牌的列表.Add(上边游戏牌列表[i]);

                右边使用的牌列表.Add(右边游戏牌列表[i]);
                右现有牌的列表.Add(右边游戏牌列表[i]);

            }
            我现有牌的列表.Add(我的牌列表[i]);
        }
        #endregion
        #region 将胡牌放到回放摸牌位置

        我的胡牌.transform.SetParent(我的牌列表[13].transform.parent);
        左边的胡牌.transform.SetParent(左边使用的牌列表[13].transform.parent);
        上边的胡牌.transform.SetParent(上边使用的牌列表[13].transform.parent);
        右边的胡牌.transform.SetParent(右边使用的牌列表[13].transform.parent);
        右边的胡牌.transform.SetAsFirstSibling();

        我的胡牌.GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[13].GetComponent<RectTransform>().anchoredPosition.x, 我的牌列表[13].GetComponent<RectTransform>().anchoredPosition.y - 38);
        左边的胡牌.GetComponent<RectTransform>().anchoredPosition = 左边使用的牌列表[13].GetComponent<RectTransform>().anchoredPosition;

        if (!isinreview)
        {
            上边的胡牌.GetComponent<RectTransform>().anchoredPosition = new Vector2(上边使用的牌列表[13].GetComponent<RectTransform>().anchoredPosition.x, 上边使用的牌列表[13].GetComponent<RectTransform>().anchoredPosition.y - 26);
            右边的胡牌.GetComponent<RectTransform>().anchoredPosition = new Vector2(右边使用的牌列表[13].GetComponent<RectTransform>().anchoredPosition.x - 5, 右边使用的牌列表[13].GetComponent<RectTransform>().anchoredPosition.y);
        }
        else
        {

            上边的胡牌.GetComponent<RectTransform>().anchoredPosition = 上边使用的牌列表[13].GetComponent<RectTransform>().anchoredPosition;
            右边的胡牌.GetComponent<RectTransform>().anchoredPosition = 右边使用的牌列表[13].GetComponent<RectTransform>().anchoredPosition;
        }
        #endregion
    }



    void reciveZhuang(uint charid)
    {
        ZhuangJiaid = GameManager.GM.GetPlayerNum(charid);
    }
    void ShowZLastCard(List<uint> Cards, int seat)
    {
        switch (ZhuangJiaid)
        {
            case 0:
                我的牌列表[13].SetActive(true);
                break;
            case 1:
                左边使用的牌列表[13].SetActive(true);
                break;
            case 2:
                上边使用的牌列表[13].SetActive(true);
                break;
            case 3:
                右边使用的牌列表[13].SetActive(true);
                break;
            default:
                break;
        }
    }




    void KeyiH3Z()
    {
        inH3z = true;
        MyMjAllDown();//所有牌落下 



        StartCoroutine(AudioH3z());

    }
    IEnumerator AudioH3z()
    {
        Debug.Log("CardInMyHand.c2   " + CardInMyHand.Count);

        yield return null;
        for (int i = 0; i < CardInMyHand.Count; i++)
        {
            if (CardInMyHand[i] < 10)
            {//0-8
                w += 1;
            }
            else if (CardInMyHand[i] > 16 && CardInMyHand[i] < 27)
            {
                //9-17
                t += 1;
            }
            else if (CardInMyHand[i] > 32 && CardInMyHand[i] < 43)
            {
                //18-26
                b += 1;
            }
            else if (CardInMyHand[i] > 43)
            {
                EnableMjMask(我现有牌的列表[i]);
            }
        }
        if (0 < w && w < 3)
        {
            List<GameObject> x = new List<GameObject>();
            for (int i = 0; i < w; i++)
            {
                x.Add(我现有牌的列表[i]);
            }
            EnableMjMask(x);
        }
        if (0 < t && t < 3)
        {
            List<GameObject> x = new List<GameObject>();
            for (int i = 0; i < t; i++)
            {
                x.Add(我现有牌的列表[w + i]);
            }
            EnableMjMask(x);
        }
        if (0 < b && b < 3)
        {
            List<GameObject> x = new List<GameObject>();
            for (int i = 0; i < b; i++)
            {
                x.Add(我现有牌的列表[t + w + i]);
            }
            EnableMjMask(x);
        }
    }
    void DecideH3z()
    {
        //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 1, 2);

        inH3z = false;
        List<uint> h3zc = new List<uint>();

        for (int i = 0; i < 我现有牌的列表.Count; i++)
        {
            H3zHandCard.Add(我现有牌的列表[i]);
        }
        h3zc.Add(CardInMyHand[(int)H3zCards[0]]);
        h3zc.Add(CardInMyHand[(int)H3zCards[1]]);
        h3zc.Add(CardInMyHand[(int)H3zCards[2]]);
        uint val = CardInMyHand[(int)H3zCards[0]];
        uint val1 = CardInMyHand[(int)H3zCards[1]];
        uint val2 = CardInMyHand[(int)H3zCards[2]];

        CardInMyHand.Remove(val);
        CardInMyHand.Remove(val1);
        CardInMyHand.Remove(val2);
        我现有牌的列表[0].SetActive(false);
        我现有牌的列表.RemoveAt(0);
        我现有牌的列表[0].SetActive(false);
        我现有牌的列表.RemoveAt(0);
        我现有牌的列表[0].SetActive(false);
        我现有牌的列表.RemoveAt(0);
        UpdateCardValue(CardInMyHand, 0);
        //向左移动120
        MovePos(0, -120);
        MyMjAllDown();
        DisableMjMask(H3zHandCard);


        PublicEvent.GetINS.SentChange3Zhang(h3zc);

    }

    void ReciveH3z(List<uint> h3zcards)
    {

        //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 1, 2);

        MovePos(1, -30);
        MovePos(2, 66);
        MovePos(3, 30);

        左现有牌的列表[0].SetActive(false);
        左现有牌的列表[1].SetActive(false);
        左现有牌的列表[2].SetActive(false);


        右现有牌的列表[0].SetActive(false);
        右现有牌的列表[1].SetActive(false);
        右现有牌的列表[2].SetActive(false);

        上现有牌的列表[0].SetActive(false);
        上现有牌的列表[1].SetActive(false);
        上现有牌的列表[2].SetActive(false);

        StartCoroutine(ReciveH3z_(h3zcards));
    }
    IEnumerator ReciveH3z_(List<uint> h3zcards)
    {
        yield return new WaitForSeconds(1f);
        //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 2, 2);

        //向左移动120
        MovePos(0, 120);
        Debug.Log(" " + h3zcards[0] + "  " + h3zcards[1] + "  " + h3zcards[2]);
        CardInMyHand.Add(h3zcards[0]);
        CardInMyHand.Add(h3zcards[1]);
        CardInMyHand.Add(h3zcards[2]);

        H3zHandCard[0].SetActive(true);
        H3zHandCard[1].SetActive(true);
        H3zHandCard[2].SetActive(true);
        我现有牌的列表.Clear();
        for (int i = 0; i < H3zHandCard.Count; i++)
        {
            我现有牌的列表.Add(H3zHandCard[i]);
        }
        UpdateCardValue(CardInMyHand, 0);


        DisableMjMask(我的牌列表);

        MovePos(1, 30);
        MovePos(2, -66);
        MovePos(3, -30);

        左现有牌的列表[0].SetActive(true);
        左现有牌的列表[1].SetActive(true);
        左现有牌的列表[2].SetActive(true);


        右现有牌的列表[0].SetActive(true);
        右现有牌的列表[1].SetActive(true);
        右现有牌的列表[2].SetActive(true);

        上现有牌的列表[0].SetActive(true);
        上现有牌的列表[1].SetActive(true);
        上现有牌的列表[2].SetActive(true);

        MyMjAllDown();
        int[] vall = new int[3];
        vall[0] = CardInMyHand.IndexOf(h3zcards[0]);
        if (vall[0] == CardInMyHand.IndexOf(h3zcards[1]))
        {
            vall[1] = vall[0] + 1;
        }
        else
            vall[1] = CardInMyHand.IndexOf(h3zcards[1]);

        if (vall[1] == CardInMyHand.IndexOf(h3zcards[2]))
        {
            vall[2] = vall[1] + 1;
        }
        else
            vall[2] = CardInMyHand.IndexOf(h3zcards[2]);

        我现有牌的列表[vall[0]].GetComponent<RectTransform>().anchoredPosition = new Vector2(我现有牌的列表[vall[0]].GetComponent<RectTransform>().anchoredPosition.x, 190);//-46
        我现有牌的列表[vall[1]].GetComponent<RectTransform>().anchoredPosition = new Vector2(我现有牌的列表[vall[1]].GetComponent<RectTransform>().anchoredPosition.x, 190);//-46
        我现有牌的列表[vall[2]].GetComponent<RectTransform>().anchoredPosition = new Vector2(我现有牌的列表[vall[2]].GetComponent<RectTransform>().anchoredPosition.x, 190);//-46
        yield return new WaitForSeconds(1f);
        我现有牌的列表[vall[0]].GetComponent<RectTransform>().DOAnchorPosY(128, 0.2f);
        我现有牌的列表[vall[1]].GetComponent<RectTransform>().DOAnchorPosY(128, 0.2f);
        我现有牌的列表[vall[2]].GetComponent<RectTransform>().DOAnchorPosY(128, 0.2f);

    }

    void KeyiXQ()
    {

    }

    void ReciveXQ(uint charid, uint type)
    {
        if (GameManager.GM.GetPlayerNum(charid) == 0)
        {
            if (type == 1)
                Que = 0;
            if (type == 2)
                Que = 2;
            if (type == 3)
                Que = 1;
            UpdateCardValue(CardInMyHand, 0);
            MyMjAllDown();
            if (ZhuangJiaid != 0)
            {
                DisableMjMask(我的牌列表);
            }
        }

    }
    int SearchQuenum(List<uint> Cards)
    {

        int num = 0;
        int min = 0;
        int max = 0;
        switch (Que)
        {
            case 0:
                min = 0;
                max = 10;
                break;
            case 1:
                min = 16;
                max = 27;
                break;
            case 2:
                min = 32;
                max = 43;
                break;
            default:
                return 0;
        }
        for (int i = 0; i < Cards.Count; i++)
        {
            if (Cards[i] > min && Cards[i] < max)
            {
                num++;
            }
        }
        return num;
    }
    int[] SearchQue(List<uint> Cards)
    {
        int[] vals = new int[2] { 0, -1 };
        List<uint> que = new List<uint>();
        //Cards.Sort();
        int min = 0;
        int max = 0;
        switch (Que)
        {
            case 0:
                min = 0;
                max = 10;
                break;
            case 1:
                min = 16;
                max = 27;
                break;
            case 2:
                min = 32;
                max = 43;
                break;
            default:
                return null;
        }
        for (int i = 0; i < Cards.Count; i++)
        {
            uint val = Cards[i];
            if (Cards[i] > min && Cards[i] < max)
            {
                que.Add(val);
                Cards.Remove(val);
                i--;
            }
        }
        if (que.Count > 0)
        {
            for (int i = 0; i < que.Count; i++)
            {
                Cards.Add(que[i]);
            }
            /*quenum*/
            vals[0] = que.Count;
            /*questart*/
            vals[1] = Cards.IndexOf(que[0]);
        }
        return vals;
    }







    void Sheichupai(uint Charid)
    {

        if (GameManager.GM.GetPlayerNum(Charid) == 0/*&& !FirstChupai*/)
        {
            KeyiChupai = true;
        }
        else
        {
            KeyiChupai = false;
        }
    }
    /// <summary>
    /// 哪边摸牌和打时摸牌UI的关闭
    /// </summary>
    /// <param name="charid">玩家charid</param>
    /// <param name="Dachu">true:摸牌调用 false:打牌调用</param>
    /// <param name="CardId">当本地摸牌传入摸牌值 其他均传入0即可</param>
    void ReciveMoPai(uint Charid, uint card)
    {
        WaitPopCard = null;
        if (WaitGangCard != 999)
        {
            UpdateGangPai(WaitGangCharid, WaitGangCard, WaitGangOricharid);
            WaitGangCharid = 999;
            WaitGangCard = 999;
            WaitGangOricharid = 999;
        }
        UpdateLastCard(Charid, card);
    }
    void UpdateLastCard(uint charid, uint CardId, bool Dachu = false)
    {
        int seat = GameManager.GM.GetPlayerNum(charid);
        Debug.Log(charid + "   " + seat + "    " + CardId + "     " + Dachu);
        switch (seat)
        {
            case 0:

                if (!Dachu)
                {
                    //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 2, 2);
                    //哇 我摸了一张新牌！
                    if (CardId != 0)
                    {
                        DisableMask(我的牌列表[13]);
                        UpdateCardValue(CardInMyHand, 0);
                        CardInMyHand.Add(CardId);
                        我的牌列表[13].transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(CardId.ToCard().ToName());//***
                        if (Que != -1)
                        {
                            DisableMjMask(我的牌列表[13]);
                            bool isque = false;
                            switch (Que)
                            {
                                case 0:
                                    if (CardId < 10)
                                    {
                                        isque = true;
                                    }
                                    break;
                                case 1:
                                    if (CardId > 16 && CardId < 27)
                                    {
                                        isque = true;
                                    }
                                    break;
                                case 2:
                                    if (CardId > 32 && CardId < 43)
                                    {
                                        isque = true;
                                    }
                                    break;
                                default:
                                    break;
                            }

                            if (isque)
                            {
                                if (SearchQuenum(CardInMyHand) == 1)
                                {
                                    for (int i = 0; i < 我的牌列表.Count - 1; i++)
                                    {
                                        EnableMjMask(我的牌列表[i]);
                                    }
                                }
                            }
                            else
                            {
                                //*
                                if (SearchQuenum(CardInMyHand) > 0)
                                {
                                    EnableMjMask(我的牌列表[13]);
                                }
                            }

                        }
                    }
                    我的牌列表[13].SetActive(true);
                }
                else
                {
                    我的牌列表[13].SetActive(false);
                }


                break;
            case 1:
                if (!Dachu)
                {

                    if (CardId != 0 && IsInReview)
                    {
                        左边使用的牌列表[13].transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(CardId.ToCard().ToName());//***
                        CardInLeft.Add(CardId);
                    }
                    左边使用的牌列表[13].SetActive(true);
                }
                else
                    左边使用的牌列表[13].SetActive(false);
                break;
            case 2:
                if (!Dachu)
                {
                    if (CardId != 0 && IsInReview)
                    {
                        上边使用的牌列表[13].transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(CardId.ToCard().ToName());//***
                        CardInUp.Add(CardId);
                    }
                    上边使用的牌列表[13].SetActive(true);
                }
                else
                    上边使用的牌列表[13].SetActive(false);
                break;
            case 3:
                if (!Dachu)
                {
                    if (CardId != 0 && IsInReview)
                    {
                        右边使用的牌列表[13].transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(CardId.ToCard().ToName());
                        CardInRight.Add(CardId);
                    }
                    右边使用的牌列表[13].SetActive(true);
                }
                else
                    右边使用的牌列表[13].SetActive(false);
                break;
            default:
                break;

        }
    }




    //碰杠胡委托总接口

    void RecivePeng(uint charid, uint card, uint Oricharid)
    {
        if (WaitPopCard != null)
        {
            DelatePopCard();
        }
        UpdatePengPai(charid, card);
    }
    void ReciveGang(uint charid, uint card, uint oricharid)
    {
        if (WaitPopCard != null)
        {
            DelatePopCard();
        }
        WaitGangCard = card;  //等待杠的牌
        WaitGangCharid = charid;
        WaitGangOricharid = oricharid;

    }
    void ReciveHu(uint charid, uint card, uint oricharid)
    {
        if (WaitPopCard != null)
        {
            DelatePopCard();
        }
        if (WaitGangCard != 999)
        {
            if (oricharid == WaitGangCharid && card == WaitGangCard)
            {
                BeiQiangGang(WaitGangCharid, WaitGangCard);
            }
            else
            {
                UpdateGangPai(WaitGangCharid, WaitGangCard, WaitGangOricharid);
            }
            WaitGangCharid = 999;
            WaitGangCard = 999;
            WaitGangOricharid = 999;
        }
        UpdateHule(charid, card, oricharid);
    }
    void ReciveGuo(uint charid)
    {
        if ((CardInMyHand.Count - 2) % 3 == 0 && GameManager.GM.GetPlayerNum(charid) == 0)
        {
            KeyiChupai = true;
        }
    }



    /// <summary>
    /// 哪边出牌了 
    /// </summary>
    /// <param name="charid">玩家charid</param>
    /// <param name="CardId">碰牌传入牌值</param>
    void UpdateChupai(uint charid, uint CardId)
    {
        Debug.Log(charid + " Local出了" + CardId);
        int seat = GameManager.GM.GetPlayerNum(charid);
        UpdateLastCard(charid, CardId, true);//谁出牌了将他的摸牌物体关掉显示

        绿箭头.SetActive(false);
        StopCoroutine("Jump");

        switch (seat)
        {
            case 0:
                if (PengHouChuPai)
                {
                    PengHouChuPai = false;
                    if (IsInReview)
                    {
                        //移除手牌
                        我现有牌的列表[0].SetActive(false);
                        我现有牌的列表.RemoveAt(0);
                        //向左移动120
                        MovePos(0, -120);
                    }
                }
                if (IsInReview)
                {
                    CardInMyHand.Remove(CardId);
                    UpdateCardValue(CardInMyHand, 0);
                }
                StartCoroutine(ChuCard(CardId, charid, 我的出牌动画));
                break;
            case 1:
                if (PengHouChuPai)
                {
                    PengHouChuPai = false;
                    左现有牌的列表[0].SetActive(false);
                    左现有牌的列表.RemoveAt(0);
                }
                if (IsInReview)
                {
                    CardInLeft.Remove(CardId);
                    UpdateCardValue(CardInLeft, 1);
                }
                StartCoroutine(ChuCard(CardId, charid, 左边的出牌动画));
                break;
            case 2:
                if (PengHouChuPai)
                {
                    PengHouChuPai = false;
                    上现有牌的列表[0].SetActive(false);
                    上现有牌的列表.RemoveAt(0);

                    if (!IsInReview)//不是回放
                        MovePos(2, 66);
                    //右移动
                }
                if (IsInReview)
                {
                    CardInUp.Remove(CardId);
                    UpdateCardValue(CardInUp, 2);
                }
                //上边的玩家出牌
                StartCoroutine(ChuCard(CardId, charid, 上边的出牌动画));
                break;
            case 3:
                if (PengHouChuPai)
                {
                    PengHouChuPai = false;
                    右现有牌的列表[0].SetActive(false);
                    右现有牌的列表.RemoveAt(0);
                }
                if (IsInReview)
                {
                    CardInRight.Remove(CardId);
                    UpdateCardValue(CardInRight, 3);
                }
                //右边的玩家出牌
                StartCoroutine(ChuCard(CardId, charid, 右边的出牌动画));
                break;
            default:
                break;
        }

        PlayChuPaiAudios(seat, CardId);
    }
    WaitForSeconds seconds = new WaitForSeconds(0.5f);
    IEnumerator ChuCard(uint cardID, uint Charid, GameObject Card)
    {
        Debug.Log(GameManager.GM.GetPlayerNum(Charid) + "玩家出牌了");
        Card.SetActive(true);
        Card.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(cardID.ToCard().ToName());//***
        yield return seconds;

        Card.SetActive(false);

        UpdateLuoPai(Charid, cardID);
    }

    void PlayChuPaiAudios(int seat, uint card)
    {
        int card_ = -1;

        if (card < 10)
        {//0-8
            card_ = (int)card - 1;
        }
        else if (card > 16 && card < 27)
        {
            //9-17
            card_ = (int)card - 8;
        }
        else if (card > 32 && card < 43)
        {
            //18-26
            card_ = (int)card - 15;
        }
        else if (card > 48)
        {
            //27-33
            //card_ = (int)card - 22;
            card_ = (int)card - 26;

        }
        Debug.Log(card_ + "  !");
        if (card_ != -1)
        {
            if (GameManager.GM._AllPlayerData[seat].sex == 1)
            {
                //男
                //GameManager.GM.AudioGM.PlayAudio("Man/Card", card_, 4);
            }
            else
            {
                //女
                //GameManager.GM.AudioGM.PlayAudio("WoMan/Card", card_, 4);
            }
        }
    }

    /// <summary>
    /// 哪边落下牌了 
    /// </summary>
    /// <param name="charid">玩家charid</param>
    /// <param name="CardId">碰牌传入牌值</param>
    void UpdateLuoPai(uint charid, uint CardId)
    {
        //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 1, 2);
        int seat = GameManager.GM.GetPlayerNum(charid);
        switch (seat)
        {
            case 0:
                HaveANewWaste(CardId, SearchWasteIdle(我的出牌));
                绿箭头.GetComponent<RectTransform>().anchoredPosition = new Vector2(26, 98);
                break;
            case 1:
                HaveANewWaste(CardId, SearchWasteIdle(左边的出牌));
                绿箭头.GetComponent<RectTransform>().anchoredPosition = new Vector2(35, 64);
                break;
            case 2:
                HaveANewWaste(CardId, SearchWasteIdle(上边的出牌));
                绿箭头.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, 92);
                break;
            case 3:
                HaveANewWaste(CardId, SearchWasteIdle(右边的出牌));
                //if (!IsInReview)
                绿箭头.GetComponent<RectTransform>().anchoredPosition = new Vector2(35, 64);
                break;

            default:
                break;
        }
    }
    //哇 我已经是张废牌了！
    void HaveANewWaste(uint Cardid, GameObject Waste)
    {
        if (Waste == null)
        {
            Debug.Log("额 我感觉已经是个废人了");
            return;
        }
        Waste.transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(Cardid.ToCard().ToName());//***
        Transform tr = Waste.transform;
        Waste.SetActive(true);
        if (!绿箭头.activeSelf)
        {
            绿箭头.SetActive(true);
            绿箭头.transform.SetParent(tr);
            StartCoroutine("Jump");
        }
        else
        {
            StopCoroutine("Jump");
            绿箭头.transform.SetParent(tr);
            StartCoroutine("Jump");
        }
    }

    //找一个可用的废弃牌预制
    GameObject SearchWasteIdle(List<GameObject> Wastelist)
    {
        for (int i = 0; i < Wastelist.Count; i++)
        {
            if (!Wastelist[i].activeSelf)
            {
                WaitPopCard = Wastelist[i];

                return Wastelist[i];
            }
        }
        Debug.Log("落牌出错了");
        绿箭头.SetActive(false);
        return null;
    }

    bool isjumping = false;
    IEnumerator Jump()
    {

        RectTransform vatt = 绿箭头.GetComponent<RectTransform>();
        float up = 0;
        bool isDown = true;
        var pos = new Vector2(0, 0);
        while (true)
        {
            yield return null;
            yield return null;
            yield return null;
            if (isDown)
            {
                up++;
                if (up > 10)
                {
                    isDown = !isDown;
                }
                pos.x = vatt.anchoredPosition.x;
                pos.y = vatt.anchoredPosition.y + 1;
            }
            else
            {
                up--;
                if (up < 1)
                {
                    isDown = !isDown;
                }
                pos.x = vatt.anchoredPosition.x;
                pos.y = vatt.anchoredPosition.y - 1;
            }
            vatt.anchoredPosition = pos;
        }

    }
    void DelatePopCard()
    {
        WaitPopCard.SetActive(false);
        绿箭头.SetActive(false);
        WaitPopCard = null;
    }


    /// <summary>
    /// 哪边碰了牌 
    /// </summary>
    /// <param name="charid">玩家charid</param>
    /// <param name="CardId">碰牌传入牌值</param>
    /// 
    void UpdatePengPai(uint charid, uint CardId)
    {
        int seat = GameManager.GM.GetPlayerNum(charid);
        Debug.Log("玩家" + seat + "碰了牌");

        PengHouChuPai = true;

        if (!IsInRepick)
        {
            PlayPengPaiAudios(seat);
            SoundMag.GetINS.PlayPopCard("peng", GameManager.GM.GetPlayerSex(charid), GameManager.GM.GetPlayerNum(charid));
            ShowFace.Ins.PlayAnim(Face.peng, GameManager.GM.GetPlayerNum(charid));
        }

        switch (seat)
        {
            case 0:
                MyMjAllDown();

                //-1.播放碰动画
                if (!IsInRepick)
                {
                    //Frameanimation.Instance.PlayAnimation("mj0bq", @"\$BQ96", 1, 4);
                }
                //0.更新显示
                HaveANewPeng(CardId, SearchPengIdle(我的碰杠), GetPrefabSprite("3__4"));//***

                //1.存入碰杠数据List
                MyPeng.Add(CardId);
                MyPengGang.Add(CardId);

                //2.从手牌数据List中一样的数据删除
                DelateCardid(CardInMyHand, CardId);

                //3.关闭多余UI显示
                我现有牌的列表[0].SetActive(false);
                我现有牌的列表.RemoveAt(0);
                我现有牌的列表[0].SetActive(false);
                我现有牌的列表.RemoveAt(0);
                //4.更新手牌面信息
                UpdateCardValue(CardInMyHand, 0);

                break;
            case 1:

                if (!IsInRepick)
                {
                }

                HaveANewPeng(CardId, SearchPengIdle(左边的碰杠), GetPrefabSprite("0__1"));//***


                if (LeftPengGang.Count > 0) MovePos(1, -30);

                LeftPeng.Add(CardId);
                LeftPengGang.Add(CardId);

                //*如果是回放 删除牌信息
                if (IsInReview)
                    DelateCardid(CardInLeft, CardId);
                //3.关闭多余UI显示
                左现有牌的列表[0].SetActive(false);
                左现有牌的列表.RemoveAt(0);
                左现有牌的列表[0].SetActive(false);
                左现有牌的列表.RemoveAt(0);
                //4.*如果是回放 更新手牌面信息
                if (IsInReview)
                    UpdateCardValue(CardInLeft, 1);
                break;
            case 2:

                if (!IsInRepick)
                {
                    //Frameanimation.Instance.PlayAnimation("mj2bq", @"\$BQ96", 1, 4);
                }

                HaveANewPeng(CardId, SearchPengIdle(上边的碰杠), GetPrefabSprite("3__4"));//***
                TopPeng.Add(CardId);
                TopPengGang.Add(CardId);

                //*如果是回放 删除牌信息
                if (IsInReview)
                    DelateCardid(CardInUp, CardId);

                //3.关闭多余UI显示
                上现有牌的列表[0].SetActive(false);
                上现有牌的列表.RemoveAt(0);
                上现有牌的列表[0].SetActive(false);
                上现有牌的列表.RemoveAt(0);
                //4.*如果是回放 更新手牌面信息
                if (IsInReview)
                    UpdateCardValue(CardInUp, 2);
                break;
            case 3:

                if (!IsInRepick)
                {
                    //Frameanimation.Instance.PlayAnimation("mj3bq", @"\$BQ96", 1, 4);
                }

                HaveANewPeng(CardId, SearchPengIdle(右边的碰杠), GetPrefabSprite("0__1"));//***

                if (RightPengGang.Count > 0)
                {
                    MovePos(3, 30);
                }
                RightPeng.Add(CardId);
                RightPengGang.Add(CardId);

                //*如果是回放 删除牌信息
                if (IsInReview)
                    DelateCardid(CardInRight, CardId);
                //3.关闭多余UI显示
                右现有牌的列表[0].SetActive(false);
                右现有牌的列表.RemoveAt(0);
                右现有牌的列表[0].SetActive(false);
                右现有牌的列表.RemoveAt(0);
                //4.*如果是回放 更新手牌面信息
                if (IsInReview)
                    UpdateCardValue(CardInRight, 3);
                break;

            default:
                break;
        }
    }

    //哇 碰了一张牌！
    void HaveANewPeng(uint Cardid, GameObject peng, Sprite pengimgae)
    {
        if (peng == null)
        {
            Debug.LogError("萨比");
            return;
        }
        peng.SetActive(true);
        //改所有花色图片
        for (int j = 0; j < 4; j++)
        {
            peng.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(Cardid.ToCard().ToName());//***

            if (j == 3)
            {
                peng.transform.GetChild(j).gameObject.SetActive(false);
            }
            else
            {
                peng.transform.GetChild(j).GetComponent<Image>().sprite = pengimgae;
                peng.transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
            }
        }

    }
    //找一个可用的碰杠预制
    GameObject SearchPengIdle(List<GameObject> pengganglist)
    {

        for (int i = 0; i < pengganglist.Count; i++)
        {
            if (!pengganglist[i].activeSelf)
            {

                return pengganglist[i];
            }
        }
        Debug.Log("碰杠出错了");
        return null;
    }
    void PlayPengPaiAudios(int seat)
    {
        if (GameManager.GM._AllPlayerData[seat].sex == 1)
        {
            //男
            //GameManager.GM.AudioGM.PlayAudio("Man/Oper", 1, 4);
        }
        else
        {
            //女
            //GameManager.GM.AudioGM.PlayAudio("WoMan/Oper", 1, 4);
        }

    }
    List<uint> DelateCardid(List<uint> list, uint Cardid)
    {
        list.Remove(Cardid);
        list.Remove(Cardid);
        return list;
    }


    /// 被他娘抢杠了
    void BeiQiangGang(uint Charid, uint cardID)
    {
        int seat = GameManager.GM.GetPlayerNum(Charid);
        Debug.Log("被抢杠了       /");
        switch (seat)
        {
            case 0:
                if (MyPeng.Contains(cardID))
                {
                    SoundMag.GetINS.PlayPopCard("qianggang", GameManager.GM.GetPlayerSex(Charid), GameManager.GM.GetPlayerNum(Charid));

                    CardInMyHand.Remove(cardID);
                    UpdateCardValue(CardInMyHand, 0);
                    我的牌列表[13].SetActive(false);
                }
                break;
            case 1:
                if (LeftPeng.Contains(cardID))
                {
                    SoundMag.GetINS.PlayPopCard("qianggang", GameManager.GM.GetPlayerSex(Charid), GameManager.GM.GetPlayerNum(Charid));
                    左边使用的牌列表[13].SetActive(false);
                }
                break;
            case 2:
                if (TopPeng.Contains(cardID))
                {
                    SoundMag.GetINS.PlayPopCard("qianggang", GameManager.GM.GetPlayerSex(Charid), GameManager.GM.GetPlayerNum(Charid));
                    上边使用的牌列表[13].SetActive(false);
                }
                break;
            case 3:
                if (RightPeng.Contains(cardID))
                {
                    SoundMag.GetINS.PlayPopCard("qianggang", GameManager.GM.GetPlayerSex(Charid), GameManager.GM.GetPlayerNum(Charid));
                    右边使用的牌列表[13].SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 哪边杠了牌 
    /// </summary>
    /// <param name="charid">玩家charid</param>
    /// <param name="CardId">碰牌传入牌值</param>
    void UpdateGangPai(uint charid, uint CardId, uint Oricharid)
    {
        int seat = GameManager.GM.GetPlayerNum(charid);
        Debug.Log(seat + "号玩家" + charid + "杠牌了" + "Oricharid是" + Oricharid);


        if (!IsInRepick)
        {
            PlayGangPaiAudios(seat);
        }
       
        switch (seat)
        {
            case 0:
                MyMjAllDown();


                if (!IsInRepick)
                {
                    //Frameanimation.Instance.PlayAnimation("mj0bq", @"\$BQ95", 1, 4);
                }

                bool ming = isMing(charid, CardId, MyPeng, Oricharid);
                if (!ming)
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.angang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.xiayu, seat, 3);
                        SoundMag.GetINS.PlayPopCard("xiayu", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }
                else
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.gang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.guafeng, seat, 5);
                        SoundMag.GetINS.PlayPopCard("guafeng", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }


                HaveANewGang(CardId, SearchGangIdle(CardId, MyPengGang, 我的碰杠), GetPrefabSprite("3__4"), GetPrefabSprite("6__7"),//***
                ming);
                #region List数据处理 位置移动 刷新
                //去4个信息

                for (int i = 0; i < CardInMyHand.Count; i++)
                {
                    if (CardId == CardInMyHand[i])
                    {
                        CardInMyHand.RemoveAt(i);
                        --i;
                    }
                }
                if (isinpeng(CardId, MyPeng))
                {
                    Debug.Log("加杠");
                    MyPeng.Remove(CardId);
                    MyGang.Add(CardId);
                }
                else
                {
                    Debug.Log("开杠");
                    MyGang.Add(CardId);
                    MyPengGang.Add(CardId);

                    //去掉3个显示牌
                    我现有牌的列表[0].SetActive(false);
                    我现有牌的列表.RemoveAt(0);
                    我现有牌的列表[0].SetActive(false);
                    我现有牌的列表.RemoveAt(0);
                    我现有牌的列表[0].SetActive(false);
                    我现有牌的列表.RemoveAt(0);
                    //移动
                    MovePos(0, -120);
                    //更新手牌面信息
                }
                UpdateCardValue(CardInMyHand, 0);
                #endregion

                break;
            case 1:
                if (!IsInRepick)
                {
                    //Frameanimation.Instance.PlayAnimation("mj1bq", @"\$BQ95", 1, 4);
                }
                bool ming1 = isMing(charid, CardId, LeftPeng, Oricharid);
                if (!ming1)
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.angang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.xiayu, seat, 3);
                        SoundMag.GetINS.PlayPopCard("xiayu", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }
                else
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.gang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.guafeng, seat, 5);
                        SoundMag.GetINS.PlayPopCard("guafeng", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }
                HaveANewGang(CardId, SearchGangIdle(CardId, LeftPengGang, 左边的碰杠), GetPrefabSprite("0__1"), GetPrefabSprite("7__8"),//***
                ming1);
                #region List数据处理 位置移动

                //**回放
                if (IsInReview)
                {
                    for (int i = 0; i < CardInLeft.Count; i++)
                    {
                        if (CardId == CardInLeft[i])
                        {
                            CardInLeft.RemoveAt(i);
                            --i;
                        }
                    }
                }
                if (isinpeng(CardId, LeftPeng))
                {
                    Debug.Log("加杠");
                    LeftPeng.Remove(CardId);
                }
                else
                {
                    if (!IsInReview)
                    { if (LeftPengGang.Count > 0) MovePos(1, -30); }
                    Debug.Log("开杠");
                    LeftPengGang.Add(CardId);
                    //去掉3个显示牌
                    左现有牌的列表[0].SetActive(false);
                    左现有牌的列表.RemoveAt(0);
                    左现有牌的列表[0].SetActive(false);
                    左现有牌的列表.RemoveAt(0);
                    左现有牌的列表[0].SetActive(false);
                    左现有牌的列表.RemoveAt(0);
                }

                //*回放
                if (IsInReview)
                    UpdateCardValue(CardInLeft, 1);
                #endregion


                break;
            case 2:
                if (!IsInRepick)
                {
                    //Frameanimation.Instance.PlayAnimation("mj2bq", @"\$BQ95", 1, 4);
                }
                bool ming2 = isMing(charid, CardId, TopPeng, Oricharid);
                if (!ming2)
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.angang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.xiayu, seat, 3);
                        SoundMag.GetINS.PlayPopCard("xiayu", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }
                else
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.gang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.guafeng, seat, 5);
                        SoundMag.GetINS.PlayPopCard("guafeng", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }
                HaveANewGang(CardId, SearchGangIdle(CardId, TopPengGang, 上边的碰杠), GetPrefabSprite("3__4"), GetPrefabSprite("6__7"), ming2);//***
                #region List数据处理 位置移动

                //**回放
                if (IsInReview)
                {
                    for (int i = 0; i < CardInUp.Count; i++)
                    {
                        if (CardId == CardInUp[i])
                        {
                            CardInUp.RemoveAt(i);
                            --i;
                        }
                    }
                }

                if (isinpeng(CardId, TopPeng))
                {
                    Debug.Log("加杠");
                    TopPeng.Remove(CardId);
                }
                else
                {
                    Debug.Log("开杠");
                    TopPengGang.Add(CardId);
                    //去掉3个显示牌
                    上现有牌的列表[0].SetActive(false);
                    上现有牌的列表.RemoveAt(0);
                    上现有牌的列表[0].SetActive(false);
                    上现有牌的列表.RemoveAt(0);
                    上现有牌的列表[0].SetActive(false);
                    上现有牌的列表.RemoveAt(0);
                    //移动
                    if (!IsInReview) //回放时候不移动 
                        MovePos(2, 66);

                }
                //*回放
                if (IsInReview)
                    UpdateCardValue(CardInUp, 2);
                #endregion


                break;
            case 3:
                if (!IsInRepick)
                {
                    //Frameanimation.Instance.PlayAnimation("mj3bq", @"\$BQ95", 1, 4);
                }
                bool ming3 = isMing(charid, CardId, RightPeng, Oricharid);
                if (!ming3)
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.angang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.xiayu, seat, 3);
                        SoundMag.GetINS.PlayPopCard("xiayu", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }
                else
                {
                    if (GameManager.GM.GameType == "ga")
                    {
                        ShowFace.Ins.PlayAnim(Face.gang, seat, 1);
                        SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                    else
                    {
                        ShowFace.Ins.PlayAnim(Face.guafeng, seat, 5);
                        SoundMag.GetINS.PlayPopCard("guafeng", GameManager.GM.GetPlayerSex(charid), seat);
                    }
                }
                HaveANewGang(CardId, SearchGangIdle(CardId, RightPengGang, 右边的碰杠), GetPrefabSprite("0__1"), GetPrefabSprite("7__8"), ming3);//***
                #region List数据处理 位置移动

                //**回放
                if (IsInReview)
                {
                    for (int i = 0; i < CardInRight.Count; i++)
                    {
                        if (CardId == CardInRight[i])
                        {
                            CardInRight.RemoveAt(i);
                            --i;
                        }
                    }
                }
                if (isinpeng(CardId, RightPeng))
                {
                    Debug.Log("加杠");
                    RightPeng.Remove(CardId);
                }
                else
                {

                    if (!IsInReview)
                    {
                        if (RightPengGang.Count > 0) MovePos(3, 30);
                    }
                    Debug.Log("开杠");
                    RightPengGang.Add(CardId);
                    //去掉3个显示牌
                    右现有牌的列表[0].SetActive(false);
                    右现有牌的列表.RemoveAt(0);
                    右现有牌的列表[0].SetActive(false);
                    右现有牌的列表.RemoveAt(0);
                    右现有牌的列表[0].SetActive(false);
                    右现有牌的列表.RemoveAt(0);
                }
                //*回放

                if (IsInReview)
                    UpdateCardValue(CardInRight, 3);
                break;

            #endregion


            default:
                break;
        }
    }

    //哇 杠了一张牌！
    void HaveANewGang(uint Carid, GameObject gang, Sprite ming, Sprite an, bool Ming)
    {
        if (gang == null)
        {
            Debug.LogError("萨比");
            return;
        }
        gang.SetActive(true);

        if (!Ming)//暗杠
        {
            //GameManager.GM.AudioGM.PlayAudio("Mj_Common2", 1,3);
            for (int j = 0; j < 4; j++)
            {
                gang.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(Carid.ToCard().ToName());//***

                if (j == 3 && 我的碰杠.Contains(gang))
                {
                    gang.transform.GetChild(j).GetComponent<Image>().sprite = ming;
                    gang.transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    gang.transform.GetChild(j).GetComponent<Image>().sprite = an;
                    gang.transform.GetChild(j).GetChild(0).gameObject.SetActive(false);
                }
                gang.transform.GetChild(j).gameObject.SetActive(true);
            }
        }


        else//明杠
        {
            //GameManager.GM.AudioGM.PlayAudio("Mj_Common2", 0, 3);

            for (int j = 0; j < 4; j++)
            {
                gang.transform.GetChild(j).GetComponent<Image>().sprite = ming;
                gang.transform.GetChild(j).GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(Carid.ToCard().ToName());//***
                gang.transform.GetChild(j).gameObject.SetActive(true);
                gang.transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    //找一个可用的碰杠预制
    GameObject SearchGangIdle(uint cardid, List<uint> PengGangUintList, List<GameObject> pengganglist)
    {
        //之前有碰的 加杠
        for (int i = 0; i < PengGangUintList.Count; i++)
        {
            if (cardid == PengGangUintList[i])
            {
                return pengganglist[i];
            }
        }

        //无碰 找空的使用
        for (int i = 0; i < pengganglist.Count; i++)
        {
            if (!pengganglist[i].activeSelf)
            {

                return pengganglist[i];
            }
        }
        Debug.Log("碰杠出错了");
        return null;
    }

    //判断暗明杠 
    bool isMing(uint Charid, uint Carid, List<uint> Penglist, uint OriCharid)
    {

        if (OriCharid != 0 && OriCharid != Charid)
        {
            //SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(Charid), GameManager.GM.GetPlayerNum(Charid));
            //ShowFace.Ins.PlayAnim(Face.gang, GameManager.GM.GetPlayerNum(Charid));
            return true;
        }
        else
        {
            //判断是否有碰在先
            for (int i = 0; i < Penglist.Count; i++)
            {
                if (Carid == Penglist[i])
                {
                    //SoundMag.GetINS.PlayPopCard("gang", GameManager.GM.GetPlayerSex(Charid), GameManager.GM.GetPlayerNum(Charid));
                    //ShowFace.Ins.PlayAnim(Face.gang, GameManager.GM.GetPlayerNum(Charid));
                    return true;
                }

            }
            //SoundMag.GetINS.PlayPopCard("angang", GameManager.GM.GetPlayerSex(Charid), GameManager.GM.GetPlayerNum(Charid));
            //ShowFace.Ins.PlayAnim(Face.gang, GameManager.GM.GetPlayerNum(Charid));
            return false;
        }

    }

    //List数据规放
    bool isinpeng(uint cardid, List<uint> penglist)
    {
        for (int i = 0; i < penglist.Count; i++)
        {
            if (cardid == penglist[i])
            {
                return true;
            }
        }
        return false;

    }

    void PlayGangPaiAudios(int seat)
    {
        if (GameManager.GM._AllPlayerData[seat].sex == 1)
        {
            //男
            //GameManager.GM.AudioGM.PlayAudio("Man/Oper",2, 4);
        }
        else
        {
            //女
            //GameManager.GM.AudioGM.PlayAudio("WoMan/Oper", 2, 4);
        }

    }


    /// <summary>
    /// 哪边胡了 
    /// </summary>
    /// <param name="charid">玩家charid</param>
    /// <param name="CardId">碰牌传入牌值</param>
    void UpdateHule(uint charid, uint CardId, uint Oricharid)
    {

        int seat = GameManager.GM.GetPlayerNum(charid);

        if (Oricharid != 0 && Oricharid != charid)
        {//点炮
            SoundMag.GetINS.PlayPopCard("hu", GameManager.GM.GetPlayerSex(charid), GameManager.GM.GetPlayerNum(charid));
            //ShowFace.Ins.PlayAnim(Face.hu, GameManager.GM.GetPlayerNum(charid));
            PlayHuPaiAudios(seat, false);
        }
        else
        {//自摸
            SoundMag.GetINS.PlayPopCard("zimo", GameManager.GM.GetPlayerSex(charid), GameManager.GM.GetPlayerNum(charid));
            //ShowFace.Ins.PlayAnim(Face.zimo, GameManager.GM.GetPlayerNum(charid));
            PlayHuPaiAudios(seat, true);
        }
        switch (seat)
        {
            case 0:
                MyMjAllDown();
                if (!IsInRepick)
                {
                    if (Oricharid != 0 && Oricharid != charid)
                    {
                        //胡
                        //frame.PlayAnimation("mj0bq", @"\$BQ98", 1, 4);

                        //找到点炮玩家
                        int OriSeat = GameManager.GM.GetPlayerNum(Oricharid);
                        //frame.PlayAnimation("mj"+ OriSeat + "bq", @"\$BQ97", 1, 4);
                    }
                    else
                    {
                        //自摸
                        //frame.PlayAnimation("mj0bq", @"\$BQ99", 1, 4);
                    }
                }
                HaveAHu(CardId, 我的胡牌);


                我的牌列表[13].SetActive(false);


                break;
            case 1:
                if (!IsInRepick)
                {

                    if (Oricharid != 0 && Oricharid != charid)
                    {
                        //胡
                        //frame.PlayAnimation("mj1bq", @"\$BQ98", 1, 4);

                        //找到点炮玩家
                        int OriSeat = GameManager.GM.GetPlayerNum(Oricharid);
                        //frame.PlayAnimation("mj" + OriSeat + "bq", @"\$BQ97", 1, 4);
                    }
                    else
                    {
                        //自摸
                        //frame.PlayAnimation("mj1bq", @"\$BQ99", 1, 4);

                    }
                }
                HaveAHu(CardId, 左边的胡牌);

                左边使用的牌列表[13].SetActive(false);

                break;
            case 2:
                if (!IsInRepick)
                {

                    if (Oricharid != 0 && Oricharid != charid)
                    {
                        //胡
                        //frame.PlayAnimation("mj2bq", @"\$BQ98", 1, 4);

                        //找到点炮玩家
                        int OriSeat = GameManager.GM.GetPlayerNum(Oricharid);
                        //frame.PlayAnimation("mj" + OriSeat + "bq", @"\$BQ97", 1, 4);
                    }
                    else
                    {
                        //自摸
                        //frame.PlayAnimation("mj2bq", @"\$BQ99", 1, 4);
                    }
                }
                HaveAHu(CardId, 上边的胡牌);
                上边使用的牌列表[13].SetActive(false);


                break;
            case 3:
                if (!IsInRepick)
                {

                    if (Oricharid != 0 && Oricharid != charid)
                    {
                        //胡
                        //frame.PlayAnimation("mj3bq", @"\$BQ98", 1, 4);

                        //找到点炮玩家
                        int OriSeat = GameManager.GM.GetPlayerNum(Oricharid);
                        //frame.PlayAnimation("mj" + OriSeat + "bq", @"\$BQ97", 1, 4);
                    }
                    else
                    {
                        //自摸
                        //frame.PlayAnimation("mj3bq", @"\$BQ99", 1, 4);

                    }
                }
                HaveAHu(CardId, 右边的胡牌);
                右边使用的牌列表[13].SetActive(false);

                break;

            default:
                break;
        }
        //调用 剩余牌列表
        //显示剩余牌数量的button在桌面
        //等待胡的人选牌 
        //跳入结束界面 重置游戏预制Rest
        //完成
    }


    //哇 我家要开银行！
    void HaveAHu(uint Cardid, GameObject Hu)
    {
        if (Hu == null)
        {
            Debug.LogError("小目标一个亿");
            return;
        }
        Hu.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(Cardid.ToCard().ToName());//***

        Hu.SetActive(true);
    }

    void PlayHuPaiAudios(int seat, bool iszimo)
    {
        if (GameManager.GM._AllPlayerData[seat].sex == 1)
        { //男
            if (iszimo)
            {
                //GameManager.GM.AudioGM.PlayAudio("Man/Oper", 4, 4);
            }
            else
            {
                //GameManager.GM.AudioGM.PlayAudio("Man/Oper", 3, 4);
            }


        }
        else
        {
            //女
            if (iszimo)
            {
                //GameManager.GM.AudioGM.PlayAudio("WoMan/Oper", 4, 4);
            }
            else
            {
                //GameManager.GM.AudioGM.PlayAudio("WoMan/Oper", 3, 4);
            }
        }

    }


    void UpdateCardValue(List<uint> Cards, int SEAT)
    {
        Debug.Log("刷新了牌面0...........");
        int questart = -1;
        int quenum = 0;
        if (Que != -1)
        {

            Cards.Sort();
            int[] val = SearchQue(Cards);

            quenum = val[0];
            questart = val[1];
        }
        else
            Cards.Sort();

        All.SetActive(true);

        MyMjAllDown();


        List<GameObject> 有的牌列表 = new List<GameObject>();

        switch (SEAT)
        {

            case 0:
                有的牌列表 = 我现有牌的列表;
                CardInMyHand = Cards;//第一次发牌的时候记录List
                Debug.Log("CardInMyHand.c   " + CardInMyHand.Count);
                break;
            case 1:
                有的牌列表 = 左现有牌的列表;
                CardInLeft = Cards;
                break;
            case 2:
                有的牌列表 = 上现有牌的列表;
                CardInUp = Cards;
                break;
            case 3:
                有的牌列表 = 右现有牌的列表;
                CardInRight = Cards;
                break;
            default:
                break;
        }

        for (int i = 0; i < Cards.Count; i++)
        {
            Debug.Log(Cards[i].ToCard().ToName() + "  !");
            有的牌列表[i].transform.GetChild(0).GetComponent<Image>().sprite = GetPrefabSprite(Cards[i].ToCard().ToName());//***
            if (i == 13)
            {
                有的牌列表[i].SetActive(true);
            }
            if (Que != -1)
            {
                if (quenum > 0)
                {
                    //*
                    if (i >= questart && i < questart + quenum)
                    {
                        //EnableMask(有的牌列表[i]);
                        DisableMjMask(有的牌列表[i]);
                    }
                    else
                    {
                        EnableMjMask(有的牌列表[i]);
                    }
                }
                else
                {
                    // DisableMask(有的牌列表[i]);
                    DisableMjMask(有的牌列表[i]);
                }
            }

        }

    }



    //点击了麻将按钮
    void OnClickMj(int Cardlistnum)
    {
        //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 0,1);

        if (!isdrag)
        {
            if (!inH3z)
            {
                if (Cardlistnum != ShengqiPai)
                {
                    //牌升起来
                    ShengqiPai = Cardlistnum;

                    for (int i = 0; i < 我的牌列表.Count; i++)
                    {
                        我的牌列表[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[i].GetComponent<RectTransform>().anchoredPosition.x, 128);//-462
                    }
                    我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition.x, 190);//-422
                }

                else if (Cardlistnum == ShengqiPai && !KeyiChupai)
                {
                    ////牌升起来后不能打放下
                    ShengqiPai = -1;
                    我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition.x, 128);//-462

                    Debug.Log("打不了憋点了");
                }

                else if (Cardlistnum == ShengqiPai && KeyiChupai)
                {
                    KeyiChupai = false;

                    //打掉了
                    Debug.Log("老子走了");
                    ShengqiPai = -1;

                    for (int i = 0; i < 我的牌列表.Count; i++)
                    {
                        我的牌列表[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[i].GetComponent<RectTransform>().anchoredPosition.x, 128);//-462
                    }
                    if (Cardlistnum == 我的牌列表.Count - 1)
                    {
                        我的牌列表[Cardlistnum].SetActive(false);
                    }

                    Debug.Log(GetCardId(我的牌列表[Cardlistnum]));

                    PublicEvent.GetINS.Fun_SentPopCard(GetCardId(我的牌列表[Cardlistnum]));
                    CardInMyHand.Remove(GetCardId(我的牌列表[Cardlistnum]));

                    if (PengHouChuPai)
                    {//碰后打手牌
                        PengHouChuPai = false;
                        //移除手牌
                        我现有牌的列表[0].SetActive(false);
                        我现有牌的列表.RemoveAt(0);
                        //向左移动120
                        MovePos(0, -120);
                    }
                    UpdateCardValue(CardInMyHand, 0);
                    DisableMjMask(我现有牌的列表);
                }
            }
            else
            {
                //***选3张阶段
                #region 换3张点击

                if (!H3zCards.Contains((uint)Cardlistnum))
                {
                    if (CardInMyHand[Cardlistnum] < 10)
                    {//0-8
                        if (H3zType != -1 && H3zType != 0)
                        {
                            DisableMjMask(H3zMask);
                            MyMjAllDown();
                            H3zMask.Clear();
                            H3zCards.Clear();
                            H3zObj.Clear();


                            PublicEvent.GetINS.DisableH3zButton();
                        }
                        H3zType = 0;
                    }
                    else if (CardInMyHand[Cardlistnum] > 16 && CardInMyHand[Cardlistnum] < 27)
                    {
                        //9-17
                        if (H3zType != -1 && H3zType != 1)
                        {
                            DisableMjMask(H3zMask);
                            MyMjAllDown();
                            H3zMask.Clear();
                            H3zCards.Clear();
                            H3zObj.Clear();
                            PublicEvent.GetINS.DisableH3zButton();
                        }
                        H3zType = 1;

                    }
                    else if (CardInMyHand[Cardlistnum] > 32 && CardInMyHand[Cardlistnum] < 43)
                    {
                        //18-26
                        if (H3zType != -1 && H3zType != 2)
                        {
                            DisableMjMask(H3zMask);
                            MyMjAllDown();
                            H3zMask.Clear();
                            H3zCards.Clear();
                            H3zObj.Clear();
                            PublicEvent.GetINS.DisableH3zButton();
                        }
                        H3zType = 2;
                    }

                    H3zCards.Add((uint)Cardlistnum);
                    H3zObj.Add(我的牌列表[Cardlistnum]);
                    我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition.x, 190);//-422
                    Debug.Log("H3zCards.Count: " + H3zCards.Count + "  " + 我现有牌的列表.Count + "  " + w + "  " + t + "  " + b);

                    if (H3zCards.Count == 3)
                    {
                        H3zMask.Clear();
                        switch (H3zType)
                        {
                            case 0:
                                for (int i = 0; i < w; i++)
                                {
                                    H3zMask.Add(我现有牌的列表[i]);
                                }
                                break;
                            case 1:
                                for (int i = 0; i < t; i++)
                                {
                                    H3zMask.Add(我现有牌的列表[i + w]);
                                }
                                break;
                            case 2:
                                for (int i = 0; i < b; i++)
                                {
                                    H3zMask.Add(我现有牌的列表[t + w + i]);
                                }
                                break;
                            default:
                                break;
                        }
                        Debug.Log("H3zType  " + H3zType + "card.count: " + H3zMask.Count);
                        H3zMask.Remove(H3zObj[0]);
                        H3zMask.Remove(H3zObj[1]);
                        H3zMask.Remove(H3zObj[2]);
                        EnableMjMask(H3zMask);
                        PublicEvent.GetINS.EnableH3zButton();
                    }

                }
                else
                {
                    if (H3zCards.Count == 3)
                    {
                        DisableMjMask(H3zMask);
                    }
                    PublicEvent.GetINS.DisableH3zButton();
                    H3zCards.Remove((uint)Cardlistnum);
                    H3zObj.Remove(我的牌列表[Cardlistnum]);
                    我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[Cardlistnum].GetComponent<RectTransform>().anchoredPosition.x, 128);//-462
                }
                #endregion
            }
        }
    }


    //开始拖拽
    bool isdrag = false;
    GameObject INSMJ;
    GameObject DragMjObj;
    void DragMj(BaseEventData bed)
    {

        if (Input.touchCount > 1)
        {
            return;
        }
        else if (Input.touchCount > 0)
        {
            #region xx
            Debug.Log("rua");
            if (!isdrag && !inH3z && KeyiChupai)
            {
                //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 0, 1);

                DragMjObj = bed.selectedObject;
                INSMJ = Instantiate(bed.selectedObject);

                //****
                PublicEvent.GetINS.MoreThanOneTouch += DragEnd;

                //****


                INSMJ.transform.SetParent(bed.selectedObject.transform.parent);
                INSMJ.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Vector3 anv3 = bed.selectedObject.GetComponent<RectTransform>().anchoredPosition3D;
                INSMJ.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(anv3.x/*- bed.selectedObject.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition.x*/, anv3.y, anv3.z);

                if (bed.selectedObject.GetComponent<RectTransform>().anchoredPosition.y == 190 && !inH3z)
                {
                    ShengqiPai = -1;
                    bed.selectedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(bed.selectedObject.GetComponent<RectTransform>().anchoredPosition.x, 128);
                }

                for (int i = 0; i < bed.selectedObject.GetComponentsInChildren<Image>().Length; i++)
                {

                    Color c = bed.selectedObject.GetComponentsInChildren<Image>()[i].color;
                    bed.selectedObject.GetComponentsInChildren<Image>()[i].color = new Color(c.r, c.g, c.b, 0);
                }
                bedPos = bed.selectedObject.transform.parent.gameObject.GetComponent<RectTransform>();
                isdrag = true;
            }
            if (!inH3z && KeyiChupai)
            {
                Vector3 v3 = Camera.main.ScreenToViewportPoint(Input.touches[0].position);
                INSMJ.GetComponent<RectTransform>().anchoredPosition = new Vector2(v3.x * 1920 - bedPos.anchoredPosition.x, v3.y * 1080);
            }
            #endregion

        }
        else
        {
            #region x2
            Debug.Log("rua");
            if (!isdrag && !inH3z && KeyiChupai)
            {
                //GameManager.GM.AudioGM.PlayAudio("Mj_Common1", 0, 1);

                DragMjObj = bed.selectedObject;
                INSMJ = Instantiate(bed.selectedObject);
                PublicEvent.GetINS.MoreThanOneTouch += DragEnd;


                INSMJ.transform.SetParent(bed.selectedObject.transform.parent);
                INSMJ.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Vector3 anv3 = bed.selectedObject.GetComponent<RectTransform>().anchoredPosition3D;
                INSMJ.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(anv3.x, anv3.y, anv3.z);

                if (bed.selectedObject.GetComponent<RectTransform>().anchoredPosition.y == 190)
                {
                    ShengqiPai = -1;
                    bed.selectedObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(bed.selectedObject.GetComponent<RectTransform>().anchoredPosition.x, 128);
                }

                for (int i = 0; i < bed.selectedObject.GetComponentsInChildren<Image>().Length; i++)
                {

                    Color c = bed.selectedObject.GetComponentsInChildren<Image>()[i].color;
                    bed.selectedObject.GetComponentsInChildren<Image>()[i].color = new Color(c.r, c.g, c.b, 0);
                }
                bedPos = bed.selectedObject.transform.parent.gameObject.GetComponent<RectTransform>();
                isdrag = true;
            }
            if (!inH3z && KeyiChupai)
            {
                Vector3 v3 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                INSMJ.GetComponent<RectTransform>().anchoredPosition = new Vector2(v3.x * 1920 - bedPos.anchoredPosition.x, v3.y * 1080);
            }
            #endregion

        }




    }
    RectTransform bedPos;
    //停止拖拽
    void DragEnd()
    {

        if (DragMjObj != null && INSMJ != null && isdrag)
        {

            for (int i = 0; i < DragMjObj.GetComponentsInChildren<Image>().Length; i++)
            {
                Color c = DragMjObj.GetComponentsInChildren<Image>()[i].color;
                DragMjObj.GetComponentsInChildren<Image>()[i].color = new Color(c.r, c.g, c.b, 1);
            }
            DragMjObj.transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 120f / 255f);

            if (INSMJ.GetComponent<RectTransform>().anchoredPosition.y > 300 && KeyiChupai)
            {

                if (DragMjObj == 我的牌列表[我的牌列表.Count - 1])
                {
                    我的牌列表[我的牌列表.Count - 1].SetActive(false);
                }
                Debug.Log(GetCardId(DragMjObj));

                PublicEvent.GetINS.Fun_SentPopCard(GetCardId(DragMjObj));
                CardInMyHand.Remove(GetCardId(DragMjObj));

                if (PengHouChuPai)
                {//碰后打手牌
                    PengHouChuPai = false;
                    //移除手牌
                    我现有牌的列表[0].SetActive(false);
                    我现有牌的列表.RemoveAt(0);

                    //向左移动120
                    MovePos(0, -120);
                }

                UpdateCardValue(CardInMyHand, 0);
                DisableMjMask(我现有牌的列表);

                KeyiChupai = false;
            }

            Destroy(INSMJ.gameObject);
            isdrag = false;
            PublicEvent.GetINS.MoreThanOneTouch -= DragEnd;

        }
    }

    BaseEventData bed;


    //麻将添加点击事件
    void MjAddListion()
    {
        for (int i = 0; i < 我的牌列表.Count; i++)
        {
            int j = i;
            ////添加拖拽事件

            EventTrigger yuyineventTri = 我的牌列表[i].GetComponent<EventTrigger>();
            if (yuyineventTri == null)
                yuyineventTri = 我的牌列表[i].AddComponent<EventTrigger>();

            yuyineventTri.triggers = new List<EventTrigger.Entry>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Drag;
            entry.callback.AddListener(DragMj);
            yuyineventTri.triggers.Add(entry);

            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.EndDrag;
            entry1.callback.AddListener(delegate (BaseEventData bed) { DragEnd(); });
            yuyineventTri.triggers.Add(entry1);


            //添加点击事件
            我的牌列表[i].GetComponent<Button>().onClick.AddListener(delegate () { this.OnClickMj(j); });


        }




    }
    //点击的麻将获得它在手牌麻将数据List中的位置
    uint GetCardId(GameObject mj)
    {
        for (int i = 0; i < 我现有牌的列表.Count; i++)
        {

            if (mj == 我现有牌的列表[i])
            {
                return CardInMyHand[i];
            }
        }

        Debug.LogError("出错了我的哥，点的牌查不到呀");
        return 0;
    }

    void EnableMjMask(List<GameObject> mj)
    {
        Debug.Log("EnableMjMask: " + mj.Count);
        foreach (var item in mj)
        {
            item.transform.GetChild(1).gameObject.SetActive(true);
            Image[] x = item.GetComponentsInChildren<Image>();
            foreach (var item2 in x)
            {
                item2.raycastTarget = false;
            }
        }
    }
    void EnableMjMask(GameObject obj)
    {
        obj.transform.GetChild(1).gameObject.SetActive(true);
        Image[] x = obj.GetComponentsInChildren<Image>();
        foreach (var item2 in x)
        {
            item2.raycastTarget = false;
        }
    }
    void DisableMjMask(List<GameObject> mj)
    {
        foreach (var item in mj)
        {
            item.transform.GetChild(1).gameObject.SetActive(false);
            Image[] x = item.GetComponentsInChildren<Image>();
            foreach (var item2 in x)
            {
                item2.raycastTarget = true;
            }
        }
    }
    void DisableMjMask(GameObject obj)
    {
        obj.transform.GetChild(1).gameObject.SetActive(false);
        Image[] x = obj.GetComponentsInChildren<Image>();
        foreach (var item2 in x)
        {
            item2.raycastTarget = true;
        }
    }

    void EnableMask(List<GameObject> mj)
    {
        Debug.Log("EnableMask: " + mj.Count);
        foreach (var item in mj)
        {
            item.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    void EnableMask(GameObject obj)
    {
        obj.transform.GetChild(1).gameObject.SetActive(true);
    }
    void DisableMask(List<GameObject> mj)
    {
        foreach (var item in mj)
        {
            item.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    void DisableMask(GameObject mj)
    {
        mj.transform.GetChild(1).gameObject.SetActive(false);
    }
    /// <summary>
    /// 位置移动通用方法
    /// </summary>
    /// <param name="id">谁移动 0本地 1左 2上 3右</param>
    /// <param name="value">移动的距离</param>
    void MovePos(int id, float value)
    {
        //id: 0自己 1左边 2上边 3右边  value:移动的数值

        RectTransform Rt;
        switch (id)
        {
            case 0:

                Rt = 我牌的位置.GetComponent<RectTransform>();
                Rt.anchoredPosition = new Vector2(Rt.anchoredPosition.x + value, Rt.anchoredPosition.y);
                break;
            case 1:

                Rt = 左牌的位置.GetComponent<RectTransform>();
                Rt.anchoredPosition = new Vector2(Rt.anchoredPosition.x, Rt.anchoredPosition.y + value);
                break;
            case 2:

                Rt = 上牌的位置.GetComponent<RectTransform>();
                Rt.anchoredPosition = new Vector2(Rt.anchoredPosition.x + value, Rt.anchoredPosition.y);
                break;
            case 3:

                Rt = 右牌的位置.GetComponent<RectTransform>();
                Rt.anchoredPosition = new Vector2(Rt.anchoredPosition.x, Rt.anchoredPosition.y + value);
                break;
            default:
                break;
        }

    }

    void MyMjAllDown()
    {
        ShengqiPai = -1;

        for (int i = 0; i < 我的牌列表.Count; i++)
        {

            我的牌列表[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(我的牌列表[i].GetComponent<RectTransform>().anchoredPosition.x, 128);//-462
        }
    }

    //麻将恢复默认
    void RestMaJiang()
    {
        All.SetActive(false);

        MyMjAllDown();

        //物体重置------------------------------
        //隐藏胡牌
        我的胡牌.SetActive(false);
        左边的胡牌.SetActive(false);
        上边的胡牌.SetActive(false);
        右边的胡牌.SetActive(false);

        //隐藏碰杠
        for (int i = 0; i < 4; i++)
        {
            我的碰杠[i].SetActive(false);
            上边的碰杠[i].SetActive(false);
            左边的碰杠[i].SetActive(false);
            右边的碰杠[i].SetActive(false);
        }
        //隐藏打出去的牌
        for (int i = 0; i < 我的出牌.Count; i++)
        {
            我的出牌[i].SetActive(false);
            左边的出牌[i].SetActive(false);
            上边的出牌[i].SetActive(false);
            右边的出牌[i].SetActive(false);
        }
        //隐藏空中的牌

        for (int i = 0; i < 4; i++)
        {
            我的出牌动画.SetActive(false);
            左边的出牌动画.SetActive(false);
            上边的出牌动画.SetActive(false);
            右边的出牌动画.SetActive(false);
        }
        ////隐藏箭头
        StopCoroutine("Jump");
        绿箭头.SetActive(false);
        //各方本地牌回原位


        我牌的位置.GetComponent<RectTransform>().anchoredPosition = Vect_我牌的位置;
        左牌的位置.GetComponent<RectTransform>().anchoredPosition = Vect_左牌的位置;
        上牌的位置.GetComponent<RectTransform>().anchoredPosition = Vect_上牌的位置;
        右牌的位置.GetComponent<RectTransform>().anchoredPosition = Vect_右牌的位置;

        回放左边位置.GetComponent<RectTransform>().anchoredPosition = Vect_回放左边位置;
        回放上边位置.GetComponent<RectTransform>().anchoredPosition = Vect_回放上边位置;
        回放右边位置.GetComponent<RectTransform>().anchoredPosition = Vect_回放右边位置;


        //显示所有本地牌
        for (int i = 0; i < 我的牌列表.Count - 1; i++)
        {
            我的牌列表[i].SetActive(true);
            左边使用的牌列表[i].SetActive(true);
            上边使用的牌列表[i].SetActive(true);
            右边使用的牌列表[i].SetActive(true);
        }
        //关闭所有本地牌遮罩
        DisableMask(我的牌列表);

        //隐藏摸牌
        我的牌列表[13].SetActive(false);
        左边使用的牌列表[13].SetActive(false);
        上边使用的牌列表[13].SetActive(false);
        右边使用的牌列表[13].SetActive(false);


        //重置所有现有有牌列表

        RestMyHandCardList(false);
        //数据重置-----------------------------------
        //1.列表信息重置
        //本地手牌信息清空
        CardInMyHand.Clear();
        //回放各边手牌信息
        CardInLeft.Clear();
        CardInUp.Clear();
        CardInRight.Clear();

        /// <summary>
        /// 碰的牌要放在这个列表
        /// </summary>
        MyPeng.Clear();
        LeftPeng.Clear();
        TopPeng.Clear();
        RightPeng.Clear();
        /// <summary>
        /// 杠的牌要放在这个列表
        /// </summary>
        MyGang.Clear();


        //碰杠都放里头 主要是用它的顺序
        MyPengGang.Clear();
        LeftPengGang.Clear();
        TopPengGang.Clear();
        RightPengGang.Clear();

        WaitGangCard = 999;  //等待杠的牌
        WaitGangCharid = 999;
        WaitGangOricharid = 999;

        ZhuangJiaid = -1;          //庄家 0是自己

        H3zCards.Clear();
        H3zObj.Clear();
        H3zMask.Clear();
        H3zHandCard.Clear();
        H3zType = -1; //-1 null 0w 1t 2b
        w = 0;
        t = 0;
        b = 0;
        //int PengCharid = -1;           //碰的玩家在AllGameManager中的位置 0位自己
        Que = -1;
        KeyiChupai = false;     //是否可出牌
        ShengqiPai = -1;         //本地升起的牌是哪张
        PengHouChuPai = false;  //true为碰后打牌状态
        isjumping = false;
    }

    float volume;
    //重连恢复麻将
    void ReUpdateMJ(ProtoBuf.MJRoomInfo mjRoom)
    {
        Debug.Log("激活");
        All.SetActive(true);

        //重连的时候之前的信息会再次出现，导致声音重新播放，下面的是之前的做的处理
        //volume =GameManager.GM.AudioGM.GetSoundVolume();
        //GameManager.GM.AudioGM.SetSoundVolume(0);


        IsInRepick = true;
        ProtoBuf.MJGameOP[] xxx = (ProtoBuf.MJGameOP[])Enum.GetValues(typeof(ProtoBuf.MJGameOP));
        int mypos = 0;
        //bool XqEnd = true;
        for (int i = 0; i < mjRoom.charInfos.Count; i++)
        {
            //if (mjRoom.cardsInfos[i].xQue == ProtoBuf.MJCardType.MJ_CARD_TYPE_HUA)
            //{
            //    XqEnd = false;
            //}
            if (GameManager.GM.GetPlayerNum(mjRoom.charInfos[i].charId) == 0)
            {
                mypos = i;
            }
        }
        if (mjRoom.cardsInfos[mypos].xQue != ProtoBuf.MJCardType.MJ_CARD_TYPE_HUA)
        {
            //if (XqEnd)
            //{
                switch (mjRoom.cardsInfos[mypos].xQue)
                {
                    case ProtoBuf.MJCardType.MJ_CARD_TYPE_WAN:
                        Que = 0;
                        break;
                    case ProtoBuf.MJCardType.MJ_CARD_TYPE_TONG:
                        Que = 2;
                        break;
                    case ProtoBuf.MJCardType.MJ_CARD_TYPE_TIAO:
                        Que = 1;
                        break;
                    default:
                        break;
                }
            //}
        }
        #region 更新四方向的牌
        #region 更新已经落下的

        for (int i = 0; i < mjRoom.charIds.Count; i++)
        {
            //更新已经落下的牌
            for (int j = 0; j < mjRoom.cardsInfos[i].passCards.Count; j++)
            {
                if (j < mjRoom.cardsInfos[i].passCards.Count - 1)
                    UpdateLuoPai(mjRoom.charIds[i], mjRoom.cardsInfos[i].passCards[j]);
                else
                {

                    if (mjRoom.roomCache.charList.Count > 0)
                    {

                        if (mjRoom.roomCache.charList[0].oriCharId == mjRoom.charIds[i] && !mjRoom.roomCache.charList[0].opList.Contains(6))
                        {
                            for (int z = 0; z < mjRoom.roomCache.charList.Count; z++)
                            {
                                if (mjRoom.roomCache.charList[z].charId == mjRoom.charIds[i])
                                {

                                    break;
                                }
                            }
                            break;
                        }
                        else
                        {
                            UpdateLuoPai(mjRoom.charIds[i], mjRoom.cardsInfos[i].passCards[j]);
                            break;
                        }
                    }
                    else
                    {
                        UpdateLuoPai(mjRoom.charIds[i], mjRoom.cardsInfos[i].passCards[j]);
                    }
                }
            }
            #endregion
            #region 更新碰牌

            for (int j = 0; j < mjRoom.cardsInfos[i].pengCards.Count; j++)
            {
                UpdatePengPai(mjRoom.charIds[i], mjRoom.cardsInfos[i].pengCards[j]);
                int seat = GameManager.GM.GetPlayerNum(mjRoom.charIds[i]);

                //再移动一位
                switch (GameManager.GM.GetPlayerNum(mjRoom.charIds[i]))
                {
                    case 0:
                        我现有牌的列表[0].SetActive(false);
                        我现有牌的列表.RemoveAt(0);

                        //向左移动120
                        MovePos(0, -120);
                        break;
                    case 1:
                        左现有牌的列表[0].SetActive(false);
                        左现有牌的列表.RemoveAt(0);
                        break;
                    case 2:
                        上现有牌的列表[0].SetActive(false);
                        上现有牌的列表.RemoveAt(0);

                        MovePos(2, 66);
                        break;
                    case 3:
                        右现有牌的列表[0].SetActive(false);
                        右现有牌的列表.RemoveAt(0);
                        break;
                    default:

                        break;
                }
            }

            #endregion

            //更新杠牌   num++
            for (int j = 0; j < mjRoom.cardsInfos[i].mingGangCards.Count; j++)
            {
                UpdateGangPai(mjRoom.charIds[i], mjRoom.cardsInfos[i].mingGangCards[j], 1);
            }
            for (int j = 0; j < mjRoom.cardsInfos[i].anGangCards.Count; j++)
            {
                UpdateGangPai(mjRoom.charIds[i], mjRoom.cardsInfos[i].anGangCards[j], 0);
            }
            //更新胡牌
            for (int j = 0; j < mjRoom.roomCache.huCharIds.Count; j++)
            {
                int seat = GameManager.GM.GetPlayerNum(mjRoom.roomCache.huCharIds[j]);

                int vall = -1;
                for (int k = 0; k < mjRoom.charIds.Count; k++)
                {
                    if (mjRoom.roomCache.huCharIds[j] == mjRoom.charIds[k])
                        vall = k;
                }


                switch (seat)
                {
                    case 0:
                        if (vall != -1)
                            HaveAHu(mjRoom.cardsInfos[vall].huCards[0], 我的胡牌);
                        break;
                    case 1:
                        if (vall != -1)
                            HaveAHu(mjRoom.cardsInfos[vall].huCards[0], 左边的胡牌);
                        break;
                    case 2:
                        if (vall != -1)
                            HaveAHu(mjRoom.cardsInfos[vall].huCards[0], 上边的胡牌);
                        break;
                    case 3:
                        if (vall != -1)
                            HaveAHu(mjRoom.cardsInfos[vall].huCards[0], 右边的胡牌);
                        break;
                    default:
                        break;
                }

            }


            bool 显示摸牌 = false;

            for (int z = 0; z < mjRoom.roomCache.charList.Count; z++)
            {
                if (mjRoom.roomCache.charList[z].charId == mjRoom.charIds[i])
                {
                    int seat = GameManager.GM.GetPlayerNum(mjRoom.roomCache.charList[z].charId);

                    if (mjRoom.roomCache.charList[0].oriCharId == mjRoom.roomCache.charList[z].charId | mjRoom.roomCache.charList[0].oriCharId == 0)
                    {
                        Debug.Log("mjRoom.roomCache.charList[0].oriCharId==mjRoom.roomCache.charList[z].charId 显示牌面   " + mjRoom.roomCache.charList[0].oriCharId + "  估计是可以杠 和出牌" + mjRoom.roomCache.charList[z].charId);
                        显示摸牌 = true;
                    }
                    else
                    {
                        if (mjRoom.roomCache.charList[z].opList.Contains(6) && CardInMyHand.Count == 14)
                        {
                            Debug.Log("可以出牌 14张牌   " + mjRoom.roomCache.charList[0].oriCharId);
                            显示摸牌 = true;
                        }
                        else
                        {
                            Debug.Log("不能出牌   " + mjRoom.roomCache.charList[0].oriCharId + "可能是碰");
                            显示摸牌 = false;
                        }

                    }

                }

            }

            if (GameManager.GM.GetPlayerNum(mjRoom.charIds[i]) == 0)
            {
                if (显示摸牌)
                {
                    uint lastcard = mjRoom.cardsInfos[i].handCards[mjRoom.cardsInfos[i].handCards.Count - 1];
                    mjRoom.cardsInfos[i].handCards.Remove(lastcard);
                    UpdateCardValue(mjRoom.cardsInfos[i].handCards, 0);
                    UpdateLastCard(mjRoom.charIds[i], lastcard);

                    Debug.Log(CardInMyHand.Count);
                }
                else
                {
                    UpdateCardValue(mjRoom.cardsInfos[i].handCards, 0);
                }
            }
        }
        #endregion

        int pengganghu = 0;

        for (int x = 0; x < mjRoom.roomCache.charList.Count; x++)
        {

            if (GameManager.GM.GetPlayerNum(mjRoom.roomCache.charList[x].charId) != -1)
            {
                int seat = GameManager.GM.GetPlayerNum(mjRoom.roomCache.charList[x].charId);
                for (int i = 0; i < mjRoom.roomCache.charList[x].opList.Count; i++)
                {

                    Debug.Log(mjRoom.roomCache.charList.Count + "  " + x + "  " + mjRoom.roomCache.charList[x].opList[i]);
                    #region Swich ENUM
                    Debug.Log(xxx[(int)mjRoom.roomCache.charList[x].opList[i] - 1] + "          ssss");
                    switch (xxx[(int)mjRoom.roomCache.charList[x].opList[i] - 1])
                    {
                        case ProtoBuf.MJGameOP.MJ_OP_MOPAI:
                            //mo
                            UpdateLastCard(mjRoom.roomCache.charList[x].charId, 0);
                            break;
                        case ProtoBuf.MJGameOP.MJ_OP_CHUPAI:
                            if (GameManager.GM.GetPlayerNum(mjRoom.roomCache.charList[x].charId) == 0)
                                KeyiChupai = true;
                            //chu
                            PublicEvent.GetINS.Fun_DirLight(GameManager.GM.GetPlayerNum(mjRoom.roomCache.charList[x].charId));
                            break;
                        case ProtoBuf.MJGameOP.MJ_OP_GUO:
                            //guo
                            break;
                        case ProtoBuf.MJGameOP.MJ_OP_PENG:
                            if (mjRoom.roomCache.charList[x].oriCharId != 0 && mjRoom.roomCache.charList[x].oriCharId != mjRoom.roomCache.charList[x].charId && pengganghu == 0)
                            {
                                UpdateChupai(mjRoom.roomCache.charList[x].oriCharId, mjRoom.roomCache.charList[x].cardList[0]);
                                //清楚打出去的牌
                                pengganghu++;
                            }

                            //碰
                            break;
                        case ProtoBuf.MJGameOP.MJ_OP_GANG:
                            //gang
                            if (mjRoom.roomCache.charList[x].oriCharId != 0 && mjRoom.roomCache.charList[x].oriCharId != mjRoom.roomCache.charList[x].charId && pengganghu == 0)
                            {
                                UpdateChupai(mjRoom.roomCache.charList[x].oriCharId, mjRoom.roomCache.charList[x].cardList[0]);
                                pengganghu++;
                            }
                            break;
                        case ProtoBuf.MJGameOP.MJ_OP_HU:
                            if (mjRoom.roomCache.charList[x].oriCharId != 0 && mjRoom.roomCache.charList[x].oriCharId != mjRoom.roomCache.charList[x].charId && pengganghu == 0)
                            {
                                UpdateChupai(mjRoom.roomCache.charList[x].oriCharId, mjRoom.roomCache.charList[x].cardList[0]);
                                pengganghu++;
                            }

                            break;
                        default:
                            break;
                    }
                    #endregion
                }
            }
        }
        if (!KeyiChupai&&CardInMyHand.Count<14)
        {
            DisableMjMask(我的牌列表);
        }
        #region H3zXQ
        for (int i = 0; i < mjRoom.charInfos.Count; i++)
        {
            if (GameManager.GM.GetPlayerNum(mjRoom.charInfos[i].charId) == 0)
            {

                if (mjRoom.cardsInfos[i].x3zIn.Count != 0 && mjRoom.cardsInfos[i].x3zOut.Count == 0)
                {
                    List<uint> h3zc = new List<uint>();

                    for (int x = 0; x < 我现有牌的列表.Count; x++)
                    {
                        H3zHandCard.Add(我现有牌的列表[x]);
                    }
                    uint val = mjRoom.cardsInfos[i].x3zIn[0];
                    uint val1 = mjRoom.cardsInfos[i].x3zIn[1];
                    uint val2 = mjRoom.cardsInfos[i].x3zIn[2];

                    CardInMyHand.Remove(val);
                    CardInMyHand.Remove(val1);
                    CardInMyHand.Remove(val2);
                    我现有牌的列表[0].SetActive(false);
                    我现有牌的列表.RemoveAt(0);
                    我现有牌的列表[0].SetActive(false);
                    我现有牌的列表.RemoveAt(0);
                    我现有牌的列表[0].SetActive(false);
                    我现有牌的列表.RemoveAt(0);
                    UpdateCardValue(CardInMyHand, 0);
                    //向左移动120
                    MovePos(0, -120);
                    MyMjAllDown();
                    DisableMjMask(H3zHandCard);
                }
            }
        }
        #endregion

        PengHouChuPai = false;

        绿箭头.SetActive(false);

        StartCoroutine("ReUpdateMJVolume");

    }

    IEnumerator ReUpdateMJVolume()
    {
        yield return new WaitForSeconds(3f);

        //this.transform.SetParent(UIManager.Instance.FindBaseUI("PlayroomUI").GetComponent<PlayroomUI_C>().GetTransform("MJUIp"),false);
        IsInRepick = false;
        //GameManager.GM.AudioGM.SetSoundVolume(volume);

    }
    void Awake()
    {
        Reg_In();
        Ins();
    }
    void Start()
    {
        //Ins();
    }
    void OnDestroy()
    {
        Rest();
    }
    void Rest()
    {
        Reg_Out();
    }


    public void Reg_In()
    {
        PublicEvent.GetINS.Event_reciveZhuang += reciveZhuang; //拿到庄记录ID 发牌时显示他最后一张
        PublicEvent.GetINS.Event_KeyiH3z += KeyiH3Z;
        PublicEvent.GetINS.Event_KeyiXQ += KeyiXQ;

        PublicEvent.GetINS.Event_reciveGetFirstCards += UpdateCardValue;

        PublicEvent.GetINS.Event_ReciveChange3ZhangResult += ReciveH3z;
        PublicEvent.GetINS.OnClickH3z = DecideH3z;
        PublicEvent.GetINS.Event_reciveSelectQue += ReciveXQ;

        PublicEvent.GetINS.Event_KeyiChuPai += Sheichupai;

        PublicEvent.GetINS.Event_ReciveOtherPopCard += UpdateChupai;   // 服务器广播
        PublicEvent.GetINS.Event_reciveGetCard += ReciveMoPai;
        PublicEvent.GetINS.Fun_ReciveOtherPeng += RecivePeng;
        PublicEvent.GetINS.Fun_ReciveOtherGang += ReciveGang;
        PublicEvent.GetINS.Fun_ReciveOtherHu += ReciveHu;
        PublicEvent.GetINS.Fun_ReciveOtherGuo += ReciveGuo;
        PublicEvent.GetINS.Event_ReadyToPlayNew += RestMaJiang;
        PublicEvent.GetINS.Event_ReUpdateMj += ReUpdateMJ;
    }

    public void Reg_Out()
    {
        PublicEvent.GetINS.Event_reciveZhuang -= reciveZhuang; //拿到庄记录ID 发牌时显示他最后一张
        PublicEvent.GetINS.Event_KeyiH3z -= KeyiH3Z;
        PublicEvent.GetINS.Event_KeyiXQ -= KeyiXQ;
        PublicEvent.GetINS.Event_reciveGetFirstCards -= UpdateCardValue;
        PublicEvent.GetINS.Event_ReciveChange3ZhangResult -= ReciveH3z;
        PublicEvent.GetINS.OnClickH3z = null;
        PublicEvent.GetINS.Event_reciveSelectQue -= ReciveXQ;
        PublicEvent.GetINS.Event_KeyiChuPai -= Sheichupai;
        PublicEvent.GetINS.Event_ReciveOtherPopCard -= UpdateChupai;   // 服务器广播
        PublicEvent.GetINS.Event_reciveGetCard -= ReciveMoPai;
        PublicEvent.GetINS.Fun_ReciveOtherPeng -= RecivePeng;
        PublicEvent.GetINS.Fun_ReciveOtherGang -= ReciveGang;
        PublicEvent.GetINS.Fun_ReciveOtherHu -= ReciveHu;
        PublicEvent.GetINS.Fun_ReciveOtherGuo -= ReciveGuo;
        PublicEvent.GetINS.Event_ReadyToPlayNew -= RestMaJiang;
        PublicEvent.GetINS.Event_ReUpdateMj -= ReUpdateMJ;
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

}