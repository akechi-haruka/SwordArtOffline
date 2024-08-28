using HarmonyLib;
using LINK;
using LINK.UI;
using LINK.UI.HUDComponent;
using LINK.UI.HUDUtils;
using System.Collections;
using UnityEngine;

namespace SwordArtOffline.Patches.Game {
    public class UIDeclutterPatchesGame {

        [HarmonyPrefix, HarmonyPatch(typeof(HudComboCounter), "UpdateCounter")]
        static bool UpdateCounter(HudComboCounter __instance, int comboCount, out HudComboBase.State state) {
            state = HudComboBase.State.Less10;
            if (!Plugin.ConfigUIShowComboMeter.Value) {
                GameObject obj = GameObject.Find("RootObject/Canvas/HitCounter");
                if (obj != null) {
                    obj.SetActive(false);
                }
            }
            return Plugin.ConfigUIShowComboMeter.Value;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CommentObj), "CommentIn")]
        static bool CommentIn(bool switchFlag, bool connectCommentFlag, string outputText, bool isToPlayer = false) {
            return Plugin.ConfigUIShowComments.Value;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(StaticUnitData), "ShowArrow", MethodType.Getter)]
        static bool get_ShowArrow() {
            return Plugin.ConfigUIShowDirectionalArrows.Value;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(HudEXBonusController), "Start")]
        static IEnumerator Start(IEnumerator __result, HudEXBonusController __instance) {

            // Run original enumerator code
            while (__result.MoveNext())
                yield return __result.Current;

            if (Plugin.ConfigUIBonusStartsExpanded.Value && !QuestManager.Instance.IsTutorialQuest) {
                __instance.OpenWindow();
            }

            yield return __result.Current;

        }

        [HarmonyPostfix, HarmonyPatch(typeof(CommonUI_treasure), "init")]
        static void OnInitPrefab(CommonUI_treasure __instance, int CounterNum, float gaugeFill) {

            if (Plugin.ConfigUIShowMinimap.Value == Plugin.MinimapUILayout.HideMinimapAndMoveBonus && !QuestManager.Instance.IsTutorialQuest && !QuestManager.IsVsMode) {
                Vector3 vec = __instance.Debris_obj.localPosition;
                __instance.Debris_obj.localPosition = new Vector3(vec.x, vec.y + 100, vec.z);
            }

        }

        [HarmonyPostfix, HarmonyPatch(typeof(HudEXBonusController), "LateUpdate")]
        static void OnLateUpdate(HudEXBonusController __instance) {

            Transform t = __instance.exBonusCanvas.gameObject.FindChild("BonusWindow")?.transform;

            if (Plugin.ConfigUIShowMinimap.Value == Plugin.MinimapUILayout.HideMinimapAndMoveBonus && t?.localPosition.y < 200 && HudBonusMissionUtils.IsVisible && !QuestManager.Instance.IsTutorialQuest && !QuestManager.IsVsMode) {
                Vector3 vec = t.localPosition;
                t.localPosition = new Vector3(vec.x, vec.y + 380, vec.z);
                Plugin.Log.LogDebug("moved to " + t.localPosition);
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CommonUI_mapControl), "SetDisplay")]
        static bool SetDisplay(CommonUI_mapControl __instance, bool dispUI) {

            if (Plugin.ConfigUIShowMinimap.Value != Plugin.MinimapUILayout.Normal && !QuestManager.Instance.IsTutorialQuest && !QuestManager.IsVsMode) {
                return false;
            }

            return true;

        }

        [HarmonyPostfix, HarmonyPatch(typeof(DebrisParticleControll), MethodType.Constructor, typeof(Transform))]
        static void DebrisParticleControllCtor(DebrisParticleControll __instance, Transform rootTransform) {
            if (Plugin.ConfigUIShowMinimap.Value == Plugin.MinimapUILayout.HideMinimapAndMoveBonus && !QuestManager.Instance.IsTutorialQuest && !QuestManager.IsVsMode) {
                Vector3 vec = __instance.m_TargetTransform.localPosition;
                __instance.m_TargetTransform.localPosition = new Vector3(vec.x, vec.y + 100, vec.z);
            }
        }

    }
}
