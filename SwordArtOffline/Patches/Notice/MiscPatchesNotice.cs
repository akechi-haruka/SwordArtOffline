using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SwordArtOffline.Patches.Notice {
    internal class MiscPatchesNotice {

        static bool timeAdjusted;

        [HarmonyPostfix, HarmonyPatch("Boot, Assembly-CSharp", "Update")]
        static void Update(object __instance) {
            if (!timeAdjusted && Plugin.ConfigNoticeJapanTimer.Value != 10) {
                FieldInfo f = AccessTools.DeclaredField(__instance.GetType(), "timeCount");
                if ((float)f.GetValue(__instance) > Plugin.ConfigNoticeJapanTimer.Value) {
                    f.SetValue(__instance, Plugin.ConfigNoticeJapanTimer.Value * 1F);
                    timeAdjusted = true;
                }
            }
        }

    }
}
