using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//===========================================
//    1 2 3 1 2 3        11 12 13 14 15 16 
//   4 5 6 0 4 5 6      01 02 03 04 05 06 07
//    7 8 9 7 8 9        21 22 23 24 25 26 
//===========================================

public class SearchPath {



    public static Dictionary<int, List<int>> GetStepDict(int startPos) {
        Dictionary<int, List<int>> posStepDict = new Dictionary<int, List<int>>();
        int step = 0;
        List<int> centerPosList = new List<int>() { startPos };
        posStepDict.Add(step, centerPosList);
        step++;
        while (true) {
            List<int> nearPosList = NearPosList(centerPosList, posStepDict, new List<int>());
            if (nearPosList.Count > 0 || step < 8) {
                posStepDict.Add(step, nearPosList);
                step++;
                centerPosList = nearPosList;
            } else {
                break;
            }
        }
        return posStepDict;
    }

    public static List<int> QueryPath(int startPos, int targetPos, List<int> limitePosList) {
        //Debug.LogError("startPos:" + startPos + "   targetPos:" + targetPos);
        Dictionary<int, List<int>> posStepDict = new Dictionary<int, List<int>>();
        int step = 0;
        List<int> centerPosList = new List<int>() { startPos };
        posStepDict.Add(step, centerPosList);
        step++;
        while (true) {
            //Debug.LogError("====================>>>>>step:" + step);
            List<int> nearPosList = NearPosList(centerPosList, posStepDict, limitePosList);
            if (nearPosList.Count > 0) {
                posStepDict.Add(step, nearPosList);
                if (FindTarget(nearPosList, targetPos)) {
                    break;
                }
                step++;
                centerPosList = nearPosList;
            } else {
                Debug.LogError("路径被堵死，无法移动到目的地");
                break;
            }
        }
        List<int> path = GetPath(posStepDict, targetPos, limitePosList);
        return path;
    }

    private static bool FindTarget(List<int> list, int targetPos) {
        for (int i = 0; i < list.Count; i++) {
            if (list[i] == targetPos) {
                return true;
            }
        }
        return false;
    }

    private static List<int> NearPosList(List<int> centerPosList, Dictionary<int, List<int>> dict, List<int> limitePosList) {
        List<int> nearPosList = new List<int>();
        for (int i = 0; i < centerPosList.Count; i++) {
            List<int> rangePos = QueryRangePos(centerPosList[i]);   //相邻点列表
            //Debug.LogError("中心点：" + centerPosList[i]);
            //排除已经存储过的点
            for (int j = 0; j < rangePos.Count; j++) {
                if (IsInStepDict(rangePos[j], dict)) {
                    rangePos.RemoveAt(j);
                    j--;
                }
            }
            for (int j = 0; j < rangePos.Count; j++) {
                for (int m = 0; m < nearPosList.Count; m++) {
                    if (rangePos[j] == nearPosList[m]) {
                        rangePos.RemoveAt(j);
                        j--;
                        break;
                    }
                }
            }
            //排除限制点
            for (int j = 0; j < rangePos.Count; j++) {
                for (int m = 0; m < limitePosList.Count; m++) {
                    if (rangePos[j] == limitePosList[m]) {
                        rangePos.RemoveAt(j);
                        j--;
                        break;
                    }
                }
            }
            //string str="";
            //for (int n = 0; n < rangePos.Count; n++) {
            //    str += rangePos[n] + ", ";
            //}
            //Debug.LogError("排除之后的最终周围点：" + str);
            nearPosList.InsertRange(nearPosList.Count, rangePos);
        }
        return nearPosList;
    }

    //获取目标周围点列表
    private static List<int> QueryRangePos(int pos) {
        List<int> rangePos1 = new List<int>() { pos - 1, pos - 10, pos - 9, pos + 1 };
        List<int> rangePos2 = new List<int>() { pos - 1, pos + 9, pos + 10, pos + 1, pos + 20, pos + 19 };
        List<int> rangePos3 = new List<int>() { pos - 1, pos - 20, pos - 19, pos + 1 };
        List<int> rangePos = null;
        if (pos / 10 == 1) {
            rangePos = rangePos1;
        } else if (pos / 10 == 0) {
            rangePos = rangePos2;
        } else if (pos / 10 == 2) {
            rangePos = rangePos3;
        } else {
            Debug.LogError("over 3 row");
            return null;
        }
        for (int j = 0; j < rangePos.Count; j++) {
            if ((rangePos[j] / 10 == 1 && (rangePos[j] % 10 < 1 || rangePos[j] % 10 > 6))
                || (rangePos[j] / 10 == 0 && (rangePos[j] % 10 < 1 || rangePos[j] % 10 > 7))
                || (rangePos[j] / 10 == 2 && (rangePos[j] % 10 < 1 || rangePos[j] % 10 > 6))) {
                rangePos.RemoveAt(j);
                j--;
            }
        }        
        return rangePos;
    }

    private static bool IsInStepDict(int pos, Dictionary<int, List<int>> dict) {
        foreach (List<int> list in dict.Values) {
            for (int i = 0; i < list.Count; i++) {
                if (list[i] == pos) {
                    return true;
                }
            }
        }
        return false;
    }

    private static List<int> GetPath(Dictionary<int, List<int>> dict, int targetPos, List<int> limitePosList) {
        int targetStep = GetStepByPos(dict, targetPos);
        if (targetStep != -1) {
            //Debug.LogError("可到达目标点");
            List<int> path = new List<int>();
            path.Add(targetPos);
            int centerPos = targetPos;
            List<int> shortestStepPosList = null;
            while (true) {
                int curStep = GetStepByPos(dict, centerPos);
                //Debug.LogError("centerPos:" + centerPos + "   curStep:" + curStep);
                if (curStep <= 0) {
                    break;
                }
                shortestStepPosList = GetRangeShortestStepPosList(centerPos, dict, limitePosList);
                centerPos = shortestStepPosList[0];
                path.Add(centerPos);
            }
            path.Reverse();
            return path;
        } else {
            //Debug.LogError("无法到达目标点");
            List<int> path = new List<int>();
            List<int> stepList = new List<int>(dict.Keys);
            stepList.Sort(delegate(int step1, int step2) { return step1.CompareTo(step2); });
            for (int i = 0; i < stepList.Count; i++) {
                int step = stepList[i];
                path.Add(dict[step][0]);
            }
            return path;
        }
    }

    //获取某个点的周围点中，步数最短的点的集合
    private static List<int> GetRangeShortestStepPosList(int centerPos, Dictionary<int, List<int>> dict, List<int> limitePosList) {
        List<int> rangePos = QueryRangePos(centerPos);
        List<int> stepList = new List<int>();
        for (int i = 0; i < rangePos.Count; i++) {
            int step = GetStepByPos(dict, rangePos[i]);
            if (step >= 0) {
                stepList.Add(GetStepByPos(dict, rangePos[i]));
            }
        }
        int minStep = stepList[0];
        for (int i = 1; i < stepList.Count; i++) {
            if (stepList[i] < minStep) {
                minStep = stepList[i];
            }
        }
        List<int> result = new List<int>();
        for (int i = 0; i < rangePos.Count; i++) {
            int step = GetStepByPos(dict, rangePos[i]);
            if (step == minStep) {
                result.Add(rangePos[i]);
            }
        }
        return result;
    }

    //获取某个点的步数
    private static int GetStepByPos(Dictionary<int, List<int>> dict, int pos) {
        foreach (int step in dict.Keys) {
            for (int i = 0; i < dict[step].Count; i++) {
                if (pos == dict[step][i]) {
                    return step;
                }
            }
        }
        return -1;
    }

}
