# Crossblade 
#### _Crossfades for your favourite Blazor website_

Crossblade is a Blazor component for enabling crossfades when navigating between pages on your Blazor website.
It works on every hosting model: Blazor WebAssembly (ASP.NET core hosted) or Blazor ServerSide. 
Crossfades will be triggered when navigating to different pages by default.   

> See www.CodeChops.nl for this package in action.

# Getting started

1. Install the package `CodeChops.Crossblade` in your Blazor project.
2. Wrap your main component in the `Crossblade` component.
3. Provide the following parameters (or don't provide them for the defaults):
   - `AnimationDuration`: The animation duration in milliseconds (default: 350ms).
   - `FireOnNavigationChanging`: Crossfade when navigating to different locations on your website (default: true).
4. Add `builder.Services.AddCrossblade()` to your `Program.cs` and provide the correct `RenderEnvironment` argument.

> If crossfades have to be triggered manually, simply call `await CrossBlade.Execute()`.

# Try it out
In order to try out this package, you need to execute the following easy steps: 
- Create a new Blazor ServerSide / WebAssembly (hosted) project from the default templates of .NET 7.
- Follow the [Getting started](#Getting-Started) steps above.
- Optionally add the following css to your `site.css` or `app.css` (depending on the hosting model):
```css
/* This will create a white background so the crossfades look smoother. */

main {
    background-color: white;
}
```

