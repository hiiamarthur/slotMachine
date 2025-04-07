using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Blazesoft.SlotMachine.Common.Data
{
    public class Player: BaseMongoModel
    {

        [BsonElement("name")]
        public string Name { get; private set; }

        [BsonElement("balance")]
        public decimal Balance { get; private set; }

        public Player(string name, decimal initialBalance)
        {
            Name = name;
            Balance = initialBalance;
        }
        public Player() {
            Name = "";
            Balance = 0;
        }
        public bool EnoughBalance(decimal amount) => Balance >= amount;
        public decimal Debit(decimal amount) => !EnoughBalance(amount) ? throw new InvalidOperationException("Insufficient balance.") : Balance -= amount;
        public decimal Credit(decimal amount) => Balance += amount;
    }
}
