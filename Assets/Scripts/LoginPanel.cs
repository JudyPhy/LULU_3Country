using UnityEngine;
using System.Collections;

public class LoginPanel : WindowsBasePanel {

    private UIButton BtnLogin;
    private UIInput InputAccount;
    private UIInput InputPassword;
    private UIButton BtnRegister;

    public override void OnAwake() {
        this.BtnLogin = this.transform.FindChild("ButtonLogin").GetComponent<UIButton>();
        this.InputAccount = this.transform.FindChild("InputAccount").GetComponent<UIInput>();
        this.InputPassword = this.transform.FindChild("InputPassWord").GetComponent<UIInput>();
        this.BtnRegister = this.transform.FindChild("ButtonRegister").GetComponent<UIButton>();
        UIEventListener.Get(this.BtnLogin.gameObject).onClick += OnClickBtnLogin;
        UIEventListener.Get(this.BtnRegister.gameObject).onClick += OnClickBtnRegister;
    }

    public override void OnStart() {
        
    }

    private void OnClickBtnLogin(GameObject go) {
        string account = this.InputAccount.value;
        if (string.IsNullOrEmpty(account)) {
            Debug.LogError("账号名称不能为空");
            return;
        }
        string password = this.InputPassword.value;
        if (string.IsNullOrEmpty(password)) {
            Debug.LogError("密码不能为空");
            return;
        }
        LoginMsgHandler.Instance.SendMsgC2GASLogin(account, password);
    }

    private void OnClickBtnRegister(GameObject go) {
        string account = this.InputAccount.value;
        if (string.IsNullOrEmpty(account)) {
            Debug.LogError("账号名称不能为空");
            return;
        }
        string password = this.InputPassword.value;
        if (string.IsNullOrEmpty(password)) {
            Debug.LogError("密码不能为空");
            return;
        }
        LoginMsgHandler.Instance.SendMsgC2GASRegister(account, password);
    }

    public override void OnUpdate() {
        
    }

}
