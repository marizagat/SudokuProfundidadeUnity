using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Timers;
using System.Threading;

public class Algoritmo : MonoBehaviour {
	/*variáveis correspondentes aos textos exibidos na tela*/
	public Text myText;
	public Text myText2;
	public Text erro;
	
	/*variáveis utilizadas nas funções*/
	int back = 0;
	int contador = 0;
	int iteracao = 0;
	
	/*lista de adjacência*/
	int[,] lista = new int [81, 20];
	
	/*lista dos números possíveis*/
	int[,] numeros_possiveis = new int [81, 9];
	
	/*string auxiliar*/
	string minhaString;
	
	
	int[] tabuleiro = new int[81];
	
	/*função que gera a lista de adjacência*/
	void gerar_lista()
	{
		int linha = 1;
		int coluna = 1;
		int contador = 0;
		
		for(int i = 0; i < 81; i++)
		{
			/*vértices na mesma linha*/
			for(int j = i - coluna + 1; j < i + 10 - coluna; j++)
			{
				if(j != i)
					lista[i,contador++] = j;
			}
			
			/*vértices na mesma coluna*/
			for(int j = coluna - 1; j < 81; j+=9)
			{
				if(j != i)
					lista[i,contador++] = j;
			}
			
			/*vértices na mesma subgrade*/
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
	
	/*imprime o tabuleiro na tela*/
	void imprimir()
	{
		minhaString = "";
		/*pega todos os valores do tabuleiro*/
		for(int i = 0; i < 81; i+=9)
		{
			for(int j = 0; j < 9; j++)
			{
				if(tabuleiro[i+j] != 0)
					minhaString = minhaString + tabuleiro[i+j] + " ";
				else
					minhaString = minhaString + "_ ";
			}
		}
		/*seta o texto na tela*/
		myText.text = minhaString;
	}
	
	/*dá um pop na pilha de números possíveis para cada posição*/
	int pop_numeros_possiveis(int posicao)
	{
		int aux, i;
		if(numeros_possiveis[posicao,0] == 0)
			return 0;
		for(i = 0; i < 9; i++)
		{
			if(numeros_possiveis[posicao,i] == 0)
			{
				/*pega o último valor*/
				aux = numeros_possiveis[posicao,i-1];
				/*retira o valor da pilha*/
				numeros_possiveis[posicao,i-1] = 0;
				return aux;
			}
		}
		aux = numeros_possiveis[posicao,i-1];
		numeros_possiveis[posicao,i-1] = 0;
		return aux;
	}
	
	/*preenche o tabuleiro na posição que recebe como parâmetro*/
	int preencher_tabuleiro(int posicao)
	{
		print(posicao);
		/*se acabou*/
		if (posicao == 81)
			return -1;
		/*se voltou demais pois não há resolução*/
		if (posicao == -1)
		{
			return -2;
		}
		/*se não for um dos números do tabuleiro original*/
		if(numeros_possiveis[posicao,0] > -1)
		{
			/*coloca o número possível retirado da pilha*/
			tabuleiro[posicao] = pop_numeros_possiveis(posicao);
			/*se ainda havia alguma possibilidade de número*/
			if(tabuleiro[posicao] != 0)
			{
				/*se a resposta foi positiva para o número vai para o próximo*/
				if(verificar(posicao) != 0)
				{
					back = 0;
					return posicao+1;
				}
				/*se foi negativa tenta de novo*/
				else
				{
					back = 0;
					return posicao;
				}
			}
			/*se as possibilidades da pilha acabaram tenta o anterior*/
			else
			{
				for(int i = 0; i < 9; i++)
					numeros_possiveis[posicao,i] = i + 1;
				tabuleiro[posicao] = 0;
				back = 1;
				return posicao-1;
			}
		}
		/*se for um número do tabuleiro original*/
		else
		{
			/*se já tiver voltado volta mais*/
			if(back == 1){
				return posicao-1;
			}
			/*se tiver ido para a frente vai mais pra frente*/
			back = 0;
			return posicao+1;
		}
	}
	
	/*verifica se o valor inserido está correto em relação aos outros valores já estabelecidos*/
	int verificar(int posicao)
	{
		for(int i = 0; i < 20; i++)
		{
			/*se algum dos valores for igual retorna 0*/
			if (tabuleiro[posicao] == tabuleiro[lista[posicao,i]])
			{
				return 0;
			}
		}
		/*caso contrário retorna 1*/
		return 1;
	}
	
	/*função que roda na inicialização da cena no unity*/
	void Start () {
		
		/*chama a função que gera a lista de adjacência*/
		gerar_lista();
		
		/*sorteia um tabuleiro novo*/
		SortearTabuleiro();

		/*estabelece todos os números possíveis de 1 a 9*/
		for(int i = 0; i < 81; i++)
		{
			if(tabuleiro[i] == 0)
				for(int j = 0; j < 9; j++)
					numeros_possiveis[i,j] = j + 1;
			else
				numeros_possiveis[i,0] = -1;
		}
		
		/*imprime na tela o tabuleiro*/
		imprimir();
		/*imprime os valores do tabuleiro original em vermelho*/
		myText2.text = minhaString;
	}
	
	/*função original do unity que não é utilizada*/
	void Update () {
	}
	
	/*função que escolhe um tabuleiro dentro de algumas opções*/
	public void SortearTabuleiro()
	{
		iteracao = 0;
		back = 0;
		erro.text = " ";
		iteracao = 0;
		
		/*de 0 a 2: fácil*/
		if(contador == 0){
			tabuleiro = new int[]
			{
				4,3,5,0,0,9,7,8,1,
				0,0,2,5,7,1,4,0,3,
				1,9,7,8,3,4,0,6,2,
				8,2,6,1,9,5,3,4,7,
				3,7,0,0,8,2,0,1,5,
				9,5,1,7,4,3,6,2,8,
				5,1,9,3,2,6,8,7,4,
				2,4,8,9,5,7,1,3,0,
				0,6,0,4,0,8,2,5,9
			};
		}
		if(contador == 1){
			
			tabuleiro = new int[]
			{
				0,3,0,6,7,8,9,1,2,
				6,7,2,1,9,5,3,4,8,
				1,9,8,0,0,2,5,0,7,
				8,5,9,7,6,1,0,0,0,
				4,2,6,8,5,3,7,9,1,
				7,0,3,9,2,4,8,5,6,
				9,6,0,5,3,0,2,8,4,
				2,0,7,4,1,9,6,3,5,
				3,4,5,2,8,0,1,7,9
			};
			
		}
		if(contador == 2){
			tabuleiro = new int[]
			{
				0,5,7,4,8,0,6,2,0,
				6,0,9,5,2,3,1,7,4,
				3,0,4,7,6,1,9,5,8,
				2,6,5,8,3,7,4,9,1,
				7,3,1,0,0,5,2,0,6,
				9,4,8,6,1,2,5,3,7,
				8,0,2,0,5,0,3,4,9,
				4,1,3,2,9,0,7,6,5,
				5,9,0,3,7,4,8,1,2
			};
		}
		/*de 3 a 5: médio*/
		if(contador == 3){
			tabuleiro = new int[]
			{
				6,0,0,0,0,0,2,9,5,
				7,0,0,4,9,0,6,0,0,
				2,8,0,0,5,0,0,0,0,
				0,0,0,9,2,7,0,3,0,
				0,9,2,8,0,5,7,1,0,
				0,4,0,1,6,3,0,0,0,
				0,0,0,0,3,0,0,5,9,
				0,0,3,0,7,8,0,0,2,
				4,2,8,0,0,0,0,0,7
			};
		}
		if(contador == 4){
			tabuleiro = new int[]
			{
				0,0,4,0,7,0,0,0,2,
				6,7,0,1,9,5,3,0,8,
				1,0,0,3,0,2,0,0,7,
				0,0,9,7,0,0,0,2,3,
				0,2,6,8,5,3,0,9,1,
				7,1,0,9,0,4,8,0,6,
				9,0,1,0,0,0,2,0,4,
				2,8,7,0,0,0,6,3,5,
				0,0,0,0,0,0,0,0,0
			};
		}
		if(contador == 5){
			tabuleiro = new int[]
			{
				0,2,0,6,0,8,0,0,0,
				5,8,0,0,0,9,7,0,0,
				0,0,0,0,4,0,0,0,0,
				3,7,0,0,0,0,5,0,0,
				6,0,0,0,0,0,0,0,4,
				0,0,8,0,0,0,0,1,3,
				0,0,0,0,2,0,0,0,0,
				0,0,9,8,0,0,0,3,6,
				0,0,0,3,0,6,0,9,0
			};
		}
		/*de 6 a 8: difícil*/
		if(contador == 6){
			tabuleiro = new int[]
			{
				8,0,0,4,0,6,0,0,7,
				0,0,0,0,0,0,4,0,0,
				0,1,0,0,0,0,6,5,0,
				5,0,9,0,3,0,7,8,0,
				0,0,0,0,7,0,0,0,0,
				0,4,8,0,2,0,1,0,3,
				0,5,2,0,0,0,0,9,0,
				0,0,0,0,0,0,0,0,0,
				3,0,0,9,0,2,0,0,5
			};
		}
		if(contador == 7){
			tabuleiro = new int[]
			{
				5,3,0,0,7,0,0,0,0,
				6,0,0,1,9,0,0,0,0,
				0,9,8,0,0,0,0,0,0,
				8,0,0,0,6,0,0,0,0,
				4,0,0,8,0,3,0,0,0,
				7,0,0,0,0,0,0,0,0,
				0,6,0,0,0,0,0,0,0,
				0,0,0,4,1,9,0,0,5,
				0,0,0,0,8,0,0,7,9
			};
		}
		if(contador == 8){
			tabuleiro = new int[]
			{
				0,0,7,0,0,0,0,8,0,
				0,5,0,0,0,4,0,0,1,
				1,0,0,0,0,0,4,6,0,
				4,0,0,0,9,6,0,1,0,
				0,6,0,0,1,3,0,0,2,
				8,0,0,0,2,5,0,3,0,
				6,0,0,0,0,0,7,9,0,
				0,2,0,0,0,7,0,0,0,
				0,0,0,0,0,0,0,0,0
			};
		}
		contador++;
		if(contador == 9)
			contador = 0;
		
		/*zera novamente os números possíveis*/
		for(int i = 0; i < 81; i++)
		{
			if(tabuleiro[i] == 0)
				for(int j = 0; j < 9; j++)
					numeros_possiveis[i,j] = j + 1;
			else
				numeros_possiveis[i,0] = -1;
		}
		
		/*imprime o tabuleiro na tela*/
		imprimir();
		myText2.text = minhaString;
	}
	
	/*chama a próxima iteração de preencher o tabuleiro*/
	public void Iniciar()
	{
		print(iteracao);
		if(iteracao >= 0){
			iteracao = preencher_tabuleiro(iteracao);
			imprimir();
		}
	}
	
	/*chama a função de preencher o tabuleiro até exibir o resultado final*/
	public void Resultado()
	{
		while(iteracao>=0){
			iteracao = preencher_tabuleiro(iteracao);
		}
		if(iteracao == -2)
			erro.text = "impossível resolver o tabuleiro";
		
		minhaString = "";
		for(int i = 0; i < 81; i+=9)
		{
			for(int j = 0; j < 9; j++)
			{
				minhaString = minhaString + tabuleiro[i+j] + " ";
			}
		}
		myText.text = minhaString;
	}
}