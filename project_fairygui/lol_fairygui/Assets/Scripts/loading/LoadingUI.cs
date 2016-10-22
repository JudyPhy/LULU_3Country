using UnityEngine;
using System.Collections;
using FairyGUI;
using UnityEngine.SceneManagement;

public class LoadingUI : WindowsBasePanel {

    private GProgressBar _progressBar;
    private float _value;

    protected override void OnAwake() {
        base.OnAwake();

        DontDestroyOnLoad(this);
    }

    protected override void OnStart() {
        base.OnStart();

        _progressBar = _mainView.GetChild("pb").asProgress;
        StartCoroutine(DoLoad("battle"));
    }

    private IEnumerator DoLoad(string sceneName) {
        _progressBar.value = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        float startTime = Time.time;
        _value = 0;
        while (!op.isDone) {
            yield return null;
        }
        _value += 100;
        while (_progressBar.value != 100) {
            yield return null;
        }
        UIManager.Instance.ShowWindow<BattleUI>("Joystick_test", "com1");
    }

    // Update is called once per frame
    void Update() {
        _progressBar.value = _value;
    }
}
