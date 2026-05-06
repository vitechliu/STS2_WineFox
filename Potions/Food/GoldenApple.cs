using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2_WineFox.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Potions
{
    [RegisterPotion(typeof(WineFoxFoodPotionPool))]
    public sealed class GoldenApple : SellableToMerchantPotionModel
    {
        protected override int SellGold => 64;
        public override PotionRarity Rarity => PotionRarity.Rare;
        protected override TargetType CombatTargetType => TargetType.Self;
        public override bool CanBeGeneratedInCombat => false;

        public override PotionAssetProfile AssetProfile => Art(Const.Paths.GoldenApple);

        protected override async Task OnUseInCombat(PlayerChoiceContext choiceContext, Creature? target)
        {
            await CreatureCmd.Heal(Owner.Creature, 10);
            await PowerCmd.Apply<RegenPower>(choiceContext, Owner.Creature, 6, Owner.Creature, cardSource: null);
            await PlayerCmd.GainGold(64, Owner);
        }
    }
}

