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
public class TaxCollector : Role {
    public override ActedInfo GetInfo(Character charRef) {
        Gameplay gameplay = Gameplay.Instance;
        Characters instance = Characters.Instance;

        Il2CppSystem.Collections.Generic.List<CharacterData> lista = gameplay.GetScriptCharacters();
        Il2CppSystem.Collections.Generic.List<CharacterData> listb = instance.FilterNotInPlayCharacters(lista);

        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        
        // Remove the Demon's Bluff and add Wretch if in play
        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].GetCharacterType() == ECharacterType.Demon)
                listb.Remove(characters[i].GetCharacterBluffIfAble());
            else if (characters[i].GetCharacterData().name == "Wretch" && !listb.Contains(characters[i].GetCharacterData()))
                listb.Add(characters[i].GetCharacterData());
            else if (characters[i].GetCharacterData().name == "Tax Collector" && listb.Contains(characters[i].GetCharacterData()))
                listb.Remove(characters[i].GetCharacterData());
        }

        String info = "";
        if (listb.Count == 0)
            info = "Everyone's taxes are accounted for, even the Demon paid on time.";
        else {
            CharacterData randomChar = listb[UnityEngine.Random.Range(0, listb.Count)];
            info += "I've never seen the " + randomChar.name + " paying their taxes.";
        }

        return new ActedInfo(info);
    }
    public override ActedInfo GetBluffInfo(Character charRef) {   
        Il2CppSystem.Collections.Generic.List<CharacterData> inPlay = new Il2CppSystem.Collections.Generic.List<CharacterData>();
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> sortedChars = CharactersHelper.GetSortedListWithCharacterFirst(characters, charRef);
        sortedChars.RemoveAt(0);

        for (int i = 0; i < characters.Count; i++) {
            if (characters[i].GetAlignment() == EAlignment.Good && characters[i].GetCharacterData().name != "Tax Collector")
                inPlay.Add(characters[i].GetCharacterData());
        }

        CharacterData randomChar = inPlay[UnityEngine.Random.Range(0, inPlay.Count)];
        String info = "I've never seen the " + randomChar.name + " paying their taxes.";
        return new ActedInfo(info, null);
    }
    public override string Description {
        get {
            return "Knows who doesn't pay their taxes.";
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
    public TaxCollector() : base(ClassInjector.DerivedConstructorPointer<TaxCollector>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public TaxCollector(IntPtr ptr) : base(ptr) { }
}