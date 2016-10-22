using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattlePanel : WindowsBasePanel {  

    private GameObject ButtonFight;
    private GameObject HpSliderContainer;
    private List<HpSlider> HpSliderList = new List<HpSlider>();
    private Dictionary<int, GameObject> MapBlockDict = new Dictionary<int, GameObject>();
    private List<BattleRoleObj> RoleObjList = new List<BattleRoleObj>();

    private int CurRound = 1;
    private BattleRoleObj AtkRoleObj;
    private BattleRoleObj HurtRoleObj;

    public override void OnAwake() {
        this.ButtonFight = this.transform.FindChild("Button").gameObject;
        this.ButtonFight.SetActive(false);
        this.HpSliderContainer = this.transform.FindChild("HpSliderContainer").gameObject;
        this.ButtonFight.SetActive(true);
        UIEventListener.Get(this.ButtonFight).onClick += OnClickStartFight;

    }

    public override void OnStart() {
        InitBattleBlocks();
        InitRoles();
    }

    //画格子
    private void InitBattleBlocks() {
        GameObject battleBlockParent = new GameObject("BattleBlocks");
        Vector3[] pos = { new Vector3(0, 0.66f, 0), new Vector3(0.71f, 1.88f, 0), new Vector3(0.71f, -0.56f, 0) };
        for (int line = 0; line < 3; line++) {
            GameObject lineObj = Instantiate(Resources.Load("Line")) as GameObject;
            lineObj.transform.parent = battleBlockParent.transform;
            lineObj.transform.localPosition = pos[line];
            int endIndex = line > 0 ? 7 : 8;
            for (int i = 1; i <= 7; i++) {
                GameObject block = lineObj.transform.FindChild("0" + i.ToString()).gameObject;
                block.name = (line * 10 + i).ToString();
                if (line > 0 && i == 7) {
                    block.SetActive(false);
                    continue;
                }
                MapBlockDict.Add(line * 10 + i, block);
                float xStart = -4.25f;
                float z = 0.7f;
                if (line == 2) {
                    xStart = -3.52f;
                    z = -0.55f;
                } else if (line == 1) {
                    xStart = -3.52f;
                    z = 1.86f;
                }
                BattleManager.Instance.BattleBlockPosList.Add(line * 10 + i, new Vector3(xStart + (i - 1) * 1.42f, 0f, z));
            }
        }
        battleBlockParent.transform.localEulerAngles = new Vector3(90, 0, 0);
    }

    //摆放角色
    private void InitRoles() {
        BattleManager.Instance.InitBattleRolesData();
        GameObject RoleParent = new GameObject("BattleRoles");
        List<BattleRoleData> roleList = BattleManager.Instance.GetBattleRoleList();
        for (int i = 0; i < roleList.Count; i++) {
            bool isFind = false;
            for (int j = 0; j < this.RoleObjList.Count; j++) {
                if (this.RoleObjList[j].GetData().Pos == roleList[i].Pos) {
                    isFind = true;
                    Debug.LogError("角色位置重复");
                    break;
                }
            }
            if (!isFind) {
                string modelName = BattleManager.Instance.QueryRoleModelName(roleList[i].RoleID);
                if (!string.IsNullOrEmpty(modelName)) {
                    GameObject obj = Instantiate(Resources.Load("Prefabs/Model/" + modelName)) as GameObject;
                    if (obj != null) {
                        obj.transform.parent = RoleParent.transform;
                        int posIndex = roleList[i].Pos;
                        obj.name = posIndex.ToString();
                        obj.transform.localPosition = BattleManager.Instance.QueryPosByIndex(posIndex);
                        BattleRoleObj script = obj.AddComponent<BattleRoleObj>();
                        script.InitRole(roleList[i]);
                        this.RoleObjList.Add(script);
                        CreateNewHpSlider(roleList[i]);
                    } else {
                        Debug.LogError("实例化模型" + modelName + "失败.");
                    }
                }
            }
        }
        SetHpSliderPos();
    }

    //摆放血条
    private void SetHpSliderPos() {
        Debug.LogError("Screen.width:" + Screen.width + "  Screen.height:" + Screen.height);
        float x_left = -Screen.width / 2;
        float x_right = Screen.width / 2;
        float y_left = Screen.height / 2;
        float y_right = Screen.height / 2;
        for (int i = 0; i < this.HpSliderList.Count; i++) {
            if (this.HpSliderList[i].GetData().IsPlayer) {
                this.HpSliderList[i].SetPos(true, x_left, y_left);
                y_left -= 15;
            } else {
                this.HpSliderList[i].SetPos(false, x_right, y_right);
                y_right -= 15;
            }
        }
    }

    private void CreateNewHpSlider(BattleRoleData data) {
        GameObject obj = Instantiate(Resources.Load("HpSlider")) as GameObject;
        if (obj != null) {
            obj.transform.parent = this.HpSliderContainer.transform;
            HpSlider script = obj.AddComponent<HpSlider>();
            script.Init(data);
            this.HpSliderList.Add(script);
        } else {
            Debug.LogError("实例化血条失败.");
        }
    }

    private void OnClickStartFight(GameObject go) {
        this.ButtonFight.SetActive(false);
        this.CurRound = 0;
        BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewRound;
    }

    private void NextRound() {
        //Debug.LogError("新一轮：" + this.CurRound);        
        BattleManager.Instance.CurBattleStatus = BattleStatus.Idle;
        this.CurRound++;
        BattleManager.Instance.SetAndInitCurRound(this.CurRound);
        BattleManager.Instance.CurRoundAtkIndex = -1;
        BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewAtkByCurRound;
    }

    private void NextAtkByCurRound() {
        BattleManager.Instance.CurBattleStatus = BattleStatus.Idle;
        this.AtkRoleObj = null;
        this.HurtRoleObj = null;
        BattleManager.Instance.CurRoundAtkIndex++;
        BattleRoleData curAtkData = BattleManager.Instance.GetCurRoundAtkRoleData();
        if (curAtkData != null) {
            this.AtkRoleObj = GetRoleObjByPosIndex(curAtkData.Pos);
            if (this.AtkRoleObj != null) {
                ShineHpSlider(curAtkData.Pos);
                BattleRoleData hurtData = BattleManager.Instance.GetCurHurtRoleData(curAtkData);
                if (hurtData != null) {
                    this.HurtRoleObj = GetRoleObjByPosIndex(hurtData.Pos);
                    if (this.HurtRoleObj != null) {
                        //Debug.LogError("curAtkData:" + curAtkData.QueryPos() + "  hurtData:" + hurtData.QueryPos());
                        BattleManager.Instance.CalCurAtkDamage(curAtkData, hurtData);
                        BattleManager.Instance.CurBattleStatus = BattleStatus.BattleAtkerMove;
                    } else {
                        Debug.LogError("受击者角色不存在，索引号:" + hurtData.Pos);
                        BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewAtkByCurRound;
                    }
                } else {
                    //Debug.LogError("搜索打击目标失败");
                    BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewAtkByCurRound;
                }
            } else {
                Debug.LogError("当前攻击角色不存在，索引号:" + BattleManager.Instance.CurRoundAtkIndex);
                BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewAtkByCurRound;
            }
        } else {
            if (BattleManager.Instance.IsBattleOver()) {
                Debug.LogError("战斗结束");
                BattleManager.Instance.CurBattleStatus = BattleStatus.BattleOver;
            } else {
                BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewRound;
            }
        }
    }

    private void ShineHpSlider(int pos) {
        for (int i = 0; i < this.HpSliderList.Count; i++) {
            if (this.HpSliderList[i].GetData().Pos == pos) {
                
            }
        }
    }

    private BattleRoleObj GetRoleObjByPosIndex(int roleObjIndex) {
        for (int i = 0; i < this.RoleObjList.Count; i++) {
            if (this.RoleObjList[i].GetData().Pos == roleObjIndex) {
                return this.RoleObjList[i];
            }
        }
        return null;
    }

    void Update() {
        if (BattleManager.Instance.CurBattleStatus == BattleStatus.BattleNewRound) {
            NextRound();
        } else if (BattleManager.Instance.CurBattleStatus == BattleStatus.BattleNewAtkByCurRound) {
            NextAtkByCurRound();
        } else if (BattleManager.Instance.CurBattleStatus == BattleStatus.BattleOver) {
            Debug.LogError("=========================>>>>>>>>>>Game Over!!!");
            BattleManager.Instance.CurBattleStatus = BattleStatus.Idle;
        } else if (BattleManager.Instance.CurBattleStatus == BattleStatus.BattleAtkerMove) {
            CheckAtkMove();         
        } else if (BattleManager.Instance.CurBattleStatus == BattleStatus.BattleAtkerAtk) {
            CheckAtkerAtk();
        } else if (BattleManager.Instance.CurBattleStatus == BattleStatus.BattleHurt) {
            BattleManager.Instance.CurBattleStatus = BattleStatus.Idle;
            StartCoroutine(this.HurtRoleObj.BeHurt());
        }
    }

    private void CheckAtkMove() {
        BattleManager.Instance.CurBattleStatus = BattleStatus.Idle;
        if (BattleManager.Instance.CanAtk(this.AtkRoleObj, this.HurtRoleObj)) {
            //进入攻击环节
            //Debug.LogError("攻击环节");
            BattleManager.Instance.CurBattleStatus = BattleStatus.BattleAtkerAtk;
        } else {
            //进入移动环节
            //Debug.LogError("移动环节");
            this.AtkRoleObj.MoveStart(this.HurtRoleObj);
        }
    }

    private void CheckAtkerAtk() {
        BattleManager.Instance.CurBattleStatus = BattleStatus.Idle;
        if (BattleManager.Instance.CanAtk(this.AtkRoleObj, this.HurtRoleObj)) {
            //播放攻击
            //Debug.LogError("播放攻击");
            StartCoroutine(this.AtkRoleObj.AtkStart(this.HurtRoleObj));
        } else {
            //下一位
            //Debug.LogError("不能攻击，进入下一位");
            BattleManager.Instance.CurBattleStatus = BattleStatus.BattleNewAtkByCurRound;
        }
    }

}
