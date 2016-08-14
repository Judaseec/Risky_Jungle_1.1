using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Boomlagoon.JSON;
using System.Text;
using Assets.Scripts.com.ethereal.util;

/** 
* @author    Carlos Andres Carvajal <ccarvajal@etherealgf.com> 
* @version   1.0 
* @date      Marzo 27 2014
* 
* @class Inventory 
* @brief Esta clase se encarga de manejar los items de inventario
* 
*  Maneja todos los items que puede tener un usuario, maneja una cantidad maxima por cada item y si el item se autoagrega cada
*  x tiempo
*/
public class Inventory: MonoBehaviour{

    /**
    *  @brief Instancia de la clase
    */
    private static Inventory _instance = null;

    public delegate void OnInventoryReady( bool isNew );
    //public delegate void OnInventorySaved( bool success );
    private event OnInventoryReady OnReady;
    private event DataManager.OnSavedData OnSaved;

    private ArrayList _items;

    /**
    *  @brief Obtiene la instancia de la clase
    *  @return la instancia de la clase
    */
    public static Inventory GetInstance(){
        if(_instance == null){
            ObjectFactory.CreateObject<Inventory>("Inventory");   
        }
        return _instance;
    }

    /**
     *funcion que se llama cuando se "instancia" la clase, inicializa el array de items  
     */
    void Awake(){
        DontDestroyOnLoad( gameObject );
        Inventory._instance = this;
        _items = new ArrayList();   

        //cancela las local notificaciones
        CancelLocalNotifications();      
    }

    //se llama una vez cada frame, y actualiza los items que son por tiempo
    void Update(){
        EthItem itemTemp;
        bool wasUpdated = false;
        for ( int i = 0; i < _items.Count; i ++ ){
            itemTemp = (EthItem)_items[i];
            if ( itemTemp.IsAutoRefillable ){
                wasUpdated = itemTemp.CheckTime() || wasUpdated;//si ya hubo uno que no lo ponga false
            }
        }

        if ( wasUpdated ){
            Save( null );
        }
    }

    /**
     * inicia el inventario, llama al datamanager para que le de los items que esten guardados
     *
     * recibe la funcion que se llama cuando se obtenga respuesta
     */
    public void StartInventory( string remoteUrl, OnInventoryReady fn ){
        OnReady = fn;
        DataManager.URL_GAME_DATA = remoteUrl;
        DataManager.GetInstance().GetDataPersistent( OnInventoryReceived, this );
    }

    /**
     * llama al datamanager para que le de los items que esten guardados, util para
     * cuando se pasa de estado offline a online
     *
     * recibe la funcion que se llama cuando se obtenga respuesta
     */
    public void UpdateInventory( OnInventoryReady fn ){
        OnReady = fn;
        DataManager.GetInstance().GetDataPersistent( OnInventoryReceived, this );
    }

    /**
     * funcion que se llama cuando se obtiene respuesta del data manager sobre los items que estan guardados
     *
     * recibe un json con los items del inventario
     */
    private void OnInventoryReceived( bool success, string inventory, string other, bool isNew ){
        Debug.Log("llega: " + inventory);
        JSONObjectBoom json = JSONObjectBoom.Parse( inventory );
        _items = new ArrayList();
        //llena items
        if ( json != null ){
            JSONArray itemsArray = json.GetArray( "inventory" );
            for ( int i = 0; i < itemsArray.Length; i ++){
                AddItem( EthItem.ParseJson( itemsArray[i].ToString() ) );
            }
        }

        if ( OnReady != null ){
            OnReady( isNew );
        }
    }

    /**
     * funcion para encontrar un item del inventario de acuerdo al id
     *
     * recibe el id del item
     */
    public EthItem FindItem( string id ){
        EthItem itemTemp;
        for ( int i = 0; i < _items.Count; i ++ ){
            itemTemp = (EthItem)_items[i];
            if ( itemTemp.Id == id ){
                return itemTemp;
            }
        }
        return null;
    }

    /**
     * crea un item en el inventario
     *
     * recibe el nuevo item
     */
    public void CreateItem( EthItem newItem ){
        EthItem itemTemp = FindItem( newItem.Id );
        if ( itemTemp == null ){//si no existe lo mete a los datos para luego cargar
            Debug.Log("no existe?");
            AddItem( newItem );
        }
        else{
            //existe, se deja el de persistencia y se actualiza el callback y los demas datos que pueden cambiar
            itemTemp.Callback = newItem.Callback;

            itemTemp.Name = newItem.Name;
            itemTemp.Description = newItem.Description;
            itemTemp.UrlImage = newItem.UrlImage;
            ///itemTemp.MaxCount = newItem.MaxCount;
            itemTemp.InitialCount = newItem.InitialCount;
            
            itemTemp.IsAutoRefillable = newItem.IsAutoRefillable;
            //itemTemp.TimeRefill = newItem.TimeRefill;
            //itemTemp.ShowLocalNotification = newItem.ShowLocalNotification;
        }
    }

    /**
     * agrega un item al inventario
     */
    private void AddItem( EthItem newItem ){
        _items.Add( newItem );
    }

    /**
     * graba los items que esten en el inventario, usa el data manager y recibe la funcion que se llama
     * cuando se acabe de grabar los items
     */
    public void Save( DataManager.OnSavedData fn ){
        OnSaved = fn;
        DataManager.GetInstance().SaveInventory( ToString() , OnSavedInventory, this );
    }

    public override string ToString(){
        StringBuilder sb = new StringBuilder("{\"inventory\":[");
        EthItem itemTemp;
        for ( int i = 0; i < _items.Count; i ++ ){
            itemTemp = (EthItem)_items[i];
            sb.Append( itemTemp.ToJson() );
            sb.Append( "," );
        }

        if (_items.Count > 0) {
            sb.Remove(sb.Length - 1, 1);
        }

        sb.Append( "] }" );
        return sb.ToString();
    }

    /**
     * funcion que se llama cuando se acaba de grabar los items del inventario
     */
    private void OnSavedInventory( bool success ){
        Debug.Log("success: " + success);
    }

    /**
     * incrementa una cantidad a un item o su maxima cantidad
     * id el id del item que se va a modificar
     * counttoAdd la cantidad que se le va a sumar al item
     * ismaxCount para saber si es a la cantidad o a la maxima cantidad
     */
    public void IncreaseCountItem( string id, int countToAdd, bool isMaxCount = false ){
        EthItem itemTemp = FindItem( id );
        if ( itemTemp != null ){
            if ( isMaxCount ){
                itemTemp.MaxCount += countToAdd;
            }
            else{
                itemTemp.Count += countToAdd;

                if ( itemTemp.Count > itemTemp.MaxCount ){
                    itemTemp.Count = itemTemp.MaxCount;
                }

                if ( itemTemp.Count < 0 ){
                    itemTemp.Count = 0;
                }
            }
        }
    }

    /**
     * obtiene la cantidad actual de un item o su maxima cantidad
     * id el id del item que se va a modificar
     * ismaxCount para saber si es a la cantidad o a la maxima cantidad
     */
    public int GetCountItem( string id, bool isMaxCount = false){
        EthItem itemTemp = FindItem( id );
        if ( itemTemp != null ){
            if ( isMaxCount ){
                return itemTemp.MaxCount;
            }
            else{
                return itemTemp.Count;
            }
        }
        return 0;
    }

    /**
     * Funcion que se llama antes de que la aplicacion se cierr
     * */
    void OnApplicationQuit() {
        SetLocalNotifications();
    }

    /*mira los items que se rellenan por tiempo y mira si tienen habilitada la 
        notificacion local para colocarla en caso de que se necesite
     *
     * el mensaje se toma del diccionario del lenguaje, la llave tiene que ser asi: "LN_" + id del item 
    */
    private void SetLocalNotifications(){
        EthItem itemTemp;
        for ( int i = 0; i < _items.Count; i ++ ){
            itemTemp = (EthItem)_items[i];
            
            if ( itemTemp.IsAutoRefillable && itemTemp.ShowLocalNotification ){
                if ( itemTemp.Count < itemTemp.MaxCount ){
                    Debug.Log("coloca " + "LN_" + itemTemp.Id + " tiempo: " + itemTemp.GetSecondsToFill() );
                    UTNotifications.Manager.Instance.ScheduleNotification(itemTemp.GetSecondsToFill(), "WitchesVsMonsters", EthLang.GetEntry ("LN_" + itemTemp.Id, true), i);
                }
            }
        }
    }

    /*funcion que se llama cuando se pausa la aplicacion o se hace un softcierre,
    */
    void OnApplicationPause(bool pauseStatus) {
        //se pauso o se hizo un soft cierre
        if ( pauseStatus ){
            SetLocalNotifications();
        }
        else{//cancela las local notificaciones
            CancelLocalNotifications();
        }
    }

    private void CancelLocalNotifications(){
        Debug.Log("canceladas");
        UTNotifications.Manager.Instance.CancelAllNotifications();
        UTNotifications.Manager.Instance.SetBadge(0);
    }

    public void StartPushes(){
        UTNotifications.Manager notificationsManager = UTNotifications.Manager.Instance;
        //notificationsManager.OnSendRegistrationId += SendRegistrationId;
        ///notificationsManager.OnNotificationsReceived += OnNotificationsReceived;
        notificationsManager.Initialize(false, 0, false);

    }

    /// <summary>
    /// A wrapper for the <c>_SendRegistrationId</c> coroutine
    /// </summary>
    protected void SendRegistrationId(string providerName, string registrationId){
        DataManager.GetInstance().SendRegistrationId( providerName, registrationId, this);
    }

    /// <summary>
    /// Processes the received notifications.
    /// </summary>
    protected void OnNotificationsReceived(IList<UTNotifications.ReceivedNotification> receivedNotifications)
    {
        Debug.Log("llega notificacion");
    }

}
