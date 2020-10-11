// Copyright (c) TransPerfect. All rights reserved.
// See License.txt in the project root for license information.

using Microsoft.Extensions.Configuration;

namespace POC.Storage
{
    /// <summary>
    /// Interpolation configuration extensions.
    /// </summary>
    public static class InterpolationConfigurationExtensions
    {
        /// <summary>
        /// Adds the interpolation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddInterpolation(this IConfigurationBuilder builder, IConfigurationRoot config)
            => builder.Add(new InterpolationConfigurationSource(config));

        /// <summary>
        /// Adds the interpolation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddInterpolation(this IConfigurationBuilder builder, IConfigurationBuilder config)
            => AddInterpolation(builder, config.Build());

        /// <summary>
        /// Adds the interpolation.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IConfigurationBuilder AddInterpolation(this IConfigurationBuilder builder)
            => AddInterpolation(builder, builder.Build());
    }
}
