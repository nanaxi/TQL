using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Vote : MonoBehaviour {
    Transform ThisTrans = null;
    public Button Jujue = null;
    public Button TongYi = null;
    public Button Close = null;
    public Text PlayerReq = null;
	// Use this for initialization
	void Start () {
        Init();
        //Default();
    }

	void Init()
    {
        ThisTrans = this.gameObject.transform;
        //Jujue = ThisTrans.Find("BG/JuJue").GetComponent<Button>();
        //TongYi = ThisTrans.Find("BG/TongYi").GetComponent<Button>();
        //Close = ThisTrans.Find("BG2/Close").GetComponent<Button>();
        //PlayerReq = ThisTrans.Find("BG/Req").GetComponent<Text>();   
    }
    public void Default(string Name="李红衣")
    {
        PlayerReq.text = Name.ToString()+"申请退出游戏,是否同意立刻结束游戏？";
        Jujue.onClick.AddListener(ReJect);
        TongYi.onClick.AddListener(Agree);
        Close.onClick.AddListener(ReJect);
        PublicEvent.GetINS.Event_ExitRoomSucc += Rest;
        PublicEvent.GetINS.voteQuit += Rest;
    }
    void ReJect()
    {
        Debug.Log("拒绝！");
        PublicEvent.GetINS.VoteRequest(false);
        Rest();
    }
    void Agree()
    {
        Debug.Log("同意！");
        PublicEvent.GetINS.VoteRequest(true);
        Rest();
    }
    void Rest()
    {
        PublicEvent.GetINS.voteQuit -= Rest;
        GameManager.GM.DS.Voting = null;
        PublicEvent.GetINS.Event_ExitRoomSucc -= Rest;
        Destroy(this.gameObject);
    }
}
