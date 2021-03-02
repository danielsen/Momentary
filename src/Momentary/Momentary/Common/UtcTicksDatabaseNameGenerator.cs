using System;

namespace Momentary.Common
{
    /// <summary>
    /// <inheritdoc cref="IDatabaseNameGenerator"/>
    /// </summary>
    public sealed class UtcTicksDatabaseNameGenerator : IDatabaseNameGenerator
    {
        public Func<object> IdFunction { get; } = () => DateTime.UtcNow.Ticks;
        public string Prefix { get; } = "momentary";
        
        public string Generate()
        {
            return $"{Prefix}_{IdFunction()}";
        }
    }
}