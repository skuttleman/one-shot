using UnityEngine;

namespace Game.System.Events {
    public interface IEvent { }

    public struct Event<T> : IEvent {
        public readonly T data;
        public Event(T data) => this.data = data;
    }

    namespace Player {
        public enum PlayerStance { STANDING, CROUCHING, CRAWLING }
        public enum PlayerAttackMode { NONE, HAND, WEAPON, FIRING, PUNCHING }

        public struct PlayerMovementSpeedChange : IEvent {
            public readonly float data;
            public PlayerMovementSpeedChange(float speed) => data = speed;
        }

        public struct PlayerScopeChange : IEvent {
            public readonly bool data;
            public PlayerScopeChange(bool isScoping) => data = isScoping;
        }
    }

    namespace Enemy {
        public struct EnemyCanSeePlayer<T> : IEvent where T : MonoBehaviour {
            public readonly T enemy;
            public readonly Vector3 origin;
            public readonly Vector3 position;
            public readonly float distance;
            public EnemyCanSeePlayer(T enemy, Vector3 origin, Vector3 position, float distance) {
                this.enemy = enemy;
                this.origin = origin;
                this.position = position;
                this.distance = distance;
            }

            public EnemyCanSeePlayer<T> Enemy(T enemy) =>
                new(enemy, origin, position, distance);
            public EnemyCanSeePlayer<T> Origin(Vector3 origin) =>
                new(enemy, origin, position, distance);
            public EnemyCanSeePlayer<T> Position(Vector3 position) =>
                new(enemy, origin, position, distance);
            public EnemyCanSeePlayer<T> Distance(float distance) =>
                new(enemy, origin, position, distance);
        }
    }
}
