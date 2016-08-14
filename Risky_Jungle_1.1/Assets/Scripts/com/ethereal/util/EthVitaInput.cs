using UnityEngine;

using com.ethereal.display.components;
using Assets.Scripts.com.ethereal.display;
using Assets.Scripts.com.ethereal.display.components;

/** 
*	@author    EtherealGF <www.etherealgf.com> 
* 	@version   1.0 
* 	@date      Marzo 20 2015
* 
*	@class 	EthVitaInput
*   @brief 	Esta clase esta encargada de el reconocimiento de los controles de un vita.
*
*/
public class EthVitaInput
{   

	/*
	*	@brief Instancia del EthGUIScreen en esta clase para inicializar el objeto.
	*/
	private static EthGUIScreen screen;

	/*
	*	@brief Constante de tiempo de entrada de las teclas.
	*/
	public static float VITA_INPUT_TIME = 0.8f; 

	/*
	*	@brief Plataforma en la que se ejecuta el vita
	*/
	public static RuntimePlatform vitaRuntime = RuntimePlatform.WindowsPlayer;//TODO en vita dejar este RuntimePlatform.PSP2

	/*
	*	@brief Array o conjunto de teclas posibles a usar en el vita.
	*/
	public static float[] KEY_ARRAY = new float[14];

	/*
	*	@brief Array o conjunto de teclas soltadas del vita.
	*/
	public static bool[] KEY_RELEASE = new bool[14];

	/*
	*	@brief Array o conjunto de teclas presionadas del vita.
	*/
	public static bool[] KEY_PRESSED = new bool[14];

	/*
	*	@brief Array o conjunto de teclas usadas del vita.
	*/
	public static bool[] KEY_USED = new bool[14];
		
	/*
	*	@brief Tecla X la cual su valor para ser identificada sera el 0.
	*/
	public const int KEY_X = 0;

	/*
	*	@brief Tecla Circulo ○ la cual su valor para ser identificada sera el 1.
	*/
	public const int KEY_O = 1;

	/*
	*	@brief Tecla Cuadro □ la cual su valor para ser identificada sera el 2.
	*/
	public const int KEY_SQUARE = 2;

	/*
	*	@brief Tecla triángulo △ la cual su valor para ser identificada sera el 3.
	*/
	public const int KEY_TRIANGLE = 3;

	/*
	*	@brief Tecla L la cual su valor para ser identificada sera el 4.
	*/
	public const int KEY_L = 4;

	/*
	*	@brief Tecla R la cual su valor para ser identificada sera el 5.
	*/
	public const int KEY_R = 5;

	/*
	*	@brief Tecla Select la cual su valor para ser identificada sera el 6.
	*/
	public const int KEY_SELECT = 6;

	/*
	*	@brief Tecla Start la cual su valor para ser identificada sera el 7.
	*/
	public const int KEY_START = 7;

	/*
	*	@brief Tecla Arriba la cual su valor para ser identificada sera el 8.
	*/
	public const int KEY_UP = 8;

	/*
	*	@brief Tecla derecha la cual su valor para ser identificada sera el 9.
	*/
	public const int KEY_RIGHT = 9;

	/*
	*	@brief Tecla abajo la cual su valor para ser identificada sera el 10.
	*/
	public const int KEY_DOWN = 10;

	/*
	*	@brief Tecla izquierda la cual su valor para ser identificada sera el 11.
	*/
	public const int KEY_LEFT = 11;

	/*
	*	@brief Analogo izquierdo la cual su valor para ser identificada sera el 12.
	*/
	public const int KEY_ANALOG_H = 12;

	/*
	*	@brief Analogo derecho la cual su valor para ser identificada sera el 13.
	*/
	public const int KEY_ANALOG_V = 13;

	/*
	*	@brief Ultimo movimiento hecho.
	*/
	private static float lastMove = 0;

	/**
	*	@brief Método para setear el Screen.
	*	
	*	@param screen Instancia del EthGUIScreen que llega por parámetro para inicializar el objeto.
	*/
	public static void setScreen( EthGUIScreen screen )
	{
		
   	}

   	/**
	*	@brief Método encargado de reconocer todos los botones del control de vita.
	*/
	public static void checkInputs()
	{
		KEY_ARRAY[KEY_X] = Input.GetKey( KeyCode.JoystickButton0 )?1f:0f;//X button
		KEY_ARRAY[KEY_O] = Input.GetKey( KeyCode.JoystickButton1 )?1f:0f;//○ button
		KEY_ARRAY[KEY_SQUARE] = Input.GetKey( KeyCode.JoystickButton2 )?1f:0f;//□ button
		KEY_ARRAY[KEY_TRIANGLE] = Input.GetKey( KeyCode.JoystickButton3 )?1f:0f;//△ button
		KEY_ARRAY[KEY_L] = Input.GetKey( KeyCode.JoystickButton4 )?1f:0f;//L button
		KEY_ARRAY[KEY_R] = Input.GetKey( KeyCode.JoystickButton5 )?1f:0f;//R button
		KEY_ARRAY[KEY_SELECT] = Input.GetKey( KeyCode.JoystickButton6 )?1f:0f;//SELECT button
		KEY_ARRAY[KEY_START] = Input.GetKey( KeyCode.JoystickButton7 )?1f:0f;//START button
		KEY_ARRAY[KEY_UP] = Input.GetKey( KeyCode.JoystickButton8 )?1f:0f;//up button
		KEY_ARRAY[KEY_RIGHT] = Input.GetKey( KeyCode.JoystickButton9 )?1f:0f;//right button
		KEY_ARRAY[KEY_DOWN] = Input.GetKey( KeyCode.JoystickButton10 )?1f:0f;//down button
		KEY_ARRAY[KEY_LEFT] = Input.GetKey( KeyCode.JoystickButton11 )?1f:0f;//left button
		KEY_ARRAY[KEY_ANALOG_H] = Input.GetAxis("Horizontal");//left analog horizontal 
		KEY_ARRAY[KEY_ANALOG_V] = Input.GetAxis("Vertical") * -1;//left analog vertical 

		#if UNITY_EDITOR
      		KEY_ARRAY[KEY_RIGHT] = Input.GetKey( KeyCode.D )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_LEFT] = Input.GetKey( KeyCode.A )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_UP] = Input.GetKey( KeyCode.W )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_DOWN] = Input.GetKey( KeyCode.S )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_ANALOG_V] = 0;
      		KEY_ARRAY[KEY_X] = Input.GetKey( KeyCode.K )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_SQUARE] = Input.GetKey( KeyCode.I )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_TRIANGLE] = Input.GetKey( KeyCode.O )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_O] = Input.GetKey( KeyCode.L )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_L] = Input.GetKey( KeyCode.Q )?1f:0f;//SELECT button
      		KEY_ARRAY[KEY_R] = Input.GetKey( KeyCode.E )?1f:0f;//SELECT button
      	#endif

      	KEY_RELEASE[KEY_X] = Input.GetKeyUp( KeyCode.JoystickButton0 );//X button
		KEY_RELEASE[KEY_O] = Input.GetKeyUp( KeyCode.JoystickButton1 );//○ button
		KEY_RELEASE[KEY_SQUARE] = Input.GetKeyUp( KeyCode.JoystickButton2 );//□ button
		KEY_RELEASE[KEY_TRIANGLE] = Input.GetKeyUp( KeyCode.JoystickButton3 );//△ button
		KEY_RELEASE[KEY_L] = Input.GetKeyUp( KeyCode.JoystickButton4 );//L button
		KEY_RELEASE[KEY_R] = Input.GetKeyUp( KeyCode.JoystickButton5 );//R button
		KEY_RELEASE[KEY_SELECT] = Input.GetKeyUp( KeyCode.JoystickButton6 );//SELECT button
		KEY_RELEASE[KEY_START] = Input.GetKeyUp( KeyCode.JoystickButton7 );//START button

		#if UNITY_EDITOR
      		
      		KEY_RELEASE[KEY_X] = Input.GetKeyUp ( KeyCode.K );//SELECT button
      		KEY_RELEASE[KEY_SQUARE] = Input.GetKeyUp ( KeyCode.I );//SELECT button
      		KEY_RELEASE[KEY_TRIANGLE] = Input.GetKeyUp ( KeyCode.O );//SELECT button
      		KEY_RELEASE[KEY_O] = Input.GetKeyUp ( KeyCode.L );//SELECT button
      		KEY_RELEASE[KEY_L] = Input.GetKeyUp ( KeyCode.Q );//SELECT button
      		KEY_RELEASE[KEY_R] = Input.GetKeyUp ( KeyCode.E );//SELECT button
      	#endif


      	KEY_PRESSED[KEY_X] = Input.GetKeyDown( KeyCode.JoystickButton0 );//X button
		KEY_PRESSED[KEY_O] = Input.GetKeyDown( KeyCode.JoystickButton1 );//○ button
		KEY_PRESSED[KEY_SQUARE] = Input.GetKeyDown( KeyCode.JoystickButton2 );//□ button
		KEY_PRESSED[KEY_TRIANGLE] = Input.GetKeyDown( KeyCode.JoystickButton3 );//△ button
		KEY_PRESSED[KEY_L] = Input.GetKeyDown( KeyCode.JoystickButton4 );//L button
		KEY_PRESSED[KEY_R] = Input.GetKeyDown( KeyCode.JoystickButton5 );//R button
		KEY_PRESSED[KEY_SELECT] = Input.GetKeyDown( KeyCode.JoystickButton6 );//SELECT button
		KEY_PRESSED[KEY_START] = Input.GetKeyDown( KeyCode.JoystickButton7 );//START button

      	if ( Input.GetKeyDown( KeyCode.JoystickButton0 ) )
      	{
      		KEY_USED[KEY_X] = false;
      	}

		if ( Input.GetKeyDown( KeyCode.JoystickButton1 ) )
		{
			KEY_USED[KEY_O] = false;
		}

		if ( Input.GetKeyDown( KeyCode.JoystickButton2 ) )
		{
			KEY_USED[KEY_SQUARE] = false;
		}

		if ( Input.GetKeyDown( KeyCode.JoystickButton3 ) )
		{
			KEY_USED[KEY_TRIANGLE] = false;
		}

		if ( Input.GetKeyDown( KeyCode.JoystickButton4 ) )
		{
			KEY_USED[KEY_L] = false;
		}

		if ( Input.GetKeyDown( KeyCode.JoystickButton5 ) )
		{
			KEY_USED[KEY_R] = false;
		}

		if ( Input.GetKeyDown( KeyCode.JoystickButton6 ) )
		{
			KEY_USED[KEY_SELECT] = false;
		}

		if ( Input.GetKeyDown( KeyCode.JoystickButton7 ) )
		{
			KEY_USED[KEY_START] = false;
		}
	}

	/**
	*	@brief Método encargado de asignar la tecla usada.
	*	
	*	@param keyUsed Numero de la tecla usada.
	*/
	public static void SetKeyUsed( int keyUsed )
	{
		KEY_USED[ keyUsed ] = true;
	}

	/*
	*	@brief Función para navegar por un grupo de manera horizontal con los botones de izquierda y derecha de un
	*	vita, esta funcion se tiene que llamar desde un OnGui
	*
	*	@param group 		Usado para navegar un grupo de botones horizontalmente usando los controles del Vita.
	*	@param timeWaitNext Cuando uno deja presionado un botón no se debe pasar de botón en botón en milisegundos, 
	*						si no que le da un tiempo de espera.
	*	@param lunchFn 		Es para ver si cada vez que cambia de botón llame una función.
	*/
	public static void NavigateGroupHorizontal( EthToggleButtonGroup group, float timeWaitNext = 0.5f, bool lunchFn = false  )
	{
		if ( lastMove <= 0 )
		{
		    if ( KEY_ARRAY[ KEY_LEFT] == 1f || KEY_ARRAY[ KEY_ANALOG_H ] < 0.0f )
		    {
		        int index = group.getSelectedIndex();
		        index --;
		        if ( index < 0 )
		        {
		            index = 0;//group.Length() - 1;
		        }
		        group.GetButton( index ).Click( lunchFn );
		        lastMove = timeWaitNext;
		    }
		    else if ( KEY_ARRAY[ KEY_RIGHT ] == 1f || KEY_ARRAY[ KEY_ANALOG_H ] > 0.0f )
		    {
		        //int index = (group.getSelectedIndex() + 1) % group.Length();
		        int index = group.getSelectedIndex();
		        index ++;

		        if ( index >= group.Length() )
		        {
		        	index = group.Length() - 1;
		        }

		        group.GetButton( index ).Click( lunchFn );
		        lastMove = timeWaitNext;
		    }
		    else
		    {
		        lastMove = 0;
		    }
		}
		
		lastMove -= Time.deltaTime;
	}

	/*
	*	@brief Función para navegar por un grupo de manera vertical con los botones de arriba y abajo de un
	*	vita, esta función se tiene que llamar desde un OnGui
	*	
	*	@param group 		Usado para navegar un grupo de botones vertizalmente usando los controles del Vita.
	*	@param timeWaitNext	Cuando uno deja presionado un botón no se debe pasar de botón en botón en milisegundos, 
	*						si no que le da un tiempo de espera.
	*	@param lunchFn		Es para ver si cada vez que cambia de botón llame una función.
	*	@param scroll 		Scroll de la pantalla.
	*	@param deltaScroll 	Cantidad de desplazamiento del scroll al mover los botones o analogos del vita.
	*/
	public static void NavigateGroupVertical( EthToggleButtonGroup group, float timeWaitNext = 0.5f, bool lunchFn = false, EthScroll scroll = null, float deltaScroll = 10f  )
	{
		if ( lastMove <= 0 )
		{
		    if ( KEY_ARRAY[ KEY_UP] == 1f || KEY_ARRAY[ KEY_ANALOG_V ] < 0.0f )
		    {
		        int index = group.getSelectedIndex();
		        index --;
		        if ( index < 0 )
		        {
		            index = 0;//group.Length() - 1;
		        }
		        group.GetButton( index ).Click( lunchFn );
		        lastMove = timeWaitNext;

		        if ( scroll != null )
		        {
		        	Vector2 pos = scroll.getScrollPosition();
		        	pos.y -= deltaScroll;
		        	scroll.setScrollPosition( pos );
		        }
		    }
		    else if ( KEY_ARRAY[ KEY_DOWN ] == 1f || KEY_ARRAY[ KEY_ANALOG_V ] > 0.0f )
		    {
		        //int index = (group.getSelectedIndex() + 1) % group.Length();
		        int index = group.getSelectedIndex();
		        index ++;

		        if ( index >= group.Length() )
		        {
		        	index = group.Length() - 1;
		        }

		        group.GetButton( index ).Click( lunchFn );
		        lastMove = timeWaitNext;

		        if ( scroll != null )
		        {
		        	Vector2 pos = scroll.getScrollPosition();
		        	pos.y += deltaScroll;
		        	scroll.setScrollPosition( pos );
		        }
		    }
		    else
		    {
		        lastMove = 0;
		    }
		}
		
		lastMove -= Time.deltaTime;
	}

	/*
	*	@brief Función para comprobar del proximo boton horizontal.
	*	
	*	@param group 		Usado para navegar un grupo de botones horizontalmente usando los controles del Vita.
	*	@param yThreshold	Es para darle un espacio a los botones en y, si tengo 3 botones y estoy en el primero, el mira los otros 
	*						2 de acuerdo a ese espacio y escoge el menor, para que no se vaya a botones que pueden estar lejos 
	*						y la navegación se vea mal.
	*	@param timeWaitNext	Cuando uno deja presionado un botón no se debe pasar de botón en botón en milisegundos, 
	*						si no que le da un tiempo de espera.
	*	@param lunchFn		Es para ver si cada vez que cambia de botón llame una función.
	*/
	private static void CheckNextButtonHorizontal( EthToggleButtonGroup group,float yThreshold, bool left, float timeWaitNext, bool lunchFn = false)
	{
		EthToggleButton currentSelected = group.GetSelectedButton();
		EthToggleButton closerButton = null;
		EthToggleButton tempButton;

        for ( int i = 0; i < group.Length(); i ++ )
        {
        	tempButton = group.GetButton( i );
			if ( tempButton != currentSelected )
			{
				//si esta en el threshold
				if ( tempButton.Y >= currentSelected.Y - yThreshold && tempButton.Y <= currentSelected.Y + yThreshold )
				{
					if ( left )
					{
						//si esta a la izquierda del boton actual
						if ( tempButton.X < currentSelected.X )
						{
							//si esta mas cerca
							if ( closerButton == null || tempButton.X > closerButton.X )
							{
								closerButton = tempButton;
							}
						}
					}
					else
					{
						//si esta a la derecha del boton actual
						if ( tempButton.X > currentSelected.X )
						{
							//si esta mas cerca
							if ( closerButton == null || tempButton.X < closerButton.X )
							{
								closerButton = tempButton;
							}
						}
					}
					
				}
			}
        }

        if ( closerButton != null )
        {
        	closerButton.Click( lunchFn );
        	lastMove = timeWaitNext;
        }
	}

	/*
	*	@brief Función para comprobar del proximo boton vertical.
	*	
	*	@param group 		Usado para navegar un grupo de botones verticalmente usando los controles del Vita.
	*	@param xThreshold	Es para darle un espacio a los botones en x, si tengo 3 botones y estoy en el primero, el mira los otros 
	*						2 de acuerdo a ese espacio y escoge el menor, para que no se vaya a botones que pueden estar lejos 
	*						y la navegación se vea mal.
	*	@param up 			Booleano que indica si esta al inicio de la navegacion vertical.
	*	@param timeWaitNext	Cuando uno deja presionado un botón no se debe pasar de botón en botón en milisegundos, 
	*						si no que le da un tiempo de espera.
	*	@param lunchFn		Es para ver si cada vez que cambia de botón llame una función.
	*/
	private static void CheckNextButtonVertical( EthToggleButtonGroup group,float xThreshold, bool up, float timeWaitNext, bool lunchFn = false)
	{
		EthToggleButton currentSelected = group.GetSelectedButton();
		EthToggleButton closerButton = null;
		EthToggleButton tempButton;

        for ( int i = 0; i < group.Length(); i ++ )
        {
        	tempButton = group.GetButton( i );
			if ( tempButton != currentSelected )
			{
				//si esta en el threshold
				if ( tempButton.X >= currentSelected.X - xThreshold && tempButton.X <= currentSelected.X + xThreshold )
				{
					if ( up )
					{
						//si esta ariba del boton actual
						if ( tempButton.Y < currentSelected.Y )
						{
							//si esta mas cerca
							if ( closerButton == null || tempButton.Y > closerButton.Y )
							{
								closerButton = tempButton;
							}
						}
					}
					else
					{
						//si esta abajo del boton actual
						if ( tempButton.Y > currentSelected.Y )
						{
							//si esta mas cerca
							if ( closerButton == null || tempButton.Y < closerButton.Y )
							{
								closerButton = tempButton;
							}
						}
					}
					
				}
			}
        }

        if ( closerButton != null )
        {
        	closerButton.Click( lunchFn );
        	lastMove = timeWaitNext;
        }
	}

	/*
	*	@brief Función para navegar.
	*	
	*	@param group 		Usado para navegar un grupo de botones verticalmente usando los controles del Vita.
	*	@param xThreshold	Es para darle un espacio a los botones en x, si tengo 3 botones y estoy en el primero, el mira los otros 
	*						2 de acuerdo a ese espacio y escoge el menor, para que no se vaya a botones que pueden estar lejos 
	*						y la navegación se vea mal.
	*	@param yThreshold	Es para darle un espacio a los botones en y, si tengo 3 botones y estoy en el primero, el mira los otros 
	*						2 de acuerdo a ese espacio y escoge el menor, para que no se vaya a botones que pueden estar lejos 
	*						y la navegación se vea mal.
	*	@param up 			Booleano que indica si esta al inicio de la navegacion vertical.
	*	@param timeWaitNext	Cuando uno deja presionado un botón no se debe pasar de botón en botón en milisegundos, 
	*						si no que le da un tiempo de espera.
	*	@param lunchFn		Es para ver si cada vez que cambia de botón llame una función.
	*/
	public static void NavigateGroup( EthToggleButtonGroup group, float xThreshold, float yThreshold, float timeWaitNext = 0.5f, bool lunchFn = false )
	{
		if ( lastMove <= 0 )
		{
			if ( KEY_ARRAY[ KEY_LEFT] == 1f || KEY_ARRAY[ KEY_ANALOG_H ] < 0.0f )
		    {
				CheckNextButtonHorizontal( group, yThreshold, true , timeWaitNext, lunchFn );		       
			}
		    else if ( KEY_ARRAY[ KEY_RIGHT ] == 1f || KEY_ARRAY[ KEY_ANALOG_H ] > 0.0f )
		    {
		        CheckNextButtonHorizontal( group, yThreshold, false, timeWaitNext, lunchFn );
		    }
		    else if ( KEY_ARRAY[ KEY_UP ] == 1f || KEY_ARRAY[ KEY_ANALOG_V ] < 0.0f )
		    {
		        CheckNextButtonVertical( group, xThreshold, true, timeWaitNext, lunchFn );
		    }
		    else if ( KEY_ARRAY[ KEY_DOWN ] == 1f || KEY_ARRAY[ KEY_ANALOG_V ] > 0.0f )
		    {
		    	CheckNextButtonVertical( group, xThreshold, false, timeWaitNext, lunchFn );
		    }
		    else
		    {
		        lastMove = 0;
		    }
		}
		
		lastMove -= Time.deltaTime;
	}

	/*
	*	@brief funcion para navegar por un grupo de manera horizontal con los botones de izquierda y derecha de un
	*	vita, esta funcion se tiene que llamar desde un OnGui
	*
	*	@param group 		Usado para navegar un grupo de botones verticalmente usando los controles del Vita.
	*	@param timeWaitNext Cuando uno deja presionado un botón no se debe pasar de botón en botón en milisegundos, 
	*						si no que le da un tiempo de espera.
	*	@param lunchFn		Es para ver si cada vez que cambia de botón llame una función.
	*/
	public static void NavigateGroupHorizontalRL( EthToggleButtonGroup group, float timeWaitNext = 0.5f, bool lunchFn = false  )
	{
		if ( lastMove <= 0 )
		{
		    if ( KEY_ARRAY[ KEY_L] == 1f )
		    {
		        int index = (group.getSelectedIndex() + 1) % group.Length();

		        group.GetButton( index ).Click( lunchFn );
		        lastMove = timeWaitNext;
		    }
		    else if ( KEY_ARRAY[ KEY_R ] == 1f )
		    {
		        int index = group.getSelectedIndex();
		        index --;
		        if ( index < 0 )
		        {
		            index = group.Length() - 1;
		        }
		        group.GetButton( index ).Click( lunchFn );
		        lastMove = timeWaitNext;
		    }
		    else
		    {
		        lastMove = 0;
		    }
		}
		
		lastMove -= Time.deltaTime;
	}
}
