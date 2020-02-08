// Copyright 2018-2020 TAP, Inc. All Rights Reserved.

using System.Linq;
using UnityEngine;
using Unity.Entities;

public class ScenarioSelectUI : MonoBehaviour {
    private Entity _playerEntity = Entity.Null;
    private EntityManager _cachedEntityMng;
    private EntityManager _EntityMng {
        get {
            if (World.DefaultGameObjectInjectionWorld.EntityManager != _cachedEntityMng) {
                _cachedEntityMng = World.DefaultGameObjectInjectionWorld.EntityManager;
            }

            return _cachedEntityMng;
        }
    }


    void Start() {
        var playerEntities = _EntityMng.GetAllEntities()
            .Where(entity => _EntityMng.HasComponent(entity, typeof(PlayerComponent)));

        foreach (var entity in playerEntities) {
            _EntityMng.AddComponentData(entity, new CustomizeComponent(10));
            _playerEntity = entity;
            break;
        }
    }


    public void OnSelectedInScenario(int inType) {
    }
}
