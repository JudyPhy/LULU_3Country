using UnityEngine;
using System.Collections;

public class LoadingUI : WindowsBasePanel {

    private System.DateTime StartTime;
    private UISprite Sprite;

    private GameObject SelectRoleModelRoot;
    private bool RoleModelCreateOver = false;

    public override void OnAwake() {
        this.Sprite = this.transform.FindChild("Sprite").GetComponent<UISprite>();
        this.StartTime=System.DateTime.Now;
        this.Sprite.color = Color.yellow;
    }

    private bool IsOverTime() {
        if (System.DateTime.Now.Subtract(this.StartTime).TotalMilliseconds > 1000) {
            this.StartTime = System.DateTime.Now;
            return true;
        }
        return false;
    }

    public override void OnUpdate() {
        if (IsOverTime()) {
            this.Sprite.color = this.Sprite.color == Color.yellow ? Color.blue : Color.yellow;
        }
        if (this.RoleModelCreateOver) {
            UIManager.Instance.ShowMainWindow<CreateRoleUI>(eWindowsID.CreateRoleUI);
        }
    }

    public override void RevWindowEvent(WindowEvent windowEvent, params object[] args) {
        switch (windowEvent) {
            case WindowEvent.LoadingUI_CreateRoleModels: {
                    CreateRoleModels();
                }
                break;
            default:
                break;
        }
    }

    #region 登录跳转到选择角色界面时，提前初始化好三个模型

    //初始化角色模型
    public void CreateRoleModels() {
        if (this.SelectRoleModelRoot == null) {
            this.SelectRoleModelRoot = new GameObject("RoleRoot");
        }
        this.SelectRoleModelRoot.transform.localEulerAngles = Vector3.zero;
        this.SelectRoleModelRoot.transform.localPosition = new Vector3(100, 0, 0);
        this.SelectRoleModelRoot.transform.localScale = Vector3.one;
        GameManager.Instance.MainCamera.transform.localPosition = new Vector3(1000, 0, 0);
        GameManager.Instance.MainCamera.transform.localEulerAngles = Vector3.zero;
        StartCoroutine(CreateRole(RoleType.Warrior));
        StartCoroutine(CreateRole(RoleType.Assassin));
        StartCoroutine(CreateRole(RoleType.Armourer));
    }

    private IEnumerator CreateRole(RoleType roleType) {
        GameObject obj = ResourcesManager.Instance.GetModelPrefab(roleType.ToString());
        obj.name = roleType.ToString();
        obj.transform.parent = this.SelectRoleModelRoot.transform;
        obj.transform.localEulerAngles = new Vector3(0, 180, 0);
        obj.transform.localPosition = new Vector3(0, -0.6f, 3);
        obj.transform.localScale = Vector3.one;
        if (roleType == RoleType.Armourer) {
            this.RoleModelCreateOver = true;
        }
        yield break;
    }

    #endregion

}
