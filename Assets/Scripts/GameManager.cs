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
        this.MainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        DontDestroyOnLoad(this.MainCamera.gameObject);

        //读取配置表
        StartCoroutine(LoadConfigData()); 
    }

    private IEnumerator LoadConfigData() {
        ConfigData.Instance.LoadConfigs();
        this.LoadConfigOver = true;
        yield return 0;
    }

    public void SwitchScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void OnLevelWasLoaded(int index) {
        if (SceneManager.GetActiveScene().name == "Login") {
            //登录场景
            UIManager.Instance.ShowMainWindow<LoginPanel>(eWindowsID.LoginPanel);
        }
    }

    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (this.LoadConfigOver) {
            this.LoadConfigOver = false;
            //挂载开始界面
            UIManager.Instance.ShowMainWindow<GameStartPanel>(eWindowsID.GameStartPanel);
        }
	}

    public void LoginSuccess() {        
        UIManager.Instance.ShowMainWindow<LoadingUI>(eWindowsID.LoadingUI);
        LoginMsgHandler.Instance.SendMsgC2GSRequestRoleInfo(LocalFile.Load("AccountName"));
    }

}
