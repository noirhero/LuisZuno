// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using Unity.Transforms;
using Unity.Entities;
using UnityEngine;

public class CameraSystem : ComponentSystem {
    private Transform _cameraTransform;
    private float _height;

    protected override void OnStartRunning() {
        _cameraTransform = Camera.main?.transform;
        Entities.WithAll<PlayerComponent>().ForEach((ref Translation pos) => {
            _height = pos.Value.y - _cameraTransform.position.y;
        });
    }

    protected override void OnUpdate() {
        var deltaTime = Time.DeltaTime;
        Entities.WithAll<PlayerComponent>().ForEach((ref Translation pos) => {
            var newPos = _cameraTransform.position;
            newPos.x += (pos.Value.x - newPos.x) * deltaTime;             
            newPos.y += (pos.Value.y - (newPos.y+ _height)) * deltaTime * 100.0f;
            _cameraTransform.position = newPos;
        });
    }
}
