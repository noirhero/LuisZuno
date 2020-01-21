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

        Entities.ForEach((Entity playerEntity, ref TeleportComponent teleportComp, ref PlayerComponent playerComp, ref Translation pos) => {
            teleportComp.elapsedTeleportTime += Time.DeltaTime;

            if (teleportComp.elapsedTeleportTime >= teleportComp.teleportTime) {
                var desiredPos = teleportComp.destination.Value;

                // player
                pos.Value = desiredPos;

                // end
                EntityManager.RemoveComponent<TeleportComponent>(playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.teleport;
            }
        });
    }
}
