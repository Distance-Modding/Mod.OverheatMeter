using System;

namespace Distance.Overheatmeter
{
    public static class Options
    {
        public static GameManager Game => G.Sys.GameManager_;

        public static OptionsManager OptionsManager => G.Sys.OptionsManager_;

        public static class General
        {
            public static GeneralSettings Manager => OptionsManager.General_;

            public static bool MenuAnimations
            {
                get => Manager.menuAnimations_;
                set => Manager.menuAnimations_ = value;
            }

            public static Units Units
            {
                get => Manager.Units_;
                set => Manager.Units_ = value;
            }
        }
    }
}
