using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Odbc;    //para conexion a la BD
using System.Threading.Tasks;
using System.Security;
using System.Reflection;

namespace Contab {
	public partial class FrmContab : Form {

        //Parametros de busqueda
        private string idComprobanteBuscar = "";
        private string idCuentaAuxiliarBuscar = "";
        private int idMovimientoResaltar = 0;

        //Nodos claves del arbol
        private TreeNode nodoTodasLasCuentas = null;
        private FrmSelCuenta ventanaSelCuentaDetalle = new FrmSelCuenta();

		//Async tasks
		private Task<double> _tasaCambioTask = null;
		private int _tasaCambioRowIndex = -1;
		private int _treeFirstUpdateWait = 1000; //ms
        private bool _treeShowBalances = false;		//show acc balances
		private bool _treeShowBalancesSep = false;	//show balances in separated currency

        public FrmContab() {
			InitializeComponent();
		}

		private void FrmContab_Load(object sender, EventArgs e) {
			Application.DoEvents();
            //
			fechaIniDiario.Value = DateTime.Today;
			fechaFinDiario.Value = DateTime.Today;
			fechaNuevoMov.Value = DateTime.Today;
			//Title
			this.Text = Global.connUser + "@" + Global.connDb;
		}

		private string getNodeExpandedSubnodesList(TreeNodeCollection nodes) {
            string r = "";
            if (nodes != null) {
                int i; for (i = 0; i < nodes.Count; i++) {
                    TreeNode n = nodes[i];
					if (n != null && n.IsExpanded) {
						r += "[" + n.Tag + "]";
						if (n.Nodes != null) {
							r += getNodeExpandedSubnodesList(n.Nodes);
                        }
                    }
                }
            }
			return r;
		}

        private string getExpandedNodesList() {
			string r = null;
			if (arbCuentas.Nodes != null) {
				r = this.getNodeExpandedSubnodesList(arbCuentas.Nodes);
            }
            return (r == null || r.Length <= 0 ? null : r);
		}

		private void actualizaArbolCuentas(string expandedNodesLst) {
			int expandedCount = 0;
			arbCuentas.BeginUpdate(); //previene parpadeos
            arbCuentas.Nodes.Clear();
            OdbcCommand ssql = null;
			OdbcDataReader conC = null;
			//Todas las cuentas
			nodoTodasLasCuentas = arbCuentas.Nodes.Add("Todas", "Todas las cuentas", "paquete16.gif"); nodoTodasLasCuentas.SelectedImageKey = nodoTodasLasCuentas.ImageKey;
			//Cuentas (5 niveles)
			string strsql = "";
			strsql += " SELECT Cuentas.idCuenta, Cuentas.numCuenta, Cuentas.nombreCuenta, ";
			strsql += " CuentasSub.idCuentaSub, CuentasSub.numCuentaSub, CuentasSub.nombreCuentaSub, ";
			strsql += " CuentasSubSub.idCuentaSubSub, CuentasSubSub.numCuentaSubSub, CuentasSubSub.nombreCuentaSubSub, ";
			strsql += " CuentasSubSubSub.idCuentaSubSubSub, CuentasSubSubSub.numCuentaSubSubSub, CuentasSubSubSub.nombreCuentaSubSubSub, ";
			strsql += " CuentasDetalle.idCuentaDetalle, CuentasDetalle.numCuentaDetalle, CuentasDetalle.nombreCuentaDetalle, ";
            strsql += " CuentasDetalle.balanceDebe, CuentasDetalle.balanceHaber, ";
            strsql += " CuentasDetalle.balanceDebeSoloCS, CuentasDetalle.balanceHaberSoloCS, ";
            strsql += " CuentasDetalle.balanceDebeSoloUS, CuentasDetalle.balanceHaberSoloUS ";
            strsql += " FROM (((Cuentas LEFT JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
			strsql += " LEFT JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
			strsql += " LEFT JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
			strsql += " LEFT JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub";
            strsql += " ORDER BY Cuentas.numCuenta, CuentasSub.numCuentaSub, CuentasSubSub.numCuentaSubSub, CuentasSubSubSub.numCuentaSubSubSub, CuentasDetalle.numCuentaDetalle; ";
			TreeNode nodoCuenta = null, nodoCuentaSub = null, nodoCuentaSubSub = null, nodoCuentaSubSubSub = null, nodoCuentaDetalle = null;
			bool nodoCuentaExpand = false, nodoCuentaSubExpand = false, nodoCuentaSubSubExpand = false, nodoCuentaSubSubSubExpand = false;
            string ultimoIDCuenta = "", ultimoIDCuentaSub = "", ultimoIDCuentaSubSub = "", ultimoIDCuentaSubSubSub = "", ultimoIDCuentaDetalle = "";
			ssql = new OdbcCommand(strsql, Global.conn);
			conC = ssql.ExecuteReader();
			while (conC.Read()) {
				string idCuenta = conC.GetString(0);
				int numCuenta = conC.GetInt32(1);
				string nombreCuenta = conC.GetString(2);
				if (idCuenta != ultimoIDCuenta) {
                    if (nodoCuenta != null && nodoCuentaExpand) nodoCuenta.Expand();
                    nodoCuenta = nodoTodasLasCuentas.Nodes.Add("cuenta_" + idCuenta, numCuenta.ToString() + " - " + nombreCuenta, "folder.gif"); nodoCuenta.SelectedImageKey = nodoCuenta.ImageKey;
					nodoCuenta.Tag = "cuenta_" + idCuenta;
					nodoCuentaExpand = false;
                    if (expandedNodesLst != null && expandedNodesLst.IndexOf("[" + nodoCuenta.Tag + "]") >= 0) {
						nodoCuentaExpand = true;
                        expandedCount++;
                    }
					ultimoIDCuenta = idCuenta;
				}
				if (!conC.IsDBNull(3)) {
					string idCuentaSub = conC.GetString(3);
					int numCuentaSub = conC.GetInt32(4);
					string nombreCuentaSub = conC.GetString(5);
					if (idCuentaSub != ultimoIDCuentaSub) {
						if(nodoCuentaSub != null && nodoCuentaSubExpand) nodoCuentaSub.Expand();
                        nodoCuentaSub = nodoCuenta.Nodes.Add("cuentaSub_" + idCuentaSub, numCuentaSub.ToString() + " - " + nombreCuentaSub, "folder.gif"); nodoCuentaSub.SelectedImageKey = nodoCuentaSub.ImageKey;
						nodoCuentaSub.Tag = "cuentaSub_" + idCuentaSub;
						nodoCuentaSubExpand = false;
                        if (expandedNodesLst != null && expandedNodesLst.IndexOf("[" + nodoCuentaSub.Tag + "]") >= 0) {
							nodoCuentaSubExpand = true;
							expandedCount++;
                        }
                        ultimoIDCuentaSub = idCuentaSub;
                    }
				}
				if (!conC.IsDBNull(6)) {
					string idCuentaSubSub = conC.GetString(6);
					int numCuentaSubSub = conC.GetInt32(7);
					string nombreCuentaSubSub = conC.GetString(8);
					if (idCuentaSubSub != ultimoIDCuentaSubSub) {
						if(nodoCuentaSubSub != null && nodoCuentaSubSubExpand) nodoCuentaSubSub.Expand();
                        nodoCuentaSubSub = nodoCuentaSub.Nodes.Add("cuentaSubSub_" + idCuentaSubSub, numCuentaSubSub.ToString() + " - " + nombreCuentaSubSub, "folder.gif"); nodoCuentaSubSub.SelectedImageKey = nodoCuentaSubSub.ImageKey;
						nodoCuentaSubSub.Tag = "cuentaSubSub_" + idCuentaSubSub;
						nodoCuentaSubSubExpand = false;
                        if (expandedNodesLst != null && expandedNodesLst.IndexOf("[" + nodoCuentaSubSub.Tag + "]") >= 0) {
							nodoCuentaSubSubExpand = true;
							expandedCount++;
                        }
                        ultimoIDCuentaSubSub = idCuentaSubSub;
                    }
				}
				if (!conC.IsDBNull(9)) {
					string idCuentaSubSubSub = conC.GetString(9);
					int numCuentaSubSubSub = conC.GetInt32(10);
					string nombreCuentaSubSubSub = conC.GetString(11);
					if (idCuentaSubSubSub != ultimoIDCuentaSubSubSub) {
						if(nodoCuentaSubSubSub != null && nodoCuentaSubSubSubExpand) nodoCuentaSubSubSub.Expand();
                        nodoCuentaSubSubSub = nodoCuentaSubSub.Nodes.Add("cuentaSubSubSub_" + idCuentaSubSubSub, numCuentaSubSubSub.ToString() + " - " + nombreCuentaSubSubSub, "folder.gif"); nodoCuentaSubSubSub.SelectedImageKey = nodoCuentaSubSubSub.ImageKey;
						nodoCuentaSubSubSub.Tag = "cuentaSubSubSub_" + idCuentaSubSubSub;
						nodoCuentaSubSubSubExpand = false;
                        if (expandedNodesLst != null && expandedNodesLst.IndexOf("[" + nodoCuentaSubSubSub.Tag + "]") >= 0) {
							nodoCuentaSubSubSubExpand = true;
							expandedCount++;
                        }
                        ultimoIDCuentaSubSubSub = idCuentaSubSubSub;
                    }
				}
				if (!conC.IsDBNull(12)) {
					string idCuentaDetalle = conC.GetString(12);
					int numCuentaDetalle = conC.GetInt32(13);
					string nombreCuentaDetalle = conC.GetString(14);
					if (idCuentaDetalle != ultimoIDCuentaDetalle) {
                        Int64 balanceDebe = 0, balanceHaber = 0;
                        Int64 balanceDebeSoloCS = 0, balanceHaberSoloCS = 0;
                        Int64 balanceDebeSoloUS = 0, balanceHaberSoloUS = 0;
						string txtCuenta = numCuentaDetalle.ToString() + " - " + nombreCuentaDetalle;
                        //
                        if (!conC.IsDBNull(15)) balanceDebe = conC.GetInt64(15);
                        if (!conC.IsDBNull(16)) balanceHaber = conC.GetInt64(16);
						//
                        if (!conC.IsDBNull(17)) balanceDebeSoloCS = conC.GetInt64(17);
                        if (!conC.IsDBNull(18)) balanceHaberSoloCS = conC.GetInt64(18);
						//
                        if (!conC.IsDBNull(19)) balanceDebeSoloUS = conC.GetInt64(19);
                        if (!conC.IsDBNull(20)) balanceHaberSoloUS = conC.GetInt64(20);
						//
						if (_treeShowBalances) {
							if (_treeShowBalancesSep) {
                                bool mostrarCS = (balanceDebeSoloCS != 0 || balanceHaberSoloCS != 0);
                                bool mostrarUS = (balanceDebeSoloUS != 0 || balanceHaberSoloUS != 0);
                                if (mostrarCS || mostrarUS) {
                                    bool isOpen = false;
                                    txtCuenta += " (";
                                    if (mostrarCS) {
                                        if (isOpen) txtCuenta += ", "; isOpen = true;
                                        txtCuenta += "C$" + String.Format("{0:0,0.00}", (double)(balanceDebeSoloCS - balanceHaberSoloCS) / 100.0);
                                    }
                                    if (mostrarUS) {
                                        if (isOpen) txtCuenta += ", "; isOpen = true;
                                        txtCuenta += "U$" + String.Format("{0:0,0.00}", (double)(balanceDebeSoloUS - balanceHaberSoloUS) / 100.0);
                                    }
                                    txtCuenta += ")";
                                }
                            } else {
								bool mostrar = (balanceDebe != 0 || balanceHaber != 0);
								if (mostrar) {
                                    bool isOpen = false;
                                    txtCuenta += " (";
                                    if (mostrar) {
                                        if (isOpen) txtCuenta += ", "; isOpen = true;
                                        txtCuenta += "C$" + String.Format("{0:0,0.00}", (double)(balanceDebe - balanceHaber) / 100.0);
                                    }
                                    txtCuenta += ")";
                                }
							}
						}
						//
                        nodoCuentaDetalle = nodoCuentaSubSubSub.Nodes.Add("cuentaDetalle_" + idCuentaDetalle, txtCuenta, "folderdatos.gif"); nodoCuentaDetalle.SelectedImageKey = nodoCuentaDetalle.ImageKey;
						nodoCuentaDetalle.Tag = "cuentaDetalle_" + idCuentaDetalle;
						ultimoIDCuentaDetalle = idCuentaDetalle;
					}
				}
			}
			conC.Close();
			nodoTodasLasCuentas.Expand();
            if (nodoCuenta != null && nodoCuentaExpand) nodoCuenta.Expand();
            if (nodoCuentaSub != null && nodoCuentaSubExpand) nodoCuentaSub.Expand();
            if (nodoCuentaSubSub != null && nodoCuentaSubSubExpand) nodoCuentaSubSub.Expand();
            if (nodoCuentaSubSubSub != null && nodoCuentaSubSubSubExpand) nodoCuentaSubSubSub.Expand();
            arbCuentas.EndUpdate(); //previene parpadeos
			arbCuentas.Refresh();
		}

		private bool processMovs(bool createRecords, bool showMsgBoxes, ref string dstComprobante) {
			bool r = false;
            Int64 sumaDebeCS = 0, sumaHaberCS = 0; string erroresValidacion = "";
            int centsDeltaUSRemain = 0, rowsUSRemain = 0;
			//
			dstComprobante = "";
			//
            if (!procesarGridMovimientos(null, ref sumaDebeCS, ref sumaHaberCS, ref erroresValidacion, true, ref centsDeltaUSRemain, ref rowsUSRemain, 0, 0)) {
				int centsAutoajustarMax = 100;
				if (centsDeltaUSRemain >= -centsAutoajustarMax && centsDeltaUSRemain <= centsAutoajustarMax && rowsUSRemain > 0) {
                    int centsDeltaUSRemain2 = 0, rowsUSRemain2 = 0; erroresValidacion = "";
                    if (!procesarGridMovimientos(null, ref sumaDebeCS, ref sumaHaberCS, ref erroresValidacion, true, ref centsDeltaUSRemain2, ref rowsUSRemain2, centsDeltaUSRemain, rowsUSRemain)) {
                        if(showMsgBoxes) MessageBox.Show("La validacion ha fallado aun despues de ajustar " + centsDeltaUSRemain + " centavos en " + rowsUSRemain + " filas dolares: " + erroresValidacion);
                        lblValidationResult.Text = erroresValidacion;
                        lblValidationResult.ForeColor = Color.Red;
                    } else {
                        if(showMsgBoxes) MessageBox.Show("Validacion superada despues de ajustar " + centsDeltaUSRemain + " centavos en " + rowsUSRemain + " filas dolares.");
                        lblValidationResult.Text = "Autoajustados " + centsDeltaUSRemain + " centavos aplicados en las filas en dolares.";
                        lblValidationResult.ForeColor = Color.Blue;
						r = true;
                    }
                } else {
                    if(showMsgBoxes) MessageBox.Show("La validacion ha fallado: " + erroresValidacion);
                    lblValidationResult.Text = erroresValidacion;
                    lblValidationResult.ForeColor = Color.Red;
                }
            } else {
                if (showMsgBoxes) MessageBox.Show("Validacion superada sin ajustes.");
                lblValidationResult.Text = "Validacion superada";
                lblValidationResult.ForeColor = Color.Black;
				r = true;

            }
            lblSumaDebe.Text = String.Format("{0:0,0.00}", (double)sumaDebeCS / 100.0);
            lblSumaHaber.Text = String.Format("{0:0,0.00}", (double)sumaHaberCS / 100.0);
            //create record
            if (r && createRecords) {
				if (MessageBox.Show("Confirma que desea registrar los movimientos?", "Registrar movimientos?", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    string idComprobante = Global.generaSiguienteComprobante(fechaNuevoMov.Value);
                    if (idComprobante.Length <= 0) {
                        if (showMsgBoxes) MessageBox.Show("No se pudo crear el nuevo comprobante.");
                        lblValidationResult.Text = "No se pudo crear el nuevo comprobante.";
                        lblValidationResult.ForeColor = Color.Red;
                        r = false;
                    } else {
                        int centsDeltaUSRemain2 = 0, rowsUSRemain2 = 0; erroresValidacion = "";
						if (!procesarGridMovimientos(idComprobante, ref sumaDebeCS, ref sumaHaberCS, ref erroresValidacion, false, ref centsDeltaUSRemain2, ref rowsUSRemain2, centsDeltaUSRemain, rowsUSRemain)) {
                            if (showMsgBoxes) MessageBox.Show("El registro ha fallado: " + erroresValidacion + ".\n\nNingun movimiento fue registrado, puede reintentar el registro sin preocuparse de duplicar registros.");
                            lblValidationResult.Text = erroresValidacion;
                            lblValidationResult.ForeColor = Color.Red;
                            r = false;
                        } else if (erroresValidacion != "") {
                            if (showMsgBoxes) MessageBox.Show("Se registraron los datos pero se recibio el mensaje: " + erroresValidacion);
                        }
						dstComprobante = idComprobante;
                    }
                } else {
                    lblValidationResult.Text = "Usuario ha cancelado la accion.";
                    lblValidationResult.ForeColor = Color.Red;
                    r = false; //user canceled
                }
            }
            return r;
		}

		private void btnValidaMov_Click(object sender, EventArgs e) {
            string nvoComprobante = "";
            this.processMovs(false, true, ref nvoComprobante);
        }

        private void gridMovimientos_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            string nvoComprobante = "";
            this.processMovs(false, false, ref nvoComprobante);
        }

        private void btnLimpiarMovs_Click(object sender, EventArgs e) {
            if (MessageBox.Show("Desea vaciar la grilla de valores?", "Limpiar grilla", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                string nvoComprobante = "";
                gridMovimientos.Rows.Clear();
                this.processMovs(false, false, ref nvoComprobante);
            }
        }

        private void btnFinalizarNuevoMov_Click(object sender, EventArgs e) {
            string nvoComprobante = "";
            if (gridMovimientos.Rows.Count == 0) {
				gridMovimientos.Rows.Clear();
                this.processMovs(false, false, ref nvoComprobante);
            } else if (this.processMovs(true, true, ref nvoComprobante)) {
                MessageBox.Show("Comprobante registrado: " + nvoComprobante);
                gridMovimientos.Rows.Clear();
                this.processMovs(false, false, ref nvoComprobante);
            }
		}

		private void btnNomina_Click(object sender, EventArgs e) {
			//ToDo: remove or re-enable
			/*
			FrmMovNomina formu = new FrmMovNomina();
			formu.ShowDialog();
			//
			if (!formu.cancelada()) {
				string descripcion = formu.descripcion();
				//llenar grilla
				gridMovimientos.Rows.Clear();
				int indice = 0; string numCuenta = ""; double valor = 0; bool esDebe = false;
				do {
					formu.dameValor(indice, ref numCuenta, ref valor, ref esDebe);
					if (numCuenta != "" && valor != 0) {
						//obtener el nombre de la cuenta
						string nombreCuenta = "";
						OdbcCommand ssql = new OdbcCommand("SELECT nombreCuentaDetalle FROM CuentasDetalle WHERE idCuentaDetalle='" + numCuenta + "'", Global.conn);
						OdbcDataReader conC = ssql.ExecuteReader();
						if (conC.Read()) {
							nombreCuenta = conC.GetString(0);
						}
						conC.Close();
						//agregar fila
						DataGridViewTextBoxCell txtCelda;
						DataGridViewCheckBoxCell chkCelda;
						DataGridViewRow fila = new DataGridViewRow();
						//numero de cuenta
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = numCuenta;
						fila.Cells.Add(txtCelda);
						//nombre de cuenta
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = nombreCuenta;
						fila.Cells.Add(txtCelda);
						//debe
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = (esDebe ? String.Format("{0:0.00}", valor) : "");
						fila.Cells.Add(txtCelda);
						//haber
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = (!esDebe ? String.Format("{0:0.00}", valor) : "");
						fila.Cells.Add(txtCelda);
						//check 'es dolar'
						chkCelda = new DataGridViewCheckBoxCell(); chkCelda.Value = false;
						fila.Cells.Add(chkCelda);
						//tasa cambio
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "";
						fila.Cells.Add(txtCelda);
						//descripcion
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = descripcion;
						fila.Cells.Add(txtCelda);
						//
						gridMovimientos.Rows.Add(fila);
					}
					indice++;
				} while (numCuenta != "");
			}
			//
			formu = null;
			*/
		}

		private bool procesarGridMovimientos(string idComprobanteDiario, ref Int64 guardarSumaDebeCSEn, ref Int64 guardarSumaHaberCSEn, ref string guardarErroresEn, bool esPrueba, ref int centsDeltaUSlcl, ref int rowsUSCountLcl, int centsDeltaUSRemain, int rowsUSRemain) {
			bool exito = true;
			centsDeltaUSlcl = 0;
			rowsUSCountLcl = 0;
			guardarSumaDebeCSEn = 0;
			guardarSumaHaberCSEn = 0;
			int conteoFilas = gridMovimientos.Rows.Count;
			string cuentasDetallesAfectadas = ""; //lista en formato '00001...', '00002...' para usar en un WHERE IN.
            //
			OdbcCommand ssql;
            OdbcDataReader conC;
            int strSQLMovsCount = 0;
			string strSQL = "";
            strSQL += " INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) ";
            strSQL += " VALUES ";
			//
            if (conteoFilas!=0){
				int indiceFila;
				for (indiceFila = 0; indiceFila < conteoFilas; indiceFila++) {
					bool filaValida = true;
					DataGridViewRow fila = gridMovimientos.Rows[indiceFila];
					string idCuentaDetalle = ""; if (fila.Cells.Count >= 1) if (fila.Cells[0].Value != null) idCuentaDetalle = fila.Cells[0].Value.ToString();
					string nombreCuentaDetalle = ""; if (fila.Cells.Count >= 2) if (fila.Cells[1].Value != null) nombreCuentaDetalle = fila.Cells[1].Value.ToString();
					bool debeEsValido = false; Int64 debe = 0, debeCS = 0, debeUS = 0;
					string strDebe = ""; if (fila.Cells.Count >= 3) if (fila.Cells[2].Value != null) strDebe = fila.Cells[2].Value.ToString();
					bool haberEsValido = false; Int64 haber = 0, haberCS = 0, haberUS = 0;
					string strHaber = ""; if (fila.Cells.Count >= 4) if (fila.Cells[3].Value != null) strHaber = fila.Cells[3].Value.ToString();
					bool esDolar = false; if (fila.Cells.Count >= 5) if (fila.Cells[4].Value != null) esDolar = Convert.ToBoolean(fila.Cells[4].Value);
					bool tasaEsValida = false; Int64 tasaCambio = 0;
					string strTasaCambio = ""; if (fila.Cells.Count >= 6) if (fila.Cells[5].Value != null) strTasaCambio = fila.Cells[5].Value.ToString();
					string strReferencia = ""; if (fila.Cells.Count >= 7) if (fila.Cells[6].Value != null) strReferencia = fila.Cells[6].Value.ToString();
					if (strDebe != "" || strHaber != "") {
						if (strDebe != "") {
							try {
								debe = (Int64)Math.Round(double.Parse(strDebe) * 100.0, 2);
								debeEsValido = true;
							} catch (Exception) { }
						}
						if (strHaber != "") {
							try {
								haber = (Int64)Math.Round(double.Parse(strHaber) * 100.0, 2);
								haberEsValido = true;
							} catch (Exception) { }
						}
						if (strTasaCambio != "") {
							try {
								tasaCambio = (Int64)Math.Round(double.Parse(strTasaCambio) * 10000, 4);
								tasaEsValida = true;
							} catch (Exception) { }
						}
						if (idCuentaDetalle == "" && nombreCuentaDetalle == "" && !debeEsValido && !haberEsValido && !tasaEsValida) {
							//Ignorar filas que no tienen datos
						} else {
							//Verificar cuenta detalle
							if(!esPrueba) {
								if (idCuentaDetalle.Length <= 0) {
                                    filaValida = false; 
                                    guardarErroresEn += " (fila " + indiceFila + ") CuentaDetalle no specificada.";
                                } else {
									ssql = new OdbcCommand("SELECT idCuentaDetalle FROM CuentasDetalle WHERE idCuentaDetalle='" + idCuentaDetalle + "'", Global.conn);
									conC = ssql.ExecuteReader();
									if (!conC.Read()) {
										filaValida = false; //no existe
															//MessageBox.Show("La CuentaDetalle '" + idCuentaDetalle + "' no existe. IndiceFila(" + indiceFila + ")");
										guardarErroresEn += " (fila " + indiceFila + ") CuentaDetalle no existe.";
									}
									conC.Close();
								}
							}
							//
							if (esDolar && !tasaEsValida) {
								filaValida = false; //no tiene la tasa de cambio valida
								guardarErroresEn += " (fila " + indiceFila + ") Tasa no especificada.";
								//MessageBox.Show("No se especifico tasa de cambio valida. IndiceFila(" + indiceFila + ")");
							}
							//
							if ((debeEsValido && haberEsValido) || (!debeEsValido && !haberEsValido)) {
								filaValida = false; //no tiene debe ni haber, o tiene ambos
								guardarErroresEn += " (fila " + indiceFila + ") No tiene debe/haber o tiene ambos.";
								//MessageBox.Show("Tiene Debe y Haber o Ninguno de los dos. IndiceFila(" + indiceFila + ")");
							} else {
								if (debeEsValido) {
									if (esDolar) {
										debeCS = Global.convertCents(debe, tasaCambio); //tasaCambio is 4 digits, debe/haber is 2 digits
                                        debeUS = debe;
										//apply adjustment cents
										if (rowsUSRemain > 0) {
											if (centsDeltaUSRemain > 0) {
												if (rowsUSRemain == 1) {
													debeCS -= centsDeltaUSRemain;
													centsDeltaUSRemain = 0;
												} else {
													debeCS -= 1;
													centsDeltaUSRemain--;
												}
											} else if (centsDeltaUSRemain < 0) {
												if (rowsUSRemain == 1) {
													debeCS += -centsDeltaUSRemain;
													centsDeltaUSRemain = 0;
												} else {
													debeCS += 1;
													centsDeltaUSRemain++;
												}
											}
											rowsUSRemain--;
										}
										rowsUSCountLcl++;
										//
										//balanceHaberUS -= debeUS;
										//if (balanceHaberUS < 0) {
										//	balanceDebeUS = balanceDebeUS + (-balanceHaberUS);
										//	balanceHaberUS = 0;
										//	//guardarErroresEn += " (fila " + indiceFila + ") El balanceHaberUS quedaria en negativo.";
										//	//filaValida = false;
										//}
									} else {
										debeCS = debe;
										debeUS = 0;
										//balanceHaberCS -= debeCS;
										//if (balanceHaberCS < 0) {
										//	balanceDebeCS = balanceDebeCS + (-balanceHaberCS);
										//	balanceHaberCS = 0;
										//	//guardarErroresEn += " (fila " + indiceFila + ") El balanceHaberCS quedaria en negativo.";
										//	//filaValida = false;
										//}
									}
								} else if (haberEsValido) {
									if (esDolar) {
										haberCS = Global.convertCents(haber, tasaCambio); //tasaCambio is 4 digits, debe/haber is 2 digits
                                        haberUS = haber;
										//apply adjustment cents
										if (rowsUSRemain > 0) {
											if (centsDeltaUSRemain > 0) {
												if (rowsUSRemain == 1) {
													haberCS += centsDeltaUSRemain;
													centsDeltaUSRemain = 0;
												} else {
													haberCS += 1;
													centsDeltaUSRemain--;
												}
											} else if (centsDeltaUSRemain < 0) {
												if (rowsUSRemain == 1) {
													haberCS -= -centsDeltaUSRemain;
													centsDeltaUSRemain = 0;
												} else {
													haberCS -= 1;
													centsDeltaUSRemain++;
												}
											}
											rowsUSRemain--;
										}
										rowsUSCountLcl++;
										//
										//balanceDebeUS -= haberUS;
										//if (balanceDebeUS < 0) {
										//	balanceHaberUS = balanceHaberUS + (-balanceDebeUS);
										//	balanceDebeUS = 0;
										//	//guardarErroresEn += " (fila " + indiceFila + ") El balanceDebeUS quedaria en negativo.";
										//	//filaValida = false;
										//}
									} else {
										haberCS = haber;
										haberUS = 0;
										//balanceDebeCS -= haberCS;
										//if (balanceDebeCS < 0) {
										//	balanceHaberCS = balanceHaberCS + (-balanceDebeCS);
										//	balanceDebeCS = 0;
										//	//guardarErroresEn += " (fila " + indiceFila + ") El balanceDebeCS quedaria en negativo.";
										//	//filaValida = false;
										//}
									}
								}
							}
							//
							if (!filaValida) {
								exito = false;
							} else {
								if (!esPrueba) {
									if(strSQLMovsCount > 0) strSQL += ", ";
                                    strSQL += "(";
									strSQL += " '" + idComprobanteDiario + "'";
									strSQL += ", '" + idCuentaDetalle + "'";
									strSQL += ", " + (debeEsValido ? debeCS.ToString() : "NULL") + "";
									strSQL += ", " + (haberEsValido ? haberCS.ToString() : "NULL") + "";
									strSQL += ", " + (esDolar ? 1 : 0) + "";
									strSQL += ", " + (tasaEsValida ? tasaCambio.ToString() : "NULL") + "";
									strSQL += ", " + (debeEsValido && esDolar ? debeUS.ToString() : "NULL") + "";
									strSQL += ", " + (haberEsValido && esDolar ? haberUS.ToString() : "NULL") + "";
									strSQL += ", '" + strReferencia + "'";
									strSQL += " )";
									strSQLMovsCount++;
                                    //
									if (cuentasDetallesAfectadas.IndexOf("'" + idCuentaDetalle + "'") <= 0) {
										if (cuentasDetallesAfectadas.Length > 0) cuentasDetallesAfectadas += ", ";
										cuentasDetallesAfectadas += "'" + idCuentaDetalle + "'";
                                    }
								}
							}
						}
						//
						guardarSumaDebeCSEn += debeCS;
						guardarSumaHaberCSEn += haberCS;
					}
				}
			}
			if (exito) {
                Int64 diff = guardarSumaDebeCSEn - guardarSumaHaberCSEn;
                if (diff != 0) {
					exito = false;
					//MessageBox.Show("SumaDebeC$ no concuerda con la SumaHaberC$.");
                    guardarErroresEn += " Existe una diferencia en el Debe y Haber de " + String.Format("{0:0,0.00}", diff / 100.0) + ".";
				}
				centsDeltaUSlcl = (rowsUSCountLcl == 0 ? 0 : (int)diff);
				if (exito && !esPrueba) {
                    ssql = new OdbcCommand(strSQL, Global.conn);
                    ssql.ExecuteNonQuery();
                }
            }
			//actualizar balances de cuentas afectadas
			if (cuentasDetallesAfectadas.Length > 0) {
				Global.actualizarBalanceDeCuentas(cuentasDetallesAfectadas);
            }
			return exito;
		}

		private void gridMovimientos_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (e.ColumnIndex == 1) {
                //seleccionar cuenta
                ventanaSelCuentaDetalle.ShowDialog();
				string idCuentaDetalleSel = ventanaSelCuentaDetalle.dameIdCuentaDetalleSel();
				string nombreCuentaDetalleSel = ventanaSelCuentaDetalle.dameNombreCuentaDetalleSel();
				if (idCuentaDetalleSel != "") {
					gridMovimientos.Rows[e.RowIndex].Cells[0].Value = idCuentaDetalleSel;
					gridMovimientos.Rows[e.RowIndex].Cells[1].Value = nombreCuentaDetalleSel;
				}
			} else if (e.ColumnIndex == 5) {
				//tasa de cambio
				bool tasaVacia = false;
				if(gridMovimientos.Rows[e.RowIndex].Cells[5].Value == null){
					tasaVacia = true;
				} else {
					tasaVacia = (gridMovimientos.Rows[e.RowIndex].Cells[5].Value.ToString().Length == 0);
				}
				if (tasaVacia) {
                    _tasaCambioTask = Global.queryTasaCambio_bcn_gob_niAsync(fechaNuevoMov.Value);
					_tasaCambioRowIndex = e.RowIndex;
				}
			}
		}

		private void btnOKDiario_Click(object sender, EventArgs e) {
			DateTime fechaIni = fechaIniDiario.Value;
			DateTime fechaFin = fechaFinDiario.Value;
			if (fechaFin < fechaIni) {
				DateTime fechaTmp = fechaIni;
				fechaIni = fechaFin;
				fechaFin = fechaTmp;
			}
			//
			lstComprobantes.BeginUpdate();
			lstComprobantes.Items.Clear();
			lstComprobantes.Groups.Clear();
			string strSQL = "";
			strSQL += " SELECT Cuentas.idCuenta, Cuentas.nombreCuenta, ";
			strSQL += " CuentasSub.idCuentaSub, CuentasSub.nombreCuentaSub, ";
			strSQL += " CuentasSubSub.idCuentaSubSub, CuentasSubSub.nombreCuentaSubSub, ";
			strSQL += " CuentasSubSubSub.idCuentaSubSubSub, CuentasSubSubSub.nombreCuentaSubSubSub, ";
			strSQL += " CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle, ";
			strSQL += " ComprobantesDeDiario.idComprobanteDiario, ComprobantesDeDiario.fechaComprobanteDiario, ";
			strSQL += " Movimientos.idMovimiento, Movimientos.montoDebeCS, Movimientos.montoHaberCS, Movimientos.esDolares, Movimientos.tasaCambio, Movimientos.montoDebeUS, Movimientos.montoHaberUS, Movimientos.referencia ";
			strSQL += " FROM ((((Cuentas INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
			strSQL += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
			strSQL += " INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
			strSQL += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub) ";
			strSQL += " INNER JOIN (ComprobantesDeDiario INNER JOIN Movimientos ON ComprobantesDeDiario.idComprobanteDiario = Movimientos.idComprobanteDiario) ON CuentasDetalle.idCuentaDetalle = Movimientos.idCuentaDetalle";
            if (sender == null || e == null) {
                strSQL += " WHERE ComprobantesDeDiario.idComprobanteDiario = '" + idComprobanteBuscar + "' ";
            } else {
                strSQL += " WHERE ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
                idMovimientoResaltar = 0;
                idComprobanteBuscar = "";
            }
			strSQL += " ORDER BY ComprobantesDeDiario.idComprobanteDiario, CuentasDetalle.idCuentaDetalle ";
			OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
			OdbcDataReader conC = ssql.ExecuteReader();
			string ultimoIdComprobante = "";
			Int64 sumaDebeCS = 0, sumaHaberCS = 0, sumaDebeUS = 0, sumaHaberUS = 0;
			int conteoFilas = 0;
			ListViewGroup grupo = null;
			while (conC.Read()) {
				conteoFilas++;
				string idCuenta = conC.GetString(0);
				string nombreCuenta = conC.GetString(1);
				string idCuentaSub = conC.GetString(2);
				string nombreCuentaSub = conC.GetString(3);
				string idCuentaSubSub = conC.GetString(4);
				string nombreCuentaSubSub = conC.GetString(5);
				string idCuentaSubSubSub = conC.GetString(6);
				string nombreCuentaSubSubSub = conC.GetString(7);
				string idCuentaDetalle = conC.GetString(8);
				string nombreCuentaDetalle = conC.GetString(9);
				string idComprobanteDiario = conC.GetString(10);
				DateTime fechaComprobanteDiario = conC.GetDateTime(11);
				int idMovimiento = conC.GetInt32(12);
				Int64 montoDebeCS = 0; if (!conC.IsDBNull(13)) montoDebeCS = conC.GetInt64(13);
                Int64 montoHaberCS = 0; if (!conC.IsDBNull(14)) montoHaberCS = conC.GetInt64(14);
				bool esDolares = conC.GetBoolean(15);
                Int64 tasaCambio = 0; if (!conC.IsDBNull(16)) tasaCambio = conC.GetInt64(16);
                Int64 montoDebeUS = 0; if (!conC.IsDBNull(17)) montoDebeUS = conC.GetInt64(17);
                Int64 montoHaberUS = 0; if (!conC.IsDBNull(18)) montoHaberUS = conC.GetInt64(18);
				string referencia = conC.GetString(19);
				//
				if(idComprobanteDiario!=ultimoIdComprobante){
					//total del grupo anterior
					if (grupo != null) {
						ListViewItem itmGrp = lstComprobantes.Items.Add("-"); itmGrp.Font = new Font(itmGrp.Font, FontStyle.Bold);
						itmGrp.Group = grupo;
						ListViewItem.ListViewSubItem sitmGrp;
						sitmGrp = itmGrp.SubItems.Add("TOTAL"); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("");
						sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("");
					}
					//nuevo grupo
					grupo = new ListViewGroup(idComprobanteDiario);
					sumaDebeCS = 0; sumaHaberCS = 0; sumaDebeUS = 0; sumaHaberUS = 0;
					lstComprobantes.Groups.Add(grupo);
					ultimoIdComprobante = idComprobanteDiario;
				}
				ListViewItem itm = lstComprobantes.Items.Add(conteoFilas.ToString(), idCuentaDetalle, "FlechaINOUT16(2).gif");
				itm.BackColor = (idMovimientoResaltar == idMovimiento ? Color.Yellow : Color.White);
				itm.Tag = idMovimiento;
				itm.Group = grupo;
				itm.ToolTipText = nombreCuenta + " / " + nombreCuentaSub + " / " + nombreCuentaSubSub + " / " + nombreCuentaSubSubSub + " / " + nombreCuentaDetalle;
				ListViewItem.ListViewSubItem sitm;
				sitm = itm.SubItems.Add(nombreCuentaDetalle);
				if (montoDebeCS == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("C$"+String.Format("{0:0,0.00}", montoDebeCS / 100.0));
					sumaDebeCS += montoDebeCS; 
				}
				if (montoHaberCS == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", montoHaberCS / 100.0));
					sumaHaberCS += montoHaberCS;
				}
				if (tasaCambio == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add(String.Format("{0:0,0.0000}", tasaCambio / 10000.0));
				}
				if (montoDebeUS == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", montoDebeUS / 100.0));
					sumaDebeUS += montoDebeUS;
				}
				if (montoHaberUS == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", montoHaberUS / 100.0));
					sumaHaberUS += montoHaberUS;
				}
				sitm = itm.SubItems.Add(referencia);
			}
			if (grupo != null) {
				ListViewItem itmGrp = lstComprobantes.Items.Add("-"); itmGrp.Font = new Font(itmGrp.Font, FontStyle.Bold);
				itmGrp.Group = grupo;
				ListViewItem.ListViewSubItem sitmGrp;
				sitmGrp = itmGrp.SubItems.Add("TOTAL"); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("");
				sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("");
			}
			conC.Close();
			//
			lstComprobantes.EndUpdate();
			lstComprobantes.Refresh();
		}

		private struct STCuentaSaldos {
			public string idCuenta;
			public Decimal debeCS;	//result of SUM(), Decimal on integer values
			public Decimal haberCS; //result of SUM(), Decimal on integer values
            public Decimal debeUS;  //result of SUM(), Decimal on integer values
            public Decimal haberUS; //result of SUM(), Decimal on integer values
        };

		private void btnOKAuxiliar_Click(object sender, EventArgs e) {
			DateTime fechaIni = fechaIniAuxiliar.Value;
			DateTime fechaFin = fechaFinAuxiliar.Value;
            bool ocultarDolaresCorobizados = chkOcultarDolares.Checked; //Util para saldo de tarjetas de credito
			if (fechaFin < fechaIni) {
				DateTime fechaTmp = fechaIni;
				fechaIni = fechaFin;
				fechaFin = fechaTmp;
			}
			//
            Font fuenteNegrilla = new Font(lstAuxiliar.Font, FontStyle.Bold);
			lstAuxiliar.BeginUpdate();
			lstAuxiliar.Items.Clear();
			lstAuxiliar.Groups.Clear();
			string strSQL = ""; OdbcCommand ssql; OdbcDataReader conC;
			//Obtener los saldos antes del periodo
			int arrSaldosIniUso = 0; int arrSaldosCrecimiento = 1;
			STCuentaSaldos[] arrSaldosIniciales = new STCuentaSaldos[arrSaldosCrecimiento];
			strSQL = "";
			strSQL += " SELECT CuentasDetalle.idCuentaDetalle, ";
			strSQL += " SUM(Movimientos.montoDebeCS), SUM(Movimientos.montoHaberCS), SUM(Movimientos.montoDebeUS), SUM(Movimientos.montoHaberUS) ";
			strSQL += " FROM CuentasDetalle INNER JOIN (ComprobantesDeDiario INNER JOIN Movimientos ON ComprobantesDeDiario.idComprobanteDiario = Movimientos.idComprobanteDiario) ON CuentasDetalle.idCuentaDetalle = Movimientos.idCuentaDetalle ";
			strSQL += " WHERE ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
			if (sender == null || e == null) {
				strSQL += " AND CuentasDetalle.idCuentaDetalle='" + idCuentaAuxiliarBuscar + "'";
			}
			strSQL += " GROUP BY CuentasDetalle.idCuentaDetalle";
			ssql = new OdbcCommand(strSQL, Global.conn);
			conC = ssql.ExecuteReader();
			while (conC.Read()) {
				STCuentaSaldos saldo;
				saldo.idCuenta	= conC.GetString(0);
				saldo.debeCS	= 0; if (!conC.IsDBNull(1)) saldo.debeCS	= conC.GetDecimal(1); //SUM(...) returns decimal on integer values
				saldo.haberCS	= 0; if (!conC.IsDBNull(2)) saldo.haberCS	= conC.GetDecimal(2); //SUM(...) returns decimal on integer values
                saldo.debeUS	= 0; if (!conC.IsDBNull(3)) saldo.debeUS	= conC.GetDecimal(3); //SUM(...) returns decimal on integer values
                saldo.haberUS	= 0; if (!conC.IsDBNull(4)) saldo.haberUS	= conC.GetDecimal(4); //SUM(...) returns decimal on integer values
                if (arrSaldosIniciales.Length == arrSaldosIniUso) {
					STCuentaSaldos[] arrSaldosInicialesNvo = new STCuentaSaldos[arrSaldosIniciales.Length + arrSaldosCrecimiento];
					int i;
					for (i = 0; i < arrSaldosIniUso; i++ ) {
						arrSaldosInicialesNvo[i] = arrSaldosIniciales[i];
					}
					arrSaldosIniciales = arrSaldosInicialesNvo;
				}
				arrSaldosIniciales[arrSaldosIniUso++] = saldo;
			}
			conC.Close();
			//MessageBox.Show("Arreglo de saldos iniciales, generado con " + arrSaldosIniUso + " cuentas.");
			//Obtener los detalles
			strSQL = "";
			strSQL += " SELECT Cuentas.idCuenta, Cuentas.nombreCuenta, ";
			strSQL += " CuentasSub.idCuentaSub, CuentasSub.nombreCuentaSub, ";
			strSQL += " CuentasSubSub.idCuentaSubSub, CuentasSubSub.nombreCuentaSubSub, ";
			strSQL += " CuentasSubSubSub.idCuentaSubSubSub, CuentasSubSubSub.nombreCuentaSubSubSub, ";
			strSQL += " CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle, ";
			strSQL += " ComprobantesDeDiario.idComprobanteDiario, ComprobantesDeDiario.fechaComprobanteDiario, ";
			strSQL += " Movimientos.idMovimiento, Movimientos.montoDebeCS, Movimientos.montoHaberCS, Movimientos.esDolares, Movimientos.tasaCambio, Movimientos.montoDebeUS, Movimientos.montoHaberUS, Movimientos.referencia ";
			strSQL += " FROM ((((Cuentas INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
			strSQL += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
			strSQL += " INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
			strSQL += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub) ";
			strSQL += " INNER JOIN (ComprobantesDeDiario INNER JOIN Movimientos ON ComprobantesDeDiario.idComprobanteDiario = Movimientos.idComprobanteDiario) ON CuentasDetalle.idCuentaDetalle = Movimientos.idCuentaDetalle";
			string ordenPrioridad = "";
			if (sender == null || e == null) {
                strSQL += " WHERE CuentasDetalle.idCuentaDetalle='" + idCuentaAuxiliarBuscar + "'";
				
				if (idCuentaAuxiliarBuscar.Length > 2) { 
					string first2 = idCuentaAuxiliarBuscar.Substring(0, 2);
					if (first2 == "01" || first2 == "05" || first2 == "06" || first2 == "07") {
						/*prioridad debe (el balance de la cuenta debe mantenerse positivo):
						01 (activos)
						05 (costos)
						06 (gastos)
						07 (gastos no deducibles)*/
						ordenPrioridad = "debe";
					} else if (first2 == "02" || first2 == "04") {
						/*prioridad haber (el balance de la cuenta debe mantenerse negativo):
						02 (pasivos)
						04 (ingresos)*/
						ordenPrioridad = "haber";
					}
				}
				
            } else {
                strSQL += " WHERE ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
                idCuentaAuxiliarBuscar = "";
                idMovimientoResaltar = 0;
            }
			if (ordenPrioridad == "debe") {
				strSQL += " ORDER BY CuentasDetalle.idCuentaDetalle, ComprobantesDeDiario.fechaComprobanteDiario, Movimientos.montoDebeCS DESC, Movimientos.montoHaberCS DESC ";
			} else if (ordenPrioridad == "haber") {
				strSQL += " ORDER BY CuentasDetalle.idCuentaDetalle, ComprobantesDeDiario.fechaComprobanteDiario, Movimientos.montoHaberCS DESC, Movimientos.montoDebeCS DESC ";
			} else {
				strSQL += " ORDER BY CuentasDetalle.idCuentaDetalle, ComprobantesDeDiario.fechaComprobanteDiario, ComprobantesDeDiario.idComprobanteDiario ";
			}
			ssql = new OdbcCommand(strSQL, Global.conn);
			conC = ssql.ExecuteReader();
            Color coloFondoFila = Color.WhiteSmoke;
            string ultimoIdComprobante = "";
            string ultimoIdCuentaDetalle = "";
            Int64 saldoCS = 0, saldoUS = 0;
            Int64 sumaDebeCS = 0, sumaHaberCS = 0, sumaDebeUS = 0, sumaHaberUS = 0;
			int conteoFilas = 0;
			ListViewGroup grupo = null; bool fechaComprobanteAntInicializada = false; DateTime fechaComprobanteAnt = DateTime.Now;
			while (conC.Read()) {
				conteoFilas++;
				string idCuenta = conC.GetString(0);
				string nombreCuenta = conC.GetString(1);
				string idCuentaSub = conC.GetString(2);
				string nombreCuentaSub = conC.GetString(3);
				string idCuentaSubSub = conC.GetString(4);
				string nombreCuentaSubSub = conC.GetString(5);
				string idCuentaSubSubSub = conC.GetString(6);
				string nombreCuentaSubSubSub = conC.GetString(7);
				string idCuentaDetalle = conC.GetString(8);
				string nombreCuentaDetalle = conC.GetString(9);
				string idComprobanteDiario = conC.GetString(10);
				DateTime fechaComprobanteDiario = conC.GetDateTime(11);
				int idMovimiento = conC.GetInt32(12);
                Int64 montoDebeCS = 0; if (!conC.IsDBNull(13)) montoDebeCS = conC.GetInt64(13);
                Int64 montoHaberCS = 0; if (!conC.IsDBNull(14)) montoHaberCS = conC.GetInt64(14);
				bool esDolares = conC.GetBoolean(15);
                Int64 tasaCambio = 0; if (!conC.IsDBNull(16)) tasaCambio = conC.GetInt64(16);
                Int64 montoDebeUS = 0; if (!conC.IsDBNull(17)) montoDebeUS = conC.GetInt64(17);
                Int64 montoHaberUS = 0; if (!conC.IsDBNull(18)) montoHaberUS = conC.GetInt64(18);
				string referencia = conC.GetString(19);
				string notasAutomatica = "";
				//
				if (idCuentaDetalle != ultimoIdCuentaDetalle) {
					//total del grupo anterior
					if (grupo != null) {
						ListViewItem itmGrp = lstAuxiliar.Items.Add("-"); itmGrp.Font = new Font(itmGrp.Font, FontStyle.Bold);
						itmGrp.UseItemStyleForSubItems = false;
						itmGrp.ForeColor = Color.Black;
						itmGrp.Group = grupo;
						ListViewItem.ListViewSubItem sitmGrp;
						sitmGrp = itmGrp.SubItems.Add("TOTAL"); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", saldoCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);  sitmGrp.ForeColor = (saldoCS > 0 ? Color.DarkBlue : saldoCS < 0 ? Color.DarkRed : Color.Gray);
						sitmGrp = itmGrp.SubItems.Add("");
						sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
						sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", saldoUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);  sitmGrp.ForeColor = (saldoUS > 0 ? Color.DarkBlue : saldoUS < 0 ? Color.DarkRed : Color.Gray);
						sitmGrp = itmGrp.SubItems.Add("");
					}
					//nuevo grupo
					grupo = new ListViewGroup(idCuentaDetalle + " - " + nombreCuentaDetalle + " (" + nombreCuenta  + " / " + nombreCuentaSub + " / " + nombreCuentaSubSub + " / " + nombreCuentaSubSubSub + ")");
					saldoCS = 0; saldoUS = 0;
					sumaDebeCS = 0; sumaHaberCS = 0; sumaDebeUS = 0; sumaHaberUS = 0;
					lstAuxiliar.Groups.Add(grupo);
					ultimoIdCuentaDetalle = idCuentaDetalle;
					//Buscar los saldos iniciales
					int i;
					for (i = 0; i < arrSaldosIniUso; i++) {
						STCuentaSaldos saldo = arrSaldosIniciales[i];
						if (saldo.idCuenta == idCuenta) {
							saldoCS = (Int64)(saldo.debeCS - saldo.haberCS);
							saldoUS = (Int64)(saldo.debeUS - saldo.haberUS);
							sumaDebeCS = (Int64)saldo.debeCS;
							sumaHaberCS = (Int64)saldo.haberCS;
							sumaDebeUS = (Int64)saldo.debeUS;
							sumaHaberUS = (Int64)saldo.haberUS;
							break;
						}
					}
                    //
                    coloFondoFila = Color.WhiteSmoke;
                    ultimoIdComprobante = "";
					fechaComprobanteAntInicializada = false;
				}
				if (!fechaComprobanteAntInicializada) {
					fechaComprobanteAnt = fechaComprobanteDiario;
					fechaComprobanteAntInicializada = true;
				}
                if (!(ocultarDolaresCorobizados && esDolares)) {
                    saldoCS += montoDebeCS - montoHaberCS;
                }
				saldoUS += montoDebeUS - montoHaberUS;
                if (ultimoIdComprobante != idComprobanteDiario) {
                    ultimoIdComprobante = idComprobanteDiario;
                    coloFondoFila =  (coloFondoFila == Color.WhiteSmoke ? Color.White : Color.WhiteSmoke);
                }
				if(fechaComprobanteAnt.Year != fechaComprobanteDiario.Year){
					if(notasAutomatica.Length != 0) notasAutomatica += " / ";
					notasAutomatica += "cambio de año";
				} else if (fechaComprobanteAnt.Month <= 6 && fechaComprobanteDiario.Month > 6) {
					if (notasAutomatica.Length != 0) notasAutomatica += " / ";
					notasAutomatica += "cambio de semestre";
				}
				if (saldoCS >= -0.01 && saldoCS <= 0.01 && saldoUS >= -0.01 && saldoUS <= 0.01) {
					if (notasAutomatica.Length != 0) notasAutomatica += " / ";
					notasAutomatica += "balance cero";
				}
				ListViewItem itm = lstAuxiliar.Items.Add(idMovimiento.ToString(), idMovimiento.ToString(), "FlechaINOUT16(2).gif");
				itm.UseItemStyleForSubItems = false;
				itm.ForeColor = Color.Black;
                itm.BackColor = (idMovimientoResaltar == idMovimiento ? Color.Yellow : coloFondoFila);
                itm.Group = grupo;
				itm.Tag = idCuentaDetalle;
				ListViewItem.ListViewSubItem sitm;
				sitm = itm.SubItems.Add(idComprobanteDiario);
                if (montoDebeCS == 0 || (ocultarDolaresCorobizados && esDolares)) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", montoDebeCS / 100.0));
					sumaDebeCS += montoDebeCS;
				}
                if (montoHaberCS == 0 || (ocultarDolaresCorobizados && esDolares)) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", montoHaberCS / 100.0));
					sumaHaberCS += montoHaberCS;
				}
				sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", saldoCS / 100.0, (saldoCS > 0 ? Color.DarkBlue : saldoCS < 0 ? Color.DarkRed : Color.Gray), (saldoCS == 0 ? Color.LightGreen : Color.White), (saldoCS == 0 ? fuenteNegrilla : itm.Font)));
				sitm.ForeColor = (saldoCS > 0.009 ? Color.DarkBlue : saldoCS < -0.009 ? Color.DarkRed : Color.Gray);
				if (tasaCambio == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add(String.Format("{0:0,0.0000}", tasaCambio / 10000.0));
				}
				if (montoDebeUS == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", montoDebeUS / 100.0));
					sumaDebeUS += montoDebeUS;
				}
				if (montoHaberUS == 0) {
					sitm = itm.SubItems.Add("-");
				} else {
					sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", montoHaberUS / 100.0));
					sumaHaberUS += montoHaberUS;
				}
				sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", saldoUS / 100.0, (saldoUS > 0 ? Color.DarkBlue : saldoUS < 0 ? Color.DarkRed : Color.Gray), (saldoUS == 0 ? Color.LightGreen : Color.White), (saldoUS == 0 ? fuenteNegrilla : itm.Font)));
				sitm.ForeColor = (saldoUS > 0.009 ? Color.DarkBlue : saldoUS < -0.009 ? Color.DarkRed : Color.Gray);
				sitm = itm.SubItems.Add(referencia);
				sitm = itm.SubItems.Add(notasAutomatica);
				fechaComprobanteAnt = fechaComprobanteDiario;
			}
			if (grupo != null) {
				ListViewItem itmGrp = lstAuxiliar.Items.Add("-"); itmGrp.Font = new Font(itmGrp.Font, FontStyle.Bold);
				itmGrp.UseItemStyleForSubItems = false;
				itmGrp.ForeColor = Color.Black;
				itmGrp.Group = grupo;
				ListViewItem.ListViewSubItem sitmGrp;
				sitmGrp = itmGrp.SubItems.Add("TOTAL"); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("C$" + String.Format("{0:0,0.00}", saldoCS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);  sitmGrp.ForeColor = (saldoCS > 0 ? Color.DarkBlue : saldoCS < 0 ? Color.DarkRed : Color.Gray);
				sitmGrp = itmGrp.SubItems.Add("");
				sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);
				sitmGrp = itmGrp.SubItems.Add("U$" + String.Format("{0:0,0.00}", saldoUS / 100.0)); sitmGrp.Font = new Font(sitmGrp.Font, FontStyle.Bold);  sitmGrp.ForeColor = (saldoUS > 0 ? Color.DarkBlue : saldoUS < 0 ? Color.DarkRed : Color.Gray);
				sitmGrp = itmGrp.SubItems.Add("");
			}
			conC.Close();
			//
			lstAuxiliar.EndUpdate();
			lstAuxiliar.Refresh();
		}

		private void btnActualizarEstadoFin_Click(object sender, EventArgs e) {
			int nivel = 0;
			bool omitirCeros = chkFinOmitirCeros.Checked;
			bool cordobizado = chkEstadoFinCordibizado.Checked;
			double tasaCambio = 0; 
			try{
				nivel = int.Parse(cmbNivelEstadoFinanciero.Text);
			} catch(Exception){}
			if(cordobizado){
				try{
					tasaCambio = double.Parse(txtTasaEstadoFin.Text);
				} catch(Exception){}
			}
			if(nivel<=0){
				MessageBox.Show("Seleccione el nivel de detalle del Estado Financiero.");
			} else if(cordobizado && (tasaCambio<20 || tasaCambio>30)){
				MessageBox.Show("Especifique la tasa de cambio.");
			} else {
				//
				lstEstadoFin.BeginUpdate();
				lstEstadoFin.Items.Clear();
				lstEstadoFin.Groups.Clear();
				string strSQL = "";
				strSQL += " SELECT CuentasGrupos.ordenGrupo, CuentasGrupos.idCuentaGrupo, CuentasGrupos.nombreCuentaGrupo, ";
				strSQL += " Cuentas.idCuenta, Cuentas.nombreCuenta, ";
				strSQL += " CuentasSub.idCuentaSub, CuentasSub.nombreCuentaSub, ";
				strSQL += " CuentasSubSub.idCuentaSubSub, CuentasSubSub.nombreCuentaSubSub, ";
				strSQL += " CuentasSubSubSub.idCuentaSubSubSub, CuentasSubSubSub.nombreCuentaSubSubSub, ";
				strSQL += " CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle, ";
				strSQL += " CuentasDetalle.balanceDebe, CuentasDetalle.balanceHaber, CuentasDetalle.balanceDebeSoloCS, CuentasDetalle.balanceHaberSoloCS, CuentasDetalle.balanceDebeSoloUS, CuentasDetalle.balanceHaberSoloUS ";
				strSQL += " FROM ((((CuentasGrupos INNER JOIN Cuentas ON CuentasGrupos.idCuentaGrupo = Cuentas.idCuentaGrupo) ";
				strSQL += " INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
				strSQL += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
				strSQL += " INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
				strSQL += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub";
				strSQL += " ORDER BY CuentasGrupos.ordenGrupo, Cuentas.idCuenta, CuentasSub.idCuentaSub, CuentasSubSub.idCuentaSubSub, CuentasSubSubSub.idCuentaSubSubSub, CuentasDetalle.idCuentaDetalle";
				OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
				OdbcDataReader conC = ssql.ExecuteReader();
				ListViewItem itmGrupo = null, itmCuenta = null, itmCuentaSub = null, itmCuentaSubSub = null, itmCuentaSubSubSub = null;
				string ultimoIdGrupo = "", ultimoIdCuenta = "", ultimoIdCuentaSub="", ultimoIdCuentaSubSub="", ultimoIdCuentaSubSubSub="";
				string ultimoNombreGrupo = "", ultimoNombreCuenta = "", ultimoNombreCuentaSub = "", ultimoNombreCuentaSubSub = "", ultimoNombreCuentaSubSubSub = "";
				Int64 sumaDebeCS = 0, sumaHaberCS = 0, sumaDebeUS = 0, sumaHaberUS = 0;
                Int64 sumaDebeCSGrupo = 0, sumaHaberCSGrupo = 0, sumaDebeUSGrupo = 0, sumaHaberUSGrupo = 0;
                Int64 sumaDebeCSCuenta = 0, sumaHaberCSCuenta = 0, sumaDebeUSCuenta = 0, sumaHaberUSCuenta = 0;
                Int64 sumaDebeCSSub = 0, sumaHaberCSSub = 0, sumaDebeUSSub = 0, sumaHaberUSSub = 0;
                Int64 sumaDebeCSSubSub = 0, sumaHaberCSSubSub = 0, sumaDebeUSSubSub = 0, sumaHaberUSSubSub = 0;
                Int64 sumaDebeCSSubSubSub = 0, sumaHaberCSSubSubSub = 0, sumaDebeUSSubSubSub = 0, sumaHaberUSSubSubSub = 0;
				int conteoFilas = 0; ListViewItem itm; ListViewItem.ListViewSubItem sitm;
				while (conC.Read()) {
					conteoFilas++;
					int ordenGrupo = conC.GetByte(0);
					string idCuentaGrupo = conC.GetString(1);
					string nombreCuentaGrupo = conC.GetString(2);
					string idCuenta = conC.GetString(3);
					string nombreCuenta = conC.GetString(4);
					string idCuentaSub = conC.GetString(5);
					string nombreCuentaSub = conC.GetString(6);
					string idCuentaSubSub = conC.GetString(7);
					string nombreCuentaSubSub = conC.GetString(8);
					string idCuentaSubSubSub = conC.GetString(9);
					string nombreCuentaSubSubSub = conC.GetString(10);
					string idCuentaDetalle = conC.GetString(11);
					string nombreCuentaDetalle = conC.GetString(12);
                    Int64 balanceDebe = conC.GetInt64(13);
                    Int64 balanceHaber = conC.GetInt64(14);
                    Int64 balanceDebeSoloCS = conC.GetInt64(15);
                    Int64 balanceHaberSoloCS = conC.GetInt64(16);
                    Int64 balanceDebeSoloUS  = conC.GetInt64(17);
                    Int64 balanceHaberSoloUS = conC.GetInt64(18);
					//Primero procesar los cierres en orden inverso
					if (ultimoIdGrupo != idCuentaGrupo) {
						if (itmGrupo != null) {
							itm = itmGrupo;
							if (cordobizado) {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", (sumaDebeCSGrupo + ((sumaDebeUSGrupo * 100.0 * tasaCambio) / 100.0)) / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", (sumaHaberCSGrupo + ((sumaHaberUSGrupo * 100.0 * tasaCambio) / 100.0)) / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add(""); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add(""); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							} else {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							}
						}
						itm = lstEstadoFin.Items.Add("-");
						itm.SubItems.Add("-");
						itm.SubItems.Add("-");
						itm.SubItems.Add("-");
						itm.SubItems.Add("-");
						itm.SubItems.Add("-");
						itm = lstEstadoFin.Items.Add("-"); itm.Font = new Font(itm.Font, FontStyle.Bold);
						sitm = itm.SubItems.Add("" + nombreCuentaGrupo + "");
						itmGrupo = itm;
						sumaDebeCSGrupo = 0; sumaHaberCSGrupo = 0; sumaDebeUSGrupo = 0; sumaHaberUSGrupo = 0;
						ultimoIdGrupo = idCuentaGrupo;
						ultimoNombreGrupo = nombreCuentaGrupo;
					}
					if (ultimoIdCuenta != idCuenta) {
						if (itmCuenta != null) {
							if (nivel >= 1) {
								itm = itmCuenta;
								if (omitirCeros && sumaDebeCSCuenta == 0 && sumaHaberCSCuenta == 0 && sumaDebeUSCuenta == 0 && sumaHaberUSCuenta == 0) {
									itm.Remove();
								} else {
									if (cordobizado) {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSCuenta + (((sumaDebeUSCuenta * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSCuenta + (((sumaHaberUSCuenta * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									} else {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									}
								}
							}
						}
						if (nivel >= 1) {
							itm = lstEstadoFin.Items.Add(idCuenta); if (nivel > 1) itm.Font = new Font(itm.Font, FontStyle.Bold);
							sitm = itm.SubItems.Add("   " + nombreCuenta + ""); 
							itmCuenta = itm;
						}
						sumaDebeCSCuenta = 0; sumaHaberCSCuenta = 0; sumaDebeUSCuenta = 0; sumaHaberUSCuenta = 0;
						ultimoIdCuenta = idCuenta;
						ultimoNombreCuenta = nombreCuenta;
					}
					if (ultimoIdCuentaSub != idCuentaSub) {
						if (itmCuentaSub != null) {
							if (nivel >= 2) {
								itm = itmCuentaSub;
								if (omitirCeros && sumaDebeCSSub == 0 && sumaHaberCSSub == 0 && sumaDebeUSSub == 0 && sumaHaberUSSub == 0) {
									itm.Remove();
								} else {
									if (cordobizado) {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSub + (((sumaDebeUSSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSub + (((sumaHaberUSSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									} else {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									}
								}
							}
						}
						if (nivel >= 2) {
							itm = lstEstadoFin.Items.Add(idCuentaSub); if (nivel > 2) itm.Font = new Font(itm.Font, FontStyle.Bold);
							sitm = itm.SubItems.Add("      " + nombreCuentaSub + ""); 
							itmCuentaSub = itm;
						}
						sumaDebeCSSub = 0; sumaHaberCSSub = 0; sumaDebeUSSub = 0; sumaHaberUSSub = 0;
						ultimoIdCuentaSub = idCuentaSub;
						ultimoNombreCuentaSub = nombreCuentaSub;
					}
					if (ultimoIdCuentaSubSub != idCuentaSubSub) {
						if (itmCuentaSubSub != null) {
							if (nivel >= 3) {
								itm = itmCuentaSubSub;
								if (omitirCeros && sumaDebeCSSubSub == 0 && sumaHaberCSSubSub == 0 && sumaDebeUSSubSub == 0 && sumaHaberUSSubSub == 0) {
									itm.Remove();
								} else {
									if (cordobizado) {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSub + (((sumaDebeUSSubSub * 100 * tasaCambio) / 100.0) / 100.0))); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSub + (((sumaHaberUSSubSub * 100 * tasaCambio) / 100.0) / 100.0))); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									} else {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									}
								}
							}
						}
						if (nivel >= 3) {
							itm = lstEstadoFin.Items.Add(idCuentaSubSub); if (nivel > 3) itm.Font = new Font(itm.Font, FontStyle.Bold);
							sitm = itm.SubItems.Add("         " + nombreCuentaSubSub + ""); 
							itmCuentaSubSub = itm;
						}
						sumaDebeCSSubSub = 0; sumaHaberCSSubSub = 0; sumaDebeUSSubSub = 0; sumaHaberUSSubSub = 0;
						ultimoIdCuentaSubSub = idCuentaSubSub;
						ultimoNombreCuentaSubSub = nombreCuentaSubSub;
					}
					if (ultimoIdCuentaSubSubSub != idCuentaSubSubSub) {
						if (itmCuentaSubSubSub != null) {
							if (nivel >= 4) {
								itm =itmCuentaSubSubSub;
								if (omitirCeros && sumaDebeCSSubSubSub == 0 && sumaHaberCSSubSubSub == 0 && sumaDebeUSSubSubSub == 0 && sumaHaberUSSubSubSub == 0) {
									itm.Remove();
								} else {
									if (cordobizado) {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSubSub + (((sumaDebeUSSubSubSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSubSub + (((sumaHaberUSSubSubSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("-"); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									} else {
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
										sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
									}
								}
							}
						}
						if (nivel >= 4) {
							itm = lstEstadoFin.Items.Add(idCuentaSubSubSub); if (nivel > 4) itm.Font = new Font(itm.Font, FontStyle.Bold);
							sitm = itm.SubItems.Add("            " + nombreCuentaSubSubSub + "");
							itmCuentaSubSubSub = itm;
						}
						sumaDebeCSSubSubSub = 0; sumaHaberCSSubSubSub = 0; sumaDebeUSSubSubSub = 0; sumaHaberUSSubSubSub = 0;
						ultimoIdCuentaSubSubSub = idCuentaSubSubSub;
						ultimoNombreCuentaSubSubSub = nombreCuentaSubSubSub;
					}
					//
					if (nivel >= 5) {
						if (!(omitirCeros && balanceDebe == 0 && balanceHaber == 0 && balanceDebeSoloUS == 0 && balanceHaberSoloUS == 0)) {
							itm = lstEstadoFin.Items.Add(idCuentaDetalle);
							sitm = itm.SubItems.Add("               " + nombreCuentaDetalle + "");
							if (cordobizado) {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", balanceDebe + (((balanceDebeSoloUS * 100.0 * tasaCambio) / 100.0) / 100.0)));
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", balanceHaber + (((balanceHaberSoloUS * 100.0 * tasaCambio) / 100.0) / 100.0)));
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("-");
							} else {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", balanceDebe / 100.0));
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", balanceHaber / 100.0));
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", balanceDebeSoloUS / 100.0));
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", balanceHaberSoloUS / 100.0));
							}
						}
					}
					//
					sumaDebeCS += balanceDebe; sumaHaberCS += balanceHaber; sumaDebeUS += balanceDebeSoloUS; sumaHaberUS += balanceHaberSoloUS;
					sumaDebeCSGrupo += balanceDebe; sumaHaberCSGrupo += balanceHaber; sumaDebeUSGrupo += balanceDebeSoloUS; sumaHaberUSGrupo += balanceHaberSoloUS;
					sumaDebeCSCuenta += balanceDebe; sumaHaberCSCuenta += balanceHaber; sumaDebeUSCuenta += balanceDebeSoloUS; sumaHaberUSCuenta += balanceHaberSoloUS;
					sumaDebeCSSub += balanceDebe; sumaHaberCSSub += balanceHaber; sumaDebeUSSub += balanceDebeSoloUS; sumaHaberUSSub += balanceHaberSoloUS;
					sumaDebeCSSubSub += balanceDebe; sumaHaberCSSubSub += balanceHaber; sumaDebeUSSubSub += balanceDebeSoloUS; sumaHaberUSSubSub += balanceHaberSoloUS;
					sumaDebeCSSubSubSub += balanceDebe; sumaHaberCSSubSubSub += balanceHaber; sumaDebeUSSubSubSub += balanceDebeSoloUS; sumaHaberUSSubSubSub += balanceHaberSoloUS;
				}
				//Procesar los acumulados que quedaron
				if (itmGrupo != null) {
					itm = itmGrupo;
					if (cordobizado) {
						sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSGrupo + (((sumaDebeUSGrupo * 100.0 * tasaCambio) / 100.0) / 100.0))); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
						sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSGrupo + (((sumaHaberUSGrupo * 100.0 * tasaCambio) / 100.0) / 100.0))); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
						sitm = itm.SubItems.Add(""); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
						sitm = itm.SubItems.Add(""); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
					} else {
						sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
						sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
						sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
						sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSGrupo / 100.0)); sitm.Font = new Font(sitm.Font, FontStyle.Bold);
					}
				}
				if (itmCuenta != null) {
					if (nivel >= 1) {
						itm = itmCuenta;
						if (omitirCeros && sumaDebeCSCuenta == 0 && sumaHaberCSCuenta == 0 && sumaDebeUSCuenta == 0 && sumaHaberUSCuenta == 0) {
							itm.Remove();
						} else {
							if (cordobizado) {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSCuenta + (((sumaDebeUSCuenta * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSCuenta + (((sumaHaberUSCuenta * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							} else {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSCuenta / 100.0)); if (nivel > 1) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							}
						}
					}
				}
				if (itmCuentaSub != null) {
					if (nivel >= 2) {
						itm = itmCuentaSub;
						if (omitirCeros && sumaDebeCSSub == 0 && sumaHaberCSSub == 0 && sumaDebeUSSub == 0 && sumaHaberUSSub == 0) {
							itm.Remove();
						} else {
							if (cordobizado) {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSub + (((sumaDebeUSSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSub + (((sumaHaberUSSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							} else {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSSub / 100.0)); if (nivel > 2) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							}
						}
					}
				}
				if (itmCuentaSubSub != null) {
					if (nivel >= 3) {
						itm = itmCuentaSubSub;
						if (omitirCeros && sumaDebeCSSubSub == 0 && sumaHaberCSSubSub == 0 && sumaDebeUSSubSub == 0 && sumaHaberUSSubSub == 0) {
							itm.Remove();
						} else {
							if (cordobizado) {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSub + (((sumaDebeUSSubSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSub + (((sumaHaberUSSubSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							} else {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSSubSub / 100.0)); if (nivel > 3) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							}
						}
					}
				}
				if (itmCuentaSubSubSub != null) {
					if (nivel >= 4) {
						itm = itmCuentaSubSubSub;
						if (omitirCeros && sumaDebeCSSubSubSub == 0 && sumaHaberCSSubSubSub == 0 && sumaDebeUSSubSubSub == 0 && sumaHaberUSSubSubSub == 0) {
							itm.Remove();
						} else {
							if (cordobizado) {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSubSub + (((sumaDebeUSSubSubSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSubSub + (((sumaHaberUSSubSubSub * 100.0 * tasaCambio) / 100.0) / 100.0))); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("-"); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							} else {
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaDebeCSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("C$" + String.Format("{0:0,0.00}", sumaHaberCSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaDebeUSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("U$" + String.Format("{0:0,0.00}", sumaHaberUSSubSubSub / 100.0)); if (nivel > 4) sitm.Font = new Font(sitm.Font, FontStyle.Bold);
							}
						}
					}
				}
				conC.Close();
				//
				lstEstadoFin.EndUpdate();
				lstEstadoFin.Refresh();
				//
				if (cordobizado) {
					lblFinDebeCS.Text = "Debe: C$" + String.Format("{0:0,0.00}", sumaDebeCS + (((sumaDebeUS * 100.0 * tasaCambio) / 100.0) / 100.0));
					lblFinDebeUS.Text = "Debe: U$-";
					lblFinHaberCS.Text = "Haber: C$" + String.Format("{0:0,0.00}", sumaHaberCS + (((sumaHaberUS * 100.0 * tasaCambio) / 100.0) / 100.0));
					lblFinHaberUS.Text = "Haber: U$-";
				} else {
					lblFinDebeCS.Text = "Debe: C$" + String.Format("{0:0,0.00}", sumaDebeCS / 100.0);
					lblFinDebeUS.Text = "Debe: U$" + String.Format("{0:0,0.00}", sumaDebeUS / 100.0);
					lblFinHaberCS.Text = "Haber: C$" + String.Format("{0:0,0.00}", sumaHaberCS / 100.0);
					lblFinHaberUS.Text = "Haber: U$" + String.Format("{0:0,0.00}", sumaHaberUS / 100.0);
				}
			}
		}

		private void chkEstadoFinCordibizado_CheckedChanged(object sender, EventArgs e) {
			txtTasaEstadoFin.Enabled = chkEstadoFinCordibizado.Checked;
		}

		private void btnRecalcularBalance_Click(object sender, EventArgs e) {
			//ToDo: remove o r re-enable
			/*Global.actualizarBalanceDeCuentas();
			//actualizar el reporte
			btnActualizarEstadoFin_Click(null, null);*/
			MessageBox.Show("No implementado.");
		}

		private void dateTimePicker1_ValueChanged(object sender, EventArgs e) {

		}

		
		private void consultaSumaMovimientos(bool cordobas, ref double guardaDebeEn, ref double guardaHaberEn, string sqlWHERE){
			string strSQL2 = "";
			//if (cordobas){
				strSQL2 += " SELECT ROUND(SUM(Movimientos.montoDebeCS), 2), ROUND(SUM(Movimientos.montoHaberCS), 2)  ";
			//} else {
			//	strSQL2 += " SELECT SUM(Movimientos.montoDebeUS), SUM(Movimientos.montoHaberUS)  ";
			//}
			strSQL2 += " FROM ((((((CuentasGrupos INNER JOIN Cuentas ON CuentasGrupos.idCuentaGrupo = Cuentas.idCuentaGrupo) ";
			strSQL2 += " INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
			strSQL2 += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
			strSQL2 += " INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
			strSQL2 += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub)";
			strSQL2 += " INNER JOIN Movimientos ON CuentasDetalle.idCuentaDetalle = Movimientos.idCuentaDetalle) ";
			strSQL2 += " INNER JOIN ComprobantesDeDiario ON Movimientos.idComprobanteDiario = ComprobantesDeDiario.idComprobanteDiario";
			//strSQL2 += " WHERE Movimientos.esDolares = " + (cordobas?"0":"-1") + " ";
			strSQL2 += sqlWHERE;
			//strSQL2 += " WHERE CuentasGrupos.idCuentaGrupo='" + idCuentaGrupo + "'";
			//strSQL2 += " AND ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
			OdbcCommand ssql2 = new OdbcCommand(strSQL2, Global.conn);
			OdbcDataReader conC2 = ssql2.ExecuteReader();
			guardaDebeEn = 0; guardaHaberEn = 0;
			if (conC2.Read()){
				if (!conC2.IsDBNull(0)) guardaDebeEn = conC2.GetInt64(0);
				if (!conC2.IsDBNull(1)) guardaHaberEn = conC2.GetInt64(1);
			}
			conC2.Close();
		}

		private void btnOKBalanza_Click(object sender, EventArgs e){
			//ToDo: remove or re-enable
			/*
			int nivel = 0;
			bool omitirSubtotales = (chkSubtotales.CheckState == CheckState.Unchecked);
			bool omitirCeros = true; // chkFinOmitirCeros.Checked;
			bool cordobizado = false; // chkEstadoFinCordibizado.Checked;
			double tasaCambio = 0;
			DateTime fechaIni = fechaIniBalanza.Value;
			DateTime fechaFin = fechaFinBalanza.Value;
			try{
				nivel = int.Parse(cmbNivelBalanza.Text);
			} catch(Exception){}
			/ *if(cordobizado){
				try{
					tasaCambio = double.Parse(txtTasaEstadoFin.Text);
				} catch(Exception){}
			}* /
			if(nivel<=0){
				MessageBox.Show("Seleccione el nivel de detalle de la Balanza.");
			} else if(cordobizado && (tasaCambio<20 || tasaCambio>30)){
				MessageBox.Show("Especifique la tasa de cambio.");
			} else {
				//
				lstBalanza.BeginUpdate();
				lstBalanza.Items.Clear();
				lstBalanza.Groups.Clear();
				string strSQL = "";
				strSQL += " SELECT CuentasGrupos.ordenGrupo, CuentasGrupos.idCuentaGrupo, CuentasGrupos.nombreCuentaGrupo, ";
				strSQL += " Cuentas.idCuenta, Cuentas.nombreCuenta, ";
				strSQL += " CuentasSub.idCuentaSub, CuentasSub.nombreCuentaSub, ";
				strSQL += " CuentasSubSub.idCuentaSubSub, CuentasSubSub.nombreCuentaSubSub, ";
				strSQL += " CuentasSubSubSub.idCuentaSubSubSub, CuentasSubSubSub.nombreCuentaSubSubSub, ";
				strSQL += " CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle ";
				strSQL += " FROM ((((CuentasGrupos INNER JOIN Cuentas ON CuentasGrupos.idCuentaGrupo = Cuentas.idCuentaGrupo) ";
				strSQL += " INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
				strSQL += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
				strSQL += " INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
				strSQL += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub";
				strSQL += " ORDER BY CuentasGrupos.ordenGrupo, Cuentas.idCuenta, CuentasSub.idCuentaSub, CuentasSubSub.idCuentaSubSub, CuentasSubSubSub.idCuentaSubSubSub, CuentasDetalle.idCuentaDetalle";
				OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
				OdbcDataReader conC = ssql.ExecuteReader();
				//ListViewItem itmGrupo = null, itmCuenta = null, itmCuentaSub = null, itmCuentaSubSub = null, itmCuentaSubSubSub = null;
				string ultimoIdGrupo = "", ultimoIdCuenta = "", ultimoIdCuentaSub="", ultimoIdCuentaSubSub="", ultimoIdCuentaSubSubSub="";
				string ultimoNombreGrupo = "", ultimoNombreCuenta = "", ultimoNombreCuentaSub = "", ultimoNombreCuentaSubSub = "", ultimoNombreCuentaSubSubSub = "";
				/ *double sumaDebeCS = 0, sumaHaberCS = 0, sumaDebeUS = 0, sumaHaberUS = 0;
				double sumaDebeCSGrupo = 0, sumaHaberCSGrupo = 0, sumaDebeUSGrupo = 0, sumaHaberUSGrupo = 0;
				double sumaDebeCSCuenta = 0, sumaHaberCSCuenta = 0, sumaDebeUSCuenta = 0, sumaHaberUSCuenta = 0;
				double sumaDebeCSSub = 0, sumaHaberCSSub = 0, sumaDebeUSSub = 0, sumaHaberUSSub = 0;
				double sumaDebeCSSubSub = 0, sumaHaberCSSubSub = 0, sumaDebeUSSubSub = 0, sumaHaberUSSubSub = 0;
				double sumaDebeCSSubSubSub = 0, sumaHaberCSSubSubSub = 0, sumaDebeUSSubSubSub = 0, sumaHaberUSSubSubSub = 0;* /
				int conteoFilas = 0; ListViewItem itm; ListViewItem.ListViewSubItem sitm;
				while (conC.Read()) {
					conteoFilas++;
					int ordenGrupo = conC.GetByte(0);
					string idCuentaGrupo = conC.GetString(1);
					string nombreCuentaGrupo = conC.GetString(2);
					string idCuenta = conC.GetString(3);
					string nombreCuenta = conC.GetString(4);
					string idCuentaSub = conC.GetString(5);
					string nombreCuentaSub = conC.GetString(6);
					string idCuentaSubSub = conC.GetString(7);
					string nombreCuentaSubSub = conC.GetString(8);
					string idCuentaSubSubSub = conC.GetString(9);
					string nombreCuentaSubSubSub = conC.GetString(10);
					string idCuentaDetalle = conC.GetString(11);
					string nombreCuentaDetalle = conC.GetString(12);
					//Presentar
					string strWHERE = null;
					double debeCSPrev = 0, haberCSPrev = 0;//, debeUSPrev = 0, haberUSPrev = 0;
					double debeCSMov = 0, haberCSMov = 0;//, debeUSMov = 0, haberUSMov = 0;
					double debeCSFin = 0, haberCSFin = 0;//, debeUSFin = 0, haberUSFin = 0;
					if (ultimoIdGrupo != idCuentaGrupo) {
						//cierre anterior
						strWHERE = "";
						strWHERE += " WHERE CuentasGrupos.idCuentaGrupo='" + idCuentaGrupo + "'";
						strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
						consultaSumaMovimientos(true, ref debeCSPrev, ref haberCSPrev, strWHERE);
						//consultaSumaMovimientos(false, ref debeUSPrev, ref haberUSPrev, strWHERE);
						debeCSPrev -= haberCSPrev; haberCSPrev = 0; if (debeCSPrev < 0) { haberCSPrev = -debeCSPrev; debeCSPrev = 0; }
						//debeUSPrev -= haberUSPrev; haberUSPrev = 0; if (debeUSPrev < 0) { haberUSPrev = -debeUSPrev; debeUSPrev = 0; }
						//movimientos del periodo
						strWHERE = "";
						strWHERE += " WHERE CuentasGrupos.idCuentaGrupo='" + idCuentaGrupo + "'";
						strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
						consultaSumaMovimientos(true, ref debeCSMov, ref haberCSMov, strWHERE);
						//consultaSumaMovimientos(false, ref debeUSMov, ref haberUSMov, strWHERE);
						//cierre periodo
						debeCSFin = debeCSPrev + debeCSMov; haberCSFin = haberCSPrev + haberCSMov; //debeUSFin = debeUSPrev + debeUSMov; haberUSFin = haberUSPrev + haberUSMov;
						debeCSFin -= haberCSFin; haberCSFin = 0; if (debeCSFin < 0) { haberCSFin = -debeCSFin; debeCSFin = 0; }
						//debeUSFin -= haberUSFin; haberUSFin = 0; if (debeUSFin < 0) { haberUSFin = -debeUSFin; debeUSFin = 0; }
						//
						if (!(omitirCeros
							&& debeCSPrev >= -0.01 && debeCSPrev <= 0.01 && haberCSPrev >= -0.01 && haberCSPrev <= 0.01 / *&& debeUSPrev >= -0.01 && debeUSPrev <= 0.01 && haberUSPrev >= -0.01 && haberUSPrev <= 0.01* /
							&& debeCSMov >= -0.01 && debeCSMov <= 0.01 && haberCSMov >= -0.01 && haberCSMov <= 0.01 / *&& debeUSMov >= -0.01 && debeUSMov <= 0.01 && haberUSMov >= -0.01 && haberUSMov <= 0.01* /
							&& debeCSFin >= -0.01 && debeCSFin <= 0.01 && haberCSFin >= -0.01 && haberCSFin <= 0.01 / *&& debeUSFin >= -0.01 && debeUSFin <= 0.01 && haberUSFin >= -0.01 && haberUSFin <= 0.01* / ))
						{
							int miNivel = 0;
							itm = lstBalanza.Items.Add("-");
							itm.SubItems.Add("-");
							itm.SubItems.Add("-"); itm.SubItems.Add("-"); itm.SubItems.Add("-"); itm.SubItems.Add("-");
							itm.SubItems.Add("-");
							itm.SubItems.Add("-"); itm.SubItems.Add("-"); itm.SubItems.Add("-"); itm.SubItems.Add("-");
							itm.SubItems.Add("-");
							itm.SubItems.Add("-"); itm.SubItems.Add("-"); itm.SubItems.Add("-"); itm.SubItems.Add("-");
							itm = lstBalanza.Items.Add("-"); itm.Font = new Font(itm.Font, FontStyle.Bold);
							sitm = itm.SubItems.Add("" + nombreCuentaGrupo + "");
							sitm = itm.SubItems.Add("" + (debeCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSPrev) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSPrev) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSPrev != 0 ? String.Format("{0:0,0.00}", debeUSPrev) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSPrev != 0 ? String.Format("{0:0,0.00}", haberUSPrev) : "-") + "");
							sitm = itm.SubItems.Add("-");
							sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSMov) : "") + ""); sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSMov) : "") + ""); //sitm = itm.SubItems.Add("" + debeUSMov + ""); sitm = itm.SubItems.Add("" + haberUSMov + "");
							sitm = itm.SubItems.Add("-");
							sitm = itm.SubItems.Add("" + (debeCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSFin) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSFin) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSFin != 0 ? String.Format("{0:0,0.00}", debeUSFin) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSFin != 0 ? String.Format("{0:0,0.00}", haberUSFin) : "-") + "");
						}
						ultimoIdGrupo = idCuentaGrupo;
						ultimoNombreGrupo = nombreCuentaGrupo;
					}
					if (ultimoIdCuenta != idCuenta) {
						if (nivel >= 1) {
							int miNivel = 1;
							//cierre anterior
							strWHERE = "";
							strWHERE += " WHERE Cuentas.idCuenta='" + idCuenta + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
							consultaSumaMovimientos(true, ref debeCSPrev, ref haberCSPrev, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSPrev, ref haberUSPrev, strWHERE);
							debeCSPrev -= haberCSPrev; haberCSPrev = 0; if (debeCSPrev < 0) { haberCSPrev = -debeCSPrev; debeCSPrev = 0; }
							//debeUSPrev -= haberUSPrev; haberUSPrev = 0; if (debeUSPrev < 0) { haberUSPrev = -debeUSPrev; debeUSPrev = 0; }
							//movimientos del periodo
							strWHERE = "";
							strWHERE += " WHERE Cuentas.idCuenta='" + idCuenta + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
							consultaSumaMovimientos(true, ref debeCSMov, ref haberCSMov, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSMov, ref haberUSMov, strWHERE);
							//cierre periodo
							debeCSFin = debeCSPrev + debeCSMov; haberCSFin = haberCSPrev + haberCSMov; //debeUSFin = debeUSPrev + debeUSMov; haberUSFin = haberUSPrev + haberUSMov;
							debeCSFin -= haberCSFin; haberCSFin = 0; if (debeCSFin < 0) { haberCSFin = -debeCSFin; debeCSFin = 0; }
							//debeUSFin -= haberUSFin; haberUSFin = 0; if (debeUSFin < 0) { haberUSFin = -debeUSFin; debeUSFin = 0; }
							//
                            if (!(omitirCeros
                            && debeCSPrev >= -0.01 && debeCSPrev <= 0.01 && haberCSPrev >= -0.01 && haberCSPrev <= 0.01 / *&& debeUSPrev >= -0.01 && debeUSPrev <= 0.01 && haberUSPrev >= -0.01 && haberUSPrev <= 0.01* /
                            && debeCSMov >= -0.01 && debeCSMov <= 0.01 && haberCSMov >= -0.01 && haberCSMov <= 0.01 / *&& debeUSMov >= -0.01 && debeUSMov <= 0.01 && haberUSMov >= -0.01 && haberUSMov <= 0.01* /
                            && debeCSFin >= -0.01 && debeCSFin <= 0.01 && haberCSFin >= -0.01 && haberCSFin <= 0.01 / *&& debeUSFin >= -0.01 && debeUSFin <= 0.01 && haberUSFin >= -0.01 && haberUSFin <= 0.01* / ))
                            {
								itm = lstBalanza.Items.Add(idCuenta); if (nivel > 1) itm.Font = new Font(itm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("   " + nombreCuenta + "");
								sitm = itm.SubItems.Add("" + (debeCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSPrev) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSPrev) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSPrev != 0 ? String.Format("{0:0,0.00}", debeUSPrev) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSPrev != 0 ? String.Format("{0:0,0.00}", haberUSPrev) : "-") + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSMov) : "") + ""); sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSMov) : "") + ""); //sitm = itm.SubItems.Add("" + debeUSMov + ""); sitm = itm.SubItems.Add("" + haberUSMov + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + (debeCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSFin) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSFin) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSFin != 0 ? String.Format("{0:0,0.00}", debeUSFin) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSFin != 0 ? String.Format("{0:0,0.00}", haberUSFin) : "-") + "");
							}
						}
						ultimoIdCuenta = idCuenta;
						ultimoNombreCuenta = nombreCuenta;
					}
					if (ultimoIdCuentaSub != idCuentaSub) {
						if (nivel >= 2) {
							int miNivel = 2;
							//cierre anterior
							strWHERE = "";
							strWHERE += " WHERE CuentasSub.idCuentaSub='" + idCuentaSub + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
							consultaSumaMovimientos(true, ref debeCSPrev, ref haberCSPrev, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSPrev, ref haberUSPrev, strWHERE);
							debeCSPrev -= haberCSPrev; haberCSPrev = 0; if (debeCSPrev < 0) { haberCSPrev = -debeCSPrev; debeCSPrev = 0; }
							//debeUSPrev -= haberUSPrev; haberUSPrev = 0; if (debeUSPrev < 0) { haberUSPrev = -debeUSPrev; debeUSPrev = 0; }
							//movimientos del periodo
							strWHERE = "";
							strWHERE += " WHERE CuentasSub.idCuentaSub='" + idCuentaSub + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
							consultaSumaMovimientos(true, ref debeCSMov, ref haberCSMov, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSMov, ref haberUSMov, strWHERE);
							//cierre periodo
							debeCSFin = debeCSPrev + debeCSMov; haberCSFin = haberCSPrev + haberCSMov; //debeUSFin = debeUSPrev + debeUSMov; haberUSFin = haberUSPrev + haberUSMov;
							debeCSFin -= haberCSFin; haberCSFin = 0; if (debeCSFin < 0) { haberCSFin = -debeCSFin; debeCSFin = 0; }
							//debeUSFin -= haberUSFin; haberUSFin = 0; if (debeUSFin < 0) { haberUSFin = -debeUSFin; debeUSFin = 0; }
							//
                            if (!(omitirCeros
                            && debeCSPrev >= -0.01 && debeCSPrev <= 0.01 && haberCSPrev >= -0.01 && haberCSPrev <= 0.01 / *&& debeUSPrev >= -0.01 && debeUSPrev <= 0.01 && haberUSPrev >= -0.01 && haberUSPrev <= 0.01* /
                            && debeCSMov >= -0.01 && debeCSMov <= 0.01 && haberCSMov >= -0.01 && haberCSMov <= 0.01 / *&& debeUSMov >= -0.01 && debeUSMov <= 0.01 && haberUSMov >= -0.01 && haberUSMov <= 0.01* /
                            && debeCSFin >= -0.01 && debeCSFin <= 0.01 && haberCSFin >= -0.01 && haberCSFin <= 0.01 / *&& debeUSFin >= -0.01 && debeUSFin <= 0.01 && haberUSFin >= -0.01 && haberUSFin <= 0.01* / ))
                            {
								itm = lstBalanza.Items.Add(idCuentaSub); if (nivel > 2) itm.Font = new Font(itm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("      " + nombreCuentaSub + "");
								sitm = itm.SubItems.Add("" + (debeCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSPrev) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSPrev) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSPrev != 0 ? String.Format("{0:0,0.00}", debeUSPrev) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSPrev != 0 ? String.Format("{0:0,0.00}", haberUSPrev) : "-") + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSMov) : "") + ""); sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSMov) : "") + ""); //sitm = itm.SubItems.Add("" + debeUSMov + ""); sitm = itm.SubItems.Add("" + haberUSMov + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + (debeCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSFin) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSFin) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSFin != 0 ? String.Format("{0:0,0.00}", debeUSFin) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSFin != 0 ? String.Format("{0:0,0.00}", haberUSFin) : "-") + "");
							}
						}
						ultimoIdCuentaSub = idCuentaSub;
						ultimoNombreCuentaSub = nombreCuentaSub;
					}
					if (ultimoIdCuentaSubSub != idCuentaSubSub) {
						if (nivel >= 3) {
							int miNivel = 3;
							//cierre anterior
							strWHERE = "";
							strWHERE += " WHERE CuentasSubSub.idCuentaSubSub='" + idCuentaSubSub + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
							consultaSumaMovimientos(true, ref debeCSPrev, ref haberCSPrev, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSPrev, ref haberUSPrev, strWHERE);
							debeCSPrev -= haberCSPrev; haberCSPrev = 0; if (debeCSPrev < 0) { haberCSPrev = -debeCSPrev; debeCSPrev = 0; }
							//debeUSPrev -= haberUSPrev; haberUSPrev = 0; if (debeUSPrev < 0) { haberUSPrev = -debeUSPrev; debeUSPrev = 0; }
							//movimientos del periodo
							strWHERE = "";
							strWHERE += " WHERE CuentasSubSub.idCuentaSubSub='" + idCuentaSubSub + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
							consultaSumaMovimientos(true, ref debeCSMov, ref haberCSMov, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSMov, ref haberUSMov, strWHERE);
							//cierre periodo
							debeCSFin = debeCSPrev + debeCSMov; haberCSFin = haberCSPrev + haberCSMov; //debeUSFin = debeUSPrev + debeUSMov; haberUSFin = haberUSPrev + haberUSMov;
							debeCSFin -= haberCSFin; haberCSFin = 0; if (debeCSFin < 0) { haberCSFin = -debeCSFin; debeCSFin = 0; }
							//debeUSFin -= haberUSFin; haberUSFin = 0; if (debeUSFin < 0) { haberUSFin = -debeUSFin; debeUSFin = 0; }
							//
                            if (!(omitirCeros
                            && debeCSPrev >= -0.01 && debeCSPrev <= 0.01 && haberCSPrev >= -0.01 && haberCSPrev <= 0.01 / *&& debeUSPrev >= -0.01 && debeUSPrev <= 0.01 && haberUSPrev >= -0.01 && haberUSPrev <= 0.01* /
                            && debeCSMov >= -0.01 && debeCSMov <= 0.01 && haberCSMov >= -0.01 && haberCSMov <= 0.01 / *&& debeUSMov >= -0.01 && debeUSMov <= 0.01 && haberUSMov >= -0.01 && haberUSMov <= 0.01* /
                            && debeCSFin >= -0.01 && debeCSFin <= 0.01 && haberCSFin >= -0.01 && haberCSFin <= 0.01 / *&& debeUSFin >= -0.01 && debeUSFin <= 0.01 && haberUSFin >= -0.01 && haberUSFin <= 0.01* / ))
                            {
								itm = lstBalanza.Items.Add(idCuentaSubSub); if (nivel > 3) itm.Font = new Font(itm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("         " + nombreCuentaSubSub + "");
								sitm = itm.SubItems.Add("" + (debeCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSPrev) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSPrev) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSPrev != 0 ? String.Format("{0:0,0.00}", debeUSPrev) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSPrev != 0 ? String.Format("{0:0,0.00}", haberUSPrev) : "-") + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSMov) : "") + ""); sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSMov) : "") + ""); //sitm = itm.SubItems.Add("" + debeUSMov + ""); sitm = itm.SubItems.Add("" + haberUSMov + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + (debeCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSFin) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSFin) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSFin != 0 ? String.Format("{0:0,0.00}", debeUSFin) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSFin != 0 ? String.Format("{0:0,0.00}", haberUSFin) : "-") + "");

							}
						}
						ultimoIdCuentaSubSub = idCuentaSubSub;
						ultimoNombreCuentaSubSub = nombreCuentaSubSub;
					}
					if (ultimoIdCuentaSubSubSub != idCuentaSubSubSub) {
						if (nivel >= 4) {
							int miNivel = 4;
							//cierre anterior
							strWHERE = "";
							strWHERE += " WHERE CuentasSubSubSub.idCuentaSubSubSub='" + idCuentaSubSubSub + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
							consultaSumaMovimientos(true, ref debeCSPrev, ref haberCSPrev, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSPrev, ref haberUSPrev, strWHERE);
							debeCSPrev -= haberCSPrev; haberCSPrev = 0; if (debeCSPrev < 0) { haberCSPrev = -debeCSPrev; debeCSPrev = 0; }
							//debeUSPrev -= haberUSPrev; haberUSPrev = 0; if (debeUSPrev < 0) { haberUSPrev = -debeUSPrev; debeUSPrev = 0; }
							//movimientos del periodo
							strWHERE = "";
							strWHERE += " WHERE CuentasSubSubSub.idCuentaSubSubSub='" + idCuentaSubSubSub + "'";
							strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
							consultaSumaMovimientos(true, ref debeCSMov, ref haberCSMov, strWHERE);
							//consultaSumaMovimientos(false, ref debeUSMov, ref haberUSMov, strWHERE);
							//cierre periodo
							debeCSFin = debeCSPrev + debeCSMov; haberCSFin = haberCSPrev + haberCSMov; //debeUSFin = debeUSPrev + debeUSMov; haberUSFin = haberUSPrev + haberUSMov;
							debeCSFin -= haberCSFin; haberCSFin = 0; if (debeCSFin < 0) { haberCSFin = -debeCSFin; debeCSFin = 0; }
							//debeUSFin -= haberUSFin; haberUSFin = 0; if (debeUSFin < 0) { haberUSFin = -debeUSFin; debeUSFin = 0; }
							//
                            if (!(omitirCeros
                            && debeCSPrev >= -0.01 && debeCSPrev <= 0.01 && haberCSPrev >= -0.01 && haberCSPrev <= 0.01 / *&& debeUSPrev >= -0.01 && debeUSPrev <= 0.01 && haberUSPrev >= -0.01 && haberUSPrev <= 0.01* /
                            && debeCSMov >= -0.01 && debeCSMov <= 0.01 && haberCSMov >= -0.01 && haberCSMov <= 0.01 / *&& debeUSMov >= -0.01 && debeUSMov <= 0.01 && haberUSMov >= -0.01 && haberUSMov <= 0.01* /
                            && debeCSFin >= -0.01 && debeCSFin <= 0.01 && haberCSFin >= -0.01 && haberCSFin <= 0.01 / *&& debeUSFin >= -0.01 && debeUSFin <= 0.01 && haberUSFin >= -0.01 && haberUSFin <= 0.01* / ))
                            {
								itm = lstBalanza.Items.Add(idCuentaSubSubSub); if (nivel > 4) itm.Font = new Font(itm.Font, FontStyle.Bold);
								sitm = itm.SubItems.Add("            " + nombreCuentaSubSubSub + "");
								sitm = itm.SubItems.Add("" + (debeCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSPrev) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSPrev) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSPrev != 0 ? String.Format("{0:0,0.00}", debeUSPrev) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSPrev != 0 ? String.Format("{0:0,0.00}", haberUSPrev) : "-") + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSMov) : "") + ""); sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSMov) : "") + "");// sitm = itm.SubItems.Add("" + debeUSMov + ""); sitm = itm.SubItems.Add("" + haberUSMov + "");
								sitm = itm.SubItems.Add("-");
								sitm = itm.SubItems.Add("" + (debeCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSFin) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSFin) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSFin != 0 ? String.Format("{0:0,0.00}", debeUSFin) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSFin != 0 ? String.Format("{0:0,0.00}", haberUSFin) : "-") + "");
							}
						}
						ultimoIdCuentaSubSubSub = idCuentaSubSubSub;
						ultimoNombreCuentaSubSubSub = nombreCuentaSubSubSub;
					}
					//
					if (nivel >= 5) {
						int miNivel = 5;
						//cierre anterior
						strWHERE = "";
						strWHERE += " WHERE CuentasDetalle.idCuentaDetalle='" + idCuentaDetalle + "'";
						strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario < '" + Global.fechaSQL(fechaIni) + "'";
						consultaSumaMovimientos(true, ref debeCSPrev, ref haberCSPrev, strWHERE);
						//consultaSumaMovimientos(false, ref debeUSPrev, ref haberUSPrev, strWHERE);
						debeCSPrev -= haberCSPrev; haberCSPrev = 0; if (debeCSPrev < 0) { haberCSPrev = -debeCSPrev; debeCSPrev = 0; }
						//debeUSPrev -= haberUSPrev; haberUSPrev = 0; if (debeUSPrev < 0) { haberUSPrev = -debeUSPrev; debeUSPrev = 0; }
						//movimientos del periodo
						strWHERE = "";
						strWHERE += " WHERE CuentasDetalle.idCuentaDetalle='" + idCuentaDetalle + "'";
						strWHERE += " AND ComprobantesDeDiario.fechaComprobanteDiario >= '" + Global.fechaSQL(fechaIni) + "' AND ComprobantesDeDiario.fechaComprobanteDiario <= '" + Global.fechaSQL(fechaFin) + "'";
						consultaSumaMovimientos(true, ref debeCSMov, ref haberCSMov, strWHERE);
						//consultaSumaMovimientos(false, ref debeUSMov, ref haberUSMov, strWHERE);
						//cierre periodo
						debeCSFin = debeCSPrev + debeCSMov; haberCSFin = haberCSPrev + haberCSMov; //debeUSFin = debeUSPrev + debeUSMov; haberUSFin = haberUSPrev + haberUSMov;
						debeCSFin -= haberCSFin; haberCSFin = 0; if (debeCSFin < 0) { haberCSFin = -debeCSFin; debeCSFin = 0; }
						//debeUSFin -= haberUSFin; haberUSFin = 0; if (debeUSFin < 0) { haberUSFin = -debeUSFin; debeUSFin = 0; }
						//
                        if (!(omitirCeros
                            && debeCSPrev >= -0.01 && debeCSPrev <= 0.01 && haberCSPrev >= -0.01 && haberCSPrev <= 0.01 / *&& debeUSPrev >= -0.01 && debeUSPrev <= 0.01 && haberUSPrev >= -0.01 && haberUSPrev <= 0.01* /
                            && debeCSMov >= -0.01 && debeCSMov <= 0.01 && haberCSMov >= -0.01 && haberCSMov <= 0.01 / *&& debeUSMov >= -0.01 && debeUSMov <= 0.01 && haberUSMov >= -0.01 && haberUSMov <= 0.01* /
                            && debeCSFin >= -0.01 && debeCSFin <= 0.01 && haberCSFin >= -0.01 && haberCSFin <= 0.01 / *&& debeUSFin >= -0.01 && debeUSFin <= 0.01 && haberUSFin >= -0.01 && haberUSFin <= 0.01* / ))
                        {
							itm = lstBalanza.Items.Add(idCuentaDetalle);
							sitm = itm.SubItems.Add("               " + nombreCuentaDetalle + "");
							sitm = itm.SubItems.Add("" + (debeCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSPrev) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSPrev != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSPrev) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSPrev != 0 ? String.Format("{0:0,0.00}", debeUSPrev) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSPrev != 0 ? String.Format("{0:0,0.00}", haberUSPrev) : "-") + "");
							sitm = itm.SubItems.Add("-");
							sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSMov) : "") + ""); sitm = itm.SubItems.Add("" + ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSMov) : "") + ""); //sitm = itm.SubItems.Add("" + debeUSMov + ""); sitm = itm.SubItems.Add("" + haberUSMov + "");
							sitm = itm.SubItems.Add("-");
							sitm = itm.SubItems.Add("" + (debeCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", debeCSFin) : "") : "-") + ""); sitm = itm.SubItems.Add("" + (haberCSFin != 0 ? ((nivel == miNivel || !omitirSubtotales) ? String.Format("{0:0,0.00}", haberCSFin) : "") : "-") + ""); //sitm = itm.SubItems.Add("" + (debeUSFin != 0 ? String.Format("{0:0,0.00}", debeUSFin) : "-") + ""); sitm = itm.SubItems.Add("" + (haberUSFin != 0 ? String.Format("{0:0,0.00}", haberUSFin) : "-") + "");
						}
					}
				}
				conC.Close();
				//
				lstBalanza.EndUpdate();
				lstBalanza.Refresh();
			}
			*/
		}

		private void btnExportarBalanza_Click(object sender, EventArgs e){
			Global.exportarListaXLS(lstBalanza);
		}

		private void btnExportarComprobantes_Click(object sender, EventArgs e) {
			Global.exportarListaXLS(lstComprobantes);
		}

		private void btnExportarAuxiliar_Click(object sender, EventArgs e) {
			Global.exportarListaXLS(lstAuxiliar);
		}

		private void btnExportarFinanciero_Click(object sender, EventArgs e) {
			Global.exportarListaXLS(lstEstadoFin);
		}

		private void dameDetallesCuentaDetalle(string idCuentaDetalle, ref string guardaNombreCuentaEn, ref string guardaNombreCuentaSubEn, ref string guardaNombreCuentaSubSubEn, ref string guardaNombreCuentaSubSubSubEn, ref string guardaNombreCuentaDetalleEn) {
			guardaNombreCuentaEn = "";
			guardaNombreCuentaSubEn = "";
			guardaNombreCuentaSubSubEn = "";
			guardaNombreCuentaSubSubSubEn = "";
			guardaNombreCuentaDetalleEn = "";
			//
			string strSQL = "";
			strSQL += " SELECT Cuentas.idCuenta, Cuentas.nombreCuenta, ";
			strSQL += " CuentasSub.idCuentaSub, CuentasSub.nombreCuentaSub, ";
			strSQL += " CuentasSubSub.idCuentaSubSub, CuentasSubSub.nombreCuentaSubSub, ";
			strSQL += " CuentasSubSubSub.idCuentaSubSubSub, CuentasSubSubSub.nombreCuentaSubSubSub, ";
			strSQL += " CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle ";
			strSQL += " FROM ((((Cuentas INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
			strSQL += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
			strSQL += " INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
			strSQL += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub) ";
			strSQL += " WHERE CuentasDetalle.idCuentaDetalle='" + idCuentaDetalle + "'";
			OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
			OdbcDataReader conC = ssql.ExecuteReader();
			if (conC.Read()) {
				guardaNombreCuentaEn = conC.GetString(1);
				guardaNombreCuentaSubEn = conC.GetString(3);
				guardaNombreCuentaSubSubEn = conC.GetString(5);
				guardaNombreCuentaSubSubSubEn = conC.GetString(7);
				guardaNombreCuentaDetalleEn = conC.GetString(9);
			}
			conC.Close();
		}

		private void cambiarCuentaDeMovimientoToolStripMenuItem_Click(object sender, EventArgs e) {
			if (lstComprobantes.SelectedItems == null) {
				MessageBox.Show("Seleccione un registro.");
			} else {
				if (lstComprobantes.SelectedItems.Count != 1) {
					MessageBox.Show("Seleccione un registro.");
				} else {
					ListViewItem itm = lstComprobantes.SelectedItems[0];
					if (itm.Tag == null) {
						MessageBox.Show("Seleccione un registro de movimiento.");
					} else {
						int idMovimiento = (int)itm.Tag;
						string idCuentaDetalleOrig = itm.Text;
						if (idCuentaDetalleOrig != null && idCuentaDetalleOrig.Length > 0) {
							ventanaSelCuentaDetalle.seleccionaCuenta(idCuentaDetalleOrig);
						}
						ventanaSelCuentaDetalle.ShowDialog();
						string idCuentaDetalleSel = ventanaSelCuentaDetalle.dameIdCuentaDetalleSel();
						string nombreCuentaDetalleSel = ventanaSelCuentaDetalle.dameNombreCuentaDetalleSel();
						if (idCuentaDetalleSel != "") {
							string nomCuentaOrig = "", nomCuentaSubOrig = "", nomCuentaSubSubOrig = "", nomCuentaSubSubSubOrig = "", nomCuentaDetalleOrig = "";
							string nomCuentaDest = "", nomCuentaSubDest = "", nomCuentaSubSubDest = "", nomCuentaSubSubSubDest = "", nomCuentaDetalleDest = "";
							dameDetallesCuentaDetalle(idCuentaDetalleOrig, ref nomCuentaOrig, ref nomCuentaSubOrig, ref nomCuentaSubSubOrig, ref nomCuentaSubSubSubOrig, ref nomCuentaDetalleOrig);
							dameDetallesCuentaDetalle(idCuentaDetalleSel, ref nomCuentaDest, ref nomCuentaSubDest, ref nomCuentaSubSubDest, ref nomCuentaSubSubSubDest, ref nomCuentaDetalleDest);
							if (MessageBox.Show("Realmente desea cambiar la cuenta del movimiento?\n\n" +
								"Actual:\n"
								+ nomCuentaOrig + " / " + nomCuentaSubOrig + " / " + nomCuentaSubSubOrig + " / " + nomCuentaSubSubSubOrig + " / " + nomCuentaDetalleOrig + "\n\n" +
								"Final:\n"
								+ nomCuentaDest + " / " + nomCuentaSubDest + " / " + nomCuentaSubSubDest + " / " + nomCuentaSubSubSubDest + " / " + nomCuentaDetalleDest
								, "Cambiar cuenta", MessageBoxButtons.OKCancel) == DialogResult.OK) {
								//Aplicar el movimiento
								OdbcCommand ssql = new OdbcCommand("UPDATE Movimientos SET idCuentaDetalle='" + idCuentaDetalleSel + "' WHERE idMovimiento=" + idMovimiento, Global.conn);
								ssql.ExecuteNonQuery();
								itm.Text = idCuentaDetalleSel;
								itm.SubItems[1].Text = nombreCuentaDetalleSel;
								itm.BackColor = Color.Beige;
							}
						}
					}
				}
			}
		}

		private void cambiarCuentaDeMovimientoToolStripMenuItem1_Click(object sender, EventArgs e) {
			if (lstAuxiliar.SelectedItems == null) {
				MessageBox.Show("Seleccione un registro.");
			} else {
				if (lstAuxiliar.SelectedItems.Count != 1) {
					MessageBox.Show("Seleccione un registro.");
				} else {
					ListViewItem itm = lstAuxiliar.SelectedItems[0];
					if(itm.Tag==null){
						MessageBox.Show("Seleccione un registro de movimiento.");
					} else {
						int idMovimiento = int.Parse(itm.Text);
						ventanaSelCuentaDetalle.ShowDialog();
						string idCuentaDetalleOrig = (string)itm.Tag;
						string idCuentaDetalleSel = ventanaSelCuentaDetalle.dameIdCuentaDetalleSel();
						string nombreCuentaDetalleSel = ventanaSelCuentaDetalle.dameNombreCuentaDetalleSel();
						if (idCuentaDetalleSel != "") {
							string nomCuentaOrig = "", nomCuentaSubOrig = "", nomCuentaSubSubOrig = "", nomCuentaSubSubSubOrig = "", nomCuentaDetalleOrig = "";
							string nomCuentaDest = "", nomCuentaSubDest = "", nomCuentaSubSubDest = "", nomCuentaSubSubSubDest = "", nomCuentaDetalleDest = "";
							dameDetallesCuentaDetalle(idCuentaDetalleOrig, ref nomCuentaOrig, ref nomCuentaSubOrig, ref nomCuentaSubSubOrig, ref nomCuentaSubSubSubOrig, ref nomCuentaDetalleOrig);
							dameDetallesCuentaDetalle(idCuentaDetalleSel, ref nomCuentaDest, ref nomCuentaSubDest, ref nomCuentaSubSubDest, ref nomCuentaSubSubSubDest, ref nomCuentaDetalleDest);
							if (MessageBox.Show("Realmente desea cambiar la cuenta del movimiento?\n\n" +
								"Actual:\n"
								+ nomCuentaOrig + " / " + nomCuentaSubOrig + " / " + nomCuentaSubSubOrig + " / " + nomCuentaSubSubSubOrig + " / " + nomCuentaDetalleOrig + "\n\n" +
								"Final:\n"
								+ nomCuentaDest + " / " + nomCuentaSubDest + " / " + nomCuentaSubSubDest + " / " + nomCuentaSubSubSubDest + " / " + nomCuentaDetalleDest
								, "Cambiar cuenta", MessageBoxButtons.OKCancel) == DialogResult.OK) {
								//Aplicar el movimiento
								OdbcCommand ssql = new OdbcCommand("UPDATE Movimientos SET idCuentaDetalle='" + idCuentaDetalleSel + "' WHERE idMovimiento=" + idMovimiento, Global.conn);
								ssql.ExecuteNonQuery();
								itm.Text = idCuentaDetalleSel;
								itm.SubItems[1].Text = nombreCuentaDetalleSel;
								itm.BackColor = Color.Beige;
							}
						}
					}
				}
			}
		}

		

		private void button1_Click(object sender, EventArgs e) {
            //ToDo: remove or re-enable
            //int anoFinPeriodoFiscal = 2015;  string idComprobanteDestino = "2015-12-31-0016", idCuentaDetalleDestino = "030103010006";
            //int anoFinPeriodoFiscal = 2016; string idComprobanteDestino = "2016-12-31-0023", idCuentaDetalleDestino = "030103010007";
            //int anoFinPeriodoFiscal = 2017; string idComprobanteDestino = "2017-12-31-0016", idCuentaDetalleDestino = "030103010008";
            //int anoFinPeriodoFiscal = 2018; string idComprobanteDestino = "2018-12-31-0020", idCuentaDetalleDestino = "030103010009";
            //int anoFinPeriodoFiscal = 2019; string idComprobanteDestino = "2019-12-31-0007", idCuentaDetalleDestino = "030103010010";
            //int anoFinPeriodoFiscal = 2020; string idComprobanteDestino = "2020-12-31-0004", idCuentaDetalleDestino = "030103010011";
            //ToDo: remove or re-enable
			/*
            int anoFinPeriodoFiscal = 2021; string idComprobanteDestino = "2021-12-31-0007", idCuentaDetalleDestino = "030103010012";
            if (MessageBox.Show("Realmente desea mover todos los saldos de las cuentas INGRESOS, COSTOS y GASTOS hacia UTILIDADES?\n\nDestinos\nComprobante: '" + idComprobanteDestino + "'\nCuenta detalle: '" + idCuentaDetalleDestino + "'\nFechas antes de: Enero/" + (anoFinPeriodoFiscal + 1) + "", "Cierre fiscal", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                int conteoMovimientosAplicados = 0;
                string strSQL = "";
                strSQL += " SELECT CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle, Sum(Movimientos.montoDebeCS) AS SumaDemontoDebeCS, Sum(Movimientos.montoHaberCS) AS SumaDemontoHaberCS ";
                strSQL += " FROM ComprobantesDeDiario INNER JOIN (((((Cuentas INNER JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
                strSQL += " INNER JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) INNER JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
                strSQL += " INNER JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub) INNER JOIN Movimientos ON CuentasDetalle.idCuentaDetalle = Movimientos.idCuentaDetalle) ON ComprobantesDeDiario.idComprobanteDiario = Movimientos.idComprobanteDiario ";
                strSQL += " GROUP BY CuentasDetalle.idCuentaDetalle, CuentasDetalle.nombreCuentaDetalle, Cuentas.idCuenta, ComprobantesDeDiario.fechaComprobanteDiario ";
                strSQL += " HAVING (((Cuentas.idCuenta)='04' OR (Cuentas.idCuenta)='05' OR (Cuentas.idCuenta)='06' OR (Cuentas.idCuenta)='07') AND ((ComprobantesDeDiario.fechaComprobanteDiario) < '" + (anoFinPeriodoFiscal + 1) + "-01-01')) ";
                strSQL += " ORDER BY CuentasDetalle.idCuentaDetalle ";
                string nombreCuentaDetalleAnterior = "";
                string nombreCuentaDetalleAnterior125 = "";
                string idCuentaDetalleAnterior = "";
                double saldoCuentaAcum = 0.0;
                OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
                OdbcDataReader conC = ssql.ExecuteReader();
                while (conC.Read()) {
                    string idCuentaDetalle = conC.GetString(0);
                    string nomCuentaDetalle = conC.GetString(1);
                    string nomCuentaDetalle125 = nomCuentaDetalle; if (nomCuentaDetalle125.Length > 125) nomCuentaDetalle125 = nomCuentaDetalle125.Substring(0, 125);
                    double saldoDebeTmp = 0.0; if (!conC.IsDBNull(2)) saldoDebeTmp = conC.GetInt64(2);
                    double saldoHaberTmp = 0.0; if (!conC.IsDBNull(3)) saldoHaberTmp = conC.GetInt64(3);
                    if (idCuentaDetalleAnterior != idCuentaDetalle) {
                        if (idCuentaDetalleAnterior != "") {
                            double saldoDebe = 0.0;
                            double saldoHaber = 0.0;
                            if (saldoCuentaAcum < 0.0) {
                                saldoDebe = 0.0;
                                saldoHaber = -saldoCuentaAcum;
                            } else {
                                saldoDebe = saldoCuentaAcum;
                                saldoHaber = 0.0;
                            }
                            if (saldoCuentaAcum < -0.005 || saldoCuentaAcum > 0.005) {
                                string strSQL2 = "INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, montoDebeUS, montoHaberUS, referencia) VALUES('" + idComprobanteDestino + "', '" + idCuentaDetalleDestino + "', " + saldoDebe + ", " + saldoHaber + ", 0, NULL, NULL, 'Cierre fiscal " + anoFinPeriodoFiscal + " CTA " + idCuentaDetalleAnterior + " (" + nombreCuentaDetalleAnterior125 + ")')";
                                OdbcCommand ssql2 = new OdbcCommand(strSQL2, Global.conn);
                                ssql2.ExecuteNonQuery(); //MessageBox.Show("" + strSQL2);
                                strSQL2 = "INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, montoDebeUS, montoHaberUS, referencia) VALUES('" + idComprobanteDestino + "', '" + idCuentaDetalleAnterior + "', " + saldoHaber + ", " + saldoDebe + ", 0, NULL, NULL, 'Cierre fiscal " + anoFinPeriodoFiscal + " CTA " + idCuentaDetalleAnterior + " (" + nombreCuentaDetalleAnterior125 + ")')";
                                ssql2 = new OdbcCommand(strSQL2, Global.conn);
                                ssql2.ExecuteNonQuery(); //MessageBox.Show("" + strSQL2);
                                conteoMovimientosAplicados++;
                            }
                        }
                        idCuentaDetalleAnterior = idCuentaDetalle;
                        nombreCuentaDetalleAnterior = nomCuentaDetalle;
                        nombreCuentaDetalleAnterior125 = nomCuentaDetalle125;
                        saldoCuentaAcum = 0.0;
                    }
                    saldoCuentaAcum = saldoCuentaAcum + saldoDebeTmp - saldoHaberTmp;
                }
                //Procesar acumulado pendiente
                if (idCuentaDetalleAnterior != "") {
                    double saldoDebe = 0.0;
                    double saldoHaber = 0.0;
                    if (saldoCuentaAcum < 0.0) {
                        saldoDebe = 0.0;
                        saldoHaber = -saldoCuentaAcum;
                    } else {
                        saldoDebe = saldoCuentaAcum;
                        saldoHaber = 0.0;
                    }
                    if (saldoCuentaAcum < -0.005 || saldoCuentaAcum > 0.005) {
                        string strSQL2 = "INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, montoDebeUS, montoHaberUS, referencia) VALUES('" + idComprobanteDestino + "', '" + idCuentaDetalleDestino + "', " + saldoDebe + ", " + saldoHaber + ", 0, NULL, NULL, 'Cierre fiscal " + anoFinPeriodoFiscal + " CTA " + idCuentaDetalleAnterior + " (" + nombreCuentaDetalleAnterior125 + ")')";
                        OdbcCommand ssql2 = new OdbcCommand(strSQL2, Global.conn);
                        ssql2.ExecuteNonQuery(); //MessageBox.Show("" + strSQL2);
                        strSQL2 = "INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, montoDebeUS, montoHaberUS, referencia) VALUES('" + idComprobanteDestino + "', '" + idCuentaDetalleAnterior + "', " + saldoHaber + ", " + saldoDebe + ", 0, NULL, NULL, 'Cierre fiscal " + anoFinPeriodoFiscal + " CTA " + idCuentaDetalleAnterior + " (" + nombreCuentaDetalleAnterior125 + ")')";
                        ssql2 = new OdbcCommand(strSQL2, Global.conn);
                        ssql2.ExecuteNonQuery(); //MessageBox.Show("" + strSQL2);
                        conteoMovimientosAplicados++;
                    }
                }
                MessageBox.Show(conteoMovimientosAplicados + " registros aplicados");
            }
			*/
		}

        private void toolStripMenuItem1_Click(object sender, EventArgs e) {
			if (lstAuxiliar.SelectedItems == null) {
				MessageBox.Show("Seleccione por lo menos un registro.");
			} else {
				if (lstAuxiliar.SelectedItems.Count == 0) {
					MessageBox.Show("Seleccione por lo menos un registro.");
				} else {
                    string primeraIdCuentaDetalleOrig = "";
                    string idCuentaDetalleSel = "";
                    string nombreCuentaDetalleSel = "";
                    int iSel; int confirmValue = 2; //2 = ask, 1 = confirmed, 0 = denied
                    for (iSel = 0; iSel < lstAuxiliar.SelectedItems.Count && confirmValue != 0; iSel++) {
                        ListViewItem itm = lstAuxiliar.SelectedItems[iSel];
                        if (itm.Tag == null) {
                            MessageBox.Show("Seleccione un registro de movimiento.");
                        } else {
                            int idMovimiento = 0;
                            try {
                                idMovimiento = int.Parse(itm.Text);
                            } catch (Exception) {
                                MessageBox.Show("Seleccione un registro de movimiento (idMovimiento no numerico)");
                            }
                            if (idMovimiento > 0) {
                                string idCuentaDetalleOrig = "";
                                string strSQL = "SELECT idCuentaDetalle FROM Movimientos WHERE idMovimiento=" + idMovimiento;
                                OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
                                OdbcDataReader conC = ssql.ExecuteReader();
                                if (conC.Read()) {
                                    idCuentaDetalleOrig = conC.GetString(0);
                                }
                                conC.Close();
                                if (idCuentaDetalleOrig == "") {
                                    MessageBox.Show("El idMovimiento no retorna una idCuentaDetalle");
                                } else {
                                    if (primeraIdCuentaDetalleOrig == "") {
                                        primeraIdCuentaDetalleOrig = idCuentaDetalleOrig;
										if (idCuentaDetalleOrig != null && idCuentaDetalleOrig.Length > 0) {
											ventanaSelCuentaDetalle.seleccionaCuenta(idCuentaDetalleOrig);
										}
                                        ventanaSelCuentaDetalle.ShowDialog();
                                        idCuentaDetalleSel = ventanaSelCuentaDetalle.dameIdCuentaDetalleSel();
                                        nombreCuentaDetalleSel = ventanaSelCuentaDetalle.dameNombreCuentaDetalleSel();
                                    }
                                    if (idCuentaDetalleOrig != primeraIdCuentaDetalleOrig) {
                                        MessageBox.Show("Solo se moveran las que tengan cuenta de origen '" + primeraIdCuentaDetalleOrig + "'");
                                    } else {
                                        if (idCuentaDetalleSel != "") {
											//Ask confirmation
											if (confirmValue == 2) {
												string nomCuentaOrig = "", nomCuentaSubOrig = "", nomCuentaSubSubOrig = "", nomCuentaSubSubSubOrig = "", nomCuentaDetalleOrig = "";
												string nomCuentaDest = "", nomCuentaSubDest = "", nomCuentaSubSubDest = "", nomCuentaSubSubSubDest = "", nomCuentaDetalleDest = "";
												dameDetallesCuentaDetalle(idCuentaDetalleOrig, ref nomCuentaOrig, ref nomCuentaSubOrig, ref nomCuentaSubSubOrig, ref nomCuentaSubSubSubOrig, ref nomCuentaDetalleOrig);
												dameDetallesCuentaDetalle(idCuentaDetalleSel, ref nomCuentaDest, ref nomCuentaSubDest, ref nomCuentaSubSubDest, ref nomCuentaSubSubSubDest, ref nomCuentaDetalleDest);
												switch (MessageBox.Show("Realmente desea cambiar la cuenta del movimiento?\n\n" +
													"Actual:\n"
													+ nomCuentaOrig + " / " + nomCuentaSubOrig + " / " + nomCuentaSubSubOrig + " / " + nomCuentaSubSubSubOrig + " / " + nomCuentaDetalleOrig + "\n\n" +
													"Final:\n"
													+ nomCuentaDest + " / " + nomCuentaSubDest + " / " + nomCuentaSubSubDest + " / " + nomCuentaSubSubSubDest + " / " + nomCuentaDetalleDest
													, "Cambiar cuenta", MessageBoxButtons.OKCancel)) {
													case DialogResult.OK:
														confirmValue = 1;
														break;
													default:
														confirmValue = 0;
														break;
												}
											}
											//Aplicar el cambio
											if (confirmValue == 1) {
                                                //Aplicar el movimiento
                                                ssql = new OdbcCommand("UPDATE Movimientos SET idCuentaDetalle='" + idCuentaDetalleSel + "' WHERE idMovimiento=" + idMovimiento, Global.conn);
                                                ssql.ExecuteNonQuery();
                                                itm.BackColor = Color.Red;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
				}
			}
        }

        private void arbCuentas_MouseDoubleClick(object sender, MouseEventArgs e) {
            if(arbCuentas.SelectedNode != null){
				if (arbCuentas.SelectedNode.Tag != null) {
					string tagNodo = arbCuentas.SelectedNode.Tag.ToString();
					if (tagNodo.IndexOf("cuentaDetalle_") == 0) {
						tagNodo = tagNodo.Replace("cuentaDetalle_", "");
						if (tagNodo.Length == 12) {
							idCuentaAuxiliarBuscar = tagNodo;
							tabCuentas.SelectTab(1);
							btnOKAuxiliar_Click(null, null);
						}
					}
				}
            }
        }

        private void lstComprobantes_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lstComprobantes.SelectedItems != null) {
                if (lstComprobantes.SelectedItems.Count == 1) {
                    ListViewItem itm = lstComprobantes.SelectedItems[0];
                    try {
					    if(itm.Tag!=null)idMovimientoResaltar = (int)itm.Tag;
                        else idMovimientoResaltar = 0;
                    } catch (Exception) {
                        idMovimientoResaltar = 0;
                    }
                    idCuentaAuxiliarBuscar = lstComprobantes.SelectedItems[0].SubItems[0].Text;
                    tabCuentas.SelectTab(1);
                    btnOKAuxiliar_Click(null, null);
                }
            }
        }

        private void lstAuxiliar_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (lstAuxiliar.SelectedItems != null) {
                if (lstAuxiliar.SelectedItems.Count == 1) {
                    try {
                        idMovimientoResaltar = int.Parse(lstAuxiliar.SelectedItems[0].SubItems[0].Text);
                    } catch (Exception) {
                        idMovimientoResaltar = 0;
                    }
                    idComprobanteBuscar = lstAuxiliar.SelectedItems[0].SubItems[1].Text;
                    tabCuentas.SelectTab(0);
                    btnOKDiario_Click(null, null);
                }
            }
        }

        //ToDo: remove or re-enable
        /*private void aplicarAmortizacionesDeTipo(string idAmortizacionTipo) {
            bool validacionCorrecta = true;
            string idComprobante = ""; //ToDo: generate here
            if (validacionCorrecta && idComprobante == "") {
                MessageBox.Show("Seleccione el comprobante de los movimientos.");
                validacionCorrecta = false;
            }
            if (validacionCorrecta) {
                int conteoRegistrosEnComprobante = 0;
                string strSQL = "SELECT COUNT(*) AS conteo FROM Movimientos WHERE idComprobanteDiario='" + idComprobante + "'";
                OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
                OdbcDataReader conC = ssql.ExecuteReader();
                if (conC.Read()) {
                    conteoRegistrosEnComprobante = conC.GetInt32(0);
                } else {
                    MessageBox.Show("El comprobante '" + idComprobante + "' no fue encontrado en la Base de Datos");
                    validacionCorrecta = false;
                }
                conC.Close();
                if (conteoRegistrosEnComprobante != 0) {
                    if (MessageBox.Show("El comprobante '" + idComprobante + "' tiene " + conteoRegistrosEnComprobante + " registros. Desea registrar en este de todas formas?", "Registro no vacio", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                        validacionCorrecta = false;
                    }
                }
            }
            if (validacionCorrecta) {
                if (MessageBox.Show("Confirma que desea registrar las amortizaciones pendientes aplicables en el mes del comprobante?\n\nIMPORTANTE: recuerde aplicar las amortizaciones mes a mes y en orden.", "Confirmacion", MessageBoxButtons.OKCancel) == DialogResult.Cancel) {
                    validacionCorrecta = false;
                }
            }
            if (validacionCorrecta) {
                //Datos del comprobante
                string strSQL = "";
                OdbcCommand ssql = null;
                OdbcDataReader conC = null;
                //
                int anoComprobante = 0;
                int mesComprobante = 0;
                strSQL = "SELECT fechaComprobanteDiario FROM ComprobantesDeDiario WHERE idComprobanteDiario='" + idComprobante + "'";
                ssql = new OdbcCommand(strSQL, Global.conn);
                conC = ssql.ExecuteReader();
                if (conC.Read()) {
                    DateTime fecha = conC.GetDateTime(0);
                    anoComprobante = fecha.Year;
                    mesComprobante = fecha.Month;
                }
                conC.Close();
                //
                string descAmortizacionesAplicadas = "";
                int conteoAmortizacionesAplicadas = 0;
                float montoTotalAmortizado = 0.0f;
                strSQL = "SELECT Amortizaciones.idAmortizacion, Amortizaciones.nombreAmortizacion, Amortizaciones.anoInicioAmortizacion, Amortizaciones.mesInicioAmortizacion, Amortizaciones.mesesParaAmortizacionTotal, Amortizaciones.montoTotalAmortizar, (SELECT SUM(AmortizacionesDetalles.montoAmortizado) FROM AmortizacionesDetalles WHERE AmortizacionesDetalles.idAmortizacion=Amortizaciones.idAmortizacion) AS montoAmortizado, Amortizaciones.idCuentaDetalleDebe, Amortizaciones.idCuentaDetalleHaber FROM Amortizaciones WHERE Amortizaciones.idAmortizacionTipo='" + idAmortizacionTipo + "'";
                ssql = new OdbcCommand(strSQL, Global.conn);
                conC = ssql.ExecuteReader();
                while (conC.Read()) {
                    int idAmortizacion = conC.GetInt32(0);
                    string nombreAmortizacion = conC.GetString(1);
                    int anoIniAmortiza = conC.GetInt16(2);
                    int mesIniAmortiza = conC.GetByte(3);
                    int mesesParaAmortizar = conC.GetByte(4);
                    float montoTotalAmortizar = conC.GetFloat(5);
                    float montoAmortizado = 0.0f; if (!conC.IsDBNull(6)) montoAmortizado = (float)conC.GetInt64(6);
                    string idCuentaDetalleDebe = conC.GetString(7);
                    string idCuentaDetalleHaber = conC.GetString(8);
                    float montoFaltaAmortizar = montoTotalAmortizar - montoAmortizado;
                    if (montoFaltaAmortizar < 0.0) {
                        MessageBox.Show("Advertencia: la amortizacion anterior aplicada supera el monto total a amortizar para:\n'" + nombreAmortizacion + "' " + montoAmortizado + " de " + montoTotalAmortizar + "");
                    } else if (montoFaltaAmortizar > 0.0 && (anoIniAmortiza < anoComprobante || (anoIniAmortiza == anoComprobante && mesIniAmortiza <= mesComprobante))) {
                        string strSQL2 = "SELECT COUNT(*) AS conteo FROM AmortizacionesDetalles WHERE idAmortizacion=" + idAmortizacion + " AND anoAmortizacion=" + anoComprobante + " AND mesAmortizacion=" + mesComprobante + "";
                        OdbcCommand ssql2 = new OdbcCommand(strSQL2, Global.conn);
                        OdbcDataReader conC2 = ssql2.ExecuteReader();
                        if (conC2.Read()) {
                            int registrosExistentes = conC2.GetInt32(0);
                            if (registrosExistentes == 0) {
                                float montoAmortizacionMensual = montoTotalAmortizar / (float)mesesParaAmortizar;
                                float montoAmortizarEnRegistro = montoAmortizacionMensual;
                                //Validar que la amortacion no produzca un balance negativo
                                if (montoAmortizarEnRegistro > montoFaltaAmortizar) montoAmortizarEnRegistro = montoFaltaAmortizar;
                                //Si el sobrante es poco, mejor amortizar aqui.
                                float sobranteDespuesDeAmortizar = montoFaltaAmortizar - montoAmortizarEnRegistro;
                                if ((sobranteDespuesDeAmortizar / montoAmortizacionMensual) < 0.10) montoAmortizarEnRegistro = montoFaltaAmortizar;
                                //
                                int idMovimientoDebe = 0;
                                int idMovimientoHaber = 0;
                                OdbcCommand ssql3 = null;
                                OdbcCommand ssql4 = null;
                                OdbcDataReader conC4 = null;
                                //Registrar el debe
                                ssql3 = new OdbcCommand("INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) VALUES('" + idComprobante + "', '" + idCuentaDetalleDebe + "', '" + String.Format("{0:0,0.00}", montoAmortizarEnRegistro) + "', NULL, 0, NULL, NULL, NULL, '" + mesComprobante + "/" + anoComprobante + " " + nombreAmortizacion + "');", Global.conn);
                                ssql3.ExecuteNonQuery();
                                ssql3 = new OdbcCommand("SELECT MAX(idMovimiento) FROM Movimientos", Global.conn);
                                conC4 = ssql3.ExecuteReader();
                                conC4.Read();
                                idMovimientoDebe = conC4.GetInt32(0);
                                conC4.Close();
                                //Registrar el haber
                                ssql3 = new OdbcCommand("INSERT INTO Movimientos(idComprobanteDiario, idCuentaDetalle, montoDebeCS, montoHaberCS, esDolares, tasaCambio, montoDebeUS, montoHaberUS, referencia) VALUES('" + idComprobante + "', '" + idCuentaDetalleHaber + "', NULL, '" + String.Format("{0:0,0.00}", montoAmortizarEnRegistro) + "', 0, NULL, NULL, NULL, '" + mesComprobante + "/" + anoComprobante + " " + nombreAmortizacion + "');", Global.conn);
                                ssql3.ExecuteNonQuery();
                                ssql3 = new OdbcCommand("SELECT MAX(idMovimiento) FROM Movimientos", Global.conn);
                                conC4 = ssql3.ExecuteReader();
                                conC4.Read();
                                idMovimientoHaber = conC4.GetInt32(0);
                                conC4.Close();
                                //Registrar la amortizacion
                                ssql3 = new OdbcCommand("INSERT INTO AmortizacionesDetalles(idAmortizacion, anoAmortizacion, mesAmortizacion, montoAmortizado, idMovimientoDebe, idMovimientoHaber) VALUES(" + idAmortizacion + ", " + anoComprobante + ", " + mesComprobante + ", '" + String.Format("{0:0,0.00}", montoAmortizarEnRegistro) + "', " + idMovimientoDebe + ", " + idMovimientoHaber + ")", Global.conn);
                                ssql3.ExecuteNonQuery();
                                //
                                descAmortizacionesAplicadas = descAmortizacionesAplicadas + "C$ " + String.Format("{0:0,0.00}", montoAmortizarEnRegistro) + ": " + nombreAmortizacion + "\n";
                                montoTotalAmortizado += montoAmortizarEnRegistro;
                                conteoAmortizacionesAplicadas++;
                            }
                        }
                        conC2.Close();
                    }
                }
                conC.Close();
                MessageBox.Show(conteoAmortizacionesAplicadas + " amortizaciones aplicadas\n\n" + descAmortizacionesAplicadas + "\n\nTotal: C$" + String.Format("{0:0,0.00}", montoTotalAmortizado));
            }
        }*/

        private void btnAmortizaciones_Click(object sender, EventArgs e) {
			//ToDo: remove or re-enable
            //aplicarAmortizacionesDeTipo("gst");
        }

        private void btnDepreciacion_Click(object sender, EventArgs e) {
			//ToDo: remove or re-enable
            //aplicarAmortizacionesDeTipo("dep");
        }

		private void eliminarMovimientoToolStripMenuItem_Click(object sender, EventArgs e) {
			if (lstAuxiliar.SelectedItems == null) {
				MessageBox.Show("Seleccione por lo menos un registro.");
			} else {
				if (lstAuxiliar.SelectedItems.Count == 0) {
					MessageBox.Show("Seleccione por lo menos un registro.");
				} else {
					if (MessageBox.Show("Realmente dese eliminar los registros seleccionados?", "Eliminar?", MessageBoxButtons.OKCancel) == DialogResult.OK) {
						int iSel;
						for (iSel = 0; iSel < lstAuxiliar.SelectedItems.Count; iSel++) {
							ListViewItem itm = lstAuxiliar.SelectedItems[iSel];
							if (itm.Tag == null) {
								MessageBox.Show("Seleccione un registro de movimiento.");
							} else {
								int idMovimiento = 0;
								try {
									idMovimiento = int.Parse(itm.Text);
								} catch (Exception) {
									MessageBox.Show("Seleccione un registro de movimiento (idMovimiento no numerico)");
								}
								if (idMovimiento > 0) {
									string strSQL = "DELETE FROM Movimientos WHERE idMovimiento=" + idMovimiento;
									OdbcCommand ssql = new OdbcCommand(strSQL, Global.conn);
									ssql.ExecuteNonQuery();
									itm.BackColor = Color.Red;
								}
							}
						}
					}
				}
			}
		}

		private void eliminarMovimientoToolStripMenuItem1_Click(object sender, EventArgs e) {
			if (lstComprobantes.SelectedItems == null) {
				MessageBox.Show("Seleccione un registro.");
			} else {
				if (lstComprobantes.SelectedItems.Count != 1) {
					MessageBox.Show("Seleccione un registro.");
				} else {
					ListViewItem itm = lstComprobantes.SelectedItems[0];
					if (itm.Tag == null) {
						MessageBox.Show("Seleccione un registro de movimiento.");
					} else {
						int idMovimiento = (int)itm.Tag;
						if (MessageBox.Show("Realmente desea eliminar el registro del movimiento?", "Eliminar?", MessageBoxButtons.OKCancel) == DialogResult.OK) {
							//Aplicar el movimiento
							OdbcCommand ssql = new OdbcCommand("DELETE FROM Movimientos WHERE idMovimiento=" + idMovimiento, Global.conn);
							ssql.ExecuteNonQuery();
							itm.BackColor = Color.Red;
						}
					}
				}
			}
		}

        private void aOtraCuentaToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void tnSrvBasics_Click(object sender, EventArgs e) {
			//ToDo: remove or re-enable
			/*FrmServiciosBasicos formu = new FrmServiciosBasicos();
			formu.ShowDialog();
			//
			if (!formu.isCanceled) {
				Decimal agua = formu.agua;
				Decimal energia = formu.energia;
				Decimal internet = formu.internet;
				//llenar grilla
					if (agua > 0) {
						//obtener el nombre de la cuenta
						string idCuenta = "060101030002", nombreCuenta = "";
						OdbcCommand ssql = new OdbcCommand("SELECT nombreCuentaDetalle FROM CuentasDetalle WHERE idCuentaDetalle='" + idCuenta + "'", Global.conn);
						OdbcDataReader conC = ssql.ExecuteReader();
						if (conC.Read()) {
							nombreCuenta = conC.GetString(0);
						}
						conC.Close();
						//agregar fila
						DataGridViewTextBoxCell txtCelda;
						DataGridViewCheckBoxCell chkCelda;
						DataGridViewRow fila = new DataGridViewRow();
						//numero de cuenta
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = idCuenta;
						fila.Cells.Add(txtCelda);
						//nombre de cuenta
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = nombreCuenta;
						fila.Cells.Add(txtCelda);
						//debe
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = String.Format("{0:0.00}", agua);
						fila.Cells.Add(txtCelda);
						//haber
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value =  "";
						fila.Cells.Add(txtCelda);
						//check 'es dolar'
						chkCelda = new DataGridViewCheckBoxCell(); chkCelda.Value = false;
						fila.Cells.Add(chkCelda);
						//tasa cambio
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "";
						fila.Cells.Add(txtCelda);
						//descripcion
						txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "Servicio basico, agua";
						fila.Cells.Add(txtCelda);
						//
						gridMovimientos.Rows.Add(fila);
					}
				//llenar grilla
				if (energia > 0) {
					//obtener el nombre de la cuenta
					string idCuenta = "060101030001", nombreCuenta = "";
					OdbcCommand ssql = new OdbcCommand("SELECT nombreCuentaDetalle FROM CuentasDetalle WHERE idCuentaDetalle='" + idCuenta + "'", Global.conn);
					OdbcDataReader conC = ssql.ExecuteReader();
					if (conC.Read()) {
						nombreCuenta = conC.GetString(0);
					}
					conC.Close();
					//agregar fila
					DataGridViewTextBoxCell txtCelda;
					DataGridViewCheckBoxCell chkCelda;
					DataGridViewRow fila = new DataGridViewRow();
					//numero de cuenta
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = idCuenta;
					fila.Cells.Add(txtCelda);
					//nombre de cuenta
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = nombreCuenta;
					fila.Cells.Add(txtCelda);
					//debe
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = String.Format("{0:0.00}", energia);
					fila.Cells.Add(txtCelda);
					//haber
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "";
					fila.Cells.Add(txtCelda);
					//check 'es dolar'
					chkCelda = new DataGridViewCheckBoxCell(); chkCelda.Value = false;
					fila.Cells.Add(chkCelda);
					//tasa cambio
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "";
					fila.Cells.Add(txtCelda);
					//descripcion
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "Servicio basico, energia";
					fila.Cells.Add(txtCelda);
					//
					gridMovimientos.Rows.Add(fila);
				}
				//llenar grilla
				if (internet > 0) {
					//obtener el nombre de la cuenta
					string idCuenta = "060101030005", nombreCuenta = "";
					OdbcCommand ssql = new OdbcCommand("SELECT nombreCuentaDetalle FROM CuentasDetalle WHERE idCuentaDetalle='" + idCuenta + "'", Global.conn);
					OdbcDataReader conC = ssql.ExecuteReader();
					if (conC.Read()) {
						nombreCuenta = conC.GetString(0);
					}
					conC.Close();
					//agregar fila
					DataGridViewTextBoxCell txtCelda;
					DataGridViewCheckBoxCell chkCelda;
					DataGridViewRow fila = new DataGridViewRow();
					//numero de cuenta
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = idCuenta;
					fila.Cells.Add(txtCelda);
					//nombre de cuenta
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = nombreCuenta;
					fila.Cells.Add(txtCelda);
					//debe
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = String.Format("{0:0.00}", internet);
					fila.Cells.Add(txtCelda);
					//haber
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "";
					fila.Cells.Add(txtCelda);
					//check 'es dolar'
					chkCelda = new DataGridViewCheckBoxCell(); chkCelda.Value = false;
					fila.Cells.Add(chkCelda);
					//tasa cambio
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "";
					fila.Cells.Add(txtCelda);
					//descripcion
					txtCelda = new DataGridViewTextBoxCell(); txtCelda.Value = "Servicio basico, internet";
					fila.Cells.Add(txtCelda);
					//
					gridMovimientos.Rows.Add(fila);
				}
			}
			//
			formu = null;
			*/
		}

        private void tmrAsync_Tick(object sender, EventArgs e) {
			//analyze tree-first-upd
			if(_treeFirstUpdateWait > 0) {
				_treeFirstUpdateWait -= tmrAsync.Interval;
				if (_treeFirstUpdateWait <= 0) {
					_treeFirstUpdateWait = 0;
					//update tree
					string treeExpandedLst = null;
                    ClsAppState.AppStateRootServer sState = ClsAppState.getServerStateCurrent();
					if (sState != null && sState.frmContab != null) {
						_treeShowBalances = sState.frmContab.showBalances;
                        _treeShowBalancesSep = sState.frmContab.showBalancesSep;
                        treeExpandedLst = sState.frmContab.treeExpandedLst;
                    }
                    this.actualizaArbolCuentas(treeExpandedLst);
                }
            }
            //analyze tasa cambio (async)
			if(_tasaCambioTask != null) {
				if (_tasaCambioTask.IsCanceled || _tasaCambioTask.IsFaulted) {
					_tasaCambioTask = null;
					_tasaCambioRowIndex = -1;
                } else if (_tasaCambioTask.IsCompleted) {
                    double tasa = _tasaCambioTask.Result;
					if (tasa >= 30 && _tasaCambioRowIndex >= 0 && _tasaCambioRowIndex < gridMovimientos.Rows.Count) {
						if (gridMovimientos.Rows[_tasaCambioRowIndex].Cells[5].Value == null || gridMovimientos.Rows[_tasaCambioRowIndex].Cells[5].Value.ToString().Length <= 0) {
							gridMovimientos.Rows[_tasaCambioRowIndex].Cells[5].Value = String.Format("{0:0.0000}", tasa);
						} else {
							//MessageBox.Show();
						}
					}
                    _tasaCambioTask = null;
                    _tasaCambioRowIndex = -1;
                }
            }
        }

        private void renombrarToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode nodoSel = arbCuentas.SelectedNode;
            if (nodoSel == null) {
                MessageBox.Show("Seleccione un nodo del arbol.");
            } else {
                if (nodoSel.Tag == null) {
                    MessageBox.Show("Seleccione un nodo CUENTA del arbol.");
                } else {
                    int nivel = 0;
                    string etiqueta = (string)nodoSel.Tag;
                    string texto = (string)nodoSel.Text;
                    string idRegistroCuenta = "", posFijoTabla = "";
                    string numCuenta = "";
                    {
                        int posSepNum = texto.IndexOf("-");
                        if (posSepNum >= 0) {
                            numCuenta = texto.Substring(0, posSepNum).Trim();
                        }
                    }
                    if (etiqueta.IndexOf("cuenta_") == 0) {
                        nivel = 1; idRegistroCuenta = etiqueta.Replace("cuenta_", ""); posFijoTabla = "";
                    }
                    if (etiqueta.IndexOf("cuentaSub_") == 0) {
                        nivel = 2; idRegistroCuenta = etiqueta.Replace("cuentaSub_", ""); posFijoTabla = "Sub";
                    }
                    if (etiqueta.IndexOf("cuentaSubSub_") == 0) {
                        nivel = 3; idRegistroCuenta = etiqueta.Replace("cuentaSubSub_", ""); posFijoTabla = "SubSub";
                    }
                    if (etiqueta.IndexOf("cuentaSubSubSub_") == 0) {
                        nivel = 4; idRegistroCuenta = etiqueta.Replace("cuentaSubSubSub_", ""); posFijoTabla = "SubSubSub";
                    }
                    if (etiqueta.IndexOf("cuentaDetalle_") == 0) {
                        nivel = 5; idRegistroCuenta = etiqueta.Replace("cuentaDetalle_", ""); posFijoTabla = "Detalle";
                    }
                    if (nivel < 1 || nivel > 5) {
                        MessageBox.Show("Seleccione un nodo CUENTA del arbol.");
                    } else {
                        string nameRaw = nodoSel.Text;
                        string name = nameRaw;
                        //remove account number
                        if (name.IndexOf("-") > 0) {
                            while (name.Length > 0) {
                                if (name[0] == ' ' || name[0] == '0' || name[0] == '1' || name[0] == '2' || name[0] == '3' || name[0] == '4' || name[0] == '5' || name[0] == '6' || name[0] == '7' || name[0] == '8' || name[0] == '9') {
                                    name = name.Substring(1);
                                } else if (name[0] == '-') {
                                    name = name.Substring(1);
                                    break;
                                }
                            }
                            name = name.Trim();
                        }
                        //remove parentesis (extra info)
                        if (name.Length > 0 && name[name.Length - 1] == ')') {
                            //find '('
                            int parentesisStart = name.Length - 2;
                            while (parentesisStart >= 0) {
                                if (name[parentesisStart] == '(') {
                                    break;
                                }
                                parentesisStart--;
                            }
                            if (parentesisStart >= 0) {
                                name = name.Substring(0, parentesisStart).Trim();
                            }
                        }
                        //query
                        {
                            FrmSelTextLine frm = new FrmSelTextLine("Renombrar '" + nameRaw + "':", name);
                            frm.ShowDialog();
                            if (frm.textSel == null) {
                                //canceled
                            } else {
                                string nuevoNombre = frm.textSel.Trim(); if (nivel < 5) nuevoNombre = nuevoNombre.ToUpper();
                                if (nuevoNombre == "") {
                                    MessageBox.Show("Especifique el nuevo nombre para esta cuenta.");
                                } else {
                                    if (MessageBox.Show("Realmente desdea RENOMBRAR la cuenta?\n\nNuevo nombre: '" + nuevoNombre + "'", "Renombrar cuenta", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                                        //Aplicar el movimiento
                                        OdbcCommand ssql = new OdbcCommand("UPDATE Cuentas" + posFijoTabla + " SET nombreCuenta" + posFijoTabla + "='" + nuevoNombre + "' WHERE idCuenta" + posFijoTabla + "='" + idRegistroCuenta + "'", Global.conn);
                                        ssql.ExecuteNonQuery();
                                        nodoSel.Text = numCuenta + " - " + nuevoNombre;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void crearHijaToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode nodoSel = arbCuentas.SelectedNode;
            if (nodoSel == null) {
                MessageBox.Show("Seleccione un nodo del arbol.");
            } else {
                if (nodoSel.Tag == null) {
                    MessageBox.Show("Seleccione un nodo CUENTA del arbol.");
                } else {
                    string etiqueta = (string)nodoSel.Tag;
                    int nivel = 0;
                    string idRegistroCuenta = "", posfijoTabla = "", posfijoTablaHija = "";
                    if (etiqueta.IndexOf("cuenta_") == 0) {
                        nivel = 1; idRegistroCuenta = etiqueta.Replace("cuenta_", ""); posfijoTabla = ""; posfijoTablaHija = "Sub";
                    }
                    if (etiqueta.IndexOf("cuentaSub_") == 0) {
                        nivel = 2; idRegistroCuenta = etiqueta.Replace("cuentaSub_", ""); posfijoTabla = "Sub"; posfijoTablaHija = "SubSub";
                    }
                    if (etiqueta.IndexOf("cuentaSubSub_") == 0) {
                        nivel = 3; idRegistroCuenta = etiqueta.Replace("cuentaSubSub_", ""); posfijoTabla = "SubSub"; posfijoTablaHija = "SubSubSub";
                    }
                    if (etiqueta.IndexOf("cuentaSubSubSub_") == 0) {
                        nivel = 4; idRegistroCuenta = etiqueta.Replace("cuentaSubSubSub_", ""); posfijoTabla = "SubSubSub"; posfijoTablaHija = "Detalle";
                    }
                    if (etiqueta.IndexOf("cuentaDetalle_") == 0) {
                        nivel = 5; idRegistroCuenta = etiqueta.Replace("cuentaDetalle_", ""); posfijoTabla = "Detalle"; posfijoTablaHija = "";
                    }
                    if (nivel == 5) {
                        MessageBox.Show("No se puede crear cuenta hija.\nLa seleccionada es nodo hoja (último nivel).");
                    } else {
                        if (nivel < 1 || nivel > 5) {
                            MessageBox.Show("Seleccione un nodo CUENTA del arbol.");
                        } else {
                            FrmSelTextLine frm = new FrmSelTextLine("Nombre de nueva cuenta:", "");
                            frm.ShowDialog();
                            if (frm.textSel == null) {
                                //canceled
                            } else {
                                string nuevoNombre = frm.textSel.Trim(); if (nivel < 4) nuevoNombre = nuevoNombre.ToUpper();
                                if (nuevoNombre.Length <= 0) {
                                    MessageBox.Show("Especifique el nombre de la nueva cuenta hija.");
                                } else {
                                    //Averiguar el siguiente numero disponible
                                    int numDisponible = 1; bool numDisponibleEncontrado = false; string numerosUsados = "";
                                    OdbcCommand ssql = new OdbcCommand("SELECT numCuenta" + posfijoTablaHija + " FROM Cuentas" + posfijoTablaHija + " WHERE idCuenta" + posfijoTabla + "='" + idRegistroCuenta + "' ORDER BY numCuenta" + posfijoTablaHija + "", Global.conn);
                                    OdbcDataReader conC = ssql.ExecuteReader();
                                    while (conC.Read()) {
                                        int esteNum = conC.GetInt32(0);
                                        if (!numDisponibleEncontrado) {
                                            if (esteNum != numDisponible) {
                                                numDisponibleEncontrado = true;
                                            } else {
                                                numDisponible = esteNum + 1;
                                            }
                                        }
                                        numerosUsados += esteNum + ", ";
                                    }
                                    conC.Close();
                                    //MessageBox.Show("Siguiente numero disponible: " + numDisponible + " (de " + numerosUsados + ")");
                                    string idCuentaHija = idRegistroCuenta + Global.rellenaEntero(numDisponible, (nivel == 4 ? 4 : 2));
                                    if (MessageBox.Show("Realmente desdea CREAR una cuenta hija?\n\nNombre: '" + nuevoNombre + "'\nID: '" + idCuentaHija + "' (#" + numDisponible + ")", "Crear cuenta", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                                        //Crear cuenta hija
                                        OdbcCommand ssql2 = new OdbcCommand("INSERT INTO Cuentas" + posfijoTablaHija + "(idCuenta" + posfijoTabla + ", idCuenta" + posfijoTablaHija + ", numCuenta" + posfijoTablaHija + ", nombreCuenta" + posfijoTablaHija + ") VALUES('" + idRegistroCuenta + "', '" + idCuentaHija + "', " + numDisponible + ", '" + nuevoNombre + "')", Global.conn);
                                        ssql2.ExecuteNonQuery();
                                        TreeNode nuevoNodo = nodoSel.Nodes.Add("cuenta" + posfijoTablaHija + "_" + idCuentaHija, numDisponible + " - " + nuevoNombre, (nivel == 4 ? "folderdatos.gif" : "folder.gif")); nuevoNodo.SelectedImageKey = nuevoNodo.ImageKey;
                                        nuevoNodo.Tag = "cuenta" + posfijoTablaHija + "_" + idCuentaHija;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e) {
            TreeNode nodoSel = arbCuentas.SelectedNode;
            if (nodoSel == null) {
                MessageBox.Show("Seleccione un nodo del arbol.");
            } else {
                if (nodoSel.Tag == null) {
                    MessageBox.Show("Seleccione un nodo CUENTA del arbol.");
                } else {
                    string etiqueta = (string)nodoSel.Tag;
                    int nivel = 0;
                    string idRegistroCuenta = "", posFijoTabla = "";
                    if (etiqueta.IndexOf("cuenta_") == 0) {
                        nivel = 1; idRegistroCuenta = etiqueta.Replace("cuenta_", ""); posFijoTabla = "";
                    }
                    if (etiqueta.IndexOf("cuentaSub_") == 0) {
                        nivel = 2; idRegistroCuenta = etiqueta.Replace("cuentaSub_", ""); posFijoTabla = "Sub";
                    }
                    if (etiqueta.IndexOf("cuentaSubSub_") == 0) {
                        nivel = 3; idRegistroCuenta = etiqueta.Replace("cuentaSubSub_", ""); posFijoTabla = "SubSub";
                    }
                    if (etiqueta.IndexOf("cuentaSubSubSub_") == 0) {
                        nivel = 4; idRegistroCuenta = etiqueta.Replace("cuentaSubSubSub_", ""); posFijoTabla = "SubSubSub";
                    }
                    if (etiqueta.IndexOf("cuentaDetalle_") == 0) {
                        nivel = 5; idRegistroCuenta = etiqueta.Replace("cuentaDetalle_", ""); posFijoTabla = "Detalle";
                    }
                    if (nivel < 1 || nivel > 5) {
                        MessageBox.Show("Seleccione un nodo CUENTA del arbol.");
                    } else {
                        if (MessageBox.Show("Realmente desdea ELIMINAR la cuenta '" + idRegistroCuenta + "'?", "Renombrar cuenta", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                            //Aplicar la eliminacion
                            try {
                                OdbcCommand ssql = new OdbcCommand("DELETE FROM Cuentas" + posFijoTabla + " WHERE idCuenta" + posFijoTabla + "='" + idRegistroCuenta + "'", Global.conn);
                                ssql.ExecuteNonQuery();
                                nodoSel.Remove();
                            } catch (Exception excep) {
                                MessageBox.Show("No fue posible eliminar la cuenta. Posiblemente aun contenga registro hijos.\n\nExcepcion: '" + excep.Message + "'");
                            }
                        }
                    }
                }
            }
        }

        private void noMostrarToolStripMenuItem1_Click(object sender, EventArgs e) {
            _treeShowBalances = false;
            _treeShowBalancesSep = false;
            {
                string expandedNodesLst = this.getExpandedNodesList();
                //save state
                ClsAppState.AppStateRootServer sState = ClsAppState.getServerStateCurrent();
                if (sState != null) {
                    if (sState.frmContab == null) {
                        sState.frmContab = new ClsAppState.AppStateFrmContab();
                    }
                    sState.frmContab.showBalances = _treeShowBalances;
                    sState.frmContab.showBalancesSep = _treeShowBalancesSep;
                    sState.frmContab.treeExpandedLst = expandedNodesLst;
                    ClsAppState.setServerState(sState, null);
                }
                //update
                this.actualizaArbolCuentas(expandedNodesLst);
            }
        }

        private void mostrarCordobizadosToolStripMenuItem_Click(object sender, EventArgs e) {
            _treeShowBalances = true;
            _treeShowBalancesSep = false;
            {
                string expandedNodesLst = this.getExpandedNodesList();
                //save state
                ClsAppState.AppStateRootServer sState = ClsAppState.getServerStateCurrent();
                if (sState != null) {
                    if (sState.frmContab == null) {
                        sState.frmContab = new ClsAppState.AppStateFrmContab();
                    }
                    sState.frmContab.showBalances = _treeShowBalances;
                    sState.frmContab.showBalancesSep = _treeShowBalancesSep;
                    sState.frmContab.treeExpandedLst = expandedNodesLst;
                    ClsAppState.setServerState(sState, null);
                }
                //update
                this.actualizaArbolCuentas(expandedNodesLst);
            }
        }

        private void mostrarEnMonedasSeparadasToolStripMenuItem1_Click(object sender, EventArgs e) {
            _treeShowBalances = true;
            _treeShowBalancesSep = true;
            {
                string expandedNodesLst = this.getExpandedNodesList();
                //save state
                ClsAppState.AppStateRootServer sState = ClsAppState.getServerStateCurrent();
                if (sState != null) {
                    if (sState.frmContab == null) {
                        sState.frmContab = new ClsAppState.AppStateFrmContab();
                    }
                    sState.frmContab.showBalances = _treeShowBalances;
                    sState.frmContab.showBalancesSep = _treeShowBalancesSep;
                    sState.frmContab.treeExpandedLst = expandedNodesLst;
                    ClsAppState.setServerState(sState, null);
                }
                //update
                this.actualizaArbolCuentas(expandedNodesLst);
            }
        }

        private void actualizarToolStripMenuItem_Click(object sender, EventArgs e) {
            string expandedNodesLst = this.getExpandedNodesList();
            //save state
            ClsAppState.AppStateRootServer sState = ClsAppState.getServerStateCurrent();
            if (sState != null) {
                if (sState.frmContab == null) {
                    sState.frmContab = new ClsAppState.AppStateFrmContab();
                }
                sState.frmContab.showBalances = _treeShowBalances;
                sState.frmContab.showBalancesSep = _treeShowBalancesSep;
                sState.frmContab.treeExpandedLst = expandedNodesLst;
                ClsAppState.setServerState(sState, null);
            }
            //update tree
            this.actualizaArbolCuentas(expandedNodesLst);
        }

        
    }
}
