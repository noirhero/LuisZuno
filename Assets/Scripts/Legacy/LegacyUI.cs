// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;

public class LegacyUI : MonoBehaviour  {
    private Transform _myTrans;
    public Transform GetTransform() {
        if (null == _myTrans)
        {
            _myTrans = transform;
        }

        return _myTrans;
    }
}
