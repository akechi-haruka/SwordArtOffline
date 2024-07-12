using HarmonyLib;
using LINK.UI;
using LINK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using Artdink.StaticData;
using TMPro;
using UnityEngine.UI;
using static LINK.UI.CommonUI_GashaMenuDataManager;

namespace SwordArtOffline.Patches.Game {
    public class DebugPatchesGame {

        [HarmonyPostfix, HarmonyPatch(typeof(CommonUI_GashaMenu), "ChechGashaData")]
        static void ChechGashaData(ref int __result, int id, UseType type, GashaOpenType open) {
            Plugin.Log.LogDebug("Gasha " + id + "/" + type + "/" + open + " -> " + __result);
            Plugin.Log.LogDebug("Header list: " + ServerDataManager.Instance.GashaHeaderDataList.Count);
            Plugin.Log.LogDebug(CommonUI_GashaMenuDataManager.Instance.gashaDataRarity[0].gashaData.Count);
            if (CommonUI_GashaMenuDataManager.Instance.gashaDataRarity[0].gashaData.Count == 0) {
                Plugin.Log.LogDebug("Reconstructing gacha rarity tables due to weird pre-initialization glitch");
                CommonUI_GashaMenuDataManager.instance = new CommonUI_GashaMenuDataManager();
            }
        }

        [HarmonyPrefix, HarmonyPatch(typeof(CommonUI_GashaMenuDataManager), "GetPrizeData")]
        static bool WhatTheFuckIsATub(CommonUI_GashaMenuDataManager __instance, int index) {
            Plugin.Log.LogDebug("TUB??? index: " + index);
            CommonUI_GashaMenuDataManager.GashaRarity gashaRarity = __instance.gashaRarityList[index];
            Plugin.Log.LogDebug("gasha rarity: " + gashaRarity.Index + ", rarity: " + gashaRarity.Rarity);
            int rarity = gashaRarity.Rarity;
            Plugin.Log.LogDebug("lottery prize: " + UserDataManager.Instance.GashaResultData.LotteryPrizeList[gashaRarity.Index]);
            int prizeId = UserDataManager.Instance.GashaResultData.LotteryPrizeList[gashaRarity.Index].GashaPrizeId;
            int prizeNo = (int)UserDataManager.Instance.GashaResultData.LotteryPrizeList[gashaRarity.Index].PrizeNo;
            Plugin.Log.LogDebug("prize id = " + prizeId + ", prize no = " + prizeNo);
            string userCommonRewardId = UserDataManager.Instance.GashaResultData.LotteryPrizeList[gashaRarity.Index].UserCommonRewardId;
            Plugin.Log.LogDebug("Reward id = " + userCommonRewardId);
            GashaPrizeData gashaPrizeData = ServerDataManager.Instance.GashaPrizeDataList.Find((GashaPrizeData d) => d.GashaPrizeId == prizeId);
            Plugin.Log.LogDebug("gasha prize data = " + gashaPrizeData);
            RewardType commonRewardType = gashaPrizeData.CommonRewardType;
            int commonRewardId = gashaPrizeData.CommonRewardId;
            Plugin.Log.LogDebug("prize reward type = " + commonRewardType);
            Plugin.Log.LogDebug("prize reward id = " + commonRewardId);
            CommonUI_GashaMenuDataManager.GashaLogData logData = __instance.gashaDataRarity[rarity].GetLogData(commonRewardType, commonRewardId);
            Plugin.Log.LogDebug("get log data = " + logData);
            Plugin.Log.LogDebug("get log data rarity = " + logData.rarity);
            Plugin.Log.LogDebug("get log data TUB = " + logData.logData);
            logData.prizeNo = prizeNo;
            return true;
        }



        /*[HarmonyPostfix, HarmonyPatch(typeof(CommonUI_MenuDialog), "InstantiateDialog")]

        static IEnumerator InstantiateDialog(IEnumerator __result, CommonUI_MenuDialog __instance, string displayDialog, UnityAction yes, UnityAction no, UnityAction[] ex, bool isEndYes, float dialogTime, MenuUITime.TimeType backUpTimeType, string seName, object[] argument, bool textFormat = false) {
            Plugin.Log.LogDebug("inside menu dialog instantation");
            MenuUIManager.Instance.PushUIBlocker(null);
            string name = __instance.dialogDataList[displayDialog].PrefabPath;
            string path = MenuDialogFactory.GetPath(name);
            Plugin.Log.LogDebug("start wait prefab");
            while (__instance.prefabList[path] == null) {
                yield return null;
            }
            Plugin.Log.LogDebug("done wait prefab");
            GameObject obj = UnityEngine.Object.Instantiate(__instance.prefabList[path], __instance.transform);
            obj.SetActive(true);
            MenuDialogConfirmation c = MenuDialogFactory.GetComponent(obj, name);
            c.Init(__instance.dialogDataList[displayDialog], yes, no, ex, isEndYes, dialogTime, backUpTimeType, seName, delegate {
                if (__instance.beforeShowDialog != __instance.showDialog) {
                    __instance.beforeShowDialog = __instance.showDialog;
                    return;
                }
                __instance.showDialog = null;
                __instance.closeAction = null;
                __instance.beforeShowDialog = null;
            }, argument, textFormat);
            c.ParserArgs(argument);
            __instance.showDialog = obj;
            __instance.closeAction = new UnityAction<UnityAction>(c.CloseDialog);
            if (__instance.beforeShowDialog == null) {
                __instance.beforeShowDialog = obj;
            }
            Plugin.Log.LogDebug("dialog show");
            __instance.isInstantiating = false;
            MenuUIManager.Instance.PopUIBlocker();
            if (__instance.closeRequest) {
                Plugin.Log.LogDebug("close request");
                __instance.CloseDialog(__instance.closeRequestAction);
                __instance.closeRequest = false;
                __instance.closeRequestAction = null;
            }
            yield break;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(MenuUIManager), "GetDialog")]
        static bool GetDialog(ref CommonUI_MenuDialog __result) {
            Plugin.Log.LogDebug("GetDialog");
            CommonUI_MenuDialog commonUI_MenuDialog = null;
            MenuUISubSceneCtrl menuUISubSceneCtrl = MenuUIManager.Instance.GetMenuUISubSceneCtrl("SubScene_UI_MenuDialog");
            Plugin.Log.LogDebug("menuUISubSceneCtrl inst=" + menuUISubSceneCtrl);
            if (menuUISubSceneCtrl != null) {
                commonUI_MenuDialog = menuUISubSceneCtrl.GetMenuParts<CommonUI_MenuDialog>();
                Plugin.Log.LogDebug("commonUI_MenuDialog menuparts inst=" + commonUI_MenuDialog);
                commonUI_MenuDialog.InitGameobject();
            }
            __result = commonUI_MenuDialog;
            return false;
        }*/

        /*[HarmonyPrefix, HarmonyPatch(typeof(CommonUI_GashaMenu.TubData), "SetMedalInfo")]
        static void SetMedalInfo(CommonUI_GashaMenu.TubData __instance, int gashaMedalId, int num) {
            Plugin.Log.LogDebug("SetMedalInfo ID " + gashaMedalId + " NUM " + num);
            StaticGashaMedalsData staticGashaMedalsData = CommonManager<StaticGashaMedalsDataManager, StaticGashaMedalsData>.GetInstance().FindData(gashaMedalId);
            Plugin.Log.LogDebug("Ret obj name: " + staticGashaMedalsData?.Name);
            Plugin.Log.LogDebug("Ret obj type: " + staticGashaMedalsData?.GashaMedalType);
            StaticGashaMedalTypesData staticGashaMedalTypesData = CommonManager<StaticGashaMedalTypesDataManager, StaticGashaMedalTypesData>.GetInstance().FindData((int)staticGashaMedalsData.GashaMedalType);
            Plugin.Log.LogDebug("Ret obj type: " + staticGashaMedalTypesData?.Name);
            string text = "LogResource/GCOIN/" + staticGashaMedalTypesData.MedalIcon;
            Plugin.Log.LogDebug(text);
            Plugin.Log.LogDebug(__instance.gashaMenuMedalInfo);
            Plugin.Log.LogDebug(__instance.gashaMenuMedalInfo.transform.Find("MedalImage").GetComponent<Image>());
            __instance.gashaMenuMedalInfo.transform.Find("MedalImage").GetComponent<Image>().sprite = AssetManager.LogResource.FindSprite(text);
            Plugin.Log.LogDebug(__instance.gashaMenuMedalInfo.transform.Find("text/Title"));
            Plugin.Log.LogDebug(__instance.gashaMenuMedalInfo.transform.Find("text/Title").GetComponent<TextMeshProUGUI>());
            __instance.gashaMenuMedalInfo.transform.Find("text/Title").GetComponent<TextMeshProUGUI>().text = string.Format("所持{0}", staticGashaMedalsData.Name);
            Plugin.Log.LogDebug(__instance.gashaMenuMedalInfo.transform.Find("text/MedalNum"));
            Plugin.Log.LogDebug(__instance.gashaMenuMedalInfo.transform.Find("text/MedalNum").GetComponent<TextMeshProUGUI>());
            __instance.gashaMenuMedalInfo.transform.Find("text/MedalNum").GetComponent<TextMeshProUGUI>().text = string.Format("{0:#,0}", num);
        }*/

        /*[HarmonyPrefix, HarmonyPatch(typeof(CommonUI_GashaMenuDataManager), MethodType.Constructor)]
        static bool CommonUI_GashaMenuDataManagerCtor(CommonUI_GashaMenuDataManager __instance) {
            Plugin.Log.LogDebug("GASHA MENU CONSTRUCTOR");
            for (int i = 0; i < __instance.gashaDataRarity.Length; i++) {
                __instance.gashaDataRarity[i] = new CommonUI_GashaMenuDataManager.GashaDataRarity();
            }
            CommonUI_GashaMenuDataManager.GashaLogData gashaLogData = default(CommonUI_GashaMenuDataManager.GashaLogData);
            HeroLogUserData heroLogUserData = default(HeroLogUserData);
            Plugin.Log.LogDebug("HERO LOG COUNT = " + CommonManager<StaticHeroLogDataManager, StaticHeroLogData>.GetInstance().GetRecordCount());
            for (int j = 0; j < CommonManager<StaticHeroLogDataManager, StaticHeroLogData>.GetInstance().GetRecordCount(); j++) {
                StaticHeroLogData recordByIndex = CommonManager<StaticHeroLogDataManager, StaticHeroLogData>.GetInstance().GetRecordByIndex(j);
                heroLogUserData.HeroLogId = recordByIndex.HeroLogId;
                gashaLogData.logData = new SelectTubLogData();
                gashaLogData.logData.SetHeroLog(heroLogUserData);
                gashaLogData.rarity = (int)recordByIndex.Rarity;
                __instance.gashaDataRarity[(int)(recordByIndex.Rarity - 1)].gashaData.Add(gashaLogData);
            }
            for (int i = 0; i < __instance.gashaDataRarity.Length; i++) {
                Plugin.Log.LogDebug("COUNT OF RARITY " + __instance.gashaDataRarity[i].gashaData.Count);
            }
            EquipmentUserData equipmentUserData = default(EquipmentUserData);
            for (int k = 0; k < CommonManager<StaticEquipmentDataManager, StaticEquipmentData>.GetInstance().GetRecordCount(); k++) {
                StaticEquipmentData recordByIndex2 = CommonManager<StaticEquipmentDataManager, StaticEquipmentData>.GetInstance().GetRecordByIndex(k);
                equipmentUserData.EquipmentId = recordByIndex2.EquipmentId;
                gashaLogData.logData = new SelectTubLogData();
                if (recordByIndex2.WeaponTypeId == 12 || recordByIndex2.WeaponTypeId == 13) {
                    gashaLogData.logData.SetEquipmentArmorLog(equipmentUserData);
                } else {
                    gashaLogData.logData.SetEquipmentWeaponLog(equipmentUserData);
                }
                gashaLogData.rarity = (int)recordByIndex2.Rarity;
                __instance.gashaDataRarity[(int)(recordByIndex2.Rarity - 1)].gashaData.Add(gashaLogData);
            }
            ItemUserData itemUserData = default(ItemUserData);
            for (int l = 0; l < CommonManager<StaticItemDataManager, StaticItemData>.GetInstance().GetRecordCount(); l++) {
                StaticItemData recordByIndex3 = CommonManager<StaticItemDataManager, StaticItemData>.GetInstance().GetRecordByIndex(l);
                itemUserData.ItemId = recordByIndex3.ItemId;
                gashaLogData.logData = new SelectTubLogData();
                gashaLogData.logData.SetItemLog(itemUserData);
                gashaLogData.rarity = (int)recordByIndex3.Rarity;
                __instance.gashaDataRarity[(int)(recordByIndex3.Rarity - 1)].gashaData.Add(gashaLogData);
            }
            SupportLogUserData supportLogUserData = default(SupportLogUserData);
            for (int m = 0; m < CommonManager<StaticSupportLogDataManager, StaticSupportLogData>.GetInstance().GetRecordCount(); m++) {
                StaticSupportLogData recordByIndex4 = CommonManager<StaticSupportLogDataManager, StaticSupportLogData>.GetInstance().GetRecordByIndex(m);
                supportLogUserData.SupportLogId = recordByIndex4.SupportLogId;
                gashaLogData.logData = new SelectTubLogData();
                gashaLogData.logData.SetSupportLog(supportLogUserData);
                gashaLogData.rarity = (int)recordByIndex4.Rarity;
                __instance.gashaDataRarity[(int)(recordByIndex4.Rarity - 1)].gashaData.Add(gashaLogData);
            }
            __instance.TempGashaType = -1;
            for (int i = 0; i < __instance.gashaDataRarity.Length; i++) {
                Plugin.Log.LogDebug("COUNT OF RARITY " + __instance.gashaDataRarity[i].gashaData.Count);
            }
            return false;
        }*/

        /*[HarmonyPrefix, HarmonyPatch(typeof(GashaDataRarity), "GetLogData")]
        static bool GetLogData(ref CommonUI_GashaMenuDataManager.GashaLogData __result, GashaDataRarity __instance, RewardType type, int id) {
            CommonUI_GashaMenuDataManager.GashaLogData gashaLogData = default(CommonUI_GashaMenuDataManager.GashaLogData);
            Plugin.Log.LogDebug("GashaData size = " + __instance.gashaData.Count);
            Plugin.Log.LogDebug("Searching for " + type + "/" + id);
            foreach (CommonUI_GashaMenuDataManager.GashaLogData gashaLogData2 in __instance.gashaData) {
                bool flag = false;
                SelectTubLogData logData = gashaLogData2.logData;
                Plugin.Log.LogDebug("tub is a " + logData.LogIdentifier);
                switch (logData.LogIdentifier) {
                    case SelectTubLogIdentifier.HERO_LOG:
                        if (type != RewardType.HeroLog) {
                            continue;
                        }
                        if (((HeroLogUserData)logData.Log).HeroLogId == id) {
                            gashaLogData = gashaLogData2;
                            flag = true;
                        }
                        Plugin.Log.LogDebug("Not matched " + ((HeroLogUserData)logData.Log).HeroLogId);
                        break;
                    case SelectTubLogIdentifier.EQUIPMENT_WEAPON:
                    case SelectTubLogIdentifier.EQUIPMENT_ARMOR:
                        if (type != RewardType.Equipment) {
                            continue;
                        }
                        if (((EquipmentUserData)logData.Log).EquipmentId == id) {
                            gashaLogData = gashaLogData2;
                            flag = true;
                        }
                        break;
                    case SelectTubLogIdentifier.ITEM:
                        if (type != RewardType.Item) {
                            continue;
                        }
                        if (((ItemUserData)logData.Log).ItemId == id) {
                            gashaLogData = gashaLogData2;
                            flag = true;
                        }
                        break;
                    case SelectTubLogIdentifier.SUPPORT_LOG:
                        if (type != RewardType.SupportLog) {
                            continue;
                        }
                        if (((SupportLogUserData)logData.Log).SupportLogId == id) {
                            gashaLogData = gashaLogData2;
                            flag = true;
                        }
                        break;
                }
                if (flag) {
                    Plugin.Log.LogDebug("logdata FOUND");
                    break;
                }
            }

            if (gashaLogData.logData == null) {
                Plugin.Log.LogError("NO LOG DATA");
            }

            __result = gashaLogData;
            return false;

        }
        */
    }
}
