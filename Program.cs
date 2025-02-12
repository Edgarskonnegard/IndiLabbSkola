using System.ComponentModel.Design;

namespace IndiLabbSkola;

class Program
{
    static void Main(string[] args)
    {
        var dbAction = new DBAction();
        Dictionary<string,Action> actionMap = new Dictionary<string,Action>
        {
            //{"Add student", dbAction.AddStudent},
            {"Show students", dbAction.ShowStudents},
            {"Show class", dbAction.ShowClass},
            {"Show staff by department", dbAction.ShowStaffByDep},
            {"Show active courses", dbAction.ShowActiveCourses},
            {"Show all staff", dbAction.ShowStaff},
            {"Add new employee", dbAction.AddNewStaff},
            {"View grades", dbAction.GetStudentGradesById},
            {"View department salary records", dbAction.DepartmentSalaryRecords},
            {"View average salary", dbAction.AverageSalaryByDep},
            {"Show student information", dbAction.ShowStudentInfo},
            {"Assign grade", dbAction.GradeStudent}
        };
        while(true)
        {
            var choice = dbAction.Menu(actionMap.Keys.ToArray<string>());
            actionMap[choice].DynamicInvoke();
        }
    }
}
