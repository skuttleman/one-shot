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
            public enum AttackMode { NONE, HAND, WEAPON, FIRING, PUNCHING }
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
    }
}
