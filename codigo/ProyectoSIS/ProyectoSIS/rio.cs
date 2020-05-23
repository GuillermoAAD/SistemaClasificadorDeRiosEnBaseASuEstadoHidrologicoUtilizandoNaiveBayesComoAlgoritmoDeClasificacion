/*
 * Created by SharpDevelop.
 * User: Usuario
 * Date: 12/03/2020
 * Time: 11:53 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace ProyectoSIS
{
	/// <summary>
	/// Description of rio.
	/// </summary>
	public class rio
	{
		private string nombre;
		private double corriente;
		private double profundidad;
		private double temperatura;
		private int numeroEspecies;
		private double gradoContaminacion;
		private string estadoHidrologico;
		
		public rio()
		{
			limpiarRio();
		}
		
		public void setNombre(string n)
		{
			nombre = n;
		}
		public string getNombre()
		{
			return nombre;
		}
		
		public void setCorriente(double c)
		{
			corriente = c;
		}
		public double getCorriente()
		{
			return corriente;
		}
		
		public void setProfundidad(double p)
		{
			profundidad = p;
		}
		public double getProfundidad()
		{
			return profundidad;
		}
		
		public void setTemperatura(double t)
		{
			temperatura = t;
		}
		public double getTemperatura()
		{
			return temperatura;
		}
		
		public void setNumeroEspecies(int ne)
		{
			numeroEspecies = ne;
		}
		public int getNumeroEspecies()
		{
			return numeroEspecies;
		}
		
		public void setGradoContaminacion(double gc)
		{
			gradoContaminacion = gc;
		}
		public double getGradoContaminacion()
		{
			return gradoContaminacion;
		}
		
		public void setEstadoHidrologico(string eh)
		{
			estadoHidrologico = eh;
		}
		public string getEstadoHidrologico()
		{
			return estadoHidrologico;
		}
		
		public void limpiarRio(){
			nombre = "";
			corriente = 0.0;
			profundidad = 0.0;
			temperatura = 0.0;
			numeroEspecies = 0;
			gradoContaminacion = 0.0;
			estadoHidrologico = "";
		}
	}
}
