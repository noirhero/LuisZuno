// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Transforms;
using Unity.Entities;
using GlobalDefine;
using Unity.Mathematics;

public class TeleportSystem : ComponentSystem {
    private Entity _playerEntity = Entity.Null;
    private float3 _desiredPos = float3.zero;

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
            if (EntityManager.HasComponent<FadeInComponent>(_playerEntity) || EntityManager.HasComponent<FadeOutComponent>(_playerEntity))
                return;

            // start
            if (_desiredPos.Equals(float3.zero)) {
                EntityManager.AddComponentData(_playerEntity, new GamePauseComponent());
                EntityManager.AddComponentData(_playerEntity, new FadeInComponent(1.0f));
                _desiredPos = teleportComp.destination.Value;
            }
            // finish
            else if (pos.Value.Equals(_desiredPos)) {
                EntityManager.AddComponentData(_playerEntity, new GameResumeComponent());
                EntityManager.RemoveComponent<TeleportComponent>(_playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.teleport;
                _desiredPos = float3.zero;
                return;
            }

            teleportComp.elapsedTeleportTime += Time.DeltaTime;
            if (teleportComp.elapsedTeleportTime >= teleportComp.teleportTime) {
                // player
                pos.Value = _desiredPos;

                EntityManager.AddComponentData(_playerEntity, new CameraSyncComponent(_desiredPos));
                EntityManager.AddComponentData(_playerEntity, new FadeOutComponent(1.0f));
            }
        });
    }
}
