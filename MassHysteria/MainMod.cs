global using Il2Cpp;
using MassHysteria;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using UnityEngine;
using Il2CppSoftMasking.Samples;

[assembly: MelonInfo(typeof(MainMod), "Mass Hysteria", "3.0.0", "LinearPoint")]
[assembly: MelonGame("UmiArt", "Demon Bluff")]

/*
<<Known Bugs>>
- The Chancellor, Shaman, and Puppeteer can remove the in play character a Cakler or Cackler Mad Outcast are Mad as.

<<Unimplemented Madness 2.0 Bugs>>
-- Note that Madness 2.0 wont be considered for mechanics until most Medium and all High priority bugs are fixed.
-- Madness 2.0 is intended to reduce Mad's power level by making Mad characters reveal their real info on death.
- [High] Mad characters who can pick do not appear as Mad after death.
- [High] Executing a dead Cakler counts as killing an evil.
- [Medium] Death sound does not play if killed character was mad.
- [Low] Information resets for Mad characters when the round ends.
*/

namespace MassHysteria;

public class MainMod : MelonMod
{
    public override void OnInitializeMelon() {
        ClassInjector.RegisterTypeInIl2Cpp<TaxCollector>();
        ClassInjector.RegisterTypeInIl2Cpp<Actor>();
        ClassInjector.RegisterTypeInIl2Cpp<Empath>();
        ClassInjector.RegisterTypeInIl2Cpp<Pixie>();
        ClassInjector.RegisterTypeInIl2Cpp<Magician>();
        ClassInjector.RegisterTypeInIl2Cpp<Siren>();
        ClassInjector.RegisterTypeInIl2Cpp<Cakler>();
    }
    public override void OnLateInitializeMelon() {
        CharacterData Actor = new CharacterData();
        Actor.role = new Actor();
        Actor.name = "Actor";
        Actor.description = "<b>Pick 1 alive character:</b>\nI become <color=#d56ce2>Mad</color> as the character they are claiming to be, even if they are Lying.";
        Actor.flavorText = "\"It's called method acting. You have to be in character.\"";
        Actor.hints = "I will not copy any start of the game effects.\n\n" + MadnessStatic.madDisclaimer;
        Actor.ifLies = "I will not become <color=#d56ce2>Mad</color>, but will change my bluff to the target's claim and Lie.";
        Actor.picking = true;
        Actor.startingAlignment = EAlignment.Good;
        Actor.type = ECharacterType.Villager;
        Actor.abilityUsage = EAbilityUsage.Once;
        Actor.bluffable = true;
        Actor.characterId = "Actor_MaHy";
        Actor.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Actor.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Actor.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Actor.color = new Color(1f, 0.935f, 0.7302f);

        /*CharacterData Attendant = new CharacterData();
        Attendant.role = new Attendant();
        Attendant.name = "Attendant";
        Attendant.description = "<b>Ability Used:</b>\nLearn how many characters to the left and right of me [Range 2] are currently <color=#d56ce2>Mad</color>.";
        Attendant.flavorText = "\"\"";
        Attendant.hints = "Range 2:\n2 characters to the left of me and 2 characters to the right of me are checked.\n\n" + MadnessStatic.madDisclaimer;
        Attendant.ifLies = "";
        Attendant.picking = false;
        Attendant.startingAlignment = EAlignment.Good;
        Attendant.type = ECharacterType.Villager;
        Attendant.abilityUsage = EAbilityUsage.Once;
        Attendant.bluffable = true;
        Attendant.characterId = "Attendant_MaHy";
        Attendant.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Attendant.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Attendant.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Attendant.color = new Color(1f, 0.935f, 0.7302f);*/

        /*CharacterData Balloonist = new CharacterData();
        Balloonist.role = new Balloonist();
        Balloonist.name = "Balloonist";
        Balloonist.description = "<b>Pick 2 characters:</b>\nLearn if they are the same character type or not.";
        Balloonist.flavorText = "";
        Balloonist.hints = "";
        Balloonist.ifLies = "";
        Balloonist.picking = true;
        Balloonist.startingAlignment = EAlignment.Good;
        Balloonist.type = ECharacterType.Villager;
        Balloonist.abilityUsage = EAbilityUsage.ResetAfterNight;
        Balloonist.bluffable = true;
        Balloonist.characterId = "Balloonist_MaHy";
        Balloonist.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Balloonist.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Balloonist.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Balloonist.color = new Color(1f, 0.935f, 0.7302f);*/

        CharacterData Empath = new CharacterData();
        Empath.role = new Empath();
        Empath.name = "Empath";
        Empath.description = "Learn what character closest to me is Disguised.";
        Empath.flavorText = "\"Woke up one day insisting the Drunk had 'bad vibes'. She refused to elaborate further.\"";
        Empath.hints = "I will never check myself when considering which Disgusied character is closer, even when Lying.";
        Empath.ifLies = "The Learned character may or may not be in play. I might Learn my real character if I am Disguised.";
        Empath.picking = false;
        Empath.startingAlignment = EAlignment.Good;
        Empath.type = ECharacterType.Villager;
        Empath.bluffable = true;
        Empath.characterId = "Empath_MaHy";
        Empath.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Empath.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Empath.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Empath.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData TaxCollector = new CharacterData();
        TaxCollector.role = new TaxCollector();
        TaxCollector.name = "Tax Collector";
        TaxCollector.description = "Learn 1 out of play character.";
        TaxCollector.flavorText = "\"Swears the money is going to a good cause. What cause?\nWhy the demon hunting fund of course.\"";
        TaxCollector.hints = "Characters Lying about who they are might not always be Evil.";
        TaxCollector.ifLies = "";
        TaxCollector.picking = false;
        TaxCollector.startingAlignment = EAlignment.Good;
        TaxCollector.type = ECharacterType.Villager;
        TaxCollector.bluffable = true;
        TaxCollector.characterId = "TaxCollector_MaHy";
        TaxCollector.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        TaxCollector.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        TaxCollector.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        TaxCollector.color = new Color(1f, 0.935f, 0.7302f);

        CharacterData Pixie = new CharacterData();
        Pixie.role = new Pixie();
        Pixie.name = "Pixie";
        Pixie.description = "1 random Minion is added to the Deck View.\nI have the abilities of an out-of-play Minion.";
        Pixie.flavorText = "\"She insists that all of her pranks are harmless.\nYes, the poison was harmless!\"";
        Pixie.hints = "I will never add to the Deck View or copy the abilities of the \"Minion\", \"Twin Minion\", or \"Chancellor\". I think they are boring.";
        Pixie.ifLies = "A fake Minion is still added to the Deck View, but its abilities are not functioning.";
        Pixie.picking = false;
        Pixie.startingAlignment = EAlignment.Good;
        Pixie.type = ECharacterType.Outcast;
        Pixie.bluffable = true;
        Pixie.characterId = "Pixie_MaHy";
        Pixie.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        Pixie.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Pixie.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Pixie.color = new Color(0.9659f, 1f, 0.4472f);
        Characters.Instance.startGameActOrder = InsertAfterAct("Pooka", Pixie);

        CharacterData Magician = new CharacterData();
        Magician.role = new Magician();
        Magician.name = "Magician";
        Magician.description = "1 Villager to the left and right of me [Range 3] are <color=#d56ce2>Mad</color> as each other's character.\nLearn one character who is <color=#d56ce2>Mad</color>.\nIf I cannot find a Villager on either side, I become Corrupted and make no one <color=#d56ce2>Mad</color>.";
        Magician.flavorText = "\"Once managed to convince the Slayer they were immortal.\nThe Knight was less amused.\"";
        Magician.hints = MadnessStatic.madDisclaimer;
        Magician.ifLies = "No Villagers were made <color=#d56ce2>Mad</color> and the character I Learned is sane.";
        Magician.picking = false;
        Magician.startingAlignment = EAlignment.Good;
        Magician.type = ECharacterType.Outcast;
        Magician.bluffable = true;
        Magician.characterId = "Magician_MaHy";
        Magician.artBgColor = new Color(0.3679f, 0.2014f, 0.1541f);
        Magician.cardBgColor = new Color(0.102f, 0.0667f, 0.0392f);
        Magician.cardBorderColor = new Color(0.7843f, 0.6471f, 0f);
        Magician.color = new Color(0.9659f, 1f, 0.4472f);
        Characters.Instance.startGameActOrder = InsertAfterAct("Alchemist", Magician);

        /*CharacterData Fisherman = new CharacterData();
        Fisherman.role = new Fisherman();
        Fisherman.name = "Fisherman";
        Fisherman.description = "<b>At Night:</b>\nIf unrevealed I have a 50% chance of becoming <color=#d56ce2>Mad</color> as another unrevealed character. If Evil I will be <color=#d56ce2>Mad</color> as their Disguise and become Corrupted.";
        Fisherman.flavorText = "\"After bringing home that night's catch the Witness ran in panic thinking he was a sea monster.\"";
        Fisherman.hints = MadnessStatic.madDisclaimer;
        Fisherman.ifLies = "I will never become <color=#d56ce2>Mad</color> or Corrupted by my own ability.";
        Fisherman.picking = false;
        Fisherman.startingAlignment = EAlignment.Good;
        Fisherman.type = ECharacterType.Outcast;
        Fisherman.bluffable = true;
        Fisherman.characterId = "Fisherman_MaHy";
        Fisherman.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Fisherman.cardBgColor = new Color(0.26f, 0.1519f, 0.3396f);
        Fisherman.cardBorderColor = new Color(0.7133f, 0.339f, 0.8679f);
        Fisherman.color = new Color(1f, 0.935f, 0.7302f);*/

        CharacterData Cakler = new CharacterData();
        Cakler.role = new Cakler();
        Cakler.name = "Cakler";
        Cakler.description = "I am <color=#d56ce2>Mad</color> as an in-play Villager.\nAll Outcasts are <color=#d56ce2>Mad</color> as in-play Villagers.\nYou start with 2 additional health.\n\nI Lie and Disguise.";
        Cakler.flavorText = "\"Outcasts? I've never heard of any Outcasts.\nYOU MUST BE AS MAD AS ME!\nKYEHEEHEE!\"";
        Cakler.hints = "Minions try to avoid claiming Outcasts when I am in play.\n\n" + MadnessStatic.madDisclaimer;
        Cakler.ifLies = "";
        Cakler.picking = false;
        Cakler.startingAlignment = EAlignment.Evil;
        Cakler.type = ECharacterType.Demon;
        Cakler.bluffable = false;
        Cakler.characterId = "Cackler_MaHy";
        Cakler.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Cakler.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Cakler.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Cakler.color = new Color(1f, 0.3804f, 0.3804f);
        Characters.Instance.startGameActOrder = InsertAfterAct("Magician", Cakler);

        CharacterData Siren = new CharacterData();
        Siren.role = new Siren();
        Siren.name = "Siren";
        Siren.description = "1 of my Good neighbors is <color=#d56ce2>Mad</color> as a random Good Character.\n\nI Lie and Disguise.";
        Siren.flavorText = "\"If not for her constant molting, she'd probably have never been caught by the Hunter.\"";
        Siren.hints = "My <color=#d56ce2>Mad</color> neighbor will Disguise using normal Minion Disguising rules.\n\n" + MadnessStatic.madDisclaimer; 
        Siren.ifLies = "";
        Siren.picking = false;
        Siren.startingAlignment = EAlignment.Evil;
        Siren.type = ECharacterType.Minion;
        Siren.bluffable = false;
        Siren.characterId = "Siren_MaHy";
        Siren.artBgColor = new Color(1f, 0f, 0f);
        Siren.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Siren.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Siren.color = new Color(0.8491f, 0.4555f, 0f);
        Characters.Instance.startGameActOrder = InsertAfterAct("Cakler", Siren);

        /*CharacterData Changeling = new CharacterData();
        Changeling.role = new Changeling();
        Changeling.name = "Changeling";
        Changeling.description = "One Villager is Corrupted and <color=#d56ce2>Mad</color> as my Disguise.\nI will always Disguise as a not in play Villager like a Demon would and never Lie.\n\nI Disguise.";
        Changeling.flavorText = "\"\"";
        Changeling.hints = "The <color=#d56ce2>Mad</color> Villager will always Lie unless their Corruption is cured somehow."; 
        Changeling.ifLies = "I will never Lie unless I get Corrupted somehow.";
        Changeling.picking = false;
        Changeling.startingAlignment = EAlignment.Evil;
        Changeling.type = ECharacterType.Minion;
        Changeling.bluffable = false;
        Changeling.characterId = "Changeling_MaHy";
        Changeling.artBgColor = new Color(1f, 0f, 0f);
        Changeling.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Changeling.cardBorderColor = new Color(0.8208f, 0f, 0.0241f);
        Changeling.color = new Color(0.8491f, 0.4555f, 0f);
        Characters.Instance.startGameActOrder = InsertAfterAct("Plague Doctor", Changeling);*/

        /*CharacterData Levi = new CharacterData();
        Levi.role = new Levi();
        Levi.name = "Levi";
        Levi.description = "<b>At Night</b>\nAn unrevealed Good character becomes <color=#d56ce2>Mad</color>.\nNight comes sooner; Night starts after 3 reveals rather than 4.\n\nI Lie and Disguise.";
        Levi.flavorText = "\"The fog is purely for ascetics.\nThe truth is what drives them mad.\"";
        Levi.hints = MadnessStatic.madDisclaimer;
        Levi.ifLies = "";
        Levi.picking = false;
        Levi.startingAlignment = EAlignment.Evil;
        Levi.type = ECharacterType.Demon;
        Levi.bluffable = false;
        Levi.characterId = "Levi_MaHy";
        Levi.artBgColor = new Color(0.111f, 0.0833f, 0.1415f);
        Levi.cardBgColor = new Color(0.0941f, 0.0431f, 0.0431f);
        Levi.cardBorderColor = new Color(0.8196f, 0.0f, 0.0275f);
        Levi.color = new Color(1f, 0.3804f, 0.3804f);*/

        CustomScriptData caklerScriptData = new CustomScriptData();
        caklerScriptData.name = "Cackler_1";
        ScriptInfo caklerScript = new ScriptInfo();
        Il2CppSystem.Collections.Generic.List<CharacterData> caklerList = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        caklerList.Add(Cakler);
        caklerScript.mustInclude = caklerList;
        caklerScript.startingDemons = caklerList;
        caklerScript.startingTownsfolks = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingTownsfolks;
        caklerScript.startingOutsiders = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingOutsiders;
        caklerScript.startingMinions = ProjectContext.Instance.gameData.advancedAscension.possibleScriptsData[0].scriptInfo.startingMinions;
        CharactersCount caklerCounter2 = new CharactersCount(7, 4, 1, 1, 1);
        caklerCounter2.dOuts = caklerCounter2.outs + 1;
        CharactersCount caklerCounter3 = new CharactersCount(8, 5, 1, 1, 1);
        caklerCounter3.dOuts = caklerCounter3.outs + 1;
        CharactersCount caklerCounter4 = new CharactersCount(9, 5, 1, 2, 1);
        caklerCounter4.dOuts = caklerCounter4.outs + 1;
        Il2CppSystem.Collections.Generic.List<CharactersCount> caklerCounterList = new Il2CppSystem.Collections.Generic.List<CharactersCount>();
        caklerCounterList.Add(caklerCounter2);
        caklerCounterList.Add(caklerCounter3);
        caklerCounterList.Add(caklerCounter4);
        caklerScript.characterCounts = caklerCounterList;
        caklerScriptData.scriptInfo = caklerScript;

        AscensionsData advancedAscension = ProjectContext.Instance.gameData.advancedAscension;
        Il2CppReferenceArray<CharacterData> advancedAscensionDemons = new Il2CppReferenceArray<CharacterData>(advancedAscension.demons.Length + 1);
        advancedAscensionDemons = advancedAscension.demons;
        advancedAscensionDemons[advancedAscensionDemons.Length - 1] = Cakler;
        advancedAscension.demons = advancedAscensionDemons;
        Il2CppReferenceArray<CharacterData> advancedAscensionStartingDemons = new Il2CppReferenceArray<CharacterData>(advancedAscension.startingDemons.Length + 1);
        advancedAscensionStartingDemons = advancedAscension.startingDemons;
        advancedAscensionStartingDemons[advancedAscensionStartingDemons.Length - 1] = Cakler;
        advancedAscension.startingDemons = advancedAscensionStartingDemons;
        Il2CppReferenceArray<CustomScriptData> advancedAscensionScriptsData = new Il2CppReferenceArray<CustomScriptData>(advancedAscension.possibleScriptsData.Length + 1);
        advancedAscensionScriptsData = advancedAscension.possibleScriptsData;
        advancedAscensionScriptsData[advancedAscensionScriptsData.Length - 1] = caklerScriptData;
        advancedAscension.possibleScriptsData = advancedAscensionScriptsData;
        foreach (CustomScriptData scriptData in advancedAscension.possibleScriptsData) {
            ScriptInfo script = scriptData.scriptInfo;
            AddRole(script.startingTownsfolks, Actor);
            AddRole(script.startingTownsfolks, Empath);
            AddRole(script.startingTownsfolks, TaxCollector);
            AddRole(script.startingOutsiders, Pixie);
            AddRole(script.startingOutsiders, Magician);
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
public static class MadnessStatic {
    public static readonly int TRUTH_CHANCE = 7;
    public static ECharacterStatus mad = (ECharacterStatus)311;
    public static readonly string madDisclaimer = "<b>Madness:</b>\n<color=#d56ce2>Mad</color> Good characters Lie 60% of the time, but Evil or Corrupted characters always Lie.\n<color=#d56ce2>Mad</color> characters still use their actual abilities, and will not use their Disguise's abilities.";

    public static void DetermineIfTruethful(Character target, Character charRef) {
        if (!target.statuses.statuses.Contains(ECharacterStatus.Corrupted) && target.alignment == EAlignment.Good) {
            int diceRoll = Calculator.RollDice(10);
            if (diceRoll >= MadnessStatic.TRUTH_CHANCE)
                target.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            if (target.bluff.characterId is "Baker_22847064" or "Gambler_42592744" or "Alchemist_94446803" or "Knight_47970624")
                target.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
            if (target.bluff.characterId == target.GetCharacterData().characterId)
                target.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            if (target.GetCharacterData().characterId is "Drunk_15369527") {
                if (!target.statuses.statuses.Contains(ECharacterStatus.Corrupted))
                    target.statuses.AddStatus(ECharacterStatus.Corrupted, target);
                target.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
            }
        }
    }
    
    // Madness 1.0
    [HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class MadRevealPatch {
        public static void Postfix(Character __instance) {
            if (__instance.statuses.Contains(mad) && __instance.statuses.Contains(ECharacterStatus.Corrupted))
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=16>\n<Mad+Corrupt></color></size>";
            else if (__instance.statuses.Contains(mad))
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=18>\n<Mad></color></size>";    
        }
    }

    // Madness 2.0 Do Not Used until bugs are fixed.
    /*[HarmonyPatch(typeof(Character), nameof(Character.RevealAllReal))]
    public static class MadRevealPatch {
        public static void Postfix(Character __instance) {
            if (Gameplay.GameplayState == EGameplayState.Summary){
                if (__instance.statuses.Contains(mad) && __instance.statuses.Contains(ECharacterStatus.Corrupted))
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=16>\n<Mad+Corrupt></color></size>";
                else if (__instance.statuses.Contains(mad))
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=18>\n<Mad></color></size>";
            } else if (__instance.statuses.Contains(mad) && __instance.statuses.Contains(ECharacterStatus.Corrupted)) {
                __instance.bluff = null;
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=16>\n<Mad+Corrupt></color></size>";
                if(!__instance.GetCharacterData().picking) {
                    __instance.Act(ETriggerPhase.Day);
                    __instance.ChangeState(__instance.prevState);
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=16>\n<Mad+Corrupt></color></size>";
                    __instance.ChangeState(ECharacterState.Revealed);
                }
                else {
                    //__instance.Init(__instance.GetCharacterData(), __instance.id);
                    __instance.ChangeState(__instance.prevState);
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=16>\n<Mad+Corrupt></color></size>";
                    __instance.ChangeState(ECharacterState.Revealed);
                }
            }
            else if (__instance.statuses.Contains(mad)) {
                __instance.bluff = null;
                __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=18>\n<Mad></color></size>";
                if(!__instance.GetCharacterData().picking) {
                    __instance.Act(ETriggerPhase.Day);
                    __instance.ChangeState(__instance.prevState);
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=18>\n<Mad></color></size>";
                    __instance.ChangeState(ECharacterState.Revealed);
                }
                else if (__instance.GetCharacterData().characterId != "Actor_MaHy") {
                    //__instance.Init(__instance.GetCharacterData(), __instance.id);
                    __instance.ChangeState(__instance.prevState);
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=18>\n<Mad></color></size>";
                    __instance.ChangeState(ECharacterState.Revealed);
                } else
                    __instance.chName.text = __instance.dataRef.name.ToUpper() + "<color=#d56ce2><size=18>\n<Mad></color></size>";
            }
        }
    }*/

    /*([HarmonyPatch(MethodType.Constructor, typeof(Role))]
    public static class ConstructorPatch {
        public static Postfix() {
            MelonLogger.Msg("Constructor Patch Applied.");
        }
    }

    [HarmonyPatch(typeof(Role), nameof(Role.GetBluffIfAble))]
    public static class BluffMadPatch {
        public static void Postfix(Role __instance, ref CharacterData __result) {
            if (!__instance.charRef) return;
            //if (String.IsNullOrEmpty(__instance?.charRef.name)) return;
            // if (__instance.Pointer == IntPtr.Zero) return;
            //MelonLogger.Msg("About to explode");
            // MelonLogger.Msg("Role: " + __instance.charRef.name);

            if (!__instance.statuses.statuses.Contains(LinearStatic.mad)) {
                MelonLogger.Msg("#" + __instance.GetCharacterData().characterId + " is not Mad.");
                //return __instance.GetCharacterData();
            }
            MelonLogger.Msg("#" + __instance.GetCharacterData().characterId + " is Mad.");
            //return __instance.GetCharacterData();
        }
    }*/
}