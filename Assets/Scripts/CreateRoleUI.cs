using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateRoleUI : WindowsBasePanel {

    private GameObject BtnWarrior;
    private GameObject BtnAssassin;
    private GameObject BtnArmourer;
    private UIButton BtnEnterGame;
    private List<Animator> RoleModelList = new List<Animator>();
    private UIInput InputName;

    private RoleType SelectedRoleType = RoleType.Warrior;

    public override void OnAwake() {
        this.BtnWarrior = this.transform.FindChild("ButtonWarrior").gameObject;
        this.BtnAssassin = this.transform.FindChild("ButtonAssassin").gameObject;
        this.BtnArmourer = this.transform.FindChild("ButtonArmourer").gameObject;
        this.BtnEnterGame = this.transform.FindChild("ButtonEnterGame").GetComponent<UIButton>();
        GameObject roleRoot = GameObject.Find("RoleRoot");
        this.RoleModelList.Add(roleRoot.transform.FindChild(RoleType.Warrior.ToString()).GetComponent<Animator>());
        this.RoleModelList.Add(roleRoot.transform.FindChild(RoleType.Assassin.ToString()).GetComponent<Animator>());
        this.RoleModelList.Add(roleRoot.transform.FindChild(RoleType.Armourer.ToString()).GetComponent<Animator>());
        this.InputName = this.transform.FindChild("InputName").GetComponent<UIInput>();
        UIEventListener.Get(this.BtnWarrior).onClick += OnClickSelectWarrior;
        UIEventListener.Get(this.BtnAssassin).onClick += OnClickSelectAssassin;
        UIEventListener.Get(this.BtnArmourer).onClick += OnClickSelectArmourer;
        UIEventListener.Get(this.BtnEnterGame.gameObject).onClick += OnClickEnterGame;
    }

    public override void OnInitWindow() {
        UpdateRoleTypeUI();
        SelectRoleType(this.SelectedRoleType);
    }

    private void UpdateRoleTypeUI() {
        List<pb.RoleInfo> list = CreateRoleMgr.Instance.QueryRoleInfoList();
        for (int i = 0; i < list.Count; i++) {
            if (!string.IsNullOrEmpty(list[i].RoleName)) {
                UILabel label = GetButtonLabelByRoleType((RoleType)list[i].RoleType);
                if (label != null) {
                    label.text = list[i].RoleLev + "级  " + list[i].RoleName;
                } else {
                    label.text = ((RoleType)list[i].RoleType).ToString();
                }
            }
        }
    }

    private UILabel GetButtonLabelByRoleType(RoleType type) {
        switch (type) { 
            case RoleType.Warrior:
                return this.BtnWarrior.transform.FindChild("Label").GetComponent<UILabel>();
            case RoleType.Assassin:
                return this.BtnAssassin.transform.FindChild("Label").GetComponent<UILabel>();
            case RoleType.Armourer:
                return this.BtnArmourer.transform.FindChild("Label").GetComponent<UILabel>();
            default:
                return null;
        }
    }

    private void OnClickSelectWarrior(GameObject go) {
        SelectRoleType(RoleType.Warrior);
    }
    private void OnClickSelectAssassin(GameObject go) {
        SelectRoleType(RoleType.Assassin);
    }

    private void OnClickSelectArmourer(GameObject go) {
        SelectRoleType(RoleType.Armourer);
    }

    private void SelectRoleType(RoleType selectType) {
        this.SelectedRoleType = selectType;
        this.RoleModelList[0].gameObject.SetActive(this.SelectedRoleType == RoleType.Warrior);
        this.RoleModelList[1].gameObject.SetActive(this.SelectedRoleType == RoleType.Assassin);
        this.RoleModelList[2].gameObject.SetActive(this.SelectedRoleType == RoleType.Armourer);
        this.RoleModelList[0].SetInteger("ActorType", 0);
        this.RoleModelList[1].SetInteger("ActorType", 0);
        this.RoleModelList[2].SetInteger("ActorType", 0);
        GameManager.Instance.MainCamera.transform.localPosition = new Vector3(100, 0, 0);
        pb.RoleInfo info = CreateRoleMgr.Instance.QueryRoleInfoByType(selectType);
        this.InputName.gameObject.SetActive(info == null);
    }

    private void OnClickEnterGame(GameObject go) {
        GameManager.Instance.MainCamera.transform.localPosition = new Vector3(1000, 0, 0);
        UIManager.Instance.ShowMainWindow<LoadingUI>(eWindowsID.LoadingUI);
        bool isNewRole = false;
        pb.RoleInfo roleInfo = CreateRoleMgr.Instance.QueryRoleInfoByType(this.SelectedRoleType);
        if (roleInfo == null) {
            if (string.IsNullOrEmpty(this.InputName.value)) {
                Debug.LogError("名字不能为空");
                return;
            }
            isNewRole = true;            
            roleInfo.RoleType = (int)this.SelectedRoleType;
            roleInfo.RoleName = this.InputName.value;
        }
        LoginMsgHandler.Instance.SendMsgC2GSEnterGame(roleInfo, isNewRole);
    }

}
