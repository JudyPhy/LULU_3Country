using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;

public class BattleUI : WindowsBasePanel {
    
    private Joystick _joystick;
    private GameModel _curGameModel;
    private List<Character> _roleObjList = new List<Character>();
    private Transform _roleroot;

    protected override void OnAwake() {
        base.OnAwake();

        _roleroot = new GameObject("Role Root").transform;
        _roleroot.transform.localPosition = Vector3.zero;
        _roleroot.transform.localScale = Vector3.one;

        
        BattleManager.Instance.InitBPList();
        _curGameModel = GameModel.pvp_33;
    }

    protected override void OnStart() {
        base.OnStart();

        _joystick = new Joystick(_mainView);

        StartCoroutine(LoadModel());
    }

    private IEnumerator LoadModel() { 
        yield return StartCoroutine(LoadMonsterModel());
        yield return StartCoroutine(LoadRoleModel());
    }

    private IEnumerator LoadMonsterModel() {
        yield break;
    }

    Vector3[] bp;
    private IEnumerator LoadRoleModel() {
        switch (_curGameModel) {
            case GameModel.pvp_33: {
                    bp = BattleManager.Instance.GetStartBP();
                    List<CharactorData> rolelist = BattleManager.Instance.GetRoleData();
                    for (int i = 0; i < rolelist.Count; i++) {
                        GameObject role = Instantiate(Resources.Load("Model/" + rolelist[i].prefabName)) as GameObject;
                        role.transform.parent = _roleroot;
                        role.transform.localScale = Vector3.one;
                        if (rolelist[i].isSelf) {
                            _roleObjList.Add(role.AddComponent<PlayerCharacter>());
                        } else {
                            _roleObjList.Add(role.AddComponent<OtherPlayerCharacter>());
                        }
                        if (rolelist[i].roleType == BattleRoleType.Self) {
                            role.transform.position = bp[0];
                        } else if (rolelist[i].roleType == BattleRoleType.Rival) {
                            role.transform.position = bp[1];
                        }
                    }
                }
                break;
            default:
                break;
        }

        yield break;
    }

    protected override void OnUpdate() {
        base.OnUpdate();
    }

}
