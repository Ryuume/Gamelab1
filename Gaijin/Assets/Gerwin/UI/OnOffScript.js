#pragma strict

var AanUit : boolean;
var Obj : GameObject;
var Obj2 : GameObject;

function Start () {
	AanUit = false;
}

function Update () {


}

function Toggle () {
	if(AanUit == false){
		Obj.SetActive(true);
		Obj2.SetActive(false);
		AanUit = true;
	}
	else if(AanUit == true){
			Obj.SetActive(false);
			Obj2.SetActive(true);
			AanUit = false;
	}
}