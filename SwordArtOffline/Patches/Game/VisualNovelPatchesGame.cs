using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;

namespace SwordArtOffline.Patches.Game {
    internal class VisualNovelPatchesGame {

        /*[HarmonyPrefix, HarmonyPatch(typeof(TMP_Text), "SetText", typeof(string), typeof(bool))]
        static bool SetText(string text, bool syncTextInputBox) {
            Plugin.Log.LogDebug("TextMeshPro: " + text);
            return true;
        }*/
    }
}
