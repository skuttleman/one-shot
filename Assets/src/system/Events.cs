using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Utils;
using System;

namespace Game.System.Events
{
    public interface IEvent { }

    namespace Player
    {
        public class StanceChange : IEvent
        {
            public enum Stance { STANDING, CROUCHING, CRAWLING }
            public readonly Stance stance;
            public StanceChange(Stance stance) => this.stance = stance;
        }

        public class AttackModeChange : IEvent
        {
            public enum AttackMode {  NONE, HAND, WEAPON }
            public readonly AttackMode mode;
            public AttackModeChange(AttackMode mode) => this.mode = mode;
        }

        public class MovementSpeedChange : IEvent
        {
            public readonly float speed;
            public MovementSpeedChange(float speed) => this.speed = speed;
        }

        public class ScopeChange : IEvent
        {
            public readonly bool isScoping;
            public ScopeChange(bool isScoping) => this.isScoping = isScoping;
        }

        public class InputAim : IEvent
        {
            public readonly bool isAiming;
            public InputAim(bool isAiming) => this.isAiming = isAiming;
        }

        public class InputAttack : IEvent
        {
            public readonly bool isAttacking;
            public InputAttack(bool isAttacking) => this.isAttacking = isAttacking;
        }

        public class InputLook : IEvent
        {
            public readonly Vector2 direction;
            public InputLook(Vector2 direction) => this.direction = direction;
        }

        public class InputMove : IEvent
        {
            public readonly Vector2 direction;
            public InputMove(Vector2 direction) => this.direction = direction;
        }

        public class InputScope : IEvent
        {
            public readonly bool isScoping;
            public InputScope(bool isScoping) => this.isScoping = isScoping;
        }

        public class InputStance : IEvent
        {
            public readonly float value;
            public InputStance(float value) => this.value = value;
        }

        public class InputMoveModified : IEvent
        {
            public readonly bool isModified;
            public InputMoveModified(bool isModified) => this.isModified = isModified;
        }
    }
}
