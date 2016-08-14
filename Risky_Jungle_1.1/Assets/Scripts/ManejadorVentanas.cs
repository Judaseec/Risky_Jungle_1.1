using UnityEngine;


using com.ethereal.display.components;


using System.Collections.Generic;
using System;

public class ManejadorVentanas { 

	public static ManejadorVentanas instance;

	public Assets.Scripts.com.ethereal.display.EthGUIScreen inicial;
	public Assets.Scripts.com.ethereal.display.EthGUIScreen currentWindow;
	public bool returnMode = false;

	/*
	Estas dos variables se utilizan para cuando se carga una nueva escena, se coloca en true chargingNewScene, indicando
	que se esta cargando una escena, en ese momento se guarda la escena actual en la variable lastScene y se guarda el script
	que se esta usando en esa escena como lastWindowScript, cuando yo doy returnWindow() el carga la ultima escena y al terminarla
	de cargar le carga el script correspondiente (lastWindowScript), igualmente cuando yo cargo una escena nueva, podria indicar 
	que al cargarla cargue un script X, que es guardado en scriptToLoad para colocarlo cuando la nueva escena termine de cargar
	*/
	public bool chargingNewScene = false;
	public string scriptToLoad = "";
	

	string tempRutaEscena;
	string tempScriptToLoad;
	string tempRutaVentana;

	public bool OnLevelWasLoadedInvocado = false;

	public List<WindowCharged> arrWindows = new List<WindowCharged>();

	
	private ManejadorVentanas()
	{
		initData();
		Assets.Scripts.com.ethereal.display.components.EthDialog.setDefaultData("img:screens/general/back_dialog,font:fonts/Futura,fontColor:1_1_1,fontSize:21,style:bold,fontBtn:fonts/Janda,fontColorBtn:1_1_1,fontSizeBtn:20,offsetButtonsY:-15,offsetButtonsX:12,widBtn:77,heiBtn:77,useBotText:false");
		Assets.Scripts.com.ethereal.display.components.EthDialog.addDefaultDialog("OK","bot:ok,imgok:screens/general/botOk");
		Assets.Scripts.com.ethereal.display.components.EthDialog.addDefaultDialog("OK_CANCEL","bot:ok;cancel,imgok:screens/general/botOk,imgcancel:screens/general/botCancel,sepBtn:100");
	}

	public void showOkDialog(string title,string message) {
		showOkDialog(title,message,"",null);
	}

	public void showOkDialog(string title,string message,Assets.Scripts.com.ethereal.display.components.EthDialog.OnClickEvent fn) {
		showOkDialog(title,message,"",fn);
	}

	public void showOkDialog(string title,string message,string parMessage,Assets.Scripts.com.ethereal.display.components.EthDialog.OnClickEvent fn) {
		parMessage = "offsetText:0_20," + parMessage;
		//Debug.Log("Current es " + getCurrent(getRealLastScriptName()).name  + " , " +  Application.loadedLevelName  + " , " + getCurrent(getRealLastScriptName()).GetType().ToString() + " , " + getCurrent(getRealLastScriptName()).gui  + " , " + getRealLastScriptName() );
		Assets.Scripts.com.ethereal.display.components.EthDialog dialogAct = Assets.Scripts.com.ethereal.display.components.EthDialog.useDialog(getCurrent(getRealLastScriptName()).gui,"OK",message,parMessage);	
		dialogAct.getGUI().AddTexture("Textu",205,160,"img:screens/general/adornoTitulo");
		dialogAct.getGUI().AddLabel(title,335,140,"font:fonts/Janda,fontColor:1.0_0.87_0.0,fontSize:28,useLang:true,decorateColor:0.32_0.28_0.0");
		if(fn!=null) dialogAct.setFunction(fn);
	}

	public void showOkCancelDialog(string title,string message,bool useLangTitle,bool useLangMessage,Assets.Scripts.com.ethereal.display.components.EthDialog.OnClickEvent fn, string parMessage = "") {
		//Debug.Log("Current es " + getCurrent(getRealLastScriptName()) + " , " +  Application.loadedLevelName   + " , " +  getRealLastScriptName() );		
		//Debug.Log("Current es " + getCurrent(getRealLastScriptName()).name  + " , " +  Application.loadedLevelName  + " , " + getCurrent(getRealLastScriptName()).GetType().ToString() + " , " + getCurrent(getRealLastScriptName()).gui  + " , " + getRealLastScriptName() );		
		Assets.Scripts.com.ethereal.display.components.EthDialog dialogAct = Assets.Scripts.com.ethereal.display.components.EthDialog.useDialog(getCurrent(getRealLastScriptName()).gui,"OK_CANCEL",message,"useLang:"+(""+useLangMessage).ToLower()+",offsetText:0_20,offsetButtonsY:-15,offsetButtonsX:12" + parMessage);	
		dialogAct.getGUI().AddTexture("Textu",205,160,"img:screens/general/adornoTitulo");
		dialogAct.getGUI().AddLabel(title,335,140,"font:fonts/Janda,fontColor:1.0_0.87_0.0,fontSize:28,useLang:"+(""+useLangTitle).ToLower()+",decorateColor:0.32_0.28_0.0");
		if(fn!=null) dialogAct.setFunction(fn);			
	}

	public Assets.Scripts.com.ethereal.display.components.EthComponentManager showCompraOkCancelDialog(string title,string title2,string message,string item, bool useLangTitle,bool useLangMessage,bool showBackItem,Assets.Scripts.com.ethereal.display.components.EthDialog.OnClickEvent fn) {
		Assets.Scripts.com.ethereal.display.components.EthDialog dialogAct = Assets.Scripts.com.ethereal.display.components.EthDialog.useDialog(getCurrent(getRealLastScriptName()).gui,"OK_CANCEL","","useLang:false,offsetButtonsY:0,offsetButtonsX:25");	
		dialogAct.getGUI().AddTexture("CuadroFondo",312,175,"img:screens/agilizarProduccion/popUp/fondoTexto");
		dialogAct.getGUI().AddLabel(title2 + ":",325,160,"font:fonts/Futura,fontColor:1_1_1,fontSize:17,useLang:"+(""+useLangMessage).ToLower()+",w:180,wrap:true,align:UpperLeft");
		dialogAct.getGUI().AddLabel(message,325,195,"font:fonts/Futura,fontColor:1_1_1,fontSize:16,useLang:"+(""+useLangMessage).ToLower()+",w:180,wrap:true,align:UpperLeft");
		if(showBackItem) {
			dialogAct.getGUI().AddTexture("Cuadro",205,170,"img:screens/agilizarProduccion/item_popUp");
			if(item!=null)dialogAct.getGUI().AddTexture("Item",221,186,"img:"+item);
		}
		else {
			if(item!=null)dialogAct.getGUI().AddTexture("Item",221,186,"img:"+item+",h:102,w:88");
		}
		dialogAct.getGUI().AddTexture("Textu",205,140,"img:screens/general/adornoTitulo");
		dialogAct.getGUI().AddLabel(title,335,130,"font:fonts/Janda,fontColor:1.0_0.87_0.0,fontSize:28,useLang:"+(""+useLangTitle).ToLower()+",decorateColor:0.32_0.28_0.0");
		if(fn!=null) dialogAct.setFunction(fn);		

		return dialogAct.getGUI();
	}
	

	public static ManejadorVentanas getInstance(){
		if ( ManejadorVentanas.instance == null ){
			ManejadorVentanas.instance = new ManejadorVentanas();	
		}

		//ManejadorVentanas.instance.lastScene = Application.loadedLevelName;
		
		return ManejadorVentanas.instance;
	}

	public void initData() {
		//Aca se pueden inicializar todas las cosas que se necesiten para el ambiente en general	

		//Obtiene el lenguaje
		//string lenguaje = MFCData.getInstance(inicial).getDataGame(0);
		string lenguaje = "es";
		Assets.Scripts.com.ethereal.util.EthLang.ActiveLangs("lang/dict",lenguaje);
	}

	public void registrarPantallaInicial(Assets.Scripts.com.ethereal.display.EthGUIScreen inicial) {				
		this.inicial = inicial;
		currentWindow = inicial;
		inicial.CenterScreen(670,480);
		addCurrentToPipe();
	}

	public void addCurrentToPipe() {
		if(!returnMode) {
			WindowCharged wAct = new WindowCharged(Application.loadedLevelName,getCurrent().GetType().ToString());
			arrWindows.Add(wAct); 			
		}
	}

	public void addCurrentToPipe(string scriptName) {
		if(!returnMode) {
			WindowCharged wAct = new WindowCharged(Application.loadedLevelName,scriptName);
			arrWindows.Add(wAct); 
		}
	}

	public void addCurrentToPipe(string sceneName, string scriptName) {
		if(!returnMode) {
			WindowCharged wAct = new WindowCharged(sceneName,scriptName);
			arrWindows.Add(wAct); 
		}
	}

	public void cargarVentana(string rutaVentana) {
		tempRutaVentana = rutaVentana;

		if(currentWindow==null) { 
			currentWindow=getCurrent();
		}

		LoaderScreen.getInstance(currentWindow.gui).showLoader();
		new Assets.Scripts.com.ethereal.util.EthTimer( 200, cargarVentanaInt );	
	}

	public void cargarVentanaInt(object obj) {

		if(arrWindows.Count==0) {
			registrarPantallaInicial(getCurrent());
		}

		string rutaVentana = tempRutaVentana;
		RemoveScripts();
		// UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(Camera.main.gameObject, "Assets/Scripts/ManejadorVentanas.cs (158,3)", rutaVentana);
		currentWindow = getCurrent(rutaVentana);
		addCurrentToPipe(rutaVentana);		
	}

	public void cargarEscena(string rutaEscena) {
		cargarEscena(rutaEscena,"");
	}

	public void cargarEscena(string rutaEscena,string scriptToLoad) {
		tempRutaEscena = rutaEscena;
		tempScriptToLoad = scriptToLoad;

		if(currentWindow==null) {
			currentWindow=getCurrent();
		}

		//Debug.Log("El gui es " + currentWindow.gui + " de " + currentWindow);
		//Debug.Log("Hola aca");
		LoaderScreen.getInstance(currentWindow.gui).showLoader();
		new Assets.Scripts.com.ethereal.util.EthTimer( 200, cargarEscenaInt );			
	}

	public string getLastSceneName() { 
		if(arrWindows.Count<2) return "";
		return getLastWindow().loadedLevelNameAct;
	}

	public string getLastScriptName() { 
		return getLastWindow().currentScript;
	}

	public WindowCharged getLastWindow() {
		return arrWindows[arrWindows.Count-2];
	}

	public string getRealLastSceneName() { 
		return getRealLastWindow().loadedLevelNameAct;
	}

	public string getRealLastScriptName() { 
		return getRealLastWindow().currentScript;
	}

	public WindowCharged getRealLastWindow() {
		if(arrWindows.Count==0) {
			addCurrentToPipe();
		}
		
		return arrWindows[arrWindows.Count-1];
	}

	public void removeLastWindow() {
		arrWindows.RemoveAt(arrWindows.Count-1);
	}


	public void cargarEscenaInt(object obj) {

		string scriptToLoad = tempScriptToLoad;
		
		this.scriptToLoad = scriptToLoad;
		chargingNewScene = true;
		
		if(arrWindows.Count==0) {
			registrarPantallaInicial(getCurrent());
		}

		currentWindow.loadScene(tempRutaEscena);	
	}

	public void printPipe() {
		for(int i=0;i<arrWindows.Count;i++)
		{
			Debug.Log(arrWindows[i]);
		}
	}

	public void returnWindow() {

		returnMode = true;
		//printPipe();

		//Si es un cambio de escena
		if(Application.loadedLevelName!=getLastWindow().loadedLevelNameAct) {
			if(currentWindow==null) { //Debug.Log("Es null y obtendria " + getCurrent() );
				currentWindow=getCurrent();
			}

			LoaderScreen.getInstance(currentWindow.gui).showLoader();
			removeLastWindow();
			new Assets.Scripts.com.ethereal.util.EthTimer( 200, returnWindowInt );
		}
		else
		{
			cargarVentana(getLastWindow().currentScript);
			removeLastWindow();
			new Assets.Scripts.com.ethereal.util.EthTimer( 200, resetReturnMode );	
			//printPipe();
		}
	}

	public void returnWindowInt(object obj) {
		OnLevelWasLoadedInvocado = false;
		Application.LoadLevel( getRealLastSceneName() );
	}

	public void esceneLoaded() {

		currentWindow.Remove();
		OnLevelWasLoadedInvocado = true;		
		Debug.Log("Termino escena");
		//Si la que se acabo de cargar era una escena que queria cargar entra en este if
		//el caso contrario es cuando el hecho de que se carga una nueva escena es porque 
		//se invoco returnWindow();
		if(chargingNewScene) {
			if(scriptToLoad!="") {
				RemoveScripts();
				// UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(Camera.main.gameObject, "Assets/Scripts/ManejadorVentanas.cs (276,5)", scriptToLoad);
				addCurrentToPipe(scriptToLoad);
				scriptToLoad = "";
			}
			else 
			{
				if(returnMode)
				{
					//Debug.Log("Entro aca a la olla con " + getCurrent().GetType().ToString()  + " , " + getRealLastScriptName());
					//printPipe();

					if(getCurrent().GetType().ToString() != getRealLastScriptName() && getRealLastScriptName()!="") {
						RemoveScripts();
						// UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(Camera.main.gameObject, "Assets/Scripts/ManejadorVentanas.cs (289,7)", getRealLastScriptName());												
					}					
				}
				else
				{
					addCurrentToPipe();
				}
			}

			
		}
		else
		{
			//if(returnMode && lastWindowScript!="" && lastWindowScript!=null) {
			if(returnMode) {
				RemoveScripts();
				// UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(Camera.main.gameObject, "Assets/Scripts/ManejadorVentanas.cs (305,5)", getRealLastScriptName());					
			}		
		}

		currentWindow = getCurrent();	
		new Assets.Scripts.com.ethereal.util.EthTimer( 200, resetReturnMode );		
	}

	public void resetReturnMode(object obj) {
		returnMode = false;	
	}

	public bool isReturnMode() {
		return returnMode;
	}

	protected Assets.Scripts.com.ethereal.display.EthGUIScreen getCurrent(string name) {
		Component[] comps = Camera.main.GetComponents<Assets.Scripts.com.ethereal.display.EthGUIScreen>();

		for (int i = 0; i < comps.Length; ++i) {
	        Assets.Scripts.com.ethereal.display.EthGUIScreen com = (Assets.Scripts.com.ethereal.display.EthGUIScreen)comps[i];
	        if(Assets.Scripts.com.ethereal.util.Eth.GetClassType(com)==name) return com;
	    }

		return null;
	}

	protected Assets.Scripts.com.ethereal.display.EthGUIScreen getCurrent() {
		return Camera.main.GetComponent<Assets.Scripts.com.ethereal.display.EthGUIScreen>();
	}

	protected void RemoveScripts() {
		Component[] comps = Camera.main.GetComponents<Assets.Scripts.com.ethereal.display.EthGUIScreen>();

		for (int i = 0; i < comps.Length; ++i) {
	        Assets.Scripts.com.ethereal.display.EthGUIScreen com = (Assets.Scripts.com.ethereal.display.EthGUIScreen)comps[i];
	        //Si quiero implementar animaciones de salida, se podria aca crear por ejemplo
	        //removeWithAnimation o algo asi
	        com.Remove();
			//Debug.Log("Remueve a " + com);        
	        GameObject.Destroy(com);
	    }
	}
}

public class WindowCharged {

	public string currentScript;
	public string loadedLevelNameAct;

	public WindowCharged(string loadedLevelNameAct,string currentScript) {		
		this.currentScript = currentScript;
		this.loadedLevelNameAct = loadedLevelNameAct;
	}

	public override string ToString()
	{
		return "level " + loadedLevelNameAct  + " ,script " + currentScript;
	}
} 
