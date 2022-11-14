using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.Models
{
    public class GenreModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        public Guid? ParentGenreId { get; set; }
        public GenreModel ParentGenre { get; set; }
        public IEnumerable<GenreModel> Children { get; set; }
    }
}
