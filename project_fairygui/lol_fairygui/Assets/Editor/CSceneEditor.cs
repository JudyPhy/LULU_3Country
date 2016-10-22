using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CScene))]
public class CSceneEditor : Editor {

    public override void OnInspectorGUI() {
        CScene scene = (CScene)target;

        //===========>>>>>>Player
        GUILayout.Label("BP Setting(Self)");

        //search existed bp in this scene
        List<GameObject> _bplist = scene.GetBPList(BattleRoleType.Self);

        //draw editor
        for (int i = 0; i < _bplist.Count; i++) {
            GUILayout.Label(_bplist[i].name + ":");
            EditorGUILayout.BeginHorizontal();
            _bplist[i].transform.localPosition = EditorGUILayout.Vector3Field("", _bplist[i].transform.localPosition);
            if (GUILayout.Button("Remove")) {
                scene.RemoveBpObj(_bplist[i], BattleRoleType.Self);
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Add")) {
            scene.AddBPObj(_bplist.Count, BattleRoleType.Self);
            Repaint();
        }

        //=========>>>>>>>Monster
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("BP Setting(Monster)");

        List<GameObject> _bplist_monster = scene.GetBPList(BattleRoleType.Monster);
        for (int i = 0; i < _bplist_monster.Count; i++) {
            GUILayout.Label(_bplist_monster[i].name + ":");
            EditorGUILayout.BeginHorizontal();
            _bplist_monster[i].transform.localPosition = EditorGUILayout.Vector3Field("", _bplist_monster[i].transform.localPosition);
            if (GUILayout.Button("Remove")) {
                scene.RemoveBpObj(_bplist_monster[i], BattleRoleType.Monster);
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Add")) {
            scene.AddBPObj(_bplist_monster.Count, BattleRoleType.Monster);
            Repaint();
        }

        //=========>>>>>>>Boss
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.Label("BP Setting(Boss)");

        List<GameObject> _bplist_boss = scene.GetBPList(BattleRoleType.Boss);
        for (int i = 0; i < _bplist_boss.Count; i++) {
            GUILayout.Label(_bplist_boss[i].name + ":");
            EditorGUILayout.BeginHorizontal();
            _bplist_boss[i].transform.localPosition = EditorGUILayout.Vector3Field("", _bplist_boss[i].transform.localPosition);
            if (GUILayout.Button("Remove")) {
                scene.RemoveBpObj(_bplist_boss[i], BattleRoleType.Boss);
                Repaint();
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Add")) {
            scene.AddBPObj(_bplist_boss.Count, BattleRoleType.Boss);
            Repaint();
        }

    }


}
