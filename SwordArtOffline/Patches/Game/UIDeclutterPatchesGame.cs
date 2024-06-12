using HarmonyLib;
using LINK;
using LINK.UI;
using LINK.UI.HUDComponent;
using UnityEngine;

namespace SwordArtOffline.Patches.Game {
    public class UIDeclutterPatchesGame
    {

        [HarmonyPrefix, HarmonyPatch(typeof(HudComboCounter), "UpdateCounter")]
        static bool UpdateCounter(HudComboCounter __instance, int comboCount, out HudComboBase.State state)
        {
            state = HudComboBase.State.Less10;
            if (!Plugin.ConfigUIShowComboMeter.Value)
            {
                GameObject obj = GameObject.Find("RootObject/Canvas/HitCounter");
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
            return Plugin.ConfigUIShowComboMeter.Value;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CommentObj), "CommentIn")]
        static bool CommentIn(bool switchFlag, bool connectCommentFlag, string outputText, bool isToPlayer = false)
        {
            return Plugin.ConfigUIShowComments.Value;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(StaticUnitData), "ShowArrow", MethodType.Getter)]
        static bool get_ShowArrow() {
            return Plugin.ConfigUIShowDirectionalArrows.Value;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CommonUI_mapControl), "InitGameobject")]
        static void InitGameobject(CommonUI_mapControl __instance) {
            if (!Plugin.ConfigUIShowMinimap.Value) {
                Plugin.Log.LogInfo("Hiding minimap");
                __instance.MiniMap_raw.SetActive(false);
                __instance.Field_map.SetActive(false);
                __instance.Fieldmap_frame.SetActive(false);
                /*GameObject map = GameObject.Find("RootObject/Canvas/Field_map");
                if (map != null) {
                    map.SetActive(false);
                }*/
            }
        }

    }
}
