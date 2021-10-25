using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Interfaces
{
    public interface IAccount
    {
        public IEnumerable<string> GetAccounts();
    }
}
