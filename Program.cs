using System.ComponentModel.Design;

namespace IndiLabbSkola;

class Program
{
    static void Main(string[] args)
    {
        var dbAction = new DBAction();
        Dictionary<string,Action> actionMap = new Dictionary<string,Action>
        {
            {"Add student", dbAction.AddStudent},
            {"Show students", dbAction.ShowStudents},
            {"Show class", dbAction.ShowClass},
            {"Show staff by department", dbAction.ShowStaffByDep},
            {"Show active courses", dbAction.ShowActiveCourses}
        };
        while(true)
        {
            var choice = dbAction.Menu(actionMap.Keys.ToArray<string>());
            actionMap[choice].DynamicInvoke();
        }
    }
}
