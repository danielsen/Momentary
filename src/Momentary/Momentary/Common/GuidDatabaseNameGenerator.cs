using System;

namespace Momentary.Common
{
    /// <summary>
    /// <inheritdoc cref="IDatabaseNameGenerator"/>
    /// </summary>
    public class GuidDatabaseNameGenerator : IDatabaseNameGenerator
    {
        public Func<object> IdFunction { get; } = () => Guid.NewGuid().ToString("N");
        public string Prefix { get; } = "momentary";
        
        public string Generate()
        {
            return $"{Prefix}_{IdFunction()}";
        }
    }
}