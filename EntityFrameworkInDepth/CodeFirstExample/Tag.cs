using System.Collections.Generic;

namespace CodeFirstExample
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Course_Tag> CourseTags { get; set; }

        public Tag()
        {
            CourseTags = new HashSet<Course_Tag>();
        }
    }
}