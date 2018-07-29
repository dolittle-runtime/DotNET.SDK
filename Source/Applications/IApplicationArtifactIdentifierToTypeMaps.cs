using System;

namespace Dolittle.Applications
{
    /// <summary>
    /// Defines a system that is capable of mapping between <see cref="Type"/> or <see cref="object"/> to <see cref="IApplicationArtifactIdentifier"/>
    /// </summary>
    public interface IApplicationArtifactIdentifierAndTypeMaps
    {
        /// <summary>
        /// Maps from <see cref="object"/> to <see cref="IApplicationArtifactIdentifier"/>
        /// </summary>
        /// <param name="resource">The <see cref="object"/> to be mapped to <see cref="IApplicationArtifactIdentifier"/></param>
        /// <returns></returns>
        IApplicationArtifactIdentifier GetIdentifierFor(object resource);
        
        /// <summary>
        /// Maps from <see cref="Type"/> to <see cref="IApplicationArtifactIdentifier"/>
        /// </summary>
        /// <param name="type">The <see cref="Type"/>to be mapped to <see cref="IApplicationArtifactIdentifier"/></param>
        /// <returns></returns>
        IApplicationArtifactIdentifier GetIdentifierFor(Type type);
        
        /// <summary>
        /// Maps from <see cref="IApplicationArtifactIdentifier"/> to <see cref="Type"/>
        /// </summary>
        /// <param name="artifactIdentifier"></param>
        /// <returns></returns>
        Type GetTypeFor(IApplicationArtifactIdentifier artifactIdentifier);
    }
}