using System.Collections.Generic;

namespace SwitchLink.ProtocalFactory.HostNode.Helper
{
    class BankResponseHelper
    {
        private static readonly IDictionary<string, Value> Responses = new Dictionary<string, Value>
        {
            {"00", new Value{Description = "Transaction approved or request completed successfully", Action = "Complete - approved transaction"}},
            {"01", new Value{Description = "Refer to Card Issuer", Action = "Decline transaction"}},
            {"04", new Value{Description = "Pick up card", Action = "Decline transaction, retain card"}},
            {"05", new Value{Description = "Do not Honour", Action = "Decline transaction"}},
            {"06", new Value{Description = "Error", Action = "Decline transaction"}},
            {"08", new Value{Description = "Honour with signature", Action = "Approve after signature validation"}},
            {"12", new Value{Description = "Invalid Transaction", Action = "Decline transaction"}},
            {"13", new Value{Description = "Invalid Amount", Action = "Decline transaction"}},
            {"14", new Value{Description = "Invalid Card Number", Action = "Decline transaction"}},
            {"15", new Value{Description = "No such Issuer", Action = "Decline transaction"}},
            {"19", new Value{Description = "Re-enter transaction", Action = "Decline transaction - retry"}},
            {"21", new Value{Description = "No action taken", Action = "Unmatched reversal processing"}},
            {"30", new Value{Description = "Format Error", Action = "Decline transaction"}},
            {"31", new Value{Description = "Bank not supported by switch", Action = "Decline transaction"}},
            {"33", new Value{Description = "Expired card", Action = "Decline transaction, retain card"}},
            {"34", new Value{Description = "Suspected fraud", Action = "Decline transaction, retain card"}}, 
            {"36", new Value{Description = "Restricted card", Action = "Decline transaction, retain card"}},
            {"38", new Value{Description = "Allowable PIN tries exceeded", Action = "Decline transaction, retain card"}},
            {"40", new Value{Description = "Requested Function Not supported", Action = "Decline transaction"}},
            {"41", new Value{Description = "Lost card", Action = "Decline transaction, retain card"}},
            {"43", new Value{Description = "Stolen card", Action = "Decline transaction, retain card"}},
            {"44", new Value{Description = "No Investment account", Action = "Decline transaction"}},
            {"51", new Value{Description = "Not sufficient funds", Action = "Decline transaction"}},
            {"52", new Value{Description = "No Cheque account", Action = "Account requested not attached -declined"}},
            {"53", new Value{Description = "No Savings account", Action = "Account requested not attached -declined"}},
            {"54", new Value{Description = "Expired card", Action = "Decline transaction"}},
            {"55", new Value{Description = "Invalid PIN", Action = "Decline transaction, Request PIN again"}},
            {"56", new Value{Description = "No card record", Action = "Decline transaction"}},
            {"57", new Value{Description = "Transaction not permitted to Cardholder", Action = "Decline transaction"}},
            {"58", new Value{Description = "Transaction not permitted to terminal", Action = "Decline transaction"}},
            {"61", new Value{Description = "Exceeds withdrawal amount limits", Action = "Decline transaction"}},
            {"64", new Value{Description = "Original amount incorrect", Action = "Decline transaction"}},
            {"65", new Value{Description = "Exceeds Withdrawal Frequency Limit", Action = "Decline transaction"}},
            {"67", new Value{Description = "Hot Card", Action = "Decline transaction, retain card"}},
            {"91", new Value{Description = "Issuer not available", Action = "Decline transaction"}},
            {"92", new Value{Description = "Financial Institution/Intermediate network not found for routing.", Action = "Decline transaction"}},
            {"94", new Value{Description = "Duplicate transmission", Action = "Decline transaction"}},
            {"95", new Value{Description = "Reconcile error", Action = ""}},
            {"96", new Value{Description = "System malfunction", Action = "Decline transaction"}},
            {"97", new Value{Description = "Settlement date advanced by 1 and totals reset. Accompanied by ‘1’ totals in balance or ‘2’(totals out of balance) in Bit 66 settlement _code", Action = "Complete - approved transaction"}},
            {"98", new Value{Description = "MAC error", Action = "Decline transaction. Request Key change"}},
        };

        public static string GetRespDescription(string respCode)
        {
            // Try to get the result in the static Dictionary
            lock (Responses)
            {
                Value result;
                if (Responses.TryGetValue(respCode, out result))
                {
                    return result.Description;
                }
                else
                {
                    return "Undefined";
                }
            }
        }

        public static string GetRespAction(string respCode)
        {
            // Try to get the result in the static Dictionary
            lock (Responses)
            {
                Value result;
                if (Responses.TryGetValue(respCode, out result))
                {
                    return result.Action;
                }
                else
                {
                    return "Undefined";
                }
            }
        }

        public struct Value
        {
            public string Description;
            public string Action;
        }
    }
}
