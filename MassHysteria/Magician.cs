using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;

namespace MassHysteria;

[RegisterTypeInIl2Cpp]
public class Magician : Role {
    public override ActedInfo GetInfo(Character charRef) {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> madChars = Characters.Instance.FilterCharacterContainsStatus(characters, MadnessStatic.mad);
        Il2CppSystem.Collections.Generic.List<Character> randomInfo = new Il2CppSystem.Collections.Generic.List<Character>();

        String info = "";
        if (madChars.Count <= 0) {
            info = "Nobody is Mad. Strange.";
        } else {
            randomInfo.Add(madChars[UnityEngine.Random.Range(0, madChars.Count)]);
            info += "#" + randomInfo[0].id + " is mad.";
        }
        
        return new ActedInfo(info, randomInfo);
    }
    public override ActedInfo GetBluffInfo(Character charRef) {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        Il2CppSystem.Collections.Generic.List<Character> notMadChars = Characters.Instance.FilterCharacterMissingStatus(characters, MadnessStatic.mad);
        Il2CppSystem.Collections.Generic.List<Character> randomInfo = new Il2CppSystem.Collections.Generic.List<Character>();

        String info = "";
        if (notMadChars.Count <= 0) {
            info = "Nobody is Mad. Strange.";
        } else {
            randomInfo.Add(notMadChars[UnityEngine.Random.Range(0, notMadChars.Count)]);
            info += "#" + randomInfo[0].id + " is mad.";
        }
        
        return new ActedInfo(info, randomInfo);
    }
    public override string Description {
        get {
            return "Two Villagers are Mad as each other, learn 1 mad and sane character.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef)
    {
        if (trigger == ETriggerPhase.Start) {
            Characters instance = Characters.Instance;
            Il2CppSystem.Collections.Generic.List<Character> range1 = instance.GetCharactersAtRange(1, charRef);
            Il2CppSystem.Collections.Generic.List<Character> range2 = instance.GetCharactersAtRange(2, charRef);
            Il2CppSystem.Collections.Generic.List<Character> range3 = instance.GetCharactersAtRange(3, charRef);
            Il2CppSystem.Collections.Generic.List<Character> villagers1 = new Il2CppSystem.Collections.Generic.List<Character>();
            Il2CppSystem.Collections.Generic.List<Character> villagers2 = new Il2CppSystem.Collections.Generic.List<Character>();
            villagers1.Add(range1[0]);
            villagers2.Add(range1[1]);
            villagers1.Add(range2[0]);
            villagers2.Add(range2[1]);
            villagers1.Add(range3[0]);
            villagers2.Add(range3[1]);
            villagers1 = instance.FilterCharacterMissingStatus(villagers1, MadnessStatic.mad);
            villagers1 = instance.FilterRealCharacterType(villagers1, ECharacterType.Villager);
            villagers1 = instance.FilterBluffableCharacters(villagers1);
            villagers2 = instance.FilterCharacterMissingStatus(villagers2, MadnessStatic.mad);
            villagers2 = instance.FilterRealCharacterType(villagers2, ECharacterType.Villager);
            villagers2 = instance.FilterBluffableCharacters(villagers2);

            if (villagers1.Count <= 0) {
                charRef.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                return;
            } else if (villagers2.Count <= 0) {
                charRef.statuses.AddStatus(ECharacterStatus.Corrupted, charRef);
                return;
            }
            Character randomVill1;
            Character randomVill2;
            bool safetyCheck = false;
            do {
            randomVill1 = villagers1[UnityEngine.Random.Range(0, villagers1.Count)];
            randomVill2 = villagers2[UnityEngine.Random.Range(0, villagers2.Count)];
            if (randomVill1 != randomVill2)
                safetyCheck = true;
            } while (!safetyCheck);

            CharacterData bluff1 = randomVill1.dataRef;
            CharacterData bluff2 = randomVill2.dataRef;
            randomVill1.GiveBluff(bluff2);
            randomVill2.GiveBluff(bluff1);
            randomVill1.statuses.AddStatus(MadnessStatic.mad, charRef);
            randomVill2.statuses.AddStatus(MadnessStatic.mad, charRef);
            if (charRef.GetAlignment() is EAlignment.Evil){
                randomVill1.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                randomVill2.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
            }
            MadnessStatic.DetermineIfTruethful(randomVill1, charRef);
            MadnessStatic.DetermineIfTruethful(randomVill2, charRef);
        } else if (trigger == ETriggerPhase.Day)
            onActed?.Invoke(GetInfo(charRef));
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef) {
        if (trigger != ETriggerPhase.Day) return;
        onActed?.Invoke(GetBluffInfo(charRef));
    }
    public Magician() : base(ClassInjector.DerivedConstructorPointer<Magician>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Magician(IntPtr ptr) : base(ptr){}
}
