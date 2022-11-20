namespace Game.System
{
    public interface IComponent {
        public void Tick(GameSession session);
    }
}