using UnityEngine;
using Assets.Scripts.com.ethereal.appsSystem;
using Assets.Scripts.com.ethereal.display;
using GameAnalyticsSDK;

namespace com.ethereal.analytics{

	/** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthAnalytics
    *   @brief 	Esta clase se encarga de establecer el seguimiento a lo que realiza el usuario por medio de google analytics y EthAppsSystem.
    *
    */
	public class EthAnalytics
	{
		/**
        *	@brief Patrón Singleton para mantener la misma instancia de EthAnalytics en todo el juego.
        */
		private static EthAnalytics instance;
		
		/**
        *	@brief Instancia de la clase EthGoogleAnalytics.
        */
		EthGoogleAnalytics ethGoogleAnalytics;
		
		/**
        *	@brief Instancia de la clase EthAppsSystem.
        */
		EthAppsSystem ethAppSystem;

		/**
        *	@brief Instancia de la clase EthGameAnalytics.
        */
		EthGameAnalytics ethGameAnalytics;

		/**
        *	@brief Método para crear una instancia de la clase EthAnalytics.
        *
        */
		private EthAnalytics() 
		{	
		}

		/**
        *	@brief Método para definir el EthAnalytics a usar.
        *
        *	Este método es el encargado de que cuando no haya alguna instancia de EthAnalytics cree una nueva, 
        *	de lo contrario si ya hay un EthAnalytics en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de EthAnalytics.
        */
		public static EthAnalytics GetInstance()
		{
			if ( EthAnalytics.instance == null )
			{
				EthAnalytics.instance = new EthAnalytics();	
			}
			
			return EthAnalytics.instance;
		}

		/**
        *	@brief Método para configurar google analytics.
        *
        *	@param trackingCode	Código de seguimiento.
        *	@param appName		Nombre de la app.
        *	@param bundleID		Bundle ID de la app.
        *	@param appVersion	Version de la app.
        *	@param logLevel		Nivel de seguimiento de la app.
        *
        */
		public void ConfigureGoogleAnalytics( string trackingCode, string appName, string bundleID, string appVersion, GoogleAnalyticsV3.DebugMode logLevel )
		{
			ethGoogleAnalytics = EthGoogleAnalytics.getInstance( );
			ethGoogleAnalytics.ConfigureAnalytics( trackingCode, appName, bundleID, appVersion, logLevel );
		}

		/**
        *	@brief Método para configurar el EthAppSystem.
        *
        *	@param url Es el end point del servicio web.
        *	@param parent Escena que llama a ethAppsSystem requerida para poder inicializar la conexión.
        *
        */
		public void ConfigureEthSystem( string url, MonoBehaviour parent, EthAppsSystem.OnAppSystemReady onReady )
		{
			ethAppSystem = EthAppsSystem.GetInstance();
			EthAppsSystem.UrlEthAppsSystem = url;
			EthAppsSystem.Init( parent, onReady);
		}

		/**
        *	@brief Método para configurar el EthGameAnalytics.
        */
		public void ConfigureGameAnalytics( )
		{
			ethGameAnalytics = EthGameAnalytics.getInstance();
		}

		/**
        *	@brief Método para obtener el name del ethAppSystem.
        *
        *	En caso de que el ethAppSystem sea no esté inicializado el retorno de este método será null.
        *
        *	@param name Variable a ser obtenida del diccionario del ethAppSystem.
        *
        *	@return Variable encontrada en el diccionario.
        */
		public string GetVariable( string name )
		{
			if ( ethAppSystem != null )
			{
				return EthAppsSystem.GetInstance().GetVariableFromDictionary( name );
			}
			else
			{
				Debug.Log("No se ha configurado el ethAppSystem");
				return null;
			}
		}

		/**
        *	@brief Método para registrar una pantalla en ethGoogleAnalytics y ethAppSystem.
        *
        *	@param title Titulo de la ventana.
        *	@param parent Escena que llama al ethAppsSystem.
        *
        */
		public void LogScreen(string title, MonoBehaviour parent)
		{
			if ( ethGoogleAnalytics != null )
			{
				ethGoogleAnalytics.LogScreen( title );
			}

			if ( ethAppSystem != null )
			{
				EthAppsSystem.LogScreen( parent, title );
			}

			if ( ethGameAnalytics != null ){
				ethGameAnalytics.LogScreen( title );
			}
		}

		/**
        *	@brief Método para registrar un evento en ethGoogleAnalytics y ethAppSystem.
        *
        *	@param eventCategory 	Categoría del evento.
        *	@param eventAction		Acción del evento.
        *	@param eventLabel		Etiqueta del evento.
        *	@param value			Valor del evento.
        *	@param parent			Escena que llama al ethAppsSystem.
        *
        */
		public void LogEvent(string eventCategory, string eventAction, string eventLabel, long value, MonoBehaviour parent)
		{
			if ( ethGoogleAnalytics != null )
			{
				ethGoogleAnalytics.LogEvent(eventCategory, eventAction, eventLabel, value);
			}

			if ( ethAppSystem != null )
			{
				EthAppsSystem.Log( parent, eventCategory , eventAction, eventLabel, "" + value );
			}

			if ( ethGameAnalytics != null ){
				ethGameAnalytics.LogEvent(eventCategory, eventAction, eventLabel, value);
			}
		}

		/**
        *	@brief Método para registrar una excepción en ethGoogleAnalytics y ethAppSystem.
        *
        *	@param exceptionDescription	Descripción de la excepción.
        *	@param isFatal				Indica si la excepción es fatal.
        *	@param parent				Escena que llama al ethAppsSystem.
        *
        */
		public void LogException(string exceptionDescription, bool isFatal, MonoBehaviour parent)
		{
			if ( ethGoogleAnalytics != null )
			{
				ethGoogleAnalytics.LogException(exceptionDescription, isFatal);
			}

			if ( ethAppSystem != null )
			{
				//TODO EthAppsSystem.log( parent, "Exception:" + exceptionDescription );
			}

			if ( ethGameAnalytics != null ){
				ethGameAnalytics.LogException( isFatal? GA_Error.GAErrorSeverity.GAErrorSeverityCritical : GA_Error.GAErrorSeverity.GAErrorSeverityError ,
											  exceptionDescription);
			}
		}

		/**
        *	@brief Método para registrar tiempos de usuario en ethGoogleAnalytics y ethAppSystem.
        *
        *	@param timingCategory	Categoría de la medición de tiempo.
        *	@param timingInterval	Intervalo de la medición de tiempo.
        *	@param timingName		Nombre de la medición de tiempo.
        *	@param timingLabel		Etiqueta de la medición de tiempo.
        *	@param parent			Escena que llama al ethAppsSystem.
        *
        */
		public void LogTiming(string timingCategory, long timingInterval, string timingName, string timingLabel, MonoBehaviour parent)
		{
			if ( ethGoogleAnalytics != null )
			{
				 ethGoogleAnalytics.LogTiming(timingCategory, timingInterval, timingName, timingLabel);
			}

			if ( ethAppSystem != null )
			{
				//TODO EthAppsSystem.log( parent, "Timing:" + timingName + "=" + timingInterval );
			}
		}

		/**
        *	@brief Método para registrar interacciones en redes sociales en ethGoogleAnalytics y ethAppSystem.
        *
        *	@param socialNetwork	Red social con la cual se realiza la ionrección.
        *	@param socialAction		Acción que se realiza.
        *	@param socialTarget		Objetivos de la red social.
        *	@param parent			Escena que llama al ethAppsSystem.
        *
        */
		public void LogSocial(string socialNetwork, string socialAction, string socialTarget, MonoBehaviour parent)
		{
			if ( ethGoogleAnalytics != null )
			{
				ethGoogleAnalytics.LogSocial(socialNetwork, socialAction, socialTarget);
			}

			if ( ethAppSystem != null )
			{
				//TODO EthAppsSystem.log( parent, "social:" + socialNetwork + "=" + socialAction );
			}
		}

		/**
        *	@brief Método para registrar una transacción en ethGoogleAnalytics y ethAppSystem.
        *
        *	@param transID		Id de la trasnsacción.
        *	@param affiliation	Afiliación de la transacción.
        *	@param revenue		Ingreso o valor neto de la transacción.
        *	@param tax			Impesto sobre la transacción.
        *	@param shipping		Costo de envío.
        *	@param parent		Escena que llama al ethAppsSystem.
        *
        */
		public void LogTransaction(string transID, string affiliation, double revenue, double tax, double shipping, MonoBehaviour parent, string currencyCode = "USD")
		{
			if ( ethGoogleAnalytics != null )
			{
				ethGoogleAnalytics.LogTransaction(transID, affiliation, revenue, tax, shipping, currencyCode);
			}

			if ( ethAppSystem != null )
			{
				//TODO EthAppsSystem.log( parent, "Transaction:" + transID + "=" + revenue );
			}
		}

		/**
        *	@brief Método para registrar una transacción en ethGameAnalytics y ethAppSystem.
        *
        *	@param amount  integer Amount in cents. example: 99 is 0.99$
            @param itemType string  The type / category of the item. example: GoldPacks
        *   @param cartType string  The game location of the purchase. example: EndOfLevel. 
        *	@param itemId string  Specific item bought. example: 1000GoldPack.
        *
        */
		public void LogTransaction(string itemType,int amount, string cartType, string itemId, MonoBehaviour parent, string currencyCode = "USD"){
			if ( ethGameAnalytics != null )
			{
				ethGameAnalytics.LogTransaction(itemType, amount, cartType, itemId, currencyCode);
			}

			if ( ethAppSystem != null )
			{
				EthAppsSystem.Log( parent, "Monetization" , "compraRealizada", itemId, "1" );
			}
		}

		/**
        *	@brief Método para registrar la transacción de un item en ethGoogleAnalytics y ethAppSystem.
        *
        *	@param transID	Id de la trasnsacción.
        *	@param name 	Nombre del item.
        *	@param SKU		Número de referencia del item.
        *	@param category Categoria del item.
        *	@param price	Precio del item.
        *	@param quantity	Cantidad de items.
        *	@param parent	Escena que llama al ethAppsSystem.
        *
        */
		public void LogItem(string transID, string name, string SKU, string category, double price, long quantity,  MonoBehaviour parent, string currencyCode = "USD")
		{
			if ( ethGoogleAnalytics != null )
			{
				ethGoogleAnalytics.LogItem(transID, name, SKU, category, price, quantity, currencyCode);
			}

			if ( ethAppSystem != null )
			{
				//TODO EthAppsSystem.log( parent, "Item:" + name + "=" + price );
			}
		}

		/**
        @brief Método para registrar la transacción de un item virtual. Be carefull to not call the resource event too often ! 
        *
        *	@param flowType enum Add (source) or substract (sink) resource.
            @param currency string One of the available currencies set in GA_Settings (Setup tab).
            @param amount float Amount sourced or sinked.
            @param itemType string One of the available item types set in GA_Settings (Setup tab).
            @param itemId string Item id (string max length=32).
        *
        */
		public void LogItem(GA_Resource.GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId){
			if ( ethGameAnalytics != null ){
				ethGameAnalytics.LogItem( flowType, currency, amount, itemType, itemId);
			}

			if ( ethAppSystem != null ){
				//TODO EthAppsSystem.log( parent, "Transaction:" + transID + "=" + revenue );
			}
		}

		/**
        *	@brief Método para parar la sesión y cerrar el tracker.
        *
        */
		public void Dispose( )
		{
			if ( ethGoogleAnalytics != null )
			{
				ethGoogleAnalytics.Dispose();
			}
		}

		/**
        *	@brief Método para cambiar la variable estado.
        *
        */
		public void ChangeStateVariable( string newValue, MonoBehaviour parent )
		{
			Debug.Log("intenta");
			if ( ethAppSystem != null )
			{
				Debug.Log("lo envia");
				EthAppsSystem.ChangeStateVariable( parent, "level", newValue );
			}
		}


		/**
		*   @brief Método para registrar una progression.
		*
		*   @param progressionStatus    int    Status of added progression (start = 1, complete = 2, fail = 3).
		    @param progression01   string  1st progression (e.g. world01).
		    @param progression02   string   2nd progression (e.g. level01).
		    @param progression03   string   3rd progression (e.g. phase01).
		    @param score   int  The player's score..
		*
		*/
		public void LogProgression( int progressionStatus, string progression01, string progression02 = "", string progression03  = "", int score = 0){
		    if ( ethGameAnalytics != null ){
				ethGameAnalytics.LogProgression( (GA_Progression.GAProgressionStatus)progressionStatus, progression01, progression02, progression03, score);
			}
		}

		/**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthAnalytics.
        *
        */
		public override string ToString()
		{
			return "EthAnalytics";
		}
	}
}