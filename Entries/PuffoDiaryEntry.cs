using System;
using System.Collections.Generic;
using System.Linq;

namespace DiaryWarning.Entries;

public class PuffoDiaryEntry : IDiaryEntry
{
    public ContentEvent GetContentEvent() => new PuffoContentEvent();

    public string GetTitle() => "Puffo";

    public string GetLore() =>
        "Yeah, Puffos are quite interesting. But then again, all the things we find down there are interesting, aren't they? Anyway, Puffos are kinda like... pufferfish. I think that's what they used to call them. 'Pufferfish'. " +
        "We lost one of our own to a Puffo once. Cornered by 2 of 'em, and bounced around like a pinball. Hilarious it was, until we realised he weren't moving no more. " +
        "They opened him up, and found that his insides were all mushed up. Poor guy. But hey, that's the life of a SpookTuber, ain't it? Since then, we've assumed Puffos emit some kind of shockwave.";

    public IEnumerable<string> GetAbilities() =>
    [
        "Inflates when you get close to it.",
        "Upon inflating, a Puffo releases a shockwave that pushes you back and deals up to <color=red>25</color> damage."
    ];
}