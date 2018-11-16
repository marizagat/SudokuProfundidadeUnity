using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Timers;
using System.Threading;

public class Algoritmo : MonoBehaviour {
	public Text myText;
	
	double interval = 1500.0;
	
	int[,] lista = new int [81, 20];
	int[,] numeros_possiveis = new int [81, 9];
	string minhaString;
	int[] tabuleiro = new int[81]
	{
		5,0,4,0,7,0,9,0,2,
		6,7,2,1,9,5,3,4,8,
		1,9,8,3,4,2,5,6,7,
		8,5,9,7,6,1,4,2,3,
		4,2,6,8,5,3,7,9,1,
		7,1,3,9,2,4,8,5,6,
		9,6,1,0,0,0,2,8,4,
		2,8,7,0,0,0,6,3,5,
		0,0,0,0,0,0,0,0,0
	};
	
	void gerar_lista()
	{
		int linha = 1;
		int coluna = 1;
		int contador = 0;
		for(int i = 0; i < 81; i++)
		{
			for(int j = i - coluna + 1; j < i + 10 - coluna; j++)
			{
				if(j != i)
					lista[i,contador++] = j;
			}
			for(int j = coluna - 1; j < 81; j+=9)
			{
				if(j != i)
					lista[i,contador++] = j;
			}
			if(coluna == 1 || coluna == 4 || coluna == 7)
			{
				if(linha == 1 || linha == 4 || linha == 7)
				{
					lista[i,16] = i + 10;
					lista[i,17] = i + 11;
					lista[i,18] = i + 19;
					lista[i,19] = i + 20;
				}
				else if(linha == 2 || linha == 5 || linha == 8)
				{
					lista[i,16] = i - 8;
					lista[i,17] = i - 7;
					lista[i,18] = i + 10;
					lista[i,19] = i + 11;
				}
				else
				{
					lista[i,16] = i - 17;
					lista[i,17] = i - 16;
					lista[i,18] = i - 8;
					lista[i,19] = i - 7;
				}
			}
			else if(coluna == 2 || coluna == 5 || coluna == 8)
			{
				if(linha == 1 || linha == 4 || linha == 7)
				{
					lista[i,16] = i + 8;
					lista[i,17] = i + 10;
					lista[i,18] = i + 17;
					lista[i,19] = i + 19;
				}
				else if(linha == 2 || linha == 5 || linha == 8)
				{
					lista[i,16] = i - 10;
					lista[i,17] = i - 8;
					lista[i,18] = i + 8;
					lista[i,19] = i + 10;
				}
				else
				{
					lista[i,16] = i - 19;
					lista[i,17] = i - 17;
					lista[i,18] = i - 10;
					lista[i,19] = i - 8;
				}
			}
			else
			{
				if(linha == 1 || linha == 4 || linha == 7)
				{
					lista[i,16] = i + 7;
					lista[i,17] = i + 8;
					lista[i,18] = i + 16;
					lista[i,19] = i + 17;
				}
				else if(linha == 2 || linha == 5 || linha == 8)
				{
					lista[i,16] = i - 11;
					lista[i,17] = i - 10;
					lista[i,18] = i + 7;
					lista[i,19] = i + 8;
				}
				else
				{
					lista[i,16] = i - 20;
					lista[i,17] = i - 19;
					lista[i,18] = i - 11;
					lista[i,19] = i - 10;
				}
			}

			if(++coluna == 10)
			{
				coluna = 1;
				linha++;
			}
			contador = 0;
		}
	}
	
	void imprimir()
	{
		minhaString = "";
		for(int i = 0; i < 81; i+=9)
		{
			for(int j = 0; j < 9; j++)
			{
				if(tabuleiro[i+j] != 0)
					minhaString = minhaString + tabuleiro[i+j] + " ";
				else
					minhaString = minhaString + tabuleiro[i+j] + "  ";
			}
		}
		while (!(Input.GetKeyDown(KeyCode.Space)))
        {
            Debug.Log("Space key was not pressed.");
        }
		Debug.Log("Space key was pressed.");
		
		myText.text = minhaString;
	}
	
	int pop_numeros_possiveis(int posicao)
	{
		int aux, i;
		for(i = 0; i < 9; i++)
		{
			if(numeros_possiveis[posicao,i] == 0)
			{
				aux = numeros_possiveis[posicao,i-1];
				numeros_possiveis[posicao,i-1] = 0;
				return aux;
			}
		}
		aux = numeros_possiveis[posicao,i-1];
		numeros_possiveis[posicao,i-1] = 0;
		return aux;
	}
	
	int preencher_tabuleiro(int posicao)
	{
		if (posicao == 81)
			return 1;
		if (posicao == -1)
		{
			return 0;
		}
		if(numeros_possiveis[posicao,0] > -1)
		{
			tabuleiro[posicao] = pop_numeros_possiveis(posicao);
			if(tabuleiro[posicao] != 0)
			{
				if(verificar(posicao) != 0)
				{
					Invoke("imprimir",1f);
					return preencher_tabuleiro(posicao+1);
				}
				else
				{
					Invoke("imprimir",1f);
					return preencher_tabuleiro(posicao);
				}
			}
			else
			{
				for(int i = 0; i < 9; i++)
					numeros_possiveis[posicao,i] = i + 1;
				tabuleiro[posicao] = 0;
				Invoke("imprimir",1f);
				return preencher_tabuleiro(posicao-1);
			}
		}
		else
		{
			Invoke("imprimir",1f);
			return preencher_tabuleiro(posicao+1);
		}
	}
	
	int verificar(int posicao)
	{
		for(int i = 0; i < 20; i++)
		{
			if (tabuleiro[posicao] == tabuleiro[lista[posicao,i]])
			{
				return 0;
			}
		}
		return 1;
	}
	
	// Use this for initialization
	void Start () {
		gerar_lista();

		for(int i = 0; i < 81; i++)
		{
			if(tabuleiro[i] == 0)
				for(int j = 0; j < 9; j++)
					numeros_possiveis[i,j] = j + 1;
			else
				numeros_possiveis[i,0] = -1;
		}

		
		minhaString = "";
		for(int i = 0; i < 81; i+=9)
		{
			for(int j = 0; j < 9; j++)
			{
				if(tabuleiro[i+j] != 0)
					minhaString = minhaString + tabuleiro[i+j] + " ";
				else
					minhaString = minhaString + "  ";
			}
		}
		myText.text = minhaString;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	/*IEnumerator ImprimirTela()
    {
        yield return new WaitForSeconds(5);
		myText.text = minhaString;
    }*/
	
	public void SortearTabuleiro()
	{
		int aux;
		int flag;
		System.Random random = new System.Random();
		for (int i = 0; i < 81; i++)
		{
			aux = random.Next(-15, 9); 
			print(aux);
			if(aux > 0){
				tabuleiro[i] = aux;
				flag = verificar(i);
				if(flag == 0)
					i--;
			}
			else
				tabuleiro[i] = 0;
		}
		minhaString = "";
		for(int i = 0; i < 81; i+=9)
		{
			for(int j = 0; j < 9; j++)
			{
				if(tabuleiro[i+j] != 0)
					minhaString = minhaString + tabuleiro[i+j] + " ";
				else
					minhaString = minhaString + "  ";
			}
		}
		myText.text = minhaString;
	}
	
	public void Iniciar()
	{
		if(preencher_tabuleiro(0) != 0)
			imprimir();
	}
}
