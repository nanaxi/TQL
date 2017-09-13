﻿using UnityEngine;
using System.Collections;

public class SoundMag : MonoBehaviour
{
    //public AudioSource 
    // Use this for initialization
    public static float BgValue = 1;
    public static float EffectValue = 1;

    public AudioSource Player0;
    public AudioSource Player1;
    public AudioSource Player2;
    public AudioSource Player3;
    public AudioSource BGM;
    public AudioSource EffectSound;

    static SoundMag _INS;
    public static SoundMag GetINS
    {
        get
        {
            if (_INS == null)
            {
                _INS = new SoundMag();
            }
            return _INS;
        }
    }

    public SoundMag()
    {
        _INS = this;
    }
    float MusicValue=1, FxValue=1;
    void Start()
    {
        PublicEvent.GetINS.EventChangeBgValue = ChangeBgValue;
        PublicEvent.GetINS.EventChangeEffectValue = ChangeEffectValue;


        if (PlayerPrefs.HasKey("MusicValue"))
        {
            MusicValue = PlayerPrefs.GetFloat("MusicValue", 1);

        }
        SoundMag.GetINS.ChangeBgValue(MusicValue);
        if (PlayerPrefs.HasKey("FxValue"))
        {
            FxValue = PlayerPrefs.GetFloat("FxValue", 1);

        }
        SoundMag.GetINS.ChangeEffectValue(FxValue);
        //if (PlayerPrefs.HasKey("CanRead"))
        //{
        //    if (PlayerPrefs.GetInt("CanRead", 1) == 1)
        //    {
        //        CanRead = true;
        //    }
        //    else
        //    {
        //        CanRead = false;
        //    }
        //}
        //if (PlayerPrefs.HasKey("CanShake"))
        //{
        //    if (PlayerPrefs.GetInt("CanShake", 1) == 1)
        //    {
        //        CanShake = true;
        //    }
        //    else
        //    {
        //        CanShake = false;
        //    }
        //}
    }
    /// <summary>
    /// 1代表按钮按下去 ，2代表碰，3代表杠，4代表胡，5代表自摸,6代表准备，7代表加入房间，8代表牌面选中了，9代表牌打出，0代表牌放回
    /// </summary>
    /// <param name="num">1代表按钮按下去 ，2代表碰，3代表杠，4代表胡，5代表自摸,6代表准备，7代表加入房间，8代表牌面选中了，9代表牌打出</param>
    public void PlaySound(int num, int sex = 1)
    {
        switch (num)
        {
           
        }
    }
    /// <summary>
    /// 打出牌的音效
    /// </summary>
    /// <param name="num">1w 1t 1b</param>
    /// <param name="sex">1为男，0为女</param>
    public void PlayPopCard(string num, int sex = 1,int pos=0)
    {
        if (sex == 1)
        {
            #region 男
            var t = Resources.Load<AudioClip>("AudioClips/mjvoice/man/"+ num);
            if (t == null)
            {
                Debug.Log("空值");
            }
            else {
                switch (pos)
                {
                    case 0:
                        Player0.clip = t;
                        Player0.Play();
                        break;
                    case 1:
                        Player1.clip = t;
                        Player1.Play();
                        break;
                    case 2:
                        Player2.clip = t;
                        Player2.Play();
                        break;
                    case 3:
                        Player3.clip = t;
                        Player3.Play();
                        break;
                    default:
                        break;
                }

            }
            
            #endregion
        }

        else
        {
            #region 女
            var t = Resources.Load<AudioClip>("AudioClips/mjvoice/woman/" + num);
            if (t == null)
            {
                Debug.Log("空值");
            }
            else
            {
                switch (pos)
                {
                    case 0:
                        Player0.clip = t;
                        Player0.Play();
                        break;
                    case 1:
                        Player1.clip = t;
                        Player1.Play();
                        break;
                    case 2:
                        Player2.clip = t;
                        Player2.Play();
                        break;
                    case 3:
                        Player3.clip = t;
                        Player3.Play();
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }


    }
    public void ChatSound(string num, int sex = 1,int pos=0)
    {
        if (sex == 1)
        {
            #region 男
            var t = Resources.Load<AudioClip>("AudioClips/quickvoice/man/" + num);
            if (t == null)
            {
                Debug.Log("空值");
            }
            else
            {
                switch (pos)
                {
                    case 0:
                        Player0.clip = t;
                        Player0.Play();
                        break;
                    case 1:
                        Player1.clip = t;
                        Player1.Play();
                        break;
                    case 2:
                        Player2.clip = t;
                        Player2.Play();
                        break;
                    case 3:
                        Player3.clip = t;
                        Player3.Play();
                        break;
                    default:
                        break;
                }

            }

            #endregion
        }

        else
        {
            #region 女
            var t = Resources.Load<AudioClip>("AudioClips/quickvoice/woman/" + num);
            if (t == null)
            {
                Debug.Log("空值");
            }
            else
            {
                switch (pos)
                {
                    case 0:
                        Player0.clip = t;
                        Player0.Play();
                        break;
                    case 1:
                        Player1.clip = t;
                        Player1.Play();
                        break;
                    case 2:
                        Player2.clip = t;
                        Player2.Play();
                        break;
                    case 3:
                        Player3.clip = t;
                        Player3.Play();
                        break;
                    default:
                        break;
                }
            }
            #endregion
        }

    }
    public void ChangeBgValue(float value)
    {
        BgValue = value;
        BGM.volume = BgValue;
    }
    public void ChangeEffectValue(float value)
    {
        EffectValue = value;
        EffectSound.volume = EffectValue;
        Player0.volume = EffectValue;
        Player1.volume = EffectValue;
        Player2.volume = EffectValue;
        Player3.volume = EffectValue;
    }

}
