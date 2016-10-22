using UnityEngine;
using System.Collections;

public class SelectRoleItem : MonoBehaviour {

    private UILabel Name;
    private UITexture RoleImage;

    private HeroConfigData Data;

    void Awake() {
        this.Name = this.transform.FindChild("Name").GetComponent<UILabel>();
        this.RoleImage = this.transform.FindChild("HalfRoleIcon").GetComponent<UITexture>();
        UIEventListener.Get(this.gameObject).onClick = OnSelectRole;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateUI(HeroConfigData data) {
        this.Data = data;
        this.Name.text = this.Data._name;
        Texture tex = Resources.Load("Atlas/HaflHeadIcon/role" + this.Data._id.ToString("00")) as Texture;
        this.RoleImage.mainTexture = tex;
        this.RoleImage.MakePixelPerfect();
        this.RoleImage.transform.localScale = Vector3.one * 0.5f;
    }

    private void OnSelectRole(GameObject go) {
        UIManager.Instance.ShowWindow<OfflineBattlePanel>(eWindowsID.OfflineBattle);
    }

}
