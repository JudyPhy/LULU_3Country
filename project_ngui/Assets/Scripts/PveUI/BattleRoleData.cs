using UnityEngine;
using System.Collections;

public class BattleRoleData {

    public int Pos;
    public int RoleID;
    public bool IsPlayer;
    //属性
    public int MaxHp;
    public int CurHp;
    public int Atk;
    public int Speed;
    public int Def;
    public int AtkRange;
    public int TempDecHpByHurt;
    public int CurEnergy;

    public BattleRoleData(int pos, int roleId, bool isSelf) {
        Pos = pos;
        RoleID = roleId;
        IsPlayer = isSelf;
        InitBattleAttrData();
    }

    private void InitBattleAttrData() {
        if (ConfigData.Instance.HeroConfigDict.ContainsKey(this.RoleID)) {
            HeroConfigData roleCfg = ConfigData.Instance.HeroConfigDict[this.RoleID];
            this.MaxHp = roleCfg._hp;
            this.CurHp = roleCfg._hp;
            this.Atk = roleCfg._atk;
            this.Speed = roleCfg._speed;
            this.Def = roleCfg._def;
            this.AtkRange = roleCfg._atkRange;
            this.TempDecHpByHurt = 0;
            this.CurEnergy = 0;
        }
    }

}
