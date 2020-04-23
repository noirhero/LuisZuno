// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

public class FindSpritePresetSystem : ComponentSystem {
    private Dictionary<int, SpritePreset> _spritePresets = new Dictionary<int, SpritePreset>();
    protected override void OnStartRunning() {
        List<Entity> destroyEntites = new List<Entity>();
        Entities.ForEach((Entity entity, SpritePresetGuidComponent guidComp) => {
            destroyEntites.Add(entity);
            _spritePresets.Add(guidComp.guid, guidComp.preset);
        });

        foreach (var entity in destroyEntites) {
            EntityManager.DestroyEntity(entity);
        }
    }

    protected override void OnUpdate() {
        Entities.ForEach((Entity entity, ref FindSpritePresetComponent findComp) => {
            if (false == _spritePresets.TryGetValue(findComp.guid, out var spritePreset)) {
                return;
            }

            EntityManager.AddSharedComponentData(entity, new SpritePresetComponent() {
                preset = spritePreset
            });
            EntityManager.AddComponentData(entity, new SpriteStateComponent() {
                hash = spritePreset.datas.Keys.First()
            });
            EntityManager.RemoveComponent<FindSpritePresetComponent>(entity);
        });
    }
}
