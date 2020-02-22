// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Scenes;

public class SubSceneTestSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }

    private float _accumTime;
    protected override void OnUpdate() {
        _accumTime += Time.DeltaTime;
        if (3.0f > _accumTime) {
            return;
        }
        _accumTime = 0.0f;

        Entities.ForEach((Entity entity, SubScene subScene) => {
            if (EntityManager.HasComponent<RequestSceneLoaded>(entity)) {
                EntityManager.RemoveComponent<RequestSceneLoaded>(entity);
            }
            else {
                EntityManager.AddComponent<RequestSceneLoaded>(entity);
            }
        });
    }
}
