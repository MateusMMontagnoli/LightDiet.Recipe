namespace LightDiet.Recipe.Domain.SeedWork.SearchableRepository;

public class SearchOutput<TAggretate> where TAggretate : class
{
    public int CurrentPage { get; set; }

    public int PerPage { get; set; }

    public int Total { get; set; }

    public IReadOnlyList<TAggretate> Items { get; set; }

    public SearchOutput(
        int currentPage, 
        int perPage, 
        int total, 
        IReadOnlyList<TAggretate> items)
    {
        CurrentPage = currentPage;
        PerPage = perPage;
        Total = total;
        Items = items;
    }
}

