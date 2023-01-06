namespace CodeChops.Crossblade;

/// <summary>
/// The environment in which crossblade is being executed.
/// </summary>
public abstract record RenderEnvironment
{
	/// <summary>
	/// Use for the Blazor webassembly host environment (server).
	/// </summary>
	public sealed record WebassemblyHost : RenderEnvironment;
	
	/// <summary>
	/// Use for the Blazor webassembly client environment (browser).
	/// </summary>
	public sealed record WebassemblyClient : RenderEnvironment;
	
	/// <summary>
	/// Use for server side Blazor.
	/// </summary>
	public sealed record ServerSide : RenderEnvironment;
}
