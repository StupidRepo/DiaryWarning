using System.Collections.Generic;
using System.Linq;

namespace DiaryWarning.Entries;

public class PuffoDiaryEntry : IDiaryEntry
{
    public string GetTitle() => "Puffo";

    public string GetLore() =>
        "Puffo? Yeah, an interesting species. But then again, they're all interesting. Aren't they? Anyway, Puffos are kinda like... pufferfish. I think that's what they used to call them. 'Pufferfish'. " +
        "We lost someone from the team to a Puffo once. Cornered by 2; bounced around like a pinball. Hilarious it was, until we realised he weren't moving no more. " +
        "They opened him up, and found that his insides were all mushed up. Poor guy. But hey, that's the life of a researcher, ain't it? Since then, we've assumed Puffos emit some kind of shockwave.";
    public IEnumerable<string> GetAbilities() =>
    [
        "Inflates when you get close to it.",
        "Creates a shockwave, upon inflating, that pushes you back and deals up to <color=red>25</color> damage."
    ];
    
    public int GetPossibleViews()
    {
        List<ContentEventFrame> frames = [];
        DiaryWarningMod.MonsterContentProviders["Puffo"]
            .GetContent(frames, 1f, ContentPolling.m_currentPollingCamera, 1f);
        return BigNumbers.GetScoreToViews(frames.First().GetScore(), GameAPI.CurrentDay + 1);
    }
}