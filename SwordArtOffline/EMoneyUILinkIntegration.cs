using eMoneyUILink;
using Haruka.Arcade.SEGA835Lib.Devices.Card._837_15396;
using System;

namespace SwordArtOffline {
    internal class EMoneyUILinkIntegration {

        private static bool initialized;

        internal static void Initalize(AimeCardReader_837_15396 aime) {
            EMUIApi.Aime = aime;
            EMUIApi.AddCoinEvent(EMoneyUILink_OnEMoneyCoins);
            initialized = true;
            EMUIApi.SetAlive(true);
            SetCardReaderBlocked(false);
            Plugin.Log.LogInfo("EMoneyUI integration enabled");
        }

        private static void EMoneyUILink_OnEMoneyCoins(int obj) {
            Plugin.Coins += obj;
        }

        internal static void SetCardReaderBlocked(bool b) {
            if (initialized) {
                EMUIApi.SetCardReaderBlocked(b);
            }
        }
    }
}