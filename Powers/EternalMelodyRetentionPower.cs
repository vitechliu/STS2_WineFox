using MegaCrit.Sts2.Core.Combat;
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
        private bool _isActiveTurn;

        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override PowerAssetProfile AssetProfile => Icons(Const.Paths.EternalMelodyPowerIcon);

        public override Task BeforeSideTurnStart(
            PlayerChoiceContext choiceContext,
            CombatSide side,
            ICombatState combatState)
        {
            if (side == Owner.Side && Amount > 0m)
                _isActiveTurn = true;

            return Task.CompletedTask;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (!_isActiveTurn) return;
            var card = cardPlay.Card;
            if (card.Owner.Creature != Owner) return;
            if (!card.IsMagic()) return;

            Flash();
            await PowerCmd.Apply<ChantPower>(new ThrowingPlayerChoiceContext(), Owner, 1m, Owner, card);
        }

        public override async Task AfterTurnEndLate(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side != Owner.Side || !_isActiveTurn)
                return;

            _isActiveTurn = false;
            await PowerCmd.Decrement(this);
        }
    }
}
