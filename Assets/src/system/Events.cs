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
    }
}