/******************************************************
*			Flight Data Streamer Header File		  *
*******************************************************/
#ifndef _FLIGHT_DATA_STREAMER_H_
#define _FLIGHT_DATA_STREAMER_H_

#include <stdio.h>
#include <string.h>

struct FlightData 
{
	float	Time;

	float	Lat;
	float	Lon;
	float	Alt;

	float	PitchInput;
	float	RollInput;
	float	YawInput;
}; //FlightData struct


class FlightDataStreamer
{
public:
	FlightDataStreamer(); //Constructor opens file
	~FlightDataStreamer();  //Destructor closes file

	bool IsOpen();			//Returns if file is open

	//For reading the file sequentially
	bool OpenFile(const char* filename);
	bool NextState(FlightData* currentState);	//Gets data from next line in file

	//For interpolating through the data
	bool LoadData(); //Load all the data in the file
	bool Interp(float time, FlightData* currentState); //Interpolate to desired time and return data

	void ClearData(); //Reduce chance of memory leaks by Freeing allocations

private:
	FILE*	h_fp;
	bool	b_fileOpen;
	int		n_readCount;
	int		n_dataColumns;

	int		n_dataPoints;
	int		n_lastTimeIndex;
	float	f_maxTime;

	float*	f_time;
	float*	f_lat;
	float*	f_long;
	float*	f_alt;
	float*	f_pitchIn;
	float*	f_rollIn;
	float*	f_yawIn;

	void read_file_headers();
	float interpolate(float time, int index, float* f_data);
	void header_order(char *s_line);
};


#endif //Header Definition