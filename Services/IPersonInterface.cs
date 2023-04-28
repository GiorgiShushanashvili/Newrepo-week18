using System;
using Mixed.Models;
namespace Mixed.Services
{
	public interface IPersonInterface
	{
		Person Login(LoginUser modeluser);
		Person Adduser(Person person);
		Person Find(int id);
		List<Person> GetAll();
		Person Update(Person user);
		List<Person> Filter(string City, double Salary);
		void Delete(int id);

	}
}

