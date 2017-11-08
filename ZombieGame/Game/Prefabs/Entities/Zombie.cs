using ZombieGame.Game.Enums;
using ZombieGame.IO;

namespace ZombieGame.Game.Prefabs.Entities
{
    class Zombie : Enemy
    {
        public Zombie() : base(EnemyTypes.Walker)
        {
            LoadSprite(GlobalPaths.CharacterSprites + "zombie.png");
            RigidBody.Resize(new Physics.Vector(50, 50));
        }
    }
}
