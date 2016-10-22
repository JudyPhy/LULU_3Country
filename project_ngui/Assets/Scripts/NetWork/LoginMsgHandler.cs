using UnityEngine;
using System.Collections;
using System.IO;

public class LoginMsgHandler  {

    private static LoginMsgHandler instance;
    public static LoginMsgHandler Instance {
        get {
            if (instance == null) {
                instance = new LoginMsgHandler();
            }
            return instance;
        }
    }

    private string CurAccountName;
    private string CurPassword;

    #region 客户端->服务器

    //玩家-->网关服务器 登录
    public void SendMsgC2GASLogin(string account, string password) {
        Debug.Log("SendMsgC2GASLogin==>> [" + account + "]  [" + password + "]");
        pb.C2GASLogin msg = new pb.C2GASLogin();
        msg.AccountName = account;
        msg.Password = password;
        this.CurAccountName = account;
        this.CurPassword = password;
        NetworkManager.Instance.SendToGAS((int)MsgDef.C2GASLogin, msg);
    }

    //玩家-->网关服务器 注册账号
    public void SendMsgC2GASRegister(string account, string password) {
        Debug.Log("SendMsgC2GASRegister==>> [" + account + "]  [" + password + "]");
        pb.C2GASRegister msg = new pb.C2GASRegister();
        msg.AccountName = account;
        msg.Password = password;
        NetworkManager.Instance.SendToGAS((int)MsgDef.C2GASRegister, msg);
    }

    //发送心跳
    public void SendMsgC2GSHeartTick() {
        //Debug.LogError("======================>>>SendMsgC2GSHeartTick");
        pb.C2GSHeartTick msg = new pb.C2GSHeartTick();
        NetworkManager.Instance.SendToGAS((int)MsgDef.C2GSHeartTick, msg);
    }

    //请求账号角色信息
    public void SendMsgC2GSRequestRoleInfo(string account) {
        Debug.Log("======================>>>SendMsgC2GSRequestRoleInfo account:[" + account + "]");
        pb.C2GSRequestRoleInfo msg = new pb.C2GSRequestRoleInfo();
        msg.AccountName = account;
        NetworkManager.Instance.SendToGAS((int)MsgDef.C2GSRequestRoleInfo, msg);
    }

    //选定角色后登陆游戏
    public void SendMsgC2GSEnterGame(pb.RoleInfo info, bool isNewRole) {
        Debug.Log("======================>>>SendMsgC2GSEnterGame");
        pb.C2GSEnterGame msg = new pb.C2GSEnterGame();
        msg.AccountName = LocalFile.Load("AccountName");
        msg.SelectedRoleInfo = info;
        msg.IsNewRole = isNewRole;
        NetworkManager.Instance.SendToGAS((int)MsgDef.C2GSEnterGame, msg);
    }

    #endregion


    #region 服务器->客户端

    //网关服务器登录反馈
    public void RevMsgGAS2CLoginRet(int pid, byte[] msgBuf, int msgSize) {
        Debug.Log("==>> RevGAS2CLoginRet");
        Stream stream = new MemoryStream(msgBuf);
        pb.GAS2CLoginRet msg = ProtoBuf.Serializer.Deserialize<pb.GAS2CLoginRet>(stream);
        if (msg.RetCode == pb.GAS2CLoginRet.ErrorCode.LOGIN_SUCCESS) {
            LocalFile.Save("AccountName", this.CurAccountName);
            LocalFile.Save("Password", this.CurPassword);
            GameManager.Instance.LoginSuccess();
            return;
        } else if (msg.RetCode == pb.GAS2CLoginRet.ErrorCode.LOGIN_ACCOUNTNAME_ERR) {
            Debug.LogError("账号不存在");
        } else if (msg.RetCode == pb.GAS2CLoginRet.ErrorCode.LOGIN_PWD_ERR) {
            Debug.LogError("密码错误");
        } else {
            Debug.LogError("未知错误");
        }
    }

    //注册账号反馈
    public void RevMsgGAS2CRegisterRet(int pid, byte[] msgBuf, int msgSize) {
        Debug.Log("==>> RevGAS2CRegisterRet");
        Stream stream = new MemoryStream(msgBuf);
        pb.GAS2CRegisterRet msg = ProtoBuf.Serializer.Deserialize<pb.GAS2CRegisterRet>(stream);
        if (msg.RetCode == pb.GAS2CRegisterRet.ErrorCode.LOGIN_SUCCESS) {
            Debug.LogError("注册成功:" + msg.AccountName);
        } else if (msg.RetCode == pb.GAS2CRegisterRet.ErrorCode.LOGIN_FAIL_ACCOUNTEXIST) {
            Debug.LogError("账号已存在:" + msg.AccountName);
        } else {
            Debug.LogError("注册失败:" + msg.AccountName);
        }
    }

    //登录网关成功
    public void LoginGateServerSuccess() {
        NetworkManager.Instance.GateServerTcpConnect_.UpdateRecvHeartTickTime();
        NetworkManager.Instance.Test_GateConnectSuccess_ = true;
    }

    //接收心跳
    public void RevMsgGS2CHeartTickRet(int pid, byte[] msgBuf, int msgSize) {
        //Debug.LogError("RevMsgGS2CHeartTickRet");
        NetworkManager.Instance.GateServerTcpConnect_.UpdateRecvHeartTickTime();
    }

    //请求账号角色信息反馈
    public void RevMsgGS2CRequestRoleInfoRet(int pid, byte[] msgBuf, int msgSize) {
        Debug.Log("====>>>RevMsgGS2CRequestRoleInfoRet");
        Stream stream = new MemoryStream(msgBuf);
        pb.GS2CRequestRoleInfoRet msg = ProtoBuf.Serializer.Deserialize<pb.GS2CRequestRoleInfoRet>(stream);
        CreateRoleMgr.Instance.InitRoleInfoList(msg.RoleInfoList);
        UIManager.Instance.SendMessageToWindow(eWindowsID.LoadingUI, WindowEvent.LoadingUI_CreateRoleModels);
    }

    //角色详细信息
    public void RevMsgAS2CLoginRet(int pid, byte[] msgBuf, int msgSize) {
        Debug.Log("==>> RevGAS2CLoginRet");
        Stream stream = new MemoryStream(msgBuf);
        pb.GAS2CLoginRet msg = ProtoBuf.Serializer.Deserialize<pb.GAS2CLoginRet>(stream);
        if (msg.RetCode == pb.GAS2CLoginRet.ErrorCode.LOGIN_SUCCESS) {
            LocalFile.Save("AccountName", this.CurAccountName);
            LocalFile.Save("Password", this.CurPassword);
            GameManager.Instance.LoginSuccess();
            return;
        } else if (msg.RetCode == pb.GAS2CLoginRet.ErrorCode.LOGIN_ACCOUNTNAME_ERR) {
            Debug.LogError("账号不存在");
        } else if (msg.RetCode == pb.GAS2CLoginRet.ErrorCode.LOGIN_PWD_ERR) {
            Debug.LogError("密码错误");
        } else {
            Debug.LogError("未知错误");
        }
    }

    #endregion
}
