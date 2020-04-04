// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class TeleportPointData {
    public string name { get; }
    public Vector3 position { get; }
    
    public TeleportPointData(string inName, Vector3 inPos){
        name = inName;
        position = inPos;
    }
}


public class TeleportPointsPreset : MonoBehaviour {
    public List<TeleportPointData> points = new List<TeleportPointData>();
    private Transform _myTrans;

    public Transform GetTransform() {
        if (null == _myTrans) {
            _myTrans = transform;
        }

        return _myTrans;
    }

    private void OnDrawGizmosSelected() {
        Transform myTrans = GetTransform();
        for (int i = 0; i < points.Count; ++i) {
            Gizmos.DrawWireCube(myTrans.position + points[i].position, myTrans.lossyScale);
        }
    }
}
