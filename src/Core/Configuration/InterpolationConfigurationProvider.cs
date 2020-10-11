// Copyright (c) TransPerfect. All rights reserved.
// See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace POC.Storage
{
    /// <summary>
    /// InterpolationConfigurationProvider
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Configuration.IConfigurationProvider" />
    public class InterpolationConfigurationProvider : IConfigurationProvider
    {
        private readonly Regex _variablePattern;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _innerConfig;

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        protected Dictionary<string, string> Data { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterpolationConfigurationProvider"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="pattern">The pattern.</param>
        public InterpolationConfigurationProvider(Microsoft.Extensions.Configuration.IConfiguration configuration, Regex pattern)
        {
            _innerConfig = configuration;
            _variablePattern = pattern;
            Data = new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns the immediate descendant configuration keys for a given parent path based on this
        /// <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" />s data and the set of keys returned by all the preceding
        /// <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" />s.
        /// </summary>
        /// <param name="earlierKeys">The child keys returned by the preceding providers for the same parent path.</param>
        /// <param name="parentPath">The parent path.</param>
        /// <returns>
        /// The child keys.
        /// </returns>
        public IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            var prefix = parentPath == null ?
                string.Empty : parentPath + ConfigurationPath.KeyDelimiter;

            return Data
                .Where(kv =>
                    kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Select(kv => Segment(kv.Key, prefix.Length))
                .Concat(earlierKeys)
                .OrderBy(k => k, ConfigurationKeyComparer.Instance);
        }

        private static string Segment(string key, int prefixLength)
        {
            var indexOf = key.IndexOf(
                ConfigurationPath.KeyDelimiter,
                prefixLength,
                StringComparison.OrdinalIgnoreCase);

            return indexOf < 0 ?
                key.Substring(prefixLength) :
                key.Substring(prefixLength, indexOf - prefixLength);
        }

        /// <summary>
        /// Returns a change token if this provider supports change tracking, null otherwise.
        /// </summary>
        /// <returns>
        /// The change token.
        /// </returns>
        public IChangeToken GetReloadToken()
        {
            return _innerConfig.GetReloadToken();
        }

        /// <summary>
        /// Loads configuration values from the source represented by this <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" />.
        /// </summary>
        public void Load()
        {
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var config in _innerConfig.AsEnumerable())
            {
                var newValue = config.Value;
                if (!string.IsNullOrWhiteSpace(newValue))
                {
                    newValue = _variablePattern.Replace(newValue, (m) =>
                    {
                        var replacementValue = m.Value;
                        var variableName = m.Groups[1].Value;
                        if (!string.IsNullOrWhiteSpace(variableName))
                        {
                            if (!string.IsNullOrEmpty(_innerConfig.GetValue<string>(config.Key + "_" + variableName)))
                            {
                                replacementValue = _innerConfig.GetValue<string>
                                    (config.Key + "_" + variableName);
                            }
                            else if (!string.IsNullOrEmpty(_innerConfig.GetValue<string>(variableName)))
                            {
                                replacementValue = _innerConfig.GetValue<string>(variableName);
                            }
                        }
                        return replacementValue;
                    });
                }

                Data.Add(config.Key, newValue);
            }
        }

        /// <summary>
        /// Sets a configuration value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Set(string key, string value)
        {
            Data[key] = value;
        }

        /// <summary>
        /// Tries to get a configuration value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>True</c> if a value for the specified key was found, otherwise <c>false</c>.
        /// </returns>
        public bool TryGet(string key, out string? value)
        {
            return Data.TryGetValue(key, out value);
        }
    }
}
