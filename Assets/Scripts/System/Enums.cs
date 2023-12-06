public enum BattleStatus
{
    Defeat,
    Victory
}

enum EncounterType
{
    WhenInRange,
    Dialogue
}
enum TransitionType
{
    Normal,
    Additive
}

enum ActionType
{
    Load,
    Unload
}
public enum BattleState
{
    None,
    Setup,
    Battle,
    Victory,
    Defeat
}

public enum Turn
{
    Idle,
    Enemy,
    Ally
}

public enum TurnOptions
{
    Idle,
    Options,
    Target
}

enum CreatureType
{
    Ally,
    Enemy
}

public enum QuestState
{
    Requirements_Not_Met,
    Can_Start,
    In_Progress,
    Can_Finish,
    Completed,
    Hidden
}

public enum AllyPosition
{
    Position_1 = 1,
    Position_2 = 2,
    Position_3 = 3,
    Position_4 = 4
}

public enum DialogueTriggerType
{
    InRange,
    ByInteraction
}
