using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using BepInEx;
using BepInEx.Logging;
using System.Reflection;
using DefaultNamespace;
using DiaryWarning.Entries;
using MyceliumNetworking;
using DiaryWarning.Settings;
using DiaryWarning.UIStuff;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EscapeMenuDiaryPage = DiaryWarning.UIStuff.EscapeMenuDiaryPage;
using Quaternion = UnityEngine.Quaternion;

namespace DiaryWarning;

[ContentWarningPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_VERSION, false)]
[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class DiaryWarningMod : BaseUnityPlugin
{
    public static DiaryWarningMod Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger { get; private set; } = null!;

    internal static readonly List<IDiaryEntry> UnlockedDiaryEntries = [];
    internal static readonly Dictionary<Type, IDiaryEntry> DiaryEntries = new() {
        { typeof(PuffoContentEventProvider), new PuffoDiaryEntry() }
    };
    
    internal static List<GameObject> SpawnableMonsters = [];
    internal static readonly Dictionary<String, ContentProvider> MonsterContentProviders = new();

    private const string abName = "assets";
    internal static readonly AssetBundle mainAB = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, abName));

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

            var cEvents = new List<ContentEventFrame>();
            MonsterContentProviders[monster.gameObject.name].GetContent(cEvents, 1f, ContentPolling.m_currentPollingCamera, 0f);
            Logger.LogWarning($"Possible views: {BigNumbers.GetScoreToViews(cEvents.First().GetScore(), GameAPI.CurrentDay+1)} for day {GameAPI.CurrentDay}");
        };

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }
    
    // private static void TakeLobbyDataToConfig()
    // {
    //     if (MyceliumNetwork.IsHost) return;
    //     DiaryWarningSettings.TimeOnFor = MyceliumNetwork.GetLobbyData<int>("TLOL_TimeOnFor");
    // }

    private void Start()
    {
        SpawnableMonsters = Resources.LoadAll<GameObject>("").Where(GameObject => GameObject.GetComponent<BudgetCost>() != null).ToList();
        foreach (var spawnable in SpawnableMonsters)
        {
            var contentProvider = spawnable.GetComponent<ContentProvider>();
            if (contentProvider == null) continue;
            Logger.LogWarning($"{spawnable.name} has a content provider of type {contentProvider.GetType().Name}");
            
            MonsterContentProviders.Add(spawnable.name, contentProvider);
        }

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if(!PhotonNetwork.IsMasterClient) return;
            
            var button = EscapeMenuManagerAPIThingy.AddButton("DWButton", "DIARY", () => {}, 1);
            if (button == null) return;
            
            var page = EscapeMenuManagerAPIThingy.AddPageWithComp<EscapeMenuDiaryPage>("DiaryPage", mainAB.LoadAsset<GameObject>("Assets/DiaryPage.prefab"));
            if (page == null) return;
            
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (EscapeMenuManagerAPIThingy.EscapeMenu?.GetComponent<EscapeMenuUIHandler>()
                        .TransistionToPage<EscapeMenuDiaryPage>() == null)
                {
                    Logger.LogError("Failed to transition to the DiaryPage!");
                }
                page.GetComponent<EscapeMenuDiaryPage>().RefreshListings();
            });
        };
    }

    private void Update()
    {
        if(ContentPolling.contentProviders is null) return;
        foreach (var key in ContentPolling.contentProviders.Select(keyValuePair => keyValuePair.Key))
        {
            if(key is null) continue;
            
            var obj = key.gameObject;
            
            if(!DiaryEntries.ContainsKey(key.GetType())) continue;
            var diaryEntry = DiaryEntries[key.GetType()];
            
            if (UnlockedDiaryEntries.Contains(diaryEntry)) continue;
            UnlockedDiaryEntries.Add(diaryEntry);
            
            Logger.LogWarning($"Unlocked diary entry for {obj.name}! {diaryEntry.GetTitle()}! {diaryEntry.GetPossibleViews()} views!");
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