using Artdink.Native;
using Artdink.StaticData;
using GssSiteSystem;
using HarmonyLib;
using Haruka.Arcade.SEGA835Lib.Devices;
using LINK;
using LINK.ADVProgram.Inner.Sound;
using LINK.ADVProgram.Inner.UI;
using LINK.Battle;
using LINK.TestMode;
using LINK.UI;
using SwordArtOffline.NetworkEx;
using SwordArtOffline.Patches.Shared;
using System;
using System.Collections;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Utage;

namespace SwordArtOffline.Patches.Game {

    extern alias AssemblyNotice;
    using static bnAMUpdater;
    using static GssSiteSystem.GameConnect;
    using Color = Haruka.Arcade.SEGA835Lib.Misc.Color;

    public class BasePatchesGame {
        public static bool RetryNetworkFlag { get; internal set; }
        //private static CommonUI_MenuDialog LoadingDialog { get; set; }

        [HarmonyPrefix, HarmonyPatch(typeof(Win32Methods), "SetTopmostWindow")]
        static bool SetTopmostWindow(IntPtr hWnd, bool topmost) {
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Win32Methods), "SetForegroundWindowInternal")]
        static bool SetForegroundWindowInternal(IntPtr hWnd) {
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ErrorCheckManager), "OccurredError")]
        static bool OccurredError(SystemErrorCode err, bool log = true) {
            // hardcoded ip addresses, sigh
            if (err == SystemErrorCode.NET_DIFF_ROUTER) {
                return false;
            }
            // let's swallow these so switching between terminal and satellite isn't a pain and we have to change dongle IDs on the fly
            if (err == SystemErrorCode.SEC_SPEC_DONGLE) {
                return false;
            }
            //Plugin.Log.LogDebug("OccurredError: " + err + " from " + Environment.StackTrace);
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(GameConnectHttp), "connectHttp", typeof(string), typeof(byte[]), typeof(int), typeof(int), typeof(string))]
        static bool connectHttp(string url, byte[] data, int off, int size, string cont_type) {
            Plugin.Log.LogDebug("connectHttp: " + url);
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(GameConnectEncrypt), "encode")]
        static bool encode(ref byte[] __result, byte[] b, int off, int len) {
            if (Plugin.ConfigNetworkEncryption.Value) {
                return true;
            } else {
                Plugin.Log.LogDebug("Encryption disabled");
                byte[] array = new byte[len];
                Buffer.BlockCopy(b, off, array, 0, len);
                __result = array;
                return false;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(GameConnectEncrypt), "decode")]
        static bool decode(ref byte[] __result, byte[] b, int off, int len) {
            if (Plugin.ConfigNetworkEncryption.Value) {
                return true;
            } else {
                Plugin.Log.LogDebug("Decryption disabled");
                byte[] array = new byte[len];
                Buffer.BlockCopy(b, off, array, 0, len);
                __result = array;
                return false;
            }
        }


        [HarmonyPrefix, HarmonyPatch(typeof(bnReader), "StartReadCard")]
        static bool StartReadCard(ref bool __result) {
            bool result = false;
            if (bnReader.AimeIsEnable()) {
                Plugin.Log.LogDebug("AimeIsEnable OK");
                int num = bnReader.AimeGetCardReaderTotal();
                if (num > 0) {
                    Plugin.Log.LogDebug("AimeGetCardReaderTotal OK");
                    if (bnReader.AimeDetectCard(0)) {
                        Plugin.Log.LogDebug("AimeDetectCard OK");
                        if (bnReader.GetCardReaderInfo().bConnect != 0) {
                            Plugin.Log.LogDebug("GetCardReaderInfo OK");
                            result = true;
                        }
                    }
                }
            }
            __result = result;
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(GameManager), "initializeAmdaemonInfo")]
        static void initializeAmdaemonInfo() {
            Plugin.Log.LogDebug("INIT = " + GameManager.IsAllNetInit);
            amdaemon_info amdaemonInfo = GetInfo_wrapped();
            Plugin.Log.LogDebug("AUTHTYPE = " + amdaemonInfo.authType);
            Plugin.Log.LogDebug("MUCHA ERROR = " + amdaemonInfo.mucha.daemon_error);
            Plugin.Log.LogDebug("MUCHA STATE = " + amdaemonInfo.mucha.daemon_state);
            Plugin.Log.LogDebug("MUCHA STATE AUTH = " + amdaemonInfo.mucha.state_auth);
            Plugin.Log.LogDebug("ALLNET ERROR = " + amdaemonInfo.allnet.last_error);
            Plugin.Log.LogDebug("ALLNET DAEMON ERROR = " + amdaemonInfo.allnet.daemon_error);
            Plugin.Log.LogDebug("ALLNET AUTH = " + amdaemonInfo.allnet.state_auth);
            bool b = (amdaemonInfo.error == DAEMONERROR.AMDAEMON_ERROR_NONE || amdaemonInfo.error == DAEMONERROR.AMDAEMON_ERROR_AUTH_COMMUNICATION || amdaemonInfo.error == DAEMONERROR.AMDAEMON_ERROR_NOAUTH || amdaemonInfo.error == DAEMONERROR.AMDAEMON_ERROR_DL_TIMEOUT || amdaemonInfo.error == DAEMONERROR.AMDAEMON_ERROR_CHARGE_COMMUNICATION) && amdaemonInfo.allnet.state_auth == 2;
            Plugin.Log.LogDebug("--- CHECK --- " + b);
            if (b) {
                allnet_ainfo authinfo = GetAuthInfoAllnet_wrapped();
                Plugin.Log.LogDebug("NAME = " + Encoding.ASCII.GetString(authinfo.name));
                Plugin.Log.LogDebug("URL = " + Encoding.ASCII.GetString(authinfo.uri));
            }
            Plugin.Log.LogDebug("--- READY? --- ");
            Plugin.Log.LogDebug("INSTANCE = " + (GameManager.IsJig && !GameManager.IsPrintTest && (GameManager.IsSatellite || GameManager.IsTerminal)));
            Plugin.Log.LogDebug("ALLNET INIT = " + GameManager.IsAllNetInit);
            amdaemon_info amdaemon_info = GameManager.Instance.getAmdaemonInfo();
            Plugin.Log.LogDebug("ALLNET ERROR OK = " + ((amdaemon_info.error == DAEMONERROR.AMDAEMON_ERROR_NONE || amdaemon_info.error == DAEMONERROR.AMDAEMON_ERROR_AUTH_COMMUNICATION || amdaemon_info.error == DAEMONERROR.AMDAEMON_ERROR_NOAUTH || amdaemon_info.error == DAEMONERROR.AMDAEMON_ERROR_DL_TIMEOUT || amdaemon_info.error == DAEMONERROR.AMDAEMON_ERROR_CHARGE_COMMUNICATION) && amdaemon_info.allnet.state_auth == 2));
            Plugin.Log.LogDebug("ALLNET NOT AUTHED YET = " + !GameManager.s_AllNetAuth);
            Plugin.Log.LogDebug("AMPF UPDATE? = " + GameManager.Instance.m_bnAMPUpdate);
            Plugin.Log.LogDebug("AMPF KEYCHIP = " + bnAMPF.USBDongleGetSerialNumber_wrapped());

        }


        [HarmonyPrefix, HarmonyPatch(typeof(GameConnect.GssProtocolBase), "connect", typeof(int))]
        private static bool connect(ref GameConnect.GssProtocolBase __result, GameConnect.GssProtocolBase __instance, int send_buf_size) {
            //Plugin.Log.LogDebug("GssProtocolBase.connect");
            GameConnect.GssProtocolBase gssProtocolBase = null;
            GameConnectError error = GameConnectError.FAILED_ERR;
            string text = __instance.connect_getURL();
            //Plugin.Log.LogDebug("URL="+text);
            if (text != null) {
                GameConnectEncrypt encrypt = new GameConnectEncrypt(GameConnectDef.Base_3DES_key, GameConnectDef.Base_3DES_iv);
                try {
                    byte[] array = new byte[send_buf_size];
                    int num = __instance.httpRequest(array, 0, array.Length, encrypt);
                    if (num > 0) {
                        string url = GameConnectDef.Base_URL + text + ".php";
                        GameConnectHttp._response_code = 0;
                        byte[] array2 = GameConnectHttp.connectHttp(url, array, 0, num, GameConnect.GssProtocolBase.Base_ContentType);
                        if (array2 != null) {
                            if (array2.Length > 0) {
                                gssProtocolBase = __instance.httpResponse(array2, 0, array2.Length, encrypt);
                                error = GameConnectError.SUCCESS;
                            } else {
                                WebExceptionStatus error_code = GameConnectHttp._error_code;
                                if (error_code == WebExceptionStatus.Timeout) {
                                    Plugin.Log.LogError("Network request failed: Network Timeout (length = 0)");
                                    error = GameConnectError.TIME_OUT;
                                } else {
                                    Plugin.Log.LogError("Network request failed: Response length = 0");
                                    error = GameConnectError.SERVER_ERR;
                                }
                            }
                        } else {
                            WebExceptionStatus error_code2 = GameConnectHttp._error_code;
                            if (error_code2 == WebExceptionStatus.Timeout) {
                                Plugin.Log.LogError("Network request failed: Network Timeout (null)");
                                error = GameConnectError.TIME_OUT;
                            } else {
                                Plugin.Log.LogError("Network request failed: Response is null");
                                error = GameConnectError.SERVER_ERR;
                            }
                        }
                    } else {
                        Plugin.Log.LogError("Network request failed: num=0");
                        error = GameConnectError.PARAM_ERR;
                    }
                } catch (Exception ex) {
                    Plugin.Log.LogError("Network request failed: Exception " + ex);
                    error = GameConnectError.OPERATE_ERR;
                }
            }
            if (gssProtocolBase == null) {
                gssProtocolBase = new GameConnect.GssProtocolBase((GAMECONNECT_CMDID)65535);
            }
            if (!(gssProtocolBase is GssProtocolError)) {
                gssProtocolBase._error = error;
            }
            __result = gssProtocolBase;
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GameConnect.GssProtocolBase), "connect", typeof(int))]
        static void Connect(ref GameConnect.GssProtocolBase __result, GameConnect.GssProtocolBase __instance, int send_buf_size) {
            if (!(__result is GssProtocolError) && __result._error != GameConnectError.SUCCESS && Plugin.ConfigNetworkAutoRetry.Value) {
                bool isAutoRetry = Plugin.ConfigNetworkAutoRetryDelay.Value < 99;
                if (Plugin.ConfigReconnectUseDialogSystem.Value) {
                    RetryNetworkFlag = false;
                    Plugin.ShowDialog("A connection error has ocurred.<br>" + __result._cmd_id + "<br>" + __result._error + "<br><br><color=orange>" + (isAutoRetry ? "Retrying..." : "Press OK to retry."), Plugin.ConfigNetworkAutoRetryDelay.Value >= 99 ? null : Plugin.ConfigNetworkAutoRetryDelay.Value, false, (ret) => {
                        RetryNetworkFlag = true;
                    });
                    do {
                        Thread.Sleep(1000);
                    } while (!RetryNetworkFlag);
                } else if (!isAutoRetry) {
                    RetryNetworkFlag = false;
                    Plugin.Log.LogMessage("Connection error, retrying when " + Plugin.ConfigButtonRetryNetworkImmediately.Value.MainKey + " is pressed...");
                    do {
                        Thread.Sleep(1000);
                    } while (!RetryNetworkFlag);
                    RetryNetworkFlag = false;
                    Plugin.Log.LogMessage("Key detected, retrying...");
                } else {
                    RetryNetworkFlag = false;
                    int wait = Plugin.ConfigNetworkAutoRetryDelay.Value;
                    Plugin.Log.LogMessage("Connection error, retrying in " + wait + " seconds...");
                    do {
                        Thread.Sleep(1000);
                    } while (!RetryNetworkFlag && wait-- > 0);
                }
                __result = __instance.connect(send_buf_size);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MasterDataVersionCheckProtocol), "ReceieveResponse")]
        static void ReceieveResponse(MasterDataVersionCheckProtocol __instance) {
            if (((GameConnectProt.master_data_version_check_R)__instance.Response).update_flag > 0) {
                //LoadingDialog = Plugin.ShowDialog("Local game data is outdated. Updating game data from server...<br>This may take a moment.", null, null);
                //Plugin.Log.LogMessage("Updating master data... This may take a few minutes.");
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MenuUITime), "SetLimitTime")]
        static void SetLimitTime(MenuUITime __instance, float time) {
            if (Plugin.ConfigTimeExtend.Value && time >= 60F) {
                __instance.m_fLimitTime = Math.Min(time * 5, 999);
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuUITime), "updateTime", typeof(float), typeof(float), typeof(bool))]
        static bool updateTime(float nowtime, float deltaTime, bool forceActive) {
            return !Plugin.ConfigTimeFreeze.Value;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AdvSelectionTimeLimit), "OnUpdateWaitInput")]
        static bool updateTime(AdvSelectionManager selection, AdvSelectionTimeLimit __instance) {
            if (Plugin.ConfigTimeFreeze.Value) {
                return false;
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AdvSelectionTimeLimitText), "OnUpdateWaitInput")]
        static bool updateTime(AdvSelectionManager selection, AdvSelectionTimeLimitText __instance) {
            if (Plugin.ConfigTimeFreeze.Value) {
                return false;
            }

            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(ADVTimer), "Countdown")]
        static bool Countdown() {
            if (Plugin.ConfigTimeFreeze.Value) {
                return false;
            }

            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuUILocalData), "IsShopUsed", MethodType.Getter)]
        static bool IsShopUsed(ref bool __result) {
            if (Plugin.ConfigRepeatMenuSections.Value) {
                __result = false;
                return false;
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuUILocalData), "IsUsedTicketInCustomMenu", MethodType.Getter)]
        static bool IsUsedTicketInCustomMenu(ref bool __result) {
            if (Plugin.ConfigRepeatMenuSections.Value) {
                __result = false;
                return false;
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuUILocalData), "IsImportCardUsed", MethodType.Getter)]
        static bool IsImportCardUsed(ref bool __result) {
            if (Plugin.ConfigRepeatMenuSections.Value) {
                __result = false;
                return false;
            }
            return true;
        }

        internal static void Initialize() {
            // this needs to be moved to a class that's not loaded in testmode
            GameConnect.setTimeOutTime(20);
            // this needs to be initialized explicitely otherwise the terminal dies
            BnamPeripheral.GetInstance().boardName = "NBGI.SwordArtOffline I/O Emu";


            for (int i = 0; i < 79; i++) {
                CommonUI_StorageBoxDataManager.instance.saleLogs.Add(new SelectTubLogData());
            }
        }

        // reimplement this entire thing
        [HarmonyPostfix, HarmonyPatch(typeof(SubSceneCtrl_Menu_MainMenu_Parent), "GashaCM", MethodType.Getter)]
        static void InitParentScene(SubSceneCtrl_Menu_MainMenu_Parent __instance) {
            Plugin.Log.LogDebug("InitParentScene (trigger on GashaCM_get)");
            if (!Plugin.ConfigRepeatMenuSections.Value) {
                return;
            }
            if (__instance.MainMenuButtons == null || __instance.MainMenuButtons.EpisodeBtn == null || __instance.MainMenuButtons.EpisodeBtn.MenuButton == null) {
                Plugin.Log.LogWarning("MainMenu not initialized!");
                return;
            }
            MenuUIButton menuButton = __instance.MainMenuButtons.EpisodeBtn.MenuButton;
            menuButton.onDown.RemoveAllListeners();
            menuButton.onDown.AddListener(delegate () {
                if (__instance.m_NotificationEvent != null && !__instance.m_NotificationEvent.IsFinished && !__instance.m_IsTimeUpStart || __instance.m_IsTimeUpStart) {
                    return;
                }
                int need_ticket = !MenuUIManager.Instance.LocalData.IsFirstPlay || MenuUIManager.Instance.LocalData.IsFirstFreePlayedFlag ? TicketMenuUtils.GetGamePlayerPriceDataByNowRank().EpisodeTicket : 0;
                BeginnerMissionUIManager.Instance.CloseTemporary(false);
                UnityAction checkTicketAction = null;
                UnityAction okAct = delegate () {
                    __instance.GoToEpisodeScene();
                };
                UnityAction ngAct = delegate () {
                    __instance.UseTicketButtonAct(checkTicketAction);
                };
                checkTicketAction = delegate () {
                    if (__instance.checkTicket(need_ticket)) {
                        okAct();
                    } else {
                        ngAct();
                    }
                };
                checkTicketAction();
            });

            MenuUIButton menuButton2 = __instance.MainMenuButtons.TrialTowerBtn.MenuButton;
            menuButton2.onDown.RemoveAllListeners();
            menuButton2.onDown.AddListener(delegate () {
                if (__instance.m_NotificationEvent != null && !__instance.m_NotificationEvent.IsFinished && !__instance.m_IsTimeUpStart || __instance.m_IsTimeUpStart) {
                    return;
                }
                BeginnerMissionUIManager.Instance.CloseTemporary(false);
                UnityAction checkTicketAction = null;
                UnityAction okAct = delegate () {
                    __instance.GoToTrialTowerScene();
                };
                UnityAction ngAct = delegate () {
                    __instance.UseTicketButtonAct(checkTicketAction);
                };
                checkTicketAction = delegate () {
                    if (__instance.checkTicket(TicketMenuUtils.GetGamePlayerPriceDataByNowRank().TrialTowerTicket)) {
                        okAct();
                    } else {
                        ngAct();
                    }
                };
                checkTicketAction();
            });

            MenuUIButton menuButton3 = __instance.MainMenuButtons.DefragMatchBtn.MenuButton;
            menuButton3.onDown.RemoveAllListeners();
            menuButton3.onDown.AddListener(delegate () {
                if (__instance.m_NotificationEvent != null && !__instance.m_NotificationEvent.IsFinished && !__instance.m_IsTimeUpStart || __instance.m_IsTimeUpStart) {
                    return;
                }
                BeginnerMissionUIManager.Instance.CloseTemporary(false);
                UnityAction checkTicketAction = null;
                UnityAction okAct = delegate () {
                    __instance.GoToDefragMatchScene();
                };
                UnityAction ngAct = delegate () {
                    __instance.UseTicketButtonAct(checkTicketAction);
                };
                checkTicketAction = delegate () {
                    if (__instance.checkTicket(TicketMenuUtils.GetGamePlayerPriceDataByNowRank().SubdueTicket)) {
                        okAct();
                    } else {
                        ngAct();
                    }
                };
                checkTicketAction();
            });

            MenuUIButton menuButton4 = __instance.MainMenuButtons.CustomBtn.MenuButton;
            menuButton4.onDown.RemoveAllListeners();
            menuButton4.onDown.AddListener(delegate () {
                if (__instance.m_NotificationEvent != null && !__instance.m_NotificationEvent.IsFinished && !__instance.m_IsTimeUpStart || __instance.m_IsTimeUpStart) {
                    return;
                }
                BeginnerMissionUIManager.Instance.CloseTemporary(false);
                __instance.GoToCustomScene();
            });

            MenuUIButton menuButton5 = __instance.MainMenuButtons.ShopBtn.MenuButton;
            menuButton5.onDown.RemoveAllListeners();
            menuButton5.onDown.AddListener(delegate () {
                if (__instance.m_NotificationEvent != null && !__instance.m_NotificationEvent.IsFinished && !__instance.m_IsTimeUpStart || __instance.m_IsTimeUpStart) {
                    return;
                }
                BeginnerMissionUIManager.Instance.CloseTemporary(false);
                __instance.GoToShopScene();
            });

            MenuUIButton menuButton6 = __instance.MainMenuButtons.ImportCardBtn.MenuButton;
            menuButton6.onDown.RemoveAllListeners();
            menuButton6.onDown.AddListener(delegate () {
                if (__instance.m_NotificationEvent != null && !__instance.m_NotificationEvent.IsFinished && !__instance.m_IsTimeUpStart || __instance.m_IsTimeUpStart) {
                    return;
                }
                BeginnerMissionUIManager.Instance.CloseTemporary(false);
                __instance.GoToImportCardScene();
            });

            if (Plugin.ConfigAllowTerminalSwitch.Value) {
                MenuUIButton menuButton7 = __instance.MainMenuButtons.LogoutBtn.MenuButton;
                menuButton7.onDown.RemoveAllListeners();
                menuButton7.onDown.AddListener(delegate () {
                    if (__instance.m_NotificationEvent != null && !__instance.m_NotificationEvent.IsFinished && !__instance.m_IsTimeUpStart || __instance.m_IsTimeUpStart) {
                        return;
                    }
                    BeginnerMissionUIManager.Instance.CloseTemporary(false);
                    __instance.MenuDialog.DialogDisplay("CHECK_COMMON_LOGOUT", new UnityAction(__instance.SendEndPayingPlayData), delegate () {
                        __instance.MenuDialog.DialogDisplay("CHECK_DIALOG_CHANGE_TERMINAL", delegate () {
                            IsTerminalByLogOutMoveState = true;
                            GameManager.s_Terminal = true;
                            MenuUIManager.Instance.SetUpMenu(MenuUISceneSetUp.SetUpScene.TERMINAL_MAINMENU, true);
                        }, delegate () {
                            BeginnerMissionUIManager.Instance.OpenTemporary(false);
                        }, false, 10f, __instance.m_thisTimeType, null, false);
                    }, false, 10f, __instance.m_thisTimeType, null, false);
                });
            }

            // this is actually GMG logic but doesn't really hurt
            if (UserDataManager.Instance.DefragMatchBasicUserData.State == DefragMatchEventState.NONE || UserDataManager.Instance.DefragMatchBasicUserData.DefragMatchId == 0) {
                __instance.MainMenuButtons.DefragMatchBtn.SetButtonActive(false);
                __instance.MainMenuButtons.DefragMatchBtn.SetOpenEffectActive(true, "A new Defrag Match will start soon!");
            }
        }

        private static bool IsTerminalByLogOutMoveState = false;

        [HarmonyPostfix, HarmonyPatch(typeof(SubSceneCtrl_Menu_Terminal_MainMenu_Parent), "InitParentScene")]
        static IEnumerator InitParentScene(IEnumerator __result, SubSceneCtrl_Menu_Terminal_MainMenu_Parent __instance) {

            // Run original enumerator code
            while (__result.MoveNext())
                yield return __result.Current;

            if (Plugin.ConfigAllowTerminalSwitch.Value) {
                __instance.mainMenu.logOut.onDown.RemoveAllListeners();
                __instance.mainMenu.AddLogOutAction(delegate {
                    if (MenuUIManager.Instance.IsLoading) {
                        return;
                    }
                    if (!__instance.m_bOpenBeginnerMission || !BeginnerMissionUIManager.Instance.IsShowAnimEnd || __instance.mainMenu.m_bNotMove) {
                        return;
                    }
                    BeginnerMissionUIManager.Instance.CloseTemporary(false);
                    __instance.menuDialog.DialogDisplay("CHECK_COMMON_LOGOUT_TM", delegate () {
                        IsTerminalByLogOutMoveState = false;
                        GameManager.s_Terminal = false;
                        CommonUI_TerminalStaticDataManager.LogoutAction();
                    }, delegate () {
                        __instance.menuDialog.DialogDisplay("CHECK_DIALOG_CHANGE_STATION", delegate () {
                            IsTerminalByLogOutMoveState = false;
                            GameManager.s_Terminal = false;
                            MenuUIManager.Instance.SetUpMenu(MenuUISceneSetUp.SetUpScene.MAIN_MENU, true);
                        }, delegate () {
                            BeginnerMissionUIManager.Instance.OpenTemporary(false);
                        }, false, 10f, MenuUITime.TimeType.CustomMenu, null, false);
                    }, false, 10f, MenuUITime.TimeType.CustomMenu, null, false);
                });
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(SubScene_Menu_StorageBox_Parent), "InitParentScene")]
        static IEnumerator InitParentScene2(IEnumerator __result, SubScene_Menu_StorageBox_Parent __instance) {

            // Run original enumerator code
            while (__result.MoveNext())
                yield return __result.Current;

            if (Plugin.ConfigAllowTerminalSwitch.Value && !GameManager.IsTerminal) {
                __instance.returnButton.returnButton.onDown.RemoveAllListeners();
                __instance.returnButton.ReturnButtonAddLisner(delegate {
                    if (MenuUIManager.Instance.IsLoading) {
                        return;
                    }
                    __instance.m_bCanForceLogoutCall = false;
                    LoadingUIManager.Instance.Show(true);
                    __instance.StartCoroutine(CommonUI_StorageBoxDataManager.Instance.SendDataSurverLock(delegate {
                        __instance.storageBox.SaleLogWindow.IsUpdate = false;
                        MenuUIManager.Instance.SetUpMenu((!IsTerminalByLogOutMoveState) ? MenuUISceneSetUp.SetUpScene.CUSTOM_MENU : MenuUISceneSetUp.SetUpScene.TERMINAL_MAINMENU, true);
                        LoadingUIManager.Instance.Hide();
                    }));
                });
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CommonUI_MenuDialog), "requestInstantiateDialog")]
        static bool requestInstantiateDialog(string displayDialog, UnityAction yes, UnityAction no, UnityAction[] ex, bool isEndYes, float dialogTime, MenuUITime.TimeType backUpTimeType, string seName, object[] argument, bool textFormat = false) {
            if (Plugin.ConfigRepeatMenuSections.Value) {
                if (displayDialog == "CHECK_SHOP_EXIT" || displayDialog == "CHECK_CUSTOM_BACK") {
                    if (yes != null) {
                        yes.Invoke();
                        return false;
                    }
                }
            }
            return true;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GameManager), "MasterDataDownloadAfterLoad")]
        static IEnumerator MasterDataDownloadAfterLoad(IEnumerator __result) {
            if (LoadingUIManager.HasInstance()) {
                LoadingUIManager.Instance.Show();
            }
            Plugin.Log.LogDebug("Server data version: " + ServerDataManager.Instance.CurrentDataVersion);
            if (ServerDataManager.Instance.CurrentDataVersion >= 1_000_000) {
                Plugin.Log.LogInfo("Accessing extra data...");
                Type[] exDataProtocolTypes = new Type[] {
                    typeof(GetExGMGDialogUIProtocol),
                    typeof(GetExGMGTextCommonProtocol),
                    typeof(GetExGMGTextCharaMessageProtocol),
                    typeof(GetExGMGTextCharaCommentProtocol),
                    typeof(GetExGMGFixedFormChatProtocol),
                    typeof(GetExGMGFixedFormChatTextProtocol),
                    typeof(GetExGMGDefragMatchChatProtocol),
                    typeof(GetExGMGOberonMedalGetConditionProtocol)
                };
                foreach (Type ex in exDataProtocolTypes) {
                    NetworkManageComponent protocol = NetworkProtocolManager.SendRequest(ex, true);
                    yield return protocol.WaitResultCoroutine();
                    if (!protocol.IsResultSucccess()) {
                        if (LoadingUIManager.HasInstance()) {
                            LoadingUIManager.Instance.Hide();
                        }
                        ServerDataManager.Instance.CurrentDataVersion = 0;
                        ServerDataManager.Instance.DownloadMasterDataErrorFlag = true;
                        ServerDataManager.Instance.errorProtocol = protocol;
                        yield break;
                    }
                    ((GetMasterDataProtocolBase)protocol).SetupResponseData();
                }

                foreach (StaticEventData ev in CommonManager<StaticEventDataManager, StaticEventData>.GetInstance()) {

                    if (ev.EventType == 37 && ev.OpenStartDate <= DateTime.Now && DateTime.Now <= ev.OpenEndDate) {
                        if (ev.Title == "HARUKA_GMG_EX_MATCHING_SERVER_IP") {
                            if (ev.Param3 != "0") {
                                MatchingPatchesGame.ExMatchingIP = ev.Param1;
                                MatchingPatchesGame.ExMatchingPort = Int32.Parse(ev.Param2);
                            }
                        } else if (ev.Title == "HARUKA_GMG_EX_EXP_FILE_ASM") {
                            // flag unclean dlls when reporting issues
                            string local = Plugin.Md5("link_Data/Managed/" + ev.Param1) ?? Plugin.Md5("game/link_Data/Managed/" + ev.Param1);
                            if (local != ev.Param2) {
                                Plugin.Log.LogWarning("EXP_FILE_ASM FAILED (" + ev.Param1 + "): " + local);
                            }
                        } else {
                            Plugin.Log.LogWarning("Unknown EX Event: " + ev.EventId + "/" + ev.Title);
                        }
                    }
                }
            }

            //LoadingDialog?.Hide();
            if (LoadingUIManager.HasInstance()) {
                LoadingUIManager.Instance.Hide();
            }

            // Run original enumerator code
            while (__result.MoveNext())
                yield return __result.Current;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(SceneQuest), "CheckContinueOrNot")]
        static bool CheckContinueOrNot(SceneQuest __instance) {
            Plugin.AutoContinueMode val = Plugin.ConfigAutoContinueMode.Value;

            bool doInstantContinue = false;
            bool doInstantCancel = false;

            short num = CommonManager<StaticGamePlayPriceDataManager, StaticGamePlayPriceData>.GetInstance().GetRecordByIndex(0).FreeContinueNum;
            if (num > 0 && GameManager.IsJig && !ConfigService.GetInstance().continueBonus) {
                num = 0;
            }
            int lastFreeContinue = Math.Max(0, num - __instance.m_Quest.ContinueCnt);

            if (val == Plugin.AutoContinueMode.Off) {
                return true;
            } else if (val == Plugin.AutoContinueMode.NeverContinue) {
                doInstantCancel = true;
            } else if (val == Plugin.AutoContinueMode.ContinueIfFree) {
                doInstantContinue = (lastFreeContinue > 0);
            } else if (val == Plugin.AutoContinueMode.ContinueIfTicket) {
                doInstantContinue = MenuUIManager.Instance.LocalData.Ticket > 0;
            } else if (val == Plugin.AutoContinueMode.AlwaysContinue) {
                doInstantContinue = true;
            }

            if (doInstantContinue) {
                __instance.ContinueYesAction(lastFreeContinue);
                return false;
            } else if (doInstantCancel) {
                __instance.m_Quest.ResultType = QuestResultType.Failed;
                BattleManager.Instance.LocalTeamController.Logout();
                HudManager.Instance.isDialogPlaying = false;
                return false;
            } else {
                return true;
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ErrorUIManager), "ShowError")]
        static void ShowError(string dialog_record_id, string err_code, bool bGameStop) {
            if (bGameStop) {
                Plugin.Log.LogDebug("ErrorUI called from\n" + Environment.StackTrace);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ErrorUIManager), "ShowNetworkErrorAfterLogin")]
        static void ShowNetworkErrorAfterLogin(NetworkManageComponent protocol) {
            Plugin.Log.LogDebug("ErrorUI (network) called from\n" + Environment.StackTrace);
        }

        // cause matching server issues not to lock the game
        [HarmonyPrefix, HarmonyPatch(typeof(GameManager), "HandleException")]
        static bool HandleException(string logString, string stackTrace, LogType type) {
            if ((type == LogType.Exception || type == LogType.Error || type == LogType.Assert) && logString.IndexOf("Exception") > 0 && logString.IndexOf("ExitGames") < 0) {
                return true;
            }
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(TeamPad), "getKeyboardCode")]
        static bool getKeyboardCode(TeamPad.Key key) { // some debugging thing that may conflict with custom binds
            return false;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(TeamController), "updateMove")]
        static bool updateMove(TeamController __instance) { // this needs a full override so we can get rid of the default WASD bindings here
            UnitData unitByFormation = __instance.getUnitByFormation(0);
            __instance.updateParent(unitByFormation);
            Quaternion cameraRotationH = LINK.Util.SingletonMonoBehaviour<BattleManager>.Instance.GetCameraRotationH();
            Vector3 vector = Vector3.zero;
            Vector3 padAxis = __instance.PadAxis;
            bool flag = unitByFormation == null || (unitByFormation.CanMove() && !unitByFormation.jumpParam.HasRequest);
            if (flag) {
                vector = padAxis;
                vector = cameraRotationH * vector;
            }
            if (unitByFormation != null && !__instance.IsAutoPlay && vector.sqrMagnitude > Mathf.Epsilon) {
                vector = Vector3.Normalize(vector);
                if (__instance.questionRotation.IsFinished) {
                    float num = __instance.questionRotation.Value + UnityEngine.Random.Range(-180f, 180f);
                    __instance.questionRotation = new EasingValue(__instance.questionRotation.Value, num, 1f);
                }
                __instance.questionRotation.Update(Time.deltaTime);
                if (unitByFormation.badStatus[3].IsActive) {
                    vector = Quaternion.AngleAxis(__instance.questionRotation.Value, Vector3.up) * vector;
                }
                unitByFormation.unitMove.Move(unitByFormation.transform.position + vector, false);
                unitByFormation.unitMove.Rotate(vector, 0f);
            }
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MenuUIManager), "RequestAndWait")]
        static IEnumerator RequestAndWait(IEnumerator __result, params Func<NetworkManageComponent>[] requestfuncs) {

            if (LoadingUIManager.HasInstance()) {
                ShowLoading();
            }

            // Run original enumerator code
            while (__result.MoveNext())
                yield return __result.Current;

            if (LoadingUIManager.HasInstance()) {
                HideLoading();
            }

            yield return __result.Current;
        }

        private static void ShowLoading() {
            if (loadingUIFadeCoroutine != null) {
                LoadingUIManager.Instance.StopCoroutine(loadingUIFadeCoroutine);
            }
            LoadingUIManager.Instance.Show();
        }

        private static void HideLoading() {
            if (loadingUIFadeCoroutine != null) {
                LoadingUIManager.Instance.StopCoroutine(loadingUIFadeCoroutine);
            }
            loadingUIFadeCoroutine = LoadingUIManager.Instance.StartCoroutine(LoadingUIFadeOut());
        }

        private static Coroutine loadingUIFadeCoroutine;

        private static IEnumerator LoadingUIFadeOut() {
            yield return new WaitForSeconds(0.5F);
            LoadingUIManager.Instance.Hide();
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MenuDialogConfirmation), "Init")]
        static void Init(MenuDialogConfirmation __instance, StaticDialogUIData dialogData, UnityAction _yes, UnityAction _no, UnityAction[] _ex, bool _isEndYes, float _dialogTime, MenuUITime.TimeType _backUpTimeType, string _seName, UnityAction _deletePointer, object[] argument = null, bool textFormat = false) {
            if (_dialogTime < 0) {
                MenuUIManager.Instance.SetLimitTime(MenuUITime.TimeType.Generic, 0);
                MenuUIManager.Instance.StopTimerControl();
            }
        }

        // for touch
        [HarmonyPostfix, HarmonyPatch(typeof(TeamController), "execSwitch")]
        static void execSwitch(int index, bool wait_skill, ref bool __result, TeamController __instance) {
            if (__result && !__instance.otherPlayer) {
                IOPatches.CurrentCharacter = index + 1;
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(BNUsio), "SetGout")]
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


        // fix autotranslate

        private static bool pushWaitTimer;

        [HarmonyPrefix, HarmonyPriority(Priority.HigherThanNormal), HarmonyPatch(typeof(ADVNovelText), "Update")]
        static bool Update(ADVNovelText __instance) {
            if (!__instance.System.IsInitComplete) {
                return false;
            }

            AdvPage page = __instance.Engine.Page;

            __instance.UpdateColor();
            
            string currentText = __instance.novelText.text;
            __instance.SetActiveNextMarker(page.IsWaitInputInPage);
            
            if (!string.IsNullOrEmpty(currentText)) {

                if (currentText.Contains("<param=player_name>")) { // ??? w hy is this needed ???
                    currentText = currentText.Replace("<param=player_name>", UserDataManager.Instance.UserBasicData.DisplayNickName);
                }

                if (page.CurrentTextLengthMax == page.CurrentTextLength && __instance.novelText.LengthOfView != currentText.Length) {
                    // soft reset all the crap
                    page.RemakeText();
                    page.Status = AdvPage.PageStatus.SendChar;
                    page.waitingTimeInput = 0;
                    page.deltaTimeSendMessage = 0;
                    page.IsWaitingInputCommand = false;
                    page.LastInputSendMessage = false;
                    page.Contoller.Clear();
                    page.Contoller.IsWaitInput = false;
                    pushWaitTimer = true;
                }

                page.CurrentTextLengthMax = currentText.Length;
                __instance.text.maxVisibleCharacters = __instance.novelText.LengthOfView;
                __instance.text.SetText(currentText);

            }

            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(AdvCommandWaitInput), "IsWaitng")]
        static void IsWaitng(ref AdvCommandWaitInput __instance, AdvEngine engine) {
            if (pushWaitTimer) {
                __instance.pauseTime += 1F;
                pushWaitTimer = false;
            }
            if (Plugin.ConfigDisableTextAutoAdvance.Value) {
                __instance.pauseTime += Time.deltaTime;
            }
        }


        // fix buttons
        [HarmonyPostfix, HarmonyPriority(Priority.HigherThanNormal), HarmonyPatch(typeof(ADVSelection), "Init")]
        static void Init(AdvSelection data, Action<ADVSelection> ButtonClickedEvent, ADVSelection __instance) {

            string text;
            if (data.Text.Contains("[プレイヤー]")) {
                text = data.Text.Replace("[プレイヤー]", UserDataManager.Instance.UserBasicData.DisplayNickName);
            } else {
                text = data.Text;
            }

            __instance.text.SetText(text);
            __instance.text.text = text; // this is needed otherwise 25 characters...?
        }

        [HarmonyPrefix, HarmonyPatch(typeof(AdvPage), "UpdateSendChar")]
        static bool UpdateSendChar(AdvPage __instance) {
            if (__instance.CurrentTextLength < 0 || __instance.CurrentTextLength >= __instance.TextData.ParsedText.CharList.Count) {
                //Plugin.Log.LogWarning("Fixed crash? UpdateSendChar text length = " + __instance.CurrentTextLength);
                __instance.EndSendChar();
                return false;
            }
            return true;
        }

        // crashfix?
        [HarmonyPrefix, HarmonyPatch(typeof(ADVAudio), "IsPlaying", new Type[0])]
        static bool IsPlaying(ref ADVAudio __instance, ref bool __result) {
            if (__instance.PlayBack == null || __instance.PlayBack.Data == null) {
                //Plugin.Log.LogWarning("Fixed a crash? ADVAudio.IsPlaying");
                __result = false;
                return false;
            }
            return true;
        }
    }
}
