using System;
using System.Collections.Generic;
using System.Text;
using AXAMansard.Framework.ISO8583.DTO;
using AXAMansard.Framework.ISO8583.Utility;
using AXAMansard.Framework.ISO8583.Client.Configuration;
using AXAMansard.Framework.Utility;

namespace AXAMansard.Framework.ISO8583.Client.Messages
{
    internal class ChangePIN : AdministrationMessage
    {
        public ChangePIN(CardAcceptor cardAcceptor, Account acct, CardDetails theCard, byte[] newPINBlock,string transactionID, bool isRepeat)
            : base(cardAcceptor, transactionID, TransactionType.ChangePIN, theCard,acct.Type, AccountType.Default, isRepeat)
        {
            this.Fields.Add(FieldNos.F102_Account1, acct.Number);
            this.Fields.Add(FieldNos.F53_SecurityInfo, newPINBlock);

        }
    }
}
