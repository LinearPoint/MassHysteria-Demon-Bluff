global using Il2Cpp;
using LinearExpansionPack;
using HarmonyLib;
using Il2CppDissolveExample;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.IO;
using MelonLoader;
using System;
using UnityEngine;
using static Il2Cpp.Interop;
using static Il2CppSystem.Array;

[assembly: MelonInfo(typeof(MainMod), "LinearExpansionPack", "1.0", "LinearPoint")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

namespace LinearExpansionPack;

public class MainMod : MelonMod
{
    public override void OnInitializeMelon()
    {
        ClassInjector.RegisterTypeInIl2Cpp<Pixie>();
        ClassInjector.RegisterTypeInIl2Cpp<Siren>();
    }
    public override void OnLateInitializeMelon()
    {
        CharacterData Pixie = new CharacterData();
        Pixie.role = new Pixie();
        Pixie.name = "Pixie";
        Pixie.description = "One random Minion is added to the Deck View.\nI have the abilities of an out of play Minion.";
        Pixie.flavorText = "\"She insists that all of her pranks are harmless. Yes, the poison was harmless!\"";
        Pixie.hints = "If bluffed a fake Minion is still added to the Deck View, but its abilities are not functioning.";
        Pixie.ifLies = "";
        Pixie.picking = false;
        Pixie.startingAlignment = EAlignment.Good;
        Pixie.type = ECharacterType.Outcast;
        Pixie.bluffable = true;
        Pixie.characterId = "Pixie_LP";
        Pixie.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        Pixie.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Pixie.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Pixie.color = new Color(0.9659f, 1f, 0.4472f);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", Pixie);

        CharacterData Siren = new CharacterData();
        Siren.role = new Siren();
        Siren.name = "Siren";
        Siren.description = "Everyone believes I am a Good Villager.\nBoth of my neighbors appear to be a <color=#D8B200>Siren</color>.\n\nI Lie and Disguise.";
        Siren.flavorText = "\"If it weren't for her constant molting she'd probably never have been caught by the Hunter.\"";
        Siren.hints = "I will even make fellow Minions and Demons appear to be a <color=#D8B200>Siren</color>.";
        Siren.ifLies = "";
        Siren.picking = false;
        Siren.startingAlignment = EAlignment.Evil;
        Siren.type = ECharacterType.Minion;
        Siren.bluffable = false;
        Siren.characterId = "Siren_LP";
        Siren.artBgColor = new Color(1f, 0f, 0f);
        Siren.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Siren.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Siren.color = new Color(0.8491f, 0.4555f, 0f);
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", Siren);

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData)
        {
            ScriptInfo script = scriptData.scriptInfo;
            AddRole(script.startingOutsiders, Pixie);
            AddRole(script.startingMinions, Siren);
        }
    }
    public void AddRole(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data)
    {
        if (list.Contains(data))
        {
            return;
        }
        list.Add(data);
    }
    public CharacterData[] allDatas = Array.Empty<CharacterData>();
    public override void OnUpdate()
    {
        if (allDatas.Length == 0)
        {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null)
            {
                allDatas = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                {
                    allDatas[i] = loadedCharList[i]!.Cast<CharacterData>();
                }
            }
        }
    }
    public CharacterData[] InsertAfterAct(string previous, CharacterData data)
    {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        bool inserted = false;
        for (int i = 0; i < actSize; i++)
        {
            if (inserted)
            {
                newActList[i + 1] = actList[i];
            }
            else
            {
                newActList[i] = actList[i];
                if (actList[i].name == previous)
                {
                    newActList[i + 1] = data;
                    inserted = true;
                }
            }
        }
        if (!inserted)
        {
            LoggerInstance.Msg("");
        }
        return newActList;
    }
}
public static class LinearStatic
{
    public static ECharacterStatus enchanted = (ECharacterStatus)311;
}
