// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class EntityProxy : MonoBehaviour, IConvertGameObjectToEntity {

    public SpritePreset preset = null;
    public bool bTemporaryFlag = false;
    public int[] reactiveAnimList;

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

        BoxCollider2D cachedBox = GetComponent<BoxCollider2D>();
        if (cachedBox != null) {
            dstManager.AddComponentData(entity, new ReactiveComponent() {
                colliderSizeX = cachedBox.size.x,
                colliderSizeY = cachedBox.size.y,

                reactiveAnimList = reactiveAnimList,
                pendingAnim = 0
            });
        }

        dstManager.AddComponentData(entity, new TargetComponent() {
            bOn = bTemporaryFlag // temporary setting
        });
    }
}
