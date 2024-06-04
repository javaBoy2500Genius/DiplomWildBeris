using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWildBeris.Models
{

    public class User : RealmObject
    {
        [PrimaryKey, Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Login { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        [MapTo("role")]
        public int RoleRaw { get; set; }

        [Ignored]
        public UserRole Role
        {
            get => (UserRole)RoleRaw;
            set => RoleRaw = (int)value;
        }

    }


    public enum UserRole
    {
        User,
        Admin
    }
}
