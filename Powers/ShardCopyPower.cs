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
    public class ShardCopyPower : WineFoxPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override PowerAssetProfile AssetProfile => Icons(Const.Paths.ShardCopyPowerIcon);

        public override async Task AfterCardPlayedLate(PlayerChoiceContext context, CardPlay cardPlay)
        {
            var card = cardPlay.Card;
            if (card.Owner.Creature != Owner) return;
            if (!card.IsMagic()) return;
            if (Amount <= 0m) return;

            Flash();
            await PowerCmd.Apply<ChantPower>(context, Owner, Amount, Owner, card);
        }
    }
}

