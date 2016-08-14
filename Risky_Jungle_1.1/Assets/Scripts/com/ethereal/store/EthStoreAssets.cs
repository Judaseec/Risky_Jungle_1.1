/*
 * Copyright (C) 2012 Soomla Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

/** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0 
	* 	@date      Septiembre 5 2014
	* 
	*	@class 	EthStoreAssets
	*   @brief 	Clase encargada de manejar los elementos encontrados en la tienda virtual.
	*
	*/

public class EthStoreAssets : IStoreAssets {
	/**
	*	@brief Arraylist que contiene las monedas que se poseen actualmente como pagos con dinero real.
	*/
	private ArrayList currencyPacks;

	// Currencies
	/**
	*	@brief Constante que identifica las monedas virtuales como "coins".
	*/
	public const string VIRTUAL_CURRENCY = "coins";

	/**
	*	@brief Variable estatica de tipo VirtualCurrency que contiene la informacion de las monedas.
	*/
	public static VirtualCurrency COINS_CURRENCY = new VirtualCurrency (
                 "Coins", // name
                 "", // description
                 VIRTUAL_CURRENCY); // item id

	/**
    *	@brief	Método usado para obtener la version.
    *	
    *	@param pvi Item a ser adquirido.
	*	
	*	@return Número de la version actual.
    */
	public int GetVersion () {
		return 3;
	}
	
	
	/**
    *	@brief	Método usado para obtener las monedas.
	*	
	*	@return Array de tipo VirtualCurrency que contendra las monedas.
    */
	public VirtualCurrency[] GetCurrencies () {
		return  new VirtualCurrency[] {
	            COINS_CURRENCY
	        };
	}
	
	/**
    *	@brief	Método usado para obtener las mejoras.
	*	
	*	@return Array de tipo VirtualGood que contendra las mejoras.
    */
	public VirtualGood[] GetGoods () {
		return new VirtualGood[] {
		/* SingleUseVGs     --> */
		/* LifetimeVGs      --> */
		/* EquippableVGs    --> */
		/* SingleUsePackVGs --> */
		/* UpgradeVGs       --> */
	        };
	}
	
	/**
    *	@brief	Método usado para obtener los elemento de la tienda.
	*	
	*	@return Array de tipo VirtualCurrencyPack que contendra los elementos a ser parte de la tienda.
    */
	public VirtualCurrencyPack[] GetCurrencyPacks () {
		return (VirtualCurrencyPack []) currencyPacks.ToArray (typeof(VirtualCurrencyPack));
	}
	
	/**
    *	@brief	Método usado para obtener las categorias de los elementos en la tienda.
	*	
	*	@return Array de tipo VirtualCategory que contendra las categorias existentes de los items de la tienda.
    */
	public VirtualCategory[] GetCategories () {
		return new VirtualCategory[]{
	            
	        };
	}

	/**
    *	@brief	Método usado para agregar los elementos a ser comprados en la tienda.
    *	
	*	Se crea la lista de los elementos que se encontraran en la tienda, para luego identificar el producto que sera agregado 
	*	dependiendo si es de un dispositivo Android o IOS
	*
    *	@param idItem Item que sera agregado en la tienda virtual
    *	@param idAndroid Id del item del dispositivo Android.
    *	@param idIOS Id del item del dispositivo IOS.
    */
	public void AddCurrencyPack (string idItem, string idAndroid, string idIOS) {
		if ( currencyPacks == null ) {
			currencyPacks = new ArrayList ();
		}

		#if UNITY_ANDROID
			    string productId = idAndroid;
		#elif UNITY_IOS
				string productId = idIOS;
		#else
		string productId = "product.id";
		#endif

		currencyPacks.Add (
			new VirtualCurrencyPack (
				"CurrencyPack" + currencyPacks.Count, 
                "CurrencyPack" + currencyPacks.Count,
                idItem, // item id
                1, // number of currencies in the pack
                VIRTUAL_CURRENCY, // the associated currency
                new PurchaseWithMarket(new MarketItem(productId, 1.00))) );
	}
}
