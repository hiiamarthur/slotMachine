using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazesoft.SlotMachine.Common.Enums
{
    public enum StateAction
    {
        Authorize,
        StartGame,
        DeliverReceipt,
        AdminOperation,
        Back,
        Cancel,
        Error,
        Reset
    }
}
