using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using GssSiteSystem;
using HarmonyLib;
using LINK;
using LINK.UI;
using Mono.Cecil.Cil;
using SwordArtOffline.Patches.Game;
using SwordArtOffline.Patches.Notice;
using SwordArtOffline.Patches.Shared;
using SwordArtOffline.Patches.Testmode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal.Input;

namespace SwordArtOffline
{

    extern alias AssemblyNotice;

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin {

        private const String SEC_BUTTONS = "Keybindings";

        public static ConfigEntry<KeyboardShortcut> ConfigButton1;
        public static ConfigEntry<KeyboardShortcut> ConfigButton2;
        public static ConfigEntry<KeyboardShortcut> ConfigButton3;
        public static ConfigEntry<KeyboardShortcut> ConfigButton4;
        public static ConfigEntry<KeyboardShortcut> ConfigButton5;
        public static ConfigEntry<KeyboardShortcut> ConfigButton6;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonCoin;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonService;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonTest;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonTestEnter;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonTestUp;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonTestDown;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonAnalogLeft;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonAnalogUp;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonAnalogRight;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonAnalogDown;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonScanCard;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonClearError;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonRetryNetworkImmediately;
        public static ConfigEntry<KeyboardShortcut> ConfigTouchTouch;
        public static ConfigEntry<KeyboardShortcut> ConfigTouchSkill1;
        public static ConfigEntry<KeyboardShortcut> ConfigTouchSkill2;
        public static ConfigEntry<KeyboardShortcut> ConfigTouchSkill3;
        public static ConfigEntry<KeyboardShortcut> ConfigTouchSkill4;
        public static ConfigEntry<KeyboardShortcut> ConfigTouchSkill5;
        public static ConfigEntry<KeyboardShortcut> ConfigButtonAttackAll;
        public static ConfigEntry<String> CardID;
        public static ConfigEntry<bool> ConfigNetworkEncryption;
        public static ConfigEntry<bool> ConfigPhotonLogging;
        public static ConfigEntry<String> ConfigNetworkMatchingIP;
        public static ConfigEntry<bool> ConfigMoveToFrontOnStart;
        public static ConfigEntry<bool> ConfigShowCursor;
        public static ConfigEntry<bool> ConfigNetworkAutoRetry;
        public static ConfigEntry<int> ConfigNetworkAutoRetryDelay;
        public static ConfigEntry<bool> ConfigUIShowComboMeter;
        public static ConfigEntry<MinimapUILayout> ConfigUIShowMinimap;
        public static ConfigEntry<bool> ConfigUIShowComments;
        public static ConfigEntry<bool> ConfigUIShowDirectionalArrows;
        public static ConfigEntry<bool> ConfigUIMURDERYUI;
        public static ConfigEntry<bool> ConfigControlAnalogStick;
        public static ConfigEntry<MouseButtonControl> ConfigControlMouse;
        public static ConfigEntry<int> ConfigMouseRadiusWidth;
        public static ConfigEntry<bool> ConfigTimeFreeze;
        public static ConfigEntry<bool> ConfigTimeExtend;
        public static ConfigEntry<bool> ConfigRepeatMenuSections;
        public static ConfigEntry<AutoContinueMode> ConfigAutoContinueMode;
        public static ConfigEntry<bool> ConfigAllowTerminalSwitch;
        public static ConfigEntry<bool> ConfigReconnectUseDialogSystem;
        public static ConfigEntry<bool> ConfigUIBonusStartsExpanded;

        public enum MouseButtonControl {
            Off, LeftMouseButton, RightMouseButton
        }

        public enum AutoContinueMode {
            Off, ContinueIfFree, ContinueIfTicket, AlwaysContinue, NeverContinue
        }

        public enum MinimapUILayout {
            Normal, HideMinimap, HideMinimapAndMoveBonus
        }

        public static String KeychipId;
        public static int Coins;
        public static bool WantsBootSatellite = true;
        public static bool TestSwitchIsToggle;
        public static bool TestSwitchState;
        public static ManualLogSource Log;
        private bool hasMovedFront;
        private bool runGameUpdate;
        private static Plugin instance;

        public void Awake() {

            instance = this;

            Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");

            Log = Logger;

            #region Keybinding init
            ConfigButton1 = Config.Bind(SEC_BUTTONS, "Button 1 (Attack or Switch Ch. 1)", new KeyboardShortcut(UnityEngine.KeyCode.A), "This button will attack with the first character, or switch to them if they are not in use.");
            ConfigButton2 = Config.Bind(SEC_BUTTONS, "Button 2 (Attack or Switch Ch. 2)", new KeyboardShortcut(UnityEngine.KeyCode.S), "This button will attack with the second character, or switch to them if they are not in use.");
            ConfigButton3 = Config.Bind(SEC_BUTTONS, "Button 3 (Attack or Switch Ch. 3)", new KeyboardShortcut(UnityEngine.KeyCode.D), "This button will attack with the third character, or switch to them if they are not in use.");
            ConfigButton4 = Config.Bind(SEC_BUTTONS, "Button 4 (Dodge)", new KeyboardShortcut(UnityEngine.KeyCode.F), "Keyboard/Controller binding for button 4");
            ConfigButton5 = Config.Bind(SEC_BUTTONS, "Button 5 (Block)", new KeyboardShortcut(UnityEngine.KeyCode.G), "Keyboard/Controller binding for button 5");
            ConfigButton6 = Config.Bind(SEC_BUTTONS, "Button 6 (Unknown)", new KeyboardShortcut(UnityEngine.KeyCode.H), "Keyboard/Controller binding for button 6");
            ConfigButtonAttackAll = Config.Bind(SEC_BUTTONS, "Attack Ch. All", new KeyboardShortcut(UnityEngine.KeyCode.Q), "This button will attack with any character that is currently in use and not switch.");
            ConfigButtonService = Config.Bind(SEC_BUTTONS, "Service", new KeyboardShortcut(UnityEngine.KeyCode.F5), "Keyboard/Controller binding for service");
            ConfigButtonTest = Config.Bind(SEC_BUTTONS, "Test", new KeyboardShortcut(UnityEngine.KeyCode.F6), "Keyboard/Controller binding for test");
            ConfigButtonCoin = Config.Bind(SEC_BUTTONS, "Coin", new KeyboardShortcut(UnityEngine.KeyCode.F7), "Keyboard/Controller binding for coin");
            ConfigButtonTestEnter = Config.Bind(SEC_BUTTONS, "Test Menu Enter", new KeyboardShortcut(UnityEngine.KeyCode.KeypadEnter), "Keyboard/Controller binding for test menu enter");
            ConfigButtonTestUp = Config.Bind(SEC_BUTTONS, "Test Menu Up", new KeyboardShortcut(UnityEngine.KeyCode.Keypad8), "Keyboard/Controller binding for test menu up");
            ConfigButtonTestDown = Config.Bind(SEC_BUTTONS, "Test Menu Down", new KeyboardShortcut(UnityEngine.KeyCode.Keypad2), "Keyboard/Controller binding for test menu down");
            ConfigTouchTouch = Config.Bind(SEC_BUTTONS, "Touch: Hold Hands / Activate Mission", new KeyboardShortcut(UnityEngine.KeyCode.Space), "Keyboard/Controller binding for touching your main character or activating missions");
            ConfigTouchSkill1 = Config.Bind(SEC_BUTTONS, "Touch: Skill 1", new KeyboardShortcut(UnityEngine.KeyCode.Alpha1), "Keyboard/Controller binding for activating skill 1");
            ConfigTouchSkill2 = Config.Bind(SEC_BUTTONS, "Touch: Skill 2", new KeyboardShortcut(UnityEngine.KeyCode.Alpha2), "Keyboard/Controller binding for activating skill 2");
            ConfigTouchSkill3 = Config.Bind(SEC_BUTTONS, "Touch: Skill 3", new KeyboardShortcut(UnityEngine.KeyCode.Alpha3), "Keyboard/Controller binding for activating skill 3");
            ConfigTouchSkill4 = Config.Bind(SEC_BUTTONS, "Touch: Skill 4", new KeyboardShortcut(UnityEngine.KeyCode.Alpha4), "Keyboard/Controller binding for activating skill 4");
            ConfigTouchSkill5 = Config.Bind(SEC_BUTTONS, "Touch: Skill 5", new KeyboardShortcut(UnityEngine.KeyCode.Alpha5), "Keyboard/Controller binding for activating skill 5");
            ConfigButtonClearError = Config.Bind(SEC_BUTTONS, "Clear Error", new KeyboardShortcut(UnityEngine.KeyCode.F11), new ConfigDescription("For development only", null, new ConfigurationManagerAttributes() { IsAdvanced = true }));

            ConfigButtonAnalogUp = Config.Bind(SEC_BUTTONS, "Analog: Up", new KeyboardShortcut(UnityEngine.KeyCode.UpArrow), "Keyboard/Controller binding for analog up");
            ConfigButtonAnalogRight = Config.Bind(SEC_BUTTONS, "Analog: Right", new KeyboardShortcut(UnityEngine.KeyCode.RightArrow), "Keyboard/Controller binding for analog right");
            ConfigButtonAnalogDown = Config.Bind(SEC_BUTTONS, "Analog: Down", new KeyboardShortcut(UnityEngine.KeyCode.DownArrow), "Keyboard/Controller binding for analog down");
            ConfigButtonAnalogLeft = Config.Bind(SEC_BUTTONS, "Analog: Left", new KeyboardShortcut(UnityEngine.KeyCode.LeftArrow), "Keyboard/Controller binding for analog left");
            ConfigButtonScanCard = Config.Bind(SEC_BUTTONS, "Scan Card", new KeyboardShortcut(UnityEngine.KeyCode.Backspace), "Keyboard/Controller binding for scanning Aime card");

            ConfigControlAnalogStick = Config.Bind(SEC_BUTTONS, "Use Control Stick Movement", false, "Use a control stick of a connected controller for movement rather than the analog bindings.");
            ConfigControlMouse = Config.Bind(SEC_BUTTONS, "Use Mouse Movement", MouseButtonControl.Off, "Use the mouse for movement.");
            ConfigMouseRadiusWidth = Config.Bind(SEC_BUTTONS, "Use Mouse Movement: Control Radius", 300, new ConfigDescription("The size in pixels of the virtual control stick created by the mouse. If your mouse is that amount of pixels away from the screen center, maximum stick tilt is achieved.", new AcceptableValueRange<int>(1, 1000), new ConfigurationManagerAttributes() { IsAdvanced = true }));
            #endregion

            CardID = Config.Bind("Card", "Access Code", "0000000000000000", "Aime access code of your card");
            ConfigNetworkEncryption = Config.Bind("Network", "Encryption", false, "Enable network encryption again if your server requires it");
            ConfigNetworkMatchingIP = Config.Bind("Network", "Matching Server IP", "", "Set the matching server IP");
            ConfigPhotonLogging = Config.Bind("Network", "Matching Server Logging", false, new ConfigDescription("Photon Server Logging", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigMoveToFrontOnStart = Config.Bind("General", "Move to front on start", true, "Moves the game window in the foreground on startup");
            ConfigShowCursor = Config.Bind("General", "Show Mouse Cursor", true, "Shows the mouse cursor. Disable if using touchscreen.");


            ConfigButtonRetryNetworkImmediately = Config.Bind(SEC_BUTTONS, "Immediate Network Retry", new KeyboardShortcut(UnityEngine.KeyCode.Keypad0), new ConfigDescription("If Auto-Retry Delay is 0, use this key to retry network connection", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigNetworkAutoRetry = Config.Bind("Network", "Auto-Retry", true, new ConfigDescription("Automatically re-attempt network connections instead of throwing an error and stopping game operation.", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigNetworkAutoRetryDelay = Config.Bind("Network", "Auto-Retry Delay", 15, new ConfigDescription("Time (in seconds) until a failed network connection is reattempted. Set to 99 for manual reconnection.", new AcceptableValueRange<int>(5, 99), new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigReconnectUseDialogSystem = Config.Bind("Network", "Use Ingame Dialogs for Reconnection", false, new ConfigDescription("(broken do not use) Uses ingame dialogs to display connection errors, otherwise uses BepInEx.MessageCenter.", null, new ConfigurationManagerAttributes { IsAdvanced = true }));

            ConfigUIShowComboMeter = Config.Bind("User Interface", "Show Combo Meter", true, "If disabled, the combo meter (that covers the minimap partially...) is hidden.");
            ConfigUIShowMinimap = Config.Bind("User Interface", "Show Minimap", MinimapUILayout.Normal, "If disabled, the minimap is hidden and you need to find your own way. You can also move the bonus list to this position to save screen space.");
            ConfigUIShowComments = Config.Bind("User Interface", "Show Commentary", true, "If disabled, the character commentary on the left side during gameplay is hidden.");
            ConfigUIShowDirectionalArrows = Config.Bind("User Interface", "Show Directional Arrows", true, "If disabled, the giant red directional arrows (look, the GIANT COLLECTIBLE THAT YOU ABSOLUTELY DO NOT WANT TO MISS IS RIGHT HERE, DIRECTLY ON THE SCREEN) are hidden.");
            ConfigUIMURDERYUI = Config.Bind("User Interface", "Delete Yui", false, "Deleted.\n\n(Removes all Yui sprites and voice lines from gameplay)");
            ConfigUIBonusStartsExpanded = Config.Bind("User Interface", "EXBonus Always Shown", false, "If enabled, the EX bonus list will be automatically shown on quest start.");

            ConfigTimeFreeze = Config.Bind("General", "Timer Freeze", false, new ConfigDescription("Freezes menu timers.", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigTimeFreeze = Config.Bind("General", "Timer Freeze", false, new ConfigDescription("Freezes menu timers.", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigRepeatMenuSections = Config.Bind("General", "Repeated Menu Access", false, "Allows you to repeatedly enter sections in the menu menu, such as the shop or the customization menu. Also disables confirmation dialogs.");
            ConfigTimeExtend = Config.Bind("General", "Timer Extension", false, "Increases main menu time limits.");
            ConfigAutoContinueMode = Config.Bind("General", "Auto-Continue", AutoContinueMode.ContinueIfFree, "Automatically continues if you die.");
            ConfigAllowTerminalSwitch = Config.Bind("General", "Terminal/Station Switcher", true, "If selecting NO on the LOG OUT prompt, gives an option to switch between terminal/station.");


            string exe = Environment.CurrentDirectory.Split('\\').Last();
            Logger.LogDebug("Game = " + exe);

            if (exe != "testmode") {
                LoadKeychipFromIni();
            }

            if (exe == "game") {
                TestSwitchIsToggle = true;
                Harmony.CreateAndPatchAll(typeof(BasePatchesGame), "eu.haruka.gmg.sao.fixes.game.base");
                Harmony.CreateAndPatchAll(typeof(KeychipPatchesGame), "eu.haruka.gmg.sao.fixes.game.keychip");
                Harmony.CreateAndPatchAll(typeof(CardPatchesGame), "eu.haruka.gmg.sao.fixes.game.bngrw");
                Harmony.CreateAndPatchAll(typeof(BNAMPFPatchesGame), "eu.haruka.gmg.sao.fixes.game.oomph");
                Harmony.CreateAndPatchAll(typeof(TerminalDecicderPatch), "eu.haruka.gmg.sao.fixes.game.terminal");
                Harmony.CreateAndPatchAll(typeof(MatchingPatchesGame), "eu.haruka.gmg.sao.fixes.game.matching");
                Harmony.CreateAndPatchAll(typeof(UIDeclutterPatchesGame), "eu.haruka.gmg.sao.fixes.game.uideclutter");
                Harmony.CreateAndPatchAll(typeof(DataDestructionOverwriteSystem), "eu.haruka.gmg.sao.fixes.game.destroy");
                BasePatchesGame.Initialize();
                runGameUpdate = true;
            }
            if (exe == "notice") {
                TestSwitchIsToggle = true;
                Harmony.CreateAndPatchAll(typeof(TerminalDecicderPatch), "eu.haruka.gmg.sao.fixes.notice.terminal");
                Harmony.CreateAndPatchAll(typeof(KeychipPatchesNotice), "eu.haruka.gmg.sao.fixes.notice.keychip");
                Harmony.CreateAndPatchAll(typeof(CardPatchesNotice), "eu.haruka.gmg.sao.fixes.notice.bngrw");
                Harmony.CreateAndPatchAll(typeof(BNAMPFPatchesNotice), "eu.haruka.gmg.sao.fixes.notice.oomph");
            }
            if (exe == "testmode") {
                Harmony.CreateAndPatchAll(typeof(KeychipPatchesTestmode), "eu.haruka.gmg.sao.fixes.test.keychip");
                TestSwitchIsToggle = true;
                TestSwitchState = true;
            }
            Harmony.CreateAndPatchAll(typeof(IOPatches), "eu.haruka.gmg.sao.fixes.io");
            Harmony.CreateAndPatchAll(typeof(UnityPatches), "eu.haruka.gmg.sao.fixes.unity");

            Debug.s_Logger.logEnabled = true;
            Debug.unityLogger.logEnabled = true;

            WantsBootSatellite = !Environment.GetCommandLineArgs().Contains("-mod-swordartoffline-force-terminal");
            Logger.LogInfo("Booting Satellite? " + WantsBootSatellite);

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

            Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1) {
            Logger.LogDebug(arg0.name + " -> " + arg1.name);
            if (arg1.name == "SubScene_AttractPv_Parent") {
                Logger.LogMessage("SwordArtOffline " + PluginInfo.PLUGIN_VERSION + " mod for SAO Arcade: Deep Explorer - 2024 Haruka");
                Logger.LogMessage("part of the GenericMusicGames preservation project");
                Logger.LogMessage("Press F1 to open the mod settings menu.");
                Logger.LogMessage("Press " + ConfigButtonScanCard.Value.MainKey + " to scan your Banapassport.");
                Logger.LogMessage("Press " + ConfigButtonCoin.Value.MainKey + " to insert a coin.");
            } else if (arg1.name == "Quest") {
                IOPatches.ResetCharacterPosition();
            }
        }

        private void LoadKeychipFromIni() {
            INIParser ini = new INIParser();
            ini.Open("../AMCUS/WritableConfig.ini");
            KeychipId = ini.ReadValue("RuntimeConfig", "serialID", "282513041234");
            Log.LogDebug("Keychip ID from INI: " + KeychipId);
        }

        public void Update() {
            if (ConfigButtonCoin.Value.IsDown()) {
                Coins++;
            }
            if (ConfigShowCursor.Value && !Cursor.visible) {
                Cursor.visible = true;
            } else if (!ConfigShowCursor.Value && Cursor.visible) {
                Cursor.visible = false;
            }
            if (TestSwitchIsToggle && ConfigButtonTest.Value.IsDown()) {
                TestSwitchState = !TestSwitchState;
                Log.LogDebug("Test switch now " + TestSwitchState);
            }
            if (!hasMovedFront && ConfigMoveToFrontOnStart.Value) {
                IntPtr hWnd = Win32MethodsMod.FindWindow(null, Application.productName);
                if (hWnd != IntPtr.Zero) {
                    Logger.LogDebug("Moving hWnd to front: " + hWnd.ToInt64());
                    Win32MethodsMod.SetTopmostWindow(hWnd, true);
                    Win32MethodsMod.SetForegroundWindowInternal(hWnd);
                    Win32MethodsMod.SetTopmostWindow(hWnd, false);
                    hasMovedFront = true;
                }
            }
            if (ConfigButtonRetryNetworkImmediately.Value.IsDown()) {
                BasePatchesGame.RetryNetworkFlag = true;
            }
            if (UnityPatches.FakeFrames > 0) {
                UnityPatches.FakeFrames--;
            }
            if (ConfigTouchTouch.Value.IsDown()) {
                SimulateTouch(Screen.width / 2, Screen.height / 2);
            }
            if (ConfigTouchSkill1.Value.IsDown()) {
                SimulateTouch(1300, 150);
            }
            if (ConfigTouchSkill2.Value.IsDown()) {
                SimulateTouch(1425, 150);
            }
            if (ConfigTouchSkill3.Value.IsDown()) {
                SimulateTouch(1545, 150);
            }
            if (ConfigTouchSkill4.Value.IsDown()) {
                SimulateTouch(1663, 150);
            }
            if (ConfigTouchSkill5.Value.IsDown()) {
                SimulateTouch(1785, 150);
            }
            if (runGameUpdate) {
                UpdateGame();
            }
        }

        private void UpdateGame() { // this needs to be seperate otherwise there will be an assembly load exception in the testmenu
            if (ConfigButtonClearError.Value.IsDown()) {
                ErrorUIManager.Instance.isPlaySoundSE = false;
                ErrorUIManager.Instance.Hide();
            }
        }

        private void SimulateTouch(int x, int y) {
            UnityPatches.FakeTouchX = x;
            UnityPatches.FakeTouchY = y;
            UnityPatches.FakeFrames = 2; // frame 1: touch down event, frame 2: touch up event
        }

        /*internal static CommonUI_MenuDialog ShowDialog(string message, int? timer, Action onClose) {
            Log.LogDebug("ShowDialog: " + message);
            CommonUI_MenuDialog menuDialog = new CommonUI_MenuDialog();
            instance.StartCoroutine(ShowDialogCo(message, timer, onClose, menuDialog));
            return menuDialog;
        }

        private static IEnumerator<object> ShowDialogCo(string message, int? timer, Action onClose, CommonUI_MenuDialog menuDialog) {
            Log.LogDebug("ShowDialog: Instantiating");
            while (menuDialog.IsInstantiating()) {
                yield return null;
            }
            if (!SceneManager.GetSceneByName("SubScene_UI_MenuDialog").isLoaded) {
                Log.LogDebug("ShowDialog: Load Scene");
                AssetManager.Scene.Load("SubScene_UI_MenuDialog", true, true);
                while (!SceneManager.GetSceneByName("SubScene_UI_MenuDialog").isLoaded) {
                    yield return null;
                }
                Log.LogDebug("ShowDialog: Loaded");
            }
            MenuUISubSceneCtrl menuUISubSceneCtrl = MenuUIManager.Instance.GetMenuUISubSceneCtrl("SubScene_UI_MenuDialog");
            createMenuUI(menuUISubSceneCtrl, ref menuDialog, null);
            menuDialog.DialogDisplay("DIALOG_COMMON_ERROR_CODE", delegate { onClose?.Invoke(); }, delegate { onClose?.Invoke(); }, false, timer ?? 0, timer != null ? MenuUITime.TimeType.Generic : MenuUITime.TimeType.NULL, new object[] { message }, false);
            while (menuDialog.IsInstantiating()) {
                yield return null;
            }
            menuDialog.SetDisplay(true);
            yield break;
        }

        private static void createMenuUI<T>(MenuUISubSceneCtrl childScene, ref T createUI, Action init = null) where T : CommonUI_baseControl {
            if (childScene != null) {
                createUI = childScene.GetMenuParts<T>();
                if (createUI != null) {
                    createUI.InitGameobject();
                    init?.Invoke();
                } else {
                    Log.LogError("createMenuUI: createUI is null");
                }
            } else {
                Log.LogError("createMenuUI: childScene is null");
            }
        }*/
    }
}
