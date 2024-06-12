using Artdink.Native;
using Artdink.StaticData;
using ExitGames.Client.Photon;
using GssSiteSystem;
using HarmonyLib;
using LINK;
using LINK.Battle;
using LINK.TestMode;
using LINK.UI;
using SwordArtOffline.NetworkEx;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace SwordArtOffline.Patches.Game {

    extern alias AssemblyNotice;
    using static bnAMUpdater;

    public class BasePatchesGame {
        public static bool RetryNetworkFlag { get; internal set; }
        private static CommonUI_MenuDialog LoadingDialog { get; set; }

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
            if (err == SystemErrorCode.NET_DIFF_ROUTER) {
                return false;
            }
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

        /*[HarmonyPostfix, HarmonyPatch(typeof(GameManager), "IsOnline", MethodType.Getter)]
        static void IsOnline() {
            Plugin.Log.LogDebug("Flag = " + ServerDataManager.Instance.CurrentAppVersion);
            Plugin.Log.LogDebug(GameManager.IsJig);
            Plugin.Log.LogDebug(GameManager.IsAllNetAuth); // wat?
            Plugin.Log.LogDebug(GameManager.IsGameServerAlive);
            Plugin.Log.LogDebug(GameManager.IsHasUpdateData);
        }*/

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
            gssProtocolBase._error = error;
            __result = gssProtocolBase;
            return false;
        }

        [HarmonyPostfix, HarmonyPatch(typeof(GameConnect.GssProtocolBase), "connect", typeof(int))]
        static void Connect(ref GameConnect.GssProtocolBase __result, GameConnect.GssProtocolBase __instance, int send_buf_size) {
            if (__result._error != GameConnectError.SUCCESS && Plugin.ConfigNetworkAutoRetry.Value) {
                if (Plugin.ConfigReconnectUseDialogSystem.Value) {
                    RetryNetworkFlag = false;
                    /*Plugin.ShowDialog("A connection error has ocurred.<br>" + __instance._cmd_id + "<br>" + __instance._error, Plugin.ConfigNetworkAutoRetryDelay.Value, () => {
                        RetryNetworkFlag = true;
                    });*/
                    do {
                        Thread.Sleep(1000);
                    } while (!RetryNetworkFlag);
                } else if (Plugin.ConfigNetworkAutoRetryDelay.Value >= 99) {
                    RetryNetworkFlag = false;
                    Plugin.Log.LogMessage("Connection error, retrying when " + Plugin.ConfigButtonRetryNetworkImmediately.Value.MainKey + " is pressed...");
                    do {
                        Thread.Sleep(1000);
                    } while (!RetryNetworkFlag);
                    RetryNetworkFlag = false;
                    Plugin.Log.LogMessage("Key detected, retrying...");
                } else {
                    int wait = Plugin.ConfigNetworkAutoRetryDelay.Value;
                    Plugin.Log.LogMessage("Connection error, retrying in " + wait + " seconds...");
                    do {
                        Thread.Sleep(1000);
                    } while (!RetryNetworkFlag && wait --> 0);
                }
                __result = __instance.connect(send_buf_size);
            }
        }

        [HarmonyPostfix, HarmonyPatch(typeof(MasterDataVersionCheckProtocol), "ReceieveResponse")]
        static void ReceieveResponse(MasterDataVersionCheckProtocol __instance) {
            if (((GameConnectProt.master_data_version_check_R)__instance.Response).update_flag > 0) {
                //LoadingDialog = Plugin.ShowDialog("Local game data is outdated. Updating game data from server...<br>This may take a moment.", null, null);
                Plugin.Log.LogMessage("Updating master data... This may take a few minutes.");
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

        [HarmonyPrefix, HarmonyPatch(typeof(MenuUILocalData), "IsShopUsed", MethodType.Getter)]
        static bool IsShopUsed(ref bool __result) {
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
        }

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
            }
            LoadingDialog?.Hide();
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
    }
}
