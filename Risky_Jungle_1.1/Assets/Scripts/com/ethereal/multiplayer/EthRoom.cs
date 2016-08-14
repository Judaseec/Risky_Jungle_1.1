using UnityEngine;
using System.Collections;

/** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Julio 13 2014
* 
*	@class 	EthRoom
*   @brief 	Sector del servidor en el que se almacenan cierta cantidad de jugadores conectados.
*
*	Se sectoriza el servidor en rooms para que no haya muchas personas en el mismo room y asi evitar falencias de conexión.
*
*/
public class EthRoom
{
    /**
    *	@brief Id que identifica el sector dele servidor.
    */
    private string _idRoom;

    /**
    *	@brief Lista de usuarios conectados en este sector.
    */
    private ArrayList _usersList;

    /**
    *	@brief Es la manera de conexion en los juegos multijugador.
    *	Con este se peude definir exactamente lo que se va a sincronizar en la red y como debe hacerse.
    */
    private NetworkView networkView;

    /**
    *	@brief Método usado para crear un EthRoom.
    *	
    *	Para crear un EthRoom es necesario un NetworkView para que posteriomente el sector pueda llevar a cabo la conexión de los 
    *	jugadores satisfactoriamente.
    */
    public EthRoom(NetworkView nv)
    {
        _usersList = new ArrayList();
        this.networkView = nv;
    }

    /**
    *	@brief Método usado para crear un EthRoom.
    *	
    *	Para crear un EthRoom es necesario un id que lo identifique y un NetworkView para que posteriomente el sector pueda llevar 
    *	a cabo la conexión de los jugadores satisfactoriamente.
    */
    public EthRoom(string id, NetworkView nv)
    {
        this._idRoom = id;
        this.networkView = nv;
        _usersList = new ArrayList();
    }

    /**
    *	@brief Método usado para obtener un jugador de la lista del sector por medio de su id.
    *
    *	@param id Id del jugador que se desea obtener.
    *
    *	@return Jugador deseado si ha sido encontrado, de lo contrario null.
    */
    public EthPlayerNetWork GetUserById(string id)
    {
        foreach (EthPlayerNetWork input in _usersList)
        {
            if (input.Id == id)
            {
                return input;
            }
        }

        return null;
    }

    /**
    *	@brief Método usado para obtener un jugador de la lista del sector por medio de su nombre.
    *
    *	@param id Id del jugador que se desea obtener.
    *
    *	@return Jugador deseado si ha sido encontrado, de lo contrario null.
    */
    public EthPlayerNetWork GetUserByName(string name)
    {
        foreach (EthPlayerNetWork input in _usersList)
        {
            if (input.Name == name)
            {
                return input;
            }
        }

        return null;
    }

    /**
    *	@brief Método usado para unir un jugador a este sector del servidor.
    *
    *	Se agrega al jugador a la lista de usuarios que posee este sector, y a éste jugador se le notifica los jugadores conectados 
    *	actualmente en el sector.
    *
    *	@param player Jugador que se unirá al sector.
    *
    *	@return true al unir al jugador.
    */
    public bool JoinUserToRoom(EthPlayerNetWork player)
    {
        _usersList.Add(player);
        player.IdCurrentRoom = this._idRoom;

        string users = "";

        foreach (EthPlayerNetWork input in _usersList)
        {
            networkView.RPC("UserJoinedToRoom", input.Player, this._idRoom, player.Id, player.Name, player.Avatar);

            users += input.Serialize() + "|";
        }

        networkView.RPC("UserListOfRoom", player.Player, this._idRoom, users);

        return true;
    }

    /**
    *	@brief Método usado para unir un jugador a este sector del servidor.
    *
    *	Se agrega al jugador a la lista de usuarios que posee este sector, y a éste jugador se le notifica los jugadores conectados 
    *	actualmente en el sector.
    *
    *	@param idPlayer 	Jugador que se unirá al sector.
    *	@param command 		Comando que especifica como se mandara el mensaje.
    *	@param parameter 	Información que depende del comando, como por ejemplo un mensaje si el comando es chat.
    */
    public void SendToUsersInRoom(string idPlayer, string command, string parameter)
    {
        foreach (EthPlayerNetWork input in _usersList)
        {
            networkView.RPC("OnMessageFromRoom", input.Player, this._idRoom, command, idPlayer, parameter);
        }
    }

    /**
    *	@brief Método usado para remover a un jugador del sector del servidor.
    *
    *	Se remueve el jugador del sector del servidor en el que se encuentra actualmente y se manda una notificación al servidor de ello.
    *
    *	@param player  Jugador que será removido del sector del servidor.
    */
    public bool LeaveRoom(EthPlayerNetWork player)
    {
        _usersList.Remove(player);
        player.IdCurrentRoom = null;

        foreach (EthPlayerNetWork input in _usersList)
        {
            networkView.RPC("UserLeaveRoom", input.Player, this._idRoom, player.Id);
        }

        return true;
    }

    /**
    *	@brief Función usada para obtener la lista de usuarios, en el server los usuarios se obtienen también con el campo player seteado.
    *
    *	@return usuarios que se encuentran en este sector.
    */
    public ArrayList GetUsers()
    {
        return _usersList;
    }

    /**
    *	@brief	Getter y setter del atributo idRoom referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de idRoom.
    */
    public string IdRoom
    {
        get
        {
            return this._idRoom;
        }
        set
        {
            _idRoom = value;
        }
    }
}