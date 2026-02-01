using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;
using MelonLoader.ICSharpCode.SharpZipLib.Zip.Compression;

namespace MassHysteria;

[RegisterTypeInIl2Cpp]
public class Cakler : Demon {
    public override ActedInfo GetInfo(Character charRef) {
        return new ActedInfo("", null);
    }
    public override ActedInfo GetBluffInfo(Character charRef) {
        return new ActedInfo("", null);
    }
    public override string Description {
        get {
            return "All Outcasts are Mad as Villagers.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef) {
        if (trigger == ETriggerPhase.Start) {
            Health health = PlayerController.PlayerInfo.health;
            health.AddMaxHp(2);
            health.Heal(2);

            Gameplay gameplay = Gameplay.Instance;
            Characters instance = Characters.Instance;
            Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
            Il2CppSystem.Collections.Generic.List<Character> outcasts = instance.FilterRealCharacterType(characters, ECharacterType.Outcast);
            Il2CppSystem.Collections.Generic.List<Character> minions = instance.FilterRealCharacterType(characters, ECharacterType.Minion);
            Il2CppSystem.Collections.Generic.List<Character> villagers = instance.FilterRealCharacterType(characters, ECharacterType.Villager);
            if (villagers.Count is 0) return;
            CharacterData bluffToGive;
            bool duplicate;
            for (int i = 0; i < outcasts.Count; i++) {
                duplicate = false;
                do {
                    bluffToGive = villagers[UnityEngine.Random.Range(0, villagers.Count)].GetCharacterData();
                    if (bluffToGive.type != ECharacterType.Outcast)
                        duplicate = true;
                } while (!duplicate);
                outcasts[i].GiveBluff(bluffToGive);
                outcasts[i].statuses.AddStatus(MadnessStatic.mad, charRef);
                outcasts[i].statuses.AddStatus(ECharacterStatus.MessedUpByEvil, charRef);
                MadnessStatic.DetermineIfTruethful(outcasts[i], charRef);
            }
            for (int i = 0; i < minions.Count; i++) {
                int failCount = 0;
                int dieRoll = UnityEngine.Random.Range(0, 9);
                if (!minions[i].bluff) {
                    if (dieRoll >= 6) {
                        duplicate = false;
                        do {
                            bluffToGive = villagers[UnityEngine.Random.Range(0, villagers.Count)].GetCharacterData();
                            if (bluffToGive.type != ECharacterType.Outcast)
                                duplicate = true;
                            else failCount++;
                            if (failCount >= 9) break;
                        } while (!duplicate);
                    } else {
                      bluffToGive = instance.GetRandomUniqueVillagerBluff();
                      gameplay.AddScriptCharacterIfAble(ECharacterType.Villager, bluffToGive);
                    }
                    minions[i].GiveBluff(bluffToGive);
                } else if (minions[i].bluff.type == ECharacterType.Outcast) {
                    if (dieRoll <= 6) {
                        duplicate = false;
                        do {
                            bluffToGive = villagers[UnityEngine.Random.Range(0, villagers.Count)].GetCharacterData();
                            if (bluffToGive.type != ECharacterType.Outcast)
                                duplicate = true;
                            else failCount++;
                            if (failCount >= 9) break;
                        } while (!duplicate);
                    } else {
                      bluffToGive = instance.GetRandomUniqueVillagerBluff();
                      gameplay.AddScriptCharacterIfAble(ECharacterType.Villager, bluffToGive);
                    }
                    minions[i].GiveBluff(bluffToGive);
                }
            }
        }
    }
    public override CharacterData GetBluffIfAble(Character charRef)
    {
        Il2CppSystem.Collections.Generic.List<Character> characters = Gameplay.CurrentCharacters;
        characters = Characters.Instance.FilterRealCharacterType(characters, ECharacterType.Villager);
        CharacterData bluff;
        
        if (characters.Count is 0) return charRef.GetCharacterData();

        bool duplicate = false;
        do {
        bluff = characters[UnityEngine.Random.Range(0, characters.Count)].GetCharacterData();
        if (bluff.type != ECharacterType.Outcast)
            duplicate = true;
        } while (!duplicate);
        charRef.GiveBluff(bluff);
        charRef.statuses.AddStatus(MadnessStatic.mad, charRef);
        Gameplay.Instance.AddScriptCharacterIfAble(bluff.type, bluff);

        return bluff;
    }
    public Cakler() : base(ClassInjector.DerivedConstructorPointer<Cakler>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
    }
    public Cakler(IntPtr ptr) : base(ptr) { }
}