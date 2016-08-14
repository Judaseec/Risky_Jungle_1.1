using UnityEngine;

/** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Agosto 30 2014
* 
*	@class 	EthPersistent
*   @brief 	Esta clase se encarga de la persistencia de los datos de las preferencias del jugador entre sesiones de juegos.
*
*/
public class EthPersistent { 
  
	/**
	*	@brief Patron Singleton para mantener la misma instancia de EthPersistent en todo el juego.
	*/
	private static EthPersistent _instance;

	/**
	*	@brief Método encargado de crear un EthPersistent.
	*	
	*/
	private EthPersistent () 
	{

	}

	/**
	*	@brief Metodo para definir el EthPersistent a usar.
	*	
	*	Este metodo es el encargado de que cuando no haya alguna instancia de EthPersistent cree una nueva, 
	*	de lo contrario si ya hay un EthPersistent en el juego se seguira trabajando con el mismo.
	*
	*	@return Instancia de EthPersistent. 
	*/
	public static EthPersistent GetInstance () 
	{
		if ( EthPersistent._instance == null ) 
		{
			EthPersistent._instance = new EthPersistent ();	
		}

		return EthPersistent._instance;
	}

	/**
	*	@brief Establece un valor String determinado identificado por la clave.
	*
	*	Establece valores en las preferencias del jugador.
	*	
	*	@param	key 	llave identificadora.
	*	@param	value 	valor a ser asignado de acuerdo a la llave.
	*/
	public void SaveString (string key, string value) 
	{
		PlayerPrefs.SetString (key, value);
	}

	/**
	*	@brief Establece un valor Float determinado identificado por la clave.
	*
	*	Establece valores en las preferencias del jugador.
	*	
	*	@param	key 	llave identificadora.
	*	@param	value 	valor a ser asignado de acuerdo a la llave.
	*/
	public void SaveFloat (string key, float value) 
	{
		PlayerPrefs.SetFloat (key, value);
	}

	/**
	*	@brief Establece un valor Int determinado identificado por la clave.
	*
	*	Establece valores en las preferencias del jugador.
	*	
	*	@param	key 	llave identificadora.
	*	@param	value 	valor a ser asignado de acuerdo a la llave.
	*/
	public void SaveInt (string key, int value) 
	{
		PlayerPrefs.SetInt (key, value);
	}

	/**
	*	@brief Obtiene el valor correspondiente (String) a la llave en el archivo de preferencia si existe.
	*
	*	Si no existe dicho valor, se retornara el valor por defecto.
	*	
	*	@param	key 	llave identificadora.
	*
	*	@return Valor corrspondiente a la llave de las preferencias del jugador.
	*/
	public string GetString( string key, string defaultValue = "" )
	{
		return PlayerPrefs.GetString( key, defaultValue );
	}

	/**
	*	@brief Obtiene el valor correspondiente (Float) a la llave en el archivo de preferencia si existe.
	*
	*	Si no existe dicho valor, se retornara el valor por defecto.
	*	
	*	@param	key 	llave identificadora.
	*
	*	@return Valor corrspondiente a la llave de las preferencias del jugador.
	*/
	public float GetFloat (string key, float defaultValue = 0.0F) 
	{
		return PlayerPrefs.GetFloat (key, defaultValue);
	}

	/**
	*	@brief Obtiene el valor correspondiente (Int) a la llave en el archivo de preferencia si existe.
	*
	*	Si no existe dicho valor, se retornara el valor por defecto.
	*	
	*	@param	key 	llave identificadora.
	*
	*	@return Valor correspondiente a la llave de las preferencias del jugador.
	*/
	public int GetInt (string key, int defaultValue = 0) 
	{
		return PlayerPrefs.GetInt (key, defaultValue);
	}

	/**
	*	@brief Elimina la llave y su valor correspondiente de las preferencias del jugador.
	*	
	*	@param	key llave identificadora.
	*/
	public void DeleteKey (string key) 
	{
		PlayerPrefs.DeleteKey (key);
	}
	
	/**
	*	@brief Almacena en disco las preferencias del jugador.
	*	
	*/
	public void SaveToDisk()
	{
		PlayerPrefs.Save();
	}
}
