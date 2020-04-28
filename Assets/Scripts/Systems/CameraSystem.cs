// Copyright 2018-2019 TAP, Inc. All Rights Reserved

using Unity.Transforms;
using Unity.Entities;
using UnityEngine;

public class CameraSystem : ComponentSystem {
    private Transform _cameraTransform;
    private float _height = 1.0f;

    protected override void OnStartRunning() {
        _cameraTransform = Camera.main?.transform;
        Entities.WithAll<PlayerComponent>().ForEach((ref Translation pos) => {
            //_height = _cameraTransform.position.y - pos.Value.y;
        });
    }

    protected override void OnUpdate() {
        var deltaTime = Time.DeltaTime;
        Entities.WithAll<PlayerComponent>().ForEach((ref Translation pos) => {
            var newPos = _cameraTransform.position;
            newPos.x += (pos.Value.x - newPos.x) * deltaTime;
            newPos.y = pos.Value.y + _height;
            _cameraTransform.position = newPos;
        });

        Entities.ForEach((Entity entity, ref CameraSyncComponent pos) => {
            var newPos = _cameraTransform.position;
            newPos.x = pos.syncPos.x;
            newPos.y = pos.syncPos.y + _height;
            _cameraTransform.position = newPos;
            EntityManager.RemoveComponent<CameraSyncComponent>(entity);
        });
    }
}
