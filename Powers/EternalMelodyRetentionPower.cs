using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using STS2_WineFox.Cards;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Powers
{
    [RegisterPower]
    public class EternalMelodyRetentionPower : WineFoxPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.None;
        
        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        public override PowerAssetProfile AssetProfile => Icons(Const.Paths.EternalMelodyPowerIcon);

        public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var card = cardPlay.Card;
            if (card.Owner.Creature != Owner) return;
            if (!card.IsMagic()) return;
            if (Owner.CombatState is not { } combatState) return;
            if (combatState.HittableEnemies.Count == 0) return;

            var target = combatState.RunState.Rng.CombatTargets.NextItem(combatState.HittableEnemies);
            if (target == null) return;

            var threshold = Amount > 0m ? Amount : 2m;
            var chant = Math.Max(0m, Owner.GetPowerAmount<ChantPower>());
            var extraLoss = Math.Floor(chant / threshold) * 2m;
            var totalLoss = 2m + extraLoss;

            Flash();

            // Keep current HP anchored before max HP reduction as requested by design.
            await CreatureCmd.SetCurrentHp(target, target.CurrentHp);
            await CreatureCmd.LoseMaxHp(choiceContext, target, totalLoss, true);
        }
    }
}
