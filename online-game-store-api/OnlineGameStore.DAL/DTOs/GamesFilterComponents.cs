namespace OnlineGameStore.DAL.DTOs
{
    public class GamesFilterComponents
    {
        public GamesFilter Filter { get; set; }
        public GameSort Sort { get; set; } = new GameSort();
        public GamesPagination Pagination { get; set; }
    }
}
