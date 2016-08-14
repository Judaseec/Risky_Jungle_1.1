using UnityEngine;
using Boomlagoon.JSON;
using System;
using System.Text;
/** 
* @author    Carlos Andres Carvajal <ccarvajal@etherealgf.com> 
* @version   1.0 
* @date      Marzo 27 2014
*/
public class EthItem{

	private string _id;
	private string _name;
	private string _description;
	private string _urlImage;

	private int _count;
	private int _maxCount;
	private int _initialCount;

	private bool _isAutoRefillable;//se auto recarga con tiempo
	private int _timeRefill; //tiempo en segundos con el que se recarga
	private bool _showLocalNotification; //si muestra notificacion local cuando se carga todo
	private DateTime _lastDate;//ultima fecha en que se recargo o se disminuyo el maximo

	private double _secondsDelta;
	private ITimeItemFinished _callback;
	private bool _isRefilling;
	private int _countToAdd = 1;

    /**
    *  @brief Constructor de la clase
    */
    public EthItem( string id, string name, string desc, int maxCount, int initCount = 0, string img = "",
                    bool isAutoRefillable = false, int timeRefill = 1800, bool showLN = true, DateTime? lastDate = null ){
        this.Id = id;
        this.Name = name;
        this.Description = desc;
        this.UrlImage = img;
        this.MaxCount = maxCount;
        this.InitialCount = initCount;
        this.Count = initCount;
        this.IsAutoRefillable = isAutoRefillable;
        this.TimeRefill = timeRefill;
        this.ShowLocalNotification = showLN;
        this._isRefilling = false;
        DateTime lastTemp = lastDate ?? DateTime.Now;
		this.LastDate = lastTemp;

        CheckItemRefilling();
        
    }

    private void CheckItemRefilling(){
        if ( IsAutoRefillable ){
        	if ( Count < MaxCount ){
	        	this._isRefilling = true;
				CheckTime();
			}
        }
    }


    /**
    *  @brief Constructor de la clase
    */
    public EthItem(  ){
    }

	public string Id {
		get {
			return _id;
		}
		set {
			_id = value;
		}
	}

	public string Name {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public string Description {
		get {
			return _description;
		}
		set {
			_description = value;
		}
	}

	public string UrlImage {
		get {
			return _urlImage;
		}
		set {
			_urlImage = value;
		}
	}

	public int Count {
		get {
			return _count;
		}
		set {
			_count = value;
			
			if ( _count < MaxCount && ! _isRefilling){
				this.LastDate = DateTime.Now;				 
				_isRefilling = true;
			}
		}
	}

	public int MaxCount {
		get {
			return _maxCount;
		}
		set {
			_maxCount = value;
		}
	}

	public int InitialCount {
		get {
			return _initialCount;
		}
		set {
			_initialCount = value;
		}
	}

	public bool IsAutoRefillable {
		get {
			return _isAutoRefillable;
		}
		set {
			_isAutoRefillable = value;
		}
	}

	public int TimeRefill {
		get {
			return _timeRefill;
		}
		set {
			_timeRefill = value;
		}
	}

	public bool ShowLocalNotification {
		get {
			return _showLocalNotification;
		}
		set {
			_showLocalNotification = value;
		}
	}

	public int SecondsDelta {
		get {
			return (int)_secondsDelta;
		}
	}

	public DateTime LastDate {
		get {
			return _lastDate;
		}
		set {
			_lastDate = value;
		}
	}

	public ITimeItemFinished Callback{
		get {
			return _callback;
		}
		set {
			_callback = value;
		}
	}   

	public bool CheckTime( ){
		//agrega los segundos
		if ( Count < MaxCount && _isRefilling ){
			DateTime _currentDate = DateTime.Now;
			_secondsDelta = (_currentDate - _lastDate ).TotalSeconds;
			
			int intervals = (int)(_secondsDelta / _timeRefill);
			if ( intervals > 0 ){
				Count += (_countToAdd*intervals);
				this.LastDate = DateTime.Now;

				if ( Count >= MaxCount ){//se puede pasar al set
					Count = MaxCount;
					_isRefilling = false;

					if ( ShowLocalNotification ){
						//TODO mostrar notificacion local
					}
				}

				if ( _callback != null ){
					_callback.OnTimeFinished( this );
				}
				return true;
			}
		}

		return false;
	}

    public static EthItem ParseJson( string jsonString ){
        JSONObjectBoom json = JSONObjectBoom.Parse( jsonString );

        EthItem item = new EthItem();

        item.Id = json.GetString("id");
        item.Name = json.GetString("name");
        item.Description = json.GetString("desc");
        item.UrlImage = json.GetString("url");
        item.Count = (int)json.GetNumber("count");
        item.MaxCount = (int)json.GetNumber("max");
        item.InitialCount = (int)json.GetNumber("init");
        item.IsAutoRefillable = json.GetBoolean("auto");
        item.TimeRefill = (int)json.GetNumber("time");
        item.ShowLocalNotification = json.GetBoolean("showln");
        item.LastDate = DateTime.FromBinary( Convert.ToInt64( json.GetString("lastdate") ) );

        item.CheckItemRefilling();

        //Debug.Log("ahora: "+ item.LastDate);
        return item;
    }

    public string ToJson(){
        StringBuilder sb = new StringBuilder( "{");
        sb.Append("\"id\":\"" + Id + "\",");
        sb.Append("\"name\":\"" + Name + "\",");
        sb.Append("\"desc\":\"" + Description + "\",");
        sb.Append("\"url\":\"" + UrlImage + "\",");
        sb.Append("\"count\":" + Count + ",");
        sb.Append("\"max\":" + MaxCount + ",");
        sb.Append("\"init\":" + InitialCount + ",");
        sb.Append( ("\"auto\":" + IsAutoRefillable + ",").ToLower() );
        sb.Append("\"time\":" + TimeRefill + ",");
        sb.Append( ("\"showln\":" + ShowLocalNotification + ",").ToLower() );
        sb.Append("\"lastdate\":\"" + LastDate.ToBinary() + "\"}");

        //Debug.Log("era: "+ LastDate);
       // Debug.Log("json: ");

        return sb.ToString();
    }

    /**
     * Obtiene los segundos que faltan para que se llene del todo el item
     */
    public int GetSecondsToFill() {
    	//primero el tiempo que falta del actual, +, la cantidad que falta para completar
		int leftTime = (TimeRefill - SecondsDelta) + ( ( MaxCount - ( Count + 1 ) ) * TimeRefill);
		return leftTime;
	}
}
