using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;

namespace MapScrollWheelZoomFactor
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class MapScrollWheelZoomFactor : BaseUnityPlugin
    {
        public const string PluginGUID = "orax.mapscrollwheelzoomfactor";
        public const string PluginName = "Map scroll wheel zoom factor";
        public const string PluginVersion = "0.1.0";

        public static ConfigEntry<float> ZoomFactor;

        private static Harmony _hi;

        private void Awake()
        {
            ZoomFactor = Config.Bind<float>("General", "ZoomFactor", 2f, "");

            _hi = new Harmony(PluginGUID);
            _hi.PatchAll();
        }

        private void OnDestroy()
        {
            _hi?.UnpatchSelf();
        }
    }
}