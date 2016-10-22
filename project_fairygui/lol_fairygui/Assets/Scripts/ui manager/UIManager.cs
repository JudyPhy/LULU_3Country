using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;

//=========================================================================================================
//
//逻辑思路：一级窗口：不可堆叠，任何时刻只存在一个。
//             ↓
//             ↓
//          子窗口：隶属于打开其的一级窗口，可堆叠或隐藏显示。
//
//=========================================================================================================

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    public static WindowsBasePanel _curWindow;
    private List<WindowsBasePanel> _beforeWindow = new List<WindowsBasePanel>();

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(this);

        ConfigData.Instance.LoadConfigs();
    }

    public void ShowWindow<T>(string packageName, string componentName) where T : WindowsBasePanel {
        if (_curWindow != null) {
            UIPanel panel = _curWindow.gameObject.GetComponent<UIPanel>();
            if (panel == null) {
                Debug.LogError("Error!!!");
            } else {
                string curPackageName = panel.packageName;
                string curComponentName = panel.componentName;
                if (packageName == curPackageName) {
                    if (componentName == curComponentName) {
                        Debug.LogError("窗口已经打开");
                        return;
                    } else {
                        panel.componentName = componentName;
                        panel.CreateUI();
                        DestroyImmediate(_curWindow);
                        _curWindow = panel.gameObject.AddComponent<T>();
                    }
                } else {
                    UIPackage.AddPackage("UI/" + packageName);
                    _curWindow.gameObject.SetActive(false);
                    _beforeWindow.Add(_curWindow);
                    _curWindow = AddUIPanel(packageName, componentName).AddComponent<T>();
                }
            }
        } else {
            _curWindow = AddUIPanel(packageName, componentName).AddComponent<T>();
        }
    }

    private GameObject AddUIPanel(string packageName, string componentName) {
        GameObject go = new GameObject("UIPanel");
        go.layer = LayerMask.NameToLayer("UI");
        UIPanel panel = go.AddComponent<UIPanel>();
        panel.packageName = packageName;
        panel.componentName = componentName;
        panel.CreateUI();
        return go;
    }

}
