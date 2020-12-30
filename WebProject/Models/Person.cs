using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{

    //Used to store data of a person from the database
    public class Person : IEquatable<Person>
    {
        public int id { set; get; }
        public string firstName { set; get; }   
        public string lastName { set; get; }    

        public Person(int id, string firstName, string lastName)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public bool Equals(Person other)
        {
            if (other == null) return false;

            if (this.id == other.id && this.firstName.Equals(other.firstName) && this.lastName.Equals(other.lastName))
                return true;

            return false;
        }
    }
}