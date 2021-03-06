﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NetworkManager : MonoBehaviour {

    public static NetworkManager Instance;

    private DateTime ProcessNetworkEventTime_;      //定时检测网络连接状态  

    private string GateServerPath = "127.0.0.1";
    private ushort GateServerPort = 13131;

    //socket线程
    public TcpNetworkProcessor GateServerTcpConnect_ = new TcpNetworkProcessor();
    public TcpNetworkProcessor GameServerTcpConnect_ = new TcpNetworkProcessor();

    private static readonly object TcpIDLock = new object();    //同步锁
    private int TcpConnID = 0; // TCP网络连接ID
    //处理消息相关
    private Dictionary<int, PacketHandle> MsgHandleDic_ = new Dictionary<int, PacketHandle>();  //注册消息字典，只处理字典里注册过的消息
    public delegate void PacketHandle(int pid, byte[] msg, int msgSize);        //消息处理托管
    public Queue<Packet> MessageQueue_ = new Queue<Packet>();  //待处理的消息队列

    public bool Test_GateConnectSuccess_ = false;

    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitNetwork();
        RegisterAllNetworkMsgHandler();
    }

    //初始化socket连接
    public void InitNetwork() {
        this.GateServerTcpConnect_ = CreateTcpConnect();    //一个线程执行，网关TCP连接Socket
    }

    // 创建TCP连接
    public TcpNetworkProcessor CreateTcpConnect() {
        lock (TcpIDLock) {
            this.TcpConnID++;
            TcpNetworkProcessor tcp = new TcpNetworkProcessor();    //创建TCP连接时，顺带创建发送线程，后台一直监听发送消息
            tcp.ID_ = ++this.TcpConnID;
            return tcp;
        }
    }

    //注册需要处理的消息函数
    public void RegisterAllNetworkMsgHandler() {
        ////心跳反馈
        //RegisterMessageHandler((int)MsgDef.GS2CHeartTickRet, LoginMsgHandler.Instance.RevMsgGS2CHeartTickRet);
        //网关反馈
        RegisterMessageHandler((int)MsgDef.GAS2CLoginRet, LoginMsgHandler.Instance.RevMsgGAS2CLoginRet);
        //注册账号反馈
        RegisterMessageHandler((int)MsgDef.GAS2CRegisterRet, LoginMsgHandler.Instance.RevMsgGAS2CRegisterRet);
        //请求账号角色信息列表反馈
        RegisterMessageHandler((int)MsgDef.GS2CRequestRoleInfoRet, LoginMsgHandler.Instance.RevMsgGS2CRequestRoleInfoRet);
    }

    private void RegisterMessageHandler(int pid, PacketHandle hander) {
        if (!this.MsgHandleDic_.ContainsKey(pid)) {
            this.MsgHandleDic_.Add(pid, hander);
        }
    }

    //点击登录后，登录网关服务器
    public void LoginGateServer() {
        ConnectGateServer(this.GateServerPath, this.GateServerPort);
    }

    //连接网关
    public void ConnectGateServer(string addr, ushort port) {
        this.GateServerTcpConnect_.ClearWillSendMessage();
        Debug.Log(string.Format("连接网关服务器[{0}:{1}]", addr, port));
        this.GateServerTcpConnect_.Connect(addr, port);     //连接网关成功后，开启新一个接收消息的线程，该接收线程一直存在，除非断开连接，套接字关闭
    }

    void Update() {
        ////心跳
        //ProcessHeartTick();

        //根据网络状态处理网络事件（定时器）
        ProcessNetworkEventByState();

        //获取网关服务器消息
        FetchGateServerMsg();

        //根据当前消息队列，处理消息函数
        ProcMsgLogic();      
    }

    //心跳
    private void ProcessHeartTick() {
        if (this.Test_GateConnectSuccess_ && this.GateServerTcpConnect_ != null) {
            if (this.GateServerTcpConnect_.IsConnected()) {
                if (this.GateServerTcpConnect_.IsHeartTickSendTime()) {
                    LoginMsgHandler.Instance.SendMsgC2GSHeartTick();
                }
                //心跳超时，连接断开
                if (this.GateServerTcpConnect_.IsConnectOverTime()) {
                    Debug.LogError("心跳超时，连接断开");
                    this.GateServerTcpConnect_.DisConnect();
                }
            } else {
                if (!this.GateServerTcpConnect_.IsNativeSocketNull()) {
                    Debug.LogError("连接断开");
                }
            }
        }
    }

    //每100毫秒检测一次网络连接状况
    private bool IsOverProcessNetworkEventTimeInterval() {
        double interval = DateTime.Now.Subtract(this.ProcessNetworkEventTime_).TotalMilliseconds;
        return interval >= 100;
    }

    //根据网络状态处理网络事件
    private void ProcessNetworkEventByState() {
        if (IsOverProcessNetworkEventTimeInterval()) {
            //网关连接
            if (this.GateServerTcpConnect_ != null) {
                if (e_SocketState.SCK_CONNECT_SUCCESS == this.GateServerTcpConnect_.SocketState_) {
                    this.GateServerTcpConnect_.SocketState_ = e_SocketState.SCK_CONNECTED;
                    OnConnectedGateServer();
                }
            }
        }
    }

    //网关连接成功
    public void OnConnectedGateServer() {
        if (null == this.GateServerTcpConnect_) {
            Debug.LogError("OnConnectedGateServer error!");
        } else {
            Debug.Log("网关连接成功，跳转登录场景");
            GameManager.Instance.SwitchScene("Login");
        }       
    }

    //游戏服务器连接成功
    public void OnConnectedGameServer() {
        if (null == this.GameServerTcpConnect_) {
            Debug.LogError("OnConnectedGameServer error!");
        } else {
            Debug.LogError("游戏服务器连接成功，进入角色选择界面");            
        }    
    }

    //获取网关服务器消息
    public void FetchGateServerMsg() {
        if (null == this.GateServerTcpConnect_) {
            return;
        }
        Queue<Packet> queue = this.GateServerTcpConnect_.QueryRevQueue();
        if (queue.Count <= 0) {
            return;
        }
        lock (this.MessageQueue_) {
            this.GateServerTcpConnect_.PopMessage(ref this.MessageQueue_);
        }
    }

    //处理消息逻辑
    public void ProcMsgLogic() {
        int count = 10;
        while (this.MessageQueue_.Count != 0 && count > 0) {
            count--;
            Packet msg = this.MessageQueue_.Dequeue();
            if (null == msg) {
                continue;
            }
            if (!this.MsgHandleDic_.ContainsKey(msg.pid)) {
                Debug.LogError(string.Format("消息id:{0} 没有注册处理函数句柄!", msg.pid));
                continue;
            }
            this.MsgHandleDic_[msg.pid](msg.pid, msg.data, msg.length);
        }
    }


    // 发送消息到网关
    public bool SendToGAS(int id, System.Object msg) {
        if (null == this.GateServerTcpConnect_ || !this.GateServerTcpConnect_.IsConnected()) {
            Debug.LogError(string.Format("网关服务器连接断开，不能发送消息[msgid:{0}]", id));
            return false;
        }
        return this.GateServerTcpConnect_.Send(id, msg);
    }

    //断开网关
    public void DisconnectGateServer() {
        if (null != this.GateServerTcpConnect_) {
            this.GateServerTcpConnect_.DisConnect();
        }
    }


    private void OnDestroy() {
        DisconnectGateServer();
    }

}
