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
public class Siren : Minion
{
    public override ActedInfo GetInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef)
    {
        return new ActedInfo("", null);
    }
    public override string Description
    {
        get
        {
            return "Neightbors love her";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start)
        {
            charRef.statuses.AddStatus(ECharacterStatus.UnkillableByDemon, charRef);
            
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> sortedChars = CharactersHelper.GetSortedListWithCharacterFirst(characters, charRef);

            sortedChars[1].statuses.AddStatus(LinearStatic.enchanted, charRef);
            sortedChars[1].statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);

            sortedChars[sortedChars._size - 1].statuses.AddStatus(LinearStatic.enchanted, charRef);
            sortedChars[sortedChars._size - 1].statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
        }
    }
    public CharacterData myBluffData;
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        if (charRef.bluff != null) return charRef.registerAs;

        if (myBluffData != null) return myBluffData;

        Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayCh = Gameplay.Instance.GetScriptCharacters();
        notInPlayCh = Characters.Instance.FilterCharacterType(notInPlayCh, ECharacterType.Villager);
        myBluffData = notInPlayCh[UnityEngine.Random.Range(0, notInPlayCh.Count - 1)];
        return myBluffData;
    }
    public override CharacterData GetRegisterAsRole(Character charRef)
    {
        if (myBluffData != null)
            return myBluffData;

        Il2CppSystem.Collections.Generic.List<CharacterData> notInPlayCh = Gameplay.Instance.GetScriptCharacters();
        notInPlayCh = Characters.Instance.FilterCharacterType(notInPlayCh, ECharacterType.Villager);

        myBluffData = notInPlayCh[UnityEngine.Random.Range(0, notInPlayCh.Count - 1)];

        return myBluffData;
    }
    public Siren() : base(ClassInjector.DerivedConstructorPointer<Siren>())
    {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Siren(IntPtr ptr) : base(ptr) { }
}

[HarmonyPatch(typeof(Character), nameof(Character.Reveal))]
public static class pvc
{
    public static void Postfix(Character __instance)
    {
        if (__instance.statuses.statuses.Contains(LinearStatic.enchanted))
        {
            Gameplay gameplay = Gameplay.Instance;
            Characters instance = Characters.Instance;
            Il2CppSystem.Collections.Generic.List<CharacterData> lista = gameplay.GetAscensionAllStartingCharacters();
            Il2CppSystem.Collections.Generic.List<CharacterData> listFin = instance.FilterRealCharacterType(lista, ECharacterType.Minion);

            var theSiren = 0;
            for (int i = 0; i < listFin.Count; i++)
            {
                if (listFin[i].characterId == "Siren_LP")
                    theSiren = i;
            }
            __instance.UpdateRegisterAsRole(listFin[theSiren]);
        }
    }
}
