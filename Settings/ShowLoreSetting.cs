using ContentSettings.API.Attributes;
using ContentSettings.API.Settings;

namespace DiaryWarning.Settings;

[SettingRegister("DiaryWarning", "Toggleables")]
public class ShowLoreSetting : BoolSetting, ICustomSetting
{
    protected override bool GetDefaultValue() => true;

    public override void ApplyValue() => DiaryWarningSettings.ShowLore = Value;

    public string GetDisplayName() => "Show Lore in Diary Entries";
}