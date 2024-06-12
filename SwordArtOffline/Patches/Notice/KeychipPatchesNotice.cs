using HarmonyLib;

namespace SwordArtOffline.Patches.Notice
{

    extern alias AssemblyNotice;

    public class KeychipPatchesNotice
    {
        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.nbamUsbFinder), "GetSerialNumber_wrapped")]
        static bool GetSerialNumber_wrapped(ref AssemblyNotice::LINK.NbamUsbFindError __result, out string serial_str)
        {
            __result = AssemblyNotice::LINK.NbamUsbFindError.NBAM_USBFIND_SUCCESS;
            serial_str = Plugin.KeychipId;
            return false;
        }
    }
}
