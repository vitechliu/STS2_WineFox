using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2_WineFox.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Potions
{
    [RegisterPotion(typeof(WineFoxFoodPotionPool))]
    public sealed class Wheat : SellableToMerchantPotionModel
    {
        protected override int SellGold => 8;
        public override PotionRarity Rarity => PotionRarity.Common;
        protected override TargetType CombatTargetType => TargetType.Self;
        public override bool CanBeGeneratedInCombat => false;

        public override PotionAssetProfile AssetProfile => Art(Const.Paths.Wheat);

        protected override Task OnUseInCombat(PlayerChoiceContext choiceContext, Creature? target) =>
            Task.CompletedTask;

        protected override Task OnUseOutOfCombat(PlayerChoiceContext choiceContext) => Task.CompletedTask;
    }
}

