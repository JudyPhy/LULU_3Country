using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectRolePanel : WindowsBasePanel {

    private UIGrid RolesGrid;
    private List<SelectRoleItem> RoleItemList = new List<SelectRoleItem>();

    public override void OnAwake() {        
        base.OnAwake();

        Transform centerAnchor = this.transform.FindChild("CenterAnchor");
        this.RolesGrid = centerAnchor.FindChild("ChoosePanel/Grid").GetComponent<UIGrid>();

    }
    public override void OnInitWindow() {
        base.OnInitWindow();

        UpdateRoleListUI();
    }

    private void UpdateRoleListUI() {
        HideAllItem();
        List<HeroConfigData> heroList = new List<HeroConfigData>(ConfigData.Instance.HeroConfigDict.Values);
        for (int i = 0; i < heroList.Count; i++) {
            SelectRoleItem script = QueryItemByIndex(i);
            if (script != null) {
                script.UpdateUI(heroList[i]);
            }
        }
        this.RolesGrid.repositionNow = true;
    }

    private void HideAllItem() {
        for (int i = 0; i < this.RoleItemList.Count; i++) {
            this.RoleItemList[i].gameObject.SetActive(false);
        }
    }

    private SelectRoleItem QueryItemByIndex(int index) {
        if (index < this.RoleItemList.Count) {
            this.RoleItemList[index].gameObject.SetActive(true);
            return this.RoleItemList[index];
        } else {
            string prefabPath = ResourcesManager.Instance.GetResPath("SelecteRoleItem");
            SelectRoleItem item = UIManager.Instance.AddItemToList<SelectRoleItem>(prefabPath, this.RolesGrid.gameObject);
            this.RoleItemList.Add(item);
            return item;
        }
    }

    public override void OnStart() {
        base.OnStart();
    }

    public override void OnUpdate() {
        base.OnUpdate();
    }

}
