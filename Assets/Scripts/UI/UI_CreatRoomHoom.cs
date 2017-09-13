using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UI_CreatRoomHoom : MonoBehaviour
{

    Transform ThisTrans = null;
    public Button Close = null;
    public Button ga = null;
    public Button xzdd = null;
    /// <summary>
    /// 龙
    /// </summary>
    public Transform GaLong = null;
    public Transform XZDDLong = null;

    public Transform Content = null;
    GameObject r;
    Tween x;
    Tween GL;
    Tween XL;
    void Awake()
    {
        Init();
    }
    // Use this for initialization
    void Start()
    {
        DeFault();
        x = Content.DOLocalMoveX(-1600, 0.4f).From();
        x.SetEase(Ease.OutSine);
        x.SetAutoKill(false);
        x.PlayForward();

        GL = GaLong.DOLocalMoveY(47, 2f).From();
        GL.SetEase(Ease.InOutSine);
        GL.SetLoops(-1, LoopType.Yoyo);
        GL.SetAutoKill(false);

        XL = XZDDLong.DOLocalMoveY(47, 2f).From();
        XL.SetEase(Ease.InOutSine);
        XL.SetLoops(-1, LoopType.Yoyo);
        XL.SetAutoKill(false);

    }
    void Init()
    {
        ThisTrans = gameObject.transform;
        //Close = ThisTrans.Find("BG2/Close").GetComponent<Button>();
        //ga = ThisTrans.Find("BG/GA").GetComponent<Button>();
        //xzdd = ThisTrans.Find("BG/XZDD").GetComponent<Button>();
        r = ThisTrans.Find("R/r").gameObject;
    }
    void DeFault()
    {
        if (Close != null)
            Close.onClick.AddListener(Rest);
        if (ga != null)
            ga.onClick.AddListener(OpenGA);
        if (xzdd != null)
            xzdd.onClick.AddListener(OpenXZDD);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Rest()
    {
        Destroy(r);
        GameManager.GM.DS.CreateRoom_Hoom = null;
        x = Content.DOLocalMoveX(-1600, 0.3f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
    }
    void OpenGA()
    {
        Destroy(r);
        ParticleManager.GetIns.showBuleYun();
        Invoke("openga", 0);
    }
    void openga()
    {
        x = Content.DOLocalMoveX(-1600, 0.3f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
        GameManager.GM.DS.CreateRoom_Ga = GameManager.GM.PopUI(ResPath.CreateRoom_Ga);

    }
    void OpenXZDD()
    {
        //ParticleManager.GetIns.showRedYun();
        //Invoke("openxzdd", 0);
    }
    void openxzdd()
    {
        Destroy(r);
        x = Content.DOLocalMoveX(-1600, 0.3f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
        GameManager.GM.DS.CreateRoom_Xz = GameManager.GM.PopUI(ResPath.CreateRoom_Xz);
    }
}
