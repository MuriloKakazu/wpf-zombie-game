namespace ZombieGame.Game.Interfaces
{
    public interface IDestroyable
    {
        bool IsActive { get; }

        void Destroy();
    }
}
