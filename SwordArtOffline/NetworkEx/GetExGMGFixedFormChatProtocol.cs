using LINK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.NetworkEx {
    internal class GetExGMGFixedFormChatProtocol : GetExGMGProtocol<StaticFixedFormChatDataManager, StaticFixedFormChatData> {
        protected override string GetURL() {
            return "master_data_ex/get_m_ex_fixedformchat";
        }

        protected override int ReadObjectFromNet(StaticFixedFormChatData data, byte[] pDat, int off, int size) {
            data.Set("FixedFormChatId", ReadInt(pDat, ref off, size));
            data.Set("ChatType", ReadInt(pDat, ref off, size));
            data.Set("TypeNo", ReadShort(pDat, ref off, size));
            data.Set("DisplayName", ReadString(pDat, ref off, size));
            data.Set("TextId", ReadString(pDat, ref off, size));
            data.Set("VoiceId", ReadString(pDat, ref off, size));
            return off;
        }
    }
}
