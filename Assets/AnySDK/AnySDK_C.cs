using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class AnySDK_C : MonoBehaviour
{
    public Texture2D texture;
    static AnySDK_C _AnySDK_C;
    public static AnySDK_C GetIns()
    {
        return _AnySDK_C;
    }

    void Awake()
    {
        _AnySDK_C = this;
        DontDestroyOnLoad(gameObject);
        AnySDKManager.AnySDKInit(this);
        StartCoroutine(DownIcon());
    }


    IEnumerator DownIcon()
    {
        yield return null;

        string str = GameManager.GM.JsonPath + "/icon.png";
        ///读取屏幕像素点
        byte[] byt = texture.EncodeToPNG();
        //File.WriteAllBytes(str, byt);
    }




    private void AnySDKCallBack(string msg)
    {
        AnySDKManager.UserExternalCall(msg);
    }

    private void AnySDKShareCallBack(string msg)
    {
        AnySDKManager.ShareExternalCall(msg);
    }

    public void Login()
    {
        string _userInfo = AnySDKManager.GetUserInfo();
        LitJson.JsonData jsonData2 = LitJson.JsonMapper.ToObject(_userInfo);



        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes((jsonData2["nickName"]).ToString());
        string base64Str = Convert.ToBase64String(byteArray);
        GlobalSettings.LoginUserName = base64Str;

        //GlobalSettings.LoginUserName = (jsonData2["nickname"]).ToString();
        GlobalSettings.LoginUserId = (jsonData2["uid"]).ToString();
        GlobalSettings.LoginChannel = "wx";//(jsonData2["unionid"]).ToString();
        GlobalSettings.avatarUrl = (jsonData2["avatarUrl"]).ToString();
        if (uint.Parse(jsonData2["sex"].ToString()) == 1)
        {
            GlobalSettings.sex = 1;
        }
        else
        {
            GlobalSettings.sex = 0;
        }



        //{

        //    byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(System.DateTime.Now.Day + "" + System.DateTime.Now.Hour + "" + System.DateTime.Now.Minute + "" + System.DateTime.Now.Second);
        //    string base64Str = Convert.ToBase64String(byteArray);
        //    GlobalSettings.LoginUserName = base64Str;
        //    //GlobalSettings.LoginUserName = System.DateTime.Now.Day + "" + System.DateTime.Now.Hour + "" + System.DateTime.Now.Minute + "" + System.DateTime.Now.Second;//Name.textComponent.text;//"荣昌店，糖糖"; // //"1113535";"1113535"; "1113535";// "6145131";9151653 ; "13165316"; "21213945";//"22171936";荣昌店，糖糖
        //    GlobalSettings.LoginUserId = GlobalSettings.LoginUserName;//ID.textComponent.text;//"Y-ai188";//GlobalSettings.LoginUserName;Y-ai188
        //    GlobalSettings.sex = 0;
        //}
        LoginProcessor.Inst().Login();
    }
}
