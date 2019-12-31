// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;
using GlobalDefine;

public class CameraSystem : ComponentSystem {
    protected override void OnUpdate() {
        Entities.ForEach((CameraPresetComponent presetComp, ref CameraComopnent cameraComp) => {
            var desiredPos = presetComp.defaultPos;
            var currentPosX = presetComp.myTransform.position.x;

            Entities.WithAll<PlayerComponent>().ForEach((Entity entity, ref Translation pos) => {
                var velocity = (currentPosX / pos.Value.x) * Time.deltaTime;
                desiredPos.x = math.lerp(currentPosX, pos.Value.x, velocity);
            });
            presetComp.myTransform.position = desiredPos;
        });
    }
}
