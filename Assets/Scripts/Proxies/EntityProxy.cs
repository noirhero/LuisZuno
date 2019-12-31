// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;
using UnityEngine.Serialization;
using GlobalDefine;

[RequiresEntityConversion]
public class EntityProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SpritePreset preset = null;
    [FormerlySerializedAs("Reactive")] public ReactiveComponent reactive;
    [FormerlySerializedAs("Environment")] public PassiveMadnessComponent passiveMadness;
    public EntityType type;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        SetupComponents(entity, dstManager, conversionSystem);
    }

    protected virtual void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null != preset) {
            dstManager.AddSharedComponentData(entity, new SpritePresetComponent(preset));
            dstManager.AddComponentData(entity, new SpriteAnimComponent() {
                nameHash = preset.datas.Keys.First()
            });
        }

        // TODO : 개별 프록시로?
        if (EntityType.MadnessEnvironment == type) {
            dstManager.AddComponentData(entity, new PassiveMadnessComponent(passiveMadness));
        }

        if (EntityType.Player != type) {
            dstManager.AddComponentData(entity, new ReactiveComponent(ref reactive));

            if (EntityType.Wall == type) {
                dstManager.AddComponentData(entity, new TurningComponent());
            }
        }
    }
}
