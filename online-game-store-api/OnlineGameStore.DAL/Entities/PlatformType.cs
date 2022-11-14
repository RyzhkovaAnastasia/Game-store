using System.Collections.Generic;

namespace OnlineGameStore.DAL.Entities
{
    public class PlatformType : BaseEntity
    {
        public string Type { get; set; }
        public virtual ICollection<GamePlatformType> Games { get; set; }
    }
}
