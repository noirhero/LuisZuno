// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;

public class SpriteAnimSystem : ComponentSystem {
    protected override void OnUpdate() {
        var deltaTime = Time.deltaTime;

        Entities.ForEach((SpriteMeshComponent meshComp, ref SpriteAnimComponent animComp) => {
            animComp.index = 0;
            foreach (var nameHash in meshComp.nameHashes) {
                if (nameHash == animComp.nameHash) {
                    break;
                }
                ++animComp.index;
            }

            animComp.accumTime += deltaTime;
            if (meshComp.lengths[animComp.index] <= animComp.accumTime) {
                animComp.accumTime %= meshComp.lengths[animComp.index];
            }

            animComp.frame = (int)(animComp.accumTime / meshComp.frameRates[animComp.index]);
            int meshIdx = meshComp.offsets[animComp.index] + animComp.frame;
            animComp.rect = meshComp.rects[meshIdx];
        });
    }
}
