using UnityEngine;
using GameAnalyticsSDK;

namespace com.ethereal.analytics{

	/** 
    *	@author    EtherealGF <www.etherealgf.com> 
    * 	@version   1.0 
    * 	@date      Octubre 16 2014
    * 
    *	@class 	EthGameAnalytics
    *   @brief 	Esta clase se encarga de establecer el seguimiento a lo que realiza el usuario por medio de game analytics.
    *
    */
	public class EthGameAnalytics
	{
		/**
        *	@brief Patrón Singleton para mantener la misma instancia de EthGameAnalytics en todo el juego.
        */
		private static EthGameAnalytics instance;
		
		
		/**
        *	@brief Método para crear una instancia de la clase EthGameAnalytics.
        *
        */
		private EthGameAnalytics() 
		{	
			
		}

		/**
        *	@brief Método para definir el EthGameAnalytics a usar.
        *
        *	Este método es el encargado de que cuando no haya alguna instancia de EthGameAnalytics cree una nueva, 
        *	de lo contrario si ya hay un EthGameAnalytics en el juego se seguira trabajando con la misma.
        *
        *	@return Instancia de EthGameAnalytics.
        */
		public static EthGameAnalytics getInstance()
		{
			if ( EthGameAnalytics.instance == null )
			{
				EthGameAnalytics.instance = new EthGameAnalytics();	
			}
			
			return EthGameAnalytics.instance;
		}

		/**
        *	@brief Método para registrar una pantalla.
        *
        *	@param title Titulo de la ventana.
        *
        */
		public void LogScreen(string title)
		{
			GameAnalytics.NewDesignEvent( "screen:" + title);
		}

        /**
        *   @brief Método para registrar una progression.
        *
        *   @param progressionStatus    enum    Status of added progression (start, complete, fail).
            @param progression01   string  1st progression (e.g. world01).
            @param progression02   string   2nd progression (e.g. level01).
            @param progression03   string   3rd progression (e.g. phase01).
            @param score   int  The player's score..
        *
        */
        public void LogProgression(GA_Progression.GAProgressionStatus progressionStatus, string progression01, string progression02 = "", string progression03  = "", int score = 0)
        {
            GameAnalytics.NewProgressionEvent( progressionStatus, progression01, progression02, progression03, score);
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
		public void LogEvent(string eventCategory, string eventAction, string eventLabel, float value)
		{
			GameAnalytics.NewDesignEvent ( eventCategory+":"+eventAction+":"+eventLabel, value);
        }

		/**
        *	@brief Método para registrar una excepción.
        *
        *	@param exceptionDescription	Descripción de la excepción.
        *	@param isFatal				Indica si la excepción es fatal.
        *
        */
		public void LogException(GA_Error.GAErrorSeverity severity, string message)
		{
			GameAnalytics.NewErrorEvent (severity, message);
		}

		/**
        *	@brief Método para registrar una transacción.
        *
        *   @param amount  integer Amount in cents. example: 99 is 0.99$
            @param itemType string  The type / category of the item. example: GoldPacks
        *   @param cartType string  The game location of the purchase. example: EndOfLevel. 
        *	@param itemId string  Specific item bought. example: 1000GoldPack.
        *
        */
		public void LogTransaction( string itemType,int amount, string cartType, string itemId, string currencyCode = "USD" )
		{
			GameAnalytics.NewBusinessEvent (currencyCode, amount, itemType, itemId , cartType);
		}

		/**
        *	@brief Método para registrar la transacción de un item virtual. Be carefull to not call the resource event too often ! 
        *
        *	@param flowType enum Add (source) or substract (sink) resource.
            @param currency string One of the available currencies set in GA_Settings (Setup tab).
            @param amount float Amount sourced or sinked.
            @param itemType string One of the available item types set in GA_Settings (Setup tab).
            @param itemId string Item id (string max length=32).
        *
        */
		public void LogItem(GA_Resource.GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId)
		{
		 	GameAnalytics.NewResourceEvent ( flowType, currency, amount, itemType, itemId);
		}

		/**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthGameAnalytics.
        *
        */
		public override string ToString()
		{
			return "EthGameAnalytics";
		}
	}
}