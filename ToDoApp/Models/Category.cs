using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToDoApp.Models
{
    [Serializable]
    public class Category
    {
        [XmlAttribute]
        public int Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        public Category()
        {
            // empty
        }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
