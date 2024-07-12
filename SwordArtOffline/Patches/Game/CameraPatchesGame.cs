using HarmonyLib;
using LINK;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SwordArtOffline.Patches.Game {
    public class CameraPatchesGame {

        [HarmonyPostfix, HarmonyPatch(typeof(WebCamTexture), "devices", MethodType.Getter)]
        static void devices(WebCamTexture __instance, ref WebCamDevice[] __result) {
            if (Plugin.ConfigForceCamera.Value.Length > 0) {
                __result = __result.Where((c) => c.name.Contains(Plugin.ConfigForceCamera.Value)).ToArray();
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(nbamQr), "GetQrImageSize")]
        public static void GetQrImageSize([In][Out] nbamQr_encode_info_t info, ref nbamQr_retval_t __result) {
            Plugin.Log.LogDebug("QR ImageSize result: " + __result);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(nbamQr), "Encode")]
        public static void Encode([In][Out] nbamQr_encode_info_t info, ref nbamQr_retval_t __result) {
            Plugin.Log.LogDebug("QR Encode result: " + __result);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(nbamQr), "Decode")]
        static void Decode(nbamQr_imaging_param_t imgparam, [In][Out] nbamQr_decode_info_t info, ref nbamQr_retval_t __result) {
            Plugin.Log.LogDebug("QR Decode result: " + __result);
            if (__result == nbamQr_retval_t.NBAMQR_RETVAL_NO_ERROR) {
                byte[] array2 = new byte[info.decodedStrLen];
                Marshal.Copy(info.str, array2, 0, (int)info.decodedStrLen);
                Plugin.Log.LogInfo("Card data: " + qrUtils._instance.decryptData(array2, out _));
            }
        }

    }
}
