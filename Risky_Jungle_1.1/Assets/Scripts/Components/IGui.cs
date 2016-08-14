using System;

/** 
*   @author    EtherealGF <www.etherealgf.com> 
*   @version   1.0 
*   @date      Octubre 17 2015
* 
*   @class  IGui
*   @brief  Clase para establecer los métodos InitScreen y BackButtonPressed para las clases GUI hijas.
*/
public interface IGui{

	/**
  	*   @brief Método para iniciar la pantalla.
 	*/
	void InitScreen (  );

	/**
  	*   @brief Método para definir las acciones al presionar el botón de atras.
 	*/
	void BackButtonPressed( );
}