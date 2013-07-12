#include "../inc/PlatformCommunications.h"

//Constructor
MOOGCommunication::MOOGCommunication()
{
	b_portOpen = false;
	strcpy_s(c_lastError,1024,"No Error");
}

//Destructor
MOOGCommunication::~MOOGCommunication()
{
	b_portOpen = false;
}

//Opens udp connection to MOOG motion platform
void MOOGCommunication::InitializeUDP(char *host, char *port)
{
	try 
	{

		h_MoogSocket = new udp::socket(h_Moog_io_service, udp::endpoint(udp::v4(), 0));

		h_MoogResolver = new udp::resolver(h_Moog_io_service);
		h_MoogQuery = new udp::resolver::query(udp::v4(), host, port);
		h_MoogIterator = h_MoogResolver->resolve(*h_MoogQuery);

		b_portOpen = true;
	}
	catch (std::exception& e)
	{
		strcpy_s(c_lastError,1024,e.what());
		b_portOpen = false;
	}
}

/*	Sends current state to Moog motion platform
	Returns if send was successful*/
bool MOOGCommunication::SendState(MDACommand *currentState)
{
	static size_t iSendSize = sizeof(MDACommand);

	try
	{
		h_MoogSocket->send_to(boost::asio::buffer((char *)currentState, iSendSize), *h_MoogIterator);
	}
	catch (std::exception& e)
	{
		strcpy_s(c_lastError,1024,e.what());
		return false;
	}


	return true;
}


void MOOGCommunication::GetUDPError(char *dest, int max_length)
{
	strcpy_s(dest,max_length,c_lastError);
}