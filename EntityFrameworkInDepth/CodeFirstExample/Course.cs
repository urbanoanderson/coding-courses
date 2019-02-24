using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstExample
{
    public enum CourseLevel
    {
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public CourseLevel Level { get; set; }

        public Category Category { get; set; }

        public DateTime? DatePublished { get; set; }
        public float FullPrice { get; set; }
        public Author Author { get; set; }
        public virtual ICollection<Course_Tag> CourseTags { get; set; }

        public Course()
        {
            CourseTags = new HashSet<Course_Tag>();
        }
    }
}