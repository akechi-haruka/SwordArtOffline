using LINK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwordArtOffline.NetworkEx {
    internal class GetExGMGTextCharaMessageProtocol : GetExGMGProtocol<StaticTextCharaMessageDataManager, StaticTextCharaMessageData> {
        protected override string GetURL() {
            return "master_data_ex/get_m_ex_gmg_textcharamessage";
        }

        protected override int ReadObjectFromNet(StaticTextCharaMessageData data, byte[] pDat, int off, int size) {
            data.Set("TextCharaMessageId", ReadInt(pDat, ref off, size));
            data.Set("CharaId", ReadShort(pDat, ref off, size));
            data.Set("Message", ReadString(pDat, ref off, size));
            return off;
        }
    }
}
