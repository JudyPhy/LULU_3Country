using UnityEngine;
using System.Collections;

public class Define {

    public static string _bpRoot_self = "bpRoot_self";
    public static string _bpRoot_monster = "bpRoot_monster";
    public static string _bpRoot_boss = "bpRoot_boss";

    public static string GetRootName(BattleRoleType type) {
        string bproot = "";
        if (type == BattleRoleType.Self || type == BattleRoleType.Rival) {
            bproot = _bpRoot_self;
        } else if (type == BattleRoleType.Monster) {
            bproot = _bpRoot_monster;
        } else if (type == BattleRoleType.Boss) {
            bproot = _bpRoot_boss;
        }
        return bproot;
    }


}

public enum BattleRoleType {
    Self,
    Rival,
    Monster,
    Boss,
}

public enum LoadingType {
    LoginToSelectRole,  //登录跳转选择角色界面
    ToBattleScene,
}

public enum WindowEvent {
    LoadingUI_CreateRoleModels,
}

public enum GameModel {
    pvp_33,
}