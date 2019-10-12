// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

public class CameraSystem : ComponentSystem {
    protected override void OnUpdate() {
        Entities.ForEach((CameraComponent cameraComp) => {
            var desiredPos = cameraComp.defaultPos;
            var velocity = math.clamp(cameraComp.velocity * Time.deltaTime, 0.0f, 1.0f);

            Entities.ForEach((Entity entity, ref ReactiveComponent reactiveComp, ref Translation pos) => {
                if (reactiveComp.type == EntityType.Player) {
                    desiredPos.x = Mathf.Lerp(cameraComp.myTransform.position.x, pos.Value.x, velocity);
                }
            });
            cameraComp.myTransform.position = desiredPos;
        });
    }
}
