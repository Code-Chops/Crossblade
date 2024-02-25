using Microsoft.AspNetCore.Components;

namespace CodeChops.Crossblade;

public partial class Crossblade
{
    [Inject] protected IJSRuntime JsRuntime { get; init; } = null!;
    [Inject] protected NavigationManager NavigationManager { get; init; } = null!;
    [Inject] protected RenderEnvironment RenderEnvironment { get; init; } = null!;

    /// <summary>
    /// In milliseconds.
    /// </summary>
    [Parameter] public int AnimationDuration { get; set; } = 350;

    /// <summary>
    /// If true (default) crossfades occur between page navigations.
    /// </summary>
    [Parameter] public bool FireOnNavigationChanging { get; set; } = true;

    /// <summary>
    /// The transition timing function (default: ease-in).
    /// </summary>
    [Parameter] public string TransitionTimingFunction { get; set; } = "ease-in";

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public bool IsAnimating { get; private set; }

    private bool StartAnimating { get; set; }
    private bool HasCopy { get; set; }
    private bool ShouldScrollUp { get; set; }
    private IJSObjectReference? JsObject { get; set; }
    private string? PreviousRelativeUrl { get; set; }
    private IDisposable? Registration { get; set; }

    protected override async Task OnInitializedAsync()
    {
        switch (this.RenderEnvironment)
        {
            case RenderEnvironment.WebassemblyHost:
                await base.OnInitializedAsync();
                return;

            // Unfortunately there is no correct way to detect prerendering for Blazor ServerSide: https://github.com/dotnet/aspnetcore/issues/17282.
            case RenderEnvironment.ServerSide:
                try
                {
                    this.JsObject = await this.JsRuntime.InvokeAsync<IJSObjectReference>("import",
                        "./_content/CodeChops.Crossblade/Crossblade.razor.js");
                }
                catch (Exception e) when (e is InvalidOperationException)
                {
                    // Do nothing
                }

                break;
            default:
                this.JsObject = await this.JsRuntime.InvokeAsync<IJSObjectReference>("import",
                    "./_content/CodeChops.Crossblade/Crossblade.razor.js");
                break;
        }

        this.PreviousRelativeUrl = new Uri(this.NavigationManager.Uri).PathAndQuery;

        if (this.FireOnNavigationChanging)
            this.Registration = this.NavigationManager.RegisterLocationChangingHandler(this.OnNavigationChangingAsync);

        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (this.RenderEnvironment is RenderEnvironment.WebassemblyHost)
            return;

        if (!this.HasCopy && this.StartAnimating && !firstRender)
            await this.StartAnimationAsync();

        if (this.ShouldScrollUp)
            await this.ScrollUpAsync();

        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task ScrollUpAsync()
    {
        if (this.JsObject is null
            || !this.ShouldScrollUp
            || new Uri(this.NavigationManager.Uri).PathAndQuery == this.PreviousRelativeUrl)
        {
            this.ShouldScrollUp = false;
            return;
        }

        await this.JsRuntime.InvokeVoidAsync("scrollUp", "crossblade-source");
        this.ShouldScrollUp = false;
    }

    private async ValueTask OnNavigationChangingAsync(LocationChangingContext context)
    {
        if (!this.FireOnNavigationChanging)
            return;

        var targetLocation = new Uri(context.TargetLocation).PathAndQuery;

        this.PreviousRelativeUrl = targetLocation;

        if (this.IsAnimating)
            await this.StopAnimationAsync();

        this.StartAnimating = true;

        this.StateHasChanged();
    }

    public async Task ExecuteFadeAsync()
    {
        this.StartAnimating = true;
        await this.OnAfterRenderAsync(firstRender: false);
    }

    private async Task StartAnimationAsync()
    {
        if (!this.StartAnimating)
            return;

        this.StartAnimating = false;
        this.IsAnimating = true;
        this.ShouldScrollUp = true;

        await this.CopyElementAsync();

        this.StateHasChanged();

        await Task.Delay(this.AnimationDuration)
            .ContinueWith(_ => this.StopAnimationAsync());
    }

    private async Task StopAnimationAsync()
    {
        this.IsAnimating = false;

        await this.RemoveElementAsync();

        this.StateHasChanged();
    }

    private async Task CopyElementAsync()
    {
        if (this.JsObject is null || this.HasCopy)
            return;

        await this.JsRuntime.InvokeAsync<string>("copyElement", "crossblade-source", "crossblade-copy");
        this.HasCopy = true;
    }

    private async Task RemoveElementAsync()
    {
        if (this.JsObject is null || !this.HasCopy)
            return;

        await this.JsRuntime.InvokeVoidAsync("removeElement", "crossblade-copy");
        this.HasCopy = false;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        this.Registration?.Dispose();
    }
}