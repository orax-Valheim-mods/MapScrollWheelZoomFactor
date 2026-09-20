using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MapScrollWheelZoomFactor;

[HarmonyPatch]
public static class Minimap_Patch
{
    [HarmonyPatch(typeof(Minimap), nameof(Minimap.UpdateMap))]
    public static class UpdateMap_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            /*
             * PATCH:
             * num = Mathf.Clamp(num, -0.05f, 0.05f) * this.m_largeZoom * 2f;
             * 
             * AFTER:
             * num = Mathf.Clamp(num, -0.05f, 0.05f) * this.m_largeZoom * <ZoomMultiplier>;
             */

            CodeMatcher cm = new(instructions);

            cm.MatchForward(true, // false = move at the start of the match, true = move at the end of the match)
              new CodeMatch(OpCodes.Ldloc_0),
              new CodeMatch(OpCodes.Ldc_R4),
              new CodeMatch(OpCodes.Ldc_R4),
              new CodeMatch(OpCodes.Call),
              new CodeMatch(OpCodes.Ldarg_0),
              new CodeMatch(OpCodes.Ldfld),
              new CodeMatch(OpCodes.Mul),
              new CodeMatch(OpCodes.Ldc_R4),
              new CodeMatch(OpCodes.Mul),
              new CodeMatch(OpCodes.Stloc_0));

            if (cm.IsValid)
            {
                cm.Advance(-2);
                cm.SetOperandAndAdvance(MapScrollWheelZoomFactor.ZoomFactor.Value);
            }
            else
            {
                MapScrollWheelZoomFactor.Log.LogError("Unable to patch Minimap.UpdateMap(). Pattern not found.");
            }

            return cm.InstructionEnumeration();
        }
    }
}
