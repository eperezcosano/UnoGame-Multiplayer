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

#include <my_global.h>
#define port 50066
#define mysql_dire "shiva2.upc.es"
#define mysql_user "root"
#define mysql_pass "mysql"
#define mysql_ddbb "T3Juego"

#define NELEMS(x) (sizeof(x) / sizeof (x[0]))
#define maxConectados 100
#define maxPartidas 50
#define maxMano 50

//ListaConectados
typedef struct {
	char nombre[20];
	int socket;
	int mano[maxMano];
	int confirm;
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
	int sentido;
	int turno;
	int numJ;
	Tconectado jugador1;
	Tconectado jugador2;
	Tconectado jugador3;
	Tconectado jugador4;
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
void shuffle (int *array, size_t n, size_t size)
{
	char tmp [size];
	int *arr = array;
	size_t stride = size * sizeof(int);
	
	if (n > 1)
	{
		size_t i;
		for (i = 0; i < n - 1; i++)
		{
			size_t rnd = (size_t) rand();
			size_t j = i + rnd / (RAND_MAX / (n - i) + 1);
			
			memcpy (tmp, arr + j * stride, size);
			memcpy (arr + j * stride, arr + i * stride, size);
			memcpy (arr + i * stride, tmp, size);
		}
	}
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

//Consulta para loguearse, recibe nombre y contraseña
//Devuelve 1/0: OK, 1/-1: No encontrado, 1/-2:Error BBDD
void Entrar(char username[20], char password[20], char respuesta[512])
{
	//Consulta
	char consulta[512];
	int err;
	MYSQL_RES *res;
	MYSQL_ROW row;
	
	strcpy(consulta, "SELECT COUNT(Jugador.Username) FROM Jugador WHERE Jugador.Username = '");
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

//Consulta para registrarse, recibe nombre y contraseña
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
			strcpy(tabla[i]->jugador1.nombre, jugador1);
			tabla[i]->jugador1.socket = DameSocket(jugador1, &miLista);
			
			if (cont > 1)
			{
				strcpy(tabla[i]->jugador2.nombre, jugador2);
				tabla[i]->jugador2.socket = DameSocket(jugador2, &miLista);
			}
			if (cont > 2)
			{
				strcpy(tabla[i]->jugador3.nombre, jugador3);
				tabla[i]->jugador3.socket = DameSocket(jugador3, &miLista);
			}
			if (cont > 3)
			{
				strcpy(tabla[i]->jugador4.nombre, jugador4);
				tabla[i]->jugador4.socket = DameSocket(jugador4, &miLista);
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
	if (tabla[id]->jugador2.socket == socket)
		tabla[id]->jugador2.confirm = 1;
	else if (tabla[id]->jugador3.socket == socket)
		tabla[id]->jugador3.confirm = 1;
	else if (tabla[id]->jugador4.socket == socket)
		tabla[id]->jugador4.confirm = 1;
	
	if (tabla[id]->numJ == 2)
		if (tabla[id]->jugador2.confirm == 1)
			return 1;
		else
			return 0;
	else if (tabla[id]->numJ == 3)
		if (tabla[id]->jugador2.confirm == 1 && tabla[id]->jugador3.confirm == 1)
			return 1;
		else
			return 0;
	else if (tabla[id]->numJ == 4)
		if (tabla[id]->jugador2.confirm == 1 && tabla[id]->jugador3.confirm == 1 && tabla[id]->jugador4.confirm == 1)
			return 1;
		else
			return 0;
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
				
		if (codigo == 0) // Desconectar cliente
		{
			pthread_mutex_lock(&mutex);
			int err = QuitarJugador(sock_conn, &miLista);
			pthread_mutex_unlock(&mutex);
			if (err == 0)
				printf("Eliminado de la lista conectados\n");
			else
				printf("ERROR al eliminar de la lista conectados\n");
		}
		if (codigo == 1) // Entrar
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
				if (miTabla[pos].numJ > 1)
					write(miTabla[pos].jugador2.socket, invitacion, strlen(invitacion));
				if (miTabla[pos].numJ > 2)
					write(miTabla[pos].jugador3.socket, invitacion, strlen(invitacion));
				if (miTabla[pos].numJ > 3)
					write(miTabla[pos].jugador4.socket, invitacion, strlen(invitacion));
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
					char mensaje[100];
					sprintf(mensaje, "11/%d,1", id);
					printf("<< %s\n", mensaje);
					write(miTabla[id].jugador1.socket, mensaje, strlen(mensaje));
					if (miTabla[id].numJ > 1)
						write(miTabla[id].jugador2.socket, mensaje, strlen(mensaje));
					if (miTabla[id].numJ > 2)
						write(miTabla[id].jugador3.socket, mensaje, strlen(mensaje));
					if (miTabla[id].numJ > 3)
						write(miTabla[id].jugador4.socket, mensaje, strlen(mensaje));
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
				write(miTabla[id].jugador1.socket, mensaje, strlen(mensaje));
				if (miTabla[id].numJ > 1)
					write(miTabla[id].jugador2.socket, mensaje, strlen(mensaje));
				if (miTabla[id].numJ > 2)
					write(miTabla[id].jugador3.socket, mensaje, strlen(mensaje));
				if (miTabla[id].numJ > 3)
					write(miTabla[id].jugador4.socket, mensaje, strlen(mensaje));
			}
		}
		if (codigo == 12) //Mensaje Chat
		{
			int id;
			p = strtok(NULL, ",");
			id = atoi(p);

			char nombre[20];
			DameNombre(sock_conn, &miLista, nombre);
			
			char mensaje[100];
			p = strtok(NULL, ",");
			
			strcpy(mensaje, "12/");
			strcat(mensaje, nombre);
			strcat(mensaje, ",");
			strcat(mensaje, p);
			
			write(miTabla[id].jugador1.socket, mensaje, strlen(mensaje));
			
			if (miTabla[id].numJ > 1)
				write(miTabla[id].jugador2.socket, mensaje, strlen(mensaje));
			if (miTabla[id].numJ > 2)
				write(miTabla[id].jugador3.socket, mensaje, strlen(mensaje));
			if (miTabla[id].numJ > 3)
				write(miTabla[id].jugador4.socket, mensaje, strlen(mensaje));
			printf("<< (Partida %d) %s\n", id, mensaje);
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
				char nombre[20];
				DameNombre(sock_conn, &miLista, nombre);
				printf("%s se ha desconectado\n", nombre);
				break;
			}
		}
		
		if (codigo != 0 && codigo != 10 && codigo != 11  && codigo!= 12)
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
