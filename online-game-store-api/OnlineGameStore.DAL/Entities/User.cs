using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineGameStore.DAL.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Guid? PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }
        [NotMapped]
        public IEnumerable<string> Roles { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
