using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Pipelines.Pipelines;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Pipelines.PipelineSteps
{
    public class GameGenresPipeline : IPipelineStep<List<Game>, List<Game>>
    {
        private readonly List<Genre> _genres;

        public GameGenresPipeline(IEnumerable<Genre> genres)
        {
            if (genres != null)
            {
                _genres = genres.ToList();
                for (int i = 0; i < _genres.Count(); i++)
                {
                    GenresChildrenAscend(_genres[i]);
                }
            }
        }

        private void GenresChildrenAscend(Genre genre)
        {
            if (genre != null)
            {

                foreach (Genre child in genre.ChildGenres)
                {
                    GenresChildrenAscend(child);
                    _genres.Add(child);
                }
            }
        }

        public List<Game> Process(List<Game> input)
        {
            if (_genres != null && _genres.Any())
            {
                return input
                    .Where(x => x.Genres != null)
                    .Where(x => x.Genres
                        .Select(x => x.GenreId)
                        .Intersect(_genres.Select(x => x.Id))
                        .Any()
                    ).ToList();
            }
            return input;
        }
    }
}
