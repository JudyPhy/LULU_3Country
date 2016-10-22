using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BattleStatus {
    Idle,
    BattleNewRound,
    BattleNewAtkByCurRound,
    BattleAtkerMove,
    BattleAtkerAtk,
    BattleHurt,
    BattleOver,
}

public class BattleManager {

    private static BattleManager instance;
    public static BattleManager Instance {
        get {
            if (instance == null) {
                instance = new BattleManager();
            }
            return instance;
        }
    }

    //战斗格子索引和坐标
    public Dictionary<int, Vector3> BattleBlockPosList = new Dictionary<int, Vector3>();
    //当前关卡ID
    private int CurStageID = 1;
    //当前回合数
    private int CurRound = 1;
    //当前战斗双方角色数据
    private List<BattleRoleData> BattleRoleList = new List<BattleRoleData>();
    //当前回合战斗顺序
    private List<BattleRoleData> CurRoundAtkOderList = new List<BattleRoleData>();
    //当前回合中攻击者序号
    public int CurRoundAtkIndex = 0;
    //当前战斗状态
    public BattleStatus CurBattleStatus = BattleStatus.Idle;

    //初始化战斗双方数据
    public void InitBattleRolesData() {
        BattleRoleList.InsertRange(0, QuerySelfRoleList());
        BattleRoleList.InsertRange(BattleRoleList.Count, QueryEnermyRoleList());
    }

    private List<BattleRoleData> QuerySelfRoleList() {
        List<BattleRoleData> result = new List<BattleRoleData>();
        result.Add(new BattleRoleData(GetBattlePosIndex(true, 2), 1, true));
        result.Add(new BattleRoleData(GetBattlePosIndex(true, 4), 2, true));
        return result;
    }

    private List<BattleRoleData> QueryEnermyRoleList() {
        List<BattleRoleData> result = new List<BattleRoleData>();
        if (ConfigData.Instance.BattleSatgeConfigDict.ContainsKey(CurStageID)) {
            BattleSatgeConfigData curConfigData = ConfigData.Instance.BattleSatgeConfigDict[CurStageID];
            for (int i = 0; i < curConfigData._pos.Length; i++) {
                if (curConfigData._pos[i] > 0) {
                    result.Add(new BattleRoleData(GetBattlePosIndex(false, i + 1), curConfigData._pos[i], false));
                }
            }
        }
        return result;
    }

    public string QueryRoleModelName(int roleId) {
        if (ConfigData.Instance.HeroConfigDict.ContainsKey(roleId)) {
            return "Role_" + ConfigData.Instance.HeroConfigDict[roleId]._modelId;
        } else {
            return "";
        }
    }

    public List<BattleRoleData> GetBattleRoleList() {
        return this.BattleRoleList;
    }

    public int GetBattlePosIndex(bool isSelf, int pos) {
        int[] selfPos = { 11, 12, 13, 1, 2, 3, 21, 22, 23 };
        int[] rivalPos = { 14, 15, 16, 5, 6, 7, 24, 25, 26 };
        return isSelf ? selfPos[pos - 1] : rivalPos[pos - 1];
    }

    //计算本回合攻击顺序
    private void CalCurRoundAtkOderList() {
        this.CurRoundAtkOderList = new List<BattleRoleData>();
        for (int i = 0; i < this.BattleRoleList.Count; i++) {
            if (BattleRoleList[i].CurHp > 0) {
                this.CurRoundAtkOderList.Add(BattleRoleList[i]);
            }
        }
        this.CurRoundAtkOderList.Sort(delegate(BattleRoleData data1, BattleRoleData data2) { return data1.Speed.CompareTo(data2.Speed); });
        string str = "========>>>>CurRound Order(" + this.CurRoundAtkIndex + "): ";
        for (int i = 0; i < CurRoundAtkOderList.Count; i++) {
            str += CurRoundAtkOderList[i].Pos + "(" + CurRoundAtkOderList[i].Speed + "), ";
        }
        //Debug.LogError(str);
    }

    //每回合开始初始化
    public void SetAndInitCurRound(int value) {
        this.CurRound = value;
        CalCurRoundAtkOderList();
    }

    //获取当前攻击者数据
    public BattleRoleData GetCurRoundAtkRoleData() {
        //Debug.LogError("======>>>>>CurRoundAtkIndex:" + this.CurRoundAtkIndex);
        if (this.CurRoundAtkIndex >= CurRoundAtkOderList.Count) {
            return null;
        }
        BattleRoleData data = this.CurRoundAtkOderList[this.CurRoundAtkIndex];
        while (data == null || data.CurHp <= 0) {
            this.CurRoundAtkIndex++;
            if (this.CurRoundAtkIndex >= CurRoundAtkOderList.Count) {
                return null;
            }
            data = this.CurRoundAtkOderList[this.CurRoundAtkIndex];
        }
        return data;
    }

    //获取当前受击者数据
    public BattleRoleData GetCurHurtRoleData(BattleRoleData atkRoleData) {
        bool isSelf = atkRoleData.IsPlayer;
        List<BattleRoleData> rivalList = new List<BattleRoleData>();
        for (int i = 0; i < this.BattleRoleList.Count; i++) {
            if (this.BattleRoleList[i].IsPlayer == !isSelf && this.BattleRoleList[i].CurHp > 0) {
                rivalList.Add(this.BattleRoleList[i]);
            }
        }
        //与最近的敌人之间的距离
        Dictionary<int, List<int>> nearStepDict = SearchPath.GetStepDict(atkRoleData.Pos);
        List<int> step = new List<int>(nearStepDict.Keys);
        step.Sort(delegate(int step1, int step2) { return step1.CompareTo(step2); });
        int shortestStep = 0;
        for (int i = 0; i < step.Count; i++) {
            List<int> curStepPosList = nearStepDict[step[i]];
            for (int j = 0; j < rivalList.Count; j++) {
                for (int m = 0; m < curStepPosList.Count; m++) {
                    if (rivalList[j].Pos == curStepPosList[m]) {
                        shortestStep = step[i];
                        break;
                    }
                }
                if (shortestStep != 0) {
                    break;
                }
            }
            if (shortestStep != 0) {
                break;
            }
        }
        //Debug.LogError("shortestStep:" + shortestStep);        
        if (shortestStep != 0) {
            //根据最短距离搜索敌人列表
            List<BattleRoleData> shortestRoleList = new List<BattleRoleData>();
            for (int i = 0; i < rivalList.Count; i++) {
                for (int j = 0; j < nearStepDict[shortestStep].Count; j++) {
                    if (rivalList[i].Pos == nearStepDict[shortestStep][j]) {
                        shortestRoleList.Add(rivalList[i]);
                        break;
                    }
                }
            }            
            //有多个相同距离的敌人时，选择血量最少的
            List<BattleRoleData> hpRoleList = new List<BattleRoleData>();
            shortestRoleList.Sort(delegate(BattleRoleData data1, BattleRoleData data2) { return data1.CurHp.CompareTo(data2.CurHp); });
            for (int i = 0; i < shortestRoleList.Count; i++) {
                if (shortestRoleList[i].CurHp == shortestRoleList[0].CurHp) {
                    hpRoleList.Add(shortestRoleList[i]);
                }
            }           
            //血量相同时，随机抽取一个
            int index = Random.Range(0, hpRoleList.Count - 1);
            return hpRoleList[index];
        } else {            
            return null;
        }
    }

    //战斗是否结束
    public bool IsBattleOver() {
        bool enermyOver = true;
        for (int i = 0; i < this.BattleRoleList.Count; i++) {
            if (!this.BattleRoleList[i].IsPlayer && this.BattleRoleList[i].CurHp > 0) {
                enermyOver = false;
                break;
            }
        }
        bool selfOver = true;
        for (int i = 0; i < this.BattleRoleList.Count; i++) {
            if (this.BattleRoleList[i].IsPlayer && this.BattleRoleList[i].CurHp > 0) {
                selfOver = false;
                break;
            }
        }
        return enermyOver || selfOver;
    }

    //获取除受击者以外的其余角色占用点
    public List<int> GetLimitePosListBySearchPath(int targetPos) {
        List<int> limitePosList = new List<int>();
        for (int i = 0; i < this.BattleRoleList.Count; i++) {
            if (this.BattleRoleList[i].CurHp > 0 && this.BattleRoleList[i].Pos != targetPos) {
                limitePosList.Add(this.BattleRoleList[i].Pos);
            }
        }
        return limitePosList;
    }

    //根据位置索引获取位置坐标
    public Vector3 QueryPosByIndex(int index) {
        if (this.BattleBlockPosList.ContainsKey(index)) {
            return this.BattleBlockPosList[index];
        }
        return Vector3.zero;
    }

    public bool CanAtk(BattleRoleObj atker, BattleRoleObj hurt) {
        int atkRange = atker.GetData().AtkRange;
        Dictionary<int, List<int>> nearStepDict = SearchPath.GetStepDict(atker.GetData().Pos);
        foreach (int step in nearStepDict.Keys) {
            for (int i = 0; i < nearStepDict[step].Count; i++) {
                if (nearStepDict[step][i] == hurt.GetData().Pos) {
                    return step <= atkRange;
                }
            }
        }
        return false;
    }

    public void CalCurAtkDamage(BattleRoleData curAtkData, BattleRoleData hurtData) {
        hurtData.TempDecHpByHurt = curAtkData.Atk;
    }
    




}
