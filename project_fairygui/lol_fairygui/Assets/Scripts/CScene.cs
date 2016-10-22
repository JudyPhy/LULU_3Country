using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CScene : MonoBehaviour {
    
    void Awake() {
    }

    public List<GameObject> GetBPList(BattleRoleType type) {
        List<GameObject> result = new List<GameObject>();
        string bproot = Define.GetRootName(type);
        Transform parent = transform.FindChild(bproot);
        if (parent != null) {
            for (int i = 0; i < parent.childCount; i++) {
                result.Add(parent.GetChild(i).gameObject);
            }
        }
        result.Sort(delegate(GameObject obj1, GameObject obj2) { return obj1.name.CompareTo(obj2.name); });
        return result;
    }

    public void AddBPObj(int index, BattleRoleType type) {
        string bproot = Define.GetRootName(type);
        Transform parent = transform.FindChild(bproot);
        if (parent == null) {
            parent = new GameObject(bproot).transform;
            parent.parent = this.transform;
            parent.localPosition = Vector3.zero;
            parent.localScale = Vector3.one;
        }
        GameObject bp = new GameObject("bp_" + index.ToString("00"));
        bp.transform.parent = parent;
        bp.transform.localPosition = Vector3.zero;
        bp.transform.localScale = Vector3.one;
    }

    public void RemoveBpObj(GameObject bpObj, BattleRoleType type) {
        string bproot = Define.GetRootName(type);
        Transform parent = transform.FindChild(bproot);
        if (parent != null) {
            for (int i = 0; i < parent.childCount; i++) {
                GameObject obj = parent.GetChild(i).gameObject;
                if (obj == bpObj) {
                    DestroyImmediate(obj);
                    break;
                }
            }
            //update obj name
            for (int i = 0; i < parent.childCount; i++) {
                GameObject obj = parent.GetChild(i).gameObject;
                obj.name = "bp_" + i.ToString("00");
            }
        }
    }

}
