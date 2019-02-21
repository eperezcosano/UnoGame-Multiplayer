#include <mysql.h>
#include <string.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <pthread.h>
#include <math.h>
#include <time.h>

// #include <my_global.h>
#define port 9050
#define mysql_dire "localhost"
#define mysql_user "root"
#define mysql_pass "mysql"
#define mysql_ddbb "Juego"

#define NELEMS(x) (sizeof(x) / sizeof (x[0]))
#define maxConectados 100
#define maxPartidas 50
#define maxMano 50

//ListaConectados
typedef struct {
	char nombre[20];
	int socket;
	int mano[maxMano];	//Vector de numeros
	int numCartas;	//Numero cartas en la mano
	int confirm;	//0: No, 1: Si
	int robado;		//0: No puedes robara, 1: Si puede
	int porRobar;   //Cartas que quedan por robar
	int escabullirse; //Poder acumular las cartas por robar a otro jugador
	int uno;		//1: Ya ha cantado uno
} Tconectado;

typedef struct {
	Tconectado conectados[maxConectados];
	int num;
} Tlista;

//TablaPartidas
typedef struct {
	int lleno;
	int mazo[108];
	int cartaEnMesa;
	char colorEnMesa[20];
	int sentido;
	int turno;
	int numJ;
	Tconectado jugadores[4];
} TPartida;

typedef TPartida TTablaPartidas[maxPartidas];

//Variables Globales
MYSQL *conn;
pthread_mutex_t mutex = PTHREAD_MUTEX_INITIALIZER;
Tlista miLista;
TTablaPartidas miTabla;
int cartas[108];

//Dado un vector ordenado por numeros consecutivos
//mezcla el orden de estos numeros
void shuffle(int arr[], int n)
{
	for(int i = n - 1; i > 0; i--)
	{
		int j = rand() % (i + 1);
		int temp = arr[i];
		arr[i] = arr[j];
		arr[j] = temp;
	}
}

//Devuelve el numero del jugador (0-3) a partir de su socket,
//el id partida y la tabla. -1: No encontrado.
int DameNumeroJugador(int socket, int id, TTablaPartidas *tabla)
{
	for (int i = 0; i < tabla[id]->numJ; i++)
	{
		if (tabla[id]->jugadores[i].socket == socket)
			return i;
	}
	return -1;
}

//Obtiene el color de una carta
void DameColor(int carta, char color[20])
{
	if (carta % 14 == 13)
		strcpy(color, "negro");
	else
	{
		switch((int)floor(carta / 14))
		{
			case 0:
			case 4:
				strcpy(color, "rojo");
				break;

			case 1:
			case 5:
				strcpy(color, "amarillo");
				break;
			case 2:
			case 6:
				strcpy(color, "verde");
				break;
			case 3:
			case 7:
				strcpy(color, "azul");
				break;
		}
	}
}

//Obtiene el numero/tipo carta
//Prohibido: 		10
//Cambio Sentido: 	11
//+2: 				12
//Cambio Color: 	13
//+4: 				14
int DameNumero(int carta)
{
	if (carta % 14 == 13)
	{
		if ((int)floor(carta / 14) >= 0 && (int)floor(carta / 14) <= 3)
			return 13;
		if ((int)floor(carta / 14) >= 4 && (int)floor(carta / 14) <= 7)
			return 14;
	}
	return carta % 14;
}

//Obtiene la primera carta del mazo, la devuelve y
//la coloca al final de este
int RobarCarta(int deck[108])
{
	int carta = deck[0];
	for (int i = 0; i < 107; i++)
		deck[i] = deck[i+1];
	deck[107] = carta;
	return carta;
}


//Reparte un numero de cartas a la mano de un jugador
void RepartirCartas(int idJ, int idP, int cantidad, TTablaPartidas *tabla, char repartidas[100])
{
	for (int i = 0; i < cantidad; i++)
	{
		int c = RobarCarta(tabla[idP]->mazo);
		if (strlen(repartidas) == 0)
			sprintf(repartidas, "%d", c);
		else
			sprintf(repartidas, "%s,%d", repartidas, c);
		tabla[idP]->jugadores[idJ].mano[tabla[idP]->jugadores[idJ].numCartas] = c;
		tabla[idP]->jugadores[idJ].numCartas++;
	}
}

//Dado un nombre busca socket en la listaConectados
int DameSocket(char nombre[20], Tlista *lista)
{
	int i = 0;
	int encontrado = 0;

	while (i < lista->num && encontrado == 0)
	{
		if (strcmp(lista->conectados[i].nombre, nombre) == 0)
			encontrado = 1;
		else
			i++;
	}

	if (encontrado == 1)
		return lista->conectados[i].socket;
	else
		return -1;
}

//Agrega un jugador a la lista de conectados
int PonJugador (char nombre[20], int socket, Tlista *lista)
{
	if (lista->num == 100)
		return -1;
	else
	{
		strcpy (lista->conectados[lista->num].nombre, nombre);
		lista->conectados[lista->num].socket = socket;
		lista->num++;
		return 0;
	}
}

//Dado un socket, devuelve su posicion en la lista
int DamePosicion (int socket, Tlista *lista)
{
	int i = 0;
	int encontrado = 0;

	while (i < lista->num && encontrado == 0)
	{
		if (lista->conectados[i].socket == socket)
			encontrado = 1;
		else
			i++;
	}

	if (encontrado == 1)
		return i;
	else
		return -1;
}

//Dado un  socket busca nombre en la listaConectados
int DameNombre(int socket, Tlista *lista, char nombre[20])
{
	int p = DamePosicion (socket, lista);
	if (p == -1)
		return -1;
	else
		strcpy(nombre, lista->conectados[p].nombre);
}

//Elimina un jugador de la lista de conectados
int QuitarJugador (int socket, Tlista *lista)
{
	int p = DamePosicion (socket, lista);
	if (p == -1)
		return -1;
	else
	{
		printf("%s se ha desconectado\n", lista->conectados[p].nombre);
		while (p < lista->num-1)
		{
			strcpy (lista->conectados[p].nombre, lista->conectados[p+1].nombre);
			lista->conectados[p].socket = lista->conectados[p+1].socket;
			p++;
		}
		lista->num--;
		return 0;
	}
}

//Devuelve la lista de conectados de la forma:
//	6/numeroConectados/Jugador1,Jugador2,etcetera.
void DameConectados (Tlista *lista, char listaConectados[512])
{
	sprintf(listaConectados, "6/%d", lista->num);
	strcat(listaConectados, ",");

	for (int i=0; i<lista->num; i++)
	{
		strcat(listaConectados, lista->conectados[i].nombre);
		strcat(listaConectados, ",");
	}

	listaConectados[strlen(listaConectados) - 1] = '\0';
}

//Consulta para loguearse, recibe nombre y contrasena
//Devuelve 1/0: OK, 1/-1: No encontrado, 1/-2:Error BBDD
void Entrar(char username[20], char password[20], char respuesta[512])
{
	//Consulta
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;

	strcpy(consulta, "SELECT COUNT(Jugador.Username) FROM Jugador WHERE BINARY Jugador.Username = '");
	strcat(consulta, username);
	strcat(consulta, "' AND Jugador.Password = '");
	strcat(consulta, password);
	strcat(consulta, "'");

	err = mysql_query(conn, consulta);
	if (err != 0)
	{
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		strcpy(respuesta, "1/-2");
		return;
	}

	res = mysql_store_result(conn);
	row = mysql_fetch_row(res);

	if (row == NULL)
	{
		printf("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "1/-1");
		return;
	}

	strcpy(respuesta, "1/");
	strcat(respuesta, row[0]);

	if(strcmp(row[0], "1") == 0)
	{
		strcpy(respuesta, "1/0");
		printf("%s se ha conectado\n", username);
	}
	else
	{
		strcpy(respuesta, "1/-1");
		printf("Error: se han encontrado mas de un usuario\n");
	}
}

//Consulta para registrarse, recibe nombre y contrasena
//Devuelve 2/0: OK, 2/1: Ya estaba registrado, 2/-1: Error al crear cuenta, 2/-2:Error BBDD
void Registrar(char username[20], char password[20], char respuesta[512])
{
	//Consulta
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;

	strcpy(consulta, "SELECT Jugador.Username FROM Jugador WHERE Jugador.Username = '");
	strcat(consulta, username);
	strcat(consulta, "'");

	err = mysql_query(conn, consulta);
	if (err != 0)
	{
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		strcpy(respuesta, "2/-2");
		return;
	}

	res = mysql_store_result(conn);
	row = mysql_fetch_row(res);

	if (row == NULL)
	{
		char registro[512];

		strcpy(registro, "INSERT INTO Jugador VALUES ('");
		strcat(registro, username);
		strcat(registro, "','");
		strcat(registro, password);
		strcat(registro, "')");

		err = mysql_query(conn, registro);
		if (err != 0)
		{
			printf("Error al insertar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
			strcpy(respuesta, "2/-1");
		}
		else
		{
			printf("%s se ha registrado\n", username);
			strcpy(respuesta, "2/0");
		}
	}
	else
	{
		printf("%s ya se encontraba registrado\n", username);
		strcpy(respuesta, "2/1");
	}
}

//Consulta para saber el jugador con max puntuacion
//Devuelve 3/nombre: OK, 3/-1: No encontrado, 2/-2:Error BBDD
void Consulta1 (char respuesta[512])
{
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;

	strcpy(consulta, "SELECT Jugador.Username FROM Jugador, Relacion WHERE Relacion.Puntuacion = (SELECT MAX(Relacion.Puntuacion) FROM Relacion) AND Jugador.Username = Relacion.Username");

	err = mysql_query(conn, consulta);
	if (err != 0)
	{
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		strcpy(respuesta, "3/-2");
		return;
	}

	res = mysql_store_result(conn);
	row = mysql_fetch_row(res);

	if (row[0] == NULL)
	{
		printf("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "3/-1");
	}
	else
	{
		printf("Max Puntuacion: %s\n", row[0]);
		strcpy(respuesta, "3/");
		strcat(respuesta, row[0]);
	}
}

//Consulta para saber la maxima duracion de una partida dado un jugador
//Devuelve 4/duracion: OK, 4/-1: No encontrado, 4/-2:Error BBDD
void Consulta2 (char username[20], char respuesta[512])
{
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;

	strcpy(consulta, "SELECT MAX(Partida.Duracion) FROM Jugador, Partida, Relacion WHERE Jugador.Username = '");
	strcat(consulta, username);
	strcat(consulta, "' AND Partida.Id = Relacion.IdP AND Jugador.Username = Relacion.Username");

	err = mysql_query(conn, consulta);
	if (err != 0)
	{
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		strcpy(respuesta, "4/-2");
		return;
	}

	res = mysql_store_result(conn);
	row = mysql_fetch_row(res);

	if (row[0] == NULL)
	{
		printf("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "4/-1");
	}
	else
	{
		printf("Max Duracion: %s\n", row[0]);
		strcpy(respuesta, "4/");
		strcat(respuesta, row[0]);
	}
}

//Consulta para saber los jugadores que han jugado dada la id de partida
//Devuelve 5/nombre1, nombre2, etcetera: OK, 5/-1: No encontrado, 5/-2:Error BBDD
void Consulta3 (int id, char respuesta[512])
{
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;

	strcpy(consulta, "SELECT Jugador.Username FROM Jugador, Partida, Relacion WHERE Partida.Id = ");
	sprintf(consulta, "%s%d", consulta, id);
	strcat(consulta, " AND Partida.Id = Relacion.IdP AND Jugador.Username = Relacion.Username");

	err = mysql_query(conn, consulta);
	if (err != 0)
	{
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		strcpy(respuesta, "5/-2");
		return;
	}

	res = mysql_store_result(conn);
	row = mysql_fetch_row(res);

	if (row == NULL)
	{
		printf("No se han obtenido datos en la consulta\n");
		strcpy(respuesta, "5/-1");
	}
	else
	{
		strcpy(respuesta, "5/");
		strcat(respuesta, row[0]);
		row = mysql_fetch_row(res);

		while (row != NULL)
		{
			strcat(respuesta, ", ");
			strcat(respuesta, row[0]);
			row = mysql_fetch_row(res);
		}
		printf("Participantes: %s\n", respuesta);
	}
}


void ConsultaFinal (TTablaPartidas *tabla, int id, int duracion)
{
	char ganador[20];
	for (int i = 0; i < tabla[id]->numJ; i++)
	{
		if (tabla[id]->jugadores[i].numCartas == 0)
		{
			strcpy(ganador, tabla[id]->jugadores[i].nombre);
			break;
		}
	}
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;

	strcpy(consulta, "INSERT INTO Partida (Id, Duracion, Ganador) VALUES (");
	sprintf(consulta, "%s%d", consulta, id);
	strcat(consulta, ", ");
	sprintf(consulta, "%s%d", consulta, duracion);
	strcat(consulta, ", ");
	strcat(consulta, ganador);
	strcat(consulta, ")");
	mysql_query(conn, consulta);

	for (int i = 0; i < tabla[id]->numJ; i++)
	{
		int puntuacion = 100 - tabla[id]->jugadores[i].numCartas;
		if (puntuacion < 0)
			puntuacion = 0;
		strcpy(consulta, "INSERT INTO Relacion (Username, IdP, Puntuacion) VALUES (");
		strcat(consulta, tabla[id]->jugadores[i].nombre);
		strcat(consulta, ", ");
		sprintf(consulta, "%s%d", consulta, id);
		strcat(consulta, ", ");
		sprintf(consulta, "%s%d", consulta, puntuacion);
		strcat(consulta, ")");
		mysql_query(conn, consulta);
	}

}
//Consulta para darse de baja
//Devuelve 7/0: OK, 7/-2:Error BBDD
void DarseDeBaja (char username[20], char respuesta[512])
{
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;

	sprintf(consulta, "DELETE FROM Jugador WHERE Jugador.Username = '%s'", username);

	err = mysql_query(conn, consulta);
	if (err == 0)
	{
		printf("Usuario eliminado\n");
		strcpy(respuesta, "7/0");
		return;
	}
	else
	{
		printf("Error al consultar datos de la base %u %s\n", mysql_errno(conn), mysql_error(conn));
		strcpy(respuesta, "7/-2");
		return;
	}

}
//Dado un nombre, crea una nueva partida en la Tabla de Partidas
//Devuelve i (posicion de la tabla): Insertado, -1: No hay hueco
int InsertarPartida(char jugador1[20], char invitados[100], TTablaPartidas *tabla)
{
	int cont = 1;
	char jugador2[20];
	char jugador3[20];
	char jugador4[20];
	char tmpinvitados[100];
	strcpy(tmpinvitados, invitados);
	char *v = strtok(tmpinvitados, ",");

	//Recogemos nombres de los invitados
	if (v != NULL)
	{
		cont++;
		strcpy(jugador2, v);
		v = strtok(NULL, ",");
	}
	if (v != NULL)
	{
		cont++;
		strcpy(jugador3, v);
		v = strtok(NULL, ",");
	}
	if (v != NULL)
	{
		cont++;
		strcpy(jugador4, v);
	}

	//Buscamos un hueco en la tabla
	for (int i = 0; i < maxPartidas; i++)
	{
		if (tabla[i]->lleno == 0)
		{
			//Insertamos datos de los jugadores
			strcpy(tabla[i]->jugadores[0].nombre, jugador1);
			tabla[i]->jugadores[0].socket = DameSocket(jugador1, &miLista);
			tabla[i]->jugadores[0].numCartas = 0;
			tabla[i]->jugadores[0].confirm = 1;
			tabla[i]->jugadores[0].robado = 1;
			tabla[i]->jugadores[0].porRobar = 0;
			tabla[i]->jugadores[0].escabullirse = 0;
			tabla[i]->jugadores[0].uno = 0;
			if (cont > 1)
			{
				strcpy(tabla[i]->jugadores[1].nombre, jugador2);
				tabla[i]->jugadores[1].socket = DameSocket(jugador2, &miLista);
				tabla[i]->jugadores[1].numCartas = 0;
				tabla[i]->jugadores[1].robado = 1;
				tabla[i]->jugadores[1].porRobar = 0;
				tabla[i]->jugadores[1].escabullirse = 0;
				tabla[i]->jugadores[1].uno = 0;
			}
			if (cont > 2)
			{
				strcpy(tabla[i]->jugadores[2].nombre, jugador3);
				tabla[i]->jugadores[2].socket = DameSocket(jugador3, &miLista);
				tabla[i]->jugadores[2].numCartas = 0;
				tabla[i]->jugadores[2].robado = 1;
				tabla[i]->jugadores[2].porRobar = 0;
				tabla[i]->jugadores[2].escabullirse = 0;
				tabla[i]->jugadores[2].uno = 0;
			}
			if (cont > 3)
			{
				strcpy(tabla[i]->jugadores[3].nombre, jugador4);
				tabla[i]->jugadores[3].socket = DameSocket(jugador4, &miLista);
				tabla[i]->jugadores[3].numCartas = 0;
				tabla[i]->jugadores[3].robado = 1;
				tabla[i]->jugadores[3].porRobar = 0;
				tabla[i]->jugadores[3].escabullirse = 0;
				tabla[i]->jugadores[3].uno = 0;
			}

			//Insertamos datos de la partida
			tabla[i]->lleno = 1;
			tabla[i]->numJ = cont;


			return i;
		}
	}
	return -1;
}


//Marca al jugador de la partida conforme ha aceptado la invitacion
//Ademas comprueba si todos ya han aceptado.
//Devuelve 0: Todavia faltan por aceptar, 1: Ya han aceptado todos
int AceptarJugador(int id, int socket, TTablaPartidas *tabla)
{
	int idJ = DameNumeroJugador(socket, id, tabla);
	printf("%d\n", idJ);
	tabla[id]->jugadores[idJ].confirm = 1;

	for (int i = 0; i < tabla[id]->numJ; i++)
	{
		if (tabla[id]->jugadores[i].confirm == 0)
			return 0;
	}
	return 1;
}
//Funcion de atender las peticiones de un cliente
//dado un socket y ejecutado por un thread
void *AtenderCliente (void *socket)
{
	int sock_conn = * (int *) socket;;
	char peticion [512];
	char respuesta[512];
	int tam;

	for (;;) {
		tam = read(sock_conn, peticion, sizeof(peticion));
		peticion[tam] = '\0';
		printf (">> %s\n", peticion);

		char *p = strtok(peticion, "/");
		int codigo = atoi(p);

		if (codigo == 0) //Desconectar cliente
		{
			pthread_mutex_lock(&mutex);
			int err = QuitarJugador(sock_conn, &miLista);
			pthread_mutex_unlock(&mutex);
			if (err == 0)
				printf("Eliminado de la lista conectados\n");
			else
				printf("ERROR al eliminar de la lista conectados\n");
		}
		if (codigo == 1) //Entrar
		{
			char username[20];
			char password[20];

			p = strtok(NULL, ",");
			strcpy (username, p);
			p = strtok(NULL,",");
			strcpy (password, p);

			Entrar(username, password, respuesta);

			if (strcmp(respuesta, "1/0") == 0)
			{
				pthread_mutex_lock(&mutex);
				int err = PonJugador(username, sock_conn, &miLista);
				pthread_mutex_unlock(&mutex);

				if (err == -1)
					printf("Error: lista llena\n");
				if (err == 0)
					printf("Agredado a la lista conectados\n");
			}
		}
		if (codigo == 2) //Registrarse
		{
			char username[20];
			char password[20];

			p = strtok(NULL, ",");
			strcpy (username, p);
			p = strtok(NULL,",");
			strcpy (password, p);

			Registrar(username, password, respuesta);
		}
		if (codigo == 3) //Consulta #1
		{
			Consulta1(respuesta);
		}
		if (codigo == 4) //Consulta #2
		{
			char username[20];

			p = strtok(NULL, ",");
			strcpy(username, p);

			Consulta2(username, respuesta);
		}
		if (codigo == 5) //Consulta #3
		{
			int id;

			p = strtok(NULL, ",");
			id = atoi(p);

			Consulta3(id, respuesta);
		}

		if (codigo == 7) //Darse de Baja
		{
			char username[20];
			DameNombre(sock_conn, &miLista, username);
			DarseDeBaja(username, respuesta);
		}
		if (codigo == 10) //Solicitud de Invitacion (1)
		{
			char invitados[100];
			p = strtok(NULL, "/");
			strcpy(invitados, p);

			char nombre[20];
			DameNombre(sock_conn, &miLista, nombre);

			//Buscar un hueco en la tabla e insertar datos (2)
			pthread_mutex_lock(&mutex);
			int pos = InsertarPartida(nombre, invitados, &miTabla);
			pthread_mutex_unlock(&mutex);

			if (pos == -1)
				printf("Error: No hay partidas disponibles\n");
			else
			{
				printf("Partida agregada a la tabla con id: %d\n", pos);
				//Enviar a la invitacion a los destinatarios (3)
				char invitacion[100];


				sprintf(invitacion, "10/%d,%s,%s", pos, nombre, invitados);
				printf("<< %s\n", invitacion);
				for (int i = 0; i < miTabla[pos].numJ; i++)
				{
					if (miTabla[pos].jugadores[i].socket != sock_conn)
						write(miTabla[pos].jugadores[i].socket, invitacion, strlen(invitacion));
				}
				printf("Invitaciones enviadas. Esperando confirmaciones...\n");
			}
		}
		if (codigo == 11) //Respuesta a la Invitacion (4)
		{
			int id;
			p = strtok(NULL, "/");
			id = atoi(p);

			int confirm;
			p = strtok(NULL, "/");
			confirm = atoi(p);

			char nombre[20];
			DameNombre(sock_conn, &miLista, nombre);

			if (confirm == 1) //Aceptado (5)
			{
				pthread_mutex_lock(&mutex);
				int a = AceptarJugador(id, sock_conn, &miTabla);
				pthread_mutex_unlock(&mutex);
				printf("%s ha aceptado la partida %d\n", nombre, id);

				if (a == 1) //Empezar partida (6)
				{
					printf("Empezando Partida ID: %d\n", id);

					pthread_mutex_lock(&mutex);
					miTabla[id].sentido = 1; //Horario
					miTabla[id].turno = 0;
					//Mezclar mazo
					shuffle(cartas, NELEMS(cartas));
					memcpy(miTabla[id].mazo, cartas, sizeof(miTabla[id].mazo));
					//Pon una en mesa
					//No puede ser una carta especial
					int cartaEnMesa;
					for (;;)
					{
						cartaEnMesa = RobarCarta(miTabla[id].mazo);
						if (DameNumero(cartaEnMesa) >= 0 && DameNumero(cartaEnMesa) <= 9)
							break;
					}
					miTabla[id].cartaEnMesa = cartaEnMesa;
					pthread_mutex_unlock(&mutex);

					char mensaje[100];

					sprintf(mensaje, "11/%d,1,%d,%d", id, miTabla[id].numJ, miTabla[id].cartaEnMesa);
					for (int i = 0; i < miTabla[id].numJ; i++)
						sprintf(mensaje, "%s,%s", mensaje, miTabla[id].jugadores[i].nombre);
					char tmp[100];
					char repartidas[100];
					//Repartir Carta
					for (int i = 0; i < miTabla[id].numJ; i++)
					{
						repartidas[0] = '\0';
						RepartirCartas(i, id, 7, &miTabla, repartidas);
						sprintf(tmp, "%s,%s", mensaje, repartidas);

						write(miTabla[id].jugadores[i].socket, tmp, strlen(tmp));
						printf("<< (J%d): %s\n", i + 1, tmp);
					}

				}
			}
			else if (confirm == 0) //Rechazado
			{
				char mensaje[100];
				pthread_mutex_lock(&mutex);
				miTabla[id].lleno = 0;
				pthread_mutex_unlock(&mutex);
				printf("%s ha rechazado la partida %d\n", nombre, id);

				sprintf(mensaje, "11/%d,0", id);
				printf("<< %s\n", mensaje);
				for (int i = 0; i < miTabla[id].numJ; i++)
					write(miTabla[id].jugadores[i].socket, mensaje, strlen(mensaje));
			}
		}
		if (codigo == 12) //Mensaje Chat
		{
			int id;
			p = strtok(NULL, "|");
			id = atoi(p);

			char nombre[20];
			DameNombre(sock_conn, &miLista, nombre);

			char mensaje[100];
			p = strtok(NULL, "|");

			strcpy(mensaje, "12/");
			strcat(mensaje, nombre);
			strcat(mensaje, "|");
			strcat(mensaje, p);

			for (int i = 0; i < miTabla[id].numJ; i++)
				write(miTabla[id].jugadores[i].socket, mensaje, strlen(mensaje));
			printf("<< (Partida %d) %s\n", id, mensaje);
		}

		if (codigo == 13) //Salida repentina tablero
		{
			p = strtok (NULL, "/");
			int id = atoi (p);

			pthread_mutex_lock(&mutex);
			miTabla[id].lleno = 0;
			pthread_mutex_unlock(&mutex);

			char cancelar [40];
			strcpy (cancelar, "13/");

			for (int i = 0; i < miTabla[id].numJ; i++)
				write(miTabla[id].jugadores[i].socket, cancelar, strlen(cancelar));
			printf("<< (Partida %d) %s\n", id, cancelar);
		}

		if (codigo == 14) //Juegan carta
		{
			p = strtok (NULL, "/");
			int id = atoi (p);

			p = strtok (NULL, "/");
			int carta = atoi (p);

			//Es tu turno
			if (sock_conn == miTabla[id].jugadores[miTabla[id].turno].socket)
			{
				int idJ = DameNumeroJugador(sock_conn, id, &miTabla);

				char colorcartamesa[20];
				DameColor(miTabla[id].cartaEnMesa, colorcartamesa);
				int numerocartamesa = DameNumero(miTabla[id].cartaEnMesa);

				char colorcartajugada[20];
				DameColor(carta, colorcartajugada);
				int numerocartajugada = DameNumero(carta);

				if (carta == 200) //Quiere robar
				{

					if (miTabla[id].jugadores[idJ].robado == 1)
					{
						char repartidas[100];
						RepartirCartas(idJ, id, 1, &miTabla, repartidas);
						char nueva_mano[200];
						nueva_mano[0] = '\0';
						for (int i = 0; i < miTabla[id].jugadores[idJ].numCartas; i++)
						{
							if (i == 0)
								sprintf(nueva_mano, "15/%d", miTabla[id].jugadores[idJ].mano[i]);
							else
								sprintf(nueva_mano, "%s,%d", nueva_mano, miTabla[id].jugadores[idJ].mano[i]);
						}
						write(sock_conn, nueva_mano, strlen(nueva_mano));
						printf("<< %s\n", nueva_mano);

						pthread_mutex_lock(&mutex);
						miTabla[id].jugadores[idJ].escabullirse = 0;
						miTabla[id].jugadores[idJ].uno = 0;
						if (miTabla[id].jugadores[idJ].porRobar > 0)
							miTabla[id].jugadores[idJ].porRobar--;
						else
							miTabla[id].jugadores[idJ].robado = 0;
						pthread_mutex_unlock(&mutex);

					}
					else //No puedes robar mas
					{
						sprintf(respuesta, "12/Servidor|No puedes robar m¬∑s cartas");
						write(sock_conn, respuesta, strlen(respuesta));
						printf("<< %s\n", respuesta);
					}
				}
				else if (miTabla[id].jugadores[idJ].porRobar == 0 || (miTabla[id].jugadores[idJ].escabullirse == 1 && (numerocartajugada == 12 || numerocartajugada == 14)))//Las cartas normales
				{
					miTabla[id].jugadores[idJ].escabullirse = 0;
					if (strcmp(colorcartamesa, "negro") == 0)
						strcpy(colorcartamesa, miTabla[id].colorEnMesa);
					if(strcmp(colorcartamesa, colorcartajugada) == 0 || numerocartajugada == numerocartamesa || strcmp(colorcartajugada, "negro") == 0)
					{
						pthread_mutex_lock(&mutex);

						//Ponerla en la mesa
						miTabla[id].cartaEnMesa = carta;
						//Quitarla de la mano
						for (int i = 0, encontrado = 0; i < miTabla[id].jugadores[miTabla[id].turno].numCartas; i++)
						{
							if (carta == miTabla[id].jugadores[miTabla[id].turno].mano[i])
								encontrado = 1;
							if (encontrado == 1)
								miTabla[id].jugadores[miTabla[id].turno].mano[i] = miTabla[id].jugadores[miTabla[id].turno].mano[i+1];
						}
						miTabla[id].jugadores[miTabla[id].turno].numCartas--;

						if (numerocartajugada != 13 && numerocartajugada != 14)
						{
							//Siguiente turno
							if (numerocartajugada == 11)
								miTabla[id].sentido = (miTabla[id].sentido + 1) % 2;
							int saltar = 0;
							if (numerocartajugada == 10)
								saltar = 1;
							if (miTabla[id].sentido == 1) //Horario
								miTabla[id].turno = (miTabla[id].turno + miTabla[id].numJ + saltar + 1) % (miTabla[id].numJ);
							else if (miTabla[id].sentido == 0) //Anti Horario
								miTabla[id].turno = (miTabla[id].turno + miTabla[id].numJ - saltar - 1) % (miTabla[id].numJ);
							int idJ = DameNumeroJugador(sock_conn, id, &miTabla);
							miTabla[id].jugadores[idJ].robado = 1;

							if (numerocartajugada == 12)
							{
								miTabla[id].jugadores[miTabla[id].turno].porRobar = miTabla[id].jugadores[idJ].porRobar + 2;
								miTabla[id].jugadores[idJ].porRobar = 0;
								miTabla[id].jugadores[miTabla[id].turno].escabullirse = 1;
							}

							if (miTabla[id].jugadores[idJ].numCartas == 0)
							{
								sprintf(respuesta, "19/%s", miTabla[id].jugadores[idJ].nombre);
								for(int i = 0; i < miTabla[id].numJ; i++)
									write(miTabla[id].jugadores[i].socket, respuesta, strlen(respuesta));
								printf("<< %s\n", respuesta);
							}
							else
							{
								//Enviar jugada
								sprintf(respuesta, "14/%d,%d,%d", miTabla[id].cartaEnMesa, miTabla[id].turno, miTabla[id].sentido);
								for (int i = 0; i < miTabla[id].numJ; i++)
									sprintf(respuesta, "%s,%d", respuesta, miTabla[id].jugadores[i].numCartas);
								for (int i = 0; i < miTabla[id].numJ; i++)
								{
									write(miTabla[id].jugadores[i].socket, respuesta, strlen(respuesta));
									printf("<< %s\n", respuesta);
									if (sock_conn == miTabla[id].jugadores[i].socket)
									{
										char nueva_mano[512];
										nueva_mano[0] = '\0';
										for (int j = 0; j < miTabla[id].jugadores[i].numCartas; j++)
										{
											if (strlen(nueva_mano) == 0)
												sprintf(nueva_mano, "15/%d", miTabla[id].jugadores[i].mano[j]);
											else
												sprintf(nueva_mano, "%s,%d", nueva_mano, miTabla[id].jugadores[i].mano[j]);
										}
										write(sock_conn, nueva_mano, strlen(nueva_mano));
										printf("<< %s\n", nueva_mano);
									}
								}
							}
						}
						else //Preguntar el color
						{
							strcpy(respuesta, "17/");
							write(sock_conn, respuesta, strlen(respuesta));
							printf("<< %s\n", respuesta);
						}
						pthread_mutex_unlock(&mutex);
					}
					else //Jugada ilegal
					{
						sprintf(respuesta, "12/Servidor|No puedes jugar esa carta");
						write(sock_conn, respuesta, strlen(respuesta));
						printf("<< %s\n", respuesta);
					}
				}
				else
				{
					sprintf(respuesta, "12/Servidor|Debes robar %d cartas\n", miTabla[id].jugadores[idJ].porRobar);
					write(sock_conn, respuesta, strlen(respuesta));
					printf("<< %s\n", respuesta);
				}
			}
			else // No es tu turno
			{
				sprintf(respuesta, "12/Servidor|No es tu turno");
				write(sock_conn, respuesta, strlen(respuesta));
				printf("<< %s\n", respuesta);
			}
		}
		if (codigo == 16) //Pasar
		{
			p = strtok (NULL, "/");
			int id = atoi (p);
			int idJ = DameNumeroJugador(sock_conn, id, &miTabla);

			if (miTabla[id].jugadores[idJ].porRobar == 0)
			{
				if (miTabla[id].jugadores[idJ].robado == 0)
				{
					pthread_mutex_lock(&mutex);
					if (miTabla[id].sentido == 1) //Horario
						miTabla[id].turno = (miTabla[id].turno + miTabla[id].numJ + 1) % (miTabla[id].numJ);
					else if (miTabla[id].sentido == 0) //Anti Horario
						miTabla[id].turno = (miTabla[id].turno + miTabla[id].numJ- 1) % (miTabla[id].numJ);
					miTabla[id].jugadores[idJ].robado = 1;
					pthread_mutex_unlock(&mutex);
					sprintf(respuesta, "14/%d,%d,%d", miTabla[id].cartaEnMesa, miTabla[id].turno, miTabla[id].sentido);
					for (int i = 0; i < miTabla[id].numJ; i++)
						sprintf(respuesta, "%s,%d", respuesta, miTabla[id].jugadores[i].numCartas);
					for (int i = 0; i < miTabla[id].numJ; i++)
					{
						write(miTabla[id].jugadores[i].socket, respuesta, strlen(respuesta));
						printf("<< %s\n", respuesta);
					}
				}
				else
				{
					sprintf(respuesta, "12/Servidor|Debes robar una carta primero");
					write(sock_conn, respuesta, strlen(respuesta));
					printf("<< %s\n", respuesta);
				}
			}
			else
			{
				sprintf(respuesta, "12/Servidor|Debes robar %d cartas\n", miTabla[id].jugadores[idJ].porRobar);
				write(sock_conn, respuesta, strlen(respuesta));
				printf("<< %s\n", respuesta);
			}
		}
		if (codigo == 17) //Eleccion color
		{
			p = strtok (NULL, "/");
			int id = atoi (p);
			int idJ = DameNumeroJugador(sock_conn, id, &miTabla);

			pthread_mutex_lock(&mutex);

			p = strtok (NULL, "/");
			strcpy (miTabla[id].colorEnMesa, p);

			if (miTabla[id].sentido == 1) //Horario
				miTabla[id].turno = (miTabla[id].turno + miTabla[id].numJ + 1) % (miTabla[id].numJ);
			else if (miTabla[id].sentido == 0) //Anti Horario
				miTabla[id].turno = (miTabla[id].turno + miTabla[id].numJ- 1) % (miTabla[id].numJ);
			miTabla[id].jugadores[idJ].robado = 1;

			if (DameNumero(miTabla[id].cartaEnMesa) == 14)
			{
				miTabla[id].jugadores[miTabla[id].turno].porRobar = miTabla[id].jugadores[idJ].porRobar + 4;
				miTabla[id].jugadores[idJ].porRobar = 0;
				miTabla[id].jugadores[miTabla[id].turno].escabullirse = 1;
			}

			//Enviar jugada
			sprintf(respuesta, "14/%d,%s,%d,%d", miTabla[id].cartaEnMesa, miTabla[id].colorEnMesa, miTabla[id].turno, miTabla[id].sentido);
			for (int i = 0; i < miTabla[id].numJ; i++)
				sprintf(respuesta, "%s,%d", respuesta, miTabla[id].jugadores[i].numCartas);
			for (int i = 0; i < miTabla[id].numJ; i++)
			{
				write(miTabla[id].jugadores[i].socket, respuesta, strlen(respuesta));
				printf("<< %s\n", respuesta);
				if (sock_conn == miTabla[id].jugadores[i].socket)
				{
					char nueva_mano[512];
					nueva_mano[0] = '\0';
					for (int j = 0; j < miTabla[id].jugadores[i].numCartas; j++)
					{
						if (strlen(nueva_mano) == 0)
							sprintf(nueva_mano, "15/%d", miTabla[id].jugadores[i].mano[j]);
						else
							sprintf(nueva_mano, "%s,%d", nueva_mano, miTabla[id].jugadores[i].mano[j]);
					}
					write(sock_conn, nueva_mano, strlen(nueva_mano));
					printf("<< %s\n", nueva_mano);
				}
			}

			pthread_mutex_unlock(&mutex);
		}
		if (codigo == 18) //Cantan UNO
		{
			p = strtok(NULL, "/");
			int id = atoi(p);
			int jugClic = DameNumeroJugador(sock_conn, id, &miTabla);
			int jugUno;
			int i = 0;
			int encontrado = 0;
			while(i < miTabla[id].numJ && encontrado == 0)
			{
				if (miTabla[id].jugadores[i].numCartas == 1 && miTabla[id].jugadores[i].uno == 0)
					encontrado = 1;
				else
					i++;
			}

			if (encontrado == 1)
			{
				int jugUno = i;
				if (jugUno == jugClic) //Quien tiene una carta ha dicho UNO
				{
					pthread_mutex_lock(&mutex);
					miTabla[id].jugadores[jugUno].uno = 1;
					pthread_mutex_unlock(&mutex);
					sprintf(respuesta, "12/Servidor|%s ha cantado UNO\n", miTabla[id].jugadores[jugUno].nombre);
					for(int s = 0; s < miTabla[id].numJ; s++)
						write(miTabla[id].jugadores[s].socket, respuesta, strlen(respuesta));
					printf("<< %s\n", respuesta);
				}
				else //Quien tiene una carta no ha dicho UNO
				{
					char repartidas[100];
					RepartirCartas(jugUno, id, 2, &miTabla, repartidas);
					char nueva_mano[200];
					nueva_mano[0] = '\0';
					for (int i = 0; i < miTabla[id].jugadores[jugUno].numCartas; i++)
					{
						if (i == 0)
							sprintf(nueva_mano, "15/%d", miTabla[id].jugadores[jugUno].mano[i]);
						else
							sprintf(nueva_mano, "%s,%d", nueva_mano, miTabla[id].jugadores[jugUno].mano[i]);
					}
					write(miTabla[id].jugadores[jugUno].socket, nueva_mano, strlen(nueva_mano));
					printf("<< %s\n", nueva_mano);
					sprintf(respuesta, "12/Servidor|%s se le ha olvidado cantar UNO\n", miTabla[id].jugadores[jugUno].nombre);
					for(int s = 0; s < miTabla[id].numJ; s++)
						write(miTabla[id].jugadores[s].socket, respuesta, strlen(respuesta));
					printf("<< %s\n", respuesta);
				}
			}
			else
			{
				//Nadie tiene UNO
				char repartidas[100];
				RepartirCartas(jugClic, id, 2, &miTabla, repartidas);
				char nueva_mano[200];
				nueva_mano[0] = '\0';
				for (int i = 0; i < miTabla[id].jugadores[jugClic].numCartas; i++)
				{
					if (i == 0)
						sprintf(nueva_mano, "15/%d", miTabla[id].jugadores[jugClic].mano[i]);
					else
						sprintf(nueva_mano, "%s,%d", nueva_mano, miTabla[id].jugadores[jugClic].mano[i]);
				}
				write(sock_conn, nueva_mano, strlen(nueva_mano));
				printf("<< %s\n", nueva_mano);
			}

		}

		if (codigo == 20)
		{
			int id;

			p = strtok(NULL, ",");
			id = atoi(p);
			p = strtok(NULL, ",");
			int duracion = atoi(p);


			ConsultaFinal(&miTabla, id, duracion);
		}
		if (codigo == 0 || codigo == 1) //Notificacion Conectados
		{
			char nombres[512];
			DameConectados(&miLista, nombres);
			printf("<< %s\n", nombres);

			for (int i = 0; i < miLista.num; i++)
				write (miLista.conectados[i].socket, nombres, strlen(nombres));
			if (codigo == 0)
			{
				break;
			}
		}

		if (codigo != 0 && codigo != 10 && codigo != 11  && codigo!= 12 && codigo != 13 && codigo != 14 && codigo != 16 && codigo != 17 && codigo != 18 && codigo != 20)
		{
			printf("<< %s\n", respuesta);
			write (sock_conn, respuesta, strlen(respuesta));
		}
	}
	close(sock_conn);
}

int main(int argc, char *argv[])
{
	//Semilla para la aleatoriedad
	srand(time(NULL));

	//Inicio mazo de cartas
	for (int i = 0, c = 1; i < 108; i++, c++)
	{
		if (c == 56 || c == 70 || c == 84 || c == 98)
			c++;
		cartas[i] = c;
	}

	//Inicio Base de Datos
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	conn = mysql_init(NULL);
	if (conn == NULL)
	{
		printf("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	conn = mysql_real_connect(conn, mysql_dire, mysql_user, mysql_pass, mysql_ddbb, 0, NULL, 0);
	if (conn == NULL)
	{
		printf("Error al crear la conexion: %u %s\n", mysql_errno(conn), mysql_error(conn));
		exit(1);
	}

	//Inicio Server
	int sock_conn, sock_listen;
	struct sockaddr_in serv_adr;
	if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
		printf("Error creando socket\n");

	memset(&serv_adr, 0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(port);

	if (bind(sock_listen, (struct sockaddr *) &serv_adr, sizeof(serv_adr)) < 0)
		printf ("Error Bind\n");
	if (listen(sock_listen, 2) < 0)
		printf("Error Listen\n");

	//Threads
	int sockets[100];
	pthread_t thread;

	//Escucha del Server
	for (int i = 0;;i++) {
		printf ("Escuchando (%d)\n", port);
		sock_conn = accept(sock_listen, NULL, NULL);
		printf ("Conexion entrante\n");

		//Threads
		sockets[i] = sock_conn;
		pthread_create (&thread, NULL, AtenderCliente, &sockets[i]);
	}

	//Desconexion Base de Datos
	mysql_close(conn);
}
