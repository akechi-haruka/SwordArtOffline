using LINK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.NetworkEx {
    internal class GetExGMGDialogUIProtocol : GetExGMGProtocol<StaticDialogUIDataManager, StaticDialogUIData> {
        protected override string GetURL() {
            return "master_data_ex/get_m_ex_gmg_dialogui";
        }

        protected override int ReadObjectFromNet(StaticDialogUIData data, byte[] pDat, int off, int size) {
            data.Set("DialogUiId", ReadShort(pDat, ref off, size));
            data.Set("RecordId", ReadString(pDat, ref off, size));
            data.Set("Text", ReadString(pDat, ref off, size));
            data.Set("PrefabPath", ReadString(pDat, ref off, size));
            return off;
        }
    }
}
