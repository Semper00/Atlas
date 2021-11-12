using InventorySystem.Items.Coin;

using Atlas.Enums;
using Atlas.Entities.Items.Base;

namespace Atlas.Entities.Items
{
    /// <summary>
    /// Represents the in-game coin inventory item.
    /// </summary>
    public class Coin : BaseItem
    {
        internal InventorySystem.Items.Coin.Coin coin;

        public Coin(InventorySystem.Items.Coin.Coin coin, bool addToApi = false) : base(coin, addToApi)
            => this.coin= coin;

        /// <summary>
        /// Flips the coin.
        /// </summary>
        public CoinResult Flip(CoinResult? predefined = null)
        {
            if (Base.Owner != null)
            {
                CoinResult result = predefined.HasValue ? predefined.Value : GetRandomResult();

                CoinNetworkHandler.ServerProcessMessage(Base.Owner?.characterClassManager?.Connection, 
                    new CoinNetworkHandler.CoinFlipMessage(Serial, result == CoinResult.Tails));

                return result;
            }

            return CoinResult.Head;
        }

        /// <summary>
        /// Gets a random coin result.
        /// </summary>
        /// <returns>The random coin result.</returns>
        public CoinResult GetRandomCoin()
            => (CoinResult)Server.Random.Next(0, 1);

        /// <summary>
        /// Gets a random coin result.
        /// </summary>
        /// <returns>The random coin result.</returns>
        public static CoinResult GetRandomResult()
            => (CoinResult)Server.Random.Next(0, 1);
    }
}