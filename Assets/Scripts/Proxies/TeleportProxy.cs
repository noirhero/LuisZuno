// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Transforms;
using UnityEngine.Serialization;
using GlobalDefine;

[RequiresEntityConversion]
public class TeleportProxy : EntityProxy {
    // 맵정보 방식 정리되기전까지 임시
    public SceneType sceneType;
    public SubSceneType curSubSceneType;
    public SubSceneType nextSubSceneType;
    public int pointID;
    //
    [FormerlySerializedAs("TeleportTime")] public float teleportTime;
    [FormerlySerializedAs("FadeInOutTime")] public float fadeInOutTime;
    [FormerlySerializedAs("Status")] public PropStatusComponent status;


    protected override void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        base.SetupComponents(entity, dstManager, conversionSystem);

        dstManager.AddComponentData(entity, new ReactiveComponent());
        dstManager.AddComponentData(entity, new PropStatusComponent(ref status));        
        dstManager.AddComponentData(entity, new TeleportInfoComponent(sceneType, curSubSceneType, nextSubSceneType, pointID, teleportTime, fadeInOutTime));
    }
}
