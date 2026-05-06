using Godot;
using STS2RitsuLib.Scaffolding.Content;

namespace STS2_WineFox.Character
{
    // 药水池子
    public class WineFoxPotionPool : TypeListPotionPoolModel
    {
        public override string EnergyColorName => Const.EnergyColorName;
        public override string? BigEnergyIconPath => Const.Paths.EnergyIconCake;
        public override string? TextEnergyIconPath => Const.Paths.EnergyIconCake;
        public override Color LabOutlineColor => WineFox.Color;

        public static PotionAssetProfile Art(string imagePath, string? outlinePath = null)
        {
            return new(imagePath, outlinePath ?? imagePath);
        }
    }
}
