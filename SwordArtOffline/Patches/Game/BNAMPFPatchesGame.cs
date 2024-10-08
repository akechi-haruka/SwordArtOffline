﻿using HarmonyLib;
using Haruka.Arcade.SEGA835Lib.Devices;
using Haruka.Arcade.SEGA835Lib.Devices.Card._837_15396;
using LINK;
using LINK.TestMode;
using Mono.Cecil;
using SwordArtOffline.Patches.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
using Utage;
using static AssetBundles.Manager.Settings;
using static LINK.bnAMPF;
using Color = UnityEngine.Color;

namespace SwordArtOffline.Patches.Game {

    public static class BNAMPFPatchesGame {

        private static bool reading;

        [HarmonyPatch(typeof(bnAMPF), "EMoneyStartActivate")]
        [HarmonyPrefix]
        static bool EMoneyStartActivate(ref bool __result) {
            Plugin.Log.LogWarning("N/I: EMoneyStartActivate");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyStartDeactivate")]
        [HarmonyPrefix]
        static bool EMoneyStartDeactivate(ref bool __result) {
            Plugin.Log.LogWarning("N/I: EMoneyStartDeactivate");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyStartPay")]
        [HarmonyPrefix]
        static bool EMoneyStartPay(ref bool __result, EMoneyBrand brand, int value) {
            Plugin.Log.LogWarning("N/I: EMoneyStartPay");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyStartPayForCredit")]
        [HarmonyPrefix]
        static bool EMoneyStartPayForCredit(ref bool __result, EMoneyBrand brand, int amount, int credits) {
            Plugin.Log.LogWarning("N/I: EMoneyStartPayForCredit");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyStartQueryBalance")]
        [HarmonyPrefix]
        static bool EMoneyStartQueryBalance(ref bool __result, EMoneyBrand brand) {
            Plugin.Log.LogWarning("N/I: EMoneyStartQueryBalance");
            return false;
        }

        private static LeafPrinterStatus printerStatus = LeafPrinterStatus.OK;
        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterGetStatus")]
        [HarmonyPrefix]
        static bool LeafPrinterGetStatus(ref LeafPrinterStatus __result) {
            __result = printerStatus;
            return false;
        }

        private static IntPtr printerinfo;
        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterGetPrinterInfo")]
        [HarmonyPrefix]
        static bool LeafPrinterGetPrinterInfo(ref IntPtr __result) {
            if (printerinfo == IntPtr.Zero) {
                printerinfo = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(LeafPrinterPrinterInfo)));
                var property = new LeafPrinterPrinterInfo() {
                    DeviceName = Marshal.StringToHGlobalUni("EMULATOR"),
                    FirmwareVersion1 = Marshal.StringToHGlobalUni("EMULATOR"),
                    FirmwareVersion2 = Marshal.StringToHGlobalUni("EMULATOR"),
                    FirmwareVersion3 = Marshal.StringToHGlobalUni("EMULATOR"),
                    SerialNumber = Marshal.StringToHGlobalUni("EMULATOR"),
                    TotalPrintCount = 0,
                    CurrentPrintCount = 0,
                    CutterCount = 0,
                    FilledCount = 0,
                    HeadCount = 0,
                    HoloHeadCount = 0
                };
                Marshal.StructureToPtr(property, printerinfo, false);
            }
            __result = printerinfo;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterGetMediaStatus")]
        [HarmonyPrefix]
        static bool LeafPrinterGetMediaStatus(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: LeafPrinterGetMediaStatus");
            return false;
        }

        private static LeafPrinterPrintResult printResult = LeafPrinterPrintResult.OK;

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterGetPrintResult")]
        [HarmonyPrefix]
        static bool LeafPrinterGetPrintResult(ref LeafPrinterPrintResult __result) {
            //Plugin.Log.LogWarning("N/I: LeafPrinterGetPrintResult");
            __result = printResult;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterSetFilter")]
        [HarmonyPrefix]
        static bool LeafPrinterSetFilter(LeafPrinterFilter filter) {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetFilter");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterSetICCFile")]
        [HarmonyPrefix]
        static bool LeafPrinterSetICCFile(string inICC, string outICC) {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetICCFile");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterSetGammaTable")]
        [HarmonyPrefix]
        static bool LeafPrinterSetGammaTable(float inR, float inG, float inB, float outR, float outG, float outB) {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetGammaTable");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterSetGammaTableFile")]
        [HarmonyPrefix]
        static bool LeafPrinterSetGammaTableFile(string inGammaTable, string outGammaTable) {
            Plugin.Log.LogWarning("N/I: LeafPrinterSetGammaTableFile");
            return false;
        }

        private static Dictionary<LeafPrinterImageType, Texture2D> images = new Dictionary<LeafPrinterImageType, Texture2D>();

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterSetImage")]
        [HarmonyPrefix]
        static bool LeafPrinterSetImage(LeafPrinterSurface surface, LeafPrinterImageType type, int width, int height, IntPtr pixels, int pixels_size) {
            int bytesPerPixel = type == LeafPrinterImageType.Holo ? 1 : 3;
            Plugin.Log.LogDebug("alloc " + (width * height * bytesPerPixel) + " pixels");
            byte[] raw = new byte[width * height * bytesPerPixel];
            Plugin.Log.LogDebug("copy from ptr");
            Marshal.Copy(pixels, raw, 0, raw.Length);
            Plugin.Log.LogDebug("alloc texture at "+width+"x"+height);
            Texture2D tex = new Texture2D(width, height);
            Plugin.Log.LogDebug("alloc "+(width*height)+" colors");
            Color[] col = new Color[width * height];

            Plugin.Log.LogDebug("filling");
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    int num4 = (i * width + j) * bytesPerPixel;
                    byte[] px = new byte[3];
                    for (int k = 0; k < bytesPerPixel; k++) {
                        px[k] = raw[num4 + k];
                    }
                    col[i * width + j] = new Color(px[0] / 255F, px[1] / 255F, px[2] / 255F);
                }
            }

            Plugin.Log.LogDebug("set pixels to unity");
            tex.SetPixels(col);
            Plugin.Log.LogDebug("apply");
            tex.Apply();
            images[type] = tex;

            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterClearImage")]
        [HarmonyPrefix]
        static bool LeafPrinterClearImage(LeafPrinterSurface surface, LeafPrinterImageType type) {

            images[type] = null;

            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterStartPrint")]
        [HarmonyPrefix]
        static bool LeafPrinterStartPrint() {

            printResult = LeafPrinterPrintResult.Busy;
            printerStatus = LeafPrinterStatus.Busy;

            new Thread(PrintExecutor).Start();
            
            return false;
        }

        private static void PrintExecutor() {
            string file1 = PrintFile(images[LeafPrinterImageType.ColorRGB]);
            string file2 = null;
            if (Plugin.ConfigPrinterHolo.Value) {
                file2 = PrintFile(images[LeafPrinterImageType.Holo], true);
            }

            if (Plugin.ConfigPrinterUseHandler.Value && Plugin.ConfigPrinterHandler.Value != "") {
                try {
                    string exe = Plugin.ConfigPrinterHandler.Value;
                    string cwd = new FileInfo(exe).Directory.FullName;
                    string args = Plugin.ConfigPrinterHandlerArguments.Value.Replace("%IMAGE_PATH%", file1).Replace("%HOLO_PATH%", file2).Replace("%HOLO_ARG%", !String.IsNullOrEmpty(file2) ? "--holo" : "");
                    Plugin.Log.LogInfo("Print invoking: " + exe + ", cwd: " + cwd + ", args: " + args);
                    ProcessStartInfo psi = new ProcessStartInfo(exe, args) {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        WorkingDirectory = cwd
                    };
                    Process zebra = Process.Start(psi);
                    if (!zebra.WaitForExit(300_000)) {
                        Plugin.Log.LogError("WaitForExit failed: " + zebra.ExitCode);
                        printResult = LeafPrinterPrintResult.Error;
                        return;
                    }
                    Plugin.Log.LogInfo("Print invocation exit code: " + zebra.ExitCode);
                    if (zebra.ExitCode == 0) {
                        printResult = LeafPrinterPrintResult.OK;
                        return;
                    } else {
                        printResult = LeafPrinterPrintResult.Error;
                        return;
                    }
                } catch (Exception ex) {
                    Plugin.Log.LogError("Print invoke failed: " + ex.Message);
                } finally {
                    printerStatus = LeafPrinterStatus.OK;
                }
            }

            printerStatus = LeafPrinterStatus.OK;
            printResult = LeafPrinterPrintResult.OK;
        }

        private static String PrintFile(Texture2D out_tex, bool is_holo = false) {
            if (out_tex == null) {
                return null;
            }
            string dir = Plugin.ConfigPrinterDirectory.Value;
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }
            string file_pre = dir + "\\" + "card_";
            string file_full;
            for (int i = 0; true; i++) {
                file_full = file_pre + String.Format("{0:0000}", i) + (is_holo ? "_holo" : "") + ".png";
                if (!File.Exists(file_full)) {
                    break;
                }
            }
            Plugin.Log.LogDebug("pix=" + out_tex.GetPixels32().Length);
            Plugin.Log.LogDebug("size=" + out_tex.width + "x" + out_tex.height + "@" + out_tex.format);
            byte[] pngbytes = out_tex.EncodeToPNG();
            File.WriteAllBytes(file_full, pngbytes);
            return new FileInfo(file_full).FullName;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterAbortPrint")]
        [HarmonyPrefix]
        static bool LeafPrinterAbortPrint() {
            Plugin.Log.LogWarning("N/I: LeafPrinterAbortPrint");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LeafPrinterStartPrintUsage")]
        [HarmonyPrefix]
        static bool LeafPrinterStartPrintUsage(LeafPrinterPrintUsage usage) {
            Plugin.Log.LogWarning("N/I: LeafPrinterStartPrintUsage");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyAbort")]
        [HarmonyPrefix]
        static bool EMoneyAbort(ref bool __result) {
            Plugin.Log.LogWarning("N/I: EMoneyAbort");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyCompletePay")]
        [HarmonyPrefix]
        static bool EMoneyCompletePay(ref bool __result) {
            Plugin.Log.LogWarning("N/I: EMoneyCompletePay");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetAvailableBrand")]
        [HarmonyPrefix]
        static bool EMoneyGetAvailableBrand(ref IntPtr __result, int index) {
            Plugin.Log.LogWarning("N/I: EMoneyGetAvailableBrand");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetAvailableBrandCount")]
        [HarmonyPrefix]
        static bool EMoneyGetAvailableBrandCount(ref int __result) {
            Plugin.Log.LogWarning("N/I: EMoneyGetAvailableBrandCount");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetDealLog")]
        [HarmonyPrefix]
        static bool EMoneyGetDealLog(ref IntPtr __result, int index) {
            Plugin.Log.LogWarning("N/I: EMoneyGetDealLog");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetDealLogCount")]
        [HarmonyPrefix]
        static bool EMoneyGetDealLogCount(ref int __result) {
            Plugin.Log.LogWarning("N/I: EMoneyGetDealLogCount");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetEnvironment")]
        [HarmonyPrefix]
        static bool EMoneyGetEnvironment(ref EMoneyEnvironment __result) {
            Plugin.Log.LogWarning("N/I: EMoneyGetEnvironment");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetReportLog")]
        [HarmonyPrefix]
        static bool EMoneyGetReportLog(ref IntPtr __result, int index) {
            Plugin.Log.LogWarning("N/I: EMoneyGetReportLog");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetReportLogCount")]
        [HarmonyPrefix]
        static bool EMoneyGetReportLogCount(ref int __result) {
            Plugin.Log.LogWarning("N/I: EMoneyGetReportLogCount");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetRequestSound")]
        [HarmonyPrefix]
        static bool EMoneyGetRequestSound(ref IntPtr __result, int index) {
            Plugin.Log.LogWarning("N/I: EMoneyGetRequestSound");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetRequestSoundCount")]
        [HarmonyPrefix]
        static bool EMoneyGetRequestSoundCount(ref int __result) {
            __result = 0;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetResult")]
        [HarmonyPrefix]
        static bool EMoneyGetResult(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: EMoneyGetResult");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetStatus")]
        [HarmonyPrefix]
        static bool EMoneyGetStatus(ref EMoneyStatus __result) {
            __result = EMoneyStatus.OK;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetTerminalId")]
        [HarmonyPrefix]
        static bool EMoneyGetTerminalId(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: EMoneyGetTerminalId");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EMoneyGetTerminalSerial")]
        [HarmonyPrefix]
        static bool EMoneyGetTerminalSerial(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: EMoneyGetTerminalSerial");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkGetInterfaceCount")]
        [HarmonyPrefix]
        static bool NetworkGetInterfaceCount(ref int __result) {
            Plugin.Log.LogWarning("N/I: NetworkGetInterfaceCount");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkGetInterfaceStatus")]
        [HarmonyPrefix]
        static bool NetworkGetInterfaceStatus(ref IntPtr __result, int index) {
            Plugin.Log.LogWarning("N/I: NetworkGetInterfaceStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkGetLocalConnectionStatus")]
        [HarmonyPrefix]
        static bool NetworkGetLocalConnectionStatus(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: NetworkGetLocalConnectionStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkGetLocalNetworkStatus")]
        [HarmonyPrefix]
        static bool NetworkGetLocalNetworkStatus(ref NetworkStatus __result) {
            Plugin.Log.LogWarning("N/I: NetworkGetLocalNetworkStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "UpdaterGetStatus")]
        [HarmonyPrefix]
        static bool UpdaterGetStatus(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: UpdaterGetStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "USBDongleGetSerialNumber")]
        [HarmonyPrefix]
        static bool USBDongleGetSerialNumber(ref IntPtr __result) {
            __result = Plugin.GetKeychipU();
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditAdd")]
        [HarmonyPrefix]
        static bool CreditAdd(ref int __result, CreditType type, int count) {
            Plugin.Log.LogWarning("N/I: CreditAdd");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditBeginUse")]
        [HarmonyPrefix]
        static bool CreditBeginUse(ref bool __result, CreditType type, int count) {
            Plugin.Log.LogWarning("N/I: CreditBeginUse");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditClear")]
        [HarmonyPrefix]
        static bool CreditClear() {
            Plugin.Log.LogWarning("N/I: CreditClear");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditEndUse")]
        [HarmonyPrefix]
        static bool CreditEndUse(CreditType type, int count) {
            Plugin.Log.LogWarning("N/I: CreditEndUse");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditGet")]
        [HarmonyPrefix]
        static bool CreditGet(ref int __result, CreditType type) {
            if (type == CreditType.Coin) {
                __result = Plugin.Coins;
            } else {
                __result = Plugin.Service;
            }
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditGetCleared")]
        [HarmonyPrefix]
        static bool CreditGetCleared(ref int __result, CreditType type) {
            Plugin.Log.LogWarning("N/I: CreditGetCleared");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditGetClearedLastRecord")]
        [HarmonyPrefix]
        static bool CreditGetClearedLastRecord(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: CreditGetClearedLastRecord");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditGetInc")]
        [HarmonyPrefix]
        static bool CreditGetInc(ref int __result, CreditType type) {
            Plugin.Log.LogWarning("N/I: CreditGetInc");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditGetRecentlyUsed")]
        [HarmonyPrefix]
        static bool CreditGetRecentlyUsed(ref int __result, CreditType type) {
            Plugin.Log.LogWarning("N/I: CreditGetRecentlyUsed");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditSetAcceptability")]
        [HarmonyPrefix]
        static bool CreditSetAcceptability(CreditType type, bool value) {
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "CreditUse")]
        [HarmonyPrefix]
        static bool CreditUse(ref bool __result, CreditType type, int count) {
            if (type == CreditType.Coin) {
                Plugin.Coins -= count;
            } else {
                Plugin.Service -= count;
            }
            Plugin.SaveAMPFCoin();
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BasicIOGetInput")]
        [HarmonyPrefix]
        static bool BasicIOGetInput(ref int __result, BasicIOInputPort port) {
            if (port == BasicIOInputPort.SwitchTest) {
                __result = Plugin.ConfigButtonTest.Value.IsPressed() ? 1 : 0;
            } else if (port == BasicIOInputPort.SwitchService) {
                __result = Plugin.ConfigButtonService.Value.IsPressed() ? 1 : 0;
            } else if (port == BasicIOInputPort.SwitchTestDown) {
                __result = Plugin.ConfigButtonTestDown.Value.IsPressed() ? 1 : 0;
            } else if (port == BasicIOInputPort.SwitchTestUp) {
                __result = Plugin.ConfigButtonTestUp.Value.IsPressed() ? 1 : 0;
            } else if (port == BasicIOInputPort.SwitchEnter) {
                __result = Plugin.ConfigButtonTestEnter.Value.IsPressed() ? 1 : 0;
            }
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BasicIOGetUpdateCounter")]
        [HarmonyPrefix]
        static bool BasicIOGetUpdateCounter(ref long __result) {
            Plugin.Log.LogWarning("N/I: BasicIOGetUpdateCounter");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BasicIOSetOutput")]
        [HarmonyPrefix]
        static bool BasicIOSetOutput(BasicIOOutputPort port, int value) {
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassGetReaderInfo")]
        [HarmonyPrefix]
        static bool BanapassGetReaderInfo(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: BanapassGetReaderInfo");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassStartTest")]
        [HarmonyPrefix]
        static bool BanapassStartTest(BanapassTestTarget dev) {
            Plugin.Log.LogWarning("N/I: BanapassStartTest");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassAbortTest")]
        [HarmonyPrefix]
        static bool BanapassAbortTest(BanapassTestTarget dev) {
            Plugin.Log.LogWarning("N/I: BanapassAbortTest");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassGetTestResult")]
        [HarmonyPrefix]
        static bool BanapassGetTestResult(BanapassTestTarget dev) {
            Plugin.Log.LogWarning("N/I: BanapassGetTestResult");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassStartRead")]
        [HarmonyPrefix]
        static bool BanapassStartRead() {
            Plugin.Log.LogDebug("AMPF: BanapassStartRead");
            reading = true;
            if (Plugin.Aime != null) {
                Plugin.Aime.ClearCard();
                Plugin.Aime.LEDSetColor(255, 255, 255);
                Plugin.Aime.RadioOn(RadioOnType.Both);
                Plugin.Aime.StartPolling();
            }
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassAbortRead")]
        [HarmonyPrefix]
        static bool BanapassAbortRead() {
            Plugin.Log.LogDebug("AMPF: BanapassAbortRead");
            reading = false;
            if (Plugin.Aime != null) {
                Plugin.Aime.LEDSetColor(0, 0, 0);
                Plugin.Aime.StopPolling();
                Plugin.Aime.ClearCard();
            }
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassGetReadResult")]
        [HarmonyPrefix]
        static bool BanapassGetReadResult(ref BanapassReadStatus __result) {
            if (Plugin.Aime != null) {
                __result = reading && Plugin.Aime.HasDetectedCard() ? BanapassReadStatus.OK : BanapassReadStatus.Busy;
                if (!Plugin.Aime.IsPolling() && !Plugin.Aime.HasDetectedCard()) {
                    __result = (DeviceStatus)Plugin.Aime.GetLastError() == DeviceStatus.ERR_INCOMPATIBLE ? BanapassReadStatus.CardError : BanapassReadStatus.Error;
                }
            } else {
                __result = reading && Plugin.ConfigButtonScanCard.Value.IsPressed() ? BanapassReadStatus.OK : BanapassReadStatus.Busy;
            }
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BanapassGetReadCard")]
        [HarmonyPrefix]
        static bool BanapassGetReadCard(ref IntPtr __result) {
            __result = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(BanapassCardInfo)));
            BanapassCardInfo property;
            if (Plugin.Aime != null) {
                property = new BanapassCardInfo() {
                    Type = BanapassCardType.Banapass,
                    AccessCode = Marshal.StringToHGlobalUni(Plugin.Aime.GetCardUIDAsString()),
                    ChipId = Marshal.StringToHGlobalUni(Plugin.Aime.GetCardUIDAsString())
                };
                Plugin.Aime.ClearCard();
            } else {
                property = new BanapassCardInfo() {
                    Type = BanapassCardType.Banapass,
                    AccessCode = Marshal.StringToHGlobalUni(Plugin.CardID.Value),
                    ChipId = Marshal.StringToHGlobalUni(Plugin.CardID.Value)
                };
            }
            Marshal.StructureToPtr(property, __result, false);
            return false;
        }

        /*[HarmonyPatch(typeof(LINK.bnAMPF), "SettingsTryGetBoolean")]
        [HarmonyPrefix]
        static bool SettingsTryGetBoolean(ref Boolean __result, String key, out Boolean value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetBoolean");
            value = false;
            __result = false;
            return false;
        }

        [HarmonyPatch(typeof(LINK.bnAMPF), "SettingsTryGetInteger")]
        [HarmonyPrefix]
        static bool SettingsTryGetInteger(ref Boolean __result, String key, out Int32 value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetInteger");
            value = 0;
            return false;
        }

        [HarmonyPatch(typeof(LINK.bnAMPF), "SettingsTryGetString")]
        [HarmonyPrefix]
        static bool SettingsTryGetString(ref Boolean __result, String key, out String value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetString");
            value = "";
            return false;
        }

        [HarmonyPatch(typeof(LINK.bnAMPF), "SettingsTryGetTimeSpan")]
        [HarmonyPrefix]
        static bool SettingsTryGetTimeSpan(ref Boolean __result, String key, out LINK.bnAMPF.TimeSpan value) {
            Plugin.Log.LogWarning("N/I: SettingsTryGetTimeSpan");
            value = new LINK.bnAMPF.TimeSpan();
            return false;
        }*/

        [HarmonyPatch(typeof(bnAMPF), "LoggingAbortGame")]
        [HarmonyPrefix]
        static bool LoggingAbortGame(string mode) {
            Plugin.Log.LogWarning("N/I: LoggingAbortGame");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingAddCommonLogInteger")]
        [HarmonyPrefix]
        static bool LoggingAddCommonLogInteger(LoggingCommonLogField key, int value) {
            Plugin.Log.LogWarning("N/I: LoggingAddCommonLogInteger");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingAddCommonLogTimeSpan")]
        [HarmonyPrefix]
        static bool LoggingAddCommonLogTimeSpan(LoggingCommonLogField key, IntPtr value) {
            Plugin.Log.LogWarning("N/I: LoggingAddCommonLogTimeSpan");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingAddPlayLogInteger")]
        [HarmonyPrefix]
        static bool LoggingAddPlayLogInteger(LoggingPlayLogField key, int value) {
            Plugin.Log.LogWarning("N/I: LoggingAddPlayLogInteger");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingAddPlayLogTimeSpan")]
        [HarmonyPrefix]
        static bool LoggingAddPlayLogTimeSpan(LoggingPlayLogField key, IntPtr value) {
            Plugin.Log.LogWarning("N/I: LoggingAddPlayLogTimeSpan");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingBeginGame")]
        [HarmonyPrefix]
        static bool LoggingBeginGame() {
            Plugin.Log.LogWarning("N/I: LoggingBeginGame");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingClearBackupedGameStatus")]
        [HarmonyPrefix]
        static bool LoggingClearBackupedGameStatus() {
            Plugin.Log.LogWarning("N/I: LoggingClearBackupedGameStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingEndGame")]
        [HarmonyPrefix]
        static bool LoggingEndGame(string mode) {
            Plugin.Log.LogWarning("N/I: LoggingEndGame");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingGetBackupedGameStatus")]
        [HarmonyPrefix]
        static bool LoggingGetBackupedGameStatus(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: LoggingGetBackupedGameStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingGetCommonLogStatistics")]
        [HarmonyPrefix]
        static bool LoggingGetCommonLogStatistics(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: LoggingGetCommonLogStatistics");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingGetErrorLogCount")]
        [HarmonyPrefix]
        static bool LoggingGetErrorLogCount(ref int __result) {
            Plugin.Log.LogWarning("N/I: LoggingGetErrorLogCount");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingGetErrorLogEntry")]
        [HarmonyPrefix]
        static bool LoggingGetErrorLogEntry(ref IntPtr __result, int index) {
            Plugin.Log.LogWarning("N/I: LoggingGetErrorLogEntry");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingGetPlayLogStatistics")]
        [HarmonyPrefix]
        static bool LoggingGetPlayLogStatistics(ref IntPtr __result, string mode, IntPtr timeFrom, IntPtr timeTo) {
            Plugin.Log.LogWarning("N/I: LoggingGetPlayLogStatistics");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingRecordError")]
        [HarmonyPrefix]
        static bool LoggingRecordError(Error error, LoggingErrorVisibility visibility) {
            Plugin.Log.LogWarning("N/I: LoggingRecordError");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingRestoreGame")]
        [HarmonyPrefix]
        static bool LoggingRestoreGame(IntPtr backup) {
            Plugin.Log.LogWarning("N/I: LoggingRestoreGame");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingSetGameStatus")]
        [HarmonyPrefix]
        static bool LoggingSetGameStatus(IntPtr status) {
            Plugin.Log.LogWarning("N/I: LoggingSetGameStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingSetPlayLogInteger")]
        [HarmonyPrefix]
        static bool LoggingSetPlayLogInteger(LoggingPlayLogField key, int value) {
            Plugin.Log.LogWarning("N/I: LoggingSetPlayLogInteger");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingSetPlayLogString")]
        [HarmonyPrefix]
        static bool LoggingSetPlayLogString(LoggingPlayLogField key, string value) {
            Plugin.Log.LogWarning("N/I: LoggingSetPlayLogString");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "LoggingSetPlayLogTimeSpan")]
        [HarmonyPrefix]
        static bool LoggingSetPlayLogTimeSpan(LoggingPlayLogField key, IntPtr value) {
            Plugin.Log.LogWarning("N/I: LoggingSetPlayLogTimeSpan");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkCheckLocalConnection")]
        [HarmonyPrefix]
        static bool NetworkCheckLocalConnection(ref bool __result) {
            __result = true;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkCheckNetworkConnection")]
        [HarmonyPrefix]
        static bool NetworkCheckNetworkConnection(ref bool __result) {
            __result = true;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkGetAllNetStatus")]
        [HarmonyPrefix]
        static bool NetworkGetAllNetStatus(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: NetworkGetAllNetStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkGetCheckConnectionResult")]
        [HarmonyPrefix]
        static bool NetworkGetCheckConnectionResult(ref NetworkConnectionStatus __result) {
            Plugin.Log.LogWarning("N/I: NetworkGetCheckConnectionResult");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "NetworkGetGlobalNetworkStatus")]
        [HarmonyPrefix]
        static bool NetworkGetGlobalNetworkStatus(ref NetworkStatus __result) {
            Plugin.Log.LogWarning("N/I: NetworkGetGlobalNetworkStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "BeginUpdate")]
        [HarmonyPrefix]
        static bool BeginUpdate(ServiceClientStatus clientStatus) {
            Plugin.Log.LogWarning("N/I: BeginUpdate");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "ClearBackups")]
        [HarmonyPrefix]
        static bool ClearBackups(BackupCategory category) {
            Plugin.Log.LogWarning("N/I: ClearBackups");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "EndUpdate")]
        [HarmonyPrefix]
        static bool EndUpdate(ref long __result) {
            Plugin.Log.LogWarning("N/I: EndUpdate");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "GetError")]
        [HarmonyPrefix]
        static bool GetError(ref Error __result, int index) {
            Plugin.Log.LogWarning("N/I: GetError");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "GetErrorCount")]
        [HarmonyPrefix]
        static bool GetErrorCount(ref int __result) {
            __result = 0;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "GetInitStatus")]
        [HarmonyPrefix]
        static bool GetInitStatus(ref ModuleStatus __result) {
            Plugin.Log.LogWarning("N/I: GetInitStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "GetModuleInitStatus")]
        [HarmonyPrefix]
        static bool GetModuleInitStatus(ref ModuleStatus __result, ModuleType type) {
            Plugin.Log.LogWarning("N/I: GetModuleInitStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "GetModuleStatus")]
        [HarmonyPrefix]
        static bool GetModuleStatus(ref ModuleStatus __result, ModuleType type) {
            Plugin.Log.LogWarning("N/I: GetModuleStatus");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "ResetError")]
        [HarmonyPrefix]
        static bool ResetError(Error error) {
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "ServiceClientClose")]
        [HarmonyPrefix]
        static bool ServiceClientClose() {
            Plugin.Log.LogDebug("AMPF: ServiceClientClose");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "ServiceClientOpen")]
        [HarmonyPrefix]
        static bool ServiceClientOpen(ServiceClientType type) {
            Plugin.Log.LogDebug("AMPF: ServiceClientOpen(" + type + ")");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "UpdateFunc")]
        [HarmonyPrefix]
        static bool UpdateFunc(ref long __result, ServiceClientStatus clientStatus) {
            __result = 0;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "ConfigGetCurrentTime")]
        [HarmonyPrefix]
        static bool ConfigGetCurrentTime(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: ConfigGetCurrentTime");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "ConfigGetString")]
        [HarmonyPrefix]
        static bool ConfigGetString(ref bool __result, ConfigCategory category, string key, out IntPtr value) {
            Plugin.Log.LogWarning("N/I: ConfigGetString");
            value = Marshal.StringToHGlobalUni("");
            return false;
        }

        /*[HarmonyPatch(typeof(LINK.bnAMPF), "ConfigGetBoolean")]
        [HarmonyPrefix]
        static bool ConfigGetBoolean(ref Boolean __result, ConfigCategory category, String key, out Boolean value) {
            Plugin.Log.LogWarning("N/I: ConfigGetBoolean");
            value = false;
            return false;
        }

        [HarmonyPatch(typeof(LINK.bnAMPF), "ConfigGetInteger")]
        [HarmonyPrefix]
        static bool ConfigGetInteger(ref Boolean __result, ConfigCategory category, String key, out Int32 value) {
            Plugin.Log.LogWarning("N/I: ConfigGetInteger");
            value = 0;
            return false;
        }

        [HarmonyPatch(typeof(LINK.bnAMPF), "ConfigGetFloat")]
        [HarmonyPrefix]
        static bool ConfigGetFloat(ref Boolean __result, ConfigCategory category, String key, out Single value) {
            Plugin.Log.LogWarning("N/I: ConfigGetFloat");
            value = 0;
            return false;
        }*/

        [HarmonyPatch(typeof(bnAMPF), "SettingsGetBoolean")]
        [HarmonyPrefix]
        static bool SettingsGetBoolean(ref bool __result, string key, bool default_value) {
            //Plugin.Log.LogWarning("N/I: SettingsGetBoolean("+key+")");
            ConfigService config = ConfigService.GetInstance();
            if (key == SettingsKeyFreePlay) {
                __result = config.saveCoin.freePlay;
            } else if (key == SettingsKeyFreeOnceGasha) {
                __result = true;
            } else {
                __result = default_value;
            }
            return false;
        }

        private static IntPtr closingTime;
        [HarmonyPatch(typeof(bnAMPF), "SettingGetClosingTime")]
        [HarmonyPrefix]
        static bool SettingGetClosingTime(ref IntPtr __result) {
            if (closingTime == IntPtr.Zero) {
                closingTime = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(SettingsClosingTime)));
                var property = new SettingsClosingTime() {
                    Type = SettingsClosingTimeType.None
                };
                Marshal.StructureToPtr(property, closingTime, false);
            }
            __result = closingTime;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsGetInteger")]
        [HarmonyPrefix]
        static bool SettingsGetInteger(ref int __result, string key, int default_value) {
            ConfigService config = ConfigService.GetInstance();
            if (key == SettingsKeyAttractVolume) {
                __result = config.saveSound.atractVolume;
            } else if (key == SettingsKeySoundVolume) {
                __result = config.saveSound.gameVolume;
            } else {
                __result = default_value;
            }
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsGetProfile")]
        [HarmonyPrefix]
        static bool SettingsGetProfile(ref IntPtr __result) {
            Plugin.Log.LogWarning("N/I: SettingsGetProfile");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsGetString")]
        [HarmonyPrefix]
        static bool SettingsGetString(ref IntPtr __result, string key, string default_value) {
            Plugin.Log.LogWarning("N/I: SettingsGetString");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsGetTimeSpan")]
        [HarmonyPrefix]
        static bool SettingsGetTimeSpan(ref IntPtr __result, string key, IntPtr default_value) {
            Plugin.Log.LogWarning("N/I: SettingsGetTimeSpan");
            return false;
        }

        private static IntPtr closeTimePtr;
        [HarmonyPatch(typeof(bnAMPF), "SettingsGetTimeToClose")]
        [HarmonyPrefix]
        static bool SettingsGetTimeToClose(ref IntPtr __result) {
            if (closeTimePtr == IntPtr.Zero) {
                closeTimePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(bnAMPF.TimeSpan)));
                var property = new bnAMPF.TimeSpan() {
                    Seconds = 99999999
                };
                Marshal.StructureToPtr(property, closeTimePtr, false);
            }
            __result = closeTimePtr;
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsSetBoolean")]
        [HarmonyPrefix]
        static bool SettingsSetBoolean(string key, bool value) {
            Plugin.Log.LogWarning("N/I: SettingsSetBoolean");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsSetClosingTime")]
        [HarmonyPrefix]
        static bool SettingsSetClosingTime(IntPtr value) {
            Plugin.Log.LogWarning("N/I: SettingsSetClosingTime");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsSetInteger")]
        [HarmonyPrefix]
        static bool SettingsSetInteger(string key, int value) {
            Plugin.Log.LogWarning("N/I: SettingsSetInteger");
            return false;
        }

        [HarmonyPatch(typeof(bnAMPF), "SettingsSetString")]
        [HarmonyPrefix]
        static bool SettingsSetString(string key, string value) {
            Plugin.Log.LogWarning("N/I: SettingsSetString");
            return false;
        }



    }
}
