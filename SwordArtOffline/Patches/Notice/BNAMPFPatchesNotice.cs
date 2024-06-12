using HarmonyLib;
using System;
using System.Runtime.InteropServices;

namespace SwordArtOffline.Patches.Notice
{

    extern alias AssemblyNotice;
    using static AssemblyNotice::LINK.bnAMPF;
    using static AssemblyNotice::LINK.bnAMPF.EMoneyBrand;

    public static class BNAMPFPatchesNotice
    {

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyStartActivate")]
        [HarmonyPrefix]
        static bool EMoneyStartActivate(ref bool __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyStartActivate");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyStartDeactivate")]
        [HarmonyPrefix]
        static bool EMoneyStartDeactivate(ref bool __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyStartDeactivate");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyStartPay")]
        [HarmonyPrefix]
        static bool EMoneyStartPay(ref bool __result, EMoneyBrand brand, int value)
        {
            Plugin.Log.LogWarning("N/I: EMoneyStartPay");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyStartPayForCredit")]
        [HarmonyPrefix]
        static bool EMoneyStartPayForCredit(ref bool __result, EMoneyBrand brand, int amount, int credits)
        {
            Plugin.Log.LogWarning("N/I: EMoneyStartPayForCredit");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyStartQueryBalance")]
        [HarmonyPrefix]
        static bool EMoneyStartQueryBalance(ref bool __result, EMoneyBrand brand)
        {
            Plugin.Log.LogWarning("N/I: EMoneyStartQueryBalance");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterGetStatus")]
        [HarmonyPrefix]
        static bool LeafPrinterGetStatus(ref LeafPrinterStatus __result)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterGetStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterGetPrinterInfo")]
        [HarmonyPrefix]
        static bool LeafPrinterGetPrinterInfo(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterGetPrinterInfo");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterGetMediaStatus")]
        [HarmonyPrefix]
        static bool LeafPrinterGetMediaStatus(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterGetMediaStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterGetPrintResult")]
        [HarmonyPrefix]
        static bool LeafPrinterGetPrintResult(ref LeafPrinterPrintResult __result)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterGetPrintResult");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterSetFilter")]
        [HarmonyPrefix]
        static bool LeafPrinterSetFilter(LeafPrinterFilter filter)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetFilter");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterSetICCFile")]
        [HarmonyPrefix]
        static bool LeafPrinterSetICCFile(string inICC, string outICC)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetICCFile");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterSetGammaTable")]
        [HarmonyPrefix]
        static bool LeafPrinterSetGammaTable(float inR, float inG, float inB, float outR, float outG, float outB)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetGammaTable");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterSetGammaTableFile")]
        [HarmonyPrefix]
        static bool LeafPrinterSetGammaTableFile(string inGammaTable, string outGammaTable)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetGammaTableFile");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterSetImage")]
        [HarmonyPrefix]
        static bool LeafPrinterSetImage(LeafPrinterSurface surface, LeafPrinterImageType type, int width, int height, IntPtr pixels, int pixels_size)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetImage");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterClearImage")]
        [HarmonyPrefix]
        static bool LeafPrinterClearImage(LeafPrinterSurface surface, LeafPrinterImageType type)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterClearImage");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterStartPrint")]
        [HarmonyPrefix]
        static bool LeafPrinterStartPrint()
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterStartPrint");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterAbortPrint")]
        [HarmonyPrefix]
        static bool LeafPrinterAbortPrint()
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterAbortPrint");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LeafPrinterStartPrintUsage")]
        [HarmonyPrefix]
        static bool LeafPrinterStartPrintUsage(LeafPrinterPrintUsage usage)
        {
            Plugin.Log.LogWarning("N/I: LeafPrinterStartPrintUsage");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyAbort")]
        [HarmonyPrefix]
        static bool EMoneyAbort(ref bool __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyAbort");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyCompletePay")]
        [HarmonyPrefix]
        static bool EMoneyCompletePay(ref bool __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyCompletePay");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetAvailableBrand")]
        [HarmonyPrefix]
        static bool EMoneyGetAvailableBrand(ref IntPtr __result, int index)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetAvailableBrand");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetAvailableBrandCount")]
        [HarmonyPrefix]
        static bool EMoneyGetAvailableBrandCount(ref int __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetAvailableBrandCount");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetDealLog")]
        [HarmonyPrefix]
        static bool EMoneyGetDealLog(ref IntPtr __result, int index)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetDealLog");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetDealLogCount")]
        [HarmonyPrefix]
        static bool EMoneyGetDealLogCount(ref int __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetDealLogCount");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetEnvironment")]
        [HarmonyPrefix]
        static bool EMoneyGetEnvironment(ref EMoneyEnvironment __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetEnvironment");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetReportLog")]
        [HarmonyPrefix]
        static bool EMoneyGetReportLog(ref IntPtr __result, int index)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetReportLog");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetReportLogCount")]
        [HarmonyPrefix]
        static bool EMoneyGetReportLogCount(ref int __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetReportLogCount");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetRequestSound")]
        [HarmonyPrefix]
        static bool EMoneyGetRequestSound(ref IntPtr __result, int index)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetRequestSound");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetRequestSoundCount")]
        [HarmonyPrefix]
        static bool EMoneyGetRequestSoundCount(ref int __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetRequestSoundCount");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetResult")]
        [HarmonyPrefix]
        static bool EMoneyGetResult(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetResult");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetStatus")]
        [HarmonyPrefix]
        static bool EMoneyGetStatus(ref EMoneyStatus __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetTerminalId")]
        [HarmonyPrefix]
        static bool EMoneyGetTerminalId(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetTerminalId");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EMoneyGetTerminalSerial")]
        [HarmonyPrefix]
        static bool EMoneyGetTerminalSerial(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: EMoneyGetTerminalSerial");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkGetInterfaceCount")]
        [HarmonyPrefix]
        static bool NetworkGetInterfaceCount(ref int __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkGetInterfaceCount");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkGetInterfaceStatus")]
        [HarmonyPrefix]
        static bool NetworkGetInterfaceStatus(ref IntPtr __result, int index)
        {
            Plugin.Log.LogWarning("N/I: NetworkGetInterfaceStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkGetLocalConnectionStatus")]
        [HarmonyPrefix]
        static bool NetworkGetLocalConnectionStatus(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkGetLocalConnectionStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkGetLocalNetworkStatus")]
        [HarmonyPrefix]
        static bool NetworkGetLocalNetworkStatus(ref NetworkStatus __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkGetLocalNetworkStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "UpdaterGetStatus")]
        [HarmonyPrefix]
        static bool UpdaterGetStatus(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: UpdaterGetStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "USBDongleGetSerialNumber")]
        [HarmonyPrefix]
        static bool USBDongleGetSerialNumber(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: USBDongleGetSerialNumber");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditAdd")]
        [HarmonyPrefix]
        static bool CreditAdd(ref int __result, CreditType type, int count)
        {
            Plugin.Log.LogWarning("N/I: CreditAdd");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditBeginUse")]
        [HarmonyPrefix]
        static bool CreditBeginUse(ref bool __result, CreditType type, int count)
        {
            Plugin.Log.LogWarning("N/I: CreditBeginUse");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditClear")]
        [HarmonyPrefix]
        static bool CreditClear()
        {
            Plugin.Log.LogWarning("N/I: CreditClear");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditEndUse")]
        [HarmonyPrefix]
        static bool CreditEndUse(CreditType type, int count)
        {
            Plugin.Log.LogWarning("N/I: CreditEndUse");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditGet")]
        [HarmonyPrefix]
        static bool CreditGet(ref int __result, CreditType type)
        {
            //Plugin.Log.LogWarning("N/I: CreditGet");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditGetCleared")]
        [HarmonyPrefix]
        static bool CreditGetCleared(ref int __result, CreditType type)
        {
            Plugin.Log.LogWarning("N/I: CreditGetCleared");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditGetClearedLastRecord")]
        [HarmonyPrefix]
        static bool CreditGetClearedLastRecord(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: CreditGetClearedLastRecord");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditGetInc")]
        [HarmonyPrefix]
        static bool CreditGetInc(ref int __result, CreditType type)
        {
            Plugin.Log.LogWarning("N/I: CreditGetInc");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditGetRecentlyUsed")]
        [HarmonyPrefix]
        static bool CreditGetRecentlyUsed(ref int __result, CreditType type)
        {
            Plugin.Log.LogWarning("N/I: CreditGetRecentlyUsed");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditSetAcceptability")]
        [HarmonyPrefix]
        static bool CreditSetAcceptability(CreditType type, bool value)
        {
            Plugin.Log.LogWarning("N/I: CreditSetAcceptability");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "CreditUse")]
        [HarmonyPrefix]
        static bool CreditUse(ref bool __result, CreditType type, int count)
        {
            Plugin.Log.LogWarning("N/I: CreditUse");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BasicIOGetInput")]
        [HarmonyPrefix]
        static bool BasicIOGetInput(ref int __result, BasicIOInputPort port)
        {
            if (port == BasicIOInputPort.SwitchTest)
            {
                __result = Plugin.ConfigButtonTest.Value.IsPressed() ? 1 : 0;
            }
            else if (port == BasicIOInputPort.SwitchService)
            {
                __result = Plugin.ConfigButtonService.Value.IsPressed() ? 1 : 0;
            }
            else if (port == BasicIOInputPort.SwitchTestDown)
            {
                __result = Plugin.ConfigButtonTestDown.Value.IsPressed() ? 1 : 0;
            }
            else if (port == BasicIOInputPort.SwitchTestUp)
            {
                __result = Plugin.ConfigButtonTestUp.Value.IsPressed() ? 1 : 0;
            }
            else if (port == BasicIOInputPort.SwitchEnter)
            {
                __result = Plugin.ConfigButtonTestEnter.Value.IsPressed() ? 1 : 0;
            }
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BasicIOGetUpdateCounter")]
        [HarmonyPrefix]
        static bool BasicIOGetUpdateCounter(ref long __result)
        {
            Plugin.Log.LogWarning("N/I: BasicIOGetUpdateCounter");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BasicIOSetOutput")]
        [HarmonyPrefix]
        static bool BasicIOSetOutput(BasicIOOutputPort port, int value)
        {
            Plugin.Log.LogWarning("N/I: BasicIOSetOutput");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassGetReaderInfo")]
        [HarmonyPrefix]
        static bool BanapassGetReaderInfo(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: BanapassGetReaderInfo");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassStartTest")]
        [HarmonyPrefix]
        static bool BanapassStartTest(BanapassTestTarget dev)
        {
            Plugin.Log.LogWarning("N/I: BanapassStartTest");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassAbortTest")]
        [HarmonyPrefix]
        static bool BanapassAbortTest(BanapassTestTarget dev)
        {
            Plugin.Log.LogWarning("N/I: BanapassAbortTest");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassGetTestResult")]
        [HarmonyPrefix]
        static bool BanapassGetTestResult(BanapassTestTarget dev)
        {
            Plugin.Log.LogWarning("N/I: BanapassGetTestResult");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassStartRead")]
        [HarmonyPrefix]
        static bool BanapassStartRead()
        {
            Plugin.Log.LogWarning("N/I: BanapassStartRead");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassAbortRead")]
        [HarmonyPrefix]
        static bool BanapassAbortRead()
        {
            Plugin.Log.LogWarning("N/I: BanapassAbortRead");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassGetReadResult")]
        [HarmonyPrefix]
        static bool BanapassGetReadResult(ref BanapassReadStatus __result)
        {
            Plugin.Log.LogWarning("N/I: BanapassGetReadResult");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BanapassGetReadCard")]
        [HarmonyPrefix]
        static bool BanapassGetReadCard(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: BanapassGetReadCard");
            return false;
        }

        /*[HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsTryGetBoolean")]
        [HarmonyPrefix]
        static bool SettingsTryGetBoolean(ref Boolean __result, String key, out Boolean value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetBoolean");
            value = false;
            __result = false;
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsTryGetInteger")]
        [HarmonyPrefix]
        static bool SettingsTryGetInteger(ref Boolean __result, String key, out Int32 value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetInteger");
            value = 0;
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsTryGetString")]
        [HarmonyPrefix]
        static bool SettingsTryGetString(ref Boolean __result, String key, out String value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetString");
            value = "";
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsTryGetTimeSpan")]
        [HarmonyPrefix]
        static bool SettingsTryGetTimeSpan(ref Boolean __result, String key, out AssemblyNotice::LINK.bnAMPF.TimeSpan value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetTimeSpan");
            value = new AssemblyNotice::LINK.bnAMPF.TimeSpan();
            return false;
        }*/

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingAbortGame")]
        [HarmonyPrefix]
        static bool LoggingAbortGame(string mode)
        {
            Plugin.Log.LogWarning("N/I: LoggingAbortGame");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingAddCommonLogInteger")]
        [HarmonyPrefix]
        static bool LoggingAddCommonLogInteger(LoggingCommonLogField key, int value)
        {
            Plugin.Log.LogWarning("N/I: LoggingAddCommonLogInteger");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingAddCommonLogTimeSpan")]
        [HarmonyPrefix]
        static bool LoggingAddCommonLogTimeSpan(LoggingCommonLogField key, IntPtr value)
        {
            Plugin.Log.LogWarning("N/I: LoggingAddCommonLogTimeSpan");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingAddPlayLogInteger")]
        [HarmonyPrefix]
        static bool LoggingAddPlayLogInteger(LoggingPlayLogField key, int value)
        {
            Plugin.Log.LogWarning("N/I: LoggingAddPlayLogInteger");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingAddPlayLogTimeSpan")]
        [HarmonyPrefix]
        static bool LoggingAddPlayLogTimeSpan(LoggingPlayLogField key, IntPtr value)
        {
            Plugin.Log.LogWarning("N/I: LoggingAddPlayLogTimeSpan");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingBeginGame")]
        [HarmonyPrefix]
        static bool LoggingBeginGame()
        {
            Plugin.Log.LogWarning("N/I: LoggingBeginGame");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingClearBackupedGameStatus")]
        [HarmonyPrefix]
        static bool LoggingClearBackupedGameStatus()
        {
            Plugin.Log.LogWarning("N/I: LoggingClearBackupedGameStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingEndGame")]
        [HarmonyPrefix]
        static bool LoggingEndGame(string mode)
        {
            Plugin.Log.LogWarning("N/I: LoggingEndGame");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingGetBackupedGameStatus")]
        [HarmonyPrefix]
        static bool LoggingGetBackupedGameStatus(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: LoggingGetBackupedGameStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingGetCommonLogStatistics")]
        [HarmonyPrefix]
        static bool LoggingGetCommonLogStatistics(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: LoggingGetCommonLogStatistics");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingGetErrorLogCount")]
        [HarmonyPrefix]
        static bool LoggingGetErrorLogCount(ref int __result)
        {
            Plugin.Log.LogWarning("N/I: LoggingGetErrorLogCount");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingGetErrorLogEntry")]
        [HarmonyPrefix]
        static bool LoggingGetErrorLogEntry(ref IntPtr __result, int index)
        {
            Plugin.Log.LogWarning("N/I: LoggingGetErrorLogEntry");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingGetPlayLogStatistics")]
        [HarmonyPrefix]
        static bool LoggingGetPlayLogStatistics(ref IntPtr __result, string mode, IntPtr timeFrom, IntPtr timeTo)
        {
            Plugin.Log.LogWarning("N/I: LoggingGetPlayLogStatistics");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingRecordError")]
        [HarmonyPrefix]
        static bool LoggingRecordError(Error error, LoggingErrorVisibility visibility)
        {
            Plugin.Log.LogWarning("N/I: LoggingRecordError");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingRestoreGame")]
        [HarmonyPrefix]
        static bool LoggingRestoreGame(IntPtr backup)
        {
            Plugin.Log.LogWarning("N/I: LoggingRestoreGame");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingSetGameStatus")]
        [HarmonyPrefix]
        static bool LoggingSetGameStatus(IntPtr status)
        {
            Plugin.Log.LogWarning("N/I: LoggingSetGameStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingSetPlayLogInteger")]
        [HarmonyPrefix]
        static bool LoggingSetPlayLogInteger(LoggingPlayLogField key, int value)
        {
            Plugin.Log.LogWarning("N/I: LoggingSetPlayLogInteger");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingSetPlayLogString")]
        [HarmonyPrefix]
        static bool LoggingSetPlayLogString(LoggingPlayLogField key, string value)
        {
            Plugin.Log.LogWarning("N/I: LoggingSetPlayLogString");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "LoggingSetPlayLogTimeSpan")]
        [HarmonyPrefix]
        static bool LoggingSetPlayLogTimeSpan(LoggingPlayLogField key, IntPtr value)
        {
            Plugin.Log.LogWarning("N/I: LoggingSetPlayLogTimeSpan");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkCheckLocalConnection")]
        [HarmonyPrefix]
        static bool NetworkCheckLocalConnection(ref bool __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkCheckLocalConnection");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkCheckNetworkConnection")]
        [HarmonyPrefix]
        static bool NetworkCheckNetworkConnection(ref bool __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkCheckNetworkConnection");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkGetAllNetStatus")]
        [HarmonyPrefix]
        static bool NetworkGetAllNetStatus(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkGetAllNetStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkGetCheckConnectionResult")]
        [HarmonyPrefix]
        static bool NetworkGetCheckConnectionResult(ref NetworkConnectionStatus __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkGetCheckConnectionResult");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "NetworkGetGlobalNetworkStatus")]
        [HarmonyPrefix]
        static bool NetworkGetGlobalNetworkStatus(ref NetworkStatus __result)
        {
            Plugin.Log.LogWarning("N/I: NetworkGetGlobalNetworkStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "BeginUpdate")]
        [HarmonyPrefix]
        static bool BeginUpdate(ServiceClientStatus clientStatus)
        {
            Plugin.Log.LogWarning("N/I: BeginUpdate");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ClearBackups")]
        [HarmonyPrefix]
        static bool ClearBackups(BackupCategory category)
        {
            Plugin.Log.LogWarning("N/I: ClearBackups");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "EndUpdate")]
        [HarmonyPrefix]
        static bool EndUpdate(ref long __result)
        {
            Plugin.Log.LogWarning("N/I: EndUpdate");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "GetError")]
        [HarmonyPrefix]
        static bool GetError(ref Error __result, int index)
        {
            Plugin.Log.LogWarning("N/I: GetError");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "GetErrorCount")]
        [HarmonyPrefix]
        static bool GetErrorCount(ref int __result)
        {
            __result = 0;
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "GetInitStatus")]
        [HarmonyPrefix]
        static bool GetInitStatus(ref ModuleStatus __result)
        {
            Plugin.Log.LogWarning("N/I: GetInitStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "GetModuleInitStatus")]
        [HarmonyPrefix]
        static bool GetModuleInitStatus(ref ModuleStatus __result, ModuleType type)
        {
            Plugin.Log.LogWarning("N/I: GetModuleInitStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "GetModuleStatus")]
        [HarmonyPrefix]
        static bool GetModuleStatus(ref ModuleStatus __result, ModuleType type)
        {
            Plugin.Log.LogWarning("N/I: GetModuleStatus");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ResetError")]
        [HarmonyPrefix]
        static bool ResetError(Error error)
        {
            Plugin.Log.LogWarning("N/I: ResetError");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ServiceClientClose")]
        [HarmonyPrefix]
        static bool ServiceClientClose()
        {
            Plugin.Log.LogWarning("N/I: ServiceClientClose");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ServiceClientOpen")]
        [HarmonyPrefix]
        static bool ServiceClientOpen(ServiceClientType type)
        {
            Plugin.Log.LogDebug("AMPF: ServiceClientOpen(" + type + ")");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "UpdateFunc")]
        [HarmonyPrefix]
        static bool UpdateFunc(ref long __result, ServiceClientStatus clientStatus)
        {
            __result = 0;
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ConfigGetCurrentTime")]
        [HarmonyPrefix]
        static bool ConfigGetCurrentTime(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: ConfigGetCurrentTime");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ConfigGetString")]
        [HarmonyPrefix]
        static bool ConfigGetString(ref bool __result, ConfigCategory category, string key, out IntPtr value)
        {
            Plugin.Log.LogWarning("N/I: ConfigGetString");
            value = Marshal.StringToHGlobalUni("");
            return false;
        }

        /*[HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ConfigGetBoolean")]
        [HarmonyPrefix]
        static bool ConfigGetBoolean(ref Boolean __result, ConfigCategory category, String key, out Boolean value) {
            Plugin.Log.LogWarning("N/I: ConfigGetBoolean");
            value = false;
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ConfigGetInteger")]
        [HarmonyPrefix]
        static bool ConfigGetInteger(ref Boolean __result, ConfigCategory category, String key, out Int32 value) {
            Plugin.Log.LogWarning("N/I: ConfigGetInteger");
            value = 0;
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "ConfigGetFloat")]
        [HarmonyPrefix]
        static bool ConfigGetFloat(ref Boolean __result, ConfigCategory category, String key, out Single value) {
            Plugin.Log.LogWarning("N/I: ConfigGetFloat");
            value = 0;
            return false;
        }*/

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsGetBoolean")]
        [HarmonyPrefix]
        static bool SettingsGetBoolean(ref bool __result, string key, bool default_value)
        {
            //Plugin.Log.LogWarning("N/I: SettingsGetBoolean("+key+")");
            __result = default_value;
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingGetClosingTime")]
        [HarmonyPrefix]
        static bool SettingGetClosingTime(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: SettingGetClosingTime");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsGetInteger")]
        [HarmonyPrefix]
        static bool SettingsGetInteger(ref int __result, string key, int default_value)
        {
            Plugin.Log.LogWarning("N/I: SettingsGetInteger");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsGetProfile")]
        [HarmonyPrefix]
        static bool SettingsGetProfile(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: SettingsGetProfile");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsGetString")]
        [HarmonyPrefix]
        static bool SettingsGetString(ref IntPtr __result, string key, string default_value)
        {
            Plugin.Log.LogWarning("N/I: SettingsGetString");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsGetTimeSpan")]
        [HarmonyPrefix]
        static bool SettingsGetTimeSpan(ref IntPtr __result, string key, IntPtr default_value)
        {
            Plugin.Log.LogWarning("N/I: SettingsGetTimeSpan");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsGetTimeToClose")]
        [HarmonyPrefix]
        static bool SettingsGetTimeToClose(ref IntPtr __result)
        {
            Plugin.Log.LogWarning("N/I: SettingsGetTimeToClose");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsSetBoolean")]
        [HarmonyPrefix]
        static bool SettingsSetBoolean(string key, bool value)
        {
            Plugin.Log.LogWarning("N/I: SettingsSetBoolean");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsSetClosingTime")]
        [HarmonyPrefix]
        static bool SettingsSetClosingTime(IntPtr value)
        {
            Plugin.Log.LogWarning("N/I: SettingsSetClosingTime");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsSetInteger")]
        [HarmonyPrefix]
        static bool SettingsSetInteger(string key, int value)
        {
            Plugin.Log.LogWarning("N/I: SettingsSetInteger");
            return false;
        }

        [HarmonyPatch(typeof(AssemblyNotice::LINK.bnAMPF), "SettingsSetString")]
        [HarmonyPrefix]
        static bool SettingsSetString(string key, string value)
        {
            Plugin.Log.LogWarning("N/I: SettingsSetString");
            return false;
        }



    }
}
