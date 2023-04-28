using System;
namespace Mixed.Models
{
	public class Role
	{
		public int id { get; set; }
		public const string Admin = "Admin";
		public const string User = "User";
		public int PersonId { get; set; }
	}
}

