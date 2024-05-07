using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DiaryWarning.Entries;

public interface IDiaryEntry
{
    public Type GetContentProviderType();
    public string GetTitle();
    public string GetLore();
    public IEnumerable<string> GetAbilities();

    public int GetPossibleViews()
    {
        List<ContentEventFrame> frames = [];
        var comp = Activator.CreateInstance(GetContentProviderType());
        ((MonsterContentProvider)comp).GetContent(frames, 1f, ContentPolling.m_currentPollingCamera, 1f);
        return BigNumbers.GetScoreToViews(frames.First().GetScore(), GameAPI.CurrentDay + 1);
    }
    
    public string GetDescription() => $"{GetLore()}" +
                                      $"\n\n<b>Abilities:</b>\n<margin-left=1em>{string.Join("\n", GetAbilities().Select(ability => $"\\u2022<indent=3em>{ability}</indent>"))}</margin-left>" +
                                      $"\n\n<b>(You can get <i>{GetPossibleViews()}</i> possible views from recording this monster!)</b>";
}