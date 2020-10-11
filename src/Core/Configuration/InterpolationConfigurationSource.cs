// Copyright (c) TransPerfect. All rights reserved.
// See License.txt in the project root for license information.

using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace POC.Storage
{
    /// <summary>
    /// Interpolation configuration source.
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Configuration.IConfigurationSource" />
    public class InterpolationConfigurationSource : IConfigurationSource
    {
        private readonly Regex _pattern = new Regex("(?:{{(.*?)}})");
        private readonly IConfigurationRoot _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterpolationConfigurationSource"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public InterpolationConfigurationSource(IConfigurationRoot configuration)
            => _configuration = configuration;

        /// <summary>
        /// Builds the <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" /> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.Configuration.IConfigurationBuilder" />.</param>
        /// <returns>
        /// An <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" />
        /// </returns>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
            => new InterpolationConfigurationProvider(_configuration, _pattern);
    }
}
