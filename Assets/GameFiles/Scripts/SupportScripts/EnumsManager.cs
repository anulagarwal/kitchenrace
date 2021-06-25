public enum CharacterAnimationState
{
    Idle,
    Run,
    Victory,
    Defeat,
    Stumble
};

public enum CharacterCode
{
    Player,
    Enemy_1,
    Enemy_2,
    Enemy_3,
    Enemy_4,
    Enemy_5,
    None
};

public enum AIMovementType
{
    Stacking,
    Building,
    ChangingStage,
    GameOver
};

public enum Item
{
    Sweet,
    Character
}

public enum PlayerMovementType
{
    Stacking
};

public enum StageNumber
{
    Stage_1,
    Stage_2,
    Stage_3,
    Stage_4,
    Stage_5
};

public enum SoundType
{
    Collect,
    Bump,
    UI,
    Victory,
    Bridge
}