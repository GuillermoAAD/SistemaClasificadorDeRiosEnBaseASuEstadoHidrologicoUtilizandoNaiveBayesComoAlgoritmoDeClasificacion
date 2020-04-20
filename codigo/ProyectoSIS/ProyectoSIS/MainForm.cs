/*
 * Created by SharpDevelop.
 * User: Usuario
 * Date: 12/03/2020
 * Time: 11:52 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoSIS
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		AdministradorRio adminRio;
		rio rios;
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
		}
		void MainFormLoad(object sender, EventArgs e)
		{
			adminRio = new AdministradorRio();
			//adminRio.extraerDeBD();
			cargarTablaDeRiosYTodosSusDatos();
			
		}
		void Button1Click(object sender, EventArgs e)
		{
			MessageBox.Show("holo");
		}
		
		void cargarTablaDeRiosYTodosSusDatos()
		{
			dataGridView1.Columns.Clear();			
			dataGridView1.DataSource = adminRio.listarRiosPorEstadoHidrologico();

			dataGridView2.Columns.Clear();
			dataGridView2.DataSource = adminRio.mostrarDatosHidrologicos();						
		}
	}
}
