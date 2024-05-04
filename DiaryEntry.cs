using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using System.Reflection;
using DefaultNamespace;
using HarmonyLib;
using MyceliumNetworking;
using DiaryWarning.Settings;
using UnityEngine;

namespace DiaryWarning;

public class DiaryEntry : MonoBehaviour
{
    public Bot enemy = null!;
    public GameObject me = null!;
    public IBudgetCost budgetCost = null!;
    
    public void Setup(Bot e, GameObject m, IBudgetCost bc)
    {
        this.enemy = e;
        this.me = m;
        this.budgetCost = bc;
    }
}