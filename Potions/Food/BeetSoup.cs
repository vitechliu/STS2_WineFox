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
    public sealed class BeetSoup : SellableToMerchantPotionModel
    {
        protected override int SellGold => 18;
        public override PotionRarity Rarity => PotionRarity.Uncommon;
        protected override TargetType CombatTargetType => TargetType.Self;
        public override bool CanBeGeneratedInCombat => false;

        public override PotionAssetProfile AssetProfile => Art(Const.Paths.BeetSoup);

        protected override async Task OnUseInCombat(PlayerChoiceContext choiceContext, Creature? target)
        {
            await CreatureCmd.Heal(Owner.Creature, 6);
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, 2, Owner.Creature, cardSource: null);
        }
    }
}

