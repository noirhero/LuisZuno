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
    
    
    public virtual void Initialize() { }

    
    public virtual void Show() {
        gameObject.SetActive(true);
    }

    
    public virtual void Hide() {
        gameObject.SetActive(false);
    }
}
