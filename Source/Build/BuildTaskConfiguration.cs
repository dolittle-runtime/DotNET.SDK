/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Applications.Configuration;
using Dolittle.Configuration;

namespace Dolittle.Build
{
    /// <summary>
    /// Represents the configuration object for the <see cref="BuildTask"/>
    /// </summary>
    public class BuildTaskConfiguration : IConfigurationObject
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BuildTaskConfiguration"/>
        /// </summary>
        /// <param name="boundedContextConfigPath">Path to the 'bounded-context.json' file</param>
        /// <param name="useModules">Wether or not to use modules in topology</param>
        /// <param name="namespaceSegmentsToStrip">Namespace segments to strip</param>
        /// <param name="generateProxies">Wether or not to generate proxies</param>
        /// <param name="proxiesBasePath">Base path for proxy output</param>
        /// <param name="dolittleFolder">Path to the .dolittle folder</param>
        public BuildTaskConfiguration(
            string boundedContextConfigPath,
            bool useModules,
            string namespaceSegmentsToStrip,
            bool generateProxies,
            string proxiesBasePath,
            string dolittleFolder)
        {
            BoundedContextConfigPath = boundedContextConfigPath;
            UseModules = useModules;
            NamespaceSegmentsToStrip = GetNamespacesToStripForAreaFor(namespaceSegmentsToStrip);
            GenerateProxies = generateProxies;
            ProxiesBasePath = GetProxiesBasePath(proxiesBasePath);
            DolittleFolder = dolittleFolder;
        }

        /// <summary>
        /// Gets the path to the 'bounded-context.json' file
        /// </summary>
        public string BoundedContextConfigPath {  get; }

        /// <summary>
        /// Gets wether or not to use modules as part of topology
        /// </summary>
        public bool UseModules {  get; }

        /// <summary>
        /// Gets namespace segments to strip
        /// </summary>
        public IDictionary<Area, IEnumerable<string>> NamespaceSegmentsToStrip {get; }

        /// <summary>
        /// Gets wether or not to generate proxies
        /// </summary>
        public bool GenerateProxies {  get; }

        /// <summary>
        /// Gets the base path for proxy generation for output
        /// </summary>
        public IEnumerable<string> ProxiesBasePath {  get; }

        /// <summary>
        /// Gets the path to the .dolittle folder
        /// </summary>
        public string DolittleFolder { get; }

        Dictionary<Area, IEnumerable<string>> GetNamespacesToStripForAreaFor(string value)
        {
            const char separator = '|';

            var namespaceSegmentsToStrip = new Dictionary<Area, IEnumerable<string>>();

            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return namespaceSegmentsToStrip;

            var segments = value.Split(separator);

            foreach (var segment in segments)
            {
                var splittedSegment = segment.Split('=');
                if (splittedSegment.Length != 2) throw new ArgumentException("Errors while parsing NamespaceSegmentsToStrip; It should look like this:\n<NamespaceSegmentsToStrip>NamespacePrefix1=This|NamespacePrefix2=Other");
                var area = new Area()
                {
                    Value = splittedSegment[0]
                };
                var namespaceSegment = splittedSegment[1];
                
                if (! namespaceSegmentsToStrip.ContainsKey(area))
                    namespaceSegmentsToStrip.Add(area, new List<string>());
                
                var values = namespaceSegmentsToStrip[area].ToList();
                values.Add(namespaceSegment);
                namespaceSegmentsToStrip[area] = values;
            }

            return namespaceSegmentsToStrip;
        }
        IEnumerable<string> GetProxiesBasePath(string value)
        {
            return value.Split('|');
        }
    }
}