using BepInEx.Configuration;
using HarmonyLib;
using LINK;
using LINK.Battle;
using System;
using System.Linq;
using UnityEngine;

namespace SwordArtOffline.Patches.Shared {

    extern alias AssemblyNotice;

    public class IOPatches {

        private static int CurrentCharacter = 1;

        // To prevent BepInEx's keyboard locking we kinda need to do this
        [HarmonyPatch(typeof(KeyboardShortcut), "ModifierKeyTest")]
        [HarmonyPrefix]
        static bool ModifierKeyTest(KeyboardShortcut __instance, ref bool __result) {
            __result = __instance.Modifiers.All(c => c == __instance.MainKey || Input.GetKey(c));
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "Initialize")]
        static bool Initialize(BnamPeripheral __instance) {
            __instance.errCode = BnamPeripheral.ErrorCodeSio.ERR_NONE;
            __instance.firstTime = true;
            __instance.boardName = "NBGI.SwordArtOffline I/O Emu";
            Plugin.Log.LogInfo("I/O initialized");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "Terminate")]
        static bool Terminate(BnamPeripheral __instance) {
            __instance.errCode = BnamPeripheral.ErrorCodeSio.ERR_NO_INITIALIZE;
            Plugin.Log.LogInfo("I/O uninitialized");
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "Update")]
        static bool Update(BnamPeripheral __instance, ref int __result) {
            __instance.comm_stat = true;
            __instance.sw_bak = __instance.sw_now;
            __instance.sw_now = 0;
            if (Plugin.ConfigButton1.Value.IsPressed() || (CurrentCharacter == 1 && Plugin.ConfigButtonAttackAll.Value.IsPressed())) {
                __instance.sw_now |= 2097152U;
                CurrentCharacter = 1;
            }
            if (Plugin.ConfigButton2.Value.IsPressed() || (CurrentCharacter == 2 && Plugin.ConfigButtonAttackAll.Value.IsPressed())) {
                __instance.sw_now |= 1048576U;
                CurrentCharacter = 2;
            }
            if (Plugin.ConfigButton3.Value.IsPressed() || (CurrentCharacter == 3 && Plugin.ConfigButtonAttackAll.Value.IsPressed())) {
                __instance.sw_now |= 524288U;
                CurrentCharacter = 3;
            }
            if (Plugin.ConfigButton4.Value.IsPressed()) {
                __instance.sw_now |= 256U;
            }
            if (Plugin.ConfigButton5.Value.IsPressed()) {
                __instance.sw_now |= 8388608U;
            }
            if (Plugin.ConfigButton6.Value.IsPressed()) {
                __instance.sw_now |= 4194304U;
            }
            if (Plugin.TestSwitchIsToggle) {
                if (Plugin.TestSwitchState) {
                    __instance.sw_now |= 128U;
                }
            } else if (Plugin.ConfigButtonTest.Value.IsPressed()) {
                __instance.sw_now |= 128U;
            }
            if (Plugin.ConfigButtonService.Value.IsPressed()) {
                __instance.sw_now |= 16384U;
            }
            if (Plugin.ConfigButtonTestEnter.Value.IsPressed()) {
                __instance.sw_now |= 512U;
            }
            if (Plugin.ConfigButtonTestUp.Value.IsPressed()) {
                __instance.sw_now |= 8192U;
            }
            if (Plugin.ConfigButtonTestDown.Value.IsPressed()) {
                __instance.sw_now |= 4096U;
            }
            __instance.sw_on = (__instance.sw_now ^ __instance.sw_bak) & __instance.sw_now;
            __instance.sw_off = (__instance.sw_now ^ __instance.sw_bak) & __instance.sw_bak;
            __result = 0;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "GetAxis")]
        static bool GetAxis(BnamPeripheral __instance, ref float __result, string axisName) {
            __result = 0F;
            if (axisName == "Horizontal") {
                if (Plugin.ConfigControlMouse.Value != Plugin.MouseButtonControl.Off) {
                    if (Input.GetMouseButton(Plugin.ConfigControlMouse.Value == Plugin.MouseButtonControl.LeftMouseButton ? 0 : 1)) {
                        __result = Mathf.Clamp((Input.mousePosition.x - Screen.width / 2) / Plugin.ConfigMouseRadiusWidth.Value, -1F, 1F);
                    }
                } else if (Plugin.ConfigControlAnalogStick.Value) {
                    __result = Input.GetAxis("Horizontal");
                } else if (Plugin.ConfigButtonAnalogLeft.Value.IsPressed()) {
                    __result = -1;
                } else if (Plugin.ConfigButtonAnalogRight.Value.IsPressed()) {
                    __result = 1;
                }
            } else if (axisName == "Vertical") {
                if (Plugin.ConfigControlMouse.Value != Plugin.MouseButtonControl.Off) {
                    if (Input.GetMouseButton(Plugin.ConfigControlMouse.Value == Plugin.MouseButtonControl.LeftMouseButton ? 0 : 1)) {
                        __result = Mathf.Clamp((Input.mousePosition.y - Screen.height / 2) / Plugin.ConfigMouseRadiusWidth.Value, -1F, 1F);
                    }
                } else if (Plugin.ConfigControlAnalogStick.Value) {
                    __result = Input.GetAxis("Vertical");
                } else if (Plugin.ConfigButtonAnalogUp.Value.IsPressed()) {
                    __result = 1;
                } else if (Plugin.ConfigButtonAnalogDown.Value.IsPressed()) {
                    __result = -1;
                }
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "GetAxisRaw")]
        static bool GetAxisRaw(BnamPeripheral __instance, ref ushort __result, string axisName) {
            __result = 127;
            if (axisName == "Horizontal") {
                if (Plugin.ConfigControlMouse.Value != Plugin.MouseButtonControl.Off) {
                    if (Input.GetMouseButton(Plugin.ConfigControlMouse.Value == Plugin.MouseButtonControl.LeftMouseButton ? 0 : 1)) {
                        __result = (ushort)(Mathf.Clamp((Input.mousePosition.x - Screen.width / 2) / Plugin.ConfigMouseRadiusWidth.Value, -1F, 1F) * 255);
                    }
                } else if (Plugin.ConfigControlAnalogStick.Value) {
                    __result = (ushort)(127 + Input.GetAxisRaw("Horizontal") * 128);
                } else if (Plugin.ConfigButtonAnalogLeft.Value.IsPressed()) {
                    __result = 0;
                } else if (Plugin.ConfigButtonAnalogRight.Value.IsPressed()) {
                    __result = 255;
                }
            } else if (axisName == "Vertical") {
                if (Plugin.ConfigControlMouse.Value != Plugin.MouseButtonControl.Off) {
                    if (Input.GetMouseButton(Plugin.ConfigControlMouse.Value == Plugin.MouseButtonControl.LeftMouseButton ? 0 : 1)) {
                        __result = (ushort)(Mathf.Clamp((Input.mousePosition.y - Screen.height / 2) / Plugin.ConfigMouseRadiusWidth.Value, -1F, 1F) * 255);
                    }
                } else if (Plugin.ConfigControlAnalogStick.Value) {
                    __result = (ushort)(127 + Input.GetAxisRaw("Vertical") * 128);
                } else if (Plugin.ConfigButtonAnalogUp.Value.IsPressed()) {
                    __result = 255;
                } else if (Plugin.ConfigButtonAnalogDown.Value.IsPressed()) {
                    __result = 0;
                }
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "GetFirmwareVersion")]
        static bool GetFirmwareVersion(BnamPeripheral __instance, ref ushort __result) {
            __result = 125;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "GetCoin")]
        static bool GetCoin(BnamPeripheral __instance, ref ushort __result) {
            __result = (ushort)Plugin.Coins;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "GetService")]
        static bool GetService(BnamPeripheral __instance, ref ushort __result) {
            __result = 0;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "ClearCoin")]
        static bool ClearCoin(BnamPeripheral __instance) {
            Plugin.Coins = 0;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "ClearService")]
        static bool ClearService(BnamPeripheral __instance) {
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BnamPeripheral), "GetCoinError")]
        static bool GetCoinError(BnamPeripheral __instance, ref BnamPeripheral.ErrorCodeCoin __result) {
            __result = BnamPeripheral.ErrorCodeCoin.ERR_NONE;
            return false;
        }

        internal static void ResetCharacterPosition() {
            CurrentCharacter = 1;
        }

    }
}
