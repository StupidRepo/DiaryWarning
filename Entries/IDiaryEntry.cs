using System.Collections.Generic;
using System.Linq;

namespace DiaryWarning.Entries;

public interface IDiaryEntry
{
    public string GetTitle();
    public string GetLore();
    public IEnumerable<string> GetAbilities();

    public int GetPossibleViews();
    
    public string GetDescription() => $"{GetLore()}" +
                                      $"\n\n<b>Abilities:</b>\n<margin-left=1em>{string.Join("\n", GetAbilities().Select(ability => $"\\u2022<indent=3em>{ability}</indent>"))}</margin-left>" +
                                      $"\n\n<b>(You can get <i>{GetPossibleViews()}</i> possible views from recording this monster!)</b>";
}