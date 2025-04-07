using Blazesoft.SlotMachine.Common.Data;
using Blazesoft.SlotMachine.Domain.ValueObjects;

namespace Blazesoft.SlotMachine.Domain.Interfaces
{
    public interface ISpinToWin
    {
        (Player player, SpinResult result, decimal winAmount) Spin(Player player, ISpinWheel wheel, decimal betAmount);
        Player UpdateBalance(Player player,decimal amount);
    }
}
