using Il2Cpp;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LinearExpansionPack;

[RegisterTypeInIl2Cpp]
public class Pixie : Role {
    public CharacterData fakeMinion = GetGenericMinion();
    public override ActedInfo GetInfo(Character charRef) {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef) {
        return new ActedInfo("", null);
    }
    public override string Description {
        get {
            return "A good Minion";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef) {
        if (trigger == ETriggerPhase.Start) {
            Gameplay gameplay = Gameplay.Instance;
            Characters instance = Characters.Instance;
            Il2CppSystem.Collections.Generic.List<CharacterData> lista = gameplay.GetAscensionAllStartingCharacters();
            Il2CppSystem.Collections.Generic.List<CharacterData> listb = instance.FilterNotInDeckCharactersUnique(lista);
            Il2CppSystem.Collections.Generic.List<CharacterData> listc = instance.FilterRealCharacterType(listb, ECharacterType.Minion);
            Il2CppSystem.Collections.Generic.List<CharacterData> listFin = new Il2CppSystem.Collections.Generic.List<CharacterData>();

            for (int i = 0; i < listc.Count; i++) {
                if (listc[i].characterId != "Minion_71804875" && listc[i].characterId != "Twin Minion_15695218")
                    listFin.Add(listc[i]);
            }

            if (listFin == null || listFin.Count == 0)
                return;

            fakeMinion = listFin[UnityEngine.Random.RandomRangeInt(0, listFin.Count)];
            gameplay.AddScriptCharacter(ECharacterType.Minion, fakeMinion);
        }
        if (charRef.GetCharacterData().name == "Pixie")
            fakeMinion.role.Act(trigger, charRef);
    }
    public override void ActOnDied(Character charRef) {
        if (charRef.GetCharacterData().name == "Pixie")
            fakeMinion.role.ActOnDied(charRef);
    }
    public static CharacterData GetGenericMinion() {
        AscensionsData allCharactersAscension = ProjectContext.Instance.gameData.allCharactersAscension;
        for (int i = 0; i < allCharactersAscension.startingMinions.Length; i++) {
            if (allCharactersAscension.startingMinions[i].name == "Minion")
                return allCharactersAscension.startingMinions[i];
        }
        return allCharactersAscension.startingMinions[0];
    }
    public Pixie() : base(ClassInjector.DerivedConstructorPointer<Pixie>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Pixie(IntPtr ptr) : base(ptr) { }
}
public static class PixieRole {
    [HarmonyPatch(typeof(Minion), nameof(Minion.GetBluffIfAble))]
    public static class pvc {
        public static void Postfix(Minion __instance, ref CharacterData __result, Character charRef) {
            if (__result.name == "Pixie")
                __result.role.BluffAct(ETriggerPhase.Start, charRef);
        }
    }

    [HarmonyPatch(typeof(Baron), nameof(Baron.SitNextToOutsider))]
    public static class snto{
        public static void Postfix(Baron __instance, Character charRef){
            if (charRef.GetCharacterData().name == "Pixie"){
                Il2CppSystem.Collections.Generic.List<Character> outsiders = new Il2CppSystem.Collections.Generic.List<Character>(Gameplay.CurrentCharacters.Pointer);
                outsiders.Remove(charRef);
                outsiders = Characters.Instance.FilterCharacterType(outsiders, ECharacterType.Outcast);

                Character pickedOutsider = outsiders[UnityEngine.Random.Range(0, outsiders.Count)];
                pickedOutsider.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);

                Il2CppSystem.Collections.Generic.List<Character> adjacentCharacters = Characters.Instance.GetAdjacentAliveCharacters(pickedOutsider);
                Character pickedSwapCharacter = adjacentCharacters[UnityEngine.Random.Range(0, adjacentCharacters.Count)];
                CharacterData pickedData = pickedSwapCharacter.dataRef;
                pickedSwapCharacter.Init(charRef.dataRef);
                charRef.Init(pickedData);
            }
        }
    }
}