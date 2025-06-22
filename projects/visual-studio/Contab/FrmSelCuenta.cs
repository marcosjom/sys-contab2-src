using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.Odbc;    //para conexion a la BD

namespace Contab {
	public partial class FrmSelCuenta : Form {

		//Nodos claves del arbol
		private TreeNode nodoTodasLasCuentas = null;
		private string idCuentaDetalleSel = "";
		private string nombreCuentaDetalleSel = "";
        private bool autoseleccionArbolActiva = true;

		public FrmSelCuenta() {
			InitializeComponent();
        }

		private void FrmSelCuenta_Load(object sender, EventArgs e) {
            string treeExpandedLst = null;
            //load state
            {
                ClsAppState.AppStateRootServer sState = ClsAppState.getServerStateCurrent();
                if (sState != null && sState.frmAccSel != null) {
                    if (sState.frmAccSel.accSel != null && sState.frmAccSel.accSel.Length > 0) {
                        autoseleccionArbolActiva = false;
                        txtIdCuenta.Text = sState.frmAccSel.accSel;
                        autoseleccionArbolActiva = true;
                    }
                    if (sState.frmAccSel.treeExpandedLst != null && sState.frmAccSel.treeExpandedLst.Length > 0) {
                        treeExpandedLst = sState.frmAccSel.treeExpandedLst;
                    }
                }
            }
            actualizaArbolCuentas(treeExpandedLst);
		}

        public void seleccionaCuenta(string idCuenta) {
            autoseleccionArbolActiva = false;
            txtIdCuenta.Text = idCuenta;
            autoseleccionArbolActiva = true;
        }

		public string dameIdCuentaDetalleSel(){
			return idCuentaDetalleSel;
		}
		public string dameNombreCuentaDetalleSel() {
			return nombreCuentaDetalleSel;
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

        public string getExpandedNodesList() {
            string r = null;
            if (arbCuentas.Nodes != null) {
                r = this.getNodeExpandedSubnodesList(arbCuentas.Nodes);
            }
            return (r == null || r.Length <= 0 ? null : r);
        }

        private void actualizaArbolCuentas(string expandedNodesLst) {
            int expandedCount = 0;
            autoseleccionArbolActiva = false;
            string idCuentaSelActual = txtIdCuenta.Text;
            /*bool numCuentaEspecificado = false;
            int iNumCuenta = 0, iNumCuentaSub = 0, iNumCuentaSubSub = 0, iNumCuentaSubSubSub = 0, iNumCuentaDetalle = 0;
            if (idCuentaSelActual.Length==12){
                try  {
                    int idEnNumero1 = int.Parse(idCuentaSelActual.Substring(0, 6));
                    int idEnNumero2 = int.Parse(idCuentaSelActual.Substring(6, 6));
                    iNumCuenta = int.Parse(idCuentaSelActual.Substring(0, 2));
                    iNumCuentaSub = int.Parse(idCuentaSelActual.Substring(2, 2));
                    iNumCuentaSubSub = int.Parse(idCuentaSelActual.Substring(4, 2));
                    iNumCuentaSubSubSub = int.Parse(idCuentaSelActual.Substring(6, 2));
                    iNumCuentaDetalle = int.Parse(idCuentaSelActual.Substring(8, 4));
                    numCuentaEspecificado = true;
                    //MessageBox.Show("Cuenta int(" + iNumCuenta + " / " + iNumCuentaSub + " / " + iNumCuentaSubSub + " / " + iNumCuentaSubSubSub + " / " + iNumCuentaDetalle + " / )");
                } catch (Exception) {
                    MessageBox.Show("La cuenta indicada no es numerica: '" + idCuentaSelActual + "'");
                }
            } else if (idCuentaSelActual.Length!=0) {
                MessageBox.Show("La longitud de cuenta '" + idCuentaSelActual + "' no es valida " + idCuentaSelActual.Length + " de 12 digitos");
            }*/
            //
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
			strsql += " CuentasDetalle.idCuentaDetalle, CuentasDetalle.numCuentaDetalle, CuentasDetalle.nombreCuentaDetalle ";
			strsql += " FROM (((Cuentas LEFT JOIN CuentasSub ON Cuentas.idCuenta = CuentasSub.idCuenta) ";
			strsql += " LEFT JOIN CuentasSubSub ON CuentasSub.idCuentaSub = CuentasSubSub.idCuentaSub) ";
			strsql += " LEFT JOIN CuentasSubSubSub ON CuentasSubSub.idCuentaSubSub = CuentasSubSubSub.idCuentaSubSub) ";
			strsql += " LEFT JOIN CuentasDetalle ON CuentasSubSubSub.idCuentaSubSubSub = CuentasDetalle.idCuentaSubSubSub ";
            strsql += " ORDER BY Cuentas.numCuenta, CuentasSub.numCuentaSub, CuentasSubSub.numCuentaSubSub, CuentasSubSubSub.numCuentaSubSubSub, CuentasDetalle.numCuentaDetalle; ";
            TreeNode selNode = null, nodoCuenta = null, nodoCuentaSub = null, nodoCuentaSubSub = null, nodoCuentaSubSubSub = null, nodoCuentaDetalle = null;
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
                    nodoCuenta = nodoTodasLasCuentas.Nodes.Add(idCuenta, numCuenta + " - " + nombreCuenta, "folder.gif"); nodoCuenta.SelectedImageKey = nodoCuenta.ImageKey;
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
                        if (nodoCuentaSub != null && nodoCuentaSubExpand) nodoCuentaSub.Expand();
                        nodoCuentaSub = nodoCuenta.Nodes.Add(idCuentaSub, numCuentaSub + " - " + nombreCuentaSub, "folder.gif"); nodoCuentaSub.SelectedImageKey = nodoCuentaSub.ImageKey;
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
                        if (nodoCuentaSubSub != null && nodoCuentaSubSubExpand) nodoCuentaSubSub.Expand();
                        nodoCuentaSubSub = nodoCuentaSub.Nodes.Add(idCuentaSubSub, numCuentaSubSub + " - " + nombreCuentaSubSub, "folder.gif"); nodoCuentaSubSub.SelectedImageKey = nodoCuentaSubSub.ImageKey;
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
                        if (nodoCuentaSubSubSub != null && nodoCuentaSubSubSubExpand) nodoCuentaSubSubSub.Expand();
                        nodoCuentaSubSubSub = nodoCuentaSubSub.Nodes.Add(idCuentaSubSubSub, numCuentaSubSubSub + " - " + nombreCuentaSubSubSub, "folder.gif"); nodoCuentaSubSubSub.SelectedImageKey = nodoCuentaSubSubSub.ImageKey;
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
                        nodoCuentaDetalle = nodoCuentaSubSubSub.Nodes.Add(idCuentaDetalle, numCuentaDetalle + " - " + nombreCuentaDetalle, "folderDatos.gif"); nodoCuentaDetalle.SelectedImageKey = nodoCuentaDetalle.ImageKey;
                        nodoCuentaDetalle.Tag = "cuentaDetalle_" + idCuentaDetalle;
                        if (idCuentaSelActual != null && idCuentaSelActual == idCuentaDetalle) {
                            selNode = nodoCuentaDetalle;
                            nodoCuentaExpand = nodoCuentaSubExpand = nodoCuentaSubSubExpand = nodoCuentaSubSubSubExpand = true;
                        }
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
            if (selNode != null) arbCuentas.SelectedNode = selNode;
            //
            arbCuentas.EndUpdate(); //previene parpadeos
			arbCuentas.Refresh();
            autoseleccionArbolActiva = true;
		}

		private void btnCancelar_Click(object sender, EventArgs e) {
			idCuentaDetalleSel = "";
			nombreCuentaDetalleSel = "";
			this.Hide();
		}

		private void btnSeleccionar_Click(object sender, EventArgs e) {
            string strIdCuenta = txtIdCuenta.Text;
			string strsql = "SELECT idCuentaDetalle, nombreCuentaDetalle FROM CuentasDetalle WHERE CuentasDetalle.idCuentaDetalle='" + strIdCuenta + "';";
			OdbcCommand ssql = new OdbcCommand(strsql, Global.conn);
			OdbcDataReader conC = ssql.ExecuteReader();
            if(conC.Read()){
                idCuentaDetalleSel = conC.GetString(0);
				nombreCuentaDetalleSel = conC.GetString(1);
                this.Hide();
                //save state
                {
                    ClsAppState.AppStateRootServer sState = ClsAppState.getServerStateCurrent();
                    if (sState != null) {
                        if (sState.frmAccSel == null) {
                            sState.frmAccSel = new ClsAppState.AppStateFrmSelCuenta();
                        }
                        sState.frmAccSel.accSel = idCuentaDetalleSel;
                        sState.frmAccSel.treeExpandedLst = this.getExpandedNodesList();
                        ClsAppState.setServerState(sState, null);
                    }
                }
            } else {
                idCuentaDetalleSel = "";
                nombreCuentaDetalleSel = "";
                MessageBox.Show("Debe seleccionar una cuenta detalle existente.");
            }
		}

        private void arbCuentas_DoubleClick(object sender, EventArgs e) {
            if (txtIdCuenta.Text != null && txtIdCuenta.Text.Length > 0) {
                this.btnSeleccionar_Click(null, null);
            }
        }
        private void arbCuentas_KeyPress(object sender, KeyPressEventArgs e) {
            if (e != null && (e.KeyChar == 10 || e.KeyChar == 13) && txtIdCuenta.Text != null && txtIdCuenta.Text.Length > 0) {
                this.btnSeleccionar_Click(null, null);
            }
        }

        private void btnCrear_Click(object sender, EventArgs e) {
			MessageBox.Show("Esta funcion aun no esta implementada.");
		}

        private void arbCuentas_AfterSelect(object sender, TreeViewEventArgs e){
            autoseleccionArbolActiva = false;
            TreeNode nodoSel = e.Node;
			TreeNode nodoPrueba = nodoSel;
            int nivelSeleccion = 0;
			while (nodoPrueba.Parent != null) {
				nodoPrueba = nodoPrueba.Parent;
				nivelSeleccion++;
			}
			if (nivelSeleccion != 5) {
				txtIdCuenta.Text = "";
			} else {
                txtIdCuenta.Text = nodoSel.Name;
			}
            autoseleccionArbolActiva = true;
        }

        private void txtIdCuenta_TextChanged(object sender, EventArgs e){
            if (autoseleccionArbolActiva) { 
                string idCuentaSelActual = txtIdCuenta.Text;
                bool numCuentaEspecificado = false;
                int iNumCuenta = 0, iNumCuentaSub = 0, iNumCuentaSubSub = 0, iNumCuentaSubSubSub = 0, iNumCuentaDetalle = 0;
                if (idCuentaSelActual.Length==12){
                    try  {
                        int idEnNumero1 = int.Parse(idCuentaSelActual.Substring(0, 6));
                        int idEnNumero2 = int.Parse(idCuentaSelActual.Substring(6, 6));
                        iNumCuenta = int.Parse(idCuentaSelActual.Substring(0, 2));
                        iNumCuentaSub = int.Parse(idCuentaSelActual.Substring(2, 2));
                        iNumCuentaSubSub = int.Parse(idCuentaSelActual.Substring(4, 2));
                        iNumCuentaSubSubSub = int.Parse(idCuentaSelActual.Substring(6, 2));
                        iNumCuentaDetalle = int.Parse(idCuentaSelActual.Substring(8, 4));
                        numCuentaEspecificado = true;
                        //MessageBox.Show("Cuenta int(" + iNumCuenta + " / " + iNumCuentaSub + " / " + iNumCuentaSubSub + " / " + iNumCuentaSubSubSub + " / " + iNumCuentaDetalle + " / )");
                    } catch (Exception) {
                        //MessageBox.Show("La cuenta indicada no es numerica: '" + idCuentaSelActual + "'");
                    }
                } else if (idCuentaSelActual.Length!=0) {
                    //MessageBox.Show("La longitud de cuenta '" + idCuentaSelActual + "' no es valida " + idCuentaSelActual.Length + " de 12 digitos");
                }
                if (numCuentaEspecificado) {
                    /*TreeNode nodoCuentaExp = null, nodoCuentaSubExp = null, nodoCuentaSubSubExp = null, nodoCuentaSubSubSubExp = null, nodoCuentaDetalleExp = null;
                    int iCuenta;
                    for(iCuenta=0; iCuenta<arbCuentas.Nodes.Count; iCuenta++){
                        TreeNodeCollection nodosSub = arbCuentas.Nodes[iCuenta].;
                        nodosSub.
                        int iCuentaSub;
                        for(iCuentaSub=0; iCuentaSub<arbCuentas.Nodes.Count; iCuentaSub++){
                            TreeNodeCollection nodosSubSub = nodosSub..SubNodes[iCuenta].Nodes;
                            int iCuentaSubSub;
                            for(iCuentaSubSub=0; iCuentaSubSub<arbCuentas.Nodes.Count; iCuentaSubSub++){
                                TreeNodeCollection nodosSubSubSub = nodosSubSub.Nodes[iCuenta].Nodes;
                                int iCuentaSubSubSub;
                                for(iCuentaSubSubSub=0; iCuentaSubSubSub<arbCuentas.Nodes.Count; iCuentaSubSubSub++){
                                    TreeNodeCollection nodosSub = arbCuentas.Nodes[iCuenta].Nodes;
                                    int iCuentaDetalle;
                                    for(iCuentaDetalle=0; iCuentaDetalle<arbCuentas.Nodes.Count; iCuentaDetalle++){

                                    }
                                }
                            }
                        }
                    }
                    arbCuentas*/
                }
            }
        }
        
    }
}
