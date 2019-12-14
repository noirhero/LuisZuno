// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class IntelligenceSystem : ComponentSystem {
    protected override void OnCreate() {
        Enabled = false;
    }

    protected override void OnUpdate() {
    }
}
