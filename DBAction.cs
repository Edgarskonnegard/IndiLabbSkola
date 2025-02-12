using IndiLabbSkola.Models;
using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Update.Internal;

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
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine("Choose method to display data.");
            Console.ResetColor();
            System.Console.WriteLine();
            for(int i = 0; i < menuItems.Length; i++)
            {
                if(currentSelection==i)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    System.Console.WriteLine(new string(' ', 2) + menuItems[i]);
                    Console.ResetColor();
                    
                }
                else
                {
                    System.Console.WriteLine(new string(' ', 3) + menuItems[i]);
                }
            }
            key = Console.ReadKey(intercept: true).Key;
            if(key == ConsoleKey.Tab)
            {
                currentSelection = currentSelection == menuItems.Length -1 ? 0 : currentSelection +1;
            }
            else if(key == ConsoleKey.UpArrow)
            {
                currentSelection = currentSelection == 0 ? menuItems.Length -1 : currentSelection -1;
            }
            else if(key == ConsoleKey.DownArrow)
            {
                currentSelection = currentSelection == menuItems.Length -1 ? 0 : currentSelection +1;
            }

        }while(key != ConsoleKey.Enter);
        return menuItems[currentSelection];
    }

    //#####################################################################################################################################################
    // Ändringar efter inlämning
    //#####################################################################################################################################################
    public void ShowStaff()
    {
        string connectionString = "Server=localhost;Database=School;User Id=sa;Password=YourPassword123; Trust Server Certificate=True;";
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"SELECT st.FirstName, st.LastName, st.Position, DATEDIFF(YEAR, st.EmploymentDate, GETDATE()) AS YearsEmployed FROM Staff st";
            try
            {

                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.Clear();
                        System.Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Console.WriteLine("| {0,-20} | {1,-16} | {2,5} |", "Name", "Position", "Years employed");
                        Console.ResetColor();
                        System.Console.WriteLine(new string('-', 60));
                        while(reader.Read())
                        {
                            string firstName = reader.IsDBNull(0) ? "N/A" : reader.GetString(0);
                            string lastName = reader.IsDBNull(1) ? "N/A" : reader.GetString(1);
                            string position = reader.IsDBNull(2) ? "N/A" : reader.GetString(2);
                            int yearsEmployed = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                            System.Console.WriteLine("| {0,-20} | {1,-16} | {2,14} |",$"{firstName} {lastName} ", position , yearsEmployed);

                        }
                    }
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                System.Console.WriteLine("Internal error: " + ex.Message);
            }
        }
        Console.ReadKey(intercept: true);
        
    }

    public void AddNewStaff()
    {
        //Ta in variablerna
        System.Console.WriteLine("Add new employee");
        System.Console.Write("First name: ");
        string firstName = Console.ReadLine();
        System.Console.Write("Last name: ");
        string lastName = Console.ReadLine();
        System.Console.Write("SSN: ");
        string ssn = Console.ReadLine();
        System.Console.Write("Position: ");
        string position = Console.ReadLine();
        System.Console.Write("Salary: ");
        decimal salary = Convert.ToDecimal(Console.ReadLine());
        System.Console.Write("Date of emploment (YYYY-MM-DD): ");
        DateTime.TryParse(Console.ReadLine(), out var employmentDate);
        System.Console.Write("Department Id : ");
        int departmentId = Convert.ToInt16(Console.ReadLine());

        string connectionString = "Server=localhost;Database=School;User Id=sa;Password=YourPassword123; Trust Server Certificate=True;";
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {

                using(SqlCommand command = new SqlCommand("AddNewStaff", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@SSN", ssn);
                    command.Parameters.AddWithValue("@Position", position);
                    command.Parameters.AddWithValue("@Salary", salary);
                    command.Parameters.AddWithValue("@EmploymentDate", employmentDate);
                    command.Parameters.AddWithValue("@DepartmentID", departmentId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
                System.Console.WriteLine("New employee added.");
            }
            catch(SqlException ex)
            {
                foreach (SqlError error in ex.Errors)
                {
                    Console.WriteLine("SQL Error" + error.Message);
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                System.Console.WriteLine("Internal error: " + ex.Message);
            }
        }
        Console.ReadKey(intercept: true);
    }

    public void GetStudentGradesById()
    {
        string connectionString = "Server=localhost;Database=School;User Id=sa;Password=YourPassword123; Trust Server Certificate=True;";
        System.Console.WriteLine("Enter students id to view grades.");
        System.Console.Write("Student id: ");
        int studentId = Convert.ToInt32(Console.ReadLine());
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {

                using(SqlCommand command = new SqlCommand("GetStudentGradesById", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.Clear();
                        System.Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Console.WriteLine("| {0,-20} | {1,-10} | {2,-12} | {3,-20} | {4,-20}", "Student", "Grade", "Date", "Course", "Teacher");
                        Console.ResetColor();
                        System.Console.WriteLine(new string('-', 80));
                        while (reader.Read())
                        {
                            string studentName = reader.GetString(0);
                            string grade = reader.GetString(1);
                            string date = reader.GetDateTime(2).ToShortDateString();
                            string course = reader.GetString(3);
                            string teacher = reader.GetString(4);

                            Console.WriteLine("| {0,-20} | {1,-10} | {2,-12} | {3,-20} | {4,-20}", studentName, grade, date, course, teacher);
                        }
                    }
                    
                }
                connection.Close();
                //System.Console.WriteLine("New employee added.");
            }
            catch(SqlException ex)
            {
                foreach (SqlError error in ex.Errors)
                {
                    Console.WriteLine("SQL Error" + error.Message);
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                System.Console.WriteLine("Internal error: " + ex.Message);
            }
        }
        Console.ReadKey(intercept: true);
    }

    public void DepartmentSalaryRecords()
    {
        
        string connectionString = "Server=localhost;Database=School;User Id=sa;Password=YourPassword123; Trust Server Certificate=True;";
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"SELECT d.DepartmentName,SUM(st.Salary) AS TotalWages FROM Staff st JOIN Departments d ON st.DepartmentID = d.DepartmentID GROUP BY d.DepartmentName";
            try
            {

                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.Clear();
                        System.Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Console.WriteLine("| {0,-20} | {1,-16} |", "Name of department", "Total wages");
                        Console.ResetColor();
                        System.Console.WriteLine(new string('-', 60));
                        while(reader.Read())
                        {
                            string departmentName = reader.IsDBNull(0) ? "N/A" : reader.GetString(0);
                            decimal totalWages = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                            System.Console.WriteLine("| {0,-20} | {1,-16} |",$"{departmentName}", totalWages);

                        }
                    }
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                System.Console.WriteLine("Internal error: " + ex.Message);
            }
        }
        Console.ReadKey(intercept: true);
    }

    public void AverageSalaryByDep()
    {
        
        string connectionString = "Server=localhost;Database=School;User Id=sa;Password=YourPassword123; Trust Server Certificate=True;";
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var query = @"SELECT d.DepartmentName, AVG(st.Salary) AS AverageSalary FROM Staff st JOIN Departments d ON st.DepartmentID = d.DepartmentID GROUP BY d.DepartmentName";
            try
            {

                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.Clear();
                        System.Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Console.WriteLine("| {0,-20} | {1,-16} |", "Name of department", "Average salary");
                        Console.ResetColor();
                        System.Console.WriteLine(new string('-', 60));
                        while(reader.Read())
                        {
                            string departmentName = reader.IsDBNull(0) ? "N/A" : reader.GetString(0);
                            decimal averageSalary = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                            System.Console.WriteLine("| {0,-20} | {1,-16} |",$"{departmentName}", averageSalary);

                        }
                    }
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                System.Console.WriteLine("Internal error: " + ex.Message);
            }
        }
        Console.ReadKey(intercept: true);
    }

    public void ShowStudentInfo()
    {
        string connectionString = "Server=localhost;Database=School;User Id=sa;Password=YourPassword123; Trust Server Certificate=True;";
        System.Console.WriteLine("Enter students id to view information.");
        System.Console.Write("Student id: ");
        int studentId = Convert.ToInt32(Console.ReadLine());
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {

                using(SqlCommand command = new SqlCommand("ShowStudent", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.Clear();
                        System.Console.WriteLine();
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Console.WriteLine("| {0,5} | {1,-20} | {2,-10} |", "Student Id", "Name", "Class");
                        Console.ResetColor();
                        System.Console.WriteLine(new string('-', 60));
                        while(reader.Read())
                        {
                            int studentID = reader.IsDBNull(0) ? default : reader.GetInt32(0);
                            string firstName = reader.IsDBNull(1) ? "N/A" : reader.GetString(1);
                            string lastName = reader.IsDBNull(2) ? "N/A" : reader.GetString(2);
                            string className = reader.IsDBNull(3) ? "N/A" : reader.GetString(2);
                            System.Console.WriteLine("| {0,5} | {1,-20} | {2,-10} |",studentID, $"{firstName} {lastName} ", className );

                        }
                    }
                    //command.ExecuteNonQuery();
                }
                connection.Close();
                //System.Console.WriteLine("New employee added.");
            }
            catch(SqlException ex)
            {
                foreach (SqlError error in ex.Errors)
                {
                    Console.WriteLine("SQL Error" + error.Message);
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                System.Console.WriteLine("Internal error: " + ex.Message);
            }
        }
        Console.ReadKey(intercept: true);
    }

    public void GradeStudent()
    {
        Console.Clear();
        System.Console.WriteLine("Add new employee");
        System.Console.Write("Grade: ");
        string grade = Console.ReadLine();
        System.Console.Write("Student id: ");
        int studentId = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Course id: ");
        int courseId = Convert.ToInt32(Console.ReadLine());
        System.Console.Write("Teacher id: ");
        int teacherId = Convert.ToInt32(Console.ReadLine());

        string connectionString = "Server=localhost;Database=School;User Id=sa;Password=YourPassword123; Trust Server Certificate=True;";
       
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {

                using(SqlCommand command = new SqlCommand("GradeStudent", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Grade", grade);
                    command.Parameters.AddWithValue("@StudentId", studentId);
                    command.Parameters.AddWithValue("@CourseId", courseId);
                    command.Parameters.AddWithValue("@TeacherId", teacherId);

                    command.ExecuteNonQuery();
                }
                connection.Close();
                System.Console.WriteLine("New Grade added.");
            }
            catch(SqlException ex)
            {
                foreach (SqlError error in ex.Errors)
                {
                    Console.WriteLine("SQL Error" + error.Message);
                }
                connection.Close();
            }
            catch(Exception ex)
            {
                connection.Close();
                System.Console.WriteLine("Internal error: " + ex.Message);
            }
        }
        Console.ReadKey(intercept: true);
    }
}