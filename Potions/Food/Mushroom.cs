using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using STS2_WineFox.Character;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Potions
{
    [RegisterPotion(typeof(WineFoxFoodPotionPool))]
    public sealed class Mushroom : SellableToMerchantPotionModel, ICampfireTransformPotion
    {
        protected override int SellGold => 5;
        public override PotionRarity Rarity => PotionRarity.Common;
        protected override TargetType CombatTargetType => TargetType.Self;
        public override bool CanBeGeneratedInCombat => false;

        public override PotionAssetProfile AssetProfile => Art(Const.Paths.Mushroom);

        public PotionModel CampfireTransformResult => ModelDb.Potion<MushroomStew>();
        
        protected override async Task OnUseInCombat(PlayerChoiceContext choiceContext, Creature? target)
        {
            await PowerCmd.Apply<PoisonPower>(Owner.Creature, 1, Owner.Creature, null);
            await PowerCmd.Apply<DexterityPower>(Owner.Creature, 1, Owner.Creature, null);
        }

        protected override Task OnUseOutOfCombat(PlayerChoiceContext choiceContext) => Task.CompletedTask;
    }
}

