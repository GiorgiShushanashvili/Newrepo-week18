using System;
using System.ComponentModel.DataAnnotations;

namespace Mixed.Models
{
	public class Person
	{
		public int PersonId { get; set; }
		[DataType(DataType.Date)]
		public DateTime CreateDate { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Jobposition { get; set; }
		public double Workexperience { get; set; }
		public double? Salary { get; set; } = null;
		public Address PersonAddress { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;


    }
}

