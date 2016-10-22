using UnityEngine;
using System.Collections;

public class ChapterItem : MonoBehaviour {

    private UISprite ChapterIcon;
    private UILabel ChapterName;

    void Awake() {
        this.ChapterIcon = this.transform.FindChild("Image").GetComponent<UISprite>();
        this.ChapterName = this.transform.FindChild("Name").GetComponent<UILabel>();
        UIEventListener.Get(this.gameObject).onClick = OnSelectChapter;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnSelectChapter(GameObject go) {
        UIManager.Instance.ShowWindow<PveStagePanel>(eWindowsID.StageUI);
    }

}
