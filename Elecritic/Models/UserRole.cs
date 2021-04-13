using System.Collections.Generic;

namespace Elecritic.Models {

    /// <summary>
    /// Role of a <see cref="User"/> to determine his/her permissions.
    /// </summary>
    public class UserRole {

        /// <summary>
        /// Unique identifier of the role.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the role, e.g. <c>"Administrator"</c>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Users that have the role.
        /// </summary>
        public virtual List<User> Users { get; set; }
    }
}
