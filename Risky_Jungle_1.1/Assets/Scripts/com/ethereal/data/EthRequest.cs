using UnityEngine;
using Boomlagoon.JSON;
using System;
using System.Text;
/** 
* @author    Carlos Andres Carvajal <ccarvajal@etherealgf.com> 
* @version   1.0 
* @date      Marzo 27 2014
*/
public class EthRequest{

	private int _id;
	private string _user;
	private string _object;
	private string _idFB;

    /**
    *  @brief Constructor de la clase
    */
    public EthRequest( int id, string user, string objectRequest, string idFB ){
        this.Id = id;
        this.User = user;
        this.ObjectRequest = objectRequest;
        this.IdRequestFB = idFB;
        
    }

	public int Id {
		get {
			return _id;
		}
		set {
			_id = value;
		}
	}

	public string User {
		get {
			return _user;
		}
		set {
			_user = value;
		}
	}

	public string ObjectRequest {
		get {
			return _object;
		}
		set {
			_object = value;
		}
	}

	public string IdRequestFB {
		get {
			return _idFB;
		}
		set {
			_idFB = value;
		}
	}
}
