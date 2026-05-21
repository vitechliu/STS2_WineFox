using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using STS2_WineFox.Combat.Magic;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Powers
{
    [RegisterPower]
    public class ChantPower : WineFoxPower, IMagicDamageModifier
    {
        private bool _gainedThisTurn = true;

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

        public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (target == Owner && amount > 0m)
                _gainedThisTurn = true;

            return Task.CompletedTask;
        }

        public override Task BeforeSideTurnStart(
            PlayerChoiceContext choiceContext,
            CombatSide side,
            ICombatState combatState)
        {
            if (side == Owner.Side)
                _gainedThisTurn = false;

            return Task.CompletedTask;
        }

        public override Task AfterPowerAmountChanged(
            PlayerChoiceContext choiceContext,
            PowerModel power,
            decimal amount,
            Creature? applier,
            CardModel? cardSource)
        {
            if (power == this && amount > 0m)
                _gainedThisTurn = true;

            return Task.CompletedTask;
        }

        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side != Owner.Side) return;


            if (_gainedThisTurn)
                return;

            await PowerCmd.Remove(this);
        }
    }
}
