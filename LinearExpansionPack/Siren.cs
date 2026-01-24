using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;

namespace LinearExpansionPack;

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
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> sortedChars = CharactersHelper.GetSortedListWithCharacterFirst(characters, charRef);
            Il2CppSystem.Collections.Generic.List<Character> adjacentGoodCharacters= new Il2CppSystem.Collections.Generic.List<Character>();;

            sortedChars[1].GetCharacterData();
            if (sortedChars[1].GetAlignment() == EAlignment.Good)
                adjacentGoodCharacters.Add(sortedChars[1]);
            if (sortedChars[sortedChars._size - 1].GetAlignment() == EAlignment.Good)
                adjacentGoodCharacters.Add(sortedChars[sortedChars._size - 1]);
            if (adjacentGoodCharacters.Count <= 0) return;
            
            Character randomChar = adjacentGoodCharacters[UnityEngine.Random.Range(0, adjacentGoodCharacters.Count)];
            CharacterData bluff;
            
            int diceRoll = Calculator.RollDice(10);
            if (diceRoll <= 6)
                bluff = Characters.Instance.GetRandomDuplicateBluff();                
            else {
                bluff = Characters.Instance.GetRandomUniqueBluff();
                Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);
            }

            randomChar.GiveBluff(bluff);
            randomChar.statuses.AddStatus(LinearStatic.mad, charRef);
            randomChar.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);

            // 40% chance for the mad character to give correct information; 100% if mad as themselves
            diceRoll = Calculator.RollDice(10);
            if (diceRoll >= 7)
                randomChar.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            if (randomChar.GetCharacterData().name == "Baker" || randomChar.GetCharacterData().name == "Slayer" || randomChar.GetCharacterData().name == "Alchemist")
                randomChar.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
            if (randomChar.GetCharacterData().name == randomChar.GetCharacterBluffIfAble().name)
                randomChar.statuses.AddStatus(ECharacterStatus.HealthyBluff, charRef);
            if (randomChar.GetCharacterData().name == "Drunk") {
                randomChar.statuses.AddStatus(ECharacterStatus.Corrupted, randomChar);
                randomChar.statuses.statuses.Remove(ECharacterStatus.HealthyBluff);
            }
        }
    }
    public Siren() : base(ClassInjector.DerivedConstructorPointer<Siren>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Siren(IntPtr ptr) : base(ptr) { }
}
