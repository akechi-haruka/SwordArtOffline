using HarmonyLib;
using System.Diagnostics;
using UnityEngine;

namespace SwordArtOffline.Patches.Shared
{

    extern alias AssemblyNotice;

    public class UnityPatches
    {
        public static int FakeTouchX { get; internal set; }
        public static int FakeTouchY { get; internal set; }
        public static int FakeFrames { get; internal set; }

        [HarmonyPrefix, HarmonyPatch(typeof(UnityEngine.Debug), "LogWarning", typeof(object))]
        static bool LogWarning(object message)
        {
            Plugin.Log.LogWarning("[Unity] " + message);
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(UnityEngine.Debug), "LogError", typeof(object))]
        static bool LogError(object message)
        {
            Plugin.Log.LogWarning("[Unity] " + message);
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(UnityEngine.Debug), "Log", typeof(object))]
        static bool Log(object message)
        {
            Plugin.Log.LogInfo("[Unity] " + message);
            return true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Input), "GetMouseButtonDown", typeof(int))]
        static void GetMouseButtonDown(ref bool __result, int button) {
            if (FakeFrames > 0) {
                __result = true;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Input), "mousePosition", MethodType.Getter)]
        static void mousePosition(ref Vector3 __result) {
            if (FakeFrames > 0) {
                __result = new Vector3(FakeTouchX, FakeTouchY);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(Input), "GetMouseButtonUp", typeof(int))]
        static void GetMouseButtonUp(ref bool __result, int button) {
            if (FakeFrames == 1) { // on the last frame, send up
                __result = true;
            }
        }

    }
}
