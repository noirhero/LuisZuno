// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;
using GlobalDefine;

public class PanicSystem : ComponentSystem {

    protected override void OnCreate() {
        Enabled = false;
    }

    protected override void OnUpdate() {
        Entities.ForEach((Entity playerEntity, ref PanicComponent panicComp, ref PlayerComponent playerComp) => {
            if (BehaviorState.HasState(playerComp, BehaviorState.searching)) {
                return;
            }

            // initialize animation
            if (panicComp.elapsedPanicTime == 0.0f) {
                playerComp.currentAnim = panicComp.panicAnim;
            }

            panicComp.elapsedPanicTime += Time.deltaTime;
            if (panicComp.elapsedPanicTime >= panicComp.panicTime) {
                EntityManager.RemoveComponent<PanicComponent>(playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.panic;
            }
        });
    }
}
