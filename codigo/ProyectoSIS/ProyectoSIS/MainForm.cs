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
using System.Data;

namespace ProyectoSIS
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		AdministradorRio adminRio;
		rio unRio;
		
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
				
		void cargarTablaDeRiosYTodosSusDatos()
		{
			limpiarTablas();
			DataTable dt = new DataTable();
			dataGridView1.DataSource = dt;
			dataGridView1.Columns.Clear();
			adminRio.inicializarAdminRio();
			dataGridView1.DataSource = adminRio.listarRiosPorEstadoHidrologico();
			dataGridView1.ClearSelection();
			dataGridView1.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
			dataGridView1.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
			colorearCeldas();

			dataGridView2.Columns.Clear();
			dataGridView2.DataSource = adminRio.mostrarDatosHidrologicos();
			dataGridView2.ClearSelection();
		}
		
		void colorearCeldas()
		{
			for(int i = 0; i < dataGridView1.Rows.Count; i++ )
			{
				//recorre por columna
				for(int j = 0; j < 3; j++)
				{
					switch(j)
					{
						case 0:
							dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.PaleGreen;
							break;
						case 1:
							dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.LightYellow;
							break;
						case 2:
							dataGridView1.Rows[i].Cells[j].Style.BackColor = System.Drawing.Color.LightPink;
							break;
					}
				}
			}
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//MessageBox.Show("holo");
			
			
			//saco lo de los textbox y los meto a objeto rio
			unRio = new rio();
			unRio.setNombre(textBox1.Text);
			unRio.setCorriente( Convert.ToDouble(textBox2.Text));
			unRio.setProfundidad( Convert.ToDouble(textBox3.Text));
			unRio.setTemperatura( Convert.ToDouble(textBox4.Text));
			unRio.setNumeroEspecies( Convert.ToInt32(textBox5.Text));
			unRio.setGradoContaminacion( Convert.ToDouble(textBox6.Text));
			
			
			//aqui voy a llamar a la funcion que clasifica y retorna la clase
			List<rio> rios = adminRio.getListaRios();
			ClasificadorNaiveBayes cNB = new ClasificadorNaiveBayes(rios, unRio);
			
			
			string estadoHidrologico = cNB.clasificarRio();
			
			if(estadoHidrologico == "BUENO"){
				label12.ForeColor = System.Drawing.Color.LimeGreen;
			} else if(estadoHidrologico == "REGULAR"){
				label12.ForeColor = System.Drawing.Color.Orange;
			}else{
				label12.ForeColor = System.Drawing.Color.Red;
			}
			
			label12.Text = estadoHidrologico;
			
			button2.Enabled = true;
		}
		
		void Button2Click(object sender, EventArgs e)
		{
			unRio.setEstadoHidrologico(label12.Text);
			adminRio.guardarInstanciaEnBD(unRio);
			
			limpiarTablas();
			
			cargarTablaDeRiosYTodosSusDatos();
			label12.ForeColor = System.Drawing.Color.DarkGray;
			label12.Text = "SIN CLASIFICAR";
		
			
			textBox1.Clear();
			textBox2.Clear();
			textBox3.Clear();
			textBox4.Clear();
			textBox5.Clear();
			textBox6.Clear();
			
			button2.Enabled = false;
			
			unRio.limpiarRio();
		}
		
		void activarBoton1(){
			if(textBox1.Text != "" &&
			   textBox2.Text != "" &&
			   textBox3.Text != "" &&
			   textBox4.Text != "" &&
			   textBox5.Text != "" &&
			   textBox6.Text != ""){
				button1.Enabled = true;
			}else{
				button1.Enabled = false;
			}
		}
		
		void TextBox1TextChanged(object sender, EventArgs e)
		{
			activarBoton1();
		}
		void TextBox2TextChanged(object sender, EventArgs e)
		{
			activarBoton1();
		}
		void TextBox3TextChanged(object sender, EventArgs e)
		{
			activarBoton1();
		}
		void TextBox4TextChanged(object sender, EventArgs e)
		{
			activarBoton1();
		}
		void TextBox5TextChanged(object sender, EventArgs e)
		{
			activarBoton1();
		}
		void TextBox6TextChanged(object sender, EventArgs e)
		{
			activarBoton1();
		}
		
		
		void limpiarTablas(){
			//DataTable dt = new DataTable();
			
			//dataGridView1.DataSource = dt;
			dataGridView1.Columns.Clear();
			
			//dataGridView2.DataSource = dt;
			dataGridView2.Columns.Clear();
		}
	}
}
