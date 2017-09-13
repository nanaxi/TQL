using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public class UI_Notic : MonoBehaviour
{
    Transform ThisTrans = null;
    public Text TheText = null;
    public Button Close = null;
    public Transform Content = null;
    void Awake()
    {
        Init();
    }
    Tween x;
    // Use this for initialization
    void Start()
    {
        DeFault();
        x = Content.DOLocalMoveX(-1380, 0.5f).From();
        x.SetEase(Ease.OutSine);
        x.SetAutoKill(false);
        x.PlayForward();
        //x.OnComplete(delegate{});
    }
    void Init()
    {
        ThisTrans = this.gameObject.transform;
    }
    void DeFault()
    {
        if (Close != null)
            Close.onClick.AddListener(Rest);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetMessage(string value)
    {
        TheText.text = value;
    }

    void Rest()
    {
        GameManager.GM.DS.Call = null;
        x = Content.DOLocalMoveX(-1380, 0.3f);
        x.OnComplete(delegate { Destroy(this.gameObject); });
    }
    void OnDestory()
    {
        GameManager.GM.DS.Notic = null;
    }
}
