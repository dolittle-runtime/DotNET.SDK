using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Applications.Configuration;

namespace Dolittle.Build.Topology
{
    internal static class StringExtensions
    {
        internal static ModuleDefinition GetModuleFromPath(this string path)
        {
            var splitPath = path.Split(".");
            var moduleName = splitPath.First();
            var module = new ModuleDefinition()
            {
                Module = Guid.NewGuid(),
                Name = moduleName
            };
            
            var featurePath = string.Join(".", splitPath.Skip(1));
            if (!string.IsNullOrEmpty(featurePath))
            {
                module.Features = new List<FeatureDefinition>()
                {
                    featurePath.GetFeatureFromPath()
                };
            }

            return module;
        }
        internal static FeatureDefinition GetFeatureFromPath(this string path)
        {
            var stringSegmentsReversed = path.Split(".").Reverse().ToArray();
            if (stringSegmentsReversed.Count() == 0) throw new Exception("Could not get feature from path");
            
            var currentFeature = new FeatureDefinition()
            {
                Feature = Guid.NewGuid(), 
                Name = stringSegmentsReversed[0]
            };
            foreach (var featureName in stringSegmentsReversed.Skip(1))
            {
                var parentFeature = new FeatureDefinition()
                {
                    Feature = Guid.NewGuid(),
                    Name = featureName,
                };
                parentFeature.SubFeatures = new List<FeatureDefinition>(){currentFeature};
                currentFeature = parentFeature;
            }

            return currentFeature;
        }
    }
}