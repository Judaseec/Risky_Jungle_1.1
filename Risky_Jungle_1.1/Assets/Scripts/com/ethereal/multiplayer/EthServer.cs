using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Julio 16 2014
* 
*	@class 	EthServer
*   @brief 	Esta clase está encargada del servidor.
*
*	Llevara a cabo la administración de todos los clientes para que el servidor pueda proveer de sus servicios a cada unos de los usuarios,
*	sincronizando que es lo que recibiran cada uno de ellos y como lo recibiran.
*
*/
public class EthServer : MonoBehaviour {
	
	/**
	*	@brief Lista de usuarios conectados.
	*/
	private ArrayList onlineUsersList;

	/**
	*	@brief Variable que identifica si el servidor esta iniciado o no.
	*/
	private bool initiate;

	/**
	*	@brief Es la manera de conexion en los juegos multijugador.
	*	Con este se peude definir exactamente lo que se va a sincronizar en la red y como debe hacerse.
	*/
	private NetworkView networkView;
    public UnityEngine.NetworkView NetworkView
    {
        get { return networkView; }
        set { networkView = value; }
    }
	/**
	*	@brief Diccionario de datos que se encarga de los sectores del servidor identificados con un string.
	*/
	private Dictionary<string, EthRoom> rooms;
	
	
	// The event

	/**
	*	@brief	Evento llamado cuando el sever se ha inicializado, que luego sera modificado.
	*/
	public event OnServerInitializedEvent OnServerInitializedCB;

	/**
	*	@brief	Evento llamado cuando un jugador se ha conectado, que luego sera modificado.
	*/
	public event OnPlayerConnectedEvent OnPlayerConnectedCB;

	/**
	*	@brief	Evento llamado cuando un jugador se ha desconectado, que luego sera modificado.
	*/
	public event OnPlayerDisconnectedEvent OnPlayerDisconnectedCB;

	/**
	*	@brief	Evento llamado cuando se inicia un componente, que luego sera modificado.
	*/
	public event OnComponentStartedEvent OnComponentStarted;
	
	//Temp
	//Main main;

	// Use this for initialization
	/**
    *	@brief	Método usado para preparar el inicio el servidor.
    *	
    *	Se añade el componente de red Networkview al gameObject, sincronizando la red e inicializando cada uno de los atributos necesarios 
    *	para esta clase. 
    *	
    */
	void Start () {
		networkView = (NetworkView) this.gameObject.AddComponent <NetworkView>();
		networkView.stateSynchronization = NetworkStateSynchronization.Off;
				
		
		onlineUsersList = new ArrayList ();	
		initiate = false;
		
		rooms = new Dictionary <string, EthRoom> ();
		
		//temp
		//main = (Main)this.gameObject.GetComponent("Main");
		
		if ( OnComponentStarted != null ) {
			OnComponentStarted ();	
		}
	}
	
	/**
	*	@brief Método usado para actuaizar la conexión.
	*	
	*/
	void Update () {

	}
	
	/**
    *	@brief	Metodo usado para agregar un jugador a la lista de jugadores conectados.
    *	
    *	@param player Jugador que sera agregado a la lista de conectados.
    */
	private void AddPlayer (EthPlayerNetWork player) {
		onlineUsersList.Add (player);
	}
	
	/**
    *	@brief	Metodo usado para obtener un jugador de la lista de conectados.
    *	
    *	Método encargado de obtener un jugador especificado, buscandolo en la lista de conectados que esta clase posee. 
    *	
    *	@param player Jugador que sera agregado a la lista de conectados.
    *
    *	@return Jugador requerido.
	*/
	private EthPlayerNetWork GetPlayer (NetworkPlayer player) {
		foreach (EthPlayerNetWork input in onlineUsersList) {
			if ( input.Player == player ) {
				return input;	
			}
		}
		
		return null;
	}
	
	/**
    *	@brief	Metodo usado para remover un jugador del sector del servidor en el que se encuentra.
    *	
    *	Además se remueve al jugador de la lista de usuarios conectados, usado principalmente cuando el usuario se desconecta del 
    *	servidor y no ha abandonado el sector del servidor en el que se encuentra actualmente. 
    *	
    *	@param player Jugador que sera removido.
    *
    *	@return Jugador removido.
    */
	private EthPlayerNetWork RemovePlayer (NetworkPlayer player) {
		EthPlayerNetWork playerRemoved = GetPlayer (player);

		Debug.Log ("encuentra: " + playerRemoved);

		//si estaba en un room se remueve primero de ese
		if ( playerRemoved.IdCurrentRoom != null ) {
			Debug.Log ("entra a room a desconectar ");
			rooms [playerRemoved.IdCurrentRoom].LeaveRoom (playerRemoved);
		}
		
		onlineUsersList.Remove (playerRemoved);
		return playerRemoved;
	}
	
	/**
	*	@brief	Metodo usado para modificar la informacion de un jugador especificado.
    *	
    *	Éste metodo modifica el nombre, el id y el avatar del usuario. 
    *	
    *	@param player 		Jugador a modificar su información.
    *	@param name 		Nombre del usuario.
    *	@param idUsuario 	Id del usuario.
    *	@param avatar 		Avatar usado por el usuario en el juego.
    *	
    *	@return Jugador con su información modificada.
    */
	private EthPlayerNetWork SetInfoPlayer (NetworkPlayer player, string name, string idUsuario, string avatar) {
		EthPlayerNetWork playerEth = GetPlayer (player);
		
		if ( playerEth != null ) {
			playerEth.Name = name;	
			playerEth.Id = idUsuario;
			playerEth.Avatar = avatar;
		}
		
		return playerEth;
	}
	
	/**
    *	@brief	Getter y setter del atributo initiate referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de initiate.
    */
	public bool Initiate {
		get {
			return this.initiate;
		}
		set {
			initiate = value;
		}
	}
	
	/**
    *	@brief	Método encargado de recorrer los room y obtener usuarios. 
	*
    *	@param player Jugador al que se le enviaran los usuarios.
    *
    */
	public void SendRoomListToUser (EthPlayerNetWork player) {
		//recorrer los room y obtener usuariosd
	}
	
	//EVENTOS DEL SERVER
	/**
    *	@brief	Metodo usado para Iniciar el servidor.
    *	
    *	Inicia el servidor por medio de la función InitializeServer de la clase Network enviando el numero de conexiones
    *	y el puerto en donde se va a iniciar el servidor. 
    *	
    *	@param numConnections 	Número de conexiones que tendrra el servidor.
    *	@param port 			Puerto en el que sera iniciado el servidor.
    *
    */
	public void StartServer (int numConnections, int port) {
		Network.InitializeServer (numConnections, port, !Network.HavePublicAddress ());
		//MasterServer.RegisterHost("mifinquita", "mifinquita");
	}
	
	/**
    *	@brief	Metodo usado para crear un nuevo sector del servidor.
    *	
    *	Al crear un nuevo sector del servidor, éste estará indicado con un id y tendrá un máximo de usuarios
    *	que puede contener. 
    *	
    *	@param id 		Id del sector del servidor con el que seré identicado.
    *	@param maxUsers Cantidad máxima de usuarios que aceptará el nuevo sector del servidor.
    *
    */
	public void CreateRoom (string id, int maxUsers) {
		EthRoom room = new EthRoom (id, networkView);
		rooms.Add (id, room);
	}
	
	/**
    *	@brief	Método realizado una vez el servidor ya se haya inicializado.
    *	
    *	Éste se especificará de acuerdo al juego que lo ejecute. 
    *	
	*/
	void OnServerInitialized () {
		this.Initiate = true;
		
		if ( OnServerInitializedCB != null ) {
			OnServerInitializedCB ();	
		}
	}
	
	/**
    *	@brief	Método realizado una vez un jugador se haya conectado al servidor.
    *	
    *	Éste se especificará de acuerdo al juego que lo ejecute.
    *	
    *	@param player Jugador que se ha conectado.
    *
    */
	void OnPlayerConnected (NetworkPlayer player) {
		EthPlayerNetWork newPlayer = new EthPlayerNetWork ();
		newPlayer.Player = player;
		AddPlayer (newPlayer);
		
		if ( OnPlayerConnectedCB != null ) {
			OnPlayerConnectedCB (newPlayer);	
		}
	}
	
	/**
    *	@brief	Método realizado una vez un jugador se haya desconectado del servidor.
    *	
    *	Éste se especificará de acuerdo al juego que lo ejecute.
    *	
    *	@param player Jugador que se ha desconectado.
    *
    */
	void OnPlayerDisconnected (NetworkPlayer player) {
		Debug.Log ("desconectar ");
		EthPlayerNetWork removed = this.RemovePlayer (player);	
		
		if ( OnPlayerDisconnectedCB != null ) {
			OnPlayerDisconnectedCB (removed);	
		}
	}
	
	//METODOS RPC
	/**
    *	@brief	Método encargado de enviar una respuesta al cliente para modificar la información de este.
    *	
    *	Éste metodo la información de un cliente y luego de enviarlo modificado, avisa a los demás clientes que un 
    *	jugador se ha conectado.
    *	
    *	@param idUsuario 	Id de usuario a modificar.
    *	@param name 		nombre del cliente.
    *	@param avatar 		avatar del cliente usado en el juego.
    *	@param info 		Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
    *
    */
	[RPC]
	void SetInfoClient (string idUsuario, string name, string avatar, NetworkMessageInfo info) {
		EthPlayerNetWork modifiedClient = SetInfoPlayer (info.sender, name, idUsuario, avatar);
		//set de id al cliente
		networkView.RPC ("SetIdClient", info.sender, modifiedClient.Id);
		
		//envia la lista de rooms
		SendRoomListToUser (modifiedClient);
		
		//envia a los demas clientes que un usuario se ha conectado
		networkView.RPC ("NewClient", RPCMode.All, modifiedClient.Id, modifiedClient.Name, modifiedClient.Player);
		
	}
	
	/**
    *	@brief	Método usado para unir un jugador a un sector del sevidor.
    *	
    *	Se debe tener en cuenta de que si el jugador se encuentra en otro sector, primero lo remueve del actual para luego unirlo al deseado.
    *	
    *	@param id 	Id del sector del servidor al que el jugador se desea unir.
    *	@param x 	Posición x del jugador.	
    *	@param z 	Posición y del jugador.	
    *	@param info Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
    *
    */
	[RPC]
	void joinRoom (string id, float x, float z, NetworkMessageInfo info) {
		EthPlayerNetWork player = GetPlayer (info.sender);
		player.X = x;
		player.Y = z;
		
		//si estaba en un room se remueve primero de ese
		if ( player.IdCurrentRoom != null ) {
			rooms [player.IdCurrentRoom].LeaveRoom (player);
		}

        Debug.Log(string.Format("join room: {0} user: {1}", id, player.Id));
		rooms [id].JoinUserToRoom (player);

	}
	
	/**
    *	@brief	Método usado para remover a un jugador del sector del servidor en el que se encuentra.
    *	
    *	@param id 	Id del sector del servidor del que se va a remover al jugador.
    *	@param info Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
    *
    */
	[RPC]
	void LeaveRoom (string id, NetworkMessageInfo info) {
		//main.textDebug += " leave usuario a " + id ;
		EthPlayerNetWork player = GetPlayer (info.sender);
		
		//si estaba en un room se remueve primero de ese
		if ( player.IdCurrentRoom != null ) {
			rooms [player.IdCurrentRoom].LeaveRoom (player);
		}
	}
	
	/**
    *	@brief	Método usado para enviar un mensaje a todos los usuarios que hay en un sector del servidor especificado.
    *	
    *	@param idRoom 					Id del sector del servidor a donde se envia el mensaje.
    *	@param cmd 						Comando que especifica como se mandara el mensaje.
    *	@param arrayStringSerialized 	Mensaje serializado para enviar.
    *	@param info 					Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
    *
    */
	[RPC]
	void SendMessageToUsersInRoom (string idRoom, string cmd, string arrayStringSerialized, NetworkMessageInfo info) {
		EthPlayerNetWork player = GetPlayer (info.sender);
		
		rooms [idRoom].SendToUsersInRoom (player.Id, cmd, arrayStringSerialized);
	}

	/**
	*	@brief Método usado por el jugador que desea cambiar su avatar.
	*	
	*	Este método es generado como respuesta a la solicitud del usuario de cambiar el avatar y se especificará con respecto al juego.  
	*
	*	@param arrayStringSerialized	Mensaje serializado para enviar.
	*	@param info 					Estructura de datos que contiene información recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void changeAvatar (string arrayStringSerialized, NetworkMessageInfo info) {
		EthPlayerNetWork player = GetPlayer (info.sender);

		if ( player != null ) {
			player.Avatar = arrayStringSerialized;

			networkView.RPC ("OnChangeAvatar", RPCMode.Others, player.Id, arrayStringSerialized);
		}
	}
	
	//RPC CLIENTE
	/**
	*	@brief Método usado para modificar el id del cliente o usuario.
	*	
	*	Se ingresa por parametro el nuevo id que se le desea asignar al usuario actual.   
	*
	*	@param idCliente Id del cliente.
	*	@param info Estructura de datos que contiene información recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void SetIdClient (string idCliente, NetworkMessageInfo info) {
	}
	
	/**
	*	@brief Método usado para crear un nuevo cliente o usuario.
	*
	*	@param id 		Id del nuevo jugador a crear.
	*	@param name 	Nombre del nuevo jugador.
	*	@param player 	Jugador que se desea crear.
	*	@param info 	Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void NewClient (string id, string name, NetworkPlayer player, NetworkMessageInfo info) {
	}
	
	/**
	*	@brief Método usado una vez se halla hecho la desconexión del usuario.
	*	
	*	Este método esta encargado de hacer ejecutar las instrucciones necesarias una vez se halla desconectado un jugador, como lo es por 
	*	ejemplo eliminar el jugador de la lista de conectados.  
	*
	*	@param id 	Id del nuevo jugador a eliminar.
	*	@param info Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void ClientDisconnected (string id, NetworkMessageInfo info) {
	}
	
	/**
	*	@brief Método usado para unir a este usuario a un room o sector del servidor.
	*	
	*	Para asignar al usuario al sector del servidor especificado, se especifica la posición en x y en z en donde quedara el usuario.
	*
	*	@param idRoom 	Id del sector del servidor al cual se va a unir el usuario.
	*	@param idPlayer Id del jugador que desea unirse al room o sector del servidor.
	*	@param nick 	Nombre usado por el usuario en el juego.
	*	@param avatar 	Avatar del usuario usado en el juego.
	*	@param x 		Posición x en la que va el usuario.
	*	@param z 		Posición z en la que va el usuario.
	*	@param info 	Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void UserJoinedToRoom (string idRoom, string idPlayer, string nick, string avatar, float x, float z, NetworkMessageInfo info) {
	}

	/**
	*	@brief Método usado una vez el usuario este en la lista del sector del servidor.
	*	
	*	Este método se especificará con respecto al juego.  
	*
	*	@param idRoom 	Id del sector del servidor.
	*	@param players 	Jugadores que estan en la lista del sector del servidor.
	*	@param info 	Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void UserListOfRoom (string idRoom, string players, NetworkMessageInfo info) {
	}
	
	/**
	*	@brief Método usado una vez el usuario abandona un sector del servidor.
	*	
	*	Este método es generado como respuesta a la solicitud del usuario de salir de un sector del servidor y se especificará con 
	*	respecto al juego.  
	*
	*	@param idRoom 	Id del sector del servidor.
	*	@param idPlayer Id del jugador que abandona el sector del servidor.
	*	@param info 	Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void UserLeaveRoom (string idRoom, string idPlayer, NetworkMessageInfo info) {
	}
	
	/**
	*	@brief Método usado una vez un jugador mande un mensaje del sector del servidor.
	*	
	*	Este método es generado como respuesta a la solicitud del usuario de enviar un mensaje al sector del servidor en el que se 
	*	encuentra y se especificará con respecto al juego.  
	*
	*	@param idRoom 	Id del sector del servidor.
	*	@param cmd 		Comando que especifica como se mandara el mensaje.
	*	@param idPlayer Id del jugador que abandona el sector del servidor.
	*	@param msg 		Mensaje que desea ser enviado.
	*	@param info 	Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void OnMessageFromRoom (string idRoom, string cmd, string idPlayer, string msg, NetworkMessageInfo info) {
	}

	/**
	*	@brief Método usado una vez un jugador cambie su avatar.
	*	
	*	Este método es generado como respuesta a la solicitud del usuario de cambiar el avatar y se especificará con respecto al juego.  
	*
	*	@param idPlayer Id del jugador que desea cambiar el avatar.
	*	@param avatar 	Avatar del personaje a ser cambiado.
	*	@param info 	Estructura de datos que contiene información recibida del servidor con respecto a la acción ejecutada.
	*/
	[RPC]
	void onChangeAvatar (string idPlayer, string avatar, NetworkMessageInfo info) {
	}
	//FIN METODOS RPC
	
	// Delegate
	/**
	*	@brief Método ejecutado cuando el sever se ha inicializado, que luego sera modificado con 
	*	respecto a como se desee utilizar manteniendo su estructura.
	*	
	*/
	public delegate void OnServerInitializedEvent ();

	/**
	*	@brief Método ejecutado cuando un jugador se ha conectado al sevidor, que luego sera modificado con 
	*	respecto a como se desee utilizar manteniendo su estructura.
	*	
	*	@param player Jugador que se ha conectado
	*/
	public delegate void OnPlayerConnectedEvent (EthPlayerNetWork player);

	/**
	*	@brief Método ejecutado cuando un jugador se ha desconectado al sevidor, que luego sera modificado con 
	*	respecto a como se desee utilizar manteniendo su estructura.
	*	
	*	@param player Jugador que se ha desconectado
	*/
	public delegate void OnPlayerDisconnectedEvent (EthPlayerNetWork player);

	/**
	*	@brief Método ejecutado cuando un componente presenta un evento, que luego sera modificado con 
	*	respecto a como se desee utilizar manteniendo su estructura.
	*	
	*/
	public delegate void OnComponentStartedEvent ();
		
}