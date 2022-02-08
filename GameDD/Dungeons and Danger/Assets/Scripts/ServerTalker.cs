using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class ServerTalker : MonoBehaviour
{
    public static int ThisPlayerIs = 0; //What turn this player is, so each player only controls one character
    public static bool SinglePlayerMode = false;
    bool canInitialize = true; ///Tell server you joined once
    bool showDebug = false;
    static public int TakeTurn = 1;
    GameObject[] playerObjs;

    public bool checkNow = false; //Run Get data from database
    private float _checkGet = 30.0f;
    private int _getstr = 0;
    private int _getdice = 0;
    private int _getturn = 0;
    private string _gettarget = "";

    //Data
    public static int playersTotal { get; set; }
    public string tDp1Name { get; set; }
    public string tDp2Name { get; set; }
    public string tDp3Name { get; set; }
    public string tDp4Name { get; set; }
    public int tDP1mv { get; set; }
    public int tDP2mv { get; set; }
    public int tDP3mv { get; set; }
    public int tDP4mv { get; set; }
    public int tDP1fc { get; set; }
    public int tDP2fc { get; set; }
    public int tDP3fc { get; set; }
    public int tDP4fc { get; set; }
    public int tDAction { get; set; } //0 = No Action Yet, 1 = Melee, 2 = Spell, 3 = Self Skill, 4 = Self Spell
    public int tDActionID { get; set; } //the Id for the action in a list
    public string tDTargetName { get; set; }//Who is being targeted this turn
    public int tDP1MaxHP { get; set; }
    public int tDP2MaxHP { get; set; }
    public int tDP3MaxHP { get; set; } 
    public int tDP4MaxHP { get; set; } 
    public int tDP1HP { get; set; } 
    public int tDP2HP { get; set; } 
    public int tDP3HP { get; set; } 
    public int tDP4HP { get; set; } 



    // Start is called before the first frame update
    void Start() //http://localhost:8000/user/
    {
        tDp1Name = "z";
        tDp2Name = "z";
        tDp3Name = "z";
        tDp4Name = "z";
        tDP1mv = 0;
        tDP2mv = 0;
        tDP3mv = 0;
        tDP4mv = 0;
        tDP1fc = 0;
        tDP2fc = 0;
        tDP3fc = 0;
        tDP4fc = 0;
        tDAction = 0; //0 = No Action Yet, 1 = Melee, 2 = Spell, 3 = Self Skill, 4 = Self Spell
        tDActionID = 0; //the Id for the action in a list
        tDTargetName = "z";//Who is being targeted this turn
        tDP1MaxHP = 0; 
        tDP2MaxHP = 0; 
        tDP3MaxHP = 0; 
        tDP4MaxHP = 0; 
        tDP1HP = 0; 
        tDP2HP = 0; 
        tDP3HP = 0; 
        tDP4HP = 0; 
        //StartCoroutine( GetWebData("https://localhost:7114/api/Game/", "1")); //, "http://"localhost:8000/user.gameTurn  //, "foo"
        ProcessGet();
        
        //Debug.Log("Name of target at start: "+tDTargetName);
        
        // StartCoroutine(checkInternetConnection((isConnected)=>{
        //     // handle connection status here
        //     Debug.Log("Player " + myTurn + " connected.");
        // }));
    }

    public void ProcessGet()
    {
        StartCoroutine( GetWebData("http://ddrwebapi-prod.us-west-2.elasticbeanstalk.com/api/Game/", "1"));
        //StartCoroutine( GetWebData("https://localhost:7114/api/Game/", "1")); //, "http://"localhost:8000/user.gameTurn  //, "foo"
    }

    void ProcessServerResponse( string rawResponse )
    {
        JSONNode node = JSON.Parse( rawResponse );

        //Debug.Log("Username: " + node["username"]);
        //Debug.Log("Misc Data: " + node["someArray"][1]["name"] + " = " + node["someArray"][1]["value"]);

        //e.g.
        // Id
        // Players 
        // GameTurn 
        // p1Name
        // p2Name
        // p3Name
        // p4Name
        // P1mv
        // P2mv
        // P3mv
        // P4mv
        // P1fc
        // P2fc
        // P3fc
        // P4fc
        // Action //0 = No Action Yet, 1 = Melee, 2 = Spell, 3 = Self Skill, 4 = Self Spell
        // ActionID //the Id for the action in a list
        // TargetName//Who is being targeted this turn
        // P1MaxHP 
        // P2MaxHP 
        // P3MaxHP 
        // P4MaxHP 
        // P1HP 
        // P2HP 
        // P3HP 
        // P4HP 

        // if(showDebug){
        // Debug.Log("Players: " + node["players"]);
        // Debug.Log("SQL Turn: " + node["gameTurn"]);
        // Debug.Log("P1MV: " + node["p1mv"]);
        // Debug.Log("P2MV: " + node["p2mv"]);
        // Debug.Log("P3MV: " + node["p3mv"]);
        // Debug.Log("P4MV: " + node["p4mv"]);
        // Debug.Log("P1FC: " + node["p1fc"]);
        // Debug.Log("P2FC: " + node["p2fc"]);
        // Debug.Log("P3FC: " + node["p3fc"]);
        // Debug.Log("P4FC: " + node["p4fc"]);}


        playerObjs = GameObject.FindGameObjectsWithTag("Player"); //Return list of all Players

        //Initialize Once
        if(canInitialize)
        {
            //yield return new WaitForSeconds(Random.Range(1, 10));
            if(node["players"] < 4)
            {
                playersTotal = node["players"]+1;//Set local record of how many players there are and what player this is
                ThisPlayerIs = playersTotal; //Only sets once per player
                foreach(GameObject pl in playerObjs)
                {
                    if(pl.GetComponent<BudgeIt>().myTurn == 1)
                    {Debug.Log("I was named: " + node["p1Name"]); tDp1Name = node["p1Name"]; pl.GetComponent<BudgeIt>().myName = node["p1Name"];}
                    if(pl.GetComponent<BudgeIt>().myTurn == 2)
                    {Debug.Log("I was named: " + node["p2Name"]); tDp2Name = node["p2Name"]; pl.GetComponent<BudgeIt>().myName = node["p2Name"];}
                    if(pl.GetComponent<BudgeIt>().myTurn == 3)
                    {Debug.Log("I was named: " + node["p3Name"]); tDp3Name = node["p3Name"]; pl.GetComponent<BudgeIt>().myName = node["p3Name"];}
                    if(pl.GetComponent<BudgeIt>().myTurn == 4)
                    {Debug.Log("I was named: " + node["p4Name"]); tDp4Name = node["p4Name"]; pl.GetComponent<BudgeIt>().myName = node["p4Name"];}
                }
                canInitialize = false;
                ProcessPost();
            }
            else
            {
                //Error, this should never run starting with 4 players already, this must be a test
                playersTotal = 1;
                ThisPlayerIs = playersTotal;
                canInitialize = false;
                ProcessPost();
                //CHANGE THIS
            }
        }



        /*
        {"id":1,"players":4,"gameTurn":1,"p1Name":"aaa","p2Name":"bbb","p3Name":"ccc","p4Name":"itworkedmaybe",
        "p1mv":1,"p2mv":1,"p3mv":1,"p4mv":1,"p1fc":1,"p2fc":1,"p3fc":1,"p4fc":1,"action":1,"actionID":1,"targetName":"aaa",
        "p1MaxHP":1,"p2MaxHP":1,"p3MaxHP":1,"p4MaxHP":1,"p1HP":1,"p2HP":1,"p3HP":1,"p4HP":1}
        */

        //Get Data to send to other players to update
        foreach(GameObject plr in playerObjs) //Loop through each player and update with server data
        {
            Debug.Log("I should see Player 1 moving!!!");
            if(plr.GetComponent<BudgeIt>().myTurn == ThisPlayerIs && ThisPlayerIs != TakeTurn){//For other players
            //All move character right
            if(node["p1mv"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().BudgeRight();}}
            if(node["p2mv"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().BudgeRight();}}
            if(node["p3mv"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().BudgeRight();}}
            if(node["p4mv"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().BudgeRight();}}
            //All move character left
            if(node["p1mv"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().BudgeLeft();}}
            if(node["p2mv"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().BudgeLeft();}}
            if(node["p3mv"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().BudgeLeft();}}
            if(node["p4mv"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().BudgeLeft();}}
            //All move character up
            if(node["p1mv"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().BudgeUp();}}
            if(node["p2mv"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().BudgeUp();}}
            if(node["p3mv"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().BudgeUp();}}
            if(node["p4mv"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().BudgeUp();}}
            //All move character down
            if(node["p1mv"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().BudgeDown();}}
            if(node["p2mv"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().BudgeDown();}}
            if(node["p3mv"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().BudgeDown();}}
            if(node["p4mv"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().BudgeDown();}}
            
            //All face character right
            if(node["p1fc"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().FaceRight();}}
            if(node["p2fc"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().FaceRight();}}
            if(node["p3fc"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().FaceRight();}}
            if(node["p4fc"]==1){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().FaceRight();}}
            //All face character left
            if(node["p1fc"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().FaceLeft();}}
            if(node["p2fc"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().FaceLeft();}}
            if(node["p3fc"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().FaceLeft();}}
            if(node["p4fc"]==2){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().FaceLeft();}}
            //All face character up
            if(node["p1fc"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().FaceUp();}}
            if(node["p2fc"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().FaceUp();}}
            if(node["p3fc"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().FaceUp();}}
            if(node["p4fc"]==3){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().FaceUp();}}
            //All face character down
            if(node["p1fc"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<BudgeIt>().FaceDown();}}
            if(node["p2fc"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<BudgeIt>().FaceDown();}}
            if(node["p3fc"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<BudgeIt>().FaceDown();}}
            if(node["p4fc"]==4){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<BudgeIt>().FaceDown();}}
            
            //Get the dmg
            if(node["targetName"] != "" && plr.GetComponent<BudgeIt>().myTurn == TakeTurn){
                _getstr = plr.GetComponent<CharacterStats>().str;
                _getdice = plr.GetComponent<CharacterStats>().dmg;
            }

            
            _getturn = TakeTurn; _gettarget = node["targetName"]; 
            //Make the attack
            if(node["action"] == 1){
            if(node["actionID"] == 1){
                if(node["targetName"] == plr.GetComponent<BudgeIt>().myName)
                {plr.GetComponent<CharacterStats>().TakeDamage(_getstr, _getturn, _gettarget, false, _getdice); 
                }//Just to create attack effect Debug.Log("No, it's me that's the problem in the ServerTalker");
            }}


            //Reset vars... 
            //But will reset for all
            // tDP1mv = 0;
            // tDP2mv = 0;
            // tDP3mv = 0;
            // tDP4mv = 0;
            // tDP1fc = 0;
            // tDP2fc = 0;
            // tDP3fc = 0;
            // tDP4fc = 0;
            // tDAction = 0; //0 = No Action Yet, 1 = Melee, 2 = Spell, 3 = Self Skill, 4 = Self Spell
            // tDActionID = 0; //the Id for the action in a list
            // tDTargetName = "z";//Who is being targeted this turn

            }//Not current player check

            //Set Current HPs
            // if(node["p1HP"]!=0){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 1 && ThisPlayerIs != 1){plr.GetComponent<CharacterStats>().hp=node["p1HP"];}}
            // if(node["p2HP"]!=0){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 2 && ThisPlayerIs != 2){plr.GetComponent<CharacterStats>().hp=node["p2HP"];}}
            // if(node["p3HP"]!=0){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 3 && ThisPlayerIs != 3){plr.GetComponent<CharacterStats>().hp=node["p3HP"];}}
            // if(node["p4HP"]!=0){if(plr.GetComponent<BudgeIt>().myTurn == TakeTurn && TakeTurn == 4 && ThisPlayerIs != 4){plr.GetComponent<CharacterStats>().hp=node["p4HP"];}}

           //Max HP...
        }

        //ProcessPost(); 

    }


    // static void RecordGameTurn(int takeTurn)
    // {
    //     // var dummyData = {
    //     //     "gameTurn" = takeTurn
    //     // }
    //     JSONNode node = JSON.Parse(node["gameTurn"] = takeTurn);
    //     node["gameTurn"] = takeTurn;
    //     Debug.Log("Game Turn: " + node["gameTurn"]);
    // }

    public void ProcessPost()
    {
        //Debug.Log("ProcessPost fired at least");
        StartCoroutine( Upload("http://ddrwebapi-prod.us-west-2.elasticbeanstalk.com/api/Game/", "1"));
        //StartCoroutine(Upload("https://localhost:7114/api/Game/", "1"));//, "1"
        //StartCoroutine(DeleteData("https://localhost:7114/api/Game/", "2"));
    }

    //public static string Serialize (object? value, Type inputType, System.Text.Json.JsonSerializerOptions? options = default);

    // DELETE FROM "public"."Games" WHERE "Id" > 1


    //byte[] myData = System.Text.Encoding.UTF8.GetBytes("This is some test data");
    //UnityWebRequest put = UnityWebRequest.Put("http://www.my-server.com/upload", myData);

    //Put   
    public IEnumerator Upload( string address, string myId )//, string myId
    {
        //[{"id":1,"players":4,"gameTurn":4,"p1Name":"a","p2Name":"s","p3Name":"d","p4Name":"g","p1x":1,"p1y":2,"p2x":2,"p2y":2,"p3x":2,"p3y":2,"p4x":2,"p4y":2,"action":2,"actionID":2,"targetName":"dff","p1MaxHP":2,"p2MaxHP":1,"p3MaxHP":1,"p4MaxHP":1,"p1HP":1,"p2HP":1,"p3HP":1,"p4HP":1}]
        //UnityWebRequest www = UnityWebRequest.Put(URL_01, "{\"name\":\"user_01\",\"address\":{\"raw\":\"MountFiji\"}}");
        //www.SetRequestHeader ("Content-Type", "application/json");
        Debug.Log("uPLOADfIRED!!");
        WWWForm form = new WWWForm();        
        // Id
        // Players 
        // GameTurn 
        // p1Name
        // p2Name
        // p3Name
        // p4Name
        // P1mv
        // P2mv
        // P3mv
        // P4mv
        // P1fc
        // P2fc
        // P3fc
        // P4fc
        // Action //0 = No Action Yet, 1 = Melee, 2 = Spell, 3 = Self Skill, 4 = Self Spell
        // ActionID //the Id for the action in a list
        // TargetName//Who is being targeted this turn
        // P1MaxHP 
        // P2MaxHP 
        // P3MaxHP 
        // P4MaxHP 
        // P1HP 
        // P2HP 
        // P3HP 
        // P4HP  
        //tDTargetName = "NOTFRICKINGA";
        //Debug.Log("P1mv: " + tDP1mv + ", But I won't set it for no REASON!!");
        form.AddField("Id", 1);
        form.AddField("Players", playersTotal);
        form.AddField("gameTurn", TakeTurn);
        form.AddField("p1Name", tDp1Name);
        form.AddField("p2Name", tDp2Name);
        form.AddField("p3Name", tDp3Name);
        form.AddField("p4Name", tDp4Name);
        form.AddField("P1mv", tDP1mv);
        form.AddField("P2mv", tDP2mv);
        form.AddField("P3mv", tDP3mv);
        form.AddField("P4mv", tDP4mv);
        form.AddField("P5fc", tDP1fc);
        form.AddField("P6fc", tDP2fc);
        form.AddField("P7fc", tDP3fc);
        form.AddField("P8fc", tDP4fc);
        form.AddField("Action", tDAction);
        form.AddField("ActionID", tDActionID);
        form.AddField("TargetName", tDTargetName);
        form.AddField("P1MaxHP", tDP1MaxHP);
        form.AddField("P2MaxHP", tDP2MaxHP);
        form.AddField("P3MaxHP", tDP3MaxHP);
        form.AddField("P4MaxHP", tDP4MaxHP);
        form.AddField("P1HP", tDP1HP);
        form.AddField("P2HP", tDP2HP);
        form.AddField("P3HP", tDP3HP);
        form.AddField("P4HP", tDP4HP);

        byte[] rawData = form.data; 
        
        //Without Id added, error goes from 409 conflict to 405 Method Not Allowed
        string url = address;//+myId;
        var uwr = new UnityWebRequest(url, "PUT");
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(rawData);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); //'application/x-www-form-urlencoded'  ,"application/json"
        yield return uwr.SendWebRequest(); 
        if (uwr.result != UnityWebRequest.Result.Success) 
        {
            if(showDebug){Debug.Log("Turn: " + TakeTurn + ", Something went wrong: " + uwr.error);}
        }
        else
        {
            if(showDebug){Debug.Log("Form upload complete!" + TakeTurn);}
        }

        //ProcessGet(); //Now make sure results are read and distributed to all players
    }

    public IEnumerator PostNew( string address )//, string myId
    {
        WWWForm form = new WWWForm();
        form.AddField("gameTurn", TakeTurn);

        using (UnityWebRequest www = UnityWebRequest.Post(address, form))
        {
            //Send the request then wait here until it returns
            yield return www.SendWebRequest(); //uwr

            if (www.result != UnityWebRequest.Result.Success) //www
            {
                if(showDebug){Debug.Log("Turn: " + TakeTurn + ", Something went wrong: " + www.error);} //uwr
            }
            else
            {
                //Debug.Log("Form upload complete!" + TakeTurn);
            }
        }
    }

    IEnumerator GetWebData( string address, string myId )//, int theTurn 
    {
        UnityWebRequest www = UnityWebRequest.Get(address + myId);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            if(showDebug){Debug.LogError("Something went wrong dude: " + www.error);}
        }
        else
        {
            //Debug.LogError(www.downloadHandler.text);//success

            ProcessServerResponse(www.downloadHandler.text);
        }
    }

    
    //Delete
    IEnumerator DeleteData( string address, string myId )//, int theTurn 
    {
        UnityWebRequest www = UnityWebRequest.Delete(address+myId);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            if(showDebug){Debug.LogError("Can't delete it: " + www.error);}
        }
        else
        {
            if(showDebug){Debug.Log("It's deleted!");}
        }
    }

    //Continuous loop
    void Update()
    {
        //Toggle single player mode for testing
        if(Input.GetKeyDown("m"))
        {
            if(SinglePlayerMode == true){SinglePlayerMode = false;}
            else{if(SinglePlayerMode == false){SinglePlayerMode = true;}}
        }

        //Run a get data
        if(checkNow == true)
        {
            ProcessGet();
            checkNow = false;
        }

        //Get data every so often for all players
        if(_checkGet < 30.0f && ThisPlayerIs != TakeTurn)
        {
            ProcessGet();
            _checkGet = Time.deltaTime*2.0f;
        }
        if(_checkGet > 0){_checkGet -= Time.deltaTime*1.0f;}

        //Cheat to control other player turns in multiplayer test
        if(Input.GetKeyDown("1")){ThisPlayerIs = 1;}
        if(Input.GetKeyDown("2")){ThisPlayerIs = 2;}
        if(Input.GetKeyDown("3")){ThisPlayerIs = 3;}
        if(Input.GetKeyDown("4")){ThisPlayerIs = 4;}
    }

    public void ExitTheGame()
    {
        //Editor
        UnityEditor.EditorApplication.isPlaying = false;
        //WebGL
        Application.OpenURL("about:blank");
        //Stand Alone
        Application.Quit();
    }

}



