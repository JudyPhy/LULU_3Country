using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public Camera MainCamera;

    private bool LoadConfigOver = false;
   
    private void Awake() {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        //3D摄像机
        DontDestroyOnLoad(this.MainCamera.gameObject);
    }

    public void SwitchScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void OnLevelWasLoaded(int index) {
        if (SceneManager.GetActiveScene().name == "Login") {
            //登录场景
            UIManager.Instance.ShowWindow<LoginPanel>(eWindowsID.LoginPanel);
        } else if (SceneManager.GetActiveScene().name == "Main") {
            UIManager.Instance.ShowWindow<MainPanel>(eWindowsID.MainUI);
        }
    }

	void Start () {
        //读取配置表
        StartCoroutine(LoadConfigData());
	}

    private IEnumerator LoadConfigData() {
        ConfigData.Instance.LoadConfigs();
        this.LoadConfigOver = true;
        yield return 0;
    }
	
	void Update () {
        if (this.LoadConfigOver) {
            this.LoadConfigOver = false;
            //挂载开始界面
            UIManager.Instance.ShowWindow<MainPanel>(eWindowsID.MainUI);
        }
	}

    public void LoginSuccess() {
        UIManager.Instance.ShowWindow<LoadingUI>(eWindowsID.LoadingUI);
        LoginMsgHandler.Instance.SendMsgC2GSRequestRoleInfo(LocalFile.Load("AccountName"));
    }

}
