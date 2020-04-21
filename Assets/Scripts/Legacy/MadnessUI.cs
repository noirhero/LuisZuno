// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using GlobalDefine;
using UnityEngine.UI;

public class MadnessUI : LegacyUI {
    public Slider gauge;


    private void Update() {
        PlayerStatusComponent statusComp = Utility.entityMng.GetComponentData<PlayerStatusComponent>(Utility.playerEntity);
        SetGauge(statusComp.status.madness / statusComp.maxMadness);
    }


    protected void SetGauge(float inValue) {
        gauge.value = inValue;
    }
}
