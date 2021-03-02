using System;

namespace Momentary.Common
{
    /// <summary>
    /// Creates a unique database name.
    /// </summary>
    public interface IDatabaseNameGenerator
    {
        /// <summary>
        /// Function to create  reasonably random database id.
        /// </summary>
        Func<object> IdFunction { get; }
        
        /// <summary>
        /// Database name prefix.
        /// </summary>
        string Prefix { get; }
        
        /// <summary>
        /// Generate the database name. Should join the value of <see cref="Prefix"/> and the output of <see cref="IdFunction"/>
        /// </summary>
        /// <returns>The generated name.</returns>
        string Generate();
    }
}