using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using STS2_WineFox.Character;
using STS2_WineFox.Powers;
using STS2RitsuLib.Interop.AutoRegistration;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Cards.Uncommon
{
    [RegisterCard(typeof(WineFoxCardPool))]
    public class ManaSurge() : WineFoxCard(
        1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            WineFoxCardVarFactory.PowerAmountVar<ChantPower>(3m),
            new CardsVar(2),
        ];

        protected override IEnumerable<IHoverTip> AdditionalHoverTips =>
            [HoverTipFactory.FromPower<ChantPower>()];

        [Obsolete]
        protected override IEnumerable<string> RegisteredKeywordIds => [WineFoxKeywords.Magic];

        public override CardAssetProfile AssetProfile => Art(Const.Paths.CardManaSurge);

        protected override async Task OnPlay(
            PlayerChoiceContext choiceContext,
            CardPlay play)
        {
            var owner = Owner;
            var creature = owner.Creature;

            await PowerCmd.Apply<ChantPower>(choiceContext, creature, DynamicVars["ChantPower"].BaseValue, creature, this);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ChantPower"].UpgradeValueBy(1m);
        }
    }
}

