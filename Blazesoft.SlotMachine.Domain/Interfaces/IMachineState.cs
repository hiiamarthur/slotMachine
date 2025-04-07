using Blazesoft.SlotMachine.Common.Enums;
using Blazesoft.SlotMachine.Domain.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Blazesoft.SlotMachine.Domain.Interfaces
{
    public interface IMachineState
    {
        public MachineStatus Status { get; set; }
        Task EnterState(SpinToWin context);
        Task ExitState(SpinToWin context);
        Task<T?> PerformAction<T>(SpinToWin context, MachineStatus status);
        Task PerformAction(SpinToWin context, MachineStatus status);
        MachineStatus ValidateTransition(SpinToWin context, StateAction action);
    }

}
