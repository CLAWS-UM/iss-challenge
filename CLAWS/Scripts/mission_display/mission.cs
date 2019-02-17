using System;
public class Mission {

	// Sturct and Enum used by Mission
	struct Mission_Task {
		string name;
		string info;
		bool complete;
		// TO DO: Insert Navigational Component
		public Mission_Task(string name_in, string info_in){
			name = name_in;
			info = info_in;
			complete = false;
		}
		public string get_name(){
			return name;
		}
		public string get_info(){
			return info;
		}
		public bool get_status(){
			return complete;
		}
		public void mark_complete(){
			complete = true;
		}
		public void mark_incomplete(){
			complete = false;
		}
		public void toggle(){
			if(complete){
				mark_incomplete();
			} else {
				mark_complete();
			}
		}
    }
    public enum Mission_Type : int {
    	Repair,
    	Experiment,
    	Other
    	// TO DO: Insert more here
    }

    // Mission Variables
    string title;
    string goal;
    Mission_Task[] tasks;
    Mission_Type type;
    int tasks_complete;
    float progress;

    // Mission Contructors
    public Mission() {
        title = "Repair the ISS";
        goal = "Fix parts of the ISS";
        type = Mission_Type.Repair;
        tasks_complete = 0; 
        progress = 0;
        // Populate task array
        tasks = new Mission_Task[5];
        for(int i = 0; i < 5; i++){
        	tasks[i] = new Mission_Task("Task " + i, "Fix something");
        }
    }
    public Mission(string title_in, string goal_in, string type_in, string[] task_names_in, string[] task_info_in) {
        title = title_in;
        goal = goal_in;
        tasks_complete = 0; 
        progress = 0;
        if(type_in == "Repair"){
        	type = Mission_Type.Repair;
        } else if (type_in == "Experiment") {
        	type = Mission_Type.Experiment;
        } else {
        	type = Mission_Type.Other;
        }

        // Populate task array
        tasks = new Mission_Task[task_names_in.Length];
        for(int i = 0; i < tasks.Length; i++){
        	tasks[i] = new Mission_Task(task_names_in[i], task_info_in[i]);
        }
    }
    public string get_title(){
    	return title;
    }
    public string get_goal(){
    	return goal;
    }
    // TO DO: Change so it returns these tasks rather than printing
    public void print_tasks(){
    	for(int i = 0; i < tasks.Length; i++){
    		Console.WriteLine(tasks[i].get_name());
    		Console.WriteLine(tasks[i].get_info());
    		if(tasks[i].get_status()){
    			Console.WriteLine("Complete!");
    		} else {
    			Console.WriteLine("Incomplete");
    		}
        }
    }
    public string get_type(){
    	if(type == Mission_Type.Repair){
    		return "Repair";
        } else if(type == Mission_Type.Experiment){
    		return "Experiment";
        } else {
    		return "Other";
        }
    }
    public double get_progress(){
    	return progress;
    }
    public void toggle_task_status(int num){
    	tasks[num].toggle();
    	// If marked completed, increment
    	if(tasks[num].get_status()){
    		tasks_complete++;
    	}
    	// Else decrement
    	else {
    		tasks_complete--;
    	}
    	// Update progress
    	progress = (float)tasks_complete / (float)tasks.Length;
    }
}