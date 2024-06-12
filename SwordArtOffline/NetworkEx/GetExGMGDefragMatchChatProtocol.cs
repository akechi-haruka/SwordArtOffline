using LINK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.NetworkEx {
    internal class GetExGMGDefragMatchChatProtocol : GetExGMGProtocol<StaticDefragMatchChatDataManager, StaticDefragMatchChatData> {
        protected override string GetURL() {
            return "master_data_ex/get_m_ex_defragmatchchat";
        }

        protected override int ReadObjectFromNet(StaticDefragMatchChatData data, byte[] pDat, int off, int size) {
            data.Set("DefragMatchChatId", ReadInt(pDat, ref off, size));
            data.Set("ChatType", ReadByte(pDat, ref off, size));
            data.Set("TypeNo", ReadByte(pDat, ref off, size));
            data.Set("DisplayName", ReadString(pDat, ref off, size));
            data.Set("TextId", ReadString(pDat, ref off, size));
            data.Set("VoiceId", ReadString(pDat, ref off, size));
            return off;
        }
    }
}
