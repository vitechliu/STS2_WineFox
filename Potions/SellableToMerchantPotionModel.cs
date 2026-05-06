using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Potions
{
    public abstract class SellableToMerchantPotionModel : PotionModel, IModPotionAssetOverrides
    {
        protected abstract int SellGold { get; }

        public override PotionUsage Usage => PotionUsage.AnyTime;

        public override TargetType TargetType =>
            CombatManager.Instance.IsInProgress ? CombatTargetType : TargetType.TargetedNoCreature;

        protected virtual TargetType CombatTargetType => TargetType.Self;

        public override bool PassesCustomUsabilityCheck =>
            CombatManager.Instance.IsInProgress || Owner.RunState.CurrentRoom is MerchantRoom;

        protected sealed override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            if (CombatManager.Instance.IsInProgress)
            {
                await OnUseInCombat(choiceContext, target);
                return;
            }

            if (Owner.RunState.CurrentRoom is MerchantRoom)
            {
                ShowPotionVfx(NRun.Instance?.MerchantRoom?.MerchantButton);
                await PlayerCmd.GainGold(SellGold, Owner);
            }
        }

        public virtual PotionAssetProfile AssetProfile => PotionAssetProfile.Empty;
        public virtual string? CustomImagePath => AssetProfile.ImagePath;
        public virtual string? CustomOutlinePath => AssetProfile.OutlinePath;

        protected static PotionAssetProfile Art(string imagePath, string? outlinePath = null)
        {
            return new(imagePath, outlinePath ?? imagePath);
        }

        protected abstract Task OnUseInCombat(PlayerChoiceContext choiceContext, Creature? target);

        private static void ShowPotionVfx(NMerchantButton? merchantButton)
        {
            if (TestMode.IsOn || merchantButton == null)
                return;

            var scenePath = SceneHelper.GetScenePath("vfx/vfx_slime_impact");
            var vfx = PreloadManager.Cache.GetScene(scenePath).Instantiate<Node2D>();
            merchantButton.GetParent().AddChildSafely(vfx);
            vfx.GlobalPosition = merchantButton.GlobalPosition;
        }
    }
}
