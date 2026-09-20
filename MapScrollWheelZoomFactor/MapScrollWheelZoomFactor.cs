using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace MapScrollWheelZoomFactor
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class MapScrollWheelZoomFactor : BaseUnityPlugin
    {
        public const string PluginGUID = "orax.MapScrollWheelZoomFactor";
        public const string PluginName = "MapScrollWheelZoomFactor";
        public const string PluginVersion = "0.1.1";

        internal static new ManualLogSource Log;

        public static ConfigEntry<float> ZoomFactor;

        private static Harmony _hi;

        private void Awake()
        {
            Log = base.Logger;

            ZoomFactor = Config.Bind<float>("General", "Zoom factor (game restart required)", 6f, "Adjust the speed at which you can zoom in and out of the map with the mouse scroll wheel. Game default: 2. Higher value = faster zoom in/out.");

            _hi = new Harmony(PluginGUID);
            _hi.PatchAll();
        }

        private void OnDestroy()
        {
            _hi?.UnpatchSelf();
        }
    }
}
