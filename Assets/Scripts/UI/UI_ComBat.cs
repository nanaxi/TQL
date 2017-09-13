using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UI_ComBat : MonoBehaviour
{
    Transform ThisTrans = null;
    public Button Close = null;
    public Transform Content = null;
    public Transform ContentList = null;
    GameObject DarkItem, LightItem;
    void Awake()
    {
        Init();
    }
    Tween x;
    // Use this for initialization
    void Start()
    {
        //DeFault();
        x = Content.DOLocalMoveX(-1800, 0.5f).From();
        x.SetEase(Ease.OutSine);
        x.SetAutoKill(false);
        x.PlayForward();
    }
    void Init()
    {
        ThisTrans = gameObject.transform;
        //Close = ThisTrans.Find("BG2/Close").GetComponent<Button>();
        DeFault();
    }
    void DeFault()
    {
        if (Close != null)
            Close.onClick.AddListener(Rest);
        DarkItem = Resources.Load<GameObject>("Prefabs/Btn_Zj_Select_Bg_dark");
        LightItem = Resources.Load<GameObject>("Prefabs/Btn_Zj_Select_Bg_light");
    }
    ProtoBuf.QueryInfoRsp CombatGainsRsp = null;
    public void SetComBat(ProtoBuf.QueryInfoRsp combatGainRsp)//CombatGainsRsp.mjRecords[j].rounds[t]
    {
        if (combatGainRsp != null)
        {
            CombatGainsRsp = combatGainRsp;
            for (int i = 0; i < CombatGainsRsp.mjRecords.Count; i++)
            {
                Transform tempItem = null;
                if (i % 2 == 0)
                {
                    tempItem = Instantiate(DarkItem).transform;
                    tempItem.SetParent(ContentList, false);
                    tempItem.GetComponent<Combat>().SetCombatInformation(CombatGainsRsp.mjRecords[i]);
                }
                else
                {
                    tempItem = Instantiate(LightItem).transform;
                    tempItem.SetParent(ContentList, false);
                    tempItem.GetComponent<Combat>().SetCombatInformation(CombatGainsRsp.mjRecords[i]);
                }
                //for (int z = 0; z < CombatGainsRsp.mjRecords[i].rounds.Count; z++)
                //{
                //    Debug.Log("记录：" + CombatGainsRsp.mjRecords[i].rounds[z].players[0].gold);
                //}
            }
        }
    }

    void Rest()
    {
        x = Content.DOLocalMoveX(-1800, 0.3f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
        GameManager.GM.DS.Combat = null;
        //Destroy(this.gameObject);
    }
}
