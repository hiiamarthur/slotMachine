using Blazesoft.SlotMachine.Common.Data;
using Blazesoft.SlotMachine.Common.Interfaces;
using Blazesoft.SlotMachine.Common.Types;
using Blazesoft.SlotMachine.Domain.BusinessObjects;
using Blazesoft.SlotMachine.Domain.Common;
using Blazesoft.SlotMachine.Domain.Entities;
using Blazesoft.SlotMachine.Domain.Interfaces;
using Blazesoft.SlotMachine.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;

namespace Blazesoft.SlotMachine.Tests
{
    public class SpinToWinTests
    {
        private readonly IMemoryCache _cache;
        private readonly SpinToWin _spinToWin;
        private readonly Mock<IBaseRepository<Player>> _mockRepo;
        private readonly Mock<IOptionsMonitor<MatrixSize>> _mockOptions;

        public SpinToWinTests()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _spinToWin = new SpinToWin(_cache);
            _mockRepo = new Mock<IBaseRepository<Player>>();
             _mockOptions = new Mock<IOptionsMonitor<MatrixSize>>();
            var matrixSize = new MatrixSize { Width = 5, Height = 3 };

            _mockOptions.Setup(m => m.CurrentValue).Returns(matrixSize);
        }

        [Fact]
        public void UpdateBalance_ShouldCreditPlayer()
        {
            var player = new Player("123", 100)
            {
                Id = ObjectId.GenerateNewId().ToString()
            };
            var updated = _spinToWin.UpdateBalance(player, 50);

            Assert.Equal(150, updated.Balance);
        }

        [Fact]
        public void GetOrSetPlayer_ShouldCachePlayer()
        {
            var player = new Player("abc", 200)
            {
                Id = ObjectId.GenerateNewId().ToString()
            };
            var retrieved = _spinToWin.GetOrSetPlayer(player);
            Assert.Same(player, retrieved);

            var secondCall = _spinToWin.GetOrSetPlayer(player);
            Assert.Same(player, secondCall);
        }

        [Fact]
        public void Spin_ShouldThrowIfInsufficientBalance()
        {
            var player = new Player("lowbal", 5)
            {
                Id = ObjectId.GenerateNewId().ToString()
            };
            var mockWheel = new Mock<ISpinWheel>();
            int[,] matrix = new int[3, 5]
    {
        { 3, 3, 3, 4, 5 },
        { 2, 3, 2, 3, 3 },
        { 1, 2, 3, 3, 3 }
    };

            var spinResult = new SpinResult(matrix);
            mockWheel.Setup(w => w.Spin()).Returns(new SpinResult(matrix));

            Assert.Throws<InvalidOperationException>(() =>
                _spinToWin.Spin(player, mockWheel.Object, 10));
        }

        [Fact]
        public void Spin_ShouldDebitAndCreditCorrectly()
        {
            var player = new Player("spinuser", 100)
            {
                Id = ObjectId.GenerateNewId().ToString()
            };
            int[,] matrix = new int[3, 5]
            {
        { 3, 3, 3, 4, 5 },
        { 2, 3, 2, 3, 3 },
        { 1, 2, 3, 3, 3 }
            };

            var spinResult = new SpinResult(matrix)
            {
                Win = 27
            };
            var matrixSize = new MatrixSize { Width = 5, Height = 3 };
            var mockOptions = new Mock<IOptionsMonitor<MatrixSize>>();
            mockOptions.Setup(m => m.CurrentValue).Returns(matrixSize);
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            memoryCache.Set("player_spinuser", player);
            var mockWheel = new Mock<ISpinWheel>();
            mockWheel.Setup(wheel => wheel.Spin()).Returns(spinResult);
            var spinToWin = new SpinToWin(memoryCache);
            var (updatedPlayer, result, winAmount) = spinToWin.Spin(player, mockWheel.Object, 10);
            Assert.Equal(100 - 10 + 27 * 10, updatedPlayer.Balance);
            Assert.Equal(27 * 10, winAmount);
        }

        [Fact]
        public async Task SpinAndUpdateBalance_ShouldSyncAndPersistToMongo()
        {
            var player = new Player("test-user", 100)
            {
                Id = ObjectId.GenerateNewId().ToString()
            };
            var bet = 10;
            var matrixSize = new MatrixSize { Width = 5, Height = 3 };

            var mockOptions = new Mock<IOptionsMonitor<MatrixSize>>();
            mockOptions.Setup(m => m.CurrentValue).Returns(matrixSize);

            var spinWheel = new SpinWheel(_mockOptions.Object);
            spinWheel.SetHeight(matrixSize.Height);
            spinWheel.SetWidth(matrixSize.Width);

            var matrix = new int[3, 5]
            {
            { 3, 3, 3, 1, 2 },
            { 2, 3, 2, 3, 3 },
            { 1, 2, 3, 3, 3 }
            };

            var spinResult = new SpinResult(matrix)
            {
                Win = 27
            };


            var spinWheelMock = new Mock<ISpinWheel>();
            spinWheelMock.Setup(x => x.Spin()).Returns(spinResult);

            var updatedBalance = 20;
            _spinToWin.UpdateBalance(player, updatedBalance); 
            var (updatedPlayer, result, winAmount) = _spinToWin.Spin(player, spinWheelMock.Object, bet);


            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Player>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(RepoResponse<Player?>.Success(updatedPlayer));

            var saveResult = await _mockRepo.Object.UpdateAsync(updatedPlayer);
            Assert.True(saveResult.IsSuccess);
            Assert.Equal(100 - 10 + 20 + 27 * 10, saveResult.Value!.Balance); 
            Assert.Equal(matrix, result.Matrix);
            Assert.Equal(27 * 10, winAmount);
        }

        [Fact]
        public void SpinWheel_RespectsMatrixSizeConfiguration()
        {
            var mockOptions = new Mock<IOptionsMonitor<MatrixSize>>();
            mockOptions.Setup(o => o.CurrentValue).Returns(new MatrixSize { Width = 4, Height = 3 });

            var spinWheel = new SpinWheel(mockOptions.Object);
            Assert.Equal(4, spinWheel.Width);
            Assert.Equal(3, spinWheel.Height);
        }
    }
}