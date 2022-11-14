using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public int ViewsNumber { get; set; }
        public int CommentsNumber { get; set; }
        public DateTime Added { get; set; }
        public DateTime Published { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public short Discount { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        public short UnitsInStock { get; set; }

        [Required]
        public bool Discontinued { get; set; }
        public bool IsCommented { get; set; }
        public bool IsDownloaded { get; set; }

        public Guid? PublisherId { get; set; }
        public PublisherModel Publisher { get; set; }

        public IEnumerable<GenreModel> Genres { get; set; }

        public IEnumerable<PlatformTypeModel> PlatformTypes { get; set; }

        public GameModel()
        {
            ViewsNumber = 0;
            Price = Math.Round(Price, 2);
        }
    }
}
