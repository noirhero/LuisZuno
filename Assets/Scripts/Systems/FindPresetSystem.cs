// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

public class FindPresetSystem : ComponentSystem {
    private Dictionary<int, PresetComponent> _presetComponents = new Dictionary<int, PresetComponent>();
    protected override void OnStartRunning() {
        Entities.ForEach((Entity entity, PresetComponent presetComp) => {
            _presetComponents.Add(presetComp.guid, presetComp);
        });
    }

    protected override void OnUpdate() {
        Entities.ForEach((Entity entity, ref FindPresetComponent findComp) => {
            if (false == _presetComponents.TryGetValue(findComp.guid, out var presetComp)) {
                return;
            }

            if (false == ReferenceEquals(null, presetComp.preset)) {
                EntityManager.AddSharedComponentData(entity, new SpritePresetComponent() {
                    preset = presetComp.preset
                });
                EntityManager.AddComponentData(entity, new SpriteStateComponent() {
                    hash = presetComp.preset.datas.Keys.First()
                });
            }

            if (false == ReferenceEquals(null, presetComp.npcAIPreset)) {
                EntityManager.AddSharedComponentData(entity, new NPCAIPresetComponent() {
                    preset = presetComp.npcAIPreset
                });
            }

            EntityManager.RemoveComponent<FindPresetComponent>(entity);
        });
    }
}
