using System;
using System.Linq;

namespace CodeFirstExample
{    
    class Program
    {
        static void Main(string[] args)
        {
            PlutoContext context = new PlutoContext();

            //LINQ syntax
            var query =
                from c in context.Categories
                where c.Name.Contains("Web")
                orderby c.Name
                select c;

            foreach(var category in query)
                Console.WriteLine(category.Name);

            // Linq Extension Methods
            var courses = context.Courses
                .Where(c => c.Name.Contains("C#"))
                .OrderBy(c => c.Name);
            
            foreach (var course in courses)
                Console.WriteLine(course.Name);
        }
    }
}
