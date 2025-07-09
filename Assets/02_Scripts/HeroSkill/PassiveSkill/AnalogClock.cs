public class AnalogClock : PassiveSkill
{
    public override PassiveSkillStatType PassiveSkillType => PassiveSkillStatType.ItemGetRange;

    protected override void Apply()
    {
        if (_heroModel == null)
            return;

        float currentBonus = _heroModel.ItemGetRange.Bonus;
        float newBonus = currentBonus - _previousBonusValue + _bonusValue;

        _heroModel.ItemGetRange.SetBonus(newBonus);
    }
}
