// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using GlobalDefine;
using UnityEngine.UI;

public class MadnessUI : LegacyUI {
    public Slider gauge;


    private void Update() {
        PlayerStatusComponent statusComp = Utility._entityMng.GetComponentData<PlayerStatusComponent>(Utility._playerEntity);
        SetGauge(statusComp.status.madness / statusComp.maxMadness);
    }


    protected void SetGauge(float inValue) {
        gauge.value = inValue;
    }
}
