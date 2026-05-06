using MegaCrit.Sts2.Core.Entities.Ascension;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using STS2_WineFox.Potions;
using STS2RitsuLib.Utils;
using STS2RitsuLib.Patching.Models;

namespace STS2_WineFox.Patches
{
    public sealed class WineFoxFoodPotionRewardPatch : IPatchMethod
    {
        public static string PatchId => "winefox_food_potion_reward_roll";
        public static bool IsCritical => true;
        public static string Description => "Adds separate food potion drop roll";

        private const float BaseOdds = 0.4f;
        private const float TargetOdds = 0.5f;
        private const float EliteBonus = 0.25f;
        private const float Step = 0.1f;

        // SavedAttachedState doesn't support float, so store as basis points (0..10000).
        private static readonly SavedAttachedState<Player, int> FoodOddsBasisPoints =
            new("winefox_food_potion_reward_odds_bp", defaultValueFactory: () => (int)(BaseOdds * 10000));

        public static ModPatchTarget[] GetTargets()
        {
            return [new(typeof(RewardsSet), nameof(RewardsSet.WithRewardsFromRoom))];
        }

        public static void Postfix(RewardsSet __instance, AbstractRoom room)
        {
            if (room is not CombatRoom combatRoom)
                return;

            var player = __instance.Player;
            if (player?.RunState == null)
                return;

            // Shops are not combat rooms, but keep this explicit.
            if (room.RoomType is not (RoomType.Monster or RoomType.Elite or RoomType.Boss))
                return;

            var rng = player.PlayerRng.Rewards;
            var asc = RunManager.Instance?.AscensionManager;

            var currentBp = FoodOddsBasisPoints.GetValueOrDefault(player, (int)(BaseOdds * 10000));
            var current = currentBp / 10000f;
            var roll = rng.NextFloat();

            // Mirror PotionRewardOdds behavior (separate tracker).
            if (roll < current)
                current -= Step;
            else
                current += Step;

            FoodOddsBasisPoints[player] = Math.Clamp((int)MathF.Round(current * 10000f), 0, 10000);

            var bonus = room.RoomType == RoomType.Elite ? EliteBonus : 0f;
            var success = roll < current + bonus * (TargetOdds);

            if (!success)
                return;

            var potion = FoodPotionFactory.CreateRandomFoodPotionForReward(player, rng)?.ToMutable();
            if (potion == null)
                return;

            __instance.Rewards.Add(new PotionReward(potion, player));
        }
    }
}

