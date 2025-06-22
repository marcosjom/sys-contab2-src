/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;*/

using System.Collections;
using System.Windows.Forms;

namespace Contab {
	public class ClsOrdenadorDeLista : IComparer {
		private int indiceColumnaAOrdenar;
		private SortOrder tipoOrdenamiento;
		private CaseInsensitiveComparer comparadorDeTexto;

		public ClsOrdenadorDeLista() {
			indiceColumnaAOrdenar = 0;
			tipoOrdenamiento = SortOrder.None;
			comparadorDeTexto = new CaseInsensitiveComparer();
		}

		public bool esDecimal(string texto) {
			bool exito = false;
			if (texto.Length > 0) {
				exito = true;
				//revisar todos los caracteres (un signo menos, digitos y un punto)
				int posicion; bool puntoEncontrado = false;
				for (posicion = 0; posicion < texto.Length && exito; posicion++) {
					char caracter = texto[posicion];
					bool esDigito = (caracter == '0' || caracter == '1' || caracter == '2' || caracter == '3' || caracter == '4' || caracter == '5' || caracter == '6' || caracter == '7' || caracter == '8' || caracter == '9');
					exito = (
						esDigito //si es un digito
						|| (caracter == '.' && !puntoEncontrado) //si es el primer punto
						|| (caracter == '-' && posicion == 0) //si es un signo menos al principio
					);
					if (caracter == '.') {
						puntoEncontrado = true;
					}
				}
			}
			return exito;
		}

		public int Compare(object x, object y) {
			int resultadoComparacion = 0;
			if (tipoOrdenamiento != SortOrder.None) {
				ListViewItem itemX = (ListViewItem)x;
				ListViewItem itemY = (ListViewItem)y;
				string textoX = itemX.SubItems[indiceColumnaAOrdenar].Text;
				string textoY = itemY.SubItems[indiceColumnaAOrdenar].Text;
				bool xEsDecimal = esDecimal(textoX);
				bool yEsDecimal = esDecimal(textoY);
				if (xEsDecimal && yEsDecimal) {
					//Comparar como numero
					decimal diferencia = decimal.Parse(textoY) - decimal.Parse(textoX);
					if (diferencia < 0) {
						resultadoComparacion = -1;
					} else if (diferencia > 0) {
						resultadoComparacion = 1;
					}
				} else {
					//comparar como texto
					resultadoComparacion = comparadorDeTexto.Compare(textoX, textoY);
				}
				//Invertir, si el orden esta invertido
				if (tipoOrdenamiento == SortOrder.Descending) {
					resultadoComparacion = (-resultadoComparacion);
				}
			}
			return resultadoComparacion;
		}

		public int indiceColumnaOrdenamiento {
			set {
				indiceColumnaAOrdenar = value;
			}
			get {
				return indiceColumnaAOrdenar;
			}
		}

		public SortOrder tipoDeOrdenamiento {
			set {
				tipoOrdenamiento = value;
			}
			get {
				return tipoOrdenamiento;
			}
		}
	}
}
