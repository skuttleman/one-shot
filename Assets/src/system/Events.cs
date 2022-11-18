using UnityEngine;

namespace Game.System.Events
{
    public interface IEvent { }

    namespace Player
    {
        public struct StanceChange : IEvent
        {
            public enum Stance { STANDING, CROUCHING, CRAWLING }
            public readonly Stance stance;
            public StanceChange(Stance stance) => this.stance = stance;
        }

        public struct AttackModeChange : IEvent
        {
            public enum AttackMode {  NONE, HAND, WEAPON }
            public readonly AttackMode mode;
            public AttackModeChange(AttackMode mode) => this.mode = mode;
        }

        public struct MovementSpeedChange : IEvent
        {
            public readonly float speed;
            public MovementSpeedChange(float speed) => this.speed = speed;
        }

        public struct ScopeChange : IEvent
        {
            public readonly bool isScoping;
            public ScopeChange(bool isScoping) => this.isScoping = isScoping;
        }

        public struct InputAim : IEvent
        {
            public readonly bool isAiming;
            public InputAim(bool isAiming) => this.isAiming = isAiming;
        }

        public struct InputAttack : IEvent
        {
            public readonly bool isAttacking;
            public InputAttack(bool isAttacking) => this.isAttacking = isAttacking;
        }

        public struct InputLook : IEvent
        {
            public readonly Vector2 direction;
            public InputLook(Vector2 direction) => this.direction = direction;
        }

        public struct InputMove : IEvent
        {
            public readonly Vector2 direction;
            public InputMove(Vector2 direction) => this.direction = direction;
        }

        public struct InputScope : IEvent
        {
            public readonly bool isScoping;
            public InputScope(bool isScoping) => this.isScoping = isScoping;
        }

        public struct InputStance : IEvent
        {
            public readonly float value;
            public InputStance(float value) => this.value = value;
        }

        public struct InputMoveModified : IEvent
        {
            public readonly bool isModified;
            public InputMoveModified(bool isModified) => this.isModified = isModified;
        }
    }
}
