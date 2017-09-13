using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
public class UI_UsedChat : MonoBehaviour
{
    Transform ThisTrans = null;
    List<Button> Faces = new List<Button>();
    List<Button> Chats = new List<Button>();

    public Transform FaceContent = null;
    public Transform ChatContent = null;
    public Text InputText = null;
    public Button Sent = null;
    public Button Close = null;
    public Transform Content = null;
    Tween x;
    void Awake()
    {
        Init();
    }
    // Use this for initialization
    void Start()
    {
        Default();
        x = Content.DOLocalMoveX(-1800, 0.8f).From();
        x.SetEase(Ease.OutSine);
        x.SetAutoKill(false);
        x.PlayForward();
    }
    void Init()
    {
        ThisTrans = this.gameObject.transform;
        int temp = FaceContent.childCount;
        for (int i = 0; i < temp; i++)
        {
            int tempNum = 0;
            tempNum = i;
            Faces.Add(FaceContent.GetChild(i).GetComponent<Button>());
            Faces[i].onClick.AddListener(delegate
            {
                SentFace(tempNum);
                Rest();
            });
        }
        temp = ChatContent.childCount;

        for (int i = 0; i < temp; i++)
        {
            Chats.Add(ChatContent.GetChild(i).GetComponent<Button>());
            Button tempButton = null;
            tempButton = Chats[i];
            Chats[i].onClick.AddListener(
                delegate
                {
                    SendMessagePre(tempButton.transform.GetComponentInChildren<Text>().text);
                }
                );
        }
    }
    void Default()
    {
        Sent.onClick.AddListener(SentMessage);
        Close.onClick.AddListener(Rest);
        PublicEvent.GetINS.Event_ExitRoomSucc += Rest;
    }
    void SentFace(int face)
    {
        Debug.Log("第" + face + "个face！");
        PublicEvent.GetINS.SentMegssageImage("x0xxd"+face);
    }
    void SendMessagePre(string Value)
    {
        Debug.Log(Value);
        PublicEvent.GetINS.SentMegssageText(Value);
        Rest();
    }
    void SentMessage()
    {
        if (InputText.text != "")
        {
            Debug.Log(InputText.text);          
            PublicEvent.GetINS.SentMegssageText(InputText.text);
            InputText.text = "";
        }
        Rest();
    }
    void Rest()
    {
        PublicEvent.GetINS.Event_ExitRoomSucc -= Rest;
        x = Content.DOLocalMoveX(-1600, 0.4f);
        x.OnComplete(delegate 
        {
            Destroy(this.gameObject);
            Destroy(this);
        });
    }
}
