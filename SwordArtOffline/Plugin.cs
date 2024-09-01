using AssetBundles.Manager;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Haruka.Arcade.SEGA835Lib.Devices;
using Haruka.Arcade.SEGA835Lib.Devices.Card;
using Haruka.Arcade.SEGA835Lib.Devices.Card._837_15396;
using Haruka.Arcade.SEGA835Lib.Devices.IO;
using Haruka.Arcade.SEGA835Lib.Devices.IO._835_15257_01;
using Haruka.Arcade.SEGA835Lib.Devices.LED._837_15093;
using LINK;
using LINK.UI;
using SwordArtOffline.Patches;
using SwordArtOffline.Patches.Game;
using SwordArtOffline.Patches.Notice;
using SwordArtOffline.Patches.Shared;
using SwordArtOffline.Patches.Testmode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SwordArtOffline {

    extern alias AssemblyNotice;

    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("eu.haruka.gmg.apm.emoneyuilink", BepInDependency.DependencyFlags.SoftDependency)]
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
        public static ConfigEntry<string> ConfigPrinterDirectory;
        public static ConfigEntry<string> ConfigPrinterHandler;
        public static ConfigEntry<string> ConfigPrinterHandlerArguments;
        public static ConfigEntry<bool> ConfigPrinterCleanOnStart;
        public static ConfigEntry<bool> ConfigPrinterUseHandler;
        public static ConfigEntry<bool> ConfigPrinterHolo;
        public static ConfigEntry<string> ConfigForceCamera;
        public static ConfigEntry<int> ConfigAimeReaderPort;
        public static ConfigEntry<bool> ConfigShowMenuKeybinds;
        public static ConfigEntry<bool> ConfigUseIO4Stick;
        public static ConfigEntry<int> ConfigIO4AxisX;
        public static ConfigEntry<bool> ConfigIO4AxisXInvert;
        public static ConfigEntry<int> ConfigIO4AxisY;
        public static ConfigEntry<bool> ConfigIO4AxisYInvert;
        public static ConfigEntry<int> ConfigAttackButtonLed;
        public static ConfigEntry<int> ConfigAttackButtonLed2;
        public static ConfigEntry<int> ConfigAttackButtonLed3;
        public static ConfigEntry<int> ConfigIO4StickDeadzone;
        public static ConfigEntry<bool> ConfigHardTranslations;
        public static ConfigEntry<int> ConfigIO4AxisXMin;
        public static ConfigEntry<int> ConfigIO4AxisXMax;
        public static ConfigEntry<int> ConfigIO4AxisYMin;
        public static ConfigEntry<int> ConfigIO4AxisYMax;
        public static ConfigEntry<bool> ConfigDisableTextAutoAdvance;
        public static ConfigEntry<int> Config15093Port;
        public static ConfigEntry<int> ConfigLEDBoardAddr;
        public static ConfigEntry<int> ConfigLEDHostAddr;
        public static ConfigEntry<bool> ConfigLogSegaLibMessages;

        private static String keychip;
        private static IntPtr keychipA = IntPtr.Zero;
        private static IntPtr keychipU = IntPtr.Zero;

        public static AimeCardReader_837_15396 Aime;
        public static bool AimeConnected;
        public static IO4USB_835_15257_01 Io4;
        public static LED_837_15093_06 Led;
        public static bool IoConnected;
        public static JVSUSBReportIn? IoReport;
        public static bool LedConnected;

        public enum MouseButtonControl {
            Off, LeftMouseButton, RightMouseButton
        }

        public enum AutoContinueMode {
            Off, ContinueIfFree, ContinueIfTicket, AlwaysContinue, NeverContinue
        }

        public enum MinimapUILayout {
            Normal, HideMinimap, HideMinimapAndMoveBonus
        }

        public static int Coins;
        public static int Service;
        public static bool WantsBootSatellite = true;
        public static bool TestSwitchIsToggle;
        public static bool TestSwitchState;
        public static ManualLogSource Log;
        internal static byte[] ledStatus = new byte[255];
        private static Plugin instance;
        private bool hasMovedFront;
        private bool runGameUpdate;

        public void Awake() {

            instance = this;

            Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loading...");

            Log = Logger;

            #region Keybinding init
            ConfigButton1 = Config.Bind(SEC_BUTTONS, "Button 1 (Attack or Switch Ch. 1)", new KeyboardShortcut(KeyCode.A), "This button will attack with the first character, or switch to them if they are not in use.");
            ConfigButton2 = Config.Bind(SEC_BUTTONS, "Button 2 (Attack or Switch Ch. 2)", new KeyboardShortcut(KeyCode.S), "This button will attack with the second character, or switch to them if they are not in use.");
            ConfigButton3 = Config.Bind(SEC_BUTTONS, "Button 3 (Attack or Switch Ch. 3)", new KeyboardShortcut(KeyCode.D), "This button will attack with the third character, or switch to them if they are not in use.");
            ConfigButton4 = Config.Bind(SEC_BUTTONS, "Button 4 (Dodge)", new KeyboardShortcut(KeyCode.F), "Keyboard/Controller binding for button 4");
            ConfigButton5 = Config.Bind(SEC_BUTTONS, "Button 5 (Block)", new KeyboardShortcut(KeyCode.G), "Keyboard/Controller binding for button 5");
            ConfigButton6 = Config.Bind(SEC_BUTTONS, "Button 6 (Unknown)", new KeyboardShortcut(KeyCode.H), "Keyboard/Controller binding for button 6");
            ConfigButtonAttackAll = Config.Bind(SEC_BUTTONS, "Attack Ch. All", new KeyboardShortcut(KeyCode.Q), "This button will attack with any character that is currently in use and not switch.");
            ConfigButtonService = Config.Bind(SEC_BUTTONS, "Service", new KeyboardShortcut(KeyCode.F5), "Keyboard/Controller binding for service");
            ConfigButtonTest = Config.Bind(SEC_BUTTONS, "Test", new KeyboardShortcut(KeyCode.F6), "Keyboard/Controller binding for test");
            ConfigButtonCoin = Config.Bind(SEC_BUTTONS, "Coin", new KeyboardShortcut(KeyCode.F7), "Keyboard/Controller binding for coin");
            ConfigButtonTestEnter = Config.Bind(SEC_BUTTONS, "Test Menu Enter", new KeyboardShortcut(KeyCode.KeypadEnter), "Keyboard/Controller binding for test menu enter");
            ConfigButtonTestUp = Config.Bind(SEC_BUTTONS, "Test Menu Up", new KeyboardShortcut(KeyCode.Keypad8), "Keyboard/Controller binding for test menu up");
            ConfigButtonTestDown = Config.Bind(SEC_BUTTONS, "Test Menu Down", new KeyboardShortcut(KeyCode.Keypad2), "Keyboard/Controller binding for test menu down");
            ConfigTouchTouch = Config.Bind(SEC_BUTTONS, "Touch: Hold Hands / Activate Mission", new KeyboardShortcut(KeyCode.Space), "Keyboard/Controller binding for touching your main character or activating missions");
            ConfigTouchSkill1 = Config.Bind(SEC_BUTTONS, "Touch: Skill 1", new KeyboardShortcut(KeyCode.Alpha1), "Keyboard/Controller binding for activating skill 1");
            ConfigTouchSkill2 = Config.Bind(SEC_BUTTONS, "Touch: Skill 2", new KeyboardShortcut(KeyCode.Alpha2), "Keyboard/Controller binding for activating skill 2");
            ConfigTouchSkill3 = Config.Bind(SEC_BUTTONS, "Touch: Skill 3", new KeyboardShortcut(KeyCode.Alpha3), "Keyboard/Controller binding for activating skill 3");
            ConfigTouchSkill4 = Config.Bind(SEC_BUTTONS, "Touch: Skill 4", new KeyboardShortcut(KeyCode.Alpha4), "Keyboard/Controller binding for activating skill 4");
            ConfigTouchSkill5 = Config.Bind(SEC_BUTTONS, "Touch: Skill 5", new KeyboardShortcut(KeyCode.Alpha5), "Keyboard/Controller binding for activating skill 5");
            ConfigButtonClearError = Config.Bind(SEC_BUTTONS, "Clear Error", new KeyboardShortcut(KeyCode.F11), new ConfigDescription("For development only, expect things to go wrong when pressing this.", null, new ConfigurationManagerAttributes() { IsAdvanced = true }));

            ConfigButtonAnalogUp = Config.Bind(SEC_BUTTONS, "Analog: Up", new KeyboardShortcut(KeyCode.UpArrow), "Keyboard/Controller binding for analog up");
            ConfigButtonAnalogRight = Config.Bind(SEC_BUTTONS, "Analog: Right", new KeyboardShortcut(KeyCode.RightArrow), "Keyboard/Controller binding for analog right");
            ConfigButtonAnalogDown = Config.Bind(SEC_BUTTONS, "Analog: Down", new KeyboardShortcut(KeyCode.DownArrow), "Keyboard/Controller binding for analog down");
            ConfigButtonAnalogLeft = Config.Bind(SEC_BUTTONS, "Analog: Left", new KeyboardShortcut(KeyCode.LeftArrow), "Keyboard/Controller binding for analog left");
            ConfigButtonScanCard = Config.Bind(SEC_BUTTONS, "Scan Card", new KeyboardShortcut(KeyCode.Backspace), "Keyboard/Controller binding for scanning Aime card");

            ConfigControlAnalogStick = Config.Bind(SEC_BUTTONS, "Use Control Stick Movement", false, "Use a control stick of a connected controller for movement rather than the analog bindings.");
            ConfigControlMouse = Config.Bind(SEC_BUTTONS, "Use Mouse Movement", MouseButtonControl.Off, "Use the mouse for movement. The center point is the middle of the screen.");
            ConfigMouseRadiusWidth = Config.Bind(SEC_BUTTONS, "Use Mouse Movement: Control Radius", 300, new ConfigDescription("The size in pixels of the virtual control stick created by the mouse. If your mouse is that amount of pixels away from the screen center, maximum stick tilt is achieved.", new AcceptableValueRange<int>(1, 1000), new ConfigurationManagerAttributes() { IsAdvanced = true }));
            #endregion

            CardID = Config.Bind("Card", "Access Code", "0000000000000000", "Aime access code of your card");
            ConfigNetworkEncryption = Config.Bind("Network", "Encryption", false, "Enable network encryption again if your server requires it");
            ConfigNetworkMatchingIP = Config.Bind("Network", "Matching Server IP", "", "Set the matching server IP. If the server supports exdata, this setting is not needed.");
            ConfigPhotonLogging = Config.Bind("Network", "Matching Server Logging", false, new ConfigDescription("Photon Server packet logging, only useful for debugging issues with the photon emulator", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigMoveToFrontOnStart = Config.Bind("General", "Move to front on start", true, "Moves the game window to the foreground on startup");
            ConfigShowCursor = Config.Bind("General", "Show Mouse Cursor", true, "Shows the mouse cursor. Disable if using touchscreen.");
            ConfigShowMenuKeybinds = Config.Bind("General", "Show Keybindings on Startup", true, "Shows the most important keybindings when the game starts up (if MessageCenter is active)");
            ConfigHardTranslations = Config.Bind("General", "Hard Translations", true, "Translates some hard-coded strings to English");
            ConfigDisableTextAutoAdvance = Config.Bind("General", "Disable Text Auto-Advance", false, "During cutscenes, disable text auto-advancing");

            ConfigButtonRetryNetworkImmediately = Config.Bind(SEC_BUTTONS, "Immediate Network Retry", new KeyboardShortcut(KeyCode.Keypad0), new ConfigDescription("If Auto-Retry Delay is 0, use this key to retry network connection", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigNetworkAutoRetry = Config.Bind("Network", "Enable Network Retry", true, new ConfigDescription("Retry failed network connections instead of throwing an error and stopping game operation.", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigNetworkAutoRetryDelay = Config.Bind("Network", "Auto-Retry Delay", 59, new ConfigDescription("Time (in seconds) until a failed network connection is reattempted. Set to 99 for manual reconnection.", new AcceptableValueRange<int>(5, 99), new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigReconnectUseDialogSystem = Config.Bind("Network", "Use Ingame Dialogs", false, "Uses ingame dialogs to display connection errors, otherwise uses BepInEx.MessageCenter.");

            ConfigUIShowComboMeter = Config.Bind("User Interface", "Show Combo Meter", true, "If disabled, the combo meter (that covers the minimap partially...) is hidden.");
            ConfigUIShowMinimap = Config.Bind("User Interface", "UI Layout / Minimap", MinimapUILayout.Normal, "If disabled, the minimap is hidden and you need to find your own way. You can also move the bonus list to this position to save screen space.");
            ConfigUIShowComments = Config.Bind("User Interface", "Show Commentary", true, "If disabled, the character commentary on the left side during gameplay is hidden.");
            ConfigUIShowDirectionalArrows = Config.Bind("User Interface", "Show Directional Arrows", true, "If disabled, the giant red directional arrows (look, the GIANT COLLECTIBLE THAT YOU ABSOLUTELY DO NOT WANT TO MISS IS RIGHT HERE, DIRECTLY ON THE SCREEN) are hidden.");
            ConfigUIMURDERYUI = Config.Bind("User Interface", "Delete Yui", false, "Did you know who ruined the anime before Oberon? Yui.\n\n(Removes all Yui sprites and voice lines from everywhere, excluding the medals)");
            ConfigUIBonusStartsExpanded = Config.Bind("User Interface", "EXBonus Always Shown", false, "If enabled, the EX bonus list will be automatically expanded on quest start.");

            ConfigTimeFreeze = Config.Bind("General", "Timer Freeze", false, new ConfigDescription("Freezes menu timers.", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigRepeatMenuSections = Config.Bind("General", "Repeated Menu Access", true, "Allows you to repeatedly enter sections in the menu menu without playing a mission inbetween, such as the shop or the customization menu. Also disables confirmation dialogs.");
            ConfigTimeExtend = Config.Bind("General", "Timer Extension", false, "Increases main menu time limits.");
            ConfigAutoContinueMode = Config.Bind("General", "Auto-Continue", AutoContinueMode.Off, "Automatically continues if you die.");
            ConfigForceCamera = Config.Bind("General", "Force Camera", "", "Forces a specific camera name to be used. This can be used to ignore virtual cameras. Leave blank to disable.");

            ConfigAllowTerminalSwitch = Config.Bind("Terminal", "Terminal/Station Switcher", true, "If selecting NO on the LOG OUT prompt, gives an option to switch between terminal/station.");
            ConfigPrinterDirectory = Config.Bind("Terminal", "Printer Directory", "../nvram/print", "The directory where printed cards are written to");
            ConfigPrinterUseHandler = Config.Bind("Terminal", "Printer Handler", false, "Uses the specified printer handler to print.");
            ConfigPrinterHandler = Config.Bind("Terminal", "Printer Handler: Executable", "../ZebraSpooler.exe", "The name of an executable that is executed after printing");
            ConfigPrinterHandlerArguments = Config.Bind("Terminal", "Printer Handler: Executable Arguments", "nvram/print", "The arguments passed to the printer handler executable. %IMAGE_PATH% and %HOLO_PATH% can be used as placeholders for the printed files.");
            ConfigPrinterCleanOnStart = Config.Bind("Terminal", "Clean Printer Directory On Start", false, "If enabled, the contents of the printer directory will be emptied on start.");
            ConfigPrinterHolo = Config.Bind("Terminal", "Print Holo Layer", true, "If enabled, the holo layer will also be saved as a seperate file.");

            ConfigLogSegaLibMessages = Config.Bind("Real Hardware", "Activate Logging", false, "Enables Sega835Lib log messages. If LEDs are enabled, this can become quite busy.");
            ConfigAimeReaderPort = Config.Bind("Real Hardware", "Aime Reader Port", 0, "Port for a 837-15396 Aime card reader (0 to disable)");
            ConfigUseIO4Stick = Config.Bind("Real Hardware", "Enable IO4", false, "Connects to a IO4 board");
            ConfigIO4AxisX = Config.Bind("Real Hardware", "X Axis ADC", 0, "The ADC to use for X axis input");
            ConfigIO4AxisY = Config.Bind("Real Hardware", "Y Axis ADC", 4, "The ADC to use for Y axis input");
            ConfigIO4AxisXMin = Config.Bind("Real Hardware", "X Axis ADC Min", 0, "Min value for X axis");
            ConfigIO4AxisXMax = Config.Bind("Real Hardware", "X Axis ADC Max", 65535, "Max value for X axis");
            ConfigIO4AxisYMin = Config.Bind("Real Hardware", "Y Axis ADC Min", 0, "Min value for Y axis");
            ConfigIO4AxisYMax = Config.Bind("Real Hardware", "Y Axis ADC Max", 65535, "Max value for Y axis");
            ConfigIO4StickDeadzone = Config.Bind("Real Hardware", "Stick Deadzone", 15, "The stick deadzone in percent");
            ConfigIO4AxisXInvert = Config.Bind("Real Hardware", "X Axis Invert", false, "Inverts the X axis");
            ConfigIO4AxisYInvert = Config.Bind("Real Hardware", "Y Axis Invert", false, "Inverts the Y axis");
            ConfigAttackButtonLed = Config.Bind("Real Hardware", "IO4 Attack LED 1", 0, "The LED for the Attack 1 button on an IO4");
            ConfigAttackButtonLed2 = Config.Bind("Real Hardware", "IO4 Attack LED 2", 0, "The LED for the Attack 2 button on an IO4");
            ConfigAttackButtonLed3 = Config.Bind("Real Hardware", "IO4 Attack LED 3", 0, "The LED for the Attack 3 button on an IO4");
            Config15093Port = Config.Bind("Real Hardware", "837-15093-06 LED Board Port", 0, "The Port for a SEGA 837-15093-06 LED board (0 to disable, for LED mappings see readme)");
            ConfigLEDHostAddr = Config.Bind("Real Hardware", "LED Host Address", 2, new ConfigDescription("LED Board host address", null, new ConfigurationManagerAttributes { IsAdvanced = true }));
            ConfigLEDBoardAddr = Config.Bind("Real Hardware", "LED Board Address", 1, new ConfigDescription("LED Board own address", null, new ConfigurationManagerAttributes { IsAdvanced = true }));

            string exe = Environment.CurrentDirectory.Split('\\').Last();
            Logger.LogDebug("Game = " + exe);

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
                Harmony.CreateAndPatchAll(typeof(DebugPatchesGame), "eu.haruka.gmg.sao.fixes.game.debug");
                Harmony.CreateAndPatchAll(typeof(CameraPatchesGame), "eu.haruka.gmg.sao.fixes.game.camera");
                Harmony.CreateAndPatchAll(typeof(TranslationPatchesGame), "eu.haruka.gmg.sao.fixes.game.tl");
                BasePatchesGame.Initialize();
                runGameUpdate = true;
            }
            if (exe == "notice") {
                TestSwitchIsToggle = true;
                Harmony.CreateAndPatchAll(typeof(TerminalDecicderPatch), "eu.haruka.gmg.sao.fixes.notice.terminal");
                Harmony.CreateAndPatchAll(typeof(KeychipPatchesNoticeAndTest), "eu.haruka.gmg.sao.fixes.notice.keychip");
                Harmony.CreateAndPatchAll(typeof(CardPatchesNotice), "eu.haruka.gmg.sao.fixes.notice.bngrw");
                Harmony.CreateAndPatchAll(typeof(BNAMPFPatchesNotice), "eu.haruka.gmg.sao.fixes.notice.oomph");
            }
            if (exe == "testmode") {
                Harmony.CreateAndPatchAll(typeof(KeychipPatchesNoticeAndTest), "eu.haruka.gmg.sao.fixes.notice.keychip");
                Harmony.CreateAndPatchAll(typeof(IOPatchesTest), "eu.haruka.gmg.sao.fixes.testmode.io");
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
            
            if (ConfigPrinterCleanOnStart.Value) {
                if (Directory.Exists(ConfigPrinterDirectory.Value)) {
                    Directory.Delete(ConfigPrinterDirectory.Value, true);
                }
            }
            if (!Directory.Exists(ConfigPrinterDirectory.Value)) {
                Directory.CreateDirectory(ConfigPrinterDirectory.Value);
            }

            Haruka.Arcade.SEGA835Lib.Debugging.Log.Mute = true;
            Haruka.Arcade.SEGA835Lib.Debugging.Log.LogMessageWritten += Log_LogMessageWritten;
            if (ConfigAimeReaderPort.Value > 0) {
                Log.LogInfo("Aime connection on port " + ConfigAimeReaderPort.Value);
                Aime = new AimeCardReader_837_15396(ConfigAimeReaderPort.Value);
                DeviceStatus ret = Aime.Connect();
                AimeConnected = ret == DeviceStatus.OK;
                if (AimeConnected) {
                    Aime.LEDReset();
                    Aime.GetHWVersion(out string v);
                    Log.LogInfo("Aime Board: " + v);
                } else {
                    Log.LogMessage("Aime connection failed: " + ret);
                }
            }
            if (ConfigUseIO4Stick.Value) {
                Log.LogInfo("Connecting to IO4");
                Io4 = new IO4USB_835_15257_01();
                DeviceStatus ret = Io4.Connect();
                IoConnected = ret == DeviceStatus.OK;
                if (IoConnected) {
                    Io4.ResetBoardStatus();
                    new Thread(Io4Polling).Start();
                } else {
                    Log.LogMessage("IO4 connection failed: " + ret);
                }
            }
            if (Config15093Port.Value > 0) {
                Log.LogInfo("Connecting to LED");
                Led = new LED_837_15093_06(Config15093Port.Value, (byte)ConfigLEDHostAddr.Value, (byte)ConfigLEDBoardAddr.Value);
                DeviceStatus ret = Led.Connect();
                LedConnected = ret == DeviceStatus.OK;
                if (LedConnected) {
                    Led.SetResponseDisabled(true);
                } else {
                    Log.LogMessage("LED connection failed: " + ret);
                }
            }

            BaseUnityPlugin emoneyuilink = Chainloader.Plugins.Find(p => p.Info.Metadata.GUID == "eu.haruka.gmg.apm.emoneyuilink");
            if (emoneyuilink != null) {
                EMoneyUILinkIntegration.Initalize(Aime);
            } else {
                Plugin.Log.LogWarning("EMoneyUI integration not found");
            }

            if (!WantsBootSatellite) {
                LoadAMPFCoin();
            }


            Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void Io4Polling() {
            while (IoConnected) {
                DeviceStatus ret = Io4.Poll(out JVSUSBReportIn r);
                if (ret != DeviceStatus.OK) {
                    IoConnected = false;
                    Log.LogError("IO4 poll failed: " + ret);
                    break;
                } else {
                    IoReport = r;
                }
                Thread.Sleep(1);
            }
            Io4?.Disconnect();
        }

        private void Log_LogMessageWritten(Haruka.Arcade.SEGA835Lib.Debugging.LogEntry obj) {
            if (Plugin.ConfigLogSegaLibMessages.Value) {
                Logger.LogInfo("Sega835Lib: " + obj.Message);
            }
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1) {
            Logger.LogDebug(arg0.name + " -> " + arg1.name);
            if (arg1.name == "SubScene_AttractPv_Parent") {
                Logger.LogMessage("SwordArtOffline " + PluginInfo.PLUGIN_VERSION + " mod for SAO Arcade: Deep Explorer - 2024 Haruka");
                Logger.LogMessage("part of the GenericMusicGames preservation project");
                if (Plugin.ConfigShowMenuKeybinds.Value) {
                    Logger.LogMessage("Press F1 to open the mod settings menu.");
                    Logger.LogMessage("Press " + ConfigButtonScanCard.Value.MainKey + " to scan your Banapassport.");
                    Logger.LogMessage("Press " + ConfigButtonCoin.Value.MainKey + " to insert a coin.");
                }
            } else if (arg1.name == "Quest") {
                IOPatches.ResetCharacterPosition();
            }
        }

        public void Update() {
            if (ConfigButtonCoin.Value.IsDown()) {
                Coins++;
                SaveAMPFCoin();
            }
            if (ConfigButtonService.Value.IsDown()) {
                Service++;
                SaveAMPFCoin();
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

        public static void LoadAMPFCoin() {
            string f = "F:\\saocf_ampf_coin.bin";
            if (File.Exists(f)) {
                try {
                    string[] data = File.ReadAllText(f).Split(',');
                    Coins = Int32.Parse(data[0]);
                    Service = Int32.Parse(data[1]);
                    Log.LogDebug("Restored credits from AMPF backup");
                } catch {
                    Log.LogError("Failed to restore credits from AMPF backup");
                }
            } else {
                Log.LogWarning("No AMPF backup found");
            }
        }

        public static void SaveAMPFCoin() {
            if (GameManager.IsTerminal) {
                string f = "F:\\saocf_ampf_coin.bin";
                try {
                    File.WriteAllText(f, Coins + "," + Service);
                    Log.LogDebug("AMPF backup saved");
                } catch (Exception ex) {
                    Log.LogError("Failed to save AMPF backup: " + ex);
                }
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

        internal static void ShowDialog(string message, int? timer, bool isYesNo, Action<bool> onClose) {
            Log.LogDebug("ShowDialog: " + message);
            instance.StartCoroutine(ShowDialogCo(message, timer, isYesNo, onClose));
        }

        
        private static IEnumerator<object> ShowDialogCo(string message, int? timer, bool isYesNo, Action<bool> onClose) {
            Log.LogDebug("ShowDialog: Instantiating");

            if (isYesNo) {
                message += "<size=0%>";
            }

            bool? isEnd = null;

            while (!SceneManager.GetSceneByName("SubScene_UI_MenuDialog").isLoaded) {
                while (AssetBundleManager.AssetBundleManifestObject == null) {
                    //Log.LogDebug("NOT ASSETBUNDLE");
                    yield return null;
                }
                MenuUIManager.Instance.AddScene("SubScene_UI_MenuDialog");
                //Log.LogDebug("NOT LOADED");
                yield return null;
            }

            CommonUI_MenuDialog dlg = MenuUIManager.Instance.GetDialog();
            if (dlg == null) {
                Log.LogError("CommonUI_MenuDialog is null!");
            }

            string[] dialogKeys = (!isYesNo ? new string[] { "DIALOG_SWORDARTOFFLINE_CUSTOM_MESSAGE", "DIALOGN_ERROR_UNABLE_TO_PLAY_01" } : new string[] { "CHECK_SWORDARTOFFLINE_CUSTOM_CHOICE", "CHECK_SHOP_USE_EVENT_ITEM" });

            string dialogKey = dialogKeys[dialogKeys.Length - 1];
            foreach (string dialog in dialogKeys) {
                dialogKey = dialog;
                if (dlg.dialogDataList.ContainsKey(dialogKey)) {
                    break;
                }
            }

            if (!dlg.dialogDataList.ContainsKey(dialogKey)) {
                Log.LogError("Can't display custom dialog, UI table is not yet loaded!");
                Log.LogError("Text was: " + message);
                yield break;
            }

            bool hasLoadingBlocker = false;
            if (LoadingUIManager.HasInstance() && LoadingUIManager.Instance.m_Connecting.activeSelf) {
                Log.LogDebug("Getting rid of loading UI");
                hasLoadingBlocker = true;
                LoadingUIManager.Instance.Hide();
            }
            int countOfBlockers = MenuUIManager.Instance.m_BlockerStack.Count;
            MenuUIManager.Instance.ClearUIBlocker();


            dlg.SetDisplay(true);

            Log.LogDebug("ShowDialogCo: " + dialogKey);
            if (isYesNo) {
                dlg.DialogDisplay(dialogKey, delegate {
                    isEnd = true;
                }, delegate {
                    isEnd = false;
                }, false, timer.GetValueOrDefault(-1), timer != null ? MenuUITime.TimeType.Generic : MenuUITime.TimeType.NULL, new object[] { message, message, message, message }, true);
            } else {
                dlg.DialogDisplay(dialogKey, delegate {
                    isEnd = true;
                }, timer.GetValueOrDefault(-1), timer.HasValue ? MenuUITime.TimeType.Generic : MenuUITime.TimeType.NULL, new object[] { message }, true);
            }
            
            while (isEnd == null) {
                if (BasePatchesGame.RetryNetworkFlag) {
                    dlg.CloseRequest();
                }
                yield return null;
            }

            onClose(isEnd.Value);

            if (hasLoadingBlocker) {
                Log.LogDebug("Restoring loading blocker");
                LoadingUIManager.Instance.Show();
            }
            Log.LogDebug("Restoring "+countOfBlockers+" other blocker(s)");
            for (int i = 0; i < countOfBlockers; i++) {
                MenuUIManager.Instance.PushUIBlocker();
            }

            yield return null;
        }

        public static string GetKeychip() {
            if (keychip == null) {
                InitKeychip();
            }
            return keychip;
        }

        public static IntPtr GetKeychipA() {
            if (keychipA == IntPtr.Zero) {
                InitKeychip();
            }
            return keychipA;
        }

        public static IntPtr GetKeychipU() {
            if (keychipU == IntPtr.Zero) {
                InitKeychip();
            }
            return keychipU;
        }

        private static void InitKeychip() {
            const string DUMMY_ID = "000000000000";
            string id;
            Plugin.Log.LogInfo("Loading keychip...");
            try {
                IniFile i = new IniFile("F:\\WritableConfig.ini");
                id = i.Read("serialID", "RuntimeConfig");
                if (id == "") {
                    id = DUMMY_ID;
                }
                Plugin.Log.LogInfo("Keychip = " + id);
                string text2 = id.Substring(0, 4);
                string text3 = id.Substring(4, 1);
                string text4 = id.Substring(5, 1);
                string text5 = id.Substring(6, 2);
                if (text2 != "2825") {
                    Plugin.Log.LogError("Invalid keychip! (Game ID)");
                } else if (text5 != "04" && text5 != "05") {
                    Plugin.Log.LogError("Invalid keychip! (Model ID)");
                }
                if (text3 != "1") {
                    Plugin.Log.LogError("Invalid keychip! (Region ID)");
                }
                if (text4 != "3" && text4 != "4" && text4 != "5") {
                    Plugin.Log.LogError("Invalid keychip! (Charge Mode)");
                }
            } catch (Exception ex) {
                Plugin.Log.LogError("Keychip retrieval failed: " + ex);
                id = DUMMY_ID;
            }
            keychip = id;
            keychipA = Marshal.StringToHGlobalAnsi(id);
            keychipU = Marshal.StringToHGlobalUni(id);
        }

        public static string Md5(string filename) {
            if (!File.Exists(filename)) {
                return null;
            }
            using (var md5 = MD5.Create()) {
                using (var stream = File.OpenRead(filename)) {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
