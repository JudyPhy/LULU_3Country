using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleManager {

    private static BattleManager _instance;
    public static BattleManager Instance {
        get {
            if (_instance == null) {
                _instance = new BattleManager();
            }
            return _instance;
        }
    }

    public Dictionary<BattleRoleType, List<Character>> battleRoleDict = new Dictionary<BattleRoleType, List<Character>>();
    public Dictionary<BattleRoleType, List<Vector3>> bpList = new Dictionary<BattleRoleType, List<Vector3>>();

    public void InitBPList() {
        BattleScene _scene = GameObject.FindObjectOfType<BattleScene>();
        if (_scene != null) {            
            bpList.Add(BattleRoleType.Self, _scene.GetBpList(BattleRoleType.Self));
            bpList.Add(BattleRoleType.Monster, _scene.GetBpList(BattleRoleType.Monster));
            bpList.Add(BattleRoleType.Boss, _scene.GetBpList(BattleRoleType.Boss));
            Debug.LogError("Self:" + bpList[BattleRoleType.Self].Count);
        }
    }

    public List<Vector3> GetBPlist(BattleRoleType type) {
        if (bpList.ContainsKey(type)) {
            return bpList[type];
        }
        return new List<Vector3>();
    }

    public List<CharactorData> GetRoleData() {
        List<CharactorData> result = new List<CharactorData>();
        for (int i = 0; i < 3; i++) {
            CharactorData data = new CharactorData();
            data.roleType = BattleRoleType.Self;
            if (i == 0) {
                data.isSelf = true;
            }
            result.Add(data);
        }
        for (int i = 0; i < 3; i++) {
            CharactorData data = new CharactorData();
            data.roleType = BattleRoleType.Rival;
            result.Add(data);
        }
        return result;
    }

    public Vector3[] GetStartBP() {
        Vector3[] result = new Vector3[2];
        List<Vector3> selfbp = GetBPlist(BattleRoleType.Self);
        if (selfbp.Count > 1) {
            int selfIndex = Random.Range(0, selfbp.Count);
            int rivalIndex = selfIndex + 1 >= selfbp.Count ? selfIndex + 1 - selfbp.Count : selfIndex + 1;
            result[0] = selfbp[selfIndex];
            result[1] = selfbp[rivalIndex];
        }
        return result;
    }  

}
