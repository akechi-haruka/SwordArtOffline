using HarmonyLib;
using LINK.CriWareSound;
using LINK.UI;
using LINK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SwordArtOffline.Patches.Game {
    internal class DataDestructionOverwriteSystem {

        [HarmonyPrefix, HarmonyPatch(typeof(HudManager), "PlayYuiVoice")]
        static bool PlayYuiVoice(string yuiVoiceID) {
            return !Plugin.ConfigUIMURDERYUI.Value;
        }

        private static void MurderYui() {
            GameObject yui = GameObject.Find("RootObject/Canvas/Skill_gauge/Skillup/yousei_pos/skillup_fuchi_a");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/Skill_gauge/Skillup/yousei_pos/yui_49_hakari");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/Skill_gauge/Skillup/yousei_pos/yui_49_un");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/TutorialsOfHUD/Anim/YuiComment/YUI");
            if (yui != null) {
                //GameObject.Destroy(yui);
                yui.SetActive(false);
            }
            yui = GameObject.Find("RootObject/Canvas/TutorialsOfHUD/Anim/YuiComment/Comment/Eff_TextHighlight");
            if (yui != null) {
                //GameObject.Destroy(yui);
                yui.SetActive(false);
            }
            yui = GameObject.Find("RootObject/Canvas/Canvas_Mission/congratulations_s/congratulations_matome/cngr_yousei_ichi");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/Canvas_Mission/Missionstart_s/missionstart_top/yui_pos_fuyuu");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/Debris/Debris_drop/logget_all/yui_pos_fuyuu");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/cutin_boss/boss_cutin_matome/yui_pos_fuyuu");
            if (yui != null) {
                yui.transform.position = new Vector3(9999, 9999, 9999);
                yui.SetActive(false);
                //UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/ResultMain/Bonus/Analyze/main/YuiChance/gasha_yuichance/yuichance_matome/yuichance_yui_matome/yuichance_yui_pos_fuyuu");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/Canvas/Message/Character/YUI");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            yui = GameObject.Find("RootObject/OverlayCanvas/RandomBonus/Anim/Yui");
            if (yui != null) {
                UnityEngine.Object.Destroy(yui);
            }
            // 
        }

        [HarmonyPrefix, HarmonyPatch(typeof(YuiComment_tutorial), "PlayYuiComment")]
        static bool PlayYuiComment(YuiCommentUnit unit) {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(YuiComment_tutorial), "PlayYuiComment")]
        static bool PlayYuiComment(YuiComment_tutorial __instance, YuiCommentUnit unit) {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
                __instance.HightLightEffect = new GameObject().transform;
            }
            return true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(YuiCommentUnit), MethodType.Constructor, typeof(StaticQuestTutorialData))]
        static void YuiCommentUnitCtor(YuiCommentUnit __instance, StaticQuestTutorialData record) {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                __instance.VoiceID = string.Empty;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CommonUI_skillsControll), "PlaySkillLevelUp")]
        static void PlaySkillLevelUp(CommonUI_skillsControll __instance) {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CommonUI_MissionList), "PlayQuestEnd")]
        static void PlaySkillLevelUp(CommonUI_MissionList __instance) {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ADXSoundManager), "Play", typeof(string), typeof(Func<Vector3>))]
        static bool Play(string tagName, Func<Vector3> func = null) {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                if (tagName != null) {
                    if (tagName.Contains("CH_YUI")) {
                        return false;
                    }
                }
            }
            return true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(UIResultHeroLogAnalyzeMainControl), "StartYuiChance")]
        static void StartYuiChance() {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CommonUI_CutInPlay.Boss), "playBossCutInAtNow")]
        static IEnumerator playBossCutInAtNow(IEnumerator __result, int unitID) {

            // Run original enumerator code
            while (__result.MoveNext())
                yield return __result.Current;

            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(CommonUI_Logout), "changeChracter")]
        static void changeChracter() {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(QuestVsManager), "LotRandomBonus")]
        static void LotRandomBonus() {
            if (Plugin.ConfigUIMURDERYUI.Value) {
                MurderYui();
            }
        }

    }
}
