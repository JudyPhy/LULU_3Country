using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateRoleMgr {

    private static CreateRoleMgr instance;
    public static CreateRoleMgr Instance {
        get {
            if (instance == null) {
                instance = new CreateRoleMgr();
            }
            return instance;
        }
    }

    private List<pb.RoleInfo> RoleInfoList = new List<pb.RoleInfo>();

    public void InitRoleInfoList(List<pb.RoleInfo> list) {
        this.RoleInfoList = list;
    }

    public List<pb.RoleInfo> QueryRoleInfoList() {
        return this.RoleInfoList;
    }

    public pb.RoleInfo QueryRoleInfoByType(RoleType type) {
        for (int i = 0; i < this.RoleInfoList.Count; i++) {
            if (this.RoleInfoList[i].RoleType == (int)type) {
                return this.RoleInfoList[i];
            }
        }
        return null;
    }

}
