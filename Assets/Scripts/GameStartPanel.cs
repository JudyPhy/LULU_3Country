using UnityEngine;
using System.Collections;

public class GameStartPanel : WindowsBasePanel {

    private GameObject BtnConnectServer_;

    public override void OnAwake() {
        this.BtnConnectServer_ = this.transform.FindChild("Button").gameObject;
        UIEventListener.Get(this.BtnConnectServer_).onClick += OnStartGame;
    }

    private void OnStartGame(GameObject go) {
        NetworkManager.Instance.LoginGateServer();
    }

}
