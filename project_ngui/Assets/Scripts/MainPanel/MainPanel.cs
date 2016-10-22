using UnityEngine;
using System.Collections;

public class MainPanel : WindowsBasePanel {

    private UIButton Btn_Battle;
    private UIButton Btn_OfflineGame;
    private UIButton Btn_OnlineGame;

    public override void OnAwake() {
        base.OnAwake();

        Transform bottomAnchor = this.transform.FindChild("BottomAnchor");
        this.Btn_OfflineGame = bottomAnchor.FindChild("Grid/OfflineButton").GetComponent<UIButton>();
        this.Btn_OnlineGame = bottomAnchor.FindChild("Grid/OnlineButton").GetComponent<UIButton>();
        UIEventListener.Get(this.Btn_OfflineGame.gameObject).onClick = OnEnterSelectRole;
        UIEventListener.Get(this.Btn_OnlineGame.gameObject).onClick = OnEnterLoadingUI;

        Transform bottomRightAnchor = this.transform.FindChild("BottomRightAnchor");
        this.Btn_Battle = bottomRightAnchor.FindChild("Button_Fuben").GetComponent<UIButton>();
        UIEventListener.Get(this.Btn_Battle.gameObject).onClick = OnEnterBattle;
    }

    public override void OnInitWindow() {
        base.OnInitWindow();
    }

    public override void OnStart() {
        base.OnStart();
    }
    RaycastHit hit;
    public override void OnUpdate() {
        base.OnUpdate();

        if (Input.GetMouseButton(0)) {
            Ray ray = UIManager.UICamera_.ScreenPointToRay(Input.mousePosition);            
            Debug.DrawLine(ray.origin, hit.point, Color.red, 2);
            LayerMask mask = 1 << 1 << LayerMask.NameToLayer("UI");
            if (Physics.Raycast(ray, out hit, 1000, mask.value)) {
                Debug.LogError("11");
                Debug.DrawLine(ray.origin, hit.point);
            }
        }
    }

    public override void RevWindowEvent(WindowEvent windowEvent, params object[] args) {
        base.RevWindowEvent(windowEvent, args);
    }

    private void OnEnterBattle(GameObject go) {
        UIManager.Instance.ShowWindow<BattlePanel>(eWindowsID.BattleUI);
    }

    private void OnEnterSelectRole(GameObject go) {
        UIManager.Instance.ShowWindow<SelectRolePanel>(eWindowsID.SelecteRole);
    }

    private void OnEnterLoadingUI(GameObject go) {

    }

}
