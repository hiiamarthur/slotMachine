using Blazesoft.SlotMachine.Domain.Common;
using Blazesoft.SlotMachine.Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazesoft.SlotMachine.Domain.Interfaces
{
    public interface ISpinWheelFactory
    {
        SpinWheel Create();
    }

    public class SpinWheelFactory : ISpinWheelFactory
    {
        private readonly IOptionsMonitor<MatrixSize> _options;
        public SpinWheelFactory(IOptionsMonitor<MatrixSize> options)
        {
            _options = options;
        }
        public SpinWheel Create()
        {
            return new SpinWheel(_options);
        }
    }
}
