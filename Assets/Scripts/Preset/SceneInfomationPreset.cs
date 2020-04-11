// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Unity.Mathematics;
using GlobalDefine;

[Serializable]
public class TeleportPointData {
    public string name;
    public Vector3 position;


    public TeleportPointData(string inName, Vector3 inPos){
        name = inName;
        position = inPos;
    }
}

[Serializable]
public class TeleportPointDataDictionary : SerializableDictionaryBase<int, TeleportPointData> {
}

public class SceneInfomationPreset : MonoBehaviour {
    public SceneType sceneType;
    public TeleportPointDataDictionary points = new TeleportPointDataDictionary();

    private Transform _myTrans;
    public Transform GetTransform() {
        if (null == _myTrans) {
            _myTrans = transform;
        }

        return _myTrans;
    }


    private void OnDrawGizmosSelected() {
        for (int i = 0; i < points.Count; ++i) {
            Gizmos.DrawWireSphere(
                GetTransform().position + points[i].position, 
                GetTransform().lossyScale.x);
        }
    }


    public float3 GetPoint(int inID) {
        if (false == points.ContainsKey(inID)) {
            return new float3();
        }
        return new float3(points[inID].position.x, points[inID].position.y, points[inID].position.z);
    }


    public bool IsNewPosition(Vector3 inPos) {
        foreach (var data in points) {
            if (inPos == data.Value.position) {
                return false;
            }
        }

        return true;
    }
}
