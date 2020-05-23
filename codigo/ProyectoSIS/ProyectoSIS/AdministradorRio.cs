/*
 * Created by SharpDevelop.
 * User: Chu
 * Date: 24/03/2020
 * Time: 11:49 a.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace ProyectoSIS
{
	/// <summary>
	/// Description of AdministradorRio.
	/// </summary>
	public class AdministradorRio
	{
		public List<rio> rios;
		
		//Variables para conectarse a la BD
		string datosDeConexionBD;
		//string consulta;
		bool conexionAbierta; // indica si la conexion  la BD esta abierta
		
		MySqlConnection conexionDB; //establece la conexion con la BD
		
				
		public AdministradorRio()
		{
			inicializarAdminRio();
		}
		
		public void inicializarAdminRio(){
			rios = new List<rio>();
			datosDeConexionBD = "datasource=127.0.0.1;port=3306;username=mw;password=1234;database=rios;";
			//consulta = "";
			conexionAbierta = false;
			rios = extraerDeBD();//inicializa la lista derios, con lo de la base de datos
		}
		
		public void cargarArchivo()
		{
			
		}
		
		public void guardarMultiplesInstanciasEnBD(){}
		
		public List<rio> extraerDeBD()
		{
			string consultaSQL = "SELECT DISTINCT `nombre_rio`,`corriente`,`profundidad`,"+
								 "`temperatura`,`numero_especies`,`estado_hidrologico` "+
								 "FROM `rio` "+
								 "JOIN `hidrologia` ON `rio`.`id_rio` = `hidrologia`.`id_rio` " +
								 "ORDER BY `id_hidrologico` DESC;";
			consultaEnBD(consultaSQL);
			return rios;
		}
		
		public List<rio> getListaRios()
		{
			return rios;
		}
		
		public int obtenerUltimoID(){
			string ultimoID = "SELECT MAX(id_rio) FROM rio";
			//conectarBD();
			
			conexionDB = new MySqlConnection(datosDeConexionBD);
			
			conexionDB.Open();
			
			MySqlCommand comandosDB; //
			MySqlDataReader resultadoConsulta; //
			//conexionDB = new MySqlConnection(datosDeConexionBD);
			comandosDB = new MySqlCommand(ultimoID, conexionDB);
			comandosDB.CommandTimeout = 60;

			// Ejecuta la consultas
			resultadoConsulta = comandosDB.ExecuteReader();
			
			int id = 0;
			while (resultadoConsulta.Read())
			{
				id = resultadoConsulta.GetInt16(0);
			}

			desconectarBD();
			
			return id;
		}
		
		public void consultaguardarDB(string consulta){

			
			conexionDB = new MySqlConnection(datosDeConexionBD);
			
			conexionDB.Open();
			
			MySqlCommand comandosDB; //
			//mysql resultadoConsulta; //
			//conexionDB = new MySqlConnection(datosDeConexionBD);
			comandosDB = new MySqlCommand(consulta, conexionDB);
			//comandosDB.CommandTimeout = 60;

			// Ejecuta la consultas
			comandosDB.ExecuteNonQuery();
			
			desconectarBD();
			
			
		}
		
		public void guardarInstanciaEnBD(rio unRio){
			
			string insertaRio = "INSERT INTO rio(id_rio,nombre_rio) VALUES(NULL,'"+
				unRio.getNombre()+"');";
			
			consultaguardarDB(insertaRio);
			
			int id = obtenerUltimoID();
			
			string insertaHidrologia = "INSERT INTO hidrologia(id_hidrologico,id_rio,corriente,profundidad,"+
								 		"temperatura,numero_especies,grado_contaminacion,estado_hidrologico) "+
										"VALUES(NULL,"+id+","+
										unRio.getCorriente()+","+
										unRio.getProfundidad()+","+
										unRio.getTemperatura()+","+
										unRio.getNumeroEspecies()+","+
										unRio.getGradoContaminacion()+",'"+
										unRio.getEstadoHidrologico()+"');";

			
			consultaguardarDB(insertaHidrologia);
			MessageBox.Show("Instancia agregada a la base de datos.");
		}
		
		public DataTable listarRiosPorEstadoHidrologico()
		{
			DataTable riosPorEstadoHidrologico = new DataTable();
			
			List<string> buenos, regulares, malos;
			buenos = new List<string>();
			regulares = new List<string>();
			malos = new List<string>();
			
			riosPorEstadoHidrologico.Columns.Add("BUENO");
			riosPorEstadoHidrologico.Columns.Add("REGULAR");
			riosPorEstadoHidrologico.Columns.Add("MALO");
			
			foreach (rio unRio in rios) { // divide rios en tres listas
				string estadoHidrologico = unRio.getEstadoHidrologico();
				
				if(estadoHidrologico == "BUENO"){
					//fila[0] = unRio.getNombre();
					//MessageBox.Show(unRio.getNombre());
					buenos.Add(unRio.getNombre());
				}else if(estadoHidrologico == "REGULAR"){
					//fila[1] = unRio.getNombre();
					//MessageBox.Show(unRio.getNombre());
					regulares.Add(unRio.getNombre());
				}else{
					//fila[2] = unRio.getNombre();
					//MessageBox.Show(unRio.getNombre());
					malos.Add(unRio.getNombre());
				}
				
			}
			//MessageBox.Show(buenos.Count.ToString());
			
			int i = 0;
			while(i < buenos.Count || i < regulares.Count || i < malos.Count ){
				DataRow fila = riosPorEstadoHidrologico.NewRow();
				
				if(i < buenos.Count){
					fila[0] = buenos[i];
					//MessageBox.Show(buenos.Count.ToString());
				}
				if(i < regulares.Count){
					fila[1] = regulares[i];
				}
				if(i < malos.Count){
					fila[2] = malos[i];
				}
				
				i++;
				
				riosPorEstadoHidrologico.Rows.Add(fila);
			}
			
			return riosPorEstadoHidrologico;
		}

		public DataTable mostrarDatosHidrologicos()
		{
			// en realidad lo que hace es pasar los datos de la lista a un datatable, para simplificar el proceso de mostrar
			DataTable datosHidrologicos = new DataTable();
			
			datosHidrologicos.Columns.Add("Nombre");
			datosHidrologicos.Columns.Add("Corriente");
			datosHidrologicos.Columns.Add("Profundidad");
			datosHidrologicos.Columns.Add("Temperatura");
			datosHidrologicos.Columns.Add("Numero de Especies");
			datosHidrologicos.Columns.Add("Estado Hidrologico");
			
			foreach (rio unRio in rios) {
				DataRow fila = datosHidrologicos.NewRow();
				fila[0] = unRio.getNombre();
				fila[1] = unRio.getCorriente().ToString();
			    fila[2] = unRio.getProfundidad().ToString();
				fila[3] = unRio.getTemperatura().ToString();
				fila[4] = unRio.getNumeroEspecies().ToString();
				fila[5] = unRio.getEstadoHidrologico();
				                    	   
				datosHidrologicos.Rows.Add(fila);
			}
			
			return datosHidrologicos;
		}
		
		public void clasificacionIndividual(){}

		public void clasificacionMultiple(){}

		public void conectarBD()
		{
			// Prepara la conexión
			conexionDB = new MySqlConnection(datosDeConexionBD);
			
			try
			{
			    // Abre la base de datos
			    conexionDB.Open();
			    //MessageBox.Show("Se realizo la conexión a la BD.");
			    conexionAbierta = true;//si truena no se pondra como true y seguira en false
			    
			}
			catch (Exception ex)
			{
			    // Mostrar cualquier excepción
			    string msg = "No se pudo conectar con la BD.\n\n";
			    MessageBox.Show(msg + ex.Message);
			}
		}
		
		public void desconectarBD()
		{
			try
			{
			    // Cerrar la conexión
			    conexionDB.Close();
			    //MessageBox.Show("Se desconecto de la BD.");
			    conexionAbierta = false;//si truena no se pondra como false y seguira en true
			    // Ejecuta la consultas
			}
			catch (Exception ex)
			{
			    string msg = "No se pudo desconectar de la BD.\n\n";
			    MessageBox.Show(msg + ex.Message);
			}
			
		}
		
		public bool consultaEnBD(string consulta)
		{
			bool seEncontraronDatos = false;
			
			conectarBD();
			
			if(conexionAbierta){
				MySqlCommand comandosDB; //
				MySqlDataReader resultadoConsulta; //
			
				comandosDB = new MySqlCommand(consulta, conexionDB);
				comandosDB.CommandTimeout = 60;
				
				// Ejecuta la consultas
			    resultadoConsulta = comandosDB.ExecuteReader();

			    
			    if (resultadoConsulta.HasRows)
			   	{
			   		seEncontraronDatos = true;
			        while (resultadoConsulta.Read())
			        {
			            // En nuestra base de datos, el array contiene:  ID 0, FIRST_NAME 1,LAST_NAME 2, ADDRESS 3
			            // Hacer algo con cada fila obtenida
			            rio unRio = new rio();
			            
			            unRio.setNombre(resultadoConsulta.GetString(0));
			            unRio.setCorriente(resultadoConsulta.GetDouble(1));
			            unRio.setProfundidad(resultadoConsulta.GetDouble(2));
			            unRio.setTemperatura(resultadoConsulta.GetDouble(3));
			            unRio.setNumeroEspecies(resultadoConsulta.GetInt16(4));
			            //unRio.setGradoContaminacion(resultadoConsulta.GetInt16(5));
			            unRio.setEstadoHidrologico(resultadoConsulta.GetString(5));
			            
			            rios.Add(unRio);
			            
			        }
			    }				
				
				desconectarBD();
			}
			
			return seEncontraronDatos;
		}
	}
}
