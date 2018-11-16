using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Botoes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void Sair(){
		Application.Quit();
	}
	
	public void Info(){
		SceneManager.LoadScene("Info");
	}
	
	public void Sudoku(){
		SceneManager.LoadScene("Sudoku");
	}

	public void Voltar(){
		SceneManager.LoadScene("Menu");
	}
}