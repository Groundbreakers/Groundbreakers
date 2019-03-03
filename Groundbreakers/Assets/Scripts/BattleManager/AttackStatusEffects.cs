namespace BattleManager
{
    using System;

    /// <summary>
    /// I assume the attack effects are handled in 'CharacterAttributes' Class in the following ways:
    /// 1. Having a field of type 'AttackEffects' that store attack effects.
    /// 2. since this is a flag, which means you should use 'AttackEffects.HasFlag() Method'
    /// to check if one has the desired effects. 
    /// </summary>
    /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.enum.hasflag?view=netframework-4.7.2"/>
    [Flags]
    public enum AttackEffects
    {
        /// <summary>
        /// Release multiple projectiles at once. [Ranged]
        /// </summary>
        Burst,

        /// <summary>
        /// Projectiles bounce to other enemies. [Ranged]
        /// </summary>
        Ricochet,

        /// <summary>
        /// Does laser things. [Ranged]
        /// </summary>
        Laser,

        /// <summary>
        /// Damage all enemies around target. [Ranged]
        /// </summary>
        Splash,

        /// <summary>
        /// Projectiles pierce through enemies and obstacles. [Ranged]
        /// </summary>
        Pierce,

        /// <summary>
        /// Projectiles chase enemies. [Ranged]
        /// </summary>
        Trace,

        /// <summary>
        /// Damage all enemies in front. [Melee]
        /// </summary>
        Cleave,

        /// <summary>
        /// Damage all enemies within range [Melee]
        /// </summary>
        Whirlwind,

        /// <summary>
        /// Melee attacks become ranged. [Melee]
        /// </summary>
        Reach,
    }

    /// <summary>
    /// I assume these effects are handled within 'EnemyGeneric' class. Do the same as above.
    /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.enum.hasflag?view=netframework-4.7.2"/>
    /// </summary>
    [Flags]
    public enum StatusEffects
    {
        /// <summary>
        /// Half movement speed.
        /// </summary>
        Slow,

        /// <summary>
        /// Cannot move.
        /// </summary>
        Stun,

        /// <summary>
        /// Lose HP over time.
        /// </summary>
        Burn,

        /// <summary>
        /// 25% more damage taken.
        /// </summary>
        Mark,

        /// <summary>
        /// Disable Auras and Revenges
        /// </summary>
        Purge,

        /// <summary>
        /// Disable Armor.
        /// </summary>
        Break,

        /// <summary>
        /// Lose HP over time (stacks).
        /// </summary>
        Blight,

        /// <summary>
        /// 0% chance to dodge.
        /// </summary>
        Net,

        /// <summary>
        /// Slow and 0% chance to dodge. [Scavenger Only]
        /// </summary>
        Trap,
    }

    /// <summary>
    /// Same as all above. I assume this is handled in Character's code.
    /// </summary>
    [Flags]
    public enum MonsterEffects
    {
        /// <summary>
        /// Cannot perform any action.
        /// </summary>
        Stun,

        /// <summary>
        /// 50% chance to miss attacks.
        /// </summary>
        Blind,

        /// <summary>
        /// Disable all abilities.
        /// </summary>
        Plague,

        /// <summary>
        /// Halve POW.
        /// </summary>
        Interfere,

        /// <summary>
        /// Cannot Retreat or Deploy.
        /// </summary>
        Shackle,

        /// <summary>
        /// Movement + 50%.
        /// </summary>
        HasteAura,
    }
}