namespace Blazesoft.SlotMachine.Api.Models
{
    public class SpinResponse
    {
        public int[][]? Matrix { get; set; }
        public decimal WinAmount { get; set; }
        public decimal Balance { get; set; }
    }
}
