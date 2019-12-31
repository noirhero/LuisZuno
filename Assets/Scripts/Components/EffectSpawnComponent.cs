// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine.Serialization;

[Serializable]
public struct EffectSpawnComponent : IComponentData {
    public Entity prefab;
    public float lifetime;
    public float duration;
}

[Serializable]
public struct EffectSpawnExistComponent : IComponentData {
    
}
