using UnityEngine;


using System.Collections.Generic;
using System.Collections;
using System;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:

No necesariamente tienen que ser strings, pueden ser objetos los que se agregan y se muestra lo correspondiente a toString

EthComboBox combo = gui.AddComboBox("combo",0,0,"img:comboItem,text:HOLA,font:fonts/Avenger,fontColor:0_0_0,fontSize:18,useLang:false,heightItems:100");
combo.AddItem("Item0");
combo.AddItem("Item1");
combo.AddItem("Item2");
combo.AddItem("Item3");
combo.AddItem("Item4");
combo.AddItem("Item5");
combo.AddItem("Item6");
combo.AddItem("Item7");

combo.setFunction(clickCombo);

public void clickCombo(object objCombo) {
	Debug.Log(objCombo);
}

┌───────────────┬───┐
│ 		1		│ 2 │ 
├───────────────┼───┘
│ 		3		│
├───────────────┤
│ 		3		│
└───────────────┘

img-> (1)la imagen del combo seria por ejemplo "combo" para corresponder a un recurso "combo.png"
la imagen 2 es la misma 1 con _arrow al final, corresponderia entonces a un recurso "combo_arrow.png"
la imagen 3 es la misma 1 con _item al final, corresponderia entonces a un recurso "combo_item.png"

las imagenes que se usan para el scroll, por ejemplo seria la base (para mas info ver el EthScroll) "combo_scroll"

heightItems->El alto que tendra la seccion donde se muestran los items

los mismos args que recibe el combo se pasan a los componentes, por lo que la fuente y todos los parametros deben ser 
pasados al EthComboBox, para que este los replique a sus items.
*/
namespace Assets.Scripts.com.ethereal.display.components {
    
	/** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0 
	* 	@date      Octubre 16 2014
	* 
	*	@class 	EthComboBox
	*   @brief 	Esta clase se encarga de crear un único combo box, al cual posteriormente se le podrán agregar más items.
	*
	*/
	public class EthComboBox : EthComponent {
			
		/**
		*	@brief guiBot de la clase. permite manejar el elemento en pantalla y controlar sus opciones de visualización.
		*/
		protected GUIStyle guiBot;
		
		/**
		*	@brief el texto que mostrará el combo box.
		*/
		private string _text;
		
		/**
		*	@brief botón que va en cada ítem del combo box.
		*/
        private EthButton _botItem;
		
		/**
		*	@brief botón para mover la barra de desplazamiento.
		*/
        private EthButton _arrowItem;
		
		/**
		*	@brief scroll para desplazarse por el combo box.
		*/
        private EthScroll _scrollItems;
		
		/**
		*	@brief lista de los ítems del combo box.
		*/
        private ArrayList _items;
		
		/**
		*	@brief lista con los botones (EthButton) que contiene cada ítem del combo box.
		*/
        private List<EthButton> _itemsBots;
		
		/**
		*	@brief define si los ítems del combo box están desplegados o no.
		*/
        private bool _itemsDesplegados = false;
		
		/**
		*	@brief es el nombre de la imagen del combo box, este mismo nombre permitirá agregar los recursos del arrow y del ítem. 
		*/
        private string _nomImg;
		
		/**
		*	@brief parámetros con los cuales se va a crear el combo box.
		*/
        private EthArguments _args;
		
		/**
		*	@brief altura de cada item del combo box.
		*/
        private float _altoItem;
		
		/**
		*	@brief índice seleccionado actualmente en el combo box.
		*/
		private int _indexSelected;
		
		/**
		*	@brief objeto seleccionado actualmente en el combo box.
		*/
        private object _objSelected;

        /**
		*	@brief Asignación de las propiedades de lectura y escritura del atributo _objSelected.
		*/
        public object SelectedItem {
            get { return _objSelected; }
            set { _objSelected = value; }
        }

		/**
		*	@brief evento onClick para el combo box, que recibe el item seleccionado.
		*/
		public delegate void OnClickEvent (object objCombo);

		/**
		*	@brief evento para detectar el click sobre algún ítem del combo box.
		*/
		public event OnClickEvent OnClick;

		/**
		*	@brief constructor de la clase EthComboBox.
		*
		*	este método permite crear una instancia de la clase EthComboBox
		*
		*	@param args parámetros con los cuales se creará la clase.
		*	@param parentGUI 
		*/
		public EthComboBox (EthArguments args, EthComponentManager parentGUI) : base(args, parentGUI) {
			this._args = args;
			_items = new ArrayList ();
			
			if ( args ["img"] != null ) {
				_nomImg = args ["img"];
				_botItem = new EthButton (args, parentGUI);
				_botItem.SetFunction (Deploy);
				
				Texture2D textura = Resources.Load (args ["img"]) as Texture2D;

				args ["img"] = _nomImg + "_arrow";
				args ["text"] = "";

				args ["x"] = "" + (float.Parse (args ["x"])) + textura.width;
				_arrowItem = new EthButton (args, parentGUI);
				_arrowItem.SetFunction (Deploy);
				
				Texture2D itemTexture = Resources.Load (_nomImg + "_item") as Texture2D;
				_altoItem = itemTexture.height;

				float heightItems = float.Parse ((string) Eth.GetVal (args ["heightItems"], "200"));
				Texture2D texturaArrow = Resources.Load (_nomImg + "_arrow") as Texture2D;

                _scrollItems = _gui.AddScroll("bot", X, Y + _altoItem, textura.width + texturaArrow.width, heightItems, "img:" + _nomImg + "_scroll");
				_scrollItems.Visible=false;
			}				
		}

		/**
		*	@brief Adiciona un ítem al combo box.
		*
		*	Permite agregar otro campo con un ítem al final del combo box.
		*	
		*	@param obj el objeto a agregar al combo box.
		*/
		public void AddItem (object obj) {

			_args ["img"] = "" + _nomImg + "_item";
			_args ["name"] = "" + _items.Count;
			_args ["x"] = "" + 0;
			_args ["y"] = "" + ((_altoItem * (_items.Count)));
			_args ["text"] = (string) obj;			

			EthButton botItem = _scrollItems.gui.AddButton ("" + _items.Count, 0, _altoItem * (_items.Count), _args);
			botItem.SetFunction (selectItem);			
			_items.Add (obj);
		}
		

		/**
		*	@brief Método para cambiar el estado de la variable itemsDesplegados.
		*
		*	Sí el botón no esta desplegado, lo despliega y viceversa.
		*/
		public void Deploy (string nameBot) {
			_itemsDesplegados = !_itemsDesplegados;
		}

		/**
		*	@brief Método que al seleccionar un elemento del comboBox quita el despliegue y cambia el valor por el seleccionado,
		*
		*	@param nameBot Nombre del elemento seleccionado.
		*/
		public void selectItem (string nameBot) {

			_itemsDesplegados = false;
			
			_indexSelected = Convert.ToInt32 (nameBot);
			SelectedItem = _items [_indexSelected];

			_botItem.Text = (string) _items [_indexSelected];

			if ( OnClick != null ) {
				OnClick (_items [_indexSelected]);
			}			
		}

		/**
		*	@brief Devuelve el índice seleccionado.
		*
		*	Retorna el índice del objeto seleccionado en el combo box.
		*/
		public int getSelectedIndex () {
			return _indexSelected;
		}

		/**
		*	@brief Muestra el combo box en la pantalla.
		*
		*	Se encarga de mostrar el combo box en la pantalla en la ubicación elegida.
		*	
		*	@param offset vector2 que da las cordenadas para ubicar el objeto en un punto específico de la pantalla.
		*/
		public override void Draw (Vector2? offset = null) {
			if ( !Visible ) {
				return;
			}
			
			_botItem.Draw (offset);
			_arrowItem.Draw (offset);
				
			_scrollItems.Visible = _itemsDesplegados;			
		}

		/**
		*	@brief Permite adicionar el evento OnClick al combo box.
		*
		*	Permite adicionar el evento OnClick al que responderán los ítems en el combo box al ser presionados.
		*
		*	@param fn el evento on click que es escuchado por el combo box y los ítems en este.
		*/
		public void SetFunction (OnClickEvent fn) {
			OnClick += fn;
		}

		/**
		*	@brief Método toString de la clase.
		*
		*	Retorna el nombre de la clase como una cadena de caracteres.
		*
		*	@return la clase representada en un string.
		*/
		public override string ToString () {
			return "EthComboBox (" + Name + ")";
		}
	}
}
