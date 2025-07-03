using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class Home
    {
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private bool _initialized = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _initialized = true;
                StateHasChanged();
            }
        }
    }
}
