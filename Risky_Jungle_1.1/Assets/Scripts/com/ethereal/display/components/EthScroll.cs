using UnityEngine;


using System;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:

EthScroll scroll = gui.AddScroll("bot",0,0,200,200,"img:img");

scroll.gui.AddButton("bot",0,0,"img:avatar/Funcionamiento/btn_principal,text:HOLA,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");
scroll.gui.AddButton("bot",0,40,"img:avatar/Funcionamiento/btn_principal,text:HO,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");
scroll.gui.AddButton("bot",0,80,"img:avatar/Funcionamiento/btn_principal,text:HO,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");
scroll.gui.AddButton("bot",0,120,"img:avatar/Funcionamiento/btn_principal,text:HO,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");
scroll.gui.AddButton("bot",0,160,"img:avatar/Funcionamiento/btn_principal,text:HO,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");
scroll.gui.AddButton("bot",0,200,"img:avatar/Funcionamiento/btn_principal,text:HO,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");

img -> La base para el scroll, se utiliza segun lo siguiente, en este ejemplo se usa img:scrollEjemplo
		La barra utiliza el recurso scrollEjemplo
		El boton central de la barra utiliza scrollEjemplo_button
		El boton de arriba (subir el scroll) utiliza scrollEjemplo_up
		El boton de abajo (bajar el scroll) utiliza scrollEjemplo_down
*/
namespace Assets.Scripts.com.ethereal.display.components {
    /** 
	*	@author    EtherealGF <www.etherealgf.com> 
	* 	@version   1.0 
	* 	@date      Noviembre 8 2014
	* 
	*	@class 	EthScroll
	*   @brief 	Esta clase se encarga de crear un scroll, vertical u horizontal, que 
	*	puede ser usado para moverse en una pantalla, una lista o un combo box.
	*
	*/
	public class EthScroll : EthComponent {
	
		/**
        *   @brief EthComponentManager al que pertenece la EthModalWindow.
        */
		new public EthComponentManager gui;
		
		/**
		*	@brief la posición del scroll en pantalla.
		*/
		Vector2 scrollPosition;

		/**
		*	@brief se usa para implementar el drag en el scroll.
		*/
		Vector2 addScrollPosition;
		
		/**
		*	@brief la posición del scroll en pantalla.
		*/
		Vector2 scrollPositionTemp;
		
		/**
		*	@brief Valor para arrastrar el EthScroll en el eje Y.
		*/
		float dragPosY;

		/**
		*	@brief Variable que define sí el EthScroll esta siendo arrastrado.
		*/
		bool inDrag = false;
		
		/**
		*	@brief Variable que define sí cuando el EthScroll esta siendo arrastrado se desabilitan todos sus hijos.
		*/
		public bool disableChildrenWhileDrag = true;

		/**
		*	@brief la imagen base para el scroll, esta es la que usa la barra.
		*/
		Texture2D textura;
		
		/**
		*	@brief la imagen que usa el botón central en la barra del scroll.
		*/
		Texture2D texturaBot;
		
		/**
		*	@brief la imagen que usa el botón para subir el scroll.
		*/
		Texture2D texturaUp;
		
		/**
		*	@brief la imagen que usa el botón para bajar el scroll. 
		*/
		Texture2D texturaDown;

		/**
		*	@brief el ancho de los controles del scroll.
		*/
		float widControllers;
		
		/**
		*	@brief Define la apariencia y comportamiento de la GUI.
		*/
		GUISkin backUpSkin;

		/**
		*	@brief define si el scroll se desplazará automáticamente o no.
		*/
		public bool autoScroll = false;
		
		/**
		*	@brief permite saber si el scroll se está moviendo o no.
		*/
		public bool estadoBajando = false;
		
		/**
		*	@brief define si se oculta el scroll o no.
		*/
		public bool hideVerticalScrollbar = false;

		//changes made in the multimedias
		/**
		*	@brief Define el cambio hecho en multimedia.
		*/
		private float vSbarValue;

		/**
		*	@brief define sí la imagen se debe ajustar al EthScroll.
		*/
		private bool _shouldAdjustImage = false;
		

		/**
		*	@brief constructor de la clase EthScroll.
		*
		*	este método permite crear una instancia única de la clase EthScroll, usada para desplazarse por una ventana, una lista o un combo box.
		*
		*	@param args parametros con los cuales se creará la clase.
		*	@param parentGUI 
		*/
		public EthScroll (EthArguments args, EthComponentManager parentGUI) : base(args, parentGUI) {
			
			gui = new EthComponentManager (parentGUI);

			scrollPosition = new Vector2 (0, 0);

			if ( args ["img"] != null ) {
				string nomBot = args ["img"];			
				textura = Resources.Load (nomBot) as Texture2D;
				widControllers = textura.width;

				texturaBot = Resources.Load (nomBot + "_button") as Texture2D;		
				texturaUp = Resources.Load (nomBot + "_up") as Texture2D;		
				texturaDown = Resources.Load (nomBot + "_down") as Texture2D;
			}	

			if(args["adjustImage"]=="true") { 
				_shouldAdjustImage = true;
			}
		}

		/**
		*	@brief Muestra el scroll en pantalla.
		*
		*	Este método permite mostrar el GuiStyle del scroll en la pantalla.
		*
		*	@param offSet vector2 que da las cordenadas para ubicar el objeto en un punto específico de la pantalla.
		*/
        public override void Draw(Vector2? offset = null)
        {

            if (!Visible)
            {
                return;
            }

            Vector2 offset2 = offset ?? Vector2.zero;
            float xTemp = X + offset2.x;
            float yTemp = Y + offset2.y;

            Rect rctScroll = new Rect((xTemp * gui.WRatio) + gui.WOffset, (yTemp * gui.HRatio) + gui.HOffset, Wid * gui.WRatio, Hei * gui.HRatio);
            if (Input.GetMouseButtonDown(0))
            {
                if (gui.GuiParent.ShouldUseGroup)
                {
                    float value = rctScroll.x + (gui.GuiParent.WRatio * gui.GuiParent.X + gui.GuiParent.GuiParent.WOffset);
                    if (Input.mousePosition.x > value && Input.mousePosition.x < (rctScroll.width + value))
                    {
                        if ((Screen.height - Input.mousePosition.y) > rctScroll.y && (Screen.height - Input.mousePosition.y) < (rctScroll.height + rctScroll.y))
                        {
                            estadoBajando = false;
                            inDrag = true;
                            dragPosY = Input.mousePosition.y;
                        }
                    }
                }
                else
                {
                    if (Input.mousePosition.x > rctScroll.x - (offset2.x * gui.WRatio) && Input.mousePosition.x < (rctScroll.width + rctScroll.x - widControllers) - (offset2.x * gui.WRatio))
                    {
                        if ((Screen.height - Input.mousePosition.y) > rctScroll.y && (Screen.height - Input.mousePosition.y) < (rctScroll.height + rctScroll.y))
                        {
                            estadoBajando = false;
                            inDrag = true;
                            dragPosY = Input.mousePosition.y;
                        }
                    }
                }
            }

            /*lo movi para abajo pero no se si dañe algo, esto se hizo para que no haga clic cuando se esta arrastrando*/
            /*if(Input.GetMouseButtonUp(0)) {

                if(inDrag){
                    Debug.Log("Event.current: " +Event.current);
                    finishScrollDrag();
                }
            }//*/

            if (Input.GetMouseButton(0))
            {
                if (inDrag)
                {

                    float posAct = (Input.mousePosition.y);
                    if ((dragPosY - posAct) != 0)
                    {
                        if (disableChildrenWhileDrag)
                        {
                            this.gui.Enable = false;
                        }

                        addScrollDrag(new Vector2(0, -(dragPosY - posAct)));
                        //Debug.Log("esto: " + -(dragPosY-posAct));	
                    }
                    else
                    {
                        this.gui.Enable = true;
                    }
                }
            }

            GUISkin bkSkin = GUI.skin;
            GUISkin customSkin = GUI.skin;
            //change
            customSkin.verticalScrollbar = new GUIStyle();
            customSkin.verticalScrollbarThumb = new GUIStyle();
            customSkin.verticalScrollbarUpButton = new GUIStyle();
            customSkin.verticalScrollbarDownButton = new GUIStyle();

            if (inDrag)
            {
                scrollPositionTemp = scrollPosition + addScrollPosition;
                scrollPositionTemp = GUI.BeginScrollView(new Rect((xTemp * gui.WRatio) + gui.WOffset, (yTemp * gui.HRatio) + gui.HOffset, Wid * gui.WRatio, Hei * gui.HRatio), scrollPositionTemp, new Rect(gui.WOffset, 0, (Wid * gui.WRatio) - gui.WOffset, gui.MaxY), false, false);
                vSbarValue = scrollPositionTemp.y;
            }
            else
            {
                if (estadoBajando)
                {
                    scrollPosition.y = gui.MaxY - (Hei * gui.HRatio);
                    vSbarValue = scrollPosition.y;
                }

                scrollPosition = GUI.BeginScrollView(new Rect((xTemp * gui.WRatio) + gui.WOffset, (yTemp * gui.HRatio) + gui.HOffset, Wid * gui.WRatio, Hei * gui.HRatio), scrollPosition, new Rect(gui.WOffset, 0, (Wid * gui.WRatio) - gui.WOffset, gui.MaxY), false, false);

                if (gui.MaxY > Hei * gui.HRatio && autoScroll)
                {
                    if (Math.Ceiling(scrollPosition.y + (Hei * gui.HRatio)) >= Math.Ceiling(gui.MaxY))
                    {
                        estadoBajando = true;
                    }
                    else
                    {
                        estadoBajando = false;
                    }
                }
            }

            gui.Draw();
            GUI.EndScrollView();

            customSkin.horizontalScrollbar = new GUIStyle();

            if (textura != null)
            {
                customSkin.verticalScrollbar.normal.background = textura;
                //customSkin.verticalScrollbar.fixedWidth = textura.width;
            }

            if (texturaBot)
            {
                customSkin.verticalScrollbarThumb.normal.background = texturaBot;
                //customSkin.verticalScrollbarThumb.fixedWidth = texturaBot.width;				
                customSkin.verticalScrollbarThumb.fixedHeight = texturaBot.height;
            }

            if (texturaUp != null)
            {
                customSkin.verticalScrollbarUpButton.normal.background = texturaUp;
                //customSkin.verticalScrollbarUpButton.fixedWidth = texturaUp.width;
                customSkin.verticalScrollbarUpButton.fixedHeight = texturaUp.height;

                customSkin.verticalScrollbarDownButton.normal.background = texturaDown;
                //customSkin.verticalScrollbarDownButton.fixedWidth = texturaDown.width;
                customSkin.verticalScrollbarDownButton.fixedHeight = texturaDown.height;
            }

            if (_shouldAdjustImage)
            {
                if (gui.MaxY - Hei * gui.HRatio > 0)
                {
                    if (textura != null)
                    {
                        vSbarValue = GUI.VerticalScrollbar(new Rect((xTemp * gui.WRatio) + gui.WOffset + Wid * gui.WRatio - (textura.width / 2) * gui.WRatio, (yTemp * gui.HRatio) + gui.HOffset, (textura.width / 2) * gui.WRatio, Hei * gui.HRatio), vSbarValue, 1.0F, 0.0F, gui.MaxY - Hei * gui.HRatio);
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") != 0)
                    {
                        vSbarValue = scrollPosition.y;
                    }
                    else
                    {
                        scrollPosition.y = vSbarValue;
                    }
                }

                //cada frame puede entrar varias veces aca, entonces se toma solo el evento cuando es used
                if (Event.current.type == EventType.MouseUp || Event.current.type == EventType.used)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (inDrag)
                        {
                            finishScrollDrag();
                        }
                    }
                }

                GUI.skin = bkSkin;
            }
        }

		/**
		*	@brief Método para agregar el arrastre al scroll.
		*
		*	@param add vector2 que da las cordenadas para ubicar el objeto en un punto específico de la pantalla.
		*/
		public void addScrollDrag (Vector2 add) {
			addScrollPosition = add;
		}

		/**
		*	@brief Método para finalizar el arrastre del scroll.
		*/
		public void finishScrollDrag () {

			inDrag = false;
			
			if ( scrollPositionTemp.y < scrollPosition.y + addScrollPosition.y ) {
				float valueDes = (scrollPosition.y + addScrollPosition.y) - scrollPositionTemp.y;
				addScrollPosition.y = addScrollPosition.y - valueDes; 
			} else if ( scrollPosition.y + addScrollPosition.y < 0 ) {
				float valueDes = (scrollPosition.y + addScrollPosition.y);
				addScrollPosition.y = addScrollPosition.y - valueDes;
			}

			scrollPosition = scrollPosition + addScrollPosition;
			addScrollPosition = new Vector2 (0, 0);

			this.gui.Enable = true;
		}

		/**
		*	@brief Método para eliminar todos los elementos del EthComponentManager (gui) y reiniciar la posición del scroll.
		*/
		public void reinit () {
			gui.RemoveAllComponents ();
			setScrollPosition (new Vector2 (0, 0));
		}

		/**
		*	@brief Método para modificar el vector de nueva posición del EthScroll.
		*
		*	@param newPos Vector de nueva posición del EthScroll.
		*/
		public void setScrollPosition (Vector2 newPos) {
			scrollPosition = newPos;
		}

		/**
		*	@brief Método para acceder al vector posicion del EthScroll (scrollPosition).
		*
		*	@return El vector posicion del EthScroll (scrollPosition).
		*/
		public Vector2 getScrollPosition( ) {
			return scrollPosition;
		}

		/**
		*	@brief Método para modificar la variable autoScroll, ademas la variable estadoBajando queda con el mismo valor.
		*/
		public void setAutoScroll (bool autoScroll) {
			this.autoScroll = autoScroll;
			estadoBajando = autoScroll;
		}

		/**
		*	@brief metodo toString de la clase.
		*
		*	Retorna el nombre de la clase como una cadena de caracteres.
		*
		*	@return la clase representada en un string.
		*/
		public override string ToString () {
			return "EthScroll (" + Name + ")";
		}
	}
}
