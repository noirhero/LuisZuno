// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;
using GlobalDefine;

[RequiresEntityConversion]
public class EntityProxy : MonoBehaviour, IConvertGameObjectToEntity {

    public SpritePreset preset = null;
    public EntityType entityType = EntityType.Static;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        SetupComponents(entity, dstManager, conversionSystem);
    }

    public virtual void SetupComponents(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null != preset) {
            dstManager.AddSharedComponentData(entity, new SpritePresetComponent(preset));
            dstManager.AddComponentData(entity, new SpriteAnimComponent() {
                nameHash = preset.datas.Keys.First()
            });
        }

        Vector2 cachedSize = Vector2.zero;
        BoxCollider2D cachedBox = GetComponent<BoxCollider2D>();
        if (cachedBox != null) {
            cachedSize.x = cachedBox.size.x;
            cachedSize.y = cachedBox.size.y;
        }

        dstManager.AddComponentData(entity, new ReactiveComponent() {
            colliderSizeX = cachedSize.x,
            colliderSizeY = cachedSize.y,
            type = entityType,
        });

        dstManager.AddComponentData(entity, new TargetComponent() {
            targetIndex = int.MaxValue,
        });
    }
}
