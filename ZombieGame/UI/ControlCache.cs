namespace ZombieGame.UI
{
    public static class ControlCache
    {
        public static WeaponInfoUI WeaponInfo;
        public static StoreUC StoreControl;
        public static ProjectileInfoUI ProjectileInfo;
        public static ChoosePlayerUI ChoosePlayer;
        public static PauseMenuUI PauseMenu;

        public static void Setup()
        {
            StoreControl = new StoreUC();
            WeaponInfo = new WeaponInfoUI();
            ProjectileInfo = new ProjectileInfoUI();
            ChoosePlayer = new ChoosePlayerUI();
            PauseMenu = new PauseMenuUI();
        }
    }
}
