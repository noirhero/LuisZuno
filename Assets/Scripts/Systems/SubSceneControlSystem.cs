// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using Unity.Scenes;
using GlobalDefine;

public class SubSceneControlSystem : ComponentSystem {
    private SubScenePreset _preset;
    protected override void OnStartRunning() {
        Entities.ForEach((SubScenePresetComponent preset) => { _preset = preset.preset; });
    }


    private SubScene GetSubSceneByType(int type) {
        SubScene loadSubScene = null;
        switch ((SubSceneType)type) {
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
                break;
        }
        return loadSubScene;
    }


    protected override void OnUpdate() {
        var subSceneEntities = Entities.WithAll<SubScene>();
        var subSceneControlEntities = Entities.WithAll<SubSceneControlComponent>();
        
        subSceneControlEntities.WithAny<SubSceneLoadComponent, SubSceneUnLoadComponent>()
            .ForEach((Entity subSceneControlEntity) => {

                if (EntityManager.HasComponent<SubSceneLoadComponent>(subSceneControlEntity)) {
                    var subSceneLoad = EntityManager.GetComponentData<SubSceneLoadComponent>(subSceneControlEntity);

                    SubScene loadSubScene = GetSubSceneByType(subSceneLoad.type);
                    subSceneEntities.ForEach((Entity entity, SubScene subScene) => {
                        if (loadSubScene == subScene) {
                            EntityManager.AddComponent<RequestSceneLoaded>(entity);
                        }
                    });

                    EntityManager.RemoveComponent<SubSceneLoadComponent>(subSceneControlEntity);
                }

                else if (EntityManager.HasComponent<SubSceneUnLoadComponent>(subSceneControlEntity)) {
                    var subSceneUnload = EntityManager.GetComponentData<SubSceneUnLoadComponent>(subSceneControlEntity);

                    SubScene unloadSubScene = GetSubSceneByType(subSceneUnload.type);
                    subSceneEntities.ForEach((Entity entity, SubScene subScene) => {
                        if (unloadSubScene == subScene) {
                            EntityManager.RemoveComponent<RequestSceneLoaded>(entity);
                        }
                    });

                    EntityManager.RemoveComponent<SubSceneUnLoadComponent>(subSceneControlEntity);
                } 
        });
    }
}