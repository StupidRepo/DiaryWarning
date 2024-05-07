using System;
using System.Collections.Generic;
using System.Linq;
using DiaryWarning.Settings;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DiaryWarning.Entries;

public interface IDiaryEntry<TDiaryProvider> where TDiaryProvider : MonsterContentProvider, new()
{
    public string GetTitle();
    public string GetLore();
    public IEnumerable<string> GetAbilities();

    public int GetPossibleViews()
    {
        List<ContentEventFrame> frames = [];
        var tempGo = new GameObject();
        
        var comp = tempGo.AddComponent<TDiaryProvider>();
        comp.GetContent(frames, 1f, ContentPolling.m_currentPollingCamera, 1f);
        Object.DestroyImmediate(tempGo);
        
        DiaryWarningMod.Logger.LogWarning(comp.GetType());
        
        return BigNumbers.GetScoreToViews(frames.First().GetScore(), GameAPI.CurrentDay + 1);
    }
    
    public string GetDescription() => (DiaryWarningSettings.ShowLore ? $"{GetLore()}\n\n" : "") +
                                      $"<b>Abilities:</b>\n<margin-left=0.5em>{string.Join("\n", GetAbilities().Select(ability => $"- <indent=1.5em>{ability}</indent>"))}</margin>" +
                                      $"\n\n<b>(You can get <i>{GetPossibleViews()}</i> possible views from recording this monster!)</b>";
}