using System.Collections.Generic;

namespace OnlineGameStore.BLL.Models
{
    public class FilteredGameModel
    {
        public IEnumerable<GameModel> Games { get; set; }
        public int AllGameNumber { get; set; }
    }
}
