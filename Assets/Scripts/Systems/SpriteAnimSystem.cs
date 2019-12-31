// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;

public class SpriteAnimSystem : ComponentSystem {
    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        Entities.ForEach((SpritePresetComponent presetComp, ref SpriteAnimComponent animComp) => {
            if (false == presetComp.preset.datas.TryGetValue(animComp.nameHash, out var presetData)) {
                return;
            }

            animComp.accumTime += deltaTime;
            if (presetData.length <= animComp.accumTime) {
                animComp.accumTime %= presetData.length;
            }
            animComp.frame = (int)(animComp.accumTime / presetData.frameRate);
        });
    }
}
