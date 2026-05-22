using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace lab9_RPM
{
    public partial class Contacts
    {
        public Contacts() { }  // Пустой конструктор для EF

        public Contacts(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
