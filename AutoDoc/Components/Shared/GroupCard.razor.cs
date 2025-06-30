using AutoDoc.Shared.Model.DTO.SectionGroupDTO;
using Microsoft.AspNetCore.Components;

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
        /// Režim prikaza kartice ("admin" ili "select").
        /// </summary>
        [Parameter] public string Mode { get; set; } = "admin";

        /// <summary>
        /// Event koji se poziva kada korisnik selektuje grupu (samo u "select" režimu).
        /// </summary>
        [Parameter] public EventCallback<SectionGroupGetDTO> OnSelect { get; set; }

        /// <summary>
        /// Obrada klika na karticu u "select" režimu.
        /// </summary>
        private async Task HandleClick()
        {
            if (Mode == "select" && OnSelect.HasDelegate)
            {
                await OnSelect.InvokeAsync(Group);
            }
        }
    }
}
