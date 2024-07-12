using HarmonyLib;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SwordArtOffline.Patches.Notice {

    extern alias AssemblyNotice;

    public class KeychipPatchesNoticeAndTest {

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.nbamUsbFinder), "GetSerialNumber")]
        public static bool GetSerialNumber(ref AssemblyNotice::LINK.NbamUsbFindError __result, ulong usercode, [Out] IntPtr serialnumber) {
            Plugin.Log.LogDebug(serialnumber);
            string id = Plugin.GetKeychip();
            byte[] kcbytes = Encoding.ASCII.GetBytes(id + char.MinValue);
            Marshal.Copy(kcbytes, 0, serialnumber, kcbytes.Length);
            __result = AssemblyNotice::LINK.NbamUsbFindError.NBAM_USBFIND_SUCCESS;
            return false;
        }

    }
}
