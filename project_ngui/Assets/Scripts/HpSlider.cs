using UnityEngine;
using System.Collections;

public class HpSlider : MonoBehaviour {

    private UIWidget Widget;
    private UILabel Label_Name;
    private UISlider Slider;

    private BattleRoleData RoleData;

    private void Awake() {
        this.Widget = this.GetComponent<UIWidget>();
        this.Label_Name = this.transform.FindChild("Name").GetComponent<UILabel>();
        this.Slider = this.transform.FindChild("Slider").GetComponent<UISlider>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float curValue = (this.RoleData.CurHp + 0.00f) / this.RoleData.MaxHp;
        if (this.Slider.value != curValue) {
            this.Slider.value = curValue;
        }
	}

    public void Init(BattleRoleData data) {
        this.RoleData = data;
        this.Slider.value = (data.CurHp + 0.00f) / data.MaxHp;
        HeroConfigData cfg = ConfigDataManager.QueryHeroCfgByID(data.RoleID);
        if (cfg != null) {
            this.Label_Name.text = cfg._name;
        }
    }

    public BattleRoleData GetData() {
        return this.RoleData;
    }

    public void SetPos(bool isLeft, float x, float y) {
        if (isLeft) {
            this.Label_Name.pivot = UIWidget.Pivot.Left;
            this.Label_Name.alignment = NGUIText.Alignment.Left;
            this.Slider.transform.localPosition = this.Label_Name.transform.localPosition + new Vector3(this.Label_Name.width + 10, 0, 0);
            this.Widget.pivot = UIWidget.Pivot.TopLeft;
        } else {
            this.Label_Name.pivot = UIWidget.Pivot.Right;
            this.Label_Name.alignment = NGUIText.Alignment.Right;
            this.Slider.transform.localPosition = this.Label_Name.transform.localPosition - new Vector3(this.Label_Name.width + 10 + this.Slider.backgroundWidget.width, 0, 0);
            this.Widget.pivot = UIWidget.Pivot.TopRight;
        }
        this.transform.localPosition = new Vector3(x, y, 0);
    }

}
