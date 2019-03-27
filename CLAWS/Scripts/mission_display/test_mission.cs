using System;
//using Unity.UI; // Used for editing Unity UI

public class Test_Mission_Class
{
    // For setting text within Unity
    //public Text mission_title = null;
    //public Text mission_goal = null;

    static void Main()
    {
    	Mission mission = new Mission();
        Console.WriteLine(mission.get_title() + " : " + mission.get_type() + " - " + mission.get_goal());
        mission.print_tasks();
        Console.WriteLine("Progress: " + mission.get_progress());

        // ie. Here we could do...
        // mission_title = mission.get_title();
        // mission_goal = mission.get_goal(); 
        // This would update the corresponding in unity

        mission.toggle_current_task_status();
        mission.toggle_current_task_status();
        Console.WriteLine(mission.get_title() + ": " + mission.get_type() + " - " + mission.get_goal());
        mission.print_tasks();
        Console.WriteLine("Progress: " + mission.get_progress());

        // Test file input
        
        Mission mission1 = new Mission("missionInput.txt");
        Console.WriteLine(mission1.get_title() + " : " + mission1.get_type() + " - " + mission1.get_goal());
        mission1.print_tasks();
        Console.WriteLine("Progress: " + mission1.get_progress());

        // ie. Here we could do...
        // mission_title = mission.get_title();
        // mission_goal = mission.get_goal(); 
        // This would update the corresponding in unity

        mission1.toggle_current_task_status();
        mission1.toggle_current_task_status();
        Console.WriteLine(mission1.get_title() + ": " + mission1.get_type() + " - " + mission1.get_goal());
        mission1.print_tasks();
        Console.WriteLine("Progress: " + mission1.get_progress());




    }
}
