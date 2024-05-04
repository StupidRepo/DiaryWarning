using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using System.Reflection;
using HarmonyLib;
using MyceliumNetworking;
using DiaryWarning.Settings;
using Photon.Pun;
using UnityEngine;

namespace DiaryWarning;

[ContentWarningPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION, false)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class DiaryWarningMod : BaseUnityPlugin
{
    public static DiaryWarningMod Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;
    internal static Harmony Harmony { get; private set; } = null!;
    
    internal static List<DiaryEntry> SeenDiaryEntries = new List<DiaryEntry>();

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        // PatchAll();
        
        // MyceliumNetwork.RegisterLobbyDataKey("TLOL_TimeOnFor");
        // MyceliumNetwork.LobbyEntered += () =>
        // {
        //     if (MyceliumNetwork.IsHost)
        //     {
        //         MyceliumNetwork.SetLobbyData("TLOL_TimeOnFor", DiaryWarningSettings.TimeOnFor);
        //     }
        //     else
        //         TakeLobbyDataToConfig();
        // };
        // MyceliumNetwork.LobbyDataUpdated += (_) => TakeLobbyDataToConfig();
        
        On.RoundSpawner.SpawnMonster += (orig, self, monster, point) =>
        {
            // orig(self, monster, point);
            var go = PhotonNetwork.Instantiate(monster.gameObject.name, point.transform.position, Quaternion.identity, 0, null);
            
            Logger.LogWarning($"Spawning {go.name}, rarity = {monster.Rarity}");
            
            var diaryEntry = go.AddComponent<DiaryEntry>();
            diaryEntry.Setup(go.GetComponent<Bot>(), go, monster);
        };

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
    
    // private static void TakeLobbyDataToConfig()
    // {
    //     if (MyceliumNetwork.IsHost) return;
    //     DiaryWarningSettings.TimeOnFor = MyceliumNetwork.GetLobbyData<int>("TLOL_TimeOnFor");
    // }

    private void Update()
    {
        foreach (var key in ContentPolling.contentProviders.Select(keyValuePair => keyValuePair.Key))
        {
            var obj = key.gameObject;
            if (!obj) continue;
            
            var dEntry = obj.GetComponent<DiaryEntry>();
            if (!dEntry) continue;
            
            if (!SeenDiaryEntries.Contains(dEntry))
            {
                SeenDiaryEntries.Add(dEntry);
                Logger.LogWarning($"Added {obj.name} to DiaryEntries!");
            }
            else
            {
                Logger.LogWarning($"Hello again, {obj.name}!");
            }
        }
    }

    // internal static void PatchAll()
    // {
    //     Logger.LogDebug("Patching!");
    //     
    //     Harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
    //     Harmony.PatchAll(Assembly.GetExecutingAssembly());
    //     
    //     Logger.LogDebug("Patched!");
    // }
}