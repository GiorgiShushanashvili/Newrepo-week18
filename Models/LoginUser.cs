using System;
using System.ComponentModel.DataAnnotations;
namespace Mixed.Models
{
	public class LoginUser
	{
		public int id { get; set; }
		[Required(ErrorMessage ="Username is required")]
		public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
    }
}

