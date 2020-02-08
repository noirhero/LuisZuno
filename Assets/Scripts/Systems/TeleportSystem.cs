// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Transforms;
using Unity.Entities;
using GlobalDefine;
using Unity.Mathematics;

public class TeleportSystem : ComponentSystem {
    private Entity _playerEntity = Entity.Null;

    protected override void OnStartRunning() {
        Entities.WithAll<PlayerComponent>().ForEach((Entity entity, ref InventoryComponent inventoryComp) => {
            _playerEntity = entity;
        });
    }

    protected override void OnUpdate() {
        if (_playerEntity.Equals(Entity.Null)) {
            return;
        }

        Entities.ForEach((ref TeleportComponent teleportComp, ref PlayerComponent playerComp, ref Translation pos) => {
            if (teleportComp.elapsedTeleportTime <= 0.0f) {
                var desiredPos = teleportComp.destination.Value;
                EntityManager.AddComponentData(_playerEntity, new CameraSyncComponent(desiredPos));
                EntityManager.AddComponentData(_playerEntity, new GamePauseComponent());

                // player
                pos.Value = desiredPos;
            }

            if (teleportComp.elapsedTeleportTime >= teleportComp.teleportTime) {
                EntityManager.AddComponentData(_playerEntity, new GameResumeComponent());
                EntityManager.RemoveComponent<TeleportComponent>(_playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.teleport;
            }
            teleportComp.elapsedTeleportTime += Time.DeltaTime;
        });
    }
}
