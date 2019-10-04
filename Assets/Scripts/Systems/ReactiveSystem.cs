// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using System.Text;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

public class ReactiveSystem : ComponentSystem {
    protected override void OnUpdate() {
        var chooseNameHash = 0;
        var compareIndex = int.MaxValue;
        var compareDistance = float.MaxValue;
        var bMoving = false;

        Entities.ForEach((ref ReactiveComponent baseReactiveComp, ref MovementComponent baseMoveComp, ref TargetComponent baseTargetComp, ref SpriteAnimComponent baseAnimComp) => {
            chooseNameHash = 0;
            compareIndex = baseTargetComp.targetIndex;
            compareDistance = baseTargetComp.targetDistance;
            bMoving = math.FLT_MIN_NORMAL < math.lengthsq(baseMoveComp.value);

            Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp) => {
                if (compareIndex != entity.Index)
                    return;

                if (bMoving) {
                    foreach (var b in Encoding.ASCII.GetBytes(AnimationType.Walk.ToString())) {
                        chooseNameHash += b;
                    }
                    reactiveComp.reactivingDuration = 0.0f;
                }
                else {
                    if (reactiveComp.reactiveLength < reactiveComp.reactivingDuration) {
                        foreach (var b in Encoding.ASCII.GetBytes(AnimationType.SomethingDoIt.ToString())) {
                            chooseNameHash += b;
                        }
                    }
                    else {
                        foreach (var b in Encoding.ASCII.GetBytes(AnimationType.Idle.ToString())) {
                            chooseNameHash += b;
                        }
                    }
                    reactiveComp.reactivingDuration += Time.deltaTime;
                }
            });

            if (baseAnimComp.nameHash != chooseNameHash) {
                baseAnimComp.nameHash = chooseNameHash;
            }
        });
    }
}
