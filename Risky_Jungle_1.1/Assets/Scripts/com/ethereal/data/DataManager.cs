using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.com.ethereal.util;
using Boomlagoon.JSON;
using com.ethereal.social;
using System;

/** 
* @author    Carlos Andres Carvajal <ccarvajal@etherealgf.com> 
* @version   1.0 
* @date      Marzo 27 2014
* 
* @class DataManager 
* @brief Esta clase se encarga de manejar los items de inventario
* 
*  Maneja todos los items que puede tener un usuario, maneja una cantidad maxima por cada item y si el item se autoagrega cada
*  x tiempo
*/
public class DataManager{

    /**
    *  @brief Instancia de la clase
    */
    private static DataManager _instance = null;
    
    private const string KEY_INVENTORY = "INVENTORY";
    private const string KEY_OTHER_PERSISTENCE = "PERSISTENCE";
    private const string KEY_LEVEL = "LEVEL_GAME";
    private const string KEY_DATA_USER = "FB_DATA_USER";
    
	private const string KEY_FB_ID_PUSH = "FB_ID_PUSH";
    private const string KEY_ID_PUSH = "ID_PUSH";
    private const string KEY_CURRENT_TIME = "CURRENT_TIME";

    public delegate void OnDataReceived( bool success, string inventoryJSON, string otherPersistence, bool isNewPersistence );
    public delegate void OnSavedData( bool success );
    public delegate void OnRequestsReceived( bool success, string requestsJson );
    public delegate void OnNewMessagesReceived( int count );

    public event OnDataReceived OnData;
    public event OnSavedData OnSavedInventory;
    public event OnSavedData OnSavedOther;
    public event OnSavedData OnSavedRequest;
    public event OnRequestsReceived OnRequests;
    public event OnNewMessagesReceived OnNewMessages;

    private bool _online;
    private int _idGame = 0;
    public static string URL_GAME_DATA;
    private string _otherPersistence = "";
    private string _defaultPersistence = "";

	private string idPush = null;
    /**
    *  @brief Obtiene la instancia de la clase
    *  @return la instancia de la clase
    */
    public static DataManager GetInstance(){
        if(_instance == null){
            _instance = new DataManager();
        }
        return _instance;
    }

    public void SetDefaultPersistence( string defPersistence){
        _defaultPersistence = defPersistence;
    } 

    /**
    *  @brief Constructor de la clase
    */
    private DataManager(){
        
		idPush = EthPersistent.GetInstance().GetString( KEY_ID_PUSH, "" );
    }

    public void GetDataPersistent( OnDataReceived fn, MonoBehaviour parent ){
        OnData = fn;

        //check si esta online
        _online = CheckOnline( parent );

        if ( _online ){
            //obtener id si esta online
            string idUser = GetUser();
            //obtener idgame si esta online
            string idGame = GetIdGame();

            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            data.Add("user", idUser);
            data.Add("game",idGame);
                                           
            loader.OnRespCB += callbackGetInventory;
            loader.POST( URL_GAME_DATA + "getUserInv.php", data);
        }
        else{
           GetLocalInventory();
        }
    }

    public void callbackDataUser( bool success, string resp ){
        if ( success ){
            if ( resp == "OK" ){
                EthPersistent.GetInstance().SaveInt( KEY_DATA_USER, 1 );
            }
        }
    }

    public void callbackGetInventory( bool success, string resp ){
        string inventory = "";
        int levelOnline = -1;
        bool isNewPersistence = true;
        
        if ( success ){
            JSONObjectBoom json = JSONObjectBoom.Parse(resp);
            if(json.GetString("resp") == "ok") {
                inventory = json.GetObject("inv").ToString();
                OtherPersistence = json.GetObject("other").ToString();
                if ( OtherPersistence == "{}"){
                    OtherPersistence = EthPersistent.GetInstance().GetString( KEY_OTHER_PERSISTENCE, _defaultPersistence );
                }

                levelOnline = (int)json.GetNumber("lev");
                if ( OtherPersistence == "" ){
                    OtherPersistence = _defaultPersistence;
                }
            }
        }

        int levelLocal = EthPersistent.GetInstance().GetInt( KEY_LEVEL, 0 );

        if ( levelOnline < levelLocal ){
            isNewPersistence = false;
            inventory = EthPersistent.GetInstance().GetString( KEY_INVENTORY, "" );
            OtherPersistence = EthPersistent.GetInstance().GetString( KEY_OTHER_PERSISTENCE, _defaultPersistence );
        }
        
        if ( OnData != null ){
            OnData( success, inventory, OtherPersistence, isNewPersistence );
        }
    }

    public void SaveInventory( string inventory, OnSavedData fn, MonoBehaviour parent){
        OnSavedInventory = fn;

        //check si esta online
        _online = CheckOnline( parent );

        if ( _online ){
            //obtener id si esta online
            string idUser = GetUser();
            //obtener idgame si esta online
            string idGame = GetIdGame();

            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            data.Add("user", idUser);
            data.Add("game",idGame);
            data.Add("inv",inventory);
                                           
            loader.OnRespCB += CallbackSaveInventory;
            loader.POST( URL_GAME_DATA + "setUserInv.php", data);
        }
        
        SaveLocalInventory( inventory );
        
    }

    public void CallbackSaveInventory( bool success, string resp ){
        bool wasSaved = false;
        if ( success ){
            JSONObjectBoom json = JSONObjectBoom.Parse(resp);
            if(json.GetString("resp") == "ok") {
               wasSaved = true;
            }
        }

        if ( OnSavedInventory != null ){
            OnSavedInventory( wasSaved );
        }
    }

    //Graba la otra persistencia del juego
    public void SaveOtherPersistence( string persistence, int level, OnSavedData fn, MonoBehaviour parent){
        OnSavedOther = fn;

        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            //obtener id si esta online
            string idUser = GetUser();
            //obtener idgame si esta online
            string idGame = GetIdGame();

            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            data.Add("user", idUser);
            data.Add("game",idGame);
            data.Add("per",persistence);
            data.Add("lev",""+level);
                                           
            loader.OnRespCB += CallbackSavePersistence;
            loader.POST( URL_GAME_DATA + "setUserPers.php", data);

            if ( EthPersistent.GetInstance().GetInt( KEY_DATA_USER, 0 ) == 0 && EthFacebook.GetInstance().Name != null){
                URLLoader loaderData = new URLLoader( parent );

                Dictionary<string, string> data2 = new Dictionary<string, string>();             
                    
                data2.Add("user", idUser);
                data2.Add("game",idGame);
                data2.Add("name", EthFacebook.GetInstance().Name);
                data2.Add("email", (EthFacebook.GetInstance().Email == null ? "" : EthFacebook.GetInstance().Email));
                data2.Add("birthday", (EthFacebook.GetInstance().Birthday == null ? "" : EthFacebook.GetInstance().Birthday));
                data2.Add("gender", (EthFacebook.GetInstance().Gender == null ? "" : EthFacebook.GetInstance().Gender));
                            
                loaderData.OnRespCB += callbackDataUser;
                loaderData.POST( URL_GAME_DATA + "setUserInfo.php", data2);
            }
        }
        
        SaveLocalPersistence( persistence, level );
    }

    public void CallbackSavePersistence( bool success, string resp ){
        bool wasSaved = false;
        if ( success ){
            JSONObjectBoom json = JSONObjectBoom.Parse(resp);
            if(json.GetString("resp") == "ok") {
               wasSaved = true;
            }
        }

        if ( OnSavedOther != null ){
            OnSavedOther( wasSaved );
        }
    }

    private void SaveLocalInventory( string inventory ){
        EthPersistent.GetInstance().SaveString( KEY_INVENTORY, inventory );
        EthPersistent.GetInstance().SaveToDisk();
        if ( OnSavedInventory != null ){
            OnSavedInventory( true );
        }
    }

    private void GetLocalInventory(){
        string inventory = EthPersistent.GetInstance().GetString( KEY_INVENTORY, "" );
        OtherPersistence = EthPersistent.GetInstance().GetString( KEY_OTHER_PERSISTENCE, _defaultPersistence );
        
        if ( OnData != null ){
            OnData( true, inventory, OtherPersistence, true );
        }
    }

    private void SaveLocalPersistence( string persistence, int level ){
        EthPersistent.GetInstance().SaveString( KEY_OTHER_PERSISTENCE, persistence );
        EthPersistent.GetInstance().SaveInt( KEY_LEVEL, level );
        EthPersistent.GetInstance().SaveToDisk();
        if ( OnSavedOther != null ){
            OnSavedOther( true );
        }
    }

    public void SaveRequest( string resultFBRequest, string objectId, MonoBehaviour parent, OnSavedData fn ){
        OnSavedRequest = fn;
        
        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            string idUser = GetUser();
            string idGame = GetIdGame();
            data.Add("user", idUser);
            data.Add("game",idGame);
            data.Add("result",resultFBRequest);
            data.Add("objectId",objectId);
                                           
            loader.OnRespCB += CallbackSaveRequest;
            loader.POST( URL_GAME_DATA + "saveRequest.php", data);
        }
        else{
            if ( OnSavedRequest != null ){
                OnSavedRequest( false );
            }
        }
    }

    public void CallbackSaveRequest( bool success, string resp ){
        bool wasSaved = false;
        if ( success ){
            JSONObjectBoom json = JSONObjectBoom.Parse(resp);
            if(json.GetString("resp") == "ok") {
               wasSaved = true;
            }
        }

        if ( OnSavedRequest != null ){
            OnSavedRequest( wasSaved );
        }
    }

    public void GetRequests( MonoBehaviour parent, OnRequestsReceived fn  ){
        OnRequests = fn;
        
        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            string idUser = GetUser();
            string idGame = GetIdGame();
            data.Add("user", idUser);
            data.Add("game",idGame);
                                           
            loader.OnRespCB += CallbackGetRequests;
            loader.POST( URL_GAME_DATA + "getRequests.php", data);
        }
        else{
            if ( OnRequests != null ){
                OnRequests( false, "" );
            }
        }
    }

    private void CallbackGetRequests( bool success, string resp ){
        if ( OnRequests != null ){
            OnRequests( success, resp );
        }
    }

    public void NoTakeRequest( int idRequest, MonoBehaviour parent ){
        TakeRequest( idRequest, parent, "5" );
    }

    public void TakeRequest( int idRequest, MonoBehaviour parent, string state = "4" ){
        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            string idUser = GetUser();
            string idGame = GetIdGame();
            
            data.Add("user", idUser);
            data.Add("game", idGame);
            data.Add("id", ""+idRequest);
            data.Add("state", state );
                                           
            loader.POST( URL_GAME_DATA + "takeRequest.php", data);
        }
    }

    public void RejectRequest( int idRequest, MonoBehaviour parent ){
        AcceptRequest( idRequest, parent, "3" );
    }

	//idrequest que se acepta, state de aceptar el request, por defecto es 2, url game, url que se le agrega a la url
    //esto por si se quiere cambiar el normal que solo cambia el estado
    public void AcceptRequest( int idRequest, MonoBehaviour parent, string state = "2", string urlGame = "" ){
        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            string idUser = GetUser();
            string idGame = GetIdGame();
            
            data.Add("user", idUser);
            data.Add("game", idGame);
            data.Add("id", ""+idRequest);
            data.Add("state", state );
                                           
            loader.POST( URL_GAME_DATA + urlGame + "acceptRequest.php", data);
        }
    }

    public void ClearAllRequests( MonoBehaviour parent ){
        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            string idUser = GetUser();
            string idGame = GetIdGame();
            
            data.Add("user", idUser);
            data.Add("game", idGame);
                                           
            loader.POST( URL_GAME_DATA + "clearRequests.php", data);
        }
    }

    public void RemoveRequestsObject( string objectRequest, MonoBehaviour parent ){
        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            string idUser = GetUser();
            string idGame = GetIdGame();
            
            data.Add("user", idUser);
            data.Add("game", idGame);
            data.Add("obj", objectRequest);
                                           
            loader.POST( URL_GAME_DATA + "remRequestsObject.php", data);
        }
    }

    public void GetCountNewMessages( MonoBehaviour parent, OnNewMessagesReceived fn){
        OnNewMessages = fn;
        //check si esta online
        _online = CheckOnline( parent );
        
        if ( _online ){
            URLLoader loader = new URLLoader( parent );

            Dictionary<string, string> data = new Dictionary<string, string>();             
                
            string idUser = GetUser();
            string idGame = GetIdGame();
            
            data.Add("user", idUser);
            data.Add("game", idGame);
                                  
            loader.OnRespCB += CallbackCountNew;
            loader.POST( URL_GAME_DATA + "countNewMes.php", data);
        }
    }

    private void CallbackCountNew( bool success, string resp ){
        int count = 0;
        if ( success ){
            JSONObjectBoom json = JSONObjectBoom.Parse(resp);
            count = (int)json.GetNumber("count");
        }

        if ( OnNewMessages != null ){
            OnNewMessages( count );
        }
    } 

    private string GetUser(){
        if ( EthFacebook.GetInstance().IsLoggedIn() ){
            return EthFacebook.GetInstance().IdUser();
        }

        return "GUEST";
    }

    private string GetIdGame(){
        if ( _idGame == 0 ){
            Debug.LogWarning("Ojo-no se ha puesto el id del Game");
        }
        return ""+_idGame;
    }

    public void SetIdGame(int idGame ){
        _idGame = idGame;
    }

    public string OtherPersistence {
        get {
            return _otherPersistence;
        }
        set {
            _otherPersistence = value;
        }
    }

    private bool CheckOnline( MonoBehaviour parent ){

        
        //mira si esta logueado para guardar el id de facebook con los datos de push
        if ( EthFacebook.GetInstance().IsLoggedIn() && EthPersistent.GetInstance().GetInt( KEY_FB_ID_PUSH, 0 ) == 0){
            SendFbToPush(parent);
        }

        return EthFacebook.GetInstance().IsLoggedIn();

    }
	private void SendFbToPush( MonoBehaviour parent ){
        if ( idPush != null ){
            URLLoader loaderData = new URLLoader( parent );

            Dictionary<string, string> data2 = new Dictionary<string, string>();  

            string idUser = GetUser();
            string idGame = GetIdGame();           
                
            data2.Add("user", idUser);
            data2.Add("game",idGame);
            data2.Add("id", idPush);
            
            loaderData.OnRespCB += callbackFbIdPush;
            loaderData.POST( URL_GAME_DATA + "/push/setFbId.php", data2);
        }
        
    }

    public string GetLocalLanguage(){
        string tempPersistence = EthPersistent.GetInstance().GetString( KEY_OTHER_PERSISTENCE, _defaultPersistence );
        
        if ( tempPersistence != "" ){
            JSONObjectBoom json = JSONObjectBoom.Parse(tempPersistence);

            if ( json.ContainsKey("lang") ){
                return json.GetString("lang");
            }
        }

        return "en";
    }



    public int GetLocalLevels(){
        return EthPersistent.GetInstance().GetInt( KEY_LEVEL, 0 );
    }

    public void SendLog( string log, MonoBehaviour parent ){
        //obtener idgame si esta online
        string idGame = GetIdGame();

        URLLoader loader = new URLLoader( parent );

        Dictionary<string, string> data = new Dictionary<string, string>();             
            
        data.Add("log", log);
        data.Add("game",idGame);
                                       
        loader.POST( URL_GAME_DATA + "setLog.php", data);
    }

    public void SendError( string log, MonoBehaviour parent ){
        //obtener idgame si esta online
        string idGame = GetIdGame();

        URLLoader loader = new URLLoader( parent );

        Dictionary<string, string> data = new Dictionary<string, string>();             
            
        data.Add("log", log);
        data.Add("game",idGame);
                                       
        loader.POST( URL_GAME_DATA + "setError.php", data);
    }
	
    public void SendRegistrationId(string providerName, string registrationId, MonoBehaviour parent ){
        if ( idPush != registrationId ){
            EthPersistent.GetInstance().SaveString( KEY_ID_PUSH, registrationId );
            idPush = registrationId;
            EthPersistent.GetInstance().SaveInt( KEY_FB_ID_PUSH, 0 );
        }
        
        string idGame = GetIdGame();

        URLLoader loader = new URLLoader( parent );

        Dictionary<string, string> data = new Dictionary<string, string>();             
            
        data.Add("provider", providerName);
        data.Add("game",idGame);
        data.Add("id",registrationId);

        string user = GetUser();
        if ( user != "GUEST" ){
            data.Add("user",user);
        }
                                       
        loader.POST( URL_GAME_DATA + "/push/register.php", data);
    }

    public void callbackFbIdPush( bool success, string resp ){
        if ( success ){
            if ( resp == "OK" ){
                EthPersistent.GetInstance().SaveInt( KEY_FB_ID_PUSH, 1 );
            }
        }
    }

    public void SaveInstallTime(){
        DateTime currentDate = DateTime.Now;       
        EthPersistent.GetInstance().SaveString( KEY_CURRENT_TIME, ""+ currentDate.ToBinary() );
    }

    public DateTime GetInstallTime(){
        DateTime lastDate;
        string date = EthPersistent.GetInstance().GetString( KEY_CURRENT_TIME,  "" );
        if ( date == "" ){
            lastDate = DateTime.Now;
        }
        else{
            lastDate = DateTime.FromBinary( Convert.ToInt64( date ) );
        }
        return lastDate;
    }

    
}
