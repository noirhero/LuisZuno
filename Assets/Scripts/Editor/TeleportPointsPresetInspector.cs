// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TeleportPointsPreset))]
[CanEditMultipleObjects]
public class TeleportPointsPresetInspector : Editor {
    private bool _bConvertData = false;


    public override void OnInspectorGUI() {
        bool cachedState = GUILayout.Toggle(_bConvertData, "ConvertToPosition");
        if (cachedState != _bConvertData) {
            _bConvertData = cachedState;

            if (_bConvertData) {
                ConvertToPosition(target as TeleportPointsPreset);
            }
            else {
                ConvertToGameObject(target as TeleportPointsPreset);
            }
        }

        base.OnInspectorGUI();
    }


    private void ConvertToPosition(TeleportPointsPreset inPreset) {
        inPreset.points.Clear();

        Transform[] childs = inPreset.GetTransform().GetComponentsInChildren<Transform>();
        for (int i = childs.Length-1; i >= 0 ; --i) {
            if (ReferenceEquals(childs[i], inPreset.GetTransform())) {
                continue;
            }

            if (inPreset.points.Find(t => t.position == childs[i].position) == null) {
                inPreset.points.Add(new TeleportPointData(childs[i].name, childs[i].position));
            }
            GameObject.DestroyImmediate(childs[i].gameObject);
        }
    }


    private void ConvertToGameObject(TeleportPointsPreset inPreset) {
        for (int i = inPreset.points.Count-1; i >= 0; --i) {
            GameObject cachedObject = new GameObject(inPreset.points[i].name);

            Transform cachedTrans = cachedObject.transform;
            cachedTrans.SetParent(inPreset.GetTransform());
            cachedTrans.SetPositionAndRotation(inPreset.points[i].position, Quaternion.identity);
        }

        inPreset.points.Clear();
    }
}
