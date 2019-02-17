using System;
using Unity.UI; // Used for editing Unity UI
// To compile: csc /target:exe /out:test_mission.exe test_mission.cs mission.cs
class Test_Mission_Class
{
    // For setting text within Unity
    public Text mission_title = null;
    public Text mission_goal = null;

    static void Main()
    {
    	Mission mission = new Mission();
        Console.WriteLine(mission.get_title());
        Console.WriteLine(mission.get_goal());
        mission.print_tasks();
        Console.WriteLine(mission.get_type());
        Console.WriteLine(mission.get_progress());

        // ie. Here we could do...
        // mission_title = mission.get_title();
        // mission_goal = mission.get_goal(); 
        // This would update the corresponding in unity

        mission.toggle_task_status(2);
        mission.toggle_task_status(4);
        Console.WriteLine(mission.get_title());
        Console.WriteLine(mission.get_goal());
        mission.print_tasks();
        Console.WriteLine(mission.get_type());
        Console.WriteLine(mission.get_progress());
    }
}
