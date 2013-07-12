// FlightDataReaderConsole.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <time.h>

void interpolate_through_data()
{
	float t_start, t_last, t_current, t_elapsed;
	float f_freq = 0;
	bool keepReading = true;

	t_start = t_last = 1.0f * clock() / CLOCKS_PER_SEC;
	FlightDataStreamer *h_flightDataStreamer = new FlightDataStreamer();
	FlightData *h_flightData = new FlightData();

	if (h_flightDataStreamer->OpenFile("D:\\flightdata.txt") == false)
	{
		return;
	}

	if (h_flightDataStreamer->LoadData() == false)
	{
		return;
	}

	t_current = 1.0f * clock() / CLOCKS_PER_SEC;
	t_elapsed = t_current - t_start;

	while (h_flightDataStreamer->Interp(t_elapsed,h_flightData))
	{
		f_freq = 1.0f / (float) (t_current - t_last);
		printf("Lat: %0.4f, Long: %0.4f, Alt: %0.4f, Freq: %0.1f Hz  \t\r",h_flightData->Lat,h_flightData->Lon,h_flightData->Alt, f_freq);
		//Sleep(27);
		t_last = t_current;
		t_current = 1.0f * clock() / CLOCKS_PER_SEC;
		t_elapsed = t_current - t_start;
	}
}


void loop_all_data()
{
	time_t	t_last, t_current;
	float f_freq = 0;

	t_last = clock();
	FlightDataStreamer *h_flightDataStreamer = new FlightDataStreamer();
	FlightData *h_flightData = new FlightData();

	if (h_flightDataStreamer->OpenFile("D:\\flightdata.txt") == false)
	{
		return;
	}

	while (h_flightDataStreamer->NextState(h_flightData))
	{
		t_current = clock();
		f_freq = 1.0f / (float) (t_current - t_last) * CLOCKS_PER_SEC;
		printf("Lat: %0.4f, Long: %0.4f, Alt: %0.4f, Freq: %0.1f Hz  \t\r",h_flightData->Lat,h_flightData->Lon,h_flightData->Alt, f_freq);
		Sleep(100);
		t_last = t_current;
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	interpolate_through_data();

	return 0;
}

