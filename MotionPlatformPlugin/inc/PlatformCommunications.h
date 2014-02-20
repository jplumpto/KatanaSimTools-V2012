#include <boost/asio.hpp>
#include <cstdlib>
#include <cstring>
#include <iostream>

using boost::asio::ip::udp;


//Motion Command Words
const unsigned char MOOG_DISABLE = 0x00DC;
const unsigned char MOOG_PARK = 0xD2;
const unsigned char MOOG_ENGAGE = 0xB4;
const unsigned char MOOG_RESET = 0xA0;
const unsigned char MOOG_MDAMODE = 0x8C;
const unsigned char MOOG_NEWMDA = 0x80; //New MDA accelerations

const unsigned char MOOG_ControlCommand = 0x36; //Made up for sending control inputs


//MOOG MDA mode command
struct MDACommand 
{
	unsigned int MCW; //Motion Command Word

	float	a_roll;			// rad/s2
	float	a_pitch;		// rad/s2
	float	a_z;			// m/s2
	float	a_x;			// m/s2
	float	a_yaw;			// rad/s2
	float	a_y;			// m/s2
	
	float	v_roll;			// rad/h_MoogSocket
	float	v_pitch;		// rad/h_MoogSocket
	float	v_yaw;			// rad/h_MoogSocket
	float	roll;			// rad
	float	pitch;			// rad
	float	yaw;			// rad

	float	buffet_roll;	// rad
	float	buffet_pitch;	// rad
	float	buffet_z;		// m
	float	buffet_x;		// m
	float	buffet_yaw;		// rad
	float	buffet_y;		// m

	float	v_vehicle;		// m/h_MoogSocket

	//unsigned int spare1;
	//unsigned int spare2;

	//Get time elapsed since midnight
	double	elapsedTime;
}; //MDACommand structure


class MOOGCommunication {

public:

	MOOGCommunication();
	~MOOGCommunication();

	void InitializeUDP(char *host, char *port);
	bool IsOpen(){return b_portOpen;}
	void GetUDPError(char *dest, int max_length);

	bool SendState(MDACommand *currentState);

private:

	char	c_lastError[1024];
	bool	b_portOpen;
	boost::asio::io_service h_Moog_io_service;

	udp::socket *h_MoogSocket;

	udp::resolver *h_MoogResolver;
	udp::resolver::query *h_MoogQuery;
	udp::resolver::iterator h_MoogIterator;


};
