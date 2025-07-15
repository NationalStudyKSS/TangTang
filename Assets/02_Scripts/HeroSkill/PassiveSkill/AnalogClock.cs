public class AnalogClock : PassiveSkill
{
    protected override void Apply()
    {
        if (_heroModel == null)
            return;

        _heroModel.SetStat(_statName, StatType.Stage, _bonusValue);
    }
    private void OnValidate()
    {
        _passiveSkillType = PassiveSkillType.AnalogClock;
    }
}