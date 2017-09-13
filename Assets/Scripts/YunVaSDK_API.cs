using UnityEngine;
using System.Collections;
using System;
using YunvaIM;

public class YunVaSDK_API : MonoBehaviour
{
    private string sUserId = "123";
    private string labelText = "ssss";
    string filePath = "";
    private string recordPath = string.Empty;//返回录音地址
    private string recordUrlPath = string.Empty;//返回录音url地址




    void Start()
    {
        Events.Inst().StartMic += OnClick;   //注册点击方法. 第一次触发录音第二次触发结束录音..在触发结束录音时如果传入false值则不发送
        Events.Inst().PlaySound += DownloadSound;
        int init = YunVaImSDK.instance.YunVa_Init(0,1001147, Application.persistentDataPath, false);
        EventListenerManager.AddListener(ProtocolEnum.IM_RECORD_VOLUME_NOTIFY, ImRecordVolume);//录音音量大小回调监听
        OnLogin("");
    }

    public void OnClick(bool isSend)
    {
        if (isSend)
        {
            Debug.Log("anxiaqule");
            OnStartSoundRecording();
        }
        else
        {
            Debug.Log("quxiao");
            OnStopSoundRecording();
        }
    }

    /// <summary>
    /// 登录方法
    /// </summary>
    void OnLogin(string tt1, string GameSeverID = "1111")
    {
        sUserId = BaseProto.playerInfo.m_id.ToString();
        string ttFormat = "{{\"nickname\":\"{0}\",\"uid\":\"{1}\"}}";
        string tt = string.Format(ttFormat, sUserId, sUserId);
        string[] wildcard = new string[2];
        wildcard[0] = "0x001";
        wildcard[1] = "0x002";
        YunVaImSDK.instance.YunVaOnLogin(tt, GameSeverID, wildcard, 0, (data) =>
        {
            if (data.result == 0)
            {

                //  labelText = string.Format("登录成功，昵称:{0},用户ID:{1}", data.nickName, data.userId);
                YunVaImSDK.instance.RecordSetInfoReq(true);//开启录音的音量大小回调
            }
            else
            {
                // labelText = string.Format("登录失败，错误消息：{0}", data.msg);
            }
        });
    }

    public void OnStartSoundRecording()
    {
        
        filePath = string.Format("{0}/{1}.amr", Application.persistentDataPath, DateTime.Now.ToFileTime());
        YunVaImSDK.instance.RecordStartRequest(filePath);
    }
    public void OnStopSoundRecording()
    {
        YunVaImSDK.instance.RecordStopRequest(StopRecordResponse);
    }


    public void PlaySound()
    {
        //labelText = "播放语音.........";
        
        string ext = DateTime.Now.ToFileTime().ToString();
        YunVaImSDK.instance.RecordStartPlayRequest(recordPath, "", ext, (data2) =>
            {
            });
        //SoundMag.GetINS.PlayPopCard("4w", 1, 0);
    }
   
    /// <summary>
    /// 上传录音到服务器
    /// </summary>
    public void SendSound()
    {
        string fileId = DateTime.Now.ToFileTime().ToString();
        YunVaImSDK.instance.UploadFileRequest(recordPath, fileId, (data1) =>
        {
            if (data1.result == 0)
            {
                recordUrlPath = data1.fileurl;
                ProtoBuf.ChatMessageReq temp = new ProtoBuf.ChatMessageReq();
                temp.msgType = ProtoBuf.ChatMessageReq.MsgType.InputVoice;
                temp.msgString = recordUrlPath;
                temp.senderId = BaseProto.playerInfo.m_id;
                BaseProto.Inst().SendChatMsgRequest(temp);
                DownloadSound(recordUrlPath);
            }
        });
    }
    public void DownloadSound(string url)
    {

        YunVaImSDK.instance.RecordStartPlayRequest("", url, "", (data2) =>
          {

          });
    }


    private void StopRecordResponse(ImRecordStopResp data)
    {
        if (!string.IsNullOrEmpty(data.strfilepath))
        {
            recordPath = data.strfilepath;
            SendSound();
        }
    }
    public void ImRecordVolume(object data)
    {
        ImRecordVolumeNotify RecordVolumeNotify = data as ImRecordVolumeNotify;
    }

}
