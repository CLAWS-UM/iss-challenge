/*  MissionPanel.cs

    Contains all class definitions and functions related 
    to initializing and updating the mission panel 
    displayed on the upper left hand corner of the 
    Hololens display.

    Structure of a Mission:

    Mission
    | 
     -- Mission_Phase
        |
         -- Mission_Task
            |
             -- Mission_Subtask

    POC: Riley Schnee
*/

using System;
using UnityEngine;
using UnityEngine.UI;

// For voice recognition
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using System.Linq;

public class MissionPanel : MonoBehaviour {

    // Declare panel for reference during text placement
    public GameObject panel;

    // Declare missionFile used for reading mission information
    public TextAsset missionFile;

    // Declare GameObject for each task entry in panel
    public GameObject goalObj;
    public GameObject phaseObj;
    public GameObject taskObj;
    public GameObject nextObj;
    public GameObject progObj;

    // Declare text fields for each of the tasks
    public Text goalMesh;
    public Text phaseMesh;
    public Text taskMesh;
    public Text nextMesh;
    public Text progText;

    // Declare check-related sprites
    public Sprite checkmark;
    public Sprite checkcircle;

    // Declare 
    public GameObject[] subTexts;
    public GameObject[] sprites;
    public Mission_Phase curPhase;
    public Mission_Task curTask;
    public Mission_Subtask[] curSubtasks;

    // Voice Recognition
    public KeywordRecognizer keywordRecognizer;
    public Dictionary<string, System.Action> keywords = 
            new Dictionary<string, System.Action>();

    // Declare global Mission object to hold information
    public Mission m;

    /*  Mission_Subtask Class 
        Contains subtask metadata.
    */
    public class Mission_Subtask
    {
        // Subtask Data
        string text;
        bool complete;
        // TO DO: Insert Navigational Component

        // Constructor 
        public Mission_Subtask(string text_in)
        {
            text = text_in;
            complete = false;
        }

        // Getters
        public string get_text()
        {
            return text;
        }

        public bool get_status()
        {
            return complete;
        }

        // Mark subtask complete/incomplete
        public void mark_complete()
        {
            complete = true;
        }

        public void mark_incomplete()
        {
            complete = false;
        }
        
    }
    
    /*  Mission_Task Class 
        Contains array of type Mission_Subtask, task metadata, 
        and variables to control the progression logic/mission flow.
    */    
    public class Mission_Task
    {
        // Task Data
        string text;
        bool complete;
        Mission_Subtask[] subtasks;
        // Progression logic variable
        int subtasks_complete;
        
        // --- Constructors ---

        // Empty subtask array
        public Mission_Task(string text_in)
        {
            text = text_in;
            complete = false;
            subtasks_complete = 0;
            subtasks = new Mission_Subtask[0];
        }  
        // Set subtask array    
        public Mission_Task(string text_in, Mission_Subtask[] subtasks_in)
        {
            text = text_in;
            complete = false;
            subtasks = subtasks_in;
            subtasks_complete = 0;
        }

        // Setter - subtasks
        public void set_subtasks(Mission_Subtask[] subtasks_in)
        {
            subtasks = subtasks_in;
        }

        // Getters - subtasks
        public Mission_Subtask get_prev_subtask()
        {
            return subtasks[subtasks_complete - 1];
        }

        public Mission_Subtask get_cur_subtask()
        {
            return subtasks[subtasks_complete];
        }

        public Mission_Subtask get_next_subtask()
        {
            return subtasks[subtasks_complete + 1];
        }

        public Mission_Subtask get_first_subtask()
        {
            return subtasks[0];
        }

        public Mission_Subtask get_last_subtask()
        {
            return subtasks[subtasks.Length - 1];
        }

        public Mission_Subtask[] get_subtasks()
        {
            return subtasks;
        }

        // Getters - subtasks numbers
        public int get_subtasks_length()
        {
            return subtasks.Length;
        }
        public int get_subtasks_complete()
        {
            return subtasks_complete;
        }
        // Getters - task data
        public string get_text()
        {
            return text;
        }
        public bool get_status()
        {
            return complete;
        }

        // Mark task complete/incomplete
        public void mark_complete()
        {
            complete = true;
        }
        public void mark_incomplete()
        {
            complete = false;
        }

        // Mark subtask complete/incomplete
        public void mark_subtask_complete()
        {
            subtasks[subtasks_complete].mark_complete();
            subtasks_complete++;
            // If all of this task's subtasks are marked complete, 
            //   mark the task as complete, otherwise mark task as incomplete.
            if (subtasks_complete < subtasks.Length)
            {
                mark_incomplete();
            }
            else if (subtasks_complete == subtasks.Length)
            {
                mark_complete();
            }
        }
        public int mark_subtask_incomplete()
        {
            if(subtasks_complete == 0){ 
                // The current task is not the 
                // owner of the subtask to be marked incomplete
                return -1;
            }
            else
            {
                // Mark the last subtask complete as incomplete
                subtasks[subtasks_complete - 1].mark_incomplete();
                subtasks_complete--;
            }
            // If all of this task's subtasks are marked complete, 
            //   mark the task as complete, otherwise mark task as incomplete.
            if (subtasks_complete < subtasks.Length)
            {
                mark_incomplete();
            }
            else if (subtasks_complete == subtasks.Length)
            {
                mark_complete();
                // Shouldn't be triggered in this case, but it's for safety.
            }
            return 0;
        }
    }

    /*  Mission_Phase Class 
        Contains array of type Mission_Task, phase metadata, 
        and variables to control the progression logic/mission flow.
    */
    public class Mission_Phase
    {
        // Phase Data
        string text;
        bool complete;
        Mission_Task[] tasks;
        // For progression logic
        int tasks_complete;

        // --- Constructors ---

        // Empty task array 
        public Mission_Phase(string text_in)
        {
            text = text_in;
            complete = false;
            tasks_complete = 0;
            tasks = new Mission_Task[0];
        }
        // Set task array
        public Mission_Phase(string text_in, Mission_Task[] tasks_in)
        {
            text = text_in;
            complete = false;
            tasks = tasks_in;
            tasks_complete = 0;
        }

        // Setters
        public void set_tasks(Mission_Task[] tasks_in)
        {
            tasks = tasks_in;
        }
        public void set_subtasks(int task_index, Mission_Subtask[] subtasks_in)
        {
            tasks[task_index].set_subtasks(subtasks_in);
        }

        // Getters - tasks
        public Mission_Task get_prev_task()
        {
            return tasks[tasks_complete - 1];
        }
        public Mission_Task get_cur_task()
        {
            return tasks[tasks_complete];
        }
        public Mission_Task get_next_task()
        {
            return tasks[tasks_complete + 1]; 
        }
        public Mission_Task get_first_task()
        {
            return tasks[0];
        }
        public Mission_Task get_last_task()
        {
            return tasks[tasks.Length - 1];
        }
        public Mission_Task[] get_tasks()
        {
            return tasks;
        }

        // Getters - tasks numbers
        public int get_tasks_length()
        {
            return tasks.Length;
        }
        public int get_tasks_complete()
        {
            return tasks_complete;
        }

        // Getters - phase data
        public string get_text()
        {
            return text;
        }
        public bool get_status()
        {
            return complete;
        }

        // Mark phase complete/incomplete
        public void mark_complete()
        {
            complete = true;
        }
        public void mark_incomplete()
        {
            complete = false;
        }
        
        // Mark subtask complete/incomplete
        public void mark_subtask_complete()
        {
            tasks[tasks_complete].mark_subtask_complete();
            
            if (tasks[tasks_complete].get_status())
            {
                tasks_complete++;
            }
            if (tasks_complete < tasks.Length)
            {
                mark_incomplete();
            }
            else if (tasks_complete == tasks.Length)
            {
                mark_complete();
            }
        }

        public int mark_subtask_incomplete()
        {
                int ret1 = tasks[tasks_complete].mark_subtask_incomplete();
                if(ret1 == -1){ // Meaning that task doesn't have the subtask marked incomplete
                    // Try to mark the previous task's subtask incomplete
                    if(tasks_complete > 0){
                        int ret2 = tasks[tasks_complete - 1].mark_subtask_incomplete();
                        if(ret2 == -1){ // This shouldn't be triggered...safety
                            return -2;
                        }
                        else {
                            tasks_complete--;
                            // Subtask to be marked complete belonged to previous
                        }
                    } else {
                        // Meaning we need to mark in previous phase
                        return -1;
                    }
                }

                if (tasks_complete < tasks.Length)
                {
                    mark_incomplete();
                }
                else if (tasks_complete == tasks.Length)
                {
                    mark_complete();
                }
                return 0;
        }

    }

    // Enum for Mission Type
    public enum Mission_Type : int
    {
        Repair,
        Experiment,
        Other
        // TO DO: Insert more here
    }

    /*  Mission Class 
        Contains array of type Mission_Phase, mission metadata, 
        and variables to control the progression logic/mission flow.
    */
    public class Mission {
        
        // Mission Variables
        string title;
        string goal;
        Mission_Phase[] phases;
        Mission_Type type;
        float progress;
        // Used for the progression logic
        int phases_complete;
        int total_subtasks;
        int num_subtasks_complete;

        // --- Contructors ---

        // Default
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

        // Custom (no file input)
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

        // Custom (with file input)
        public Mission(TextAsset missionFile) {
            string missionText = missionFile.text;
            string[] lines = System.Text.RegularExpressions.Regex.Split(missionText, "\n");
     
            //string[] lines = System.IO.File.ReadAllLines(missionFile);

            // Set Mission metadata
            title = lines[0];
            goal = lines[1];
            phases_complete = 0; 
            progress = 0;

            // Initialize progression logic vars
            total_subtasks = 0;
            num_subtasks_complete = 0;

            // Set mission type
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
            // Iterate through phases
            for(int i = 0; i < phases.Length; i++){
                phases[i] = new Mission_Phase(lines[it]);
                it++;

                int numTasks = int.Parse(lines[it]);
                it++;
                phases[i].set_tasks(new Mission_Task[numTasks]);
                // Iterate through tasks
                for(int k = 0; k < phases[i].get_tasks().Length; k++){
                    phases[i].get_tasks()[k] = new Mission_Task(lines[it]);
                    it++;

                    int numSubtasks = int.Parse(lines[it]);
                    it++;
                    phases[i].set_subtasks(k, new Mission_Subtask[numSubtasks]);
                    // Iterate through subtasks
                    for(int j = 0; j < phases[i].get_tasks()[k].get_subtasks_length(); j++){
                        phases[i].get_tasks()[k].get_subtasks()[j] = 
                                new Mission_Subtask(lines[it]);
                        it++;
                        total_subtasks++;
                    }
                }
            }
        }

        // Getters - Mission metadata
        public string get_title(){
            return title;
        }
        public string get_goal(){
            return goal;
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

        // Getters - Mission_Subtasks
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
            } else if(phases[phases_complete].get_tasks_complete() < 
                        phases[phases_complete].get_tasks_length()) {
                return phases[phases_complete].get_next_task().get_first_subtask();
            }
                //if(phases_complete < phases.Length)
            else {
                return phases[phases_complete + 1].get_first_task().get_first_subtask();
            }
        
        }
        
        // Getter - Mission_Task
        public Mission_Task get_next_task(){
            if(phases[phases_complete].get_tasks_complete() < 
                    phases[phases_complete].get_tasks_length() - 1){
                return phases[phases_complete].get_next_task();
            } else if(phases_complete < phases.Length - 1){
                return phases[phases_complete + 1].get_first_task();
            } else {
                return new Mission_Task("");
            }
        }
        
        // Mark subtask complete/incomplete
        public void mark_subtask_complete(){
            if(num_subtasks_complete < total_subtasks){
                phases[phases_complete].mark_subtask_complete();
                num_subtasks_complete++;

                // If phase marked completed, increment
                if(phases[phases_complete].get_status() == true){
                    phases_complete++;
                }
                
                // Update progress
                progress = (float)num_subtasks_complete / (float)total_subtasks;
            }
        }
        public void mark_subtask_incomplete(){
            if(num_subtasks_complete > 0){
                int ret1 = phases[phases_complete].mark_subtask_incomplete();
                if(ret1 == 0){ // All good
                    num_subtasks_complete--;
                }
                else if(phases_complete > 0 && ret1 == -1){
                    int ret2 = phases[phases_complete - 1].mark_subtask_incomplete();
                    if(ret2 < 0){
                        return; // This shouldn't happen...safety
                    } else {
                        phases_complete--;
                        num_subtasks_complete--;
                    }
                }
                else if (ret1 == -2){
                    Debug.Log("MARK_INCOMPLETE ERROR"); // Debug
                }
                
                // Update progress
                progress = (float)num_subtasks_complete / (float)total_subtasks;
            }
            
        }
        
        // Getters - numbers for progression logic
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

        // Getter - current phase
        public Mission_Phase get_cur_phase(){
            return phases[phases_complete];
        }

    }

    // FormatText() Source: "https://forum.unity.com/threads/3d-text-wrap.32227"
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
                charCount += word.Length + 1; 
                    //+1, because we assume, that there will be a space after every word
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

    // Voice Recognition
    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    /* --- Start Function --- */

    void Start(){

        // Voice Recognition 
        // Create keywords for keyword recognizer
        keywords.Add("mark done", () =>
        {
            Mark_Complete();
        });
        keywords.Add("mark not done", () =>
        {
            Mark_Incomplete();
        });
        
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();

        // Initialize Text components for tasks
        goalMesh = goalObj.GetComponent<Text>();
        phaseMesh = phaseObj.GetComponent<Text>();
        taskMesh = taskObj.GetComponent<Text>();
        nextMesh = nextObj.GetComponent<Text>();

        // Arrange task GameObjects
        RectTransform taskRect = (RectTransform)taskObj.transform;
        nextObj.transform.localPosition = new Vector3(nextObj.transform.localPosition.x, 
                (taskObj.transform.localPosition.y + taskRect.rect.height), 0);

        progObj.transform.SetParent(panel.transform);
        progText = progObj.GetComponent<Text>();
        progText.text = "0%";
        
        RectTransform progRect = progObj.GetComponent<RectTransform>();
        progRect.localEulerAngles = new Vector3(0,0,0);
        progObj.transform.localEulerAngles = new Vector3(0,0,0);

        // Initialize mission 'm' with missionFile input
        m = new Mission(missionFile);
        // Display the mission title
        goalMesh.text = m.get_title();
        // Update
        Update_Tasks_List();
    }

    /* --- Update Tasks Function --- 
        Called when user indicates they want to mark a 
        subtask as complete/incomplete.
    */

    void Update_Tasks_List(){
        // Set cur phase text
        curPhase = m.get_cur_phase();
        phaseMesh.text = curPhase.get_text();

        // Set cur task
        curTask = curPhase.get_cur_task();
        taskMesh.text = curTask.get_text();

        // Update curSubtasks array
        curSubtasks = curPhase.get_cur_task().get_subtasks();
        
        // Update progress
        progText.text = ((int)(m.get_progress() * 100)).ToString() + "%";

        // Clear the existing array of tasks if all subtasks have been completed
        for (int i = 0; i < subTexts.Length; i++)
        {
            subTexts[i].GetComponent<Text>().text = "";
            sprites[i].GetComponent<SpriteRenderer>().sprite = null;
            Destroy(subTexts[i]);
            Destroy(sprites[i]);
        }
        if (subTexts != null) // Safety
        {
            System.Array.Clear(subTexts, 0, subTexts.Length);
            System.Array.Clear(sprites, 0, sprites.Length);
        }

        // Create the array of subtasks 
        subTexts = new GameObject[curSubtasks.Length];
        sprites = new GameObject[curSubtasks.Length];

        // Placement
        float startingY = taskObj.GetComponent<RectTransform>().localPosition.y - 
                    (taskObj.GetComponent<RectTransform>().sizeDelta.y * 2 / 10);
        float startingX = taskObj.GetComponent<RectTransform>().localPosition.x;
        Debug.Log("TaskObj.localPosition.y" + 
                    taskObj.GetComponent<RectTransform>().localPosition.y);
        Debug.Log("OffsetMin" + taskObj.GetComponent<RectTransform>().offsetMin.y);
        Debug.Log("Starting Y: " + startingY + "| Starting X: " + startingX);
        Debug.Log("# of Subtasks: " + subTexts.Length);

        // Loop through the subtasks and create ui element
        for (int i = 0; i < curSubtasks.Length; i++)
        {
            // Checkmark sprite
            sprites[i] = new GameObject();
            // Text
            subTexts[i] = new GameObject();
            // Set names of UI elements
            subTexts[i].name = "Subtext #" + i.ToString();
            sprites[i].name = "Check #" + i.ToString();

            // Set parents and create components
            subTexts[i].transform.SetParent(panel.transform);
            Text sTtext = subTexts[i].AddComponent<Text>();

            sprites[i].transform.SetParent(panel.transform);
            SpriteRenderer srend = sprites[i].AddComponent<SpriteRenderer>();

            // expansion -- TODO: Fix or implement alternative
            ContentSizeFitter csf = subTexts[i].AddComponent<ContentSizeFitter>();

            // Set Text attributes
            sTtext.fontSize = 0;
            sTtext.horizontalOverflow = HorizontalWrapMode.Wrap;
            sTtext.verticalOverflow = VerticalWrapMode.Overflow;
            sTtext.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            sTtext.alignment = TextAnchor.MiddleLeft;

            // Provide Text position and size using RectTransform.
            RectTransform rectTransform;
            rectTransform = subTexts[i].GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(1, startingY, 0);
            rectTransform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
            rectTransform.sizeDelta = new Vector2(120, 15);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            subTexts[i].transform.localEulerAngles = new Vector3(0,0,0);
            rectTransform.localEulerAngles = new Vector3(0,0,0);

            // Pick correct check sprite for subtask
            if (curSubtasks[i].get_status())
            {
                srend.sprite = checkmark;
            }
            else
            {
                srend.sprite = checkcircle;
            }
            // Set size and attributes of check sprite
            srend.size = new Vector2(2f, 2f);
            srend.transform.localEulerAngles = new Vector3(0,0,0);

            sprites[i].AddComponent<RectTransform>();
            sTtext.text = curSubtasks[i].get_text();
            Debug.Log(sTtext.text); // Debug
            sprites[i].GetComponent<RectTransform>().pivot = 
                        new Vector2(0.5f, 0.5f);
            srend.GetComponent<RectTransform>().localEulerAngles = 
                        new Vector3(0, 0, 0);
            sprites[i].GetComponent<RectTransform>().localPosition = 
                        new Vector3(-6.5f, startingY, 0);
            sprites[i].transform.localScale = new Vector3(3f, 3f, 3f);

            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            // Update the starting y position value for the next
            startingY = startingY - 
                    (subTexts[i].GetComponent<RectTransform>().sizeDelta.y * 2 / 10) - 0.5f;
            Debug.Log("New Starting Y: " + startingY); // Debug
        }
        nextMesh.transform.localPosition = new Vector3(0, startingY, 0);

        // Update "next task"
        if (m.get_subtasks_complete() < m.get_total_subtasks())
            nextMesh.text = m.get_next_task().get_text();
        else
            nextMesh.text = "";
    }

    // Voice Recognition - Mark Subtask Complete/Incomplete
    public void Mark_Complete(){
        m.mark_subtask_complete();
        Update_Tasks_List();
    }
    public void Mark_Incomplete(){
        m.mark_subtask_incomplete();
        Update_Tasks_List();
    }
}
