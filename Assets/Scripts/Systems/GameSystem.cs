// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

public class GameSystem : ComponentSystem {
    protected override void OnUpdate() {
        if (Utility._playerEntity.Equals(Entity.Null)) {
            return;
        }

        if (EntityManager.HasComponent<GameStartComponent>(Utility._playerEntity)) {
            EntityManager.RemoveComponent<GameStartComponent>(Utility._playerEntity);
            EnableSystem(true);
            return;
        }

        if (EntityManager.HasComponent<GameClearComponent>(Utility._playerEntity)) {
            EntityManager.RemoveComponent<GameClearComponent>(Utility._playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GameOverComponent>(Utility._playerEntity)) {
            EntityManager.RemoveComponent<GameOverComponent>(Utility._playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GamePauseComponent>(Utility._playerEntity)) {
            EntityManager.RemoveComponent<GamePauseComponent>(Utility._playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GameResumeComponent>(Utility._playerEntity)) {
            EntityManager.RemoveComponent<GameResumeComponent>(Utility._playerEntity);
            EnableSystem(true);
            UpdateUI();
            return;
        }
    }

    
    private void EnableSystem(bool inEnable) {
        foreach (var system in World.DefaultGameObjectInjectionWorld.Systems) {
            if (system.GetType() == typeof(TargetingSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(AutoMovementSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(IntelligenceSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(EffectSpawnSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(EntitySpawnSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(LifeCycleSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(HoldingSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(SearchingSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(PanicSystem)) {
                system.Enabled = inEnable;
            }
            else if (system.GetType() == typeof(MadnessSystem)) {
                system.Enabled = inEnable;
            }
        }
    }


    private void UpdateUI() {
        // 이동후 게임재개시 보여질 ui
        foreach (var system in World.DefaultGameObjectInjectionWorld.Systems) {
            if (system is TeleportSystem) {
                var guiComp = Utility._entityMng.GetComponentData<GUIComponent>(Utility._playerEntity);
                TeleportSystem teleportSystem = system as TeleportSystem;
                switch (teleportSystem.CurSubSceneType) {
                    case SubSceneType.sceneSelect :
                        guiComp.currentUI = GUIState.scenarioSelect;
                        break;
                    case SubSceneType.Scenario001_Hallway :
                    case SubSceneType.Scenario001_Basement :
                        guiComp.currentUI = GUIState.inventory;
                        guiComp.currentUI |= GUIState.bubble;
                        break;
                    case SubSceneType.Scenario001_LegacyOfClan :
                        guiComp.currentUI = GUIState.inventory;
                        guiComp.currentUI |= GUIState.bubble;
                        guiComp.currentUI |= GUIState.madness;
                        break;
                    default:
                        guiComp.currentUI = GUIState.none;
                        break;
                }
                Utility._entityMng.SetComponentData(Utility._playerEntity, guiComp);
            }
        }
    }
}
