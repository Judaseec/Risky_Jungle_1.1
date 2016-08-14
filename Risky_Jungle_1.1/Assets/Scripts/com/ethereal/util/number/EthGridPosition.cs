using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Assets.Scripts.com.ethereal.util.number {

	/** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0 
	* 	@date      Julio 21 2014
	* 
	*	@class 	EthGridPosition
	*   @brief 	Esta clase estara encargada de maneja la información de una cuadrícula ubicada en una GUI para así poder organizar 
	*			adecuadamente en ella los objetos.
	*	
	*	Se tiene en cuenta que esta cuadrícula tendrá una margen en X y en Y en donde comenzará y un aumento en X y en Y para ir ubicando las 
	*	columnas o celdas que posea la cuadrícula.
	*/
	public class EthGridPosition {   

		/**
		*	@brief Margen en X.
		*/
		public float borderX;

		/**
		*	@brief Margen en Y.
		*/
		public float borderY;

		/**
		*	@brief Aumento o incremento en X.
		*/
		public float incrementX;

		/**
		*	@brief Aumento o incremento en Y.
		*/
		public float incrementY;

		/**
		*	@brief Columnas de la cuadrícula.
		*/
		public int columnsNumber;

		/**
		*	@brief Método usado para Instanciar un EthGridPosition.
		*	
		*	Este método es el encargado de crear un nuevo EthGridPosition asignando todos los atributos que requiere esta clase, como lo son 
		*	el margen en x y en y, el incremento en x y en y la cantidad de columnas.
		*
		*	@param borderX			Margen en x de la cuadrícula.
		*	@param borderY			Margen en y de la cuadrícula.
		*	@param incrementX		Incremento en x.
		*	@param incrementY		Incremento en y.
		*	@param columnsNumber	Número de columnas.
		*/
		public EthGridPosition (float borderX, float borderY, float incrementX, float incrementY, int columnsNumber) {
			this.borderX = borderX;
			this.borderY = borderY;
			this.incrementX = incrementX;
			this.incrementY = incrementY;
			this.columnsNumber = columnsNumber;
		}

		/**
		*	@brief Método usado para obtener la posición de la cuadricula de acuerdo a un número especificado.
		*	
		*	Este método permite obtener la siguiente cuadricula del especificado.
		*
		*	@param step	Número especificado para obtener la celda de la cuadrícula..
		*
		*/
		public float[] GetGridPosition (int step) {

			int partX = step % columnsNumber;
			int partY = step / columnsNumber;

			return new float[] {
				borderX + (partX * incrementX),
				borderY + (partY * incrementY)
				};
		}
	}
}