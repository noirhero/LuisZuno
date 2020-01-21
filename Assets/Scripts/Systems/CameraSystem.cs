// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using Unity.Transforms;
using Unity.Entities;
using UnityEngine;

public class CameraSystem : ComponentSystem {
    private Transform _cameraTransform;
    protected override void OnStartRunning() {
        _cameraTransform = Camera.main?.transform;
    }

    protected override void OnUpdate() {
        var deltaTime = Time.DeltaTime;
        Entities
            .WithAll<PlayerComponent>()
            .ForEach((ref Translation pos) => {
                var newPos = _cameraTransform.position;
                newPos.x += (pos.Value.x - newPos.x) * deltaTime;
                _cameraTransform.position = newPos;
            });
    }
}
