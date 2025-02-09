using IndiLabbSkola.Models;

class DBAction
{   
    //Lägg till ny elev
    public void AddStudent()
    {
        Console.Clear();
        System.Console.WriteLine("Add student.");
        System.Console.Write("Enter firstname: ");
        string firstName = Console.ReadLine();
        System.Console.Write("Enter firstname: ");
        string lastName = Console.ReadLine();
        System.Console.Write("Enter ssn: ");
        string ssn = Console.ReadLine();
        System.Console.Write("Enter class id: ");
        int classId = Convert.ToInt32(Console.ReadLine());

        using(var context = new SchoolContext())
        {
            if(context.Classes.FirstOrDefault(c => c.ClassId == classId) != null 
            && ssn.All(char.IsDigit) && ssn.Length == 10)
            {
                var newStudent = new Student()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Ssn = ssn,
                    ClassId = classId

                };
                context.Students.Add(newStudent);
                System.Console.WriteLine("New student added.");
                
                context.SaveChanges();
                Console.ReadKey(intercept: true);
            }
            else{
                System.Console.WriteLine("Invalid input error.");
                Console.ReadKey(intercept: true);
            }
        }
    }

    // Visa alla elever i en klass
    public void ShowClass()
    {
        using(var context = new SchoolContext())
        {
            var classes = context.Classes.Select(c => c.ClassName).ToArray();
            //List<string> classNames = new List<string>();
            if(classes != null)
            {
                /*
                foreach(var c in classes)
                {
                    classNames.Add(c.ClassName);
                }
                */
                string classToShow = Menu(classes);
                var students = context.Students.Where(s => s.ClassId == context.Classes.FirstOrDefault(c => c.ClassName == classToShow).ClassId ).ToList();
                //int classID = classes.;
                string[] menuItems = {"Order by Firstname", "Order by Lastname"};
                string choice = Menu(menuItems);
                students = choice == menuItems[0] ? students.OrderBy(s => s.FirstName).ToList() : students.OrderBy(s => s.LastName).ToList();
                Console.Clear();
                System.Console.WriteLine($"Students in class {classToShow}");
                System.Console.WriteLine(new string('-',10));
                foreach(var s in students)
                {
                    System.Console.WriteLine($"{s.FirstName} {s.LastName}");
                }
                System.Console.WriteLine();
                Console.ReadKey(intercept: true);
            }
            else{
                System.Console.WriteLine("There are no classes in the database.");
            }
            
        }
    }

    public void ShowStaffByDep()
    {
        using(var context = new SchoolContext())
        {
            var staff = from s in context.Staff
                join d in context.Departments on s.DepartmentId equals d.DepartmentId
                orderby s.DepartmentId
                select new
                {
                    StaffFirstName = s.FirstName,
                    StaffLastName = s.LastName,
                    DepartmentName = d.DepartmentName
                };
            if(staff != null)
            {
                Console.Clear();
                System.Console.WriteLine("Staff");
                System.Console.WriteLine(new string('-',10));
                foreach(var s in staff)
                {
                    System.Console.WriteLine($"{s.StaffFirstName} {s.StaffLastName} | {s.DepartmentName}");
                }
                System.Console.WriteLine();
                Console.ReadKey(intercept: true);
            }
            else{
                System.Console.WriteLine("There are no students in the database.");
            }
        }
    }

    public void ShowActiveCourses()
    {
        using(var context = new SchoolContext())
        {
            var courses = context.Courses.AsQueryable().Where(c => c.IsActive == true);
            if(courses != null)
            {   
                
                Console.Clear();
                System.Console.WriteLine("Active courses");
                System.Console.WriteLine(new string('-',10));
                foreach(var c in courses)
                {
                    System.Console.WriteLine($"{c.CourseId} {c.CourseName}");
                }
                System.Console.WriteLine();
                Console.ReadKey(intercept: true);
            }
            else{
                System.Console.WriteLine("There are no active courses in the database.");
            }
        }
    }
    //visa alla elever
    public void ShowStudents()
    {
        using(var context = new SchoolContext())
        {
            var students = from s in context.Students
                join cl in context.Classes on s.ClassId equals cl.ClassId
                join c in context.Courses on cl.ClassId equals c.ClassId
                orderby cl.ClassId
                select new
                {
                    StudentFirstName = s.FirstName,
                    StudentLastName = s.LastName,
                    StudentClassName = cl.ClassName,
                    StudentCourseName = c.CourseName
                };
            if(students != null)
            {   
                
                Console.Clear();
                System.Console.WriteLine("Students");
                System.Console.WriteLine(new string('-',10));
                foreach(var s in students)
                {
                    System.Console.WriteLine($"{s.StudentFirstName} {s.StudentLastName} | {s.StudentClassName} | {s.StudentCourseName}");
                }
                System.Console.WriteLine();
                Console.ReadKey(intercept: true);
            }
            else{
                System.Console.WriteLine("There are no students in the database.");
            }
        }
    }

    public string Menu(string[] menuItems)
    {
        ConsoleKey key;
        int currentSelection = 0;
        do
        {
            Console.Clear();
            System.Console.WriteLine("Choose method to display data.");
            for(int i = 0; i < menuItems.Length; i++)
            {
                System.Console.WriteLine(i == currentSelection ? ">" + menuItems[i] : " " + menuItems[i]);
            }
            key = Console.ReadKey(intercept: true).Key;
            if(key == ConsoleKey.Tab)
            {
                currentSelection = currentSelection == menuItems.Length -1 ? 0 : currentSelection +1;
            }
        }while(key != ConsoleKey.Enter);
        return menuItems[currentSelection];
    }

}