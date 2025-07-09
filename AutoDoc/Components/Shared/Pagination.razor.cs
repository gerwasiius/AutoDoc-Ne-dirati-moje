using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AutoDocFront.Components.Shared;

public partial class Pagination
{
    [Parameter] public int CurrentPage { get; set; }
    [Parameter] public int TotalPages { get; set; }
    [Parameter] public EventCallback<int> OnPageChanged { get; set; }
    [Inject] private IToastService ToastService { get; set; } = default!;

    private IEnumerable<int> PageNumbers
    {
        get
        {
            var pages = new List<int>();
            int maxPagesToShow = 5;
            int startPage;
            int endPage;

            if (TotalPages <= maxPagesToShow)
            {
                startPage = 1;
                endPage = TotalPages;
            }
            else
            {
                if (CurrentPage <= 3)
                {
                    startPage = 1;
                    endPage = maxPagesToShow;
                }
                else if (CurrentPage + 2 >= TotalPages)
                {
                    startPage = TotalPages - (maxPagesToShow - 1);
                    endPage = TotalPages;
                }
                else
                {
                    startPage = CurrentPage - 2;
                    endPage = CurrentPage + 2;
                }
            }

            for (int i = startPage; i <= endPage; i++)
                pages.Add(i);

            return pages;
        }
    }

    private bool ShowEllipsis => TotalPages > 5 && PageNumbers.Last() < TotalPages;

    private async Task ChangePage(int page)
    {
        try
        {
            if (page < 1 || page > TotalPages || page == CurrentPage)
                return;

            if (OnPageChanged.HasDelegate)
                await OnPageChanged.InvokeAsync(page);
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"Greška prilikom promjene stranice: {ex.Message}");
        }
    }
}