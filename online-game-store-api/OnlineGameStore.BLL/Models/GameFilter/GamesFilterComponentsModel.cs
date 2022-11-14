using Microsoft.AspNetCore.Mvc;

namespace OnlineGameStore.BLL.Models
{
    public class GamesFilterComponentsModel
    {
        [FromQuery]
        public GamesFilterModel Filter { get; set; }
        [FromQuery]
        public GameSortModel Sort { get; set; }
        [FromQuery]
        public GamesPaginationModel Pagination { get; set; }
    }
}
