using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.Patches.Game {
    public class MatchingPatchesGame {

        [HarmonyPrefix, HarmonyPatch(typeof(PhotonNetwork), "ConnectUsingSettings")]
        static bool ConnectUsingSettings(string gameVersion) {
            Plugin.Log.LogDebug("Matching Server connection hook hit");
            string matchingIP = Plugin.ConfigNetworkMatchingIP.Value;
            PhotonNetwork.PhotonServerSettings.NetworkLogging = Plugin.ConfigPhotonLogging.Value ? ExitGames.Client.Photon.DebugLevel.ALL : ExitGames.Client.Photon.DebugLevel.ERROR;
            PhotonNetwork.PhotonServerSettings.PunLogging = Plugin.ConfigPhotonLogging.Value ? PhotonLogLevel.Full : PhotonLogLevel.ErrorsOnly;
            if (matchingIP != "") {
                PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.SelfHosted;
                PhotonNetwork.PhotonServerSettings.ServerAddress = matchingIP;
                Plugin.Log.LogMessage("Matching Server IP: " + matchingIP);
            }
            return true;
        }
    }
}
