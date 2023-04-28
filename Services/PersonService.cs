using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Mixed.Models;
using Mixed.Connections;

namespace Mixed.Services
{
    public class PersonService : IPersonInterface
    {
        private readonly PersonContext _person;
        
        public PersonService(PersonContext person)
        {
            _person = person;
        }


        public Person Adduser(Person person)
        {
            foreach(var pers in _person.Persons)
            {
                if (pers.Username == person.Username)
                {
                    return null;
                }
            }
            _person.Add(person);
            _person.SaveChanges();
            return person;
        }


        public void Delete(int id)
        {
            Person person = _person.Persons.FirstOrDefault(x=>x.PersonId==id);
            _person.Persons.Remove(person);
            _person.SaveChanges();
            return;
        }

        public List<Person> Filter(string City, double Salary)
        {
            var person = _person.Persons.Include(a => a.PersonAddress).
                Where(x => x.PersonAddress.City == City && x.Salary == Salary);
            return person.ToList();
        }

        public Person Find(int id)
        {
            var person = _person.Persons.Include(x => x.PersonAddress).
                FirstOrDefault(a => a.PersonId == id);
            return person;
        }

        public List<Person> GetAll()
        {
            return _person.Persons.Include(x => x.PersonAddress).ToList();
        }

        public Person Login(LoginUser modeluser)
        {
            if (string.IsNullOrEmpty(modeluser.Username)
                || string.IsNullOrEmpty(modeluser.Password))
                return null;

            var user = _person.Persons.SingleOrDefault
                (x => x.Username == modeluser.Username);
            if (user == null)
                return null;
            if (Passwordhasher.hashPass(modeluser.Password) != user.Password)
                return null;
            return user;

        }

        public Person Update(Person loginmodel)
        {
            if (string.IsNullOrEmpty(loginmodel.Username)
                || string.IsNullOrEmpty(loginmodel.Password))
                return null;

            var user = _person.Persons.SingleOrDefault
                (x => x.Username == loginmodel.Username);
            if (user == null)
                return null;
            if (Passwordhasher.hashPass(loginmodel.Password) != user.Password)
                return null;
            _person.Persons.Update(user);
            _person.SaveChanges();
            return user;
    
        }

        
    }
}

