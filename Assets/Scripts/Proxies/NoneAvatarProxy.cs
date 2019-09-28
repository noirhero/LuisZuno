// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class NoneAvatarProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public SpritePreset preset = null;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null != preset) {
            dstManager.AddSharedComponentData(entity, new SpritePresetComponent(preset));
            dstManager.AddComponentData(entity, new SpriteAnimComponent() {
                nameHash = preset.datas.Keys.First()
            });
        }

        BoxCollider2D cachedBox = GetComponent<BoxCollider2D>();
        if (cachedBox != null) {
            dstManager.AddComponentData(entity, new ReactiveComponent() {
                colliderSizeX = cachedBox.size.x,
                colliderSizeY = cachedBox.size.y
            });
        }

        dstManager.AddComponentData(entity, new TargetComponent());
    }
}