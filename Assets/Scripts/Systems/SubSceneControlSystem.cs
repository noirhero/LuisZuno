// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Scenes;

public class SubSceneControlSystem : ComponentSystem {
    private SubScenePreset _preset;
    protected override void OnStartRunning() {
        Entities.ForEach((SubScenePresetComponent preset) => { _preset = preset.preset; });
    }

    protected override void OnUpdate() {
        var subSceneEntities = Entities.WithAll<SubScene>();
        var subSceneControlEntities = Entities.WithAll<SubSceneControlComponent>();

        subSceneControlEntities
            .ForEach((Entity subSceneControlEntity, ref SubSceneLoadComponent subSceneLoad) => {
                SubScene loadSubScene = null;
                switch ((SubSceneType) subSceneLoad.type) {
                    case SubSceneType.sceneSelect:
                        loadSubScene = _preset.scenarioSelectSubScene;
                        break;
                    default:
                        return;
                }

                subSceneEntities.ForEach((Entity entity, SubScene subScene) => {
                    if (loadSubScene == subScene) {
                        EntityManager.AddComponent<RequestSceneLoaded>(entity);
                    }
                });

                EntityManager.RemoveComponent<SubSceneLoadComponent>(subSceneControlEntity);
            });
    }
}