using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager GetIns = null;

    public GameObject Sun = null;
    public GameObject Flower = null;
    //public GameObject Transition = null;

    public GameObject LoginBg = null;
    public GameObject MainBg = null;

    public ParticleSystem red = null, bule = null;
    /// <summary>
    /// 过场动画
    /// </summary>
    public GameObject changec = null;
    void Awake()
    {
        if (GetIns == null)
            GetIns = this;
    }
    IEnumerator OpenVoice()
    {
        yield return new WaitForSeconds(5.0f);
        this.gameObject.GetComponent<YunVaSDK_API>().enabled = true;
    }

    public Tween t;
    /// <summary>
    /// 切换场景的时候顺带调用 0：切换到登陆界面   1：切换到主界面  2：过场切换  3：反向切换
    /// </summary>
    public void SwitchSence(int Num)
    {
        switch (Num)
        {
            case 0:
                PublicEvent.GetINS.voteTime = 0;
                PublicEvent.GetINS.IsMyVote = false;
                BaseProto.playerInfo.m_atRoomId = 0;
                BaseProto.playerInfo.m_cdRoomId = 0;
                LoginBg.SetActive(true);
                MainBg.SetActive(false);
                Sun.SetActive(true);
                Flower.SetActive(true);
                changec.SetActive(true);
                GameManager.GM.DS.Main.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3544, 0);
                Invoke("ReLogin", 0.6f);
                changec.transform.SetAsLastSibling();
                t = changec.transform.DOLocalMoveX(-4700, 1.5f).From();
                t.SetAutoKill(false);
                break;
            case 1:
                //Debug.Log("asd");
                //战绩查询
                GameManager.GM.GameType = "none";
                PublicEvent.GetINS.ZhanjiHuiFangRequst();
                PublicEvent.GetINS.DiamondRequst();

                PublicEvent.GetINS.voteTime = 0;
                PublicEvent.GetINS.IsMyVote = false;
                //BaseProto.playerInfo.m_atRoomId = 0;
                //BaseProto.playerInfo.m_cdRoomId = 0;
                GameManager.GM.DS.Main.GetComponent<UI_Main>().SetInfo(GameManager.GM._AllPlayerData[0].Name, GameManager.GM._AllPlayerData[0].ID.ToString(), GameManager.GM._AllPlayerData[0].Diamond.ToString(),GlobalSettings.avatarUrl);
                Sun.SetActive(false);
                Flower.SetActive(true);

                changec.SetActive(true);
                changec.transform.SetAsLastSibling();
                Invoke("show", 0.3f);
                t =changec.transform.DOLocalMoveX(-4000, 1.5f).From();
                t.SetAutoKill(false);
                break;
            case 2:
                Sun.SetActive(false);
                changec.SetActive(true);
                Flower.SetActive(false);
                changec.transform.SetAsLastSibling();
                Invoke("dis", 0.1f);
                t = changec.transform.DOLocalMoveX(-2000, 1.5f).From();
                t.SetAutoKill(false);
                if (GameManager.GM.DS.Login != null)
                {
                    LoginBg.SetActive(false);
                    MainBg.SetActive(true);
                    Destroy(GameManager.GM.DS.Login);
                }
                break;
            case 3:
                changec.SetActive(true);
                changec.transform.SetAsLastSibling();
                Invoke("dis", 0.6f);
                t.PlayBackwards();
                t = changec.transform.DOLocalMoveX(-4000, 1.5f).From();
                break;
            default:
                break;
        }
    }
    // float i =0;
    //void temp()
    //{
    //    //yield return new WaitForSeconds(0.2f);

    //    Debug.Log(i);
    //    i += 30;
    //    changec.GetComponent<RectTransform>().anchoredPosition = new Vector2(i,0);
    //}
    void ReLogin()
    {    
        GameManager.GM.DS.Login =GameManager.GM.PopUI(ResPath.Login);   
    }
    void show()
    {
        GameManager.GM.DS.Main.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
    }
    void dis()
    {
        GameManager.GM.DS.Main.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3544, 0);
    }
    public void showRedYun()
    {
        //red.gameObject.SetActive(true);
        red.Play();
    }
    public void showBuleYun()
    {
        //bule.gameObject.SetActive(true);
        bule.Play();
    }
}
