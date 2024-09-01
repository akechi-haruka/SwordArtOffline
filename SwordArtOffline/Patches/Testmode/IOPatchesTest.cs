using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.Patches.Testmode {

    extern alias AssemblyNotice;

    internal class IOPatchesTest {

        [HarmonyPrefix, HarmonyPatch(typeof(AssemblyNotice::BNUsio), "SetGout")]
        static bool SetGout(byte in_ucChannel, byte in_ucValue, ref int __result) {
            if (Plugin.Io4 != null && Plugin.IoConnected) {
                if (Plugin.ledStatus[in_ucChannel] != in_ucValue) {
                    Plugin.ledStatus[in_ucChannel] = in_ucValue;
                    if (in_ucChannel == 1 && Plugin.ConfigAttackButtonLed.Value > 0) {
                        Plugin.Io4.SetGPIO(Plugin.ConfigAttackButtonLed.Value, in_ucValue > 0);
                    } else if (in_ucChannel == 2 && Plugin.ConfigAttackButtonLed2.Value > 0) {
                        Plugin.Io4.SetGPIO(Plugin.ConfigAttackButtonLed2.Value, in_ucValue > 0);
                    } else if (in_ucChannel == 3 && Plugin.ConfigAttackButtonLed3.Value > 0) {
                        Plugin.Io4.SetGPIO(Plugin.ConfigAttackButtonLed3.Value, in_ucValue > 0);
                    }
                }
            }
            __result = 0;
            return false;
        }

    }
}
