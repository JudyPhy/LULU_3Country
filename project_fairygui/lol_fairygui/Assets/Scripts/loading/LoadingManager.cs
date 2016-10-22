using UnityEngine;
using System.Collections;

public class LoadingManager {

    private static LoadingManager _instance;
    public static LoadingManager Instance {
        get {
            if (_instance == null) {
                _instance = new LoadingManager();
            }
            return _instance;
        }
    }

    public LoadingType curLoadingType;

}
