using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleScene : MonoBehaviour {

    void Awake() {
    }

	// Use this for initialization
	void Start () {
	}

    public List<Vector3> GetBpList(BattleRoleType type) {
        List<Vector3> result = new List<Vector3>();
        string pbroot = Define.GetRootName(type);
        Transform bproot = this.transform.FindChild(pbroot);
        if (bproot != null) {
            for (int i = 0; i < bproot.childCount; i++) {
                result.Add(bproot.GetChild(i).position);
            }
        }
        return result;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
