using Blazesoft.SlotMachine.Api.Models;
using Blazesoft.SlotMachine.Common;
using Blazesoft.SlotMachine.Common.Data;
using Blazesoft.SlotMachine.Common.Interfaces;
using Blazesoft.SlotMachine.Common.Types;
using Blazesoft.SlotMachine.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SlotMachine.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]

    [ApiVersion("1.0")]
    public class MachineController : ControllerBase
    {
        private readonly ILogger<MachineController> _logger;
        private readonly ISpinToWin _spinToWin;
        private readonly IBaseRepository<Player> _player;
        private readonly ISpinWheelFactory _spinWheelFactory;

        public MachineController(ILogger<MachineController> logger,ISpinToWin spinToWin,IBaseRepository<Player> Player, ISpinWheelFactory spinWheelFactory)
        {
            _logger = logger;
            _player = Player;
            _spinWheelFactory = spinWheelFactory;
            _spinToWin = spinToWin;
        }

        [HttpPost]
        [Route("spin")]
        public async Task<ActionResult<SpinResponse>> Spin([FromBody] SpinRequest requestBase) {
            try
            {
                RepoResponse<Player?> response = await _player.GetFirstOrDefaultAsync();
                Player? player = response.Value;
                if (player != null) {
                    var spinResult = _spinToWin.Spin(player, _spinWheelFactory.Create(), requestBase.BetAmount);
                    var result = await _player.UpdateAsync(spinResult.player);
                    return Ok(new SpinResponse{ 
                        Matrix = spinResult.result.Matrix.ToJagged(),
                        WinAmount = spinResult.winAmount,
                        Balance = spinResult.player.Balance
                    });
                } 
                else return NotFound("Player Not Found");
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("update-balance")]
        public async Task<ActionResult<UpdateBalanceResponse>> UpdateBalance([FromBody] UpdateBalanceRequest requestBase) {
            try
            {
                RepoResponse<Player?> response = await _player.GetFirstOrDefaultAsync();
                
                if (response.Value != null) {
                   var result =  _spinToWin.UpdateBalance(response.Value, requestBase.CreditAmount);
                    return Ok(new UpdateBalanceResponse(){ 
                    Balance = result.Balance,
                    });
                }
                else return NotFound("Player Not Found"); 
            }
            catch (Exception e) { 
            return BadRequest($"{e.Message}");
            }
        }

        [HttpPost]
        [Route("player")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Player>> AddPlayer([FromBody] PlayerRequest request)
        {
            try
            {
                // Example code to create a new player, assume you have a CreatePlayerRequest model
                var player = new Player(request.Name, request.Balance);

                RepoResponse<Player> response = await _player.AddAsync(player);

                if (response.IsSuccess)
                {
                    return Ok(response.Value);
                }

                return BadRequest("Failed to create player");
            }
            catch (Exception e)
            {
                return BadRequest($"Error: {e.Message}");
            }
        }

    }
}
