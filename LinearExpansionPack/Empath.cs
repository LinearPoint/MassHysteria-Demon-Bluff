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
using System.ComponentModel;

namespace LinearExpansionPack;

[RegisterTypeInIl2Cpp]
public class Empath : Role {
    public override ActedInfo GetInfo(Character charRef) {
        Gameplay gameplay = Gameplay.Instance;
        
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> sortedChars = CharactersHelper.GetSortedListWithCharacterFirst(characters, charRef);
        sortedChars.RemoveAt(0);
        Il2CppSystem.Collections.Generic.List<Character> disguisedChars = new Il2CppSystem.Collections.Generic.List<Character>();

        for (int i = 0; i < sortedChars.Count / 2; i++) {
            if (sortedChars[i].GetCharacterData().name != sortedChars[i].GetCharacterBluffIfAble().name && !disguisedChars.Contains(sortedChars[i]))
                disguisedChars.Add(sortedChars[i]);
            if (sortedChars[^(i + 1)].GetCharacterData().name != sortedChars[^(i + 1)].GetCharacterBluffIfAble().name && !disguisedChars.Contains(sortedChars[^(i + 1)]))
                disguisedChars.Add(sortedChars[^(i + 1)]);
            if (disguisedChars.Count >= 1)
                break;
        }

        Character randomChar = disguisedChars[UnityEngine.Random.Range(0, disguisedChars.Count)];

        String info = "I sense the presence of the " + randomChar.GetCharacterData().name + " nearby.";

        return new ActedInfo(info);
    }
    public override ActedInfo GetBluffInfo(Character charRef) {   
        Gameplay gameplay = Gameplay.Instance;
        Characters instance = Characters.Instance;
        
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> sortedChars = CharactersHelper.GetSortedListWithCharacterFirst(characters, charRef);
        sortedChars.RemoveAt(0);
        Il2CppSystem.Collections.Generic.List<Character> disguisedChars = new Il2CppSystem.Collections.Generic.List<Character>();

        for (int i = 0; i < sortedChars.Count / 2; i++) {
            if (sortedChars[i].GetCharacterData().name != sortedChars[i].GetCharacterBluffIfAble().name && !disguisedChars.Contains(sortedChars[i]))
                disguisedChars.Add(sortedChars[i]);
            if (sortedChars[^(i + 1)].GetCharacterData().name != sortedChars[^(i + 1)].GetCharacterBluffIfAble().name && !disguisedChars.Contains(sortedChars[^(i + 1)]))
                disguisedChars.Add(sortedChars[^(i + 1)]);
            if (disguisedChars.Count >= 1)
                break;
        }

        Il2CppSystem.Collections.Generic.List<CharacterData> scriptChars = gameplay.GetScriptCharacters();
        Il2CppSystem.Collections.Generic.List<CharacterData> fakeDisguises = instance.FilterRealCharacterType(scriptChars, ECharacterType.Minion);
        Il2CppSystem.Collections.Generic.List<CharacterData> Demon = instance.FilterRealCharacterType(scriptChars, ECharacterType.Demon);
        for (int i = 0; i < Demon.Count; i++) {
            if (!fakeDisguises.Contains(Demon[i]))
                fakeDisguises.Add(Demon[i]);
        }
        for (int i = 0; i < scriptChars.Count; i++) {
            if (scriptChars[i].name == "Drunk" || scriptChars[i].name == "Doppelganger")
                fakeDisguises.Add(scriptChars[i]);
        }
        for (int i = 0; i < disguisedChars.Count; i++) {
            if (fakeDisguises.Contains(disguisedChars[i].GetCharacterData()))
                fakeDisguises.Remove(disguisedChars[i].GetCharacterData());
        }

        if (fakeDisguises.Count == 0) {
            Il2CppSystem.Collections.Generic.List<CharacterData> villagers = instance.FilterRealCharacterType(scriptChars, ECharacterType.Villager);
            Il2CppSystem.Collections.Generic.List<CharacterData> fakeVillagers = instance.FilterNotInPlayCharacters(villagers);

            for (int i = 0; i < fakeVillagers.Count; i++)
                villagers.Remove(fakeVillagers[i]);
            for (int i = 0; i < villagers.Count; i++) {
                if (villagers[i].name == "Empath")
                    villagers.Remove(villagers[i]);
            }
            fakeDisguises = villagers;
        }   

        CharacterData randomChar = fakeDisguises[UnityEngine.Random.Range(0, fakeDisguises.Count)];

        String info = "I sense the presence of the " + randomChar.name + " nearby.";

        return new ActedInfo(info);
    }
    public override string Description {
        get {
            return "Knows when someone is pretending to be somebody else.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef) {
        if (trigger == ETriggerPhase.Day)
            onActed?.Invoke(GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef) {
        if (trigger == ETriggerPhase.Day)
            onActed?.Invoke(GetBluffInfo(charRef));
    }
    public Empath() : base(ClassInjector.DerivedConstructorPointer<Empath>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Empath(IntPtr ptr) : base(ptr) { }

}
