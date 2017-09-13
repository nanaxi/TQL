using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class UI_CreatRoomXz : MonoBehaviour
{
    Transform ThisTrans = null;
    public Button Close = null;
    public Button Enter = null;
    public Transform Content = null;
  
    public Toggle Ju4, Ju8;
    public Toggle[] fengding=new Toggle[5];
    public Toggle hsz, tiandihu, dingque, mengqingzhongzhnag, yaojiujiangdui, zimojiafan, zimojiadi, dianganghua, diangangpao;
    GameObject r;
    Tween x;
    void Awake()
    {
        Init();
    }
    // Use this for initialization
    void Start()
    {
        DeFault();
        x = Content.DOLocalMoveX(-1800, 0.8f).From();
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
        if (Enter != null)
            Enter.onClick.AddListener(EnterRoom);

        r = ThisTrans.Find("R/r").gameObject;

        PublicEvent.GetINS.Event_joinRoomSuccess += enter;

        Ju4.onValueChanged.AddListener(delegate { ReturnValue[0] = 4; });
        Ju8.onValueChanged.AddListener(delegate { ReturnValue[0] = 8; });

        {
            fengding[0].onValueChanged.AddListener(delegate {
                    ReturnValue[1] = 2;
                    Debug.Log("fengding开启2");
            });
            fengding[1].onValueChanged.AddListener(delegate {
                    ReturnValue[1] = 3;
                    Debug.Log("fengding开启3");
            });
            fengding[2].onValueChanged.AddListener(delegate {
                    ReturnValue[1] = 4;
                    Debug.Log("fengding开启4");
            });
            fengding[3].onValueChanged.AddListener(delegate {
                    ReturnValue[1] = 5;
                    Debug.Log("fengding开启5");
            });
            fengding[4].onValueChanged.AddListener(delegate {
                    ReturnValue[1] = 0;
                    Debug.Log("fengding开启无限");               
            });
        }

        hsz.onValueChanged.AddListener(delegate {
            if (ReturnValue[2] == 1)
            {
                ReturnValue[2] = 0;
            }
            else
            {
                ReturnValue[2] = 1;
                Debug.Log("hsz开启");
            }
        });
        tiandihu.onValueChanged.AddListener(delegate {
            if (ReturnValue[3] == 1)
            {
                ReturnValue[3] = 0;
            }
            else
            {
                ReturnValue[3] = 1;
                Debug.Log("tiandihu开启");
            }
        });
        dingque.onValueChanged.AddListener(delegate {
            if (ReturnValue[4] == 1)
            {
                ReturnValue[4] = 0;
            }
            else
            {
                ReturnValue[4] = 1;
                Debug.Log("dingque开启");
            }
        });
        mengqingzhongzhnag.onValueChanged.AddListener(delegate {
            if (ReturnValue[5] == 1)
            {
                ReturnValue[5] = 0;
            }
            else
            {
                ReturnValue[5] = 1;
                Debug.Log("mengqingzhongzhnag开启");
            }
        });
        yaojiujiangdui.onValueChanged.AddListener(delegate {
            if (ReturnValue[6] == 1)
            {
                ReturnValue[6] = 0;
            }
            else
            {
                ReturnValue[6] = 1;
                Debug.Log("yaojiujiangdui开启");
            }
        });
        zimojiafan.onValueChanged.AddListener(delegate {
            if (ReturnValue[7] == 1)
            {
                ReturnValue[7] = 0;
            }
            else
            {
                ReturnValue[7] = 1;
                Debug.Log("zimojiafan开启");
            }
        });
        zimojiadi.onValueChanged.AddListener(delegate {
            if (ReturnValue[8] == 1)
            {
                ReturnValue[8] = 0;
            }
            else
            {
                ReturnValue[8] = 1;
                Debug.Log("zimojiadi开启");
            }
        });
        dianganghua.onValueChanged.AddListener(delegate {
            if (ReturnValue[9] == 1)
            {
                ReturnValue[9] = 0;
            }
            else
            {
                ReturnValue[9] = 1;
                Debug.Log("dianganghua开启");
            }
        });
        diangangpao.onValueChanged.AddListener(delegate {
            if (ReturnValue[10] == 1)
            {
                ReturnValue[10] = 0;
            }
            else
            {
                ReturnValue[10] = 1;
                Debug.Log("diangangpao开启");
            }
        });

    }
    // Update is called once per frame
    void Update()
    {

    }
    void Rest()
    {
        if (r != null)
            Destroy(r);
        PublicEvent.GetINS.Event_joinRoomSuccess -= enter;
        GameManager.GM.DS.CreateRoom_Hoom = GameManager.GM.PopUI(ResPath.CreateRoom_Hoom);
        GameManager.GM.DS.CreateRoom_Xz = null;
        x = Content.DOLocalMoveX(-1600, 0.3f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
    }
    int[] ReturnValue = { 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    void EnterRoom()
    {
        if(Enter.IsInteractable())
        Enter.interactable = false;
        Invoke("reOpen", 2.0f);
        if (r != null)
            Destroy(r);
        Debug.Log("进入房间");
        PublicEvent.GetINS.NewRoom(ReturnValue,"xz");
    }
    void reOpen()
    {
        Enter.interactable = true;
    }
    void enter(ProtoBuf.EnterRoomRsp rsp)
    {
        if (r != null)
            Destroy(r);
        PublicEvent.GetINS.Event_joinRoomSuccess -= enter;
        GameManager.GM.DS.CreateRoom_Xz = null;
        GameManager.GM.DS.CreateRoom_Hoom = null;
        {
            //创建playroom
            GameManager.GM.DS.PlayRoom = GameManager.GM.PopUI(ResPath.PlayRoom);
            ///向房间注入当前服务器返回的房间信息
            GameManager.GM.DS.PlayRoom.GetComponent<UI_PlayRoom>().ReciveRoomData(rsp);

            //对当前的玩家进行排序
            PublicEvent.GetINS.ReciveData(rsp.mjRoom);

            ParticleManager.GetIns.SwitchSence(2);
        }
        x = Content.DOLocalMoveX(-1600, 0.3f);
        x.OnComplete(delegate
        {
            Destroy(this.gameObject);
        });
    }
}
