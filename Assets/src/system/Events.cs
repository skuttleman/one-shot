using UnityEngine;

namespace Game.System.Events {
    public interface IEvent { }

    public record Event<T>(T data) : IEvent;

    namespace Player {
        public enum PlayerStance { STANDING, CROUCHING, CRAWLING }
        public enum PlayerAttackMode { NONE, HAND, WEAPON, FIRING, PUNCHING }

        public record PlayerMovementSpeedChange(float data) : IEvent;
        public record PlayerScopeChange(bool data) : IEvent;
    }

    namespace Enemy {
        public record EnemyCanSeePlayer<T>(
            T enemy,
            Vector3 origin,
            Vector3 position,
            float distance) : IEvent where T : MonoBehaviour;
    }
}
