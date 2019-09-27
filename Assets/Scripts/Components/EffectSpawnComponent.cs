// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System;
using Unity.Entities;
using UnityEngine.Serialization;

[Serializable]
public struct EffectSpawnComponent : IComponentData {
    public Entity prefab;
}

[Serializable]
public struct EffectSpawnExistComponent : IComponentData {
    
}
