using UnityEngine;
using Assets.Scripts.com.ethereal.util;

namespace com.ethereal.analytics{

	/** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthGoogleAnalytics
    *   @brief 	Esta clase se encarga de establecer el seguimiento a lo que realiza el usuario por medio de google analytics.
    *
    */
	public class EthGoogleAnalytics
	{
		/**
        *	@brief Patrón Singleton para mantener la misma instancia de EthGoogleAnalytics en todo el juego.
        */
		private static EthGoogleAnalytics instance;

        /**
        *   @brief Constante para indicar la versión de google analytics en el primer parámetro del método invocado por el constructor.
        */
        private const string GAV3 = "GAv3";
		
		/**
        *	@brief Instancia de GoogleAnalyticsV3.
        */
		private GoogleAnalyticsV3 ga;

		/**
        *	@brief Método para crear una instancia de la clase EthGoogleAnalytics.
        *
        */
		private EthGoogleAnalytics() 
		{	
			ga = (GoogleAnalyticsV3)ObjectFactory.CreateObject<GoogleAnalyticsV3>(GAV3);
			MonoBehaviour.DontDestroyOnLoad( ga.gameObject );
		}

		/**
        *	@brief Método para definir el EthGoogleAnalytics a usar.
        *
        *	Este método es el encargado de que cuando no haya alguna instancia de EthGoogleAnalytics cree una nueva, 
        *	de lo contrario si ya hay un EthGoogleAnalytics en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de EthGoogleAnalytics.
        */
		public static EthGoogleAnalytics getInstance()
		{
			if ( EthGoogleAnalytics.instance == null )
			{
				EthGoogleAnalytics.instance = new EthGoogleAnalytics();	
			}
			
			return EthGoogleAnalytics.instance;
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
		public void ConfigureAnalytics( string trackingCode, string appName, string bundleID, string appVersion, GoogleAnalyticsV3.DebugMode logLevel )
		{
			ga.androidTrackingCode = trackingCode;
			ga.IOSTrackingCode = trackingCode;
			ga.otherTrackingCode = trackingCode;
			ga.productName = appName;
			ga.bundleIdentifier = bundleID;
			ga.bundleVersion = appVersion;
			ga.logLevel = logLevel;

			ga.StartSession();
		}

		/**
        *	@brief Método para registrar una pantalla.
        *
        *	@param title Titulo de la ventana.
        *
        */
		public void LogScreen(string title)
		{
			ga.LogScreen( title );
		}

		/**
        *	@brief Método para registrar un evento.
        *
        *	@param eventCategory 	Categoría del evento.
        *	@param eventAction		Acción del evento.
        *	@param eventLabel		Etiqueta del evento.
        *	@param value			Valor del evento.
        *
        */
		public void LogEvent(string eventCategory, string eventAction, string eventLabel, long value)
		{
			ga.LogEvent(eventCategory, eventAction, eventLabel, value);
		}

		/**
        *	@brief Método para registrar una excepción.
        *
        *	@param exceptionDescription	Descripción de la excepción.
        *	@param isFatal				Indica si la excepción es fatal.
        *
        */
		public void LogException(string exceptionDescription, bool isFatal)
		{
			ga.LogException(exceptionDescription, isFatal);
		}

		/**
        *	@brief Método para registrar tiempos de usuario.
        *
        *	@param timingCategory	Categoría de la medición de tiempo.
        *	@param timingInterval	Intervalo de la medición de tiempo.
        *	@param timingName		Nombre de la medición de tiempo.
        *	@param timingLabel		Etiqueta de la medición de tiempo.
        *
        */
		public void LogTiming(string timingCategory, long timingInterval, string timingName, string timingLabel)
		{
			 ga.LogTiming(timingCategory, timingInterval, timingName, timingLabel);
		}

		/**
        *	@brief Método para registrar interacciones en redes sociales.
        *
        *	@param socialNetwork	Red social con la cual se realiza la ionrección.
        *	@param socialAction		Acción que se realiza.
        *	@param socialTarget		Objetivos de la red social.
        *
        */
		public void LogSocial(string socialNetwork, string socialAction, string socialTarget)
		{
			ga.LogSocial(socialNetwork, socialAction, socialTarget);
		}

		/**
        *	@brief Método para registrar una transacción.
        *
        *	@param transID		Id de la trasnsacción.
        *	@param affiliation	Afiliación de la transacción.
        *	@param revenue		Ingreso o valor neto de la transacción.
        *	@param tax			Impesto sobre la transacción.
        *	@param shipping		Costo de envío.
        *
        */
		public void LogTransaction(string transID, string affiliation, double revenue, double tax, double shipping, string currencyCode = "USD")
		{
			ga.LogTransaction(transID, affiliation, revenue, tax, shipping, currencyCode);
		}

		/**
        *	@brief Método para registrar la transacción de un item.
        *
        *	@param transID	Id de la trasnsacción.
        *	@param name 	Nombre del item.
        *	@param SKU		Número de referencia del item.
        *	@param category Categoria del item.
        *	@param price	Precio del item.
        *	@param quantity	Cantidad de items.
        *
        */
		public void LogItem(string transID, string name, string SKU, string category, double price, long quantity,  string currencyCode = "USD")
		{
		 	ga.LogItem(transID, name, SKU, category, price, quantity, currencyCode);
		}

		/**
        *	@brief Método para parar la sesión y cerrar el tracker.
        *
        */
		public void Dispose()
		{
			ga.StopSession();
			ga.Dispose();
		}

		/**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthGoogleAnalytics.
        *
        */
		public override string ToString()
		{
			return "EthGoogleAnalytics";
		}
	}
}