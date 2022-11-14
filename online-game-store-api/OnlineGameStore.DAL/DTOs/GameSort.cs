using OnlineGameStore.Common.Enums;

namespace OnlineGameStore.DAL.DTOs
{
    public class GameSort
    {
        public GameSortMethod GameSortMethod { get; set; } = GameSortMethod.None;
        public bool IsAscending { get; set; }
    }
}
