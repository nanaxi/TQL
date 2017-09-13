using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UI_PlayerInfo : MonoBehaviour {
    Transform ThisTrans = null;
    public Text Name = null;
    public Text ID = null;
    public Text IP = null;
    public Text HomeCards = null;
    public Image HeadSprite = null;
    public Button Close = null;
    public Transform Content = null;
    Tween x;
    void Awake()
    {
        Init();
        //SetInfo();
    }
    // Use this for initialization
    void Start () {    
        Default();
        x = Content.DOLocalMoveX(-1100, 0.6f).From();
        x.SetEase(Ease.OutSine);
        x.SetAutoKill(false);
        x.PlayForward();
    }
    void Init()
    {
        ThisTrans = this.gameObject.transform;
        //Name = ThisTrans.Find("BG/NickName").GetComponent<Text>();
        //ID = ThisTrans.Find("BG/ID").GetComponent<Text>();
        //IP = ThisTrans.Find("BG/IP").GetComponent<Text>();
        //HomeCards = ThisTrans.Find("BG/Cards").GetComponent<Text>();
        //HeadSprite = ThisTrans.Find("BG/Head/Mask/HeadSprite").GetComponent<Image>();
        //Close=ThisTrans.Find("BG2/Close").GetComponent<Button>();
    }
    void Default()
    {       
        Close.onClick.AddListener(Rest);
        
    }
    /// <summary>
    /// 设定角色的 各部分属性
    /// </summary>
    /// <param name="Pic"></param>
    /// <param name="name"></param>
    /// <param name="id"></param>
    /// <param name="ip"></param>
    /// <param name="homecard"></param>
    public void SetInfo(string name="缺省",string id = "缺省", string ip = "缺省", string homecard = "缺省", string Pic = "")
    {
        Name.text = "昵称：" + name;
        ID.text = "ID：" + id;
        IP.text = "IP：" + ip;
        HomeCards.text = "持有房卡数：" + homecard;
        GameManager.GM.GetHead(Pic, SetHead);
    }
    void SetHead(Sprite sprite, int num = 0)
    {
        HeadSprite.sprite = sprite;
        HeadSprite.color = new Color(255, 255, 255, 255);
    }
    void Rest()
    {
        x = Content.DOLocalMoveX(-1100, 0.4f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
    }
    void OnDestory()
    {
        GameManager.GM.DS.PlayerInfo = null;
    }

}
