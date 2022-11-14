using System;

namespace OnlineGameStore.DAL.Entities
{
    public class GamePlatformType
    {
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
        public Guid PlatformTypeId { get; set; }
        public virtual PlatformType PlatformType { get; set; }
    }
}
