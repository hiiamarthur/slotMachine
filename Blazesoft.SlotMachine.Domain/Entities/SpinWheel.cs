using Blazesoft.SlotMachine.Domain.Common;
using Blazesoft.SlotMachine.Domain.Interfaces;
using Blazesoft.SlotMachine.Domain.ValueObjects;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Blazesoft.SlotMachine.Domain.Entities
{
    public class SpinWheel : IAggreagateRoot, ISpinWheel
    {
        private readonly int MAX_REELS_NO = 9;

        public SpinWheel( IOptionsMonitor<MatrixSize> config)
        {
            SetHeight(config.CurrentValue.Height);
            SetWidth(config.CurrentValue.Width);
            Reels = [.. Enumerable.Range(0, 9)];
        }

        public int[] Reels { get; private set; }
        public int Width { get; private set; }
        public int Height {get; private set;}

        public SpinResult Spin()
        {
            var ran = Random.Shared;
            var result = new int[Width, Height];

            for (int row = 0; row < Width; row++)
            {
                for (int col = 0; col < Height; col++)
                {
                    result[row, col] = Reels[ran.Next(0, MAX_REELS_NO)];
                }
            }

            return new SpinResult(result);
        }
        public int SetWidth(int width) => Width = width;
        public int SetHeight(int height) => height > MAX_REELS_NO ? throw new InvalidDataException("MAXIMUM REELS NO EXCEED") : Height = height;

    }
}
