// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using UnityEngine;
using Unity.Entities;

public class SubSceneTestController : MonoBehaviour {
    private void Start() {
        var findSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<SubSceneTestSystem>();
        findSystem.Enabled = true;
    }
}
