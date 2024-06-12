using Artdink.StaticData;
using Artdink.Utilities;
using GssSiteSystem;
using HarmonyLib;
using LINK;
using System;

namespace SwordArtOffline.NetworkEx {

    public delegate int ReadObjectDelegate<StaticData>(StaticData data, byte[] pDat, int off, int size);

    internal abstract class GetExGMGProtocol<StaticDataManager, StaticData> : GetMasterDataProtocolBase where StaticData : Utility.CsvReadLabel, new() where StaticDataManager : ManagerInterface, ISingletonable<StaticDataManager> {

        public override void SetRequestClass(params object[] args) {
            Plugin.Log.LogDebug("GMGEx: SetRequestClass");
            this.m_RequestClass = new get_m_ex_gmg_Q<StaticData>(GetURL(), ReadObjectFromNet);
        }

        public override void SetResponseClass(GameConnect.GssProtocolBase response) {
            Plugin.Log.LogDebug("GMGEx: SetResponseClass");
            this.m_ResponseClass = response;
        }
        public override void ReceieveResponse() {
            Plugin.Log.LogDebug("GMGEx: ReceiveResponse");
            base.ReceieveResponse();
        }

        public override void ConvertResponseToStaticData() {
            Plugin.Log.LogDebug("GMGEx: ConvertResponseToStaticData");
            get_m_ex_gmg_R<StaticData> get_m_ex_gmg_R = (get_m_ex_gmg_R<StaticData>)this.m_ResponseClass;
            StaticDataManager instance = CommonManager<StaticDataManager, StaticData>.GetInstance();
            instance.ClearData();
            for (int i = 0; i < get_m_ex_gmg_R.length; i++) {
                StaticData data = get_m_ex_gmg_R.dataList[i];
                instance.EntryObject(data);
            }
            instance.OnPostRead();
        }

        protected abstract String GetURL();

        protected abstract int ReadObjectFromNet(StaticData data, byte[] pDat, int off, int size);

        protected byte ReadByte(byte[] pDat, ref int off, int size) {
            byte i = GameConnectUtil.d_byte2byte(pDat, off, size);
            off += 1;
            return i;
        }

        protected short ReadShort(byte[] pDat, ref int off, int size) {
            short i = GameConnectUtil.d_byte2short(pDat, off, size);
            off += 2;
            return i;
        }

        protected int ReadInt(byte[] pDat, ref int off, int size) {
            int i = GameConnectUtil.d_byte2int(pDat, off, size);
            off += 4;
            return i;
        }

        protected String ReadString(byte[] pDat, ref int off, int size) {
            String str = null;
            int num = GameConnectUtil.d_byte2int(pDat, off, size);
            off += 4;
            if (num > 0) {
                str = GameConnectUtil.mekeString(pDat, off, num);
                off += num;
            }
            return str;
        }
    }

    public class get_m_ex_gmg_Q<StaticData> : GameConnect.GssProtocolBase where StaticData : class, new() {
        public get_m_ex_gmg_Q(String url, ReadObjectDelegate<StaticData> writeCallback) : base((GAMECONNECT_CMDID)13337) {
            this.url = url;
            this.writeCallback = writeCallback;
        }

        public override string connect_getURL() {
            return url;
        }

        public override int connectParam_req(byte[] pDat, int off, int size) {
            int num = 0;
            return num + GameConnectUtil.e_byte2byte(pDat, off + num, size, this.dummy);
        }

        public override GameConnect.GssProtocolBase connectParam_res(byte[] pDat, int off, int size) {
            bool flag = false;
            get_m_ex_gmg_R<StaticData> get_m_ex_gmg_R = null;
            try {
                get_m_ex_gmg_R = new get_m_ex_gmg_R<StaticData>(writeCallback);
                get_m_ex_gmg_R.set_data_by_byte_input(pDat, off, size);
                flag = true;
            } finally {
                if (!flag) {
                    get_m_ex_gmg_R = null;
                }
            }
            return get_m_ex_gmg_R;
        }

        public byte dummy;
        private readonly string url;
        private readonly ReadObjectDelegate<StaticData> writeCallback;
    }

    public class get_m_ex_gmg_R<StaticData> : GameConnect.GssProtocolBase where StaticData : class, new() {
        public get_m_ex_gmg_R(ReadObjectDelegate<StaticData> writeCallback) : base((GAMECONNECT_CMDID)13337) {
            this.writeCallback = writeCallback;
        }

        public int set_data_by_byte_input(byte[] pDat, int off, int size) {
            this.result = GameConnectUtil.d_byte2byte(pDat, off, size);
            off++;
            length = GameConnectUtil.d_byte2int(pDat, off, size);
            off += 4;
            dataList = new StaticData[length];
            for (int i = 0; i < length; i++) {
                StaticData data = new StaticData();
                off = writeCallback.Invoke(data, pDat, off, size);
                dataList[i] = data;
            }
            return off;
        }

        public byte result;
        public int length;
        public StaticData[] dataList;
        private readonly ReadObjectDelegate<StaticData> writeCallback;
    }

    public static class CsvUtilityExtensions {

        public static void Set(this Utility.CsvReadLabel csv, String field, object value) {
            AccessTools.PropertySetter(csv.GetType(), field).Invoke(csv, new object[] { value });
        }

    }

}
