/*
Plugin to show how to derive motion platform data from our datarefs
Thanks to Austin for allowing us to use the original X-Plane conversion code.

Based on:			Initial Sandy Barbour - 05/08/2007

Additions:			Jonathan Plumpton - April 24, 2013
*/
#include "../inc/PlatformCommunications.h"
#include "../inc/xmlConfig.h"

#include <stdio.h>
#include <string.h>
#include <ctime>
#include <chrono>

#include "../../external/XplaneSdk/CHeaders/xplm/XPLMDisplay.h"
#include "../../external/XplaneSdk/CHeaders/xplm/XPLMGraphics.h"
#include "../../external/XplaneSdk/CHeaders/xplm/XPLMProcessing.h"
#include "../../external/XplaneSdk/CHeaders/xplm/XPLMDataAccess.h"
#include "../../external//XplaneSdk/CHeaders/xplm/XPLMUtilities.h"

// Disable: unreferenced formal parameter.
#pragma warning( disable : 4100 )
#pragma warning( disable : 4996 )



//---------------------------------------------------------------------------
// Function prototypes

float MotionPlatformDataLoopCB(float elapsedMe, float elapsedSim, int counter, void * refcon);

void MotionPlatformDataDrawWindowCallback(
	XPLMWindowID         inWindowID,    
	void *               inRefcon);    

void MotionPlatformDataHandleKeyCallback(
	XPLMWindowID         inWindowID,    
	char                 inKey,    
	XPLMKeyFlags         inFlags,    
	char                 inVirtualKey,    
	void *               inRefcon,    
	int                  losingFocus);    

int MotionPlatformDataHandleMouseClickCallback(
	XPLMWindowID         inWindowID,    
	int                  x,    
	int                  y,    
	XPLMMouseStatus      inMouse,    
	void *               inRefcon);    

float MPD_fallout(float data, float low, float high);
float MPD_fltlim(float data, float min, float max);
float MPD_fltmax2 (float x1,const float x2);
void MPD_CalculateMotionData(void);
void MPD_GetMotionData(void);

void initiate_MDACommand();

double elapsedTime();