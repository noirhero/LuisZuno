// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using UnityEngine;
using GlobalDefine;

[RequiresEntityConversion]
public class TeleportProxy : MonoBehaviour, IConvertGameObjectToEntity {
    // 맵정보 방식 정리되기전까지 임시
    public SceneType sceneType;
    public SubSceneType curSubSceneType;
    public SubSceneType nextSubSceneType;
    public int pointID;
    //
    public float teleportTime;
    public float fadeInOutTime;
    public PropStatusComponent status;


    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        dstManager.AddComponentData(entity, new ReactiveComponent());
        dstManager.AddComponentData(entity, new PropStatusComponent(ref status));
        dstManager.AddComponentData(entity, new TeleportInfoComponent(sceneType, curSubSceneType, nextSubSceneType, pointID, teleportTime, fadeInOutTime));
    }
}
