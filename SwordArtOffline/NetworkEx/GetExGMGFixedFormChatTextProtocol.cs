using LINK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.NetworkEx {
    internal class GetExGMGFixedFormChatTextProtocol : GetExGMGProtocol<StaticFixedFormChatTextDataManager, StaticTextData> {
        protected override string GetURL() {
            return "master_data_ex/get_m_ex_fixedformchattext";
        }

        protected override int ReadObjectFromNet(StaticTextData data, byte[] pDat, int off, int size) {
            data.Set("Id", (uint)ReadInt(pDat, ref off, size));
            data.Set("Text", ReadString(pDat, ref off, size));
            return off;
        }
    }
}
