// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Transforms;
using Unity.Entities;
using GlobalDefine;
using Unity.Mathematics;

public class TeleportSystem : ComponentSystem {
    private float3 _desiredPos = float3.zero;
    private SubSceneType _curSubSceneType = SubSceneType.sceneSelect;
    public SubSceneType CurSubSceneType { get { return _curSubSceneType; } }

    protected override void OnUpdate() {
        if (Utility._playerEntity.Equals(Entity.Null)) {
            return;
        }

        Entities.ForEach((ref TeleportComponent teleportComp, ref PlayerComponent playerComp, ref Translation pos) => {
            if (EntityManager.HasComponent<FadeInComponent>(Utility._playerEntity) || EntityManager.HasComponent<FadeOutComponent>(Utility._playerEntity))
                return;

            // start
            if (_desiredPos.Equals(float3.zero)) {
                EntityManager.RemoveComponent<MovementComponent>(Utility._playerEntity);
                EntityManager.RemoveComponent<TargetingComponent>(Utility._playerEntity);
                playerComp.currentAnim = AnimationType.Idle;
                
                EntityManager.AddComponentData(Utility._playerEntity, new GamePauseComponent());
                EntityManager.AddComponentData(Utility._playerEntity, new FadeInComponent(teleportComp.fadeInOutTime));
                EntityManager.AddComponentData(Utility._playerEntity, new SubSceneControlComponent());
                EntityManager.AddComponentData(Utility._playerEntity, new SubSceneLoadComponent() { type = (int)teleportComp.nextSubSceneType });
                _desiredPos = GetTeleportPoint(teleportComp.sceneType, teleportComp.pointID);
            }
            // finish
            else if (pos.Value.Equals(_desiredPos)) {
                _curSubSceneType = teleportComp.nextSubSceneType;
                
                EntityManager.AddComponentData(Utility._playerEntity, new TargetingComponent());
                EntityManager.AddComponentData(Utility._playerEntity, new GameResumeComponent());
                EntityManager.RemoveComponent<TeleportComponent>(Utility._playerEntity);
                EntityManager.RemoveComponent<SubSceneControlComponent>(Utility._playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.teleport;
                _desiredPos = float3.zero;
                
                return;
            }

            teleportComp.elapsedTeleportTime += Time.DeltaTime;
            if (teleportComp.elapsedTeleportTime >= teleportComp.teleportTime) {
                // player
                pos.Value = _desiredPos;

                EntityManager.AddComponentData(Utility._playerEntity, new CameraSyncComponent(_desiredPos));
                EntityManager.AddComponentData(Utility._playerEntity, new FadeOutComponent(teleportComp.fadeInOutTime));
                EntityManager.AddComponentData(Utility._playerEntity, new SubSceneUnLoadComponent() { type = (int)teleportComp.curSubSceneType });
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
