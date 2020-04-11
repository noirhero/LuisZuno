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
                EntityManager.RemoveComponent<MovementComponent>(_playerEntity);
                EntityManager.RemoveComponent<TargetingComponent>(_playerEntity);
                playerComp.currentAnim = AnimationType.Idle;
                
                EntityManager.AddComponentData(_playerEntity, new GamePauseComponent());
                EntityManager.AddComponentData(_playerEntity, new FadeInComponent(teleportComp.fadeInOutTime));
                EntityManager.AddComponentData(_playerEntity, new SubSceneControlComponent());
                EntityManager.AddComponentData(_playerEntity, new SubSceneLoadComponent() { type = (int)teleportComp.nextSubSceneType });
                _desiredPos = GetTeleportPoint(teleportComp.sceneType, teleportComp.pointID);
            }
            // finish
            else if (pos.Value.Equals(_desiredPos)) {
                EntityManager.AddComponentData(_playerEntity, new TargetingComponent());
                EntityManager.AddComponentData(_playerEntity, new GameResumeComponent());
                EntityManager.RemoveComponent<TeleportComponent>(_playerEntity);
                EntityManager.RemoveComponent<SubSceneControlComponent>(_playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.teleport;
                _desiredPos = float3.zero;
                return;
            }

            teleportComp.elapsedTeleportTime += Time.DeltaTime;
            if (teleportComp.elapsedTeleportTime >= teleportComp.teleportTime) {
                // player
                pos.Value = _desiredPos;

                EntityManager.AddComponentData(_playerEntity, new CameraSyncComponent(_desiredPos));
                EntityManager.AddComponentData(_playerEntity, new FadeOutComponent(teleportComp.fadeInOutTime));
            }
        });
    }


    protected float3 GetTeleportPoint(SceneType inType, int inPoint) {
        float3 cachedPoint = new float3();

        Entities.ForEach((SceneInformationPresetComponent presetComp) => {
            if (inType == presetComp.preset.sceneType) {
                cachedPoint = presetComp.preset.GetPoint(inPoint);
            }
        });

        return cachedPoint;
    }
}
