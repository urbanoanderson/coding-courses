using System.Collections.Generic;

namespace CodeFirstExample
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public Author()
        {
            Courses = new HashSet<Course>();
        }
    }
}