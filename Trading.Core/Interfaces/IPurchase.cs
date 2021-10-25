using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Interfaces
{
    public interface IPurchase
    {
        public bool BuyCrypto(CurrencyType Crypto, string Amount, CurrencyType FromCurrencyType);
      
        public bool SellCrypto(CurrencyType CryptoType, string Amount, CurrencyType FromCurrencyType);

        public bool SendCrypto(CurrencyType CryptoType, string SendToAccount, string Amount, string Note = null);
    }
}
