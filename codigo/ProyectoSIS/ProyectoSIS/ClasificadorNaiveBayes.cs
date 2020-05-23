/*
 * Created by SharpDevelop.
 * User: Chu
 * Date: 05/05/2020
 * Time: 2:47 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ProyectoSIS
{
	/// <summary>
	/// Description of ClasificadorNaiveBayes.
	/// </summary>
	public class ClasificadorNaiveBayes
	{
		List<rio> rios;
		rio unRio;
		List<double> mediasBUENO; //por cada atributo
		List<double> mediasREGULAR; //por cada atributo
		List<double> mediasMALO; //por cada atributo
		
		List<double> desviacionesEstandarBUENO; //por cada atributo
		List<double> desviacionesEstandarREGULAR; //por cada atributo
		List<double> desviacionesEstandarMALO; //por cada atributo
		
		int numeroInstancias;
		int numeroInstanciasBUENO;
		int numeroInstanciasREGULAR;
		int numeroInstanciasMALO;
		
		
		public ClasificadorNaiveBayes(List<rio> riosx, rio riox)
		{
			rios= riosx;
			unRio = riox;
			
			mediasBUENO = new List<double>();
			desviacionesEstandarBUENO = new List<double>();
			
			mediasREGULAR = new List<double>();
			desviacionesEstandarREGULAR = new List<double>();
			
			mediasMALO = new List<double>();
			desviacionesEstandarMALO = new List<double>();
			
			
			
			for(int i = 0; i <= 5; i++)
			{
				mediasBUENO.Add(0.0);
				desviacionesEstandarBUENO.Add(0.0);
				
				mediasREGULAR.Add(0.0);
				desviacionesEstandarREGULAR.Add(0.0);
				
				mediasMALO.Add(0.0);
				desviacionesEstandarMALO.Add(0.0);
			}
			
			numeroInstancias = rios.Count;
			
			numeroInstanciasBUENO = 0;
			numeroInstanciasREGULAR = 0;
			numeroInstanciasMALO = 0;
			
			calcularMedias();
			calcularDesviacioneEstandar();
			
			//MessageBox.Show("MEDIASbueno[0] = " + mediasBUENO[0].ToString());
			//MessageBox.Show("DESVeSTANDARbueono[0] = " + desviacionesEstandarBUENO[0].ToString());
			
		}
		
		public string clasificarRio()
		{
			string estadoHidrologico = rios.Count.ToString();
			estadoHidrologico = calcularProbabilidadDeCadaClasePosible();
			return estadoHidrologico;
		}
		
		public string calcularProbabilidadDeCadaClasePosible()
		{
			double probabilidadBueno = calcularProbabilidadDeQueEstadoSeaBueno();
			double probabilidadRegular = calcularProbabilidadDeQueEstadoSeaRegular();
			double probabilidadMalo = calcularProbabilidadDeQueEstadoSeaMalo();
			//MessageBox.Show(probabilidadBueno.ToString());
			string claseMayor = seleccionarClaseConMayorProbabilidad(probabilidadBueno, probabilidadRegular, probabilidadMalo);
			return claseMayor;
		}
		
		public double calcularProbabilidadDeQueEstadoSeaBueno()
		{
			double probEstadoBueno = 1.0;
			
			//calcula la funcion de densidad para cada atributo
			probEstadoBueno = probEstadoBueno * calcularFuncionDeDensidad(0, "BUENO", unRio.getCorriente());
			//probEstadoBueno = Math.Round(probEstadoBueno, 5);
			probEstadoBueno = probEstadoBueno * calcularFuncionDeDensidad(1, "BUENO", unRio.getProfundidad());
			//probEstadoBueno = Math.Round(probEstadoBueno, 5);
			
			probEstadoBueno = probEstadoBueno * calcularFuncionDeDensidad(2, "BUENO", unRio.getTemperatura());
			//probEstadoBueno = Math.Round(probEstadoBueno, 4);
			probEstadoBueno = probEstadoBueno * calcularFuncionDeDensidad(3, "BUENO", unRio.getNumeroEspecies());
			//probEstadoBueno = Math.Round(probEstadoBueno, 4);
			//probEstadoBueno = probEstadoBueno * calcularFuncionDeDensidad(4, "BUENO", unRio.getGradoContaminacion());
			//probEstadoBueno = Math.Round(probEstadoBueno, 4);
			probEstadoBueno = probEstadoBueno * ((double)numeroInstanciasBUENO / (double)numeroInstancias);
			//probEstadoBueno = Math.Round(probEstadoBueno, 4);
			
			//MessageBox.Show(probEstadoBueno.ToString());
			return probEstadoBueno;
		}
		
		public double calcularProbabilidadDeQueEstadoSeaRegular()
		{
			double probEstadoRegular = 1.0;
			
			//calcula la funcion de densidad para cada atributo
			probEstadoRegular = probEstadoRegular * calcularFuncionDeDensidad(0, "REGULAR", unRio.getCorriente());
			//probEstadoRegular = Math.Round(probEstadoRegular, 4);
			probEstadoRegular = probEstadoRegular * calcularFuncionDeDensidad(1, "REGULAR", unRio.getProfundidad());
			//probEstadoRegular = Math.Round(probEstadoRegular, 4);
			probEstadoRegular = probEstadoRegular * calcularFuncionDeDensidad(2, "REGULAR", unRio.getTemperatura());
			//probEstadoRegular = Math.Round(probEstadoRegular, 4);
			probEstadoRegular = probEstadoRegular * calcularFuncionDeDensidad(3, "REGULAR", unRio.getNumeroEspecies());
			//probEstadoRegular = Math.Round(probEstadoRegular, 4);
			//probEstadoRegular = probEstadoRegular * calcularFuncionDeDensidad(4, "REGULAR", unRio.getGradoContaminacion());
			//probEstadoRegular = Math.Round(probEstadoRegular, 4);
			probEstadoRegular = probEstadoRegular * ((double)numeroInstanciasREGULAR / (double)numeroInstancias);
			//probEstadoRegular = Math.Round(probEstadoRegular, 4);
			
			return probEstadoRegular;
		}
		
		public double calcularProbabilidadDeQueEstadoSeaMalo()
		{
			double probEstadoMalo = 1.0;
			
			//calcula la funcion de densidad para cada atributo
			probEstadoMalo = probEstadoMalo * calcularFuncionDeDensidad(0, "MALO", unRio.getCorriente());
			//probEstadoMalo = Math.Round(probEstadoMalo, 4);
			probEstadoMalo = probEstadoMalo * calcularFuncionDeDensidad(1, "MALO", unRio.getProfundidad());
			//probEstadoMalo = Math.Round(probEstadoMalo, 4);
			probEstadoMalo = probEstadoMalo * calcularFuncionDeDensidad(2, "MALO", unRio.getTemperatura());
			//probEstadoMalo = Math.Round(probEstadoMalo, 4);
			probEstadoMalo = probEstadoMalo * calcularFuncionDeDensidad(3, "MALO", unRio.getNumeroEspecies());
			//probEstadoMalo = Math.Round(probEstadoMalo, 4);
			//probEstadoMalo = probEstadoMalo * calcularFuncionDeDensidad(4, "MALO", unRio.getGradoContaminacion());
//			probEstadoMalo = Math.Round(probEstadoMalo, 4);
			probEstadoMalo = probEstadoMalo * ((double)numeroInstanciasMALO / (double)numeroInstancias);
//			probEstadoMalo = Math.Round(probEstadoMalo, 4);
			
			return probEstadoMalo;
		}
		
		public string seleccionarClaseConMayorProbabilidad(double probBUENO, double probREG, double probMALO)
		{			
			string claseMasProbable = "";
			
			double divisor = (double)probBUENO + (double)probREG + (double)probMALO;
			
			double pB = (double)probBUENO / (double)divisor;
			double pR = (double)probREG / (double)divisor;
			double pM = (double)probMALO /(double)divisor;
			//MessageBox.Show("probabilidadesBueno/reg/malo = \n" + pB +"\n"+pR+"\n"+pM); //comentar para no ver probabilidades
			
			//MessageBox.Show("divisor = " + divisor.ToString());
			//MessageBox.Show("pb = " + pB);
			//MessageBox.Show("pr = " + pR);
			//MessageBox.Show("pm = " + pM);
			
			List<double> probabilidades = new List<double>();
			probabilidades.Add(pB);
			probabilidades.Add(pR);
			probabilidades.Add(pM);
			probabilidades.Sort();
			if(probabilidades[2] == pB){
				claseMasProbable = "BUENO";
			} else if (probabilidades[2] == pR) {
				claseMasProbable = "REGULAR";
			} else {
				claseMasProbable = "MALO";
			}
			
			//MessageBox.Show(claseMasProbable);
			return claseMasProbable;
		}
		
		void calcularMedias(){
			
			//recorro las 69 instancias 3 veces, para sacar el promedio
			//dependiendo la clase
			for(int i = 0; i < numeroInstancias; i++)
			{
				if(rios[i].getEstadoHidrologico() == "BUENO"){
					numeroInstanciasBUENO++;
					
					mediasBUENO[0] += rios[i].getCorriente();
					mediasBUENO[1] += rios[i].getProfundidad();
					mediasBUENO[2] += rios[i].getTemperatura();
					mediasBUENO[3] += rios[i].getNumeroEspecies();
					mediasBUENO[4] += rios[i].getGradoContaminacion();
				}else if(rios[i].getEstadoHidrologico() == "REGULAR"){
					numeroInstanciasREGULAR++;
					
					mediasREGULAR[0] += rios[i].getCorriente();
					mediasREGULAR[1] += rios[i].getProfundidad();
					mediasREGULAR[2] += rios[i].getTemperatura();
					mediasREGULAR[3] += rios[i].getNumeroEspecies();
					mediasREGULAR[4] += rios[i].getGradoContaminacion();
				}else{
					numeroInstanciasMALO++;
					
					mediasMALO[0] += rios[i].getCorriente();
					mediasMALO[1] += rios[i].getProfundidad();
					mediasMALO[2] += rios[i].getTemperatura();
					mediasMALO[3] += rios[i].getNumeroEspecies();
					mediasMALO[4] += rios[i].getGradoContaminacion();
				}
					
			}
			
			for(int i =0; i < 5; i++){
				mediasBUENO[i] = mediasBUENO[i] / numeroInstanciasBUENO;
				mediasREGULAR[i] = mediasREGULAR[i] / numeroInstanciasREGULAR;
				mediasMALO[i] = mediasMALO[i] / numeroInstanciasMALO;
			}

		}
		
		void calcularDesviacioneEstandar(){
			for(int i = 0; i < numeroInstancias ;i++)//sumatoria 
			{
				double cuadrado = 0.0;
				
				if(rios[i].getEstadoHidrologico() == "BUENO"){
					cuadrado = Math.Pow(rios[i].getCorriente() - mediasBUENO[0], 2);
					desviacionesEstandarBUENO[0] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getProfundidad() - mediasBUENO[1], 2);
					desviacionesEstandarBUENO[1] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getTemperatura() - mediasBUENO[2], 2);
					desviacionesEstandarBUENO[2] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getNumeroEspecies() - mediasBUENO[3], 2);
					desviacionesEstandarBUENO[3] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getGradoContaminacion() - mediasBUENO[4], 2);
					desviacionesEstandarBUENO[4] += cuadrado;
					
				}else if(rios[i].getEstadoHidrologico() == "REGULAR"){
					cuadrado = Math.Pow(rios[i].getCorriente() - mediasREGULAR[0], 2);
					desviacionesEstandarREGULAR[0] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getProfundidad() - mediasREGULAR[1], 2);
					desviacionesEstandarREGULAR[1] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getTemperatura() - mediasREGULAR[2], 2);
					desviacionesEstandarREGULAR[2] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getNumeroEspecies() - mediasREGULAR[3], 2);
					desviacionesEstandarREGULAR[3] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getGradoContaminacion() - mediasREGULAR[4], 2);
					desviacionesEstandarREGULAR[4] += cuadrado;
				}else{
					cuadrado = Math.Pow(rios[i].getCorriente() - mediasMALO[0], 2);
					desviacionesEstandarMALO[0] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getProfundidad() - mediasMALO[1], 2);
					desviacionesEstandarMALO[1] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getTemperatura() - mediasMALO[2], 2);
					desviacionesEstandarMALO[2] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getNumeroEspecies() - mediasMALO[3], 2);
					desviacionesEstandarMALO[3] += cuadrado;
					
					cuadrado = Math.Pow(rios[i].getGradoContaminacion() - mediasMALO[4], 2);
					desviacionesEstandarMALO[4] += cuadrado;
				}

			}
			
			for(int i = 0; i < 6; i++)
			{				
				desviacionesEstandarBUENO[i] /= numeroInstanciasBUENO;
				desviacionesEstandarBUENO[i] = Math.Sqrt(desviacionesEstandarBUENO[i]);
				
				desviacionesEstandarREGULAR[i] /= numeroInstanciasREGULAR;
				desviacionesEstandarREGULAR[i] = Math.Sqrt(desviacionesEstandarREGULAR[i]);
				
				desviacionesEstandarMALO[i] /= numeroInstanciasMALO;
				desviacionesEstandarMALO[i] = Math.Sqrt(desviacionesEstandarMALO[i]);
			}
		}
		
		double calcularFuncionDeDensidad(int posAtributo, string clase, double x){
			
			//x = el valor de un atributo para una instancia que se desea clasificar
			//posAtributo indica si que atributo es
			
			double potencia = 0.0;
			
			double resultadoFuncionDeDensidad = 0.0;
			
			if(clase == "BUENO"){				
				potencia = -1 * (Math.Pow(x - mediasBUENO[posAtributo], 2) / (2 * Math.Pow(desviacionesEstandarBUENO[posAtributo], 2)));
				resultadoFuncionDeDensidad = (1 / (Math.Sqrt(2 * Math.PI) * desviacionesEstandarBUENO[posAtributo])) *Math.Pow(Math.E, potencia);				
			} else if(clase == "REGULAR") {
				potencia = -1 * (Math.Pow(x - mediasREGULAR[posAtributo], 2) / (2 * Math.Pow(desviacionesEstandarREGULAR[posAtributo], 2)));
				resultadoFuncionDeDensidad = (1 / (Math.Sqrt(2 * Math.PI) * desviacionesEstandarREGULAR[posAtributo])) *Math.Pow(Math.E, potencia);
			} else {
				potencia = -1 * (Math.Pow(x - mediasMALO[posAtributo], 2) / (2 * Math.Pow(desviacionesEstandarMALO[posAtributo], 2)));
				resultadoFuncionDeDensidad = (1 / (Math.Sqrt(2 * Math.PI) * desviacionesEstandarMALO[posAtributo])) *Math.Pow(Math.E, potencia);
			}
			
			return resultadoFuncionDeDensidad;
		}
		
	}
}
