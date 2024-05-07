using System;
using System.Collections.Generic;
using System.Linq;
using DiaryWarning.Settings;
using Photon.Pun;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DiaryWarning.Entries;

public interface IDiaryEntry
{
    public ContentEvent GetContentEvent();
    
    public string GetTitle();
    public string GetLore();
    public IEnumerable<string> GetAbilities();

    public int GetPossibleViews()
    {
        return BigNumbers.GetScoreToViews(GetContentEvent().GetContentValue(), GameAPI.CurrentDay + 1);
    }
    
    public string GetDescription() => (DiaryWarningSettings.ShowLore ? $"{GetLore()}\n\n" : "") +
                                      $"<b>Abilities:</b>\n<margin-left=0.5em>{string.Join("\n", GetAbilities().Select(ability => $"- <indent=1.5em>{ability}</indent>"))}</margin>" +
                                      $"\n\n<b>(You can get <i>{GetPossibleViews()}</i> possible views from recording this monster!)</b>";
}