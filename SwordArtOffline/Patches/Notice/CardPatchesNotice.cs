using HarmonyLib;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SwordArtOffline.Patches.Notice
{

    extern alias AssemblyNotice;

    public static class CardPatchesNotice
    {

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "Initialize")]
        static bool Initialize(ref bool __result, string mac_addr)
        {
            Plugin.Log.LogInfo("BNGRW Initialize");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "FinalizeFunc")]
        static bool FinalizeFunc()
        {
            Plugin.Log.LogInfo("BNGRW Finalize");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "Update")]
        static bool Update()
        {
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "CommonClearResourceConfig")]
        static bool CommonClearResourceConfig(ref bool __result, IntPtr config)
        {
            Plugin.Log.LogDebug("BNGRW.CommonClearResourceConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "CommonClearConfig")]
        static bool CommonClearConfig(ref bool __result, IntPtr config)
        {
            Plugin.Log.LogDebug("BNGRW.CommonClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "ClearConfig")]
        static bool ClearConfig(ref bool __result, IntPtr config)
        {
            Plugin.Log.LogDebug("BNGRW.ClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "ClearFatalError")]
        static bool ClearFatalError(ref bool __result, IntPtr error)
        {
            Plugin.Log.LogDebug("BNGRW.ClearFatalError");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "IsReaderTaskEnd")]
        static bool IsReaderTaskEnd(ref bool __result, int index = 0)
        {
            Plugin.Log.LogDebug("BNGRW.IsReaderTaskEnd");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "getErrorCount")]
        static bool getErrorCount(ref int __result)
        {
            Plugin.Log.LogDebug("BNGRW.getErrorCount");
            __result = 0;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "SequenceSwitchMode")]
        static bool SequenceSwitchMode(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.SequenceSwitchMode");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "SequenceGetPlayerTotal")]
        static bool SequenceGetPlayerTotal(ref int __result)
        {
            Plugin.Log.LogDebug("BNGRW.SequenceGetPlayerTotal");
            __result = 1;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "SequenceStartPlay")]
        static bool SequenceStartPlay(ref bool __result, int player)
        {
            Plugin.Log.LogDebug("BNGRW.SequenceStartPlay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "SequenceContinuePlay")]
        static bool SequenceContinuePlay(ref bool __result, int player)
        {
            Plugin.Log.LogDebug("BNGRW.SequenceContinuePlay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "SequenceEndPlay")]
        static bool SequenceEndPlay(ref bool __result, int player)
        {
            Plugin.Log.LogDebug("BNGRW.SequenceEndPlay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "SequenceIsPlaying")]
        static bool SequenceIsPlaying(ref bool __result, int player)
        {
            Plugin.Log.LogDebug("BNGRW.SequenceIsPlaying");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeClearConfig")]
        static bool AimeClearConfig(ref bool __result, IntPtr config)
        {
            Plugin.Log.LogDebug("BNGRW.AimeClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeClearCardReader")]
        static bool AimeClearCardReader(ref bool __result, IntPtr config)
        {
            Plugin.Log.LogDebug("BNGRW.AimeClearCardReader");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeToStringAccessCode")]
        static bool AimeToStringAccessCode(ref bool __result, IntPtr pSrc, IntPtr wszDst, int dstSize)
        {
            Plugin.Log.LogDebug("BNGRW.AimeToStringAccessCode");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeIsEnable")]
        static bool AimeIsEnable(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.AimeIsEnable");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeGetCardReaderTotal")]
        static bool AimeGetCardReaderTotal(ref int __result)
        {
            Plugin.Log.LogDebug("BNGRW.AimeGetCardReaderTotal");
            __result = 1;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeDetectCard")]
        static bool AimeDetectCard(ref bool __result, int index)
        {
            Plugin.Log.LogDebug("BNGRW.AimeDetectCard");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeAcceptTask")]
        static bool AimeAcceptTask(ref bool __result, int index)
        {
            Plugin.Log.LogDebug("BNGRW.AimeAcceptTask");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeCancelTask")]
        static bool AimeCancelTask(ref bool __result, int index)
        {
            Plugin.Log.LogDebug("BNGRW.AimeCancelTask");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeSetLed")]
        static bool AimeSetLed(ref bool __result, int index, byte red, byte green, byte blue)
        {
            Plugin.Log.LogDebug("BNGRW.AimeSetLed");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeControlLed")]
        static bool AimeControlLed(ref bool __result, int index, AssemblyNotice::LINK.bnReader.AimeLedLightPattern pattern)
        {
            Plugin.Log.LogDebug("BNGRW.AimeControlLed");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeGetCardReaderProperty")]
        static bool AimeGetCardReaderProperty(ref bool __result, int index, IntPtr prop)
        {
            //Plugin.Log.LogDebug("BNGRW.AimeGetCardReaderProperty");
            var property = new AssemblyNotice::LINK.bnReader.AimeCardReaderProperty()
            {
                bConnect = 1,
                bHasLedBoard = 1,
                generation = 1,
                wszFirmwareVersion = "EMULATOR",
                wszHardwareVersion = "EMULATOR",
                wszLedBoardFirmwareVersion = "EMULATOR",
                wszLedBoardHardwareVersion = "EMULATOR"
            };
            Marshal.StructureToPtr(property, prop, false);
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeGetNfcChipProperty")]
        static bool AimeGetNfcChipProperty(ref bool __result, int index, IntPtr prop)
        {
            Plugin.Log.LogDebug("BNGRW.AimeGetNfcChipProperty");
            byte[] bytes = Encoding.ASCII.GetBytes(Plugin.CardID.Value);
            byte[] chipid = new byte[16];
            Array.Copy(bytes, chipid, 16);
            var property = new AssemblyNotice::LINK.bnReader.AimeNfcChipProperty()
            {
                bMobile = 0,
                chipId = chipid,
            };
            Marshal.StructureToPtr(property, prop, false);
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "AimeGetAccessCode")]
        static bool AimeGetAccessCode(ref bool __result, int index, IntPtr code)
        {
            Plugin.Log.LogDebug("BNGRW.AimeGetAccessCode");
            byte[] bytes = new byte[10];
            var accesscode = new AssemblyNotice::LINK.bnReader.AimeAccessCode
            {
                bValid = 1,
                data = bytes
            };
            Marshal.StructureToPtr(accesscode, code, false);
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyClearConfig")]
        static bool EmoneyClearConfig(ref bool __result, IntPtr config)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyClearDealResult")]
        static bool EmoneyClearDealResult(ref bool __result, IntPtr result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyClearDealResult");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyClearErrorInfo")]
        static bool EmoneyClearErrorInfo(ref bool __result, IntPtr info)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyClearErrorInfo");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyIsEnable")]
        static bool EmoneyIsEnable(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsEnable");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyQueryBalance")]
        static bool EmoneyQueryBalance(ref bool __result, AssemblyNotice::LINK.bnReader.EmoneyBrand brand)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyQueryBalance");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyPayAmount")]
        static bool EmoneyPayAmount(ref bool __result, uint value, AssemblyNotice::LINK.bnReader.EmoneyBrand brand)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyPayAmount");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyCompletePay")]
        static bool EmoneyCompletePay(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyCompletePay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyCancel")]
        static bool EmoneyCancel(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyCancel");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyActivate")]
        static bool EmoneyActivate(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyActivate");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyDeactivate")]
        static bool EmoneyDeactivate(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyDeactivate");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyIsAvailable")]
        static bool EmoneyIsAvailable(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsAvailable");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyIsActivated")]
        static bool EmoneyIsActivated(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsActivated");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetTerminalId")]
        static bool EmoneyGetTerminalId(ref IntPtr __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetTerminalId");
            __result = Marshal.StringToHGlobalAnsi("EMULATOR");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneygetTerminalSerial")]
        static bool EmoneygetTerminalSerial(ref IntPtr __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneygetTerminalSerial");
            __result = Marshal.StringToHGlobalAnsi("EMULATOR");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyIsAliveServer")]
        static bool EmoneyIsAliveServer(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsAliveServer");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyIsReporting")]
        static bool EmoneyIsReporting(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsReporting");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetBrandInfo")]
        static bool EmoneyGetBrandInfo(ref bool __result, AssemblyNotice::LINK.bnReader.EmoneyBrand brand, IntPtr info)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetBrandInfo");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetAvailableBrandTotal")]
        static bool EmoneyGetAvailableBrandTotal(ref int __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetAvailableBrandTotal");
            __result = 1;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetAvailableBrandInfo")]
        static bool EmoneyGetAvailableBrandInfo(ref bool __result, int index, IntPtr info)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetAvailableBrandInfo");
            var brand = new AssemblyNotice::LINK.bnReader.EmoneyBrandInfo()
            {
                code = AssemblyNotice::LINK.bnReader.EmoneyBrand.EMONEY_BRAND_TRANSPORT,
                wszName = "TRANSPORT",
                wszIconFilePath = "0005-01-00.png"
            };
            Marshal.StructureToPtr(brand, info, false);
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetDealLog")]
        static bool EmoneyGetDealLog(ref bool __result, IntPtr log)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetDealLog");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetReportLog")]
        static bool EmoneyGetReportLog(ref bool __result, IntPtr log)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetReportLog");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetPlaySound")]
        static bool EmoneyGetPlaySound(ref IntPtr __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetPlaySound");
            __result = new IntPtr(0);
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyIsProcedureEnd")]
        static bool EmoneyIsProcedureEnd(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsProcedureEnd");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "EmoneyGetErrorCode")]
        static bool EmoneyGetErrorCode(ref AssemblyNotice::LINK.bnReader.EmoneyDealResult __result)
        {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetErrorCode");
            __result = new AssemblyNotice::LINK.bnReader.EmoneyDealResult()
            {
            };
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "VFDStartTest")]
        static bool VFDStartTest(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.VFDStartTest");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::LINK.bnReader), "VFDCancelTest")]
        static bool VFDCancelTest(ref bool __result)
        {
            Plugin.Log.LogDebug("BNGRW.VFDCancelTest");
            __result = true;
            return false;
        }

    }
}
