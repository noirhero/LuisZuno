// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Transforms;
using Unity.Entities;
using GlobalDefine;
using Unity.Mathematics;
using UnityEngine;

public class TeleportSystem : ComponentSystem {
    private float3 _desiredPos = float3.zero;
    private SubSceneType _curSubSceneType = SubSceneType.None;
    public SubSceneType CurSubSceneType { get { return _curSubSceneType; } }

    protected override void OnUpdate() {
        if (Utility.playerEntity.Equals(Entity.Null)) {
            return;
        }

        Entities.ForEach((ref TeleportComponent teleportComp, ref PlayerComponent playerComp, ref Translation pos) => {
            if (EntityManager.HasComponent<FadeInComponent>(Utility.playerEntity) || EntityManager.HasComponent<FadeOutComponent>(Utility.playerEntity))
                return;

            // start
            if (_desiredPos.Equals(float3.zero)) {
                EntityManager.AddComponentData(Utility.playerEntity, new GamePauseComponent());
                EntityManager.AddComponentData(Utility.playerEntity, new FadeInComponent(teleportComp.fadeInOutTime));
                EntityManager.AddComponentData(Utility.playerEntity, new SubSceneControlComponent());
                EntityManager.AddComponentData(Utility.playerEntity, new SubSceneLoadComponent() { type = (int)teleportComp.nextSubSceneType });
                _desiredPos = GetTeleportPoint(teleportComp.sceneType, teleportComp.pointID);
            }
            // finish
            else if (pos.Value.Equals(_desiredPos)) {
                _curSubSceneType = teleportComp.nextSubSceneType;
                
                EntityManager.AddComponentData(Utility.playerEntity, new GameResumeComponent());
                EntityManager.RemoveComponent<TeleportComponent>(Utility.playerEntity);
                EntityManager.RemoveComponent<SubSceneControlComponent>(Utility.playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.teleport;
                _desiredPos = float3.zero;
                
                return;
            }

            teleportComp.elapsedTeleportTime += Time.DeltaTime;
            if (teleportComp.elapsedTeleportTime >= teleportComp.teleportTime) {
                // player
                pos.Value = _desiredPos;

                EntityManager.AddComponentData(Utility.playerEntity, new CameraSyncComponent(_desiredPos));
                EntityManager.AddComponentData(Utility.playerEntity, new FadeOutComponent(teleportComp.fadeInOutTime));
                EntityManager.AddComponentData(Utility.playerEntity, new SubSceneUnLoadComponent() { type = (int)teleportComp.curSubSceneType });
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
