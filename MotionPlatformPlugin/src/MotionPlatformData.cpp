#include "../inc/MotionPlatformData.h"

// Globals.
// Use MPD_ as a prefix for the global variables

// Used to store calculated motion data
float MPD_LocalGravity = 0.0f;
bool b_sendControlInputs = false;

MDACommand *MPD_MDACommand;

// control input command
XPLMMenuID   MPD_MenuId;
int          MPD_MenuItem;
int			 MPD_MenuItemID;

// Control Input DataRefs
XPLMDataRef MPD_DR_rollRatio = NULL;
XPLMDataRef MPD_DR_pitchRatio = NULL;
XPLMDataRef MPD_DR_yawRatio = NULL;

// Datarefs
XPLMDataRef MPD_DR_localG = NULL;
XPLMDataRef MPD_DR_gnrml = NULL;
XPLMDataRef MPD_DR_gaxil = NULL;
XPLMDataRef MPD_DR_gside = NULL;

XPLMDataRef	MPD_DR_groundspeed = NULL;
XPLMDataRef	MPD_DR_fnrml_prop = NULL;
XPLMDataRef	MPD_DR_fside_prop = NULL;
XPLMDataRef	MPD_DR_faxil_prop = NULL;
XPLMDataRef	MPD_DR_fnrml_aero = NULL;
XPLMDataRef	MPD_DR_fside_aero = NULL;
XPLMDataRef	MPD_DR_faxil_aero = NULL;
XPLMDataRef	MPD_DR_fnrml_gear = NULL;
XPLMDataRef	MPD_DR_fside_gear = NULL;
XPLMDataRef	MPD_DR_faxil_gear = NULL;
XPLMDataRef	MPD_DR_m_total = NULL;
XPLMDataRef	MPD_DR_theta = NULL;
XPLMDataRef	MPD_DR_psi = NULL;
XPLMDataRef	MPD_DR_phi = NULL;

//More Datarefs
XPLMDataRef MPD_DR_P = NULL;
XPLMDataRef MPD_DR_Q = NULL;
XPLMDataRef MPD_DR_R = NULL;
XPLMDataRef MPD_DR_P_dot = NULL;
XPLMDataRef MPD_DR_R_dot = NULL;
XPLMDataRef MPD_DR_Q_dot = NULL;

XPLMDataRef	MPD_DR_TAS = NULL;

//Communication Calls
MOOGCommunication *MPD_MDAConnection = NULL;

//For determining elapsedTime
time_t midnight = NULL;

//---------------------------------------------------------------------------
// SDK Mandatory Callbacks

PLUGIN_API int XPluginStart(
						char *		outName,
						char *		outSig,
						char *		outDesc)
{
	strcpy(outName, "MotionPlatformData");
	strcpy(outSig, "katanasim.motiondplatformdata");
	strcpy(outDesc, "Plug-in to send motion data from datarefs to motion platform.");

	MPD_MenuItem = -1;

	return 1;
}

void         MPD_MenuHandler(void *mRef, void *iRef)
{
	if( b_sendControlInputs)
	{
		b_sendControlInputs = false;
		XPLMCheckMenuItem(MPD_MenuId,MPD_MenuItemID,xplm_Menu_Unchecked);
	}
	else
	{
		b_sendControlInputs = true;
		XPLMCheckMenuItem(MPD_MenuId,MPD_MenuItemID,xplm_Menu_Checked);
	} //if
}

//---------------------------------------------------------------------------

PLUGIN_API void	XPluginStop(void)
{
    
}

//---------------------------------------------------------------------------

PLUGIN_API int XPluginEnable(void)
{
	XPLMDebugString("Motion Platform Plugin: Enabling Plugin...\n");

	//Add menu
	if (MPD_MenuItem < 0)
		MPD_MenuItem = XPLMAppendMenuItem(XPLMFindPluginsMenu(),"MotionData",NULL,1);

	MPD_MenuId = XPLMCreateMenu("MotionData",XPLMFindPluginsMenu(),MPD_MenuItem,MPD_MenuHandler,NULL);
	MPD_MenuItemID = XPLMAppendMenuItem(MPD_MenuId,"Send Control Inputs",(void *)"Change",1);
	XPLMCheckMenuItem(MPD_MenuId,MPD_MenuItemID,xplm_Menu_Unchecked);


	//Init DataRefs

	MPD_DR_pitchRatio	= XPLMFindDataRef("sim/joystick/yoke_pitch_ratio");
	MPD_DR_rollRatio	= XPLMFindDataRef("sim/joystick/yoke_roll_ratio");
	MPD_DR_yawRatio		= XPLMFindDataRef("sim/joystick/yoke_heading_ratio");
	
	MPD_DR_localG = XPLMFindDataRef("sim/weather/gravity_mss");
	MPD_DR_gnrml = XPLMFindDataRef("sim/flightmodel/forces/g_nrml");
	MPD_DR_gaxil = XPLMFindDataRef("sim/flightmodel/forces/g_axil");
	MPD_DR_gside = XPLMFindDataRef("sim/flightmodel/forces/g_side");

	MPD_DR_groundspeed = XPLMFindDataRef("sim/flightmodel/position/groundspeed");
	MPD_DR_fnrml_prop = XPLMFindDataRef("sim/flightmodel/forces/fnrml_prop");
	MPD_DR_fside_prop = XPLMFindDataRef("sim/flightmodel/forces/fside_prop");
	MPD_DR_faxil_prop = XPLMFindDataRef("sim/flightmodel/forces/faxil_prop");
	MPD_DR_fnrml_aero = XPLMFindDataRef("sim/flightmodel/forces/fnrml_aero");
	MPD_DR_fside_aero = XPLMFindDataRef("sim/flightmodel/forces/fside_aero");
	MPD_DR_faxil_aero = XPLMFindDataRef("sim/flightmodel/forces/faxil_aero");
	MPD_DR_fnrml_gear = XPLMFindDataRef("sim/flightmodel/forces/fnrml_gear");
	MPD_DR_fside_gear = XPLMFindDataRef("sim/flightmodel/forces/fside_gear");
	MPD_DR_faxil_gear = XPLMFindDataRef("sim/flightmodel/forces/faxil_gear");
	MPD_DR_m_total = XPLMFindDataRef("sim/flightmodel/weight/m_total");
	MPD_DR_theta = XPLMFindDataRef("sim/flightmodel/position/theta");
	MPD_DR_psi = XPLMFindDataRef("sim/flightmodel/position/psi");
	MPD_DR_phi = XPLMFindDataRef("sim/flightmodel/position/phi");

	//More datarefs
	MPD_DR_P = XPLMFindDataRef("sim/flightmodel/position/P");
	MPD_DR_Q = XPLMFindDataRef("sim/flightmodel/position/Q");
	MPD_DR_R = XPLMFindDataRef("sim/flightmodel/position/R");
	MPD_DR_P_dot = XPLMFindDataRef("sim/flightmodel/position/P_dot");
	MPD_DR_Q_dot = XPLMFindDataRef("sim/flightmodel/position/Q_dot");
	MPD_DR_R_dot = XPLMFindDataRef("sim/flightmodel/position/R_dot");

	MPD_DR_TAS = XPLMFindDataRef("sim/flightmodel/position/true_airspeed");
	
	initiate_MDACommand();

	XPLMDebugString("Motion Platform Plugin: Registering Flight Loop callback...\n");

	XPLMRegisterFlightLoopCallback(MotionPlatformDataLoopCB, 1.0, NULL);
	
	MPD_LocalGravity = XPLMGetDataf(MPD_DR_localG);

	return 1;
}

void initiate_MDACommand()
{
	MPD_MDACommand = (MDACommand *) malloc(sizeof(MDACommand));

	MPD_MDACommand->a_roll = 0.0f;			// rad/s2
	MPD_MDACommand->a_pitch = 0.0f;		// rad/s2
	MPD_MDACommand->a_z = 0.0f;			// m/s2
	MPD_MDACommand->a_x = 0.0f;			// m/s2
	MPD_MDACommand->a_yaw = 0.0f;			// rad/s2
	MPD_MDACommand->a_y = 0.0f;			// m/s2

	MPD_MDACommand->v_roll = 0.0f;			// rad/h_MoogSocket
	MPD_MDACommand->v_pitch = 0.0f;		// rad/h_MoogSocket
	MPD_MDACommand->v_yaw = 0.0f;			// rad/h_MoogSocket
	MPD_MDACommand->roll = 0.0f;			// rad
	MPD_MDACommand->pitch = 0.0f;			// rad
	MPD_MDACommand->yaw = 0.0f;			// rad

	MPD_MDACommand->buffet_roll = 0.0f;	// rad
	MPD_MDACommand->buffet_pitch = 0.0f;	// rad
	MPD_MDACommand->buffet_z = 0.0f;		// m
	MPD_MDACommand->buffet_x = 0.0f;		// m
	MPD_MDACommand->buffet_yaw = 0.0f;		// rad
	MPD_MDACommand->buffet_y = 0.0f;		// m

	MPD_MDACommand->v_vehicle = 0.0f;		// m/h_MoogSocket


	time_t now = time(0);
	tm *today = localtime(&now);
	today->tm_hour = 0; //Setting to midnight
	today->tm_min = 0;
	today->tm_sec = 0;
	
	midnight = mktime(today);
}

//---------------------------------------------------------------------------

PLUGIN_API void XPluginDisable(void)
{
	MPD_MDAConnection = NULL;
	free(MPD_MDACommand);
	XPLMUnregisterFlightLoopCallback(MotionPlatformDataLoopCB, NULL);

	//Kill Menu
	XPLMDestroyMenu(MPD_MenuId);
	MPD_MenuId = NULL;

	//End Communications?
}

//---------------------------------------------------------------------------

PLUGIN_API void XPluginReceiveMessage(XPLMPluginID inFrom, int inMsg, void * inParam)
{
}


                                 

//---------------------------------------------------------------------------
// FlightLoop callback to calculate motion data and store it in our buffers

float MotionPlatformDataLoopCB(float elapsedMe, float elapsedSim, int counter, void * refcon)
{
	MPD_GetMotionData();

	if (MPD_MDAConnection == NULL)
	{
		char sXPlanePath[512];
		char filename[1024];
		XPLMGetSystemPath(sXPlanePath);
		sprintf(filename,"%sResources\\plugins\\MotionPlatform\\PlatformCommunications.xml",sXPlanePath);

		//Open Config File
		XmlConfig *platformConfig = new XmlConfig();
		platformConfig->Open(filename);

		//Enable Communications
		MPD_MDAConnection = new MOOGCommunication();

		if (platformConfig->IsOpen)
		{
			XPLMDebugString("Initiating UDP from Config File...\n");
			XPLMDebugString("Host: ");
			XPLMDebugString(platformConfig->HostIP);
			XPLMDebugString(":");
			XPLMDebugString(platformConfig->Port);
			XPLMDebugString("\n");
			MPD_MDAConnection->InitializeUDP(platformConfig->HostIP,platformConfig->Port);
		}
		else
		{
			XPLMDebugString("Initiating UDP from Default...\n");
			MPD_MDAConnection->InitializeUDP("192.168.0.81","5345");
		}

		if (!MPD_MDAConnection->IsOpen())
		{
			char MPD_Buffer[80];
			MPD_MDAConnection->GetUDPError(MPD_Buffer,80);
			XPLMDebugString(MPD_Buffer);
			return 0;
		}
	}
	
	if (MPD_MDAConnection->IsOpen())
	{
		try
		{
			if(!MPD_MDAConnection->SendState(MPD_MDACommand))
			{
				char MPD_Buffer[512];
				MPD_MDAConnection->GetUDPError(MPD_Buffer,512);
				XPLMDebugString("Failed to send UDP - ");
				XPLMDebugString(MPD_Buffer);
				XPLMDebugString("\n");
			}
		}
		catch (...)
		{
			XPLMDebugString("Error occurred in sending current state.\n");
		}

		
	} else {
		XPLMDebugString("UDP Connection Failure");
	}

	return (float)-1;
}

//---------------------------------------------------------------------------
// Original function used in the Xplane code.

float MPD_fallout(float data, float low, float high)
{
	if (data < low) return data;
	if (data > high) return data;
	if (data < ((low + high) * 0.5)) return low;
    return high;
}

//---------------------------------------------------------------------------
// Original function used in the Xplane code.

float MPD_fltlim(float data, float min, float max)
{
	if (data < min) return min;
	if (data > max) return max;
	return data;
}

//---------------------------------------------------------------------------
// Original function used in the Xplane code.

float MPD_fltmax2 (float x1,const float x2)
{
	return (x1 > x2) ? x1 : x2;
}

//---------------------------------------------------------------------------
// This is original Xplane code converted to use 
// our datarefs instead of the Xplane variables

void MPD_CalculateMotionData(void)
{
	float groundspeed = XPLMGetDataf(MPD_DR_groundspeed);
	float fnrml_prop = XPLMGetDataf(MPD_DR_fnrml_prop);
	float fside_prop = XPLMGetDataf(MPD_DR_fside_prop);
	float faxil_prop = XPLMGetDataf(MPD_DR_faxil_prop);
	float fnrml_aero = XPLMGetDataf(MPD_DR_fnrml_aero);
	float fside_aero = XPLMGetDataf(MPD_DR_fside_aero);
	float faxil_aero = XPLMGetDataf(MPD_DR_faxil_aero);
	float fnrml_gear = XPLMGetDataf(MPD_DR_fnrml_gear);
	float fside_gear = XPLMGetDataf(MPD_DR_fside_gear);
	float faxil_gear = XPLMGetDataf(MPD_DR_faxil_gear);
	float m_total = XPLMGetDataf(MPD_DR_m_total);
	float theta = XPLMGetDataf(MPD_DR_theta);
	float psi = XPLMGetDataf(MPD_DR_psi);
	float phi = XPLMGetDataf(MPD_DR_phi);

	//More
	float P = XPLMGetDataf(MPD_DR_P);
	float Q = XPLMGetDataf(MPD_DR_Q);
	float R = XPLMGetDataf(MPD_DR_R);
	float P_dot = XPLMGetDataf(MPD_DR_P_dot);
	float Q_dot = XPLMGetDataf(MPD_DR_Q_dot);
	float R_dot = XPLMGetDataf(MPD_DR_R_dot);
	float TAS = XPLMGetDataf(MPD_DR_TAS);

	//Calculations for linear accelerations
	float ratio = MPD_fltlim(groundspeed*0.2f,0.0f,1.0f);
	float a_nrml= MPD_fallout(fnrml_prop+fnrml_aero+fnrml_gear,-0.1f,0.1f)/MPD_fltmax2(m_total,1.0f);
	float a_side= (fside_prop+fside_aero+fside_gear)/MPD_fltmax2(m_total,1.0f)*ratio;
	float a_axil= (faxil_prop+faxil_aero+faxil_gear)/MPD_fltmax2(m_total,1.0f)*ratio;

	// Fill the MDACommand
	MPD_MDACommand->MCW		= MOOG_NEWMDA;
	MPD_MDACommand->a_pitch	= Q_dot;
	MPD_MDACommand->a_roll	= P_dot;
	MPD_MDACommand->a_yaw	= R_dot;

	MPD_MDACommand->v_pitch	= Q;
	MPD_MDACommand->v_roll	= P;
	MPD_MDACommand->v_yaw	= R;

	MPD_MDACommand->roll	= phi;
	MPD_MDACommand->pitch	= theta;
	MPD_MDACommand->yaw		= psi;

	MPD_MDACommand->a_x		= a_axil;
	MPD_MDACommand->a_y		= a_nrml;
	MPD_MDACommand->a_z		= a_side;

	//Buffeting - Assume 0 for now
	MPD_MDACommand->v_vehicle = TAS;

	MPD_MDACommand->elapsedTime = elapsedTime();
}

//---------------------------------------------------------------------------
void MPD_GetMotionData(void)
{
	//float groundspeed = XPLMGetDataf(MPD_DR_groundspeed);
	static float DEG_TO_RADS = 1.0f * (3.1415926536f / 180);
	
	// Control Inputs 
	float pitch = XPLMGetDataf(MPD_DR_pitchRatio);
	float roll = XPLMGetDataf(MPD_DR_rollRatio);
	float yaw = XPLMGetDataf(MPD_DR_yawRatio);
	
	float theta = XPLMGetDataf(MPD_DR_theta);
	float psi = XPLMGetDataf(MPD_DR_psi);
	float phi = XPLMGetDataf(MPD_DR_phi);

	//More
	float P = XPLMGetDataf(MPD_DR_P);
	float Q = XPLMGetDataf(MPD_DR_Q);
	float R = XPLMGetDataf(MPD_DR_R);
	float P_dot = XPLMGetDataf(MPD_DR_P_dot);
	float Q_dot = XPLMGetDataf(MPD_DR_Q_dot);
	float R_dot = XPLMGetDataf(MPD_DR_R_dot);
	float TAS = XPLMGetDataf(MPD_DR_TAS);

	//Calculations for linear accelerations
	float g_nrml= XPLMGetDataf(MPD_DR_gnrml);
	float g_side= XPLMGetDataf(MPD_DR_gside);
	float g_axil= XPLMGetDataf(MPD_DR_gaxil);
	MPD_LocalGravity = XPLMGetDataf(MPD_DR_localG);

	float a_nrml= MPD_LocalGravity * g_nrml;
	float a_side= MPD_LocalGravity * g_side;
	float a_axil= MPD_LocalGravity * g_axil;

	// Fill the MDACommand
	MPD_MDACommand->MCW		= MOOG_NEWMDA;
	MPD_MDACommand->a_pitch	= Q_dot * DEG_TO_RADS;
	MPD_MDACommand->a_roll	= P_dot * DEG_TO_RADS;
	MPD_MDACommand->a_yaw	= R_dot * DEG_TO_RADS;

	MPD_MDACommand->v_pitch	= Q * DEG_TO_RADS;
	MPD_MDACommand->v_roll	= P * DEG_TO_RADS;
	MPD_MDACommand->v_yaw	= R * DEG_TO_RADS;

	MPD_MDACommand->roll	= phi * DEG_TO_RADS;
	MPD_MDACommand->pitch	= theta * DEG_TO_RADS;
	MPD_MDACommand->yaw		= psi * DEG_TO_RADS;

	MPD_MDACommand->a_x		= -1.0 * a_axil;
	MPD_MDACommand->a_y		= a_side;
	MPD_MDACommand->a_z		= a_nrml - MPD_LocalGravity;

	if (b_sendControlInputs == true)
	{
		MPD_MDACommand->MCW = MOOG_ControlCommand;
		MPD_MDACommand->buffet_pitch = pitch;
		MPD_MDACommand->buffet_roll = roll;
		MPD_MDACommand->buffet_yaw = yaw;
	}
	else
	{
		MPD_MDACommand->buffet_pitch = 0.0f;
		MPD_MDACommand->buffet_roll = 0.0f;
		MPD_MDACommand->buffet_yaw = 0.0f;
	}

	//Buffeting - Assume 0 for now
	MPD_MDACommand->v_vehicle = TAS;

	MPD_MDACommand->elapsedTime = elapsedTime();
}


//Return number of seconds since start of day
double elapsedTime()
{
	auto now = std::chrono::system_clock::now();

    time_t tnow = std::chrono::system_clock::to_time_t(now);
    tm *date = std::localtime(&tnow);
    date->tm_hour = 0;
    date->tm_min = 0;
    date->tm_sec = 0;
	auto tmidnight = std::chrono::system_clock::from_time_t(std::mktime(date));
    
	std::chrono::system_clock::duration elapsed = now-tmidnight;
	auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(elapsed);
	return 1.0 * ms.count() / 1000;
}

