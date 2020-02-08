// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using Unity.Entities;

public class GameSystem : ComponentSystem {
    private Entity _playerEntity = Entity.Null;


    protected override void OnStartRunning() {
        EnableSystem(false);
        Entities.WithAll<PlayerComponent>().ForEach((Entity entity, ref InventoryComponent inventoryComp) => {
            _playerEntity = entity;
        });
    }


    protected override void OnUpdate() {
        if (_playerEntity.Equals(Entity.Null)) {
            return;
        }

        if (EntityManager.HasComponent<GameStartComponent>(_playerEntity)) {
            EntityManager.RemoveComponent<GameStartComponent>(_playerEntity);
            EnableSystem(true);
            return;
        }

        if (EntityManager.HasComponent<GameClearComponent>(_playerEntity)) {
            EntityManager.RemoveComponent<GameClearComponent>(_playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GameOverComponent>(_playerEntity)) {
            EntityManager.RemoveComponent<GameOverComponent>(_playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GamePauseComponent>(_playerEntity)) {
            EntityManager.RemoveComponent<GamePauseComponent>(_playerEntity);
            EnableSystem(false);
            return;
        }

        if (EntityManager.HasComponent<GameResumeComponent>(_playerEntity)) {
            EntityManager.RemoveComponent<GameResumeComponent>(_playerEntity);
            EnableSystem(true);
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
}
