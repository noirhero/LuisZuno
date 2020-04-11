// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using GlobalDefine;

public class ScenarioSelectInfo : MonoBehaviour {
    public string displayName;
    
    // 맵정보 방식 정리되기전까지 임시
    public SceneType sceneType;
    public SubSceneType curSubSceneType;
    public SubSceneType nextSubSceneType;
    public int pointID;
    //

    private Transform _myTrans;
    public Transform GetTransform() {
        if (null ==  _myTrans) {
            _myTrans = transform;
        }

        return _myTrans;
    }


    public string GetDisplayName() {
        if (0 <= displayName.Length) {
            return this.name;
        }
        return displayName;
    }


    public Vector3 GetPivot() {
        return GetTransform().position;
    }
}
