// Last Edited: 3-31-19
using System;
using UnityEngine;
using UnityEngine.UI;
// For voice recognition
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class MissionPanel : MonoBehaviour {
    public GameObject panel;
    // public TextMesh goalText;
    // public TextMesh prevTaskText;
    // public TextMesh curTaskText;
    // public TextMesh nextTaskText;
    public TextAsset missionFile;
    public TextMesh goalMesh;
    public TextMesh phaseMesh;
    public TextMesh taskMesh;
    public TextMesh nextMesh;

    public GameObject goalObj;
    public GameObject phaseObj;
    public GameObject taskObj;
    public GameObject nextObj;
    

    // Voice Recognition
    public KeywordRecognizer keywordRecognizer;
    public Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    public Mission m;

    public class Mission {
        //public TextAsset missionfile;
        // Sturct and Enum used by Mission
        public class Mission_Subtask {
            string text;
            bool complete;
            // TO DO: Insert Navigational Component
            public Mission_Subtask(string text_in){
                text = text_in;
                complete = false;
            }
            public string get_text(){
                return text;
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
            public bool toggle(){
                if(complete){
                    mark_incomplete();
                    return false;
                } else {
                    mark_complete();
                    return true;
                }
            }
        }

        public class Mission_Task {
            string text;
            bool complete;
            Mission_Subtask[] subtasks;
            int subtasks_complete;
            // TO DO: Insert Navigational Component
            public Mission_Task(string text_in, Mission_Subtask[] subtasks_in){
                text = text_in;
                complete = false;
                subtasks = subtasks_in;
                subtasks_complete = 0;
            }
            public Mission_Task(string text_in){
                text = text_in;
                complete = false;
                subtasks_complete = 0;
                subtasks = new Mission_Subtask[0]; 
            }
            public Mission_Subtask get_prev_subtask(){
                return subtasks[subtasks_complete - 1];
            }
            public Mission_Subtask get_cur_subtask(){
                return subtasks[subtasks_complete];
            }
            public Mission_Subtask get_next_subtask(){
                return subtasks[subtasks_complete + 1];
            }
            public Mission_Subtask get_first_subtask(){
                return subtasks[0];
            }
            public Mission_Subtask get_last_subtask(){
                return subtasks[subtasks.Length - 1];
            }
            public Mission_Subtask[] get_subtasks() {
                return subtasks;
            }
            public void set_subtasks(Mission_Subtask[] subtasks_in) {
                subtasks = subtasks_in;
            }
            public int get_subtasks_length(){
                return subtasks.Length;
            }
            public int get_subtasks_complete() {
                return subtasks_complete;
            }
            public void inc_subtasks_complete() {
                subtasks_complete++;
            }
            public void dec_subtasks_complete() {
                subtasks_complete--;
            }
            public string get_text(){
                return text;
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
            public bool toggle(){
                bool ret = subtasks[subtasks_complete].toggle();
                if(ret){
                    subtasks_complete++;
                } else {
                    subtasks_complete--;
                }
                if(subtasks_complete < subtasks.Length){
                    mark_incomplete();
                    return false;
                } else { //if (subtasks_complete == subtasks.Length){
                    mark_complete();
                    return true;
                }
            }
            public void mark_subtask_complete(){
                subtasks[subtasks_complete].mark_complete();
                subtasks_complete++;
                if(subtasks_complete < subtasks.Length){
                    mark_incomplete();
                } else if (subtasks_complete == subtasks.Length){
                    mark_complete();
                }
            }
            public void mark_subtask_incomplete(){
                subtasks[subtasks_complete].mark_incomplete();
                subtasks_complete--;
                mark_incomplete();
            }
        }
        public class Mission_Phase {
            string text;
            bool complete;
            Mission_Task[] tasks;
            int tasks_complete;
            // TO DO: Insert Navigational Component
            public Mission_Phase(string text_in, Mission_Task[] tasks_in){
                text = text_in;
                complete = false;
                tasks = tasks_in;
                tasks_complete = 0;
            }
            public Mission_Phase(string text_in){
                text = text_in;
                complete = false;
                tasks_complete = 0;
                tasks = new Mission_Task[0];
            }
            public Mission_Task get_prev_task(){
                return tasks[tasks_complete - 1];
            }
            public Mission_Task get_cur_task(){
                return tasks[tasks_complete];
            }
            public Mission_Task get_next_task(){
                return tasks[tasks_complete + 1];
            }
            public Mission_Task get_first_task(){
                return tasks[0];
            }
            public Mission_Task get_last_task(){
                return tasks[tasks.Length - 1];
            }
            public Mission_Task[] get_tasks(){
                return tasks;
            }
            public void set_tasks(Mission_Task[] tasks_in){
                tasks = tasks_in;
            }
            public void set_subtasks(int task_index, Mission_Subtask[] subtasks_in){
                tasks[task_index].set_subtasks(subtasks_in);
            }
            public int get_tasks_length() {
                return tasks.Length;
            }
            public int get_tasks_complete() {
                return tasks_complete;
            }
            public void inc_tasks_complete() {
                tasks_complete++;
            }
            public void dec_tasks_complete() {
                tasks_complete--;
            }
            public string get_text(){
                return text;
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
                bool ret = tasks[tasks_complete].toggle();
                if(ret){
                    tasks_complete++;
                } else {
                    tasks_complete--;
                }
                if(tasks_complete < tasks.Length){
                    mark_incomplete();
                } else if (tasks_complete == tasks.Length){
                    mark_complete();
                }
            }
            public void mark_subtask_complete(){
                tasks[tasks_complete].mark_subtask_complete();
                if(tasks[tasks_complete].get_subtasks_complete() == tasks[tasks_complete].get_subtasks_length()){
                    tasks_complete++;
                }
                if(tasks_complete < tasks.Length){
                    mark_incomplete();
                } else if (tasks_complete == tasks.Length){
                    mark_complete();
                }
            }
            public void mark_subtask_incomplete(){
                tasks[tasks_complete].mark_subtask_incomplete();
                mark_incomplete();
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
        Mission_Phase[] phases;
        Mission_Type type;
        float progress;
        int phases_complete;
        int total_subtasks;
        int num_subtasks_complete;

        // Mission Contructors
        public Mission() {
            title = "Repair the ISS";
            goal = "Fix parts of the ISS";
            type = Mission_Type.Repair;
            phases_complete = 0; 
            progress = 0;
            total_subtasks = 0;
            num_subtasks_complete = 0;
            // Populate task array
            Mission_Subtask[] s = new Mission_Subtask[5];
            for(int i = 0; i < 5; i++){
                s[i] = new Mission_Subtask("Subtask " + i);
                total_subtasks++;
            }
            Mission_Task[] t = new Mission_Task[5];
            for(int i = 0; i < 5; i++){
                t[i] = new Mission_Task("Task " + i, s);
            }
            phases = new Mission_Phase[5];
            for(int i = 0; i < 5; i++){
                phases[i] = new Mission_Phase("Phase " + i, t);
            }
        }
        public Mission(string title_in, string goal_in, string type_in, 
                        Mission_Phase[] phase_in, int total_subtasks_in) {
            title = title_in;
            goal = goal_in;
            phases_complete = 0; 
            progress = 0;
            num_subtasks_complete = 0;
            if(type_in == "Repair"){
                type = Mission_Type.Repair;
            } else if (type_in == "Experiment") {
                type = Mission_Type.Experiment;
            } else {
                type = Mission_Type.Other;
            }
            phases = phase_in;
            total_subtasks = total_subtasks_in;
        }
        public Mission(TextAsset missionFile) {
            string missionText = missionFile.text;
            string[] lines = System.Text.RegularExpressions.Regex.Split(missionText, "\n");
     
            //string[] lines = System.IO.File.ReadAllLines(missionFile);

            title = lines[0];
            goal = lines[1];
            phases_complete = 0; 
            progress = 0;
            total_subtasks = 0;
            num_subtasks_complete = 0;

            if(lines[2] == "Repair"){
                type = Mission_Type.Repair;
            } else if (lines[2] == "Experiment") {
                type = Mission_Type.Experiment;
            } else {
                type = Mission_Type.Other;
            }

            // Populate task array
            int numPhases = int.Parse(lines[3]);
            phases = new Mission_Phase[numPhases];
            int it = 4;
            for(int i = 0; i < phases.Length; i++){
                phases[i] = new Mission_Phase(lines[it]);
                it++;

                int numTasks = int.Parse(lines[it]);
                it++;
                phases[i].set_tasks(new Mission_Task[numTasks]);
                for(int k = 0; k < phases[i].get_tasks().Length; k++){
                    phases[i].get_tasks()[k] = new Mission_Task(lines[it]);
                    it++;

                    int numSubtasks = int.Parse(lines[it]);
                    it++;
                    phases[i].set_subtasks(k, new Mission_Subtask[numSubtasks]);
                    for(int j = 0; j < phases[i].get_tasks()[k].get_subtasks_length(); j++){
                        phases[i].get_tasks()[k].get_subtasks()[j] = new Mission_Subtask(lines[it]);
                        it++;
                        total_subtasks++;
                    }
                }
            }
        }

        public string get_title(){
            return title;
        }
        public string get_goal(){
            return goal;
        }
        // TO DO: Change so it returns these tasks rather than printing
        
        // public void print_tasks(){
        //     for(int i = 0; i < phases.Length; i++){
        //         Console.WriteLine(phases[i].get_text() + " - " + phases[i].get_status());
                
        //         for(int k = 0; k < phases[i].get_tasks_length(); k++){
        //             Console.WriteLine("\t" + phases[i].get_tasks()[k].get_text() + " - " + phases[i].get_tasks()[k].get_status());

        //             for(int j = 0; j < phases[i].get_tasks()[k].get_subtasks_length(); j++){
        //                 Console.WriteLine("\t\t" + phases[i].get_tasks()[k].get_subtasks()[j].get_text() + " - " + phases[i].get_tasks()[k].get_subtasks()[j].get_status());
        //             }
        //         }
        //     }
        // }
        public Mission_Subtask get_cur_subtask(){
            return phases[phases_complete].get_cur_task().get_cur_subtask();
        }
        public Mission_Subtask get_prev_subtask(){
            if(phases[phases_complete].get_cur_task().get_subtasks_complete() > 0){
                return phases[phases_complete].get_cur_task().get_prev_subtask();
            }
            else if(phases[phases_complete].get_tasks_complete() > 0){
                return phases[phases_complete].get_prev_task().get_last_subtask();
            }
            else{ //if(phases_complete > 0)
                return phases[phases_complete - 1].get_last_task().get_last_subtask();
            }
            
        }
        public Mission_Subtask get_next_subtask(){
            if(phases[phases_complete].get_cur_task().get_subtasks_complete() < 
                    phases[phases_complete].get_cur_task().get_subtasks_length()){
                return phases[phases_complete].get_cur_task().get_next_subtask();
            } else if(phases[phases_complete].get_tasks_complete() < phases[phases_complete].get_tasks_length()){
                return phases[phases_complete].get_next_task().get_first_subtask();
            }
                //if(phases_complete < phases.Length)
            else {
                return phases[phases_complete + 1].get_first_task().get_first_subtask();
            }
        
        }
        // public Mission_Task get_task(int task_num){
        //     // Note: Stored in array zero-based, but task_num is one based
        //     return tasks[task_num - 1];
        // }
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
        public void mark_subtask_complete(){
            phases[phases_complete].mark_subtask_complete();
            num_subtasks_complete++;

            // If phase marked completed, increment
            if(phases[phases_complete].get_status() == true){
                phases_complete++;
            }
            
            // Update progress
            progress = (float)num_subtasks_complete / (float)total_subtasks;
        }
        public void mark_subtask_incomplete(){
            phases[phases_complete].mark_subtask_incomplete();
            num_subtasks_complete--;
            
            // Update progress
            progress = (float)num_subtasks_complete / (float)total_subtasks;
        }
        // public void toggle_current_task_status(){
        //     toggle_task_status(tasks_complete);
        // }
        public int num_phases_complete(){
            return phases_complete;
        }
        public int num_phases(){
            return phases.Length;
        }
        public int get_total_subtasks(){
            return total_subtasks;
        }
        public int get_subtasks_complete(){
            return num_subtasks_complete;
        }
        public Mission_Phase get_cur_phase(){
            return phases[num_phases_complete];
        }

    }

    // "https://forum.unity.com/threads/3d-text-wrap.32227"
    public string FormatText (string text) {
        //int currentLine = 1;
        int maxLineChars = 50;
        int charCount = 0;
        string[] words = text.Split(" "[0]); //Split the string into seperate words
        string result = "";
     
        for (int index = 0; index < words.Length; index++) {
     
            string word = words[index].Trim();
       
            if (index == 0) {
                result = words[0];
                return result;
            }
     
            if (index > 0 ) {
                charCount += word.Length + 1; //+1, because we assume, that there will be a space after every word
                if (charCount <= maxLineChars) {
                    result += " " + word;
                }
                else {
                    charCount = 0;
                    result += "\n " + word;
                }
               
     
                //textObject.text = result;
                return result;
            }
        }  
        return text; 
    }
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    void Start(){
        int TaskFontSize = 15;
        check = '\u2713';
        // Voice 
        //Create keywords for keyword recognizer
        keywords.Add("mark complete", () =>
        {
            Mark_Complete_Voice();
        });
        keywords.Add("mark incomplete", () =>
        {
            Mark_Incomplete_Voice();
        });
        
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();


        // goalMesh = goalObj.GetComponent<TextMesh>();
        // prevMesh = prevObj.GetComponent<TextMesh>();
        // curMesh = curObj.GetComponent<TextMesh>();
        // nextMesh = nextObj.GetComponent<TextMesh>();

        goalMesh = goalObj.GetComponent<TextMesh>();
        phaseMesh = phaseObj.GetComponent<TextMesh>();
        taskMesh = taskObj.GetComponent<TextMesh>();
        nextMesh = nextObj.GetComponent<TextMesh>();

        RectTransform taskRect = (RectTransform)taskObj.transform;
        nextObj.transform.localPosition = (nextObj.transform.localPosition.x, taskObj.transform.localPosition.y + taskRect.height, 0);

/*
        GameObject nextObj = new GameObject();
        nextObj.transform.SetParent(panel.transform);
        nextObj.name = "Next Task Text";
        nextMesh = nextObj.AddComponent<TextMesh>();
        nextMesh.text = "Blah Blah Blah";
        nextMesh.fontSize = TaskFontSize;
        nextMesh.anchor = TextAnchor.UpperLeft;
        nextMesh.alignment = TextAlignment.Center;
        RectTransform nextRect = (RectTransform)nextObj.transform.parent.transform;
        // Image nextB = nextObj.AddComponent<Image>();
        // nextB.color = new Color32(255,255,100,255);
        Debug.Log("Height: " + nextRect.rect.height + " :: " + "Width: " + nextRect.rect.width);
        nextObj.transform.localPosition = new Vector3(-8, 6, 0);
        nextObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        */

        m = new Mission(missionFile);
        goalMesh.text = m.get_title();
        Update_Tasks_List();
    }
    void Update_Tasks_List(){
        Mission_Phase curPhase = m.get_cur_phase();
        phaseMesh.text = curPhase.get_text();
        string taskString = curPhase.get_cur_task().get_text() + "\n";
        Mission_Subtask[] curSubtasks = curPhase.get_cur_task().get_subtasks();
        for(int i = 0; i < curSubtasks.Length; i++){
            if(curSubtasks[i].complete){
                taskString = taskString + "\t" + check + " " + curSubtasks[i].get_text() + "\n";
            } else {
                taskString = taskString + "\t" + "-" + " " + curSubtasks[i].get_text() + "\n";
            }
        }
        taskMesh.text = taskString;
        if(m.get_subtasks_complete() < m.get_total_subtasks())
            nextMesh.text = m.get_next_subtask().get_text();
        else 
            nextMesh.text = "";
    }
    void Mark_Complete_Voice(){
        m.mark_subtask_complete();
        Update_Tasks_List();
    }
    void Mark_Incomplete_Voice(){
        m.mark_subtask_incomplete();
        Update_Tasks_List();
    }
    // void Go_To_Task_Voice(int task_num){
    //     if(m.task_num - 1 > 0)
    //         prevMesh.GetComponent<Text>().text = m.get_task(task_num - 1).get_name() + ": " + m.get_task(task_num - 1).get_info();
    //     curMesh.GetComponent<Text>().text = m.get_task(task_num).get_name() + ": " + m.get_task(task_num).get_info();
    //     if(m.task_num < m.num_tasks())
    //         nextMesh.GetComponent<Text>().text = m.get_task(task_num + 1).get_name() + ": " + m.get_task(task_num + 1).get_info();   
    // }
    // void Return_To_Current_Task_Voice(){
    //      Update_Tasks_List();
    // }
    void Flag(){

    }
    void Unflag(){

    }
    
}
