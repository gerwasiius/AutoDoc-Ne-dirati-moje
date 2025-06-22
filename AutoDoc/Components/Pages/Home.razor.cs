using Microsoft.AspNetCore.Components;

namespace AutoDocFront.Components.Pages
{
    public partial class Home
    {
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
