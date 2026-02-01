using Il2CppInterop.Runtime.Injection;
using Il2CppInterop.Runtime.InteropTypes;
using MelonLoader;

namespace MassHysteria;

[RegisterTypeInIl2Cpp]
public class Actor : Role {
    Character? chRef;
    bool checkedDead;
    bool checkedOuted;
    private Il2CppSystem.Action characterPickedAction;
    private Il2CppSystem.Action stopPickAction;
    private Il2CppSystem.Action characterPickedDrunkAction;
    public override ActedInfo GetInfo(Character charRef) {
        string info = "";
        if (checkedDead) {
            charRef.Init(charRef.GetCharacterData(), charRef.id);
            checkedDead = false;
        } else if (checkedOuted) {
            charRef.Init(charRef.GetCharacterData(), charRef.id);
            checkedOuted = false;
        }
        return new ActedInfo(info, null);
    }
    public override ActedInfo GetBluffInfo(Character charRef) {
        return new ActedInfo("", null);
    }
    public override string Description {
        get {
            return "Mad as chosen character's Role or Bluff.";
        }
    }
    public override void Act(ETriggerPhase trigger, Character charRef) {
        if (trigger == ETriggerPhase.Day) {
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(1);
            CharacterPicker.OnCharactersPicked += characterPickedAction;
            CharacterPicker.OnStopPick += stopPickAction;
        }
    }
    public override void BluffAct(ETriggerPhase trigger, Character charRef) {
        if (trigger == ETriggerPhase.Day) {
            chRef = charRef;
            CharacterPicker.Instance.StartPickCharacters(1);
            CharacterPicker.OnCharactersPicked += characterPickedDrunkAction;
            CharacterPicker.OnStopPick += stopPickAction;
        }
    }
    private void CharacterPicked() {
        if (chRef == null) return;
        CharacterPicker.OnCharactersPicked -= characterPickedAction;
        CharacterPicker.OnStopPick -= stopPickAction;

        Character target = CharacterPicker.PickedCharacters[0];
        if (target.state is ECharacterState.Dead) {
            checkedDead = true;
            onActed?.Invoke(GetInfo(charRef));
            charRef.RefreshCharacter();
            return;
        }
        CharacterData madRole;

        // checking if targeted character is evil or good to apply messedupbyevil or not
        if (target.GetAlignment() is EAlignment.Evil) {
            chRef.statuses.AddStatus(ECharacterStatus.MessedUpByEvil, target);
            if (!target.bluff) {
                checkedOuted = true;
                onActed?.Invoke(GetInfo(charRef));
                charRef.RefreshCharacter();
                return;
            }
            madRole = target.bluff;
        } else {
            if (!target.bluff)
                madRole = target.GetCharacterData();
            else
                madRole = target.bluff;
        }
        
        chRef.GiveBluff(madRole);
        chRef.statuses.AddStatus(MadnessStatic.mad, chRef);
        MadnessStatic.DetermineIfTruethful(chRef, chRef);
        chRef.RevealBluff();
        chRef.RefreshCharacter();
        chRef.Act(ETriggerPhase.Day);
        if (chRef.state == ECharacterState.Dead) {
            chRef.RevealAllReal();
            chRef.state = ECharacterState.Dead;
        }
    }
    private void CharacterPickedDrunk() {
        if (chRef is null) return;
        CharacterPicker.OnCharactersPicked -= characterPickedDrunkAction;
        CharacterPicker.OnStopPick -= stopPickAction;

        Character target = CharacterPicker.PickedCharacters[0];
        if (target.state is ECharacterState.Dead) {
            checkedDead = true;
            onActed?.Invoke(GetInfo(charRef));
            charRef.RefreshCharacter();
            return;
        } else if (target.GetAlignment() is EAlignment.Evil && !target.bluff) {
            checkedOuted = true;
            onActed?.Invoke(GetInfo(charRef));
            charRef.RefreshCharacter();
            return;
        }
        CharacterData madRole;
        bool amICorrupted = false;
        if (chRef.statuses.Contains(ECharacterStatus.Corrupted))
            amICorrupted = true;
        if (!target.bluff)
            madRole = target.GetCharacterData();
        else
            madRole = target.bluff;

        chRef.GiveBluff(madRole);
        if (amICorrupted && !chRef.statuses.Contains(ECharacterStatus.Corrupted))
            chRef.statuses.AddStatus(ECharacterStatus.Corrupted, chRef);
        //chRef.bluff.name = "Actor";
        chRef.RevealBluff();
        chRef.RefreshCharacter();
        chRef.Act(ETriggerPhase.Day);
        if (chRef.state == ECharacterState.Dead)
            chRef.RevealAllReal();
    }
    private void StopPick() {
        CharacterPicker.OnCharactersPicked -= characterPickedDrunkAction;
        CharacterPicker.OnCharactersPicked -= characterPickedAction;
        CharacterPicker.OnStopPick -= stopPickAction;
    }
    public Actor() : base(ClassInjector.DerivedConstructorPointer<Actor>()) {
        ClassInjector.DerivedConstructorBody((Il2CppObjectBase)this);
        characterPickedAction = new System.Action(CharacterPicked);
        stopPickAction = new System.Action(StopPick);
        characterPickedDrunkAction = new System.Action(CharacterPickedDrunk);
    }
    public Actor(IntPtr ptr) : base(ptr)
    {
        characterPickedAction = new System.Action(CharacterPicked);
        stopPickAction = new System.Action(StopPick);
        characterPickedDrunkAction = new System.Action(CharacterPickedDrunk);
    }
}