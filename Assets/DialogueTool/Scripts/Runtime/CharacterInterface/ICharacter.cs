using dialogues.node;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    string CharacterName { get; }
    CharacterType CharacterType_ { get; }
    Sprite CharacterIcon { get; }
}
