using Artdink.StaticData;
using HarmonyLib;
using LINK;
using LINK.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;

namespace SwordArtOffline.Patches {
    internal class TranslationPatchesGame {

        [HarmonyPrefix, HarmonyPatch(typeof(MenuDialogBuyTicket), "BuyTicket")]
        static bool BuyTicket(int credit, int ticket, MenuDialogBuyTicket __instance) {
            if (Plugin.ConfigHardTranslations.Value) {
                TextMeshProUGUI component = __instance.transform.Find("Contents/MessageText").GetComponent<TextMeshProUGUI>();
                string text = string.Concat(new object[] { ticket, " Ticket(s) with ", credit, " Credit(s)", component.text });
                component.SetText(text);
                return false;
            }
            return true;
        }


        [HarmonyPrefix, HarmonyPatch(typeof(MenuDialogCheckSaveBack), "BuyTicket")]
        static bool BuyTicket(ref string nextScene, MenuDialogBuyTicket __instance) {
            if (Plugin.ConfigHardTranslations.Value) {
                if (nextScene == "カスタムメニュー") {
                    nextScene = "Customization Menu";
                } else if (nextScene == "ガシャメニュー") {
                    nextScene = "Gacha Menu";
                } else if (nextScene == "メインメニュー") {
                    nextScene = "Main Menu";
                } else if (nextScene == "デフラグマッチメニュー") {
                    nextScene = "Defrag Match Menu";
                }
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuDialogCPGasha), "UsedCredit")]
        static bool UsedCredit(int creditNum, int openLogNum, MenuDialogCPGasha __instance) {
            if (Plugin.ConfigHardTranslations.Value) {
                TextMeshProUGUI component = __instance.transform.Find("Contents/MessageText").GetComponent<TextMeshProUGUI>();
                string text = ((openLogNum != 3) ? "remaining" : "3");
                string text2 = string.Format(component.text, creditNum, text);
                component.text = text2;
                return false;
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuDialogCreditLack), "ChangeText")]
        static bool ChangeText(int needCredit, string addText, MenuDialogCreditLack __instance) {
            if (Plugin.ConfigHardTranslations.Value) {
                TextMeshProUGUI component = __instance.transform.Find("Contents/MessageText").GetComponent<TextMeshProUGUI>();
                if (__instance.firstString == string.Empty) {
                    __instance.firstString = component.text;
                }
                string text = string.Concat(new object[] { "Lacking \u3000", needCredit, " credit(s)\n\n", __instance.firstString, addText });
                component.text = text;
                return false;
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuDialogCustomRank4Check), "CheckRank")]
        static bool CheckRank(List<SelectTubLogData> data, bool isCheckItem, bool isCheckLevelMax, bool isNotice, MenuDialogCustomRank4Check __instance) {
            if (Plugin.ConfigHardTranslations.Value) {
                TextMeshProUGUI component = __instance.transform.Find("Contents/MessageText").GetComponent<TextMeshProUGUI>();
                string text = string.Empty;
                if (isNotice) {
                    text = "<color=#FFCC00><< CAUTION >></color>";
                }
                if (isCheckLevelMax) {
                    text = "<color=#FFCC00><< CAUTION >>\n<size=70%>The level expansion limit will be exceeded with the used items!\nExcess levels are rounded down.</size>\n</color>";
                    component.text = string.Format(component.text, text);
                    return false;
                }
                int i = 0;
                while (i < data.Count) {
                    switch (data[i].LogIdentifier) {
                        case SelectTubLogIdentifier.HERO_LOG:
                            if (CommonManager<StaticHeroLogDataManager, StaticHeroLogData>.GetInstance().FindData(((HeroLogUserData)data[i].Log).HeroLogId).Rarity >= 4) {
                                goto IL_019D;
                            }
                            break;
                        case SelectTubLogIdentifier.EQUIPMENT_WEAPON:
                        case SelectTubLogIdentifier.EQUIPMENT_ARMOR:
                            if (CommonManager<StaticEquipmentDataManager, StaticEquipmentData>.GetInstance().FindData(((EquipmentUserData)data[i].Log).EquipmentId).Rarity >= 4) {
                                goto IL_019D;
                            }
                            break;
                        case SelectTubLogIdentifier.ITEM:
                            if (isCheckItem && CommonManager<StaticItemDataManager, StaticItemData>.GetInstance().FindData(((ItemUserData)data[i].Log).ItemId).ItemTypeId != 8 && CommonManager<StaticItemDataManager, StaticItemData>.GetInstance().FindData(((ItemUserData)data[i].Log).ItemId).Rarity >= 4) {
                                goto IL_019D;
                            }
                            break;
                        case SelectTubLogIdentifier.SUPPORT_LOG:
                            if (CommonManager<StaticSupportLogDataManager, StaticSupportLogData>.GetInstance().FindData(((SupportLogUserData)data[i].Log).SupportLogId).Rarity >= 4) {
                                goto IL_019D;
                            }
                            break;
                        case SelectTubLogIdentifier.NO_DATA:
                            break;
                        default:
                            goto IL_019D;
                    }
                    i++;
                    continue;
IL_019D:
                    text = "<color=#FFCC00><< CAUTION >>\nRare resources are selected!\n</color>";
                    break;
                }
                component.text = string.Format(component.text, text);
                return false;
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuDialogCustomUsedTicket), "ChangeText")]
        static bool ChangeText(MenuDialogCustomUsedTicket __instance) {
            if (Plugin.ConfigHardTranslations.Value) {
                __instance.transform.Find("Contents/MessageText").GetComponent<TextMeshProUGUI>().text = string.Format(__instance.transform.Find("Contents/MessageText").GetComponent<TextMeshProUGUI>().text, MenuUIManager.Instance.LocalData.Ticket.ToString(), (MenuUIManager.Instance.LocalData.Ticket - 1 >= 0) ? "spend" : "purchase");
                return false;
            }
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuDialogOpenLog), "UsedCredit")]
        static bool UsedCredit(int pay, int openLogNum, MenuDialogOpenLog __instance) {
            if (Plugin.ConfigHardTranslations.Value) {
                TextMeshProUGUI component = __instance.transform.Find("Contents/MessageText").GetComponent<TextMeshProUGUI>();
                string text = ((openLogNum != 3) ? "remaining" : "3");
                string text2 = string.Format(component.text, pay, text);
                component.text = text2; 
                return false;
            }
            return true;
        }

    }
}
