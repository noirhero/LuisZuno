﻿// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

[UpdateAfter(typeof(GameSystem))]
public class SearchingSystem : ComponentSystem {
    protected override void OnUpdate() {
        Entities.ForEach((Entity playerEntity, ref SearchingComponent searchingComp, ref PlayerComponent playerComp) => {
            // initialize
            if (searchingComp.elapsedSearchingTime == 0.0f) {
                playerComp.currentAnim = searchingComp.searchingAnim;

                var statusComp = EntityManager.GetComponentData<PlayerStatusComponent>(playerEntity);
                searchingComp.searchingTime *= statusComp.searchingWeight;
            }

            searchingComp.elapsedSearchingTime += Time.DeltaTime;
            if (searchingComp.elapsedSearchingTime >= searchingComp.searchingTime) {
                EntityManager.RemoveComponent<SearchingComponent>(playerEntity);
                playerComp.currentBehaviors ^= BehaviorState.searching;
            }
        });
    }
}
