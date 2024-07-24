using HarmonyLib;
using Haruka.Arcade.SEGA835Lib.Devices.Card._837_15396;
using LINK;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace SwordArtOffline.Patches.Game {

    extern alias AssemblyNotice;

    public static class CardPatchesGame {

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "Initialize")]
        static bool Initialize(ref bool __result, string mac_addr) {
            Plugin.Log.LogInfo("BNGRW Initialize");
            __result = true;
            if (Plugin.Aime != null) {
                __result = Plugin.AimeConnected;
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "FinalizeFunc")]
        static bool FinalizeFunc() {
            Plugin.Log.LogInfo("BNGRW Finalize");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "Update")]
        static bool Update() {
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "CommonClearResourceConfig")]
        static bool CommonClearResourceConfig(ref bool __result, IntPtr config) {
            Plugin.Log.LogDebug("BNGRW.CommonClearResourceConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "CommonClearConfig")]
        static bool CommonClearConfig(ref bool __result, IntPtr config) {
            Plugin.Log.LogDebug("BNGRW.CommonClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "ClearConfig")]
        static bool ClearConfig(ref bool __result, IntPtr config) {
            Plugin.Log.LogDebug("BNGRW.ClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "ClearFatalError")]
        static bool ClearFatalError(ref bool __result, IntPtr error) {
            Plugin.Log.LogDebug("BNGRW.ClearFatalError");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "IsReaderTaskEnd")]
        static bool IsReaderTaskEnd(ref bool __result, int index = 0) {
            //Plugin.Log.LogDebug("BNGRW.IsReaderTaskEnd");
            __result = Plugin.ConfigButtonScanCard.Value.IsPressed();
            if (Plugin.Aime != null) {
                __result = Plugin.Aime.HasDetectedCard() || !Plugin.Aime.IsPolling();
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "getErrorCount")]
        static bool getErrorCount(ref int __result) {
            Plugin.Log.LogDebug("BNGRW.getErrorCount");
            __result = 0;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "SequenceSwitchMode")]
        static bool SequenceSwitchMode(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.SequenceSwitchMode");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "SequenceGetPlayerTotal")]
        static bool SequenceGetPlayerTotal(ref int __result) {
            Plugin.Log.LogDebug("BNGRW.SequenceGetPlayerTotal");
            __result = 1;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "SequenceStartPlay")]
        static bool SequenceStartPlay(ref bool __result, int player) {
            Plugin.Log.LogDebug("BNGRW.SequenceStartPlay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "SequenceContinuePlay")]
        static bool SequenceContinuePlay(ref bool __result, int player) {
            Plugin.Log.LogDebug("BNGRW.SequenceContinuePlay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "SequenceEndPlay")]
        static bool SequenceEndPlay(ref bool __result, int player) {
            Plugin.Log.LogDebug("BNGRW.SequenceEndPlay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "SequenceIsPlaying")]
        static bool SequenceIsPlaying(ref bool __result, int player) {
            Plugin.Log.LogDebug("BNGRW.SequenceIsPlaying");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeClearConfig")]
        static bool AimeClearConfig(ref bool __result, IntPtr config) {
            Plugin.Log.LogDebug("BNGRW.AimeClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeClearCardReader")]
        static bool AimeClearCardReader(ref bool __result, IntPtr config) {
            Plugin.Log.LogDebug("BNGRW.AimeClearCardReader");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeToStringAccessCode")]
        static bool AimeToStringAccessCode(ref bool __result, IntPtr pSrc, IntPtr wszDst, int dstSize) {
            Plugin.Log.LogDebug("BNGRW.AimeToStringAccessCode");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeIsEnable")]
        static bool AimeIsEnable(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.AimeIsEnable");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeGetCardReaderTotal")]
        static bool AimeGetCardReaderTotal(ref int __result) {
            Plugin.Log.LogDebug("BNGRW.AimeGetCardReaderTotal");
            __result = 1;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeDetectCard")]
        static bool AimeDetectCard(ref bool __result, int index) {
            Plugin.Log.LogDebug("BNGRW.AimeDetectCard");
            __result = true;
            if (Plugin.Aime != null) {
                EMoneyUILinkIntegration.SetCardReaderBlocked(true);
                Plugin.Aime.LEDSetColor(255, 255, 255);
                Plugin.Aime.RadioOn(RadioOnType.Both);
                __result = Plugin.Aime.StartPolling() == Haruka.Arcade.SEGA835Lib.Devices.DeviceStatus.OK;
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeAcceptTask")]
        static bool AimeAcceptTask(ref bool __result, int index) {
            Plugin.Log.LogDebug("BNGRW.AimeAcceptTask");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeCancelTask")]
        static bool AimeCancelTask(ref bool __result, int index) {
            Plugin.Log.LogDebug("BNGRW.AimeCancelTask");
            __result = true;
            if (Plugin.Aime != null) {
                EMoneyUILinkIntegration.SetCardReaderBlocked(false);
                __result = Plugin.Aime.StopPolling() == Haruka.Arcade.SEGA835Lib.Devices.DeviceStatus.OK;
                Plugin.Aime.LEDSetColor(0, 0, 0);
                Plugin.Aime.ClearCard();
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeSetLed")]
        static bool AimeSetLed(ref bool __result, int index, byte red, byte green, byte blue) {
            Plugin.Log.LogDebug("BNGRW.AimeSetLed");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeControlLed")]
        static bool AimeControlLed(ref bool __result, int index, bnReader.AimeLedLightPattern pattern) {
            Plugin.Log.LogDebug("BNGRW.AimeControlLed");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeGetCardReaderProperty")]
        static bool AimeGetCardReaderProperty(ref bool __result, int index, IntPtr prop) {
            //Plugin.Log.LogDebug("BNGRW.AimeGetCardReaderProperty");
            var property = new bnReader.AimeCardReaderProperty() {
                bConnect = (byte)(Plugin.Aime == null || Plugin.AimeConnected ? 1 : 0),
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

        public static byte[] ConvertHexStringToByteArray(string hexString) {
            if (hexString.Length % 2 != 0) {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++) {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeGetNfcChipProperty")]
        static bool AimeGetNfcChipProperty(ref bool __result, int index, IntPtr prop) {
            Plugin.Log.LogDebug("BNGRW.AimeGetNfcChipProperty");
            byte[] bytes;
            if (Plugin.Aime != null) {
                if (!Plugin.Aime.HasDetectedCard()) {
                    __result = false;
                    return false;
                }
                bytes = Plugin.Aime.GetCardUID();
            } else {
                bytes = ConvertHexStringToByteArray(Plugin.CardID.Value);
            }
            byte[] chipid = new byte[16];
            Array.Copy(bytes, chipid, bytes.Length);
            var property = new bnReader.AimeNfcChipProperty() {
                bMobile = 0,
                chipId = chipid,
            };
            Marshal.StructureToPtr(property, prop, false);
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "AimeGetAccessCode")]
        static bool AimeGetAccessCode(ref bool __result, int index, IntPtr code) {
            Plugin.Log.LogDebug("BNGRW.AimeGetAccessCode");
            if (Plugin.Aime != null) {
                __result = Plugin.Aime.HasDetectedCard();
                if (__result) {
                    var accesscode = new bnReader.AimeAccessCode {
                        bValid = 1,
                        data = Plugin.Aime.GetCardUID()
                    };
                    Marshal.StructureToPtr(accesscode, code, false);
                }
            } else {
                byte[] accesscodeBytes = new byte[10];
                for (int i = 0; i < Plugin.CardID.Value.Length && i < accesscodeBytes.Length * 2; i += 2) {
                    accesscodeBytes[i / 2] = Byte.Parse(Plugin.CardID.Value.Substring(i, 2), NumberStyles.HexNumber);
                }
                var accesscode = new bnReader.AimeAccessCode {
                    bValid = 1,
                    data = accesscodeBytes
                };
                Marshal.StructureToPtr(accesscode, code, false);
                __result = true;
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyClearConfig")]
        static bool EmoneyClearConfig(ref bool __result, IntPtr config) {
            Plugin.Log.LogDebug("BNGRW.EmoneyClearConfig");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyClearDealResult")]
        static bool EmoneyClearDealResult(ref bool __result, IntPtr result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyClearDealResult");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyClearErrorInfo")]
        static bool EmoneyClearErrorInfo(ref bool __result, IntPtr info) {
            Plugin.Log.LogDebug("BNGRW.EmoneyClearErrorInfo");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyIsEnable")]
        static bool EmoneyIsEnable(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsEnable");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyQueryBalance")]
        static bool EmoneyQueryBalance(ref bool __result, bnReader.EmoneyBrand brand) {
            Plugin.Log.LogDebug("BNGRW.EmoneyQueryBalance");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyPayAmount")]
        static bool EmoneyPayAmount(ref bool __result, uint value, bnReader.EmoneyBrand brand) {
            Plugin.Log.LogDebug("BNGRW.EmoneyPayAmount");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyCompletePay")]
        static bool EmoneyCompletePay(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyCompletePay");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyCancel")]
        static bool EmoneyCancel(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyCancel");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyActivate")]
        static bool EmoneyActivate(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyActivate");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyDeactivate")]
        static bool EmoneyDeactivate(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyDeactivate");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyIsAvailable")]
        static bool EmoneyIsAvailable(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsAvailable");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyIsActivated")]
        static bool EmoneyIsActivated(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsActivated");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetTerminalId")]
        static bool EmoneyGetTerminalId(ref IntPtr __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetTerminalId");
            __result = Marshal.StringToHGlobalAnsi("EMULATOR");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneygetTerminalSerial")]
        static bool EmoneygetTerminalSerial(ref IntPtr __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneygetTerminalSerial");
            __result = Marshal.StringToHGlobalAnsi("EMULATOR");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyIsAliveServer")]
        static bool EmoneyIsAliveServer(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsAliveServer");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyIsReporting")]
        static bool EmoneyIsReporting(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsReporting");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetBrandInfo")]
        static bool EmoneyGetBrandInfo(ref bool __result, bnReader.EmoneyBrand brand, IntPtr info) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetBrandInfo");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetAvailableBrandTotal")]
        static bool EmoneyGetAvailableBrandTotal(ref int __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetAvailableBrandTotal");
            __result = 1;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetAvailableBrandInfo")]
        static bool EmoneyGetAvailableBrandInfo(ref bool __result, int index, IntPtr info) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetAvailableBrandInfo");
            var brand = new bnReader.EmoneyBrandInfo() {
                code = bnReader.EmoneyBrand.EMONEY_BRAND_TRANSPORT,
                wszName = "TRANSPORT",
                wszIconFilePath = "0005-01-00.png"
            };
            Marshal.StructureToPtr(brand, info, false);
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetDealLog")]
        static bool EmoneyGetDealLog(ref bool __result, IntPtr log) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetDealLog");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetReportLog")]
        static bool EmoneyGetReportLog(ref bool __result, IntPtr log) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetReportLog");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetPlaySound")]
        static bool EmoneyGetPlaySound(ref IntPtr __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetPlaySound");
            __result = new IntPtr(0);
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyIsProcedureEnd")]
        static bool EmoneyIsProcedureEnd(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyIsProcedureEnd");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "EmoneyGetErrorCode")]
        static bool EmoneyGetErrorCode(ref bnReader.EmoneyDealResult __result) {
            Plugin.Log.LogDebug("BNGRW.EmoneyGetErrorCode");
            __result = new bnReader.EmoneyDealResult() {
            };
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "VFDStartTest")]
        static bool VFDStartTest(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.VFDStartTest");
            __result = true;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "VFDCancelTest")]
        static bool VFDCancelTest(ref bool __result) {
            Plugin.Log.LogDebug("BNGRW.VFDCancelTest");
            __result = true;
            return false;
        }

    }
}
