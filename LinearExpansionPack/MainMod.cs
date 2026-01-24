global using Il2Cpp;
using LinearExpansionPack;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(MainMod), "LinearExpansionPack", "2.2.1", "LinearPoint")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

/*
<<Known Bugs>>
- Somebody mad as a Pixie does not add a fake Minion to the Deckview rarely (HealthyBluff not being true?)
- Baker sometimes interacts poorly after Baking someone with the Mad status
- Empath skipped over a Siren one time? Very difficult to reproduce.
*/

namespace LinearExpansionPack;

public class MainMod : MelonMod
{
    public override void OnInitializeMelon() {
        ClassInjector.RegisterTypeInIl2Cpp<TaxCollector>();
        ClassInjector.RegisterTypeInIl2Cpp<Empath>();
        ClassInjector.RegisterTypeInIl2Cpp<Pixie>();
        ClassInjector.RegisterTypeInIl2Cpp<Siren>();
    }
    public override void OnLateInitializeMelon() {
        CharacterData Empath = new CharacterData();
        Empath.role = new Empath();
        Empath.name = "Empath";
        Empath.description = "Learn what character closest to me is Disguised.";
        Empath.flavorText = "\"Woke up one day insisting the Drunk had 'bad vibes'. She refused to elaborate further.\"";
        Empath.hints = "I am pretty sure the Wretch is a Minion in Disguise.\nI'll never check myself, but if lying might say my character is nearby.";
        Empath.ifLies = "The character learned is not the closest Disguised character to me. It may or may not be in play.";
        Empath.picking = false;
        Empath.startingAlignment = EAlignment.Good;
        Empath.type = ECharacterType.Villager;
        Empath.bluffable = true;
        Empath.characterId = "Empath_LP";
        Empath.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Empath.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Empath.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Empath.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData TaxCollector = new CharacterData();
        TaxCollector.role = new TaxCollector();
        TaxCollector.name = "Tax Collector";
        TaxCollector.description = "Learn one out of play Character.\nFalls for the Demon's bluff.";
        TaxCollector.flavorText = "\"Even the Demon knows to pay on time. The minions and Drunk never seem to learn their lesson though.\"";
        TaxCollector.hints = "I will not call out the Demon's bluff as being out of play.";
        TaxCollector.ifLies = "The character learned is in play.";
        TaxCollector.picking = false;
        TaxCollector.startingAlignment = EAlignment.Good;
        TaxCollector.type = ECharacterType.Villager;
        TaxCollector.bluffable = true;
        TaxCollector.characterId = "TaxCollector_LP";
        TaxCollector.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        TaxCollector.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        TaxCollector.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        TaxCollector.color = new Color(1f, 0.935f, 0.7302f);

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
        Siren.description = "One of my neighbors is <color=#d56ce2>Mad</color>.\n\nI Lie and Disguise.";
        Siren.flavorText = "\"If not for her constant molting she'd probably never have been caught by the Hunter.\"";
        Siren.hints = "<color=#d56ce2>Mad</color> characters will still have their original abilities function properly. Not their Disguise's abilities.\n<color=#d56ce2>Mad</color> characters will Disguise following Minion Disguising rules.\n<color=#d56ce2>Mad</color> characters will Lie 60% of the time if their Disguise gives information.\nIf <color=#d56ce2>Mad</color> about the character they actually are, they will always tell the truth.";
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


        // Planned Demon Unnamed - All Outcasts are Mad

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData) {
            ScriptInfo script = scriptData.scriptInfo;
            AddRole(script.startingTownsfolks, Empath);
            AddRole(script.startingTownsfolks, TaxCollector);
            AddRole(script.startingOutsiders, Pixie);
            AddRole(script.startingMinions, Siren);
        }
    }
    public void AddRole(Il2CppSystem.Collections.Generic.List<CharacterData> list, CharacterData data) {
        if (list.Contains(data))
            return;
        list.Add(data);
    }
    public CharacterData[] allDatas = Array.Empty<CharacterData>();
    public override void OnUpdate() {
        if (allDatas.Length == 0) {
            var loadedCharList = Resources.FindObjectsOfTypeAll(Il2CppType.Of<CharacterData>());
            if (loadedCharList != null) {
                allDatas = new CharacterData[loadedCharList.Length];
                for (int i = 0; i < loadedCharList.Length; i++)
                    allDatas[i] = loadedCharList[i]!.Cast<CharacterData>();
            }
        }
    }
    public CharacterData[] InsertAfterAct(string previous, CharacterData data) {
        CharacterData[] actList = Characters.Instance.startGameActOrder;
        int actSize = actList.Length;
        CharacterData[] newActList = new CharacterData[actSize + 1];
        bool inserted = false;
        for (int i = 0; i < actSize; i++) {
            if (inserted)
                newActList[i + 1] = actList[i];
            else {
                newActList[i] = actList[i];
                if (actList[i].name == previous) {
                    newActList[i + 1] = data;
                    inserted = true;
                }
            }
        }
        if (!inserted)
            LoggerInstance.Msg("");
        return newActList;
    }
}
public static class LinearStatic {
    public static ECharacterStatus mad = (ECharacterStatus)311;

    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class pvt {
        public static void Postfix(Character __instance) {
            if (__instance.statuses.Contains(mad))
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=18>\n<Mad></color></size>";
        }
    }
}
