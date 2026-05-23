using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2_WineFox.Cards;
using STS2_WineFox.Combat.Magic;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Powers
{
    [RegisterPower]
    public class ChantPower : WineFoxPower, IMagicDamageModifier, IMagicBlockModifier
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override bool AllowNegative => true;
        public override PowerAssetProfile AssetProfile => Icons(Const.Paths.ChantPowerIcon);

        public decimal ModifyMagicDamageAdditive(
            Creature? target,
            decimal baseAmount,
            Creature dealer,
            CardModel cardSource)
        {
            if (dealer != Owner) return 0m;

            return Amount;
        }

        public decimal ModifyMagicBlockAdditive(
            Creature defender,
            decimal baseAmount,
            CardModel cardSource)
        {
            if (defender != Owner) return 0m;
            if (!cardSource.IsMagic()) return 0m;

            return Amount;
        }

        public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
        {
            return Task.CompletedTask;
        }

        public override Task BeforeSideTurnStart(
            PlayerChoiceContext choiceContext,
            CombatSide side,
            IReadOnlyList<Creature> participants,
            ICombatState combatState)
        {
            return Task.CompletedTask;
        }

        public override Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amount,
            Creature? applier,
            CardModel? cardSource)
        {
            return Task.CompletedTask;
        }

        public override async Task AfterSideTurnEnd(
            PlayerChoiceContext choiceContext,
            CombatSide side,
            IEnumerable<Creature> participants)
        {
            if (side != Owner.Side) return;

            var halved = Math.Floor(Amount / 2m);
            if (halved == Amount)
                return;

            if (halved == 0m)
            {
                await PowerCmd.Remove(this);
                return;
            }

            await PowerCmd.Apply<ChantPower>(choiceContext, Owner, halved - Amount, Owner, null);
        }
    }
}
