using Blazesoft.SlotMachine.Common.Data;
using Blazesoft.SlotMachine.Domain.Interfaces;
using Blazesoft.SlotMachine.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;

namespace Blazesoft.SlotMachine.Domain.BusinessObjects
{
    public class SpinToWin: ISpinToWin
    {
        private readonly IMemoryCache _memoryCache;

        private const String _PLAYER_CACHE_KEY = "PLAYER_CACHE";
        public SpinToWin(IMemoryCache memoryCache) {
            _memoryCache = memoryCache;
        }

        public bool TryGetFromCache<T>(string key, out T? value) =>
    (_memoryCache.TryGetValue(key, out var cached) && cached is T t) ? (value = t) != null : (value = default) == null;
        protected T SaveToCache<T>(string key, T value) => _memoryCache.Set(key, value);

        public Player GetOrSetPlayer(Player player) {
            if (TryGetFromCache(_PLAYER_CACHE_KEY, out Player? cachedPlayer) && cachedPlayer?.Id == player.Id)
            {
                return cachedPlayer!;
            }
            else {
                SaveToCache(_PLAYER_CACHE_KEY, player);
                return player;
            }
                
        }
        public Player UpdateBalance(Player player,decimal amount)
        {
            Player activePlayer = GetOrSetPlayer(player);
            activePlayer.Credit(amount);
            return activePlayer;

        }

        public (Player player,SpinResult result, decimal winAmount) Spin(Player player, ISpinWheel wheel, decimal betAmount)
        {
            Player activePlayer = GetOrSetPlayer(player);
            if (!activePlayer.EnoughBalance(betAmount))
                throw new InvalidOperationException("Player does not have enough balance");

            activePlayer.Debit(betAmount);
            var result = wheel.Spin();
            var winAmount = result.CalculateTotalWin() * betAmount;
            activePlayer.Credit(winAmount);

            return (activePlayer, result, winAmount);
        }
    }
}
