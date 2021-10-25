using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Interfaces
{
    public interface IConfirm
    {
        public bool Confirm(string passcode);
    }
}
