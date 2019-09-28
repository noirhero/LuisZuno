// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Entities;

public class ReactiveSystem : ComponentSystem {

    protected override void OnUpdate() {
        Entities.ForEach((ref PlayerComponent playerCompo, ref ReactiveComponent playerReactiveComp) => {

            var cachedNameHash = 0;

            Entities.ForEach((ref ReactiveComponent reactiveComp, ref TargetComponent targetComp) => {
                if (targetComp.bOn) {
                    cachedNameHash = reactiveComp.reactiveAnimList[0];
                    return;
                }
            });

            playerReactiveComp.pendingAnim = cachedNameHash;
        });
    }
}
