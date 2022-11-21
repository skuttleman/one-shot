using System.Collections.Generic;
using UnityEngine;

namespace Game.System.AI {
    namespace Interfaces {
        namespace Patrol {
            public interface IPatrol : IEnumerable<IPatrolStep> { }
            public interface IPatrolStep {
                public IEnumerable<YieldInstruction> DoStep(Transform transform);
            }
        }
    }
}
