using UnityEngine;
using System.Collections;

public class ConfigDataManager {

    public static HeroConfigData QueryHeroCfgByID(int id) {
        if (ConfigData.Instance.HeroConfigDict.ContainsKey(id)) {
            return ConfigData.Instance.HeroConfigDict[id];
        }
        return null;
    }

    


}
