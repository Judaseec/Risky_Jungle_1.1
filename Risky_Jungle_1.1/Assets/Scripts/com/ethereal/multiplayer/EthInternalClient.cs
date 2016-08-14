using UnityEngine;
using System.Text;
using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;


/** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Julio 9 2014
* 
*	@class 	EthInternalCliente
*   @brief 	Esta clase se encarga de manejar la informacion de un cliente.
*
*	Esta clase es utilizada para almenar la informacion de la cuenta de un cliente en la aplicacion, como su nombre de cliente, el id del 
*	cliente, es estado de conecccion, entre otros.
*
*/
public class EthInternalClient : MonoBehaviour
{
    /**
    *	@brief Es la manera de conexion en los juegos multijugador.
    *	Con este se peude definir exactamente lo que se va a sincronizar en la red y como debe hacerse.
    */
    private NetworkView _networkView;
    public NetworkView NetworkView
    {
        get { return _networkView; }
        set { _networkView = value; }
    }
    /**
    *	@brief Nombre del cliente.
    */
    private static string _clientName;

    /**
    *	@brief Id del cliente.
    */
    private static string _clientId;

    /**
    *	@brief Avatar del usuario usado en el juego.
    */
    private static string _avatar;

    /**
    *	@brief Estado de conección del cliente.
    */
    private static bool _isOnline;

    /**
    *	@brief Sector en el que se encuentra actualmente el cliente en el server.
    */
    private static string _currentRoom;

    /**
    *	@brief Lista de usuarios conectados.
    */
    private ArrayList _onlineUsersList;


    // The events

    /**
    *	@brief	Evento llamado cuando el jugador se ha conectado al servidor, que luego sera modificado.
    */
    public event OnConnectedToServerEvent OnConnectedToServerCB;

    /**
    *	@brief	Evento llamado cuando el jugador se ha unido al servidor, que luego sera modificado.
    */
    public event OnPlayerJoined OnPlayerJoinedCB;

    /**
    *	@brief	Evento llamado cuando el jugador ha salido del servidor, que luego sera modificado.
    */
    public event OnPlayerOut OnPlayerOutCB;

    /**
    *	@brief	Evento llamado cuando el jugador se haya desconectado del servidor, que luego sera modificado.
    */
    public event OnDisconnectedFromServerEvent OnDisconnectedFromServerEventCB;

    /**
    *	@brief	Evento llamado cuando el jugador se haya unido a un sector del servidor, que luego sera modificado.
    */
    public event OnUserJoinedToRoomEvent OnUserJoinedToRoomCB;

    /**
    *	@brief	Evento llamado cuando el jugador se haya unido a un sector del servidor, que luego sera modificado.
    */
    public event OnUserListOfRoomEvent OnUserListOfRoomCB;

    /**
    *	@brief	Evento llamado cuando el jugador haya abandonado un sector del servidor, que luego sera modificado.
    */
    public event OnUserLeaveRoomEvent OnUserLeaveRoomCB;

    /**
    *	@brief	Evento llamado cuando el jugador haya enviado un mensaje al sector del servidor en el que se encuentra, que luego sera modificado.
    */
    public event OnMsgFromRoomEvent OnMsgFromRoomCB;

    /**
    *	@brief	Evento llamado cuando el jugador desee cambiar el avatar, que luego sera modificado.
    */
    public event OnChangeAvatarEvent OnChangeAvatarCB;

    // Use this for initialization
    /**
    *	@brief Método usado para iniciar la conexión.
    *	
    *	Este método esta encargado de configurar el networkView para iniciar la conexion al servidor.
    *	
    */
    public void Start()
    {
        _networkView = (NetworkView)this.gameObject.AddComponent<NetworkView>();
        _networkView.stateSynchronization = NetworkStateSynchronization.Off;
        _onlineUsersList = new ArrayList();
    }

    /**
    *	@brief Método usado para actuaizar la conexión.
    *	
    */
    public void Update()
    {

    }

    // Client methods
    /**
    *	@brief Método usado para unirse al servidor con una ip y un puerto especifico.
    *	
    *	@param ip 		Dirección ip a la que se desea conectar.
    *	@param port 	Puerto en el que se conectara el usuario.
    *	@param idUser 	Id del usuario o del cliente.
    *	@param nick 	Nombre usado por el usuario en el juego.
    *	@param temporalAvatar 	Avatar del usuario usado en el juego.
    */
    public void JoinServer(string ip, int port, string idUser, string nick, string temporalAvatar)
    {
        ClientName = nick;
        ClientId = idUser;
        _avatar = temporalAvatar;
        Network.Connect(ip, port);
    }

    /**
    *	@brief Método usado para la desconexión de un usuario.
    *	
    *	Se desconecta al usuario cerrando todas las conexiones que posea este usuario y apagando la interface de red.
    *	
    */
    public void DisconnectServer()
    {
        _isOnline = false;
        Network.Disconnect();
    }

    /**
    *	@brief Método usado para enviar una solicitud de ingreso de un usuario a un sector del server.
    *	
    *	Se envia la solicitud con la información requerida para unir el usuario a un sector del server.
    *	
    *	@param id 	Id del sector del servidor al que se va a unir el jugador.
    *	@param x 	Posición x en la que va el usuario.
    *	@param z 	Posición z en la que va el usuario.
    */
    public void JoinUserToRoom(string id, float x, float z)
    {
        _networkView.RPC("JoinRoom", RPCMode.Server, id, x, z);
    }

    /**
    *	@brief Método usado para enviar una solicitud al servidor para que un usuario salga del sector en el que se encuentra.
    *	
    *	Se envia la solicitud con la información requerida para permitir a un usuario salir de un sector del server en el que se 
    *	encuentre actualmente.
    *	
    */
    public void LeaveUserRoom()
    {
        _networkView.RPC("LeaveRoom", RPCMode.Server, "");
    }

    /**
    *	@brief Método usado para enviar una solicitud de envio de mensajes por parte del usuario al sector en el qeu se encuentra actualmente.
    *	
    *	Ésta solicitud pemitiar al usuario enviar un mensaje a todos los demas usuarios conectados en el sector del servidor actual de 
    *	dicho usuario. 
    *	
    *	@param cmd 					Comando que especifica como se mandara el mensaje.
    *	@param messageSerialized	Mensaje serializado para enviar.
    */
    public void SendMessageToRoom(string cmd, string messageSerialized)
    {
        if (!_isOnline)
        {
            return;
        }
        _networkView.RPC("SendMessageToUsersInRoom", RPCMode.Server, _currentRoom, cmd, messageSerialized);
    }

    /**
    *	@brief Método usado para enviar una solicitud de cambio de avatar por parte del usuario.
    *	
    *	@param messageSerialized Mensaje serializado sobre el cambio del avatar.
    */
    public void SendChangeAvatar(string messageSerialized)
    {
        if (!_isOnline)
        {
            return;
        }
        _networkView.RPC("ChangeAvatar", RPCMode.Server, messageSerialized);
    }

    /**
    *	@brief Método usado para enviar una solicitud de conexión del cliente.
    *	
    *	Se envía una solicitud conexión al servidor por parte del usuario que desee ingresar al juego.  
    *
    */
    public void OnConnectedToServer()
    {
        _isOnline = true;
        _networkView.RPC("SetInfoClient", RPCMode.Server, _clientId, ClientName, _avatar);
    }

    /**
    *	@brief Método usado para identificar la forma de desconexión del cliente.
    *	
    *	Las formas de desconexión por parte del usuario pueden se que él desee salir o desconectarse
    *	del juego o posiblemente haya perdido la conexión con el servidor. 
    *
    *	@param info Información sobre la forma de desconexión (perdida de conexión o una desconexión satisfactoria) 
    *	del usuario.
    */
    public void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        bool cause;
        if (info == NetworkDisconnection.LostConnection)
        {
            cause = false;
            Debug.Log("Lost connection to the server");
        }
        else
        {
            cause = true;
            Debug.Log("Successfully diconnected from the server");
        }

        if (OnDisconnectedFromServerEventCB != null)
        {
            OnDisconnectedFromServerEventCB(cause);
        }

    }

    //METODOS RPC
    /**
    *	@brief Método usado para modificar el id del cliente o usuario.
    *	
    *	Se ingresa por parametro el nuevo id que se le desea asignar al usuario actual.   
    *
    *	@param idCliente Id del cliente.
    *	@param info Estructura de datos que contiene información recibida del servidor con respecto a la acción ejecutada.
    */
    [RPC]
    public void SetIdClient(string idCliente, NetworkMessageInfo info)
    {
        this.ClientId = idCliente;

        if (OnConnectedToServerCB != null)
        {
            OnConnectedToServerCB();
        }
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
    public void NewClient(string id, string name, NetworkPlayer player, NetworkMessageInfo info)
    {
        EthPlayerNetWork newPlayer = new EthPlayerNetWork();
        newPlayer.Id = id;
        newPlayer.Name = name;
        newPlayer.Player = player;

        this.AddOnlineUser(newPlayer);

        if (OnPlayerJoinedCB != null)
        {
            OnPlayerJoinedCB(newPlayer);
        }
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
    public void ClientDisconnected(string id, NetworkMessageInfo info)
    {
        EthPlayerNetWork removed = this.RemovePlayer(id);

        if (OnPlayerOutCB != null)
        {
            OnPlayerOutCB(removed);
        }
    }

    /**
    *	@brief Método usado para unir a este usuario a un room o sector del servidor.
    *	
    *	Para asignar al usuario al sector del servidor especificado, se especifica la posición en x y en z en donde quedara el usuario.
    *
    *	@param idRoom 	Id del sector del servidor al cual se va a unir el usuario.
    *	@param idPlayer Id del jugador que desea unirse al room o sector del servidor.
    *	@param nick 	Nombre usado por el usuario en el juego.
    *	@param avatar 	Avatar del usuario usado en el juego
    *	@param x 		Posición x en la que va el usuario.
    *	@param z 		Posición z en la que va el usuario.
    *	@param info 	Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
    */
    [RPC]
    // llega un array de string, primera posicion el id del room, segunda el id del usuario
    public void UserJoinedToRoom(string idRoom, string idPlayer, string nick, string avatar, float x, float z, NetworkMessageInfo info)
    {
        // string[] list = (string[])EthSerializador.deserializar( parametros);

        if (_clientId == idPlayer)
        {
            _currentRoom = idRoom;
        }
        //callbacks
        if (OnUserJoinedToRoomCB != null)
        {
            OnUserJoinedToRoomCB(idRoom, idPlayer, nick, avatar, x, z);
        }
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
    public void UserListOfRoom(string idRoom, string players, NetworkMessageInfo info)
    {
        if (OnUserListOfRoomCB != null)
        {
            OnUserListOfRoomCB(idRoom, players);
        }
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
    public void UserLeaveRoom(string idRoom, string idPlayer, NetworkMessageInfo info)
    {
        if (OnUserLeaveRoomCB != null)
        {
            OnUserLeaveRoomCB(idRoom, idPlayer);
        }
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
    public void OnMessageFromRoom(string idRoom, string cmd, string idPlayer, string msg, NetworkMessageInfo info)
    {
        // Debug.Log("mensaje enviado: " + idRoom + " del player: " + idPlayer + " cmd " + cmd + " msg: " + msg);
        if (OnMsgFromRoomCB != null)
        {
            OnMsgFromRoomCB(idRoom, cmd, idPlayer, msg);
        }
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
    public void OnChangeAvatar(string idPlayer, string avatar, NetworkMessageInfo info)
    {
        if (OnChangeAvatarCB != null)
        {
            OnChangeAvatarCB(idPlayer, avatar);
        }
    }

    //RPC SERVER
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
    public void SetInfoClient(string idUsuario, string name, string avatar, NetworkMessageInfo info)
    {
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
    public void JoinRoom(string id, float x, float z, NetworkMessageInfo info)
    {
    }

    /**
    *	@brief	Método usado para remover a un jugador del sector del servidor en el que se encuentra.
    *	
    *	@param id 	Id del sector del servidor del que se va a remover al jugador.
    *	@param info Estructura de datos que contiene informacion recibida del servidor con respecto a la acción ejecutada.
    *
    */
    [RPC]
    public void LeaveRoom(string id, NetworkMessageInfo info)
    {
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
    public void SendMessageToUsersInRoom(string idRoom, string cmd, string arrayStringSerialized, NetworkMessageInfo info)
    {
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
    public void ChangeAvatar(string arrayStringSerialized, NetworkMessageInfo info)
    {
    }
    //FIN METODOS RPC

    /**
    *	@brief Método usado para agregar los usuarios conectados.
    *	
    *	Este método esta encargado de agregar los jugadores a la lista de _onlineUserList(lista de usuarios conectados).
    *
    *	@param player Jugador a ser agregado a la lista.
    */
    public void AddOnlineUser(EthPlayerNetWork player)
    {
        _onlineUsersList.Add(player);
    }

    /**
    *	@brief Método usado para obtener la informacion de un player deseado.
    *
    *	@param id Id del player que se desea conocer.
    */
    public EthPlayerNetWork GetPlayer(string id)
    {
        foreach (EthPlayerNetWork input in _onlineUsersList)
        {
            if (input.Id == id)
            {
                return input;
            }
        }

        return null;
    }

    /**
    *	@brief Método usado para remover a un jugador de la lista de usuarios conectados.
    *	
    *	Se usa principalmente cuando el usuario se ha desconectado.
    *
    *	@param id Id del player que se desea remover.
    */
    public EthPlayerNetWork RemovePlayer(string player)
    {
        EthPlayerNetWork playerRemoved = GetPlayer(player);
        _onlineUsersList.Remove(playerRemoved);
        return playerRemoved;
    }

    /**
    *	@brief	Getter y setter del atributo isOnline referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de isOnline.
    */
    public bool IsOnline
    {
        get
        {
            return _isOnline;
        }
        set
        {
            _isOnline = value;
        }
    }

    /**
    *	@brief	Getter y setter del atributo clientId referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de clientId.
    */
    public string ClientId
    {
        get
        {
            return _clientId;
        }
        set
        {
            _clientId = value;
        }
    }

    /**
    *	@brief	Getter y setter del atributo clientName referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de clientName.
    */
    public string ClientName
    {
        get
        {
            return _clientName;
        }
        set
        {
            _clientName = value;
        }
    }

    /**
    *	@brief	Getter y setter del atributo currentRoom referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de currentRoom.
    */
    public string CurrentRoom
    {
        get
        {
            return _currentRoom;
        }
        set
        {
            _currentRoom = value;
        }
    }

    /**
    *	@brief	Metodo usado para cuando se vaya a destruir algo.
    *	
    */
    public void OnDestroy()
    {

    }

    // Delegate

    /**
    *	@brief Método ejecutado cuando el jugador se haya unido al servidor que luego sera modificado con respecto a como se desee 
    *	utilizar manteniendo su estructura. 
    *
    *	@param player Jugador que se ha unido al servidor.
    */
    public delegate void OnPlayerJoined(EthPlayerNetWork player);

    /**
    *	@brief Método ejecutado cuando el jugador haya salido del servidor que luego sera modificado con respecto a como se desee 
    *	utilizar manteniendo su estructura. 
    *
    *	@param player Jugador que ha salido del servidor.
    */
    public delegate void OnPlayerOut(EthPlayerNetWork player);

    /**
    *	@brief Método ejecutado cuando el jugador se haya conectado al servidor, que luego sera modificado con respecto a como se desee 
    *	utilizar manteniendo su estructura. 
    */
    public delegate void OnConnectedToServerEvent();

    /**
    *	@brief Método ejecutado cuando el jugador se haya desconectado del servidor, que luego sera modificado con respecto a como se desee 
    *	utilizar manteniendo su estructura. 
    *
    *	@param success Variable que indica que el proceso fue realizado correctamente
    */
    public delegate void OnDisconnectedFromServerEvent(bool success);

    /**
    *	@brief Método ejecutado cuando el jugador se haya unido a un sector del servidor, que luego sera modificado con respecto a como 
    *	se desee utilizar manteniendo su estructura. 
    *
    *	@param idRoom 	Id del sector del servidor al cual se va a unir el usuario.
    *	@param idUser	Id del usuario que desea unirse al room o sector del servidor.
    *	@param nick 	Nombre usado por el usuario en el juego.
    *	@param avatar 	Avatar del usuario usado en el juego
    *	@param x 		Posición x en la que va el usuario.
    *	@param z 		Posición z en la que va el usuario.		
    */
    public delegate void OnUserJoinedToRoomEvent(string idRoom, string idUser, string nick, string avatar, float x, float z);

    /**
    *	@brief Método ejecutado cuando el jugador haya salido de un sector del servidor, que luego sera modificado con respecto a como 
    *	se desee utilizar manteniendo su estructura. 
    *	
    *	@param idRoom	Id del sector del sevidor del que ha salido el jugador.
    *	@param idUser	Id del usuario que sale del sector del servidor.
    */
    public delegate void OnUserLeaveRoomEvent(string idRoom, string idUser);

    /**
    *	@brief Método ejecutado cuando se tenga la lista de usuarios coenctados del sector del servidor, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    *	@param idRoom	Id del sector actual.
    *	@param players	Jugadores pertenecientes al sector del servidor que se ha especificado.
    */
    public delegate void OnUserListOfRoomEvent(string idRoom, string players);

    /**
    *	@brief Método ejecutado cuando se tenga un mensaje para un sector del servidor, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    *	@param idRoom	Id del sector actual.
    *	@param cmd		Comando que especifica como se mandara el mensaje.
    *	@param idPlayer Jugador que manda el mensaje.
    *	@param msg 		Mensaje enviado. 
    */
    public delegate void OnMsgFromRoomEvent(string idRoom, string cmd, string idPlayer, string msg);

    /**
    *	@brief Método ejecutado cuando un jugador desea cambiar su avatar, que luego sera modificado con 
    *	respecto a como se desee utilizar manteniendo su estructura.
    *	
    *	@param idPlayer	Id del jugador que desea cambiar el avatar.
    *	@param avatar	Avatar del personaje a ser cambiado.
    */
    public delegate void OnChangeAvatarEvent(string idPlayer, string avatar);

}
