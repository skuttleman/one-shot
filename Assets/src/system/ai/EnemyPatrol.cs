using System;
using System.Collections;
using System.Collections.Generic;
using Game.System.AI.Interfaces.Patrol;
using Game.System.AI.Patrol.Components;
using Game.Utils;
using UnityEngine;

namespace Game.System.AI.Patrol {
    public abstract class APatrol : IPatrol {
        private readonly IEnumerable<IPatrol> instructions;

        protected APatrol(IEnumerable<IPatrol> instructions) =>
            this.instructions = instructions;

        public static IEnumerator<YieldInstruction> DoPatrol(IPatrol patrol, Transform transform) {
            foreach (IPatrolStep step in patrol)
                foreach (YieldInstruction instruction in step.DoStep(transform.gameObject.transform))
                    yield return instruction;
        }

        public IEnumerator<IPatrolStep> GetEnumerator() {
            foreach (IPatrol instruction in instructions)
                foreach (IPatrolStep step in instruction)
                    yield return step;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class Patrol : APatrol {
        public Patrol(params IPatrol[] instructions) : base(instructions) { }
        public Patrol(IEnumerable<IPatrol> instructions) : base(instructions) { }
    }

    public class PatrolStep : IPatrol {
        private readonly IEnumerable<IPatrolStep> steps;
        public PatrolStep(params IPatrolStep[] steps) => this.steps = steps;
        public PatrolStep(IEnumerable<IPatrolStep> steps) => this.steps = steps;

        public IEnumerator<IPatrolStep> GetEnumerator() {
            foreach (IPatrolStep step in steps)
                yield return step;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PatrolCycle : APatrol {
        private PatrolCycle(IEnumerable<IPatrol> instructions) : base(instructions) { }
        public static PatrolCycle Of(IEnumerable<IPatrol> instructions) =>
            new(Sequences.Cycle(instructions));
    }

    namespace Composables {
        /*
         * IPatrol Composables
         * */
        public class PatrolWalkTo : IPatrol {
            private readonly IPatrol patrol;

            public PatrolWalkTo(
                Transform transform,
                Vector3 position,
                float movementSpeed,
                float turnSpeed) {

                patrol = new PatrolDoWhile(
                    new PatrolMultiAsync(
                        new PatrolMovemvent(position, movementSpeed),
                        new PatrolTurn(position, turnSpeed)),
                    () => Maths.Distance(transform.position, position) > movementSpeed);
            }

            public IEnumerator<IPatrolStep> GetEnumerator() =>
                patrol.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class PatrolLookAt : IPatrol {
            private readonly IPatrol patrol;

            public PatrolLookAt(Transform transform, Vector3 position, float speed) {

                patrol = new PatrolDoWhile(
                    new PatrolTurn(position, speed),
                    () => {
                        float rotationZ = Vectors.AngleTo(transform.position, position);
                        return Mathf.Abs(transform.rotation.eulerAngles.z - rotationZ) >= speed;
                    });
            }

            public IEnumerator<IPatrolStep> GetEnumerator() => patrol.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }

    namespace Components {
        /*
         * IPatrolStep Components
         * */
        public class PatrolYield : IPatrolStep {
            readonly YieldInstruction instruction;
            public PatrolYield(YieldInstruction instruction) =>
                this.instruction = instruction;
            public IEnumerable<YieldInstruction> DoStep(Transform _) {
                yield return instruction;
            }
        }

        public class PatrolMovemvent : IPatrolStep {
            private readonly Vector3 position;
            private readonly float speed;
            public PatrolMovemvent(Vector3 position, float speed) {
                this.position = position;
                this.speed = speed;
            }

            public IEnumerable<YieldInstruction> DoStep(Transform transform) {
                transform.position += (position - transform.position).normalized * speed;
                yield return new WaitForEndOfFrame();
            }
        }

        public class PatrolTurn : IPatrolStep {
            private readonly Vector3 position;
            private readonly float speed;
            public PatrolTurn(Vector3 position, float speed) {
                this.position = position;
                this.speed = speed;
            }

            public IEnumerable<YieldInstruction> DoStep(Transform transform) {
                float rotationZ = Vectors.AngleTo(transform.position, position);

                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.Euler(0, 0, rotationZ),
                    speed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        public class PatrolMultiSync : IPatrolStep {
            private readonly IEnumerable<IPatrolStep> steps;
            public PatrolMultiSync(params IPatrolStep[] steps) =>
                this.steps = steps;

            public IEnumerable<YieldInstruction> DoStep(Transform transform) {
                foreach (IPatrolStep step in steps)
                    foreach (YieldInstruction instruction in step.DoStep(transform))
                        yield return instruction;
                yield return new WaitForEndOfFrame();
            }
        }

        public class PatrolMultiAsync : IPatrolStep {
            private readonly IEnumerable<IPatrolStep> steps;
            public PatrolMultiAsync(params IPatrolStep[] steps) =>
                this.steps = steps;

            public IEnumerable<YieldInstruction> DoStep(Transform transform) {
                foreach (IPatrolStep step in steps)
                    step.DoStep(transform).GetEnumerator().MoveNext();
                yield return new WaitForEndOfFrame();
            }
        }

        public class PatrolAction : IPatrolStep {
            private readonly Action action;
            public PatrolAction(Action action) => this.action = action;
            public IEnumerable<YieldInstruction> DoStep(Transform transform) {
                action();
                yield break;
            }
        }

        /*
         * IPatrol Components
         * */
        public class PatrolDoWhile : IPatrol {
            private readonly IPatrolStep step;
            private readonly Func<bool> pred;
            public PatrolDoWhile(IPatrolStep step, Func<bool> pred) {
                this.step = step;
                this.pred = pred;
            }

            public IEnumerator<IPatrolStep> GetEnumerator() {
                do {
                    yield return step;
                } while (pred());
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
