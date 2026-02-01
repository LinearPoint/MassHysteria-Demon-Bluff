using System.ComponentModel;
using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;

namespace MassHysteria;

[RegisterTypeInIl2Cpp]
public class Siren : Minion {
    public override ActedInfo GetInfo(Character charRef) {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef) {
        return new ActedInfo("", null);
    }
    public override string Description {
        get {
            return "One of her Good neighbors is mad.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef) {
        if (trigger == ETriggerPhase.Start) {
            Characters instance = Characters.Instance;
            Il2CppSystem.Collections.Generic.List<Character> neighbors = instance.GetAdjacentCharacters(charRef);

            if (neighbors[1].GetAlignment() != EAlignment.Good || neighbors[1].statuses.Contains(MadnessStatic.mad))
                neighbors.RemoveAt(1);
            if (neighbors[0].GetAlignment() != EAlignment.Good || neighbors[0].statuses.Contains(MadnessStatic.mad))
                neighbors.RemoveAt(0);
            if (neighbors.Count <= 0) return;

            Character randomChar = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];

            // Disguise Logic
            bool isMadAsSelf = true;
            CharacterData bluff;
            do {
                bluff = GetBluffIfAble(charRef);
                if (bluff != randomChar.GetCharacterData())
                    isMadAsSelf = false;
            } while (isMadAsSelf);

            randomChar.GiveBluff(bluff);
            randomChar.statuses.AddStatus(MadnessStatic.mad, charRef);
            randomChar.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
            MadnessStatic.DetermineIfTruethful(randomChar, charRef);
        }
    }
    public Siren() : base(ClassInjector.DerivedConstructorPointer<Siren>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Siren(IntPtr ptr) : base(ptr) { }
}