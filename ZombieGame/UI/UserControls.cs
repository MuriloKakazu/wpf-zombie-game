namespace ZombieGame.UI
{
    public static class UserControls
    {
        public static WeaponInfoUI WeaponInfo;
        public static StoreUC StoreControl;
        public static ProjectileInfoUI ProjectileInfo;
        public static ChoosePlayerUI ChoosePlayer;

        public static void Setup()
        {
            StoreControl = new StoreUC();
            WeaponInfo = new WeaponInfoUI();
            ProjectileInfo = new ProjectileInfoUI();
            ChoosePlayer = new ChoosePlayerUI();
        }
    }
}
