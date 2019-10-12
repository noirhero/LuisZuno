// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;

[RequiresEntityConversion]
public class CameraProxy : MonoBehaviour, IConvertGameObjectToEntity {
    public Camera peset = null;
    public float velocity = 0.5f;

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {
        if (null != peset) {
            dstManager.AddSharedComponentData(entity, new CameraComponent(peset, velocity));
        }
    }
}
