// Copyright 2018-2019 TAP, Inc. All Rights Reserved.

using System.Text;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

public class AnimStateSystem : JobComponentSystem {
    struct AnimStateSystemJob : IJobForEach<SpriteAnimComponent, MovementComponent> {
        public void Execute(ref SpriteAnimComponent animComp, [ReadOnly] ref MovementComponent moveComp) {
            if (math.FLT_MIN_NORMAL < math.lengthsq(moveComp.value)) {
                animComp.nameHash = 0;
                foreach (var b in Encoding.ASCII.GetBytes("Walk")) {
                    animComp.nameHash += b;
                }
            }
            else {
                animComp.nameHash = 0;
                foreach (var b in Encoding.ASCII.GetBytes("Idle")) {
                    animComp.nameHash += b;
                }
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new AnimStateSystemJob();
        return job.Schedule(this, inputDependencies);
    }
}