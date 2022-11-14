using OnlineGameStore.Common.Enums;
using OnlineGameStore.DAL.Entities;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.DTOs
{
    public class GamesFilter
    {
        public string Name { get; set; }
        public GamePublishedDate Published { get; set; } = GamePublishedDate.allTime;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<PlatformType> Platforms { get; set; }
        public IEnumerable<Publisher> Publishers { get; set; }
    }
}
