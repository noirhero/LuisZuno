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
                    case SubSceneType.Scenario001_Hallway:
                        loadSubScene = _preset.scenario001_HallWay;
                        break;
                    case SubSceneType.Scenario001_Basement:
                        loadSubScene = _preset.scenario001_Basement;
                        break;
                    case SubSceneType.Scenario001_LegacyOfClan:
                        loadSubScene = _preset.scenario001_LegacyOfClan;
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