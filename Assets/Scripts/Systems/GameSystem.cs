// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;
using GlobalDefine;

public class GameSystem : ComponentSystem {
    protected override void OnStartRunning() {
        if (Entity.Null == Utility.playerEntity || null == Utility.tablePreset) {
            EnableSystem(false);
        }
        else {
            EntityManager.AddComponentData(Utility.playerEntity, new GameStartComponent());
            EntityManager.AddComponentData(Utility.playerEntity, new GUIComponent());
        }
    }

    
    protected override void OnUpdate() {
        if (Entity.Null == Utility.playerEntity || null == Utility.tablePreset)  {
            return;
        }

        if (EntityManager.HasComponent<GameStartComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameStartComponent>(Utility.playerEntity);
            EntityManager.AddComponentData(Utility.playerEntity, new GUISyncComponent(GUIGroupType.GameStart));
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GameClearComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameClearComponent>(Utility.playerEntity);
            EntityManager.AddComponentData(Utility.playerEntity, new GUISyncComponent(GUIGroupType.GameClear));
            EnableSystem(false);
            TemporaryPauseMove();
            return;
        }

        if (EntityManager.HasComponent<GameOverComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameOverComponent>(Utility.playerEntity);
            EntityManager.AddComponentData(Utility.playerEntity, new GUISyncComponent(GUIGroupType.GameOver));
            EnableSystem(false);
            TemporaryPauseMove();
            return;
        }

        if (EntityManager.HasComponent<GamePauseComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GamePauseComponent>(Utility.playerEntity);
            EntityManager.AddComponentData(Utility.playerEntity, new GUISyncComponent(GUIGroupType.None));
            EnableSystem(false);
            TemporaryPauseMove();
            return;
        }

        if (EntityManager.HasComponent<GameResumeComponent>(Utility.playerEntity)) {
            EntityManager.RemoveComponent<GameResumeComponent>(Utility.playerEntity);
            EntityManager.AddComponentData(Utility.playerEntity, new GUISyncComponent(GUIGroupType.GamePlay));
            EnableSystem(true);
            TemporaryResumeMove();
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
            else if (system.GetType() == typeof(InventorySystem)) {
                system.Enabled = inEnable;
            }
        }
    }


    private void TemporaryPauseMove() { 
        EntityManager.RemoveComponent<MovementComponent>(Utility.playerEntity);
        EntityManager.RemoveComponent<TargetingComponent>(Utility.playerEntity);

        var playerComp = EntityManager.GetComponentData<PlayerComponent>(Utility.playerEntity);
        playerComp.currentAnim = AnimationType.Idle;
        EntityManager.SetComponentData(Utility.playerEntity, playerComp);
    }


    private void TemporaryResumeMove() {
        EntityManager.AddComponentData(Utility.playerEntity, new TargetingComponent());
    }
}
