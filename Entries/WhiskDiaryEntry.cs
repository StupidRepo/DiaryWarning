using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zorro.Core;
using Object = UnityEngine.Object;

namespace DiaryWarning.Entries;

public class WhiskDiaryEntry : IDiaryEntry
{
    public Type GetContentProviderType() => typeof(ToolkitContentProvider);

    public string GetTitle() => "Whisk";
    public string GetLore() =>
        "See, this kind of species is NOT to be messed with. They're like... the worst thing to find when you're exploring down there. So, if you see one, run the f**k away. And fast. " +
        "Here's the story. Me and some other guys had a close encounter with one, and I swear, I ain't ever been so scared in my life up until I saw one of these Whisk things. I was sent down there with these 2 other random people and a camera. Recording footage, y'know, like they tell you to do, and that's when I saw it. It was just standing there, starin' at me. " +
        "I actually thought it was harmless, but we all know that anything that moves is most likely dangerous. Including our own teams. Anyway, it started runnin' at me with INSANE speed! " +
        "Like, you can't even define the word fast until you meet one of these Whisk guys. So, I ran but I just couldn't outrun it. It caught up to me, and I could already see my life flashing before my eyes. " +
        "But then... it just stopped? It looked at me for a good couple seconds, and then it turned around and ran away. I don't know why, but I'm glad it did. I don't think I would've survived if it hadn't turned back around. " +
        "This was the only registered case of one of these things NOT attacking a human. They still don't know what changed its mind, if these monsters even have minds, but one thing's for sure: " +
        "I'm never going near one of those things again.";
    
    public IEnumerable<string> GetAbilities() =>
    [
        "If it spots a player, it will turn towards them. After a few seconds, it will start running in their direction.",
        "If it hits a player, it will deal <color=red>95</color> damage.",
        "It doesn't stop running until it hits a wall, where it will fall over for a few seconds, get up, and then try to find another player to mix.",
        "There have been cases where Whisks just run off and hide, instead of attacking players."
    ];
}