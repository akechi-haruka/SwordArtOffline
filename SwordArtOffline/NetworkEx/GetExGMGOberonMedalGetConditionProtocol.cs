using LINK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.NetworkEx {
    internal class GetExGMGOberonMedalGetConditionProtocol : GetExGMGProtocol<StaticYuiMedalGetConditionDataManager, StaticYuiMedalGetConditionData> {
        protected override string GetURL() {
            return "master_data_ex/get_m_ex_oberonmedalgetcondition";
        }

        protected override int ReadObjectFromNet(StaticYuiMedalGetConditionData data, byte[] pDat, int off, int size) {
            data.Set("YuiMedalGetConditionId", ReadShort(pDat, ref off, size));
            data.Set("NeedLoginDays", ReadInt(pDat, ref off, size));
            data.Set("Num", ReadInt(pDat, ref off, size));
            return off;
        }
    }
}
