// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SceneInfomationPreset))]
[CanEditMultipleObjects]
public class SceneInfomationPresetInspector : Editor {
    private bool _bConvertData = false;


    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        bool cachedState = GUILayout.Toggle(_bConvertData, "ConvertToTeleportPoint");
        if (cachedState != _bConvertData) {
            _bConvertData = cachedState;

            if (_bConvertData) {
                ConvertToPosition(target as SceneInfomationPreset);
            }
            else {
                ConvertToGameObject(target as SceneInfomationPreset);
            }
        }
    }


    private void ConvertToPosition(SceneInfomationPreset inPreset) {
        ConvertToGameObject(inPreset);

        Transform[] childs = inPreset.GetTransform().GetComponentsInChildren<Transform>();
        for (int i = childs.Length-1; i >= 0 ; --i) {
            if (ReferenceEquals(childs[i], inPreset.GetTransform())) {
                continue;
            }

            if (inPreset.IsNewPosition(childs[i].position)) {
                inPreset.points.Add(inPreset.points.Count, new TeleportPointData(childs[i].name, childs[i].position));
            }
            GameObject.DestroyImmediate(childs[i].gameObject);
        }
    }


    private void ConvertToGameObject(SceneInfomationPreset inPreset) {
        for (int i = inPreset.points.Count-1; i >= 0; --i) {
            GameObject cachedObject = new GameObject(inPreset.points[i].name);

            Transform cachedTrans = cachedObject.transform;
            cachedTrans.SetParent(inPreset.GetTransform());
            cachedTrans.SetPositionAndRotation(inPreset.points[i].position, Quaternion.identity);
        }

        inPreset.points.Clear();
    }
}
