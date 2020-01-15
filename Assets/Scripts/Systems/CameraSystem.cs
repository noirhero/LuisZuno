// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using Unity.Transforms;
using Unity.Entities;
using Unity.Mathematics;

public class CameraSystem : ComponentSystem {
    protected override void OnUpdate() {
        Entities.ForEach((CameraPresetComponent presetComp, ref CameraComopnent cameraComp) => {
            var desiredPos = presetComp.defaultPos;
            var currentPosX = presetComp.myTransform.position.x;

            Entities.WithAll<PlayerComponent>().ForEach((Entity entity, ref Translation pos) => {
                var velocity = (currentPosX / pos.Value.x) * Time.DeltaTime;
                desiredPos.x = math.lerp(currentPosX, pos.Value.x, velocity);
            });
            presetComp.myTransform.position = desiredPos;
        });
    }
}
