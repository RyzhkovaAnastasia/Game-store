using OnlineGameStore.Common.Enums;
using System.Collections.Generic;

namespace OnlineGameStore.BLL.Models
{
    public class GamesFilterModel
    {
        public string Name { get; set; }
        public GamePublishedDate Published { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public IEnumerable<GenreModel> Genres { get; set; }
        public IEnumerable<PlatformTypeModel> Platforms { get; set; }
        public IEnumerable<PublisherModel> Publishers { get; set; }
    }
}
