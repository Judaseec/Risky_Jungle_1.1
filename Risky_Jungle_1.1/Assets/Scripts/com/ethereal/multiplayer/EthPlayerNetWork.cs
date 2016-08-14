using UnityEngine;

/** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Julio 12 2014
* 
*	@class 	EthPlayerNetWork
*   @brief 	Clase encargada del usuario conectado en la red
*	
*	Esta clase se encarga de manejar la información referente al cliente o usuario que se encuentra conectado en el juego.
*/
public class EthPlayerNetWork {

	/**
	*	@brief Es una estructura de datos con la cual se puede ubicar los jugadores en la red.
	*/
	private NetworkPlayer player;

	/**
	*	@brief Nombre del jugador.
	*/
	private string name;

	/**
	*	@brief Id del jugador.
	*/
	private string id;

	/**
	*	@brief Avatar del usuario usado en el juego.
	*/
	private string avatar;

	/**
	*	@brief Id del room o sector del servidor en el que se encuentra actualmente el jugador.
	*/
	private string idCurrentRoom;
	
	/**
	*	@brief coordenada en x
	*/
	private float x;
	
	/**
	*	@brief coordenada en y
	*/
	private float y;
	
	/**
	*	@brief Método usado para crear un EthPlayerNetWork.
	*	
	*	Este método esta encargado de crear un EthPlayerNetWork sin la necesidad de requerir parámetros.
	*/
	public EthPlayerNetWork () 
	{
	}
	
	/**
	*	@brief Método usado para crear un EthPlayerNetWork.
	*	
	*	Para crear este EthPlayerNetWork se requiere un jugador (playerTemp), un nombre del jugador (nameTemp) y el
	*	id del jugador (idTemp).
	*
	*	@param playerTemp 	Jugador que esta conectado en el juego.
	*	@param nameTemp		Nombre del jugador.
	*	@param idTemp 		Id del jugador.
	*/
	public EthPlayerNetWork (NetworkPlayer playerTemp, string nameTemp, string idTemp) {
		this.player = playerTemp;
		this.name = nameTemp;
		this.id = idTemp;
	}
	
	/**
    *	@brief	Getter y setter del atributo id referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de id.
    */
	public string Id {
		get {
			return this.id;
		}
		set {
			id = value;
		}
	}

	/**
    *	@brief	Getter y setter del atributo name referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de name.
	*/
	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	/**
	*	@brief	Getter y setter del atributo player referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de player.
    */
	public NetworkPlayer Player {
		get {
			return this.player;
		}
		set {
			player = value;
		}
	}

	/**
    *	@brief	Getter y setter del atributo idCurrentRoom referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de idCurrentRoom.
    */
	public string IdCurrentRoom {
		get {
			return this.idCurrentRoom;
		}
		set {
			idCurrentRoom = value;
		}
	}

	/**
    *	@brief	Getter y setter del atributo avatar referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de avatar.
    */
	public string Avatar {
		get {
			return this.avatar;
		}
		set {
			avatar = value;
		}
	}
	
	/**
    *	@brief	Getter y setter del atributo x referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de x.
    */
	public float X {
		get {
			return this.x;
		}
		set {
			x = value;
		}
	}

	/**
    *	@brief	Getter y setter del atributo y referenciados con respecto a esta clase.
    *	
    *	Método encargado de obtener y modificar la variable.   
    *
    *	@return Valor de y.
    */
	public float Y {
		get {
			return this.y;
		}
		set {
			y = value;
		}
	}

	/**
    *	@brief	Metodo usado para dar una estructura a la clase cuando se desee serializar
    *	
    *
    *	@return String con la estructura de la clase.
    */
	public string Serialize () {
		return id + "," + name + "," + avatar;
	}
}
