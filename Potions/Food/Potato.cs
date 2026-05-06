using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using STS2_WineFox.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Potions
{
    [RegisterPotion(typeof(WineFoxFoodPotionPool))]
    public sealed class Potato : SellableToMerchantPotionModel
    {
        protected override int SellGold => 10;
        public override PotionRarity Rarity => PotionRarity.Common;
        protected override TargetType CombatTargetType => TargetType.Self;
        public override bool CanBeGeneratedInCombat => false;

        public override PotionAssetProfile AssetProfile => Art(Const.Paths.Potato);

        protected override async Task OnUseInCombat(PlayerChoiceContext choiceContext, Creature? target)
        {
            await CreatureCmd.Heal(Owner.Creature, 2);
            await CreatureCmd.GainBlock(Owner.Creature, 3, ValueProp.Unpowered, cardPlay: null);
        }
    }
}

