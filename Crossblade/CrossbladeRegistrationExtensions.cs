using Microsoft.Extensions.DependencyInjection;

namespace CodeChops.Crossblade;

/// <summary>
/// Call <see cref="AddCrossblade"/> on startup to activate crossfade functionality on your website.
/// </summary>
public static class CrossbladeRegistrationExtensions
{
	public static IServiceCollection AddCrossblade(this IServiceCollection services, RenderEnvironment renderEnvironment)
	{
		services.AddSingleton(renderEnvironment);

		return services;
	}
}
