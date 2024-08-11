using _Scripts.Units;
using TMPro;

namespace _Scripts.UI
{
    public class SelectedUnitView : View
    {
        public TMP_Text FractionText;
        public TMP_Text HPText;
        public TMP_Text CanMoveText;
        public TMP_Text CanAttackText;
        public TMP_Text DmgText;
        public TMP_Text AttackRangeText;
        public TMP_Text MoveRange;

        public override void Init()
        {
            base.Init();
            var observer = GameController.Instance.gameplayRefsHolder.Observer;
            observer.OnUnitSelected += UpdateUnitText;
            observer.OnEmptyTileSelected += OnEmptyTileSelected;
        }

        private void UpdateUnitText(Unit unit)
        {
            Show();
            FractionText.text = $"Fraction {unit.Fraction.ToString()}";
            HPText.text = $"HP:{unit.Statistics.MaxHp}/{unit.CurrentHp}";
            CanMoveText.text = unit.CanPerformMove() ? "Can move: yes" : "Can move: no";
            CanAttackText.text = unit.CanPerformAttack() ? "Can attack: yes" : "Can attack: no";
            DmgText.text = $"DMG: {unit.Statistics.Dmg}";
            AttackRangeText.text = $"Attack Range: {unit.Statistics.AttackRange}";
            MoveRange.text = $"Move range: {unit.Statistics.MoveRange}";
        }

        private void OnEmptyTileSelected(Tile tile)
        {
            Hide();
        }

        public override void Deinit()
        {
            var observer = GameController.Instance.gameplayRefsHolder.Observer;
            observer.OnUnitSelected -= UpdateUnitText;
        }
    }
}
