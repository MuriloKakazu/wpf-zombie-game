using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZombieGame.UI
{
    public static class UserControls
    {
        public static StoreUC StoreControl;

        public static void Setup()
        {
            StoreControl = new StoreUC();
        }
    }
}
