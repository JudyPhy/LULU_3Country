using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using FairyGUI;

public class WindowsBasePanel : MonoBehaviour {

    public UIWindowsData WindowData_;
    public int Depth_ = 0;
    public System.DateTime CloseTime_;

    protected GComponent _mainView;

    void Awake() {
        OnAwake();
    }

    void OnEnable() {
        OnInitWindow();
    }

    // Use this for initialization
    void Start() {
        OnStart();
    }

    // Update is called once per frame
    void Update() {
        OnUpdate();
    }

    void OnDisable() {

    }

    void OnDestroy() {

    }

    protected virtual void OnAwake() {

    }

    //每次激活窗口时执行
    protected virtual void OnInitWindow() {

    }

    //打开窗口时执行
    protected virtual void OnStart() {
        Application.targetFrameRate = 60;
        Stage.inst.onKeyDown.Add(OnKeyDown);

        GRoot.inst.SetContentScaleFactor(1136, 640);
        _mainView = this.GetComponent<UIPanel>().ui;
    }

    private void OnKeyDown(EventContext context) {
        if (context.inputEvent.keyCode == KeyCode.Escape) {
            Application.Quit();
        }
    }

    protected virtual void OnUpdate() {

    }

    public void CloseWindow() {
        this.CloseTime_ = System.DateTime.Now;
        this.gameObject.SetActive(false);
    }

    //窗口事件
    public virtual void RevWindowEvent(WindowEvent windowEvent, params object[] args) { 
    
    }




}
