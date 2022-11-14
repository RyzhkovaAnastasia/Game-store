using OnlineGameStore.Common.Enums;

namespace OnlineGameStore.BLL.Models
{
    public class GameSortModel
    {
        public GameSortMethod GameSortMethod { get; set; }
        public bool IsAscending { get; set; }
    }
}
