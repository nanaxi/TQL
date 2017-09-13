using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UI_Share : MonoBehaviour
{

    Transform ThisTrans = null;
    public Button Close = null;
    public Transform Content = null;
    public Button ToFriend = null;
    public Button ToQuan = null;
    Tween x;
    void Awake()
    {
        Init();
    }
    // Use this for initialization
    void Start()
    {
        DeFault();
        x = Content.DOLocalMoveX(-1285, 0.5f).From();
        x.SetEase(Ease.OutSine);
        x.SetAutoKill(false);
        x.PlayForward();
    }
    void Init()
    {
        ThisTrans = gameObject.transform;
        //Close = ThisTrans.Find("BG2/Close").GetComponent<Button>();
    }
    void DeFault()
    {
        if (Close != null)
            Close.onClick.AddListener(Rest);
        if (ToFriend != null)
            ToFriend.onClick.AddListener(ShareToFriend);
        if (ToQuan != null)
            ToQuan.onClick.AddListener(ShareToQuan);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Rest()
    {
        x = Content.DOLocalMoveX(-1285, 0.5f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
        GameManager.GM.DS.Share = null;
    }
    void ShareToFriend()
    {
        GameManager.GM.Share(0);
    }
    void ShareToQuan()
    {
        GameManager.GM.Share(1);
    }
}
