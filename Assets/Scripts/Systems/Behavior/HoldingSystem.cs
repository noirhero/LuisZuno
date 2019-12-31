// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using Unity.Entities;

public class HoldingSystem : ComponentSystem {

    protected override void OnCreate() {
        Enabled = false;
    }

    protected override void OnUpdate() {
    }
}
