using UnityEngine;
using System.Collections;
using FairyGUI;

public class MainUI : WindowsBasePanel {

    protected GComponent _mainView;

    private GButton _btnbattle;

    void Awake() {
        if (UIManager._curWindow == null) {
            UIManager._curWindow = this;
        }
    }

    private void OnKeyDown(EventContext context) {
        if (context.inputEvent.keyCode == KeyCode.Escape) {
            Application.Quit();
        }
    }

    void Start() {
        //base.OnStart();

        Application.targetFrameRate = 60;
        Stage.inst.onKeyDown.Add(OnKeyDown);

        GRoot.inst.SetContentScaleFactor(1136, 640);
        _mainView = this.GetComponent<UIPanel>().ui;       

        _btnbattle = _mainView.GetChild("btn").asButton;
        _btnbattle.onClick.Add(() => {
            Debug.LogError("click enter loading");
            LoadingManager.Instance.curLoadingType = LoadingType.ToBattleScene;
            UIManager.Instance.ShowWindow<LoadingUI>("MainUI", "com2");
        });
    }


}
