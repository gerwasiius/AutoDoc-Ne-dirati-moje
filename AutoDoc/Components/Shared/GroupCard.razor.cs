using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Shared
{
    public partial class GroupCard
    {
        /// <summary>
        /// Grupa čije se informacije prikazuju u kartici.
        /// </summary>
        [Parameter] public SectionGroupGetDTO Group { get; set; }

        /// <summary>
        /// Event koji se poziva kada korisnik klikne na dugme za izmjenu grupe.
        /// </summary>
        [Parameter] public EventCallback<SectionGroupGetDTO> OnEdit { get; set; }

        /// <summary>
        /// Event koji se poziva kada korisnik klikne na dugme za prikaz članova grupe.
        /// </summary>
        [Parameter] public EventCallback<SectionGroupGetDTO> OnViewMembers { get; set; }

        /// <summary>
        /// Event koji se poziva kada korisnik klikne na dugme pregleda grupe
        /// </summary>
        [Parameter] public EventCallback<SectionGroupGetDTO> OnView { get; set; }

        /// <summary>
        /// Režim prikaza kartice ("admin" ili "select").
        /// </summary>
        [Parameter] public string Mode { get; set; } = "admin";

        /// <summary>
        /// Event koji se poziva kada korisnik selektuje grupu (samo u "select" režimu).
        /// </summary>
        [Parameter] public EventCallback<SectionGroupGetDTO> OnSelect { get; set; }
        [Inject] private IToastService ToastService { get; set; } = default!;


        /// <summary>
        /// Obrada klika na karticu u "select" režimu.
        /// </summary>
        private async Task HandleClick()
        {
            try
            {
                if (Mode == "select" && OnSelect.HasDelegate && Group != null)
                    await OnSelect.InvokeAsync(Group);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška pri selektovanju grupe: {ex.Message}");
            }
        }

        /// <summary>
        /// Obrada klika metoda na view
        /// </summary>
        /// <returns></returns>
        private async Task OnViewClicked()
        {
            try
            {
                if (OnView.HasDelegate && Group != null)
                    await OnView.InvokeAsync(Group);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Greška pri pregledu grupe: {ex.Message}");
            }
        }

    }
}
