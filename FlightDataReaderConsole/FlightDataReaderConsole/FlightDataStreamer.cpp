#include "stdafx.h"
#include "FlightDataStreamer.h"




//Constructor opens file
FlightDataStreamer::FlightDataStreamer()
{
	
}

//Destructor closes file
FlightDataStreamer::~FlightDataStreamer()
{
	fclose(h_fp);
}

//Open file
bool FlightDataStreamer::OpenFile(const char* filename)
{
	//Open the requested data file
	errno_t err = fopen_s(&h_fp,filename,"r");

	//Check if file opened
	if (err != 0)
	{
		return b_fileOpen = false;
	}

	b_fileOpen = true;

	read_file_headers();

	return b_fileOpen;
}


//Returns if file is open
bool FlightDataStreamer::IsOpen()
{
	return b_fileOpen;
}


//Gets data from next line in file
bool FlightDataStreamer::NextState(FlightData* currentState)
{
	char s_line[1024];

	//Read next line of file and ensure not EOF
	if (fgets(s_line,sizeof(s_line),h_fp) == NULL)
	{
		//Error reading next line, return false
		return false;
	}

	// Parse line, and ensure all were read correctly 
	if (7 == sscanf_s(s_line,"%f,%f,%f,%f,%f,%f,%f",&currentState->Time,&currentState->Lat,&currentState->Lon,&currentState->Alt,&currentState->PitchInput,&currentState->RollInput,&currentState->YawInput))
	{
		return true;
	} //if
	
	// Line failed to parse correctly
	return false;
}

//Load all the data in the file
bool FlightDataStreamer::LoadData()
{
	char s_line[1024];

	//We know that we have n_dataPoints lines of data
	//Initialize our arrays
	n_lastTimeIndex = 0;
	f_time = new float[n_dataPoints];
	f_lat = new float[n_dataPoints];
	f_long = new float[n_dataPoints];
	f_alt = new float[n_dataPoints];
	f_pitchIn = new float[n_dataPoints];
	f_rollIn = new float[n_dataPoints];
	f_yawIn = new float[n_dataPoints];

	//Counter
	int i = 0;	

	while (fgets(s_line,sizeof(s_line),h_fp) != NULL && i < n_dataPoints)
	{
		// Parse line, and ensure all were read correctly 
		if (7 != sscanf_s(s_line,"%f,%f,%f,%f,%f,%f,%f",&f_time[i],&f_lat[i],&f_long[i],&f_alt[i],&f_pitchIn[i],&f_rollIn[i],&f_yawIn[i]))
		{
			return false;
		} //if

		f_maxTime = f_time[i];
		//Increase counter
		i++;
	} //while

	fclose(h_fp);
	return true;
}

//Interpolate to desired elapsedTime and return data
bool FlightDataStreamer::Interp(float elapsedTime, FlightData* currentState)
{
	//First check elapsedTime doesn't exceed max elapsedTime
	if (elapsedTime + f_time[0] >= f_maxTime)
	{
		return false;
	}

	//Need to find index of first interval where f_time[i] > elapsedTime
	int i = 0;

	for (i = n_lastTimeIndex;i<n_dataPoints;i++)
	{
		if (f_time[i] > elapsedTime + f_time[0])
		{
			break;
		}
	}

	n_lastTimeIndex = i;

	currentState->Time = elapsedTime;
	currentState->Lat = interpolate(elapsedTime,i-1,f_lat);
	currentState->Lon = interpolate(elapsedTime,i-1,f_long);
	currentState->Alt = interpolate(elapsedTime,i-1,f_alt);
	currentState->PitchInput = interpolate(elapsedTime,i-1,f_pitchIn);
	currentState->RollInput = interpolate(elapsedTime,i-1,f_rollIn);
	currentState->YawInput = interpolate(elapsedTime,i-1,f_yawIn);

	return true;
}

float FlightDataStreamer::interpolate(float time, int index,float *f_data)
{
	return f_data[index] + (time - f_time[index]) * (f_data[index+1] - f_data[index]) / ( f_time[index+1] - f_time[index] );
}

//Clear memory allocations
void FlightDataStreamer::ClearData()
{
	delete [] f_time;
	delete [] f_lat;
	delete [] f_long;
	delete [] f_alt;
	delete [] f_pitchIn;
	delete [] f_rollIn;
	delete [] f_yawIn;

}

void FlightDataStreamer::read_file_headers()
{
	char s_line[1024];

	//Read number of data points
	if (fgets(s_line,sizeof(s_line),h_fp) == NULL)
	{
		//Error reading next line, return false
		b_fileOpen = false;
		return;
	} //if
	n_dataPoints = atoi(s_line);

	//Read in header lines
	if (fgets(s_line,sizeof(s_line),h_fp) == NULL)
	{
		//Error reading next line, return false
		b_fileOpen = false;
		return;
	} //if

	//May choose to confirm order of column headers at some point.
	//header_order(s_line);
}


//Check order of column headers
void FlightDataStreamer::header_order(char *s_line)
{
	//Tokenize s_line by comma and determine what the header is
	char *s_token, *s_nextToken;
	int n_columnNumber = 0;
	int n_formatIndex = 0;
	char s_format[1024];
	strcpy(s_format,"");

	s_token = strtok_s(s_line,",",&s_nextToken);

	while (s_token != NULL)
	{
		if (0 == strcmp(s_token,"Time"))
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%f");
		} 
		else if (0 == strcmp(s_token,"Lat"))
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%f");
		}
		else if (0 == strcmp(s_token,"Long"))
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%f");
		}
		else if (0 == strcmp(s_token,"Alt"))
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%f");
		}
		else if (0 == strcmp(s_token,"Pitch"))
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%f");
		}
		else if (0 == strcmp(s_token,"Roll"))
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%f");
		}
		else if (0 == strcmp(s_token,"Yaw"))
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%f");
		}
		else //Irrelevant Column
		{
			//Set associated index to n_columnNumber
			strcat_s(s_format,"%*f");
		}

		s_token = strtok_s(NULL,",",&s_nextToken);
		n_columnNumber++;
	} //while

	n_dataColumns = n_columnNumber;
}