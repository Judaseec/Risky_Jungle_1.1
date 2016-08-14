using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Assets.Scripts.com.ethereal.util;

/*
Ejemplo de uso:

Primero se crean cada uno de los contenidos de las pestañas, luego los toggle y luego se asignan al EthTabSheet

EthComponentManager ethComp1 = new EthComponentManager();		
ethComp1.AddButton("bot",500,100,"img:avatar/Funcionamiento/btn_principal,text:Bot1,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");

EthComponentManager ethComp2 = new EthComponentManager();		
ethComp2.AddButton("bot",500,150,"img:avatar/Funcionamiento/btn_principal,text:Bot2,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");

EthComponentManager ethComp3 = new EthComponentManager();		
ethComp3.AddButton("bot",500,200,"img:avatar/Funcionamiento/btn_principal,text:Bot3,font:fonts/Bayday,fontColor:1_1_1,fontSize:16,useLang:false");

EthToggleButton tog1 = gui.AddToggleButton("Tog1",100,10,"img:avatar/Funcionamiento/btn_principal,text:Tog1,font:fonts/Avenger,fontColor:1_1_1,fontSize:17,useLang:false");
EthToggleButton tog2 = gui.AddToggleButton("Tog2",100,60,"img:avatar/Funcionamiento/btn_principal,text:Tog1,font:fonts/Avenger,fontColor:1_1_1,fontSize:17,useLang:false");
EthToggleButton tog3 = gui.AddToggleButton("Tog3",100,110,"img:avatar/Funcionamiento/btn_principal,text:Tog1,font:fonts/Avenger,fontColor:1_1_1,fontSize:17,useLang:false");

EthTabSheet ethTab = gui.AddTabSheet("tabName");
ethTab.AddTab(tog1,ethComp1);
ethTab.AddTab(tog2,ethComp2);
ethTab.AddTab(tog3,ethComp3);
*/

namespace Assets.Scripts.com.ethereal.display.components
{

    /** 
    *   @author    EtherealGF <www.etherealgf.com> 
    *   @version   1.0 
    *   @date      Octubre 16 2014
    * 
    *   @class  EthTabSheet
    *   @brief  Esta clase se encarga de crear un sub-menú, el cual contiene EthToggleButtons y cada uno de ellos tiene asignado un
    *   EthComponentManager, el cual se muestra con sus componentes cuando el EthToggleButton es seleccionado.
    *
    */
    public class EthTabSheet : EthComponent
    {

        /**
        *   @brief Estilo del EthTabSheet.
        */
        protected GUIStyle guiBot;

        /**
        *   @brief Texto del EthTabSheet.
        */
        public string text;


        public EthButton botItem;
        public EthButton arrowItem;
        public EthScroll scrollItems;

        /**
        *   @brief ArrayList que contiene los ítems pertenecientes al EthTanSheet.
        */
        public ArrayList items;


        public List<EthButton> itemsBots;
        public bool itemsDesplegados = false;
        public string nomImg;

        /**
        *   @brief EthArguments del EthTanSheet.
        */
        public EthArguments args;


        public float altoItem;
        private int indexSelected;
        private object objSelected;

        /**
        *   @brief Grupo al que seran asignados los EthToggleButton del EthTabSheet.
        */
        private EthToggleButtonGroup togGroup;

        /**
        *   @brief Diccionario que contiene la relación entre los EthToggleButton y su respectivo EthComponentManager.
        */
        private Dictionary<EthToggleButton, EthComponentManager> dictTabs;

        /**
        *   @brief Cantidad de menús, es decir cantidad de EthToggleButton con su respectivo EthComponentManager.
        */
        private int cantTabs = 0;

        /**
        *	@brief Método para invocar el evento OnClick.
        */
        public delegate void OnClickEvent(object objCombo);

        /**
        *   @brief Evento para detectar cuando se hace click.
        */
        public event OnClickEvent OnClick;

        /**
        *	@brief constructor de la clase EthTabSheet.
        *
        *	Este método permite crear una instancia de la clase EthTabSheet.
        *
        *	@param args 		Parametros con los cuales se creará el objeto.
        *	@param parentGUI 	EthComponentManager al cual pertenecerá el objeto.
        */
        public EthTabSheet(EthArguments args, EthComponentManager parentGUI)
            : base(args, parentGUI)
        {
            this.args = args;
            items = new ArrayList();
            togGroup = new EthToggleButtonGroup();
            dictTabs = new Dictionary<EthToggleButton, EthComponentManager>();
        }

        /**
        *	@brief Este método agrega un ítem al submenú.
        *
        *	@param togBut 		Ítem al cual se le asignará un EthComponentManager.
        *	@param ethWindow	EthComponentManager que se mostrará al seleccionar el EthToggleButton correspondiente.
        */
        public void AddTab(EthToggleButton togBut, EthComponentManager ethWindow)
        {
            dictTabs[togBut] = ethWindow;
            ethWindow.CopyScreenValues(_gui);
            ethWindow.Visible = false;
            togBut.Name = "" + cantTabs;
            togBut.SetFunction(ClickTab);
            togGroup.AddToggleButton(togBut);
            cantTabs++;
            RefreshPosition();
        }

        /**
        *	@brief Este método refresca la posición de los ítems del submenú.
        */
        public void RefreshPosition()
        {
            foreach (KeyValuePair<EthToggleButton, EthComponentManager> kvp in dictTabs)
            {
                kvp.Key.ToBack();
            }
        }

        /**
        *	@brief Este método hace que se muestre un ítem especifico del submenú.
        *
        *	@param bot Nombre del EthToggleButton que está asociado al EthComponentManager a mostrar.
        */
        public void ClickTab(string bot)
        {
            foreach (KeyValuePair<EthToggleButton, EthComponentManager> kvp in dictTabs)
            {
                kvp.Value.Visible = (kvp.Key.Name == bot);

                if (kvp.Key.Name == bot)
                {
                    kvp.Key.ToFront();
                }
            }
        }

        /**
        *	@brief Método para dibujar una EthTabSheet.
        *
        *	@param offset Parámetro de tipo Vector2 para ser dibujado, el cual támbien puede ser nulo.
        *
        *	@see com.ethereal.display.components.EthComponent
        */
        public override void Draw(Vector2? offset = null)
        {
            if (!Visible)
            {
                return;
            }

            foreach (KeyValuePair<EthToggleButton, EthComponentManager> kvp in dictTabs)
            {
                kvp.Value.Draw(offset);
            }
        }

        /**
        *	@brief Método para suscribirse al evento OnClick.
        */
        public void SetFunction(OnClickEvent fn)
        {
            OnClick += fn;
        }

        /**
        *	@brief Método ToString que retorna el nombre de la clase.
        *
        *	@return Nombre de la clase, EthComboBox.
        */
        public override string ToString()
        {
            return "EthComboBox (" + Name + ")";
        }
    }
}
