using UnityEngine;
using System.Collections;

public class NetworkTest : MonoBehaviour {

    public static NetworkTest instance;

    public GameObject BtnLogin_;
    public UIInput InputAccount_;
    public UIInput InputPassWord_;

    void Awake() {
        instance = this;
        UIEventListener.Get(BtnLogin_).onClick = OnLoginGame;
    }

	// Use this for initialization
	void Start () {        
        
	}

    //登录网关服务器
    private void OnLoginGame(GameObject go) {              
    }

    private void ConnectGameServer(GameObject go) {

    }

	// Update is called once per frame
	void Update () {
        if (NetworkManager.Instance.GateServerTcpConnect_.IsConnected()) { 
        
        }
	}
}
