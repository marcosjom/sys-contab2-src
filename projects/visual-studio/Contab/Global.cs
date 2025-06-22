using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;               //para escribir en archivo
using System.Data.Odbc;
using System.Net;    //para conexion a la BD
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Net.Http;
using System.Threading.Tasks;

namespace Contab {
    public static class Global {
		//
		public const double versionEstaApp = 0.01;
		//tunnel
		public static ClsTunnel tunnel = null;
		public static string tunnelLysrList = null;
		public static int tunnelPort = 0;
		public static bool tunnelDllFound = false;
        //conn
		public static OdbcConnection conn = null;
		public static string connDb = "";
        public static string connUser = "";
		//

		public static string[] monthsNames = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

		public static Int64 convertCents(Int64 orgCents100, Int64 rateTenmillis10000) {
			return (orgCents100 * rateTenmillis10000) / 10000;
		}
        
        //

        public static string rellenaEntero(int entero, int digitos) {
			string str = entero.ToString();
			while (str.Length < digitos) {
				str = '0' + str;
			}
			return str;
		}
		//
		public static string fechaSQL(DateTime fecha) {
			string str = fecha.Year.ToString() + "-" + Global.rellenaEntero(fecha.Month, 2) + "-" + Global.rellenaEntero(fecha.Day, 2);
			return str;
		}

		public static void exportarListaXLS(ListView lista) {
			string rutaBaseArchivo = Directory.GetCurrentDirectory() + "\\tmp\\exportar";
			int numeroArchivo = 0;
			string rutaArchivo = "";
			do {
				numeroArchivo++;
				rutaArchivo = rutaBaseArchivo + numeroArchivo + ".xls";
			} while (System.IO.File.Exists(rutaArchivo));
			TextWriter archivo = new StreamWriter(rutaArchivo);
			string tituloInformacion = "";
			char separadorColumnas = '\t';
			archivo.WriteLine(tituloInformacion);
			archivo.WriteLine("");
			//Columnas
			string linea = "";
			int c;
			for (c = 0; c < lista.Columns.Count; c++) {
				if (linea != "") linea += separadorColumnas;
				linea += lista.Columns[c].Text.Replace(separadorColumnas, ' ');
			}
			archivo.WriteLine(linea);
			//Filas
			int f; ListViewGroup ultimoGrupo = null;
			for (f = 0; f < lista.Items.Count; f++) {
				ListViewGroup esteGrupo = lista.Items[f].Group;
				if (ultimoGrupo!=esteGrupo) {
					if (esteGrupo != null) { 
						archivo.WriteLine(esteGrupo.Header);
					}
					ultimoGrupo = esteGrupo;
				}
				linea = ""; //lstTrabajos.Items[f].Text.Replace(separadorColumnas, ' ');
				for (c = 0; c < lista.Items[f].SubItems.Count; c++) {
					if (linea != "") linea += separadorColumnas;
					string contenido = lista.Items[f].SubItems[c].Text.Replace(separadorColumnas, ' ');
					contenido = contenido.Replace("U$", "");
					linea += contenido;
				}
				archivo.WriteLine(linea);
			}
			archivo.Close();
			using (System.Diagnostics.Process prc = new System.Diagnostics.Process()) {
				prc.StartInfo.FileName = rutaArchivo;
				prc.Start();
			}
		}
		//Comprobantes (nuevos)
		public static string dameNumeroCorrespondeSiguienteComprobante(DateTime fechaComprob) {
			int numNuevoComprobante = 1;
			OdbcCommand ssql = null;
			OdbcDataReader conC = null;
            {
				ssql = new OdbcCommand("SELECT MAX(numeroComprobante) FROM ComprobantesDeDiario WHERE anoComprobanteDiario=" + fechaComprob.Year + " AND mesComprobanteDiario=" + fechaComprob.Month + " ", Global.conn);
				conC = ssql.ExecuteReader();
				conC.Read();
				if (!conC.IsDBNull(0)) {
					numNuevoComprobante = conC.GetInt32(0) + 1;
				}
				conC.Close();
			}
			return fechaComprob.Year.ToString() + "-" + Global.rellenaEntero(fechaComprob.Month, 2) + "-" + Global.rellenaEntero(fechaComprob.Day, 2) + "-" + Global.rellenaEntero(numNuevoComprobante, 4);
		}

		public static string generaSiguienteComprobante(DateTime fechaComprob) {
            int numNuevoComprobante = 1; string idComprobante = null;
            OdbcCommand ssql = null;
			OdbcDataReader conC = null;
            {
				ssql = new OdbcCommand("SELECT MAX(numeroComprobante) FROM ComprobantesDeDiario WHERE anoComprobanteDiario=" + fechaComprob.Year + " AND mesComprobanteDiario=" + fechaComprob.Month + " ", Global.conn);
				conC = ssql.ExecuteReader();
				conC.Read();
				if (!conC.IsDBNull(0)) {
					numNuevoComprobante = conC.GetInt32(0) + 1;
				}
				conC.Close();
			}
			{
				idComprobante = fechaComprob.Year.ToString() + "-" + Global.rellenaEntero(fechaComprob.Month, 2) + "-" + Global.rellenaEntero(fechaComprob.Day, 2) + "-" + Global.rellenaEntero(numNuevoComprobante, 4);
				ssql = new OdbcCommand("INSERT INTO ComprobantesDeDiario(idComprobanteDiario, fechaComprobanteDiario, anoComprobanteDiario, mesComprobanteDiario, numeroComprobante) VALUES('" + idComprobante + "', '" + Global.fechaSQL(fechaComprob) + "', " + fechaComprob.Year + ", " + fechaComprob.Month + ", " + numNuevoComprobante + ") ", Global.conn);
				ssql.ExecuteNonQuery();
			}
			return idComprobante;
		}

        public static bool actualizarBalanceDeCuentas(string accountsList) { //empty list will updated all accounts, non empty expected as: '...', '...'
			bool r = false;
			OdbcCommand ssql = null;
            string str = "";
			str += "UPDATE CuentasDetalle d SET ";
			//
			str += "balanceDebe = IFNULL((SELECT SUM(montoDebeCS) FROM Movimientos m WHERE m.idCuentaDetalle=d.idCuentaDetalle), 0) ";
            str += ", balanceHaber = IFNULL((SELECT SUM(montoHaberCS) FROM Movimientos m WHERE m.idCuentaDetalle=d.idCuentaDetalle), 0) ";
            //
            str += ", balanceDebeSoloCS = IFNULL((SELECT SUM(montoDebeCS) FROM Movimientos m WHERE m.idCuentaDetalle=d.idCuentaDetalle AND esDolares=0), 0) ";
            str += ", balanceHaberSoloCS = IFNULL((SELECT SUM(montoHaberCS) FROM Movimientos m WHERE m.idCuentaDetalle=d.idCuentaDetalle AND esDolares=0), 0) ";
            //
            str += ", balanceDebeSoloUS = IFNULL((SELECT SUM(montoDebeUS) FROM Movimientos m WHERE m.idCuentaDetalle=d.idCuentaDetalle AND esDolares<>0), 0) ";
            str += ", balanceHaberSoloUS = IFNULL((SELECT SUM(montoHaberUS) FROM Movimientos m WHERE m.idCuentaDetalle=d.idCuentaDetalle AND esDolares<>0), 0) ";
			//
			if (accountsList != null && accountsList.Length > 0) {
                str += " WHERE d.idCuentaDetalle IN (" + accountsList + ")";
            }
			//
			{
				ssql = new OdbcCommand(str, Global.conn);
				ssql.ExecuteNonQuery();
				r = true;
			}
			//
			return r;
        }

        public static async Task<double> queryTasaCambio_bcn_gob_niAsync(DateTime fecha) {
			double r = -1;
			string bodyStr = "";
            bodyStr += "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n";
            bodyStr += "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">\n";
            bodyStr += "  <soap12:Body>\n";
            bodyStr += "    <RecuperaTC_Dia xmlns=\"http://servicios.bcn.gob.ni/\">\n";
            bodyStr += "      <Ano>" + fecha.Year + "</Ano>\n";
            bodyStr += "      <Mes>" + fecha.Month + "</Mes>\n";
            bodyStr += "      <Dia>" + fecha.Day + "</Dia>\n";
            bodyStr += "    </RecuperaTC_Dia>\n";
            bodyStr += "  </soap12:Body>\n";
            bodyStr += "</soap12:Envelope>\n";
            /*
            POST /Tc_Servicio/ServicioTC.asmx HTTP/1.1
			Host: servicios.bcn.gob.ni
			Content-Type: application/soap+xml; charset=utf-8
			Content-Length: length
			*/
            //MessageBox.Show("Enviando: " + bodyStr);
            try {
				HttpClient client = new HttpClient();
                HttpContent content = new StringContent(bodyStr, Encoding.UTF8, "application/soap+xml");
                //MessageBox.Show("Posting");
                Task<HttpResponseMessage> resp = client.PostAsync("https://servicios.bcn.gob.ni/Tc_Servicio/ServicioTC.asmx", content);
				resp.Wait();
                //MessageBox.Show("Reading string");
                Task<string> respBody = resp.Result.Content.ReadAsStringAsync();
				respBody.Wait();
                //MessageBox.Show(respBody.Result);
                {
                    string respXml = respBody.Result;
					if (respXml != null && respXml.Length > 0) {
						string preStr = "<RecuperaTC_DiaResult>";
                        string postStr = "</RecuperaTC_DiaResult>";
						int prePos = respXml.IndexOf(preStr);
						if (prePos >= 0) {
                            int postPos = respXml.IndexOf(postStr, prePos + preStr.Length);
							if (postPos > 0) {
								string tasaStr = respXml.Substring(prePos + preStr.Length, postPos - prePos - preStr.Length);
								//MessageBox.Show("'" + tasaStr +  "'");
								r = double.Parse(tasaStr);
                            }
                        }
                    }
				}
			} catch (Exception) { 
				//
			}
			return r;
			/*
			double r = -1;
			string html = string.Empty;
			string dateUrl = fecha.Year.ToString();
			dateUrl += "-";
			if (fecha.Month < 10) dateUrl += "0"; dateUrl += fecha.Month;
			dateUrl += "-";
			if (fecha.Day < 10) dateUrl += "0"; dateUrl += fecha.Day;
			string url = "http://www.bcn.gob.ni/estadisticas/mercados_cambiarios/tipo_cambio/cordoba_dolar/mes.php?Fecha_inicial=" + dateUrl + "&Fecha_final=" + dateUrl;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.AutomaticDecompression = DecompressionMethods.GZip;
			try {
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream)) {
					html = reader.ReadToEnd();
					//MessageBox.Show(html.Replace('\n', ' ').Replace('\r', ' '));
					int iPos = html.LastIndexOf("-" + fecha.Year);
					if (iPos > 0) {
						//Read all the content between "<" and ">"
						string strAcum = "";
						int iOpen = html.IndexOf('<', iPos + 1);
						if (iOpen > iPos) {
							do {
								int iClose = html.IndexOf('>', iOpen + 1);
								if (iClose <= iOpen) {
									break;
								} else {
									iOpen = html.IndexOf('<', iClose + 1);
									if (iOpen <= iClose) {
										break;
									} else {
										string content = html.Substring(iClose + 1, iOpen - iClose - 1).Trim();
										if (content.Length >= 7) { //00.0000
											try {
												double v = double.Parse(content);
												if (v >= 20) {
													r = v;
													break;
												}
											} catch (Exception) {
												//
											}
										}
									}
								}
							} while (true);
						}
					}
				}
			} catch (Exception) { 
				//
			}
			return r;*/
        }
    }
}
