using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Main : MonoBehaviour
{
    Transform ThisTrans = null;

    public Button Head = null;

    public Button Setting = null;
    public Button Rule = null;
    public Button Message = null;
    public Button CheckPoint = null;
    public Button Share = null;
    public Button Store = null;
    public Button ComBat = null;
    public Button Call = null;
    public Button Roll = null;

    public Button JoinRoom = null;
    public Button CreatRoom = null;

    public Transform HeadAround = null;

    public Text Name = null;
    public Text Gold = null;
    public Text RoomCard = null;
    public Image HeadSprite = null;
    public Transform RollText = null;

    public Transform meizi = null;
    void Awake()
    {
        Init();
    }
    Tween Ro;
    // Use this for initialization
    void Start()
    {
        Default();
        //SetInfo("李红衣","666","888","");
        //Invoke("ShowMeizi", 0.0f);
        Ro=RollText.DOLocalMoveX(-1500,20.0f);
        Ro.SetLoops(-1);
        Ro.SetAutoKill(false);
        Ro.PlayForward();
    }
    void Init()
    {
        ThisTrans = this.gameObject.transform;

        HeadAround = ThisTrans.Find("UP/PlayerInformation/round").GetComponent<Transform>();

        Head = ThisTrans.Find("UP/PlayerInformation/Mask/Head").GetComponent<Button>();

        Setting = ThisTrans.Find("UP/Setting").GetComponent<Button>();
        Rule = ThisTrans.Find("UP/Rule").GetComponent<Button>();
        Message = ThisTrans.Find("UP/Message").GetComponent<Button>();

        CheckPoint = ThisTrans.Find("Down/CheckPoint").GetComponent<Button>();
        Share = ThisTrans.Find("Down/Share").GetComponent<Button>();
        Store = ThisTrans.Find("Down/Store").GetComponent<Button>();
        ComBat = ThisTrans.Find("Down/ComBat").GetComponent<Button>();
        Call = ThisTrans.Find("Down/Call").GetComponent<Button>();
        Roll = ThisTrans.Find("Down/Roll").GetComponent<Button>();

        CreatRoom = ThisTrans.Find("Middle/CreatRoom").GetComponent<Button>();
        JoinRoom = ThisTrans.Find("Middle/JoinRoom").GetComponent<Button>();

        Name = ThisTrans.Find("UP/PlayerInformation/ID").GetComponent<Text>();
        Gold = ThisTrans.Find("UP/PlayerInformation/Gold").GetComponent<Text>();
        RoomCard = ThisTrans.Find("UP/PlayerInformation/RoomCard").GetComponent<Text>();
        HeadSprite = ThisTrans.Find("UP/PlayerInformation/Mask/Head").GetComponent<Image>();
    }
    void Default()
    {
        if (Setting != null)
            Setting.onClick.AddListener(delegate
            {
                Debug.Log("打开Setting");
                GameManager.GM.DS.Setting = GameManager.GM.PopUI(ResPath.Setting);
            });
        if (Rule != null)
            Rule.onClick.AddListener(delegate
            {
                Debug.Log("打开Rule");
                GameManager.GM.DS.Rule = GameManager.GM.PopUI(ResPath.Rule);
            });
        if (Message != null)
            Message.onClick.AddListener(delegate
            {
                Debug.Log("打开Message");
                GameManager.GM.DS.Message = GameManager.GM.PopUI(ResPath.Message);
            });


        if (CheckPoint != null)
            CheckPoint.onClick.AddListener(delegate
            {
                Debug.Log("打开CheckPoint");
                GameManager.GM.DS.CheckPoint = GameManager.GM.PopUI(ResPath.CheckPoint);
            });
        if (Share != null)
            Share.onClick.AddListener(delegate
            {
                Debug.Log("打开Share");                
                GameManager.GM.DS.Share = GameManager.GM.PopUI(ResPath.Share);
                //ShowFace.Ins.PlayAnim(Face.guafeng,1, 5);
                //ShowFace.Ins.PlayAnim(Face.xiayu, 3, 3);
            });
        if (Store != null)
            Store.onClick.AddListener(delegate
            {
                //ShowFace.Ins.PlayAnim(Face.gang, 0, 1);
                Debug.Log("打开Store");
                GameManager.GM.DS.Store = GameManager.GM.PopUI(ResPath.Store);
            });
        if (ComBat != null)
            ComBat.onClick.AddListener(delegate
            {
                Debug.Log("打开ComBat");
                if (GameManager.GM.combatGainRsp == null)
                    GameManager.GM.PopUI(ResPath.Notic).GetComponent<UI_Notic>().SetMessage("没有查询到相关战绩记录！");
                else {
                    GameManager.GM.DS.Combat = GameManager.GM.PopUI(ResPath.Combat);
                    GameManager.GM.DS.Combat.GetComponent<UI_ComBat>().SetComBat(GameManager.GM.combatGainRsp);
                }
            });
        if (Call != null)
            Call.onClick.AddListener(delegate
            {
                Debug.Log("打开Call");
                GameManager.GM.DS.Call = GameManager.GM.PopUI(ResPath.Call);
            });
        if (Roll != null)
            Roll.onClick.AddListener(delegate
            {
                Debug.Log("打开Roll");
                GameManager.GM.DS.Roll = GameManager.GM.PopUI(ResPath.Roll);
            });
        if (Head != null)
            Head.onClick.AddListener(delegate
            {
                StartCoroutine("PlayAround");
                Debug.Log("打开PlayerInformation");
                if (GameManager.GM.DS.PlayerInfo != null)
                {
                    Destroy(GameManager.GM.DS.PlayerInfo);
                    GameManager.GM.DS.PlayerInfo = null;
                }
                if (GameManager.GM.DS.PlayerInfo == null)
                {
                    GameManager.GM.DS.PlayerInfo = GameManager.GM.PopUI(ResPath.PlayerInfo);
                    GameManager.GM.DS.PlayerInfo.GetComponent<UI_PlayerInfo>().SetInfo(GameManager.GM._AllPlayerData[0].Name, GameManager.GM._AllPlayerData[0].ID.ToString(), GameManager.GM._AllPlayerData[0].IP.ToString(), GameManager.GM._AllPlayerData[0].Diamond.ToString(), GlobalSettings.avatarUrl);
                }
            });
        if (CreatRoom != null)
            CreatRoom.onClick.AddListener(delegate
            {
                Debug.Log("打开创建房间");
                GameManager.GM.DS.CreateRoom_Hoom = GameManager.GM.PopUI(ResPath.CreateRoom_Hoom);
            });
        if (JoinRoom != null)
        {
            JoinRoom.onClick.AddListener(delegate
            {
                Debug.Log("打开加入房间");
                GameManager.GM.PopUI(ResPath.JoinRoom);
                //GameManager.GM.DS.JoinRoom = 
            });
        }
    }

    IEnumerator PlayAround()
    {
        Tween temp = HeadAround.DORotate(new Vector3(0, 0, -360), 0.8f,RotateMode.FastBeyond360);
        yield return null;
    }
    /// <summary>
    /// 设定角色的 各部分属性
    /// </summary>
    /// <param name="Pic"></param>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <param name="ip"></param>
    /// <param name="homecard"></param>
    public void SetInfo(string name = "缺省", string ID = "缺省", string roomCard = "缺省", string Pic = null)
    {
        Name.text = "昵称：" + name;
        Gold.text = "ID：" + ID;
        RoomCard.text = "房卡：" + roomCard;
        //HeadSprite.sprite = Pic;
        GameManager.GM.GetHead(Pic, SetHead);
        //GameManager.GM.GetHead(Pic, SetHead);
    }

    void SetHead(Sprite sprite, int num = 0)
    {
        HeadSprite.sprite = sprite;
        HeadSprite.color = new Color(255, 255, 255, 255);
    }

    Tween t;
    SkeletonAnimation SwitchAnim;
    bool AnimFinshed = true;
    public void ShowMeizi()
    {
        //废弃
        //meizi.gameObject.SetActive(true);
        //t = meizi.DOLocalMoveX(-1212, 1.0f).From();
        //t.SetAutoKill(false);
        //t.SetEase(Ease.OutElastic);
        if (AnimFinshed)
        {
            AnimFinshed = false;
            SwitchAnim = meizi.GetComponent<SkeletonAnimation>();
            SwitchAnim.AnimationName = "TQT_02";
            Invoke("ReMeizi", 6.0f);
        }

    }/// <summary>
    /// 妹子会到之前的状态
    /// </summary>
    void ReMeizi()
    {
        AnimFinshed=true;
        SwitchAnim.AnimationName = "TQT_01";
    }
}
