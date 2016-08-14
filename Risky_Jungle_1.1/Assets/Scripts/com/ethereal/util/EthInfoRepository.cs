using UnityEngine;
using System.Collections.Generic; 

/** 
*	@author    
* 	@version   1.0 
* 	@date      Marzo 18 del 2015
* 
*	@class 	EthInfoRepository
*   @brief 	
*	
*/
public class EthInfoRepository 
{	
	/**
	*	@brief Patrón Singleton para mantener la misma instancia de EthInfoRepository en todo el juego.
	*/
	private static EthInfoRepository instance;

	/*
	*	@brief Diccionario que contendrá la información del repositorio.
	*/
	private Dictionary<string,object> info;

	/*
	*	@brief Constructor de la clase EthInfoRepository en donde se asigna la informacion que contentra el repositorio 
	*	como un nuevo diccionario
	*/
	private EthInfoRepository()
	{	
		info = new Dictionary<string,object>();
	}
	
	/**
	*	@brief Método para definir el EthInfoRepository a usar.
	*	
	*	Este método es el encargado de que cuando no haya alguna instancia de EthInfoRepository cree una nueva, 
	*	de lo contrario si ya hay un EthInfoRepository en el juego se seguira trabajando con la misma.
	*
	*	@return Instancia de EthInfoRepository. 
	*/
	public static EthInfoRepository getInstance()
	{
		if ( EthInfoRepository.instance == null )
		{
			EthInfoRepository.instance = new EthInfoRepository();	
		}

		return EthInfoRepository.instance;
	}

    /*
	*	@brief Método para cambiar la información del nombre del objeto.
	*/
	public void setR( string name, object obj)
	{
		info[name] = obj;
	}

    /*
	*	@brief Método para obtener la información del nombre del objeto.
	*/
	public object getR(string name)
	{
		return info[name];
	}

    /*
    *	@brief Método para cambiar la información del nombre del objeto.
    */
	public static void setReg( string name, object obj)
	{
		EthInfoRepository.getInstance().setR(name, obj);
	}

    /*
    *	@brief Método para obtener la información del nombre del objeto.
    */
	public static object getReg(string name)
	{
		return EthInfoRepository.getInstance().getR(name);
	}
	
}

